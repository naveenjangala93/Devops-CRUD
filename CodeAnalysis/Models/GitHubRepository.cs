using Newtonsoft.Json;
using Octokit;
namespace CodeAnalysis.Models
{
    public class GitHubRepository
    {
        private readonly GitHubClient _client;
        private readonly List<string> _paths_to_be_ignored = new List<string>([".github", ".config", "Properties", "bin","obj"]);

        //CONSTRUCTOR
        public GitHubRepository(string token)
        {
            _client = new GitHubClient(new ProductHeaderValue("CodeDownloadClient"));
            var tokenAuth = new Credentials(token);
            _client.Credentials = tokenAuth;
        }

        public async Task<List<RepositoryContent>> DownloadCodeAsync(string owner, string repo)
        {
            List<RepositoryContent> contents = await GetRepositoryContentsAsync(owner, repo);
            //var jsonContents = JsonConvert.SerializeObject(contents);
            return contents;
        }

        //RECCURSIVELY FETCHING THE CONTENTS FROM FILES
        private async Task<List<RepositoryContent>> GetRepositoryContentsAsync(string owner, string repo, string path = "/")
        {
            Console.WriteLine($"Entered into the path : {path}\n");
            List<RepositoryContent> contents = new List<RepositoryContent>();

            if (ShouldIgnorePath(path))
            {
                Console.WriteLine($"Ignoring the Path : {path}\n");
                return contents;
            }
            try
            {
                var items = await _client.Repository.Content.GetAllContents(owner, repo, path);
                foreach (var item in items)
                {
                  
                    if (item.Type == ContentType.Dir)
                    {
                        var subContents = await GetRepositoryContentsAsync(owner, repo, item.Path);
                        contents.AddRange(subContents);
                    }
                    else
                    {
                        Console.WriteLine($"Downloaded the content fron the file : {item.Path}\n");
                        var fileContent = await _client.Repository.Content.GetAllContents(owner, repo, item.Path);
                        contents.AddRange(fileContent);
                    }                    
                }
            }
            catch (NotFoundException)
            {
                Console.WriteLine("Repository or path not found.\n");
            }
            return contents;
        }


        //RULES FOR IGNORING THE PATHS
        private bool ShouldIgnorePath(string path)
        {
            foreach (var ignoredPath in _paths_to_be_ignored)
            {
                if (path.Contains(ignoredPath))
                {
                    return true;
                }
            }
            return false;
        }
    }
}