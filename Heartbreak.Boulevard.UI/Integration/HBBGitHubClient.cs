using Booktex.Domain.Book.Model;
using Booktex.Domain.Parsing;
using Booktex.Domain.Util;
using Heartbreak.Boulevard.UI.Configuration;
using Heartbreak.Boulevard.UI.Integration.Story;
using Heartbreak.Boulevard.UI.Story;
using Microsoft.Extensions.Options;
using Octokit;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace Heartbreak.Boulevard.UI.Integration;

public class HBBGitHubClient : IHBBGitHubClient
{
    private IReadOnlyCollection<HBBChapterEntry> _cache = [];
    private static readonly TimeSpan CheckInterval = TimeSpan.FromMinutes(1);
    private DateTime _lastCacheCheck = DateTime.MinValue;
    private DateTime _lastCommitUpdate = DateTime.MinValue;
    private readonly HBBConfiguration _config;

    public HBBGitHubClient(IOptions<HBBConfiguration> config)
    {
        _config = config.Value;
    }


    private readonly SemaphoreSlim _updateLock = new SemaphoreSlim(1);
    public async Task<IReadOnlyCollection<HBBChapterEntry>> LoadChapterEntries()
    {
        if (DateTime.Now - _lastCacheCheck < CheckInterval) 
        {
            return _cache;
        }
        _lastCacheCheck = DateTime.Now;
        try
        {
            var octoClient = new GitHubClient(new ProductHeaderValue("heartbreak-boulevard"));
            octoClient.Credentials = new Credentials(_config.GitHubToken, AuthenticationType.Bearer);
            var repo = await octoClient.Repository.Get(_config.GitHubRepoOwner, _config.GitHubRepoName);
            var lastUpdate = repo.UpdatedAt.LocalDateTime;
            if (lastUpdate > _lastCommitUpdate)
            {
                await _updateLock.WaitAsync();
                repo = await octoClient.Repository.Get(_config.GitHubRepoOwner, _config.GitHubRepoName);
                lastUpdate = repo.UpdatedAt.LocalDateTime;
                if(lastUpdate <=  _lastCommitUpdate) 
                    return _cache;
                _lastCommitUpdate = lastUpdate;
                try
                {
                    var loaded = await LoadFromGitHub(octoClient, repo);
                    _cache = loaded;
                }
                finally
                {
                    _updateLock.Release();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }

        return _cache;

    }

    private async Task<IReadOnlyCollection<HBBChapterEntry>> LoadFromGitHub(GitHubClient octoClient, Repository repo)
    {
        var files = await LoadFiles(octoClient, repo);
        var specs = ParseIndexFile(files);
        if (specs == null)
            throw new Exception("Found not the darn index file");
        var entries = specs
            .Select(_ => new HBBChapterEntry(
                Specification: _,
                Content: ParseContentFor(_, files)
                )
            ).OrderBy(_ => _.Specification.OrderKey)
            .ToList();
        return entries;
    }





    private IReadOnlyCollection<BookChapterContent>? ParseContentFor(HBBChapterSpecification spec, IReadOnlyCollection<HBBGitHubRepoFile> files)
    {
        if (spec.FileName == null)
            return null;
        var relFile = files
            .FirstOrDefault(_ => _.FileName.ToLower().Trim() == spec.FileName.ToLower().Trim());
        if (relFile == null)
            return null;
        Console.WriteLine($"Parsing: {relFile.FileName}");
        var fixedContent = FixNewLines(relFile.FileContent);
        var contents = WritingParser.ParseFileContents(fixedContent);
        contents = AssembleShrapnel(contents);
        return contents;
    }

    private IReadOnlyCollection<BookChapterContent> AssembleShrapnel(IEnumerable<BookChapterContent> content)
    {
        var returnee = new List<BookChapterContent>();
        var forShrapnel = new List<BookChapterContent>();
        string? shrapnelName = null;
        foreach (var contentItem in content)
        {
            if (contentItem is BookChapterSection sect)
            {
                if (forShrapnel.Any() && shrapnelName != null)
                {
                    returnee.Add(new ShrapnelOfLove(shrapnelName, forShrapnel.ToList()));
                    forShrapnel.Clear();
                    shrapnelName = null;
                }
                if (sect.Title.ToLower().Pipe(tit => tit.Contains("shrapnel") && tit.Contains('-')))
                {
                    var splitted = sect.Title.Split('-');
                    shrapnelName = splitted[1].Trim();
                }
            }
            else if (shrapnelName != null)
                forShrapnel.Add(contentItem);
            else 
                returnee.Add(contentItem); 
        }
        if (forShrapnel.Any() && shrapnelName != null)
            returnee.Add(new ShrapnelOfLove(shrapnelName, forShrapnel.ToList()));

        return returnee;
    }


    private IReadOnlyCollection<HBBChapterSpecification>? ParseIndexFile(IReadOnlyCollection<HBBGitHubRepoFile> files)
    {
        var relFile = files
            .FirstOrDefault(_ => _.FileName.ToLower().Trim() == _config.IndexFile.ToLower().Trim());
        if (relFile == null)
            return null;
        var parsed = JsonSerializer.Deserialize<IReadOnlyCollection<HBBChapterSpecification>>(relFile.FileContent);
        return parsed;

    }


    private async Task<IReadOnlyCollection<HBBGitHubRepoFile>> LoadFiles(GitHubClient client, Repository repo)
    {
        var archive = await client.Repository.Content.GetArchive(repo.Id, ArchiveFormat.Zipball);
        using var byteStream = new MemoryStream(archive);
        using var zipArchive = new ZipArchive(byteStream);
        var returnee = zipArchive.Entries
            .Select(_ => (_.Name, _.FullName, Content: StreamToString(_)))
            .Select(_ => new HBBGitHubRepoFile(
                RepoName: repo.Name,
                RepoId: repo.Id.ToString(),
                FullFileName: _.FullName,
                FileName: _.Name,
                FileContent: _.Content
                ))
            .ToList();
        return returnee;

    }


    private static string StreamToString(ZipArchiveEntry ent)
    {
        var byts = ReadStream(ent);
        var returnee = UTF8Encoding.UTF8.GetString(byts);
        return returnee;
    }

    private static byte[] ReadStream(ZipArchiveEntry ent)
    {
        using var readStream = ent.Open();
        using var buffer = new MemoryStream();
        readStream.CopyTo(buffer);
        var returnee = buffer.ToArray();
        return returnee;

    }


    private static string FixNewLines(string str)
    {
        var returnee = new StringBuilder();
        var split = str.Split("\n");
        foreach (var part in split)
        {
            if (returnee.Length > 0)
            {
                if (returnee[returnee.Length - 1] != '\r')
                    returnee.Append('\r');
                returnee.Append('\n');
            }
            foreach (var ch in part)
            {
                returnee.Append(ch);
            }
        }
        return returnee.ToString();
    }
}
