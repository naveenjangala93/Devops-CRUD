using CodeAnalysis.Models;
using Microsoft.AspNetCore.Mvc;
using Octokit;
using System;
using System.Threading.Tasks;

namespace CodeAnalysis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeAnalysisController : ControllerBase
    {
        [HttpGet]
        [Route("GetName")]
        public string GetSample()
        {
            return "AzureAmigos";
        }

        [HttpPost]
        [Route("Post")]
        public string ValidateData(string age, string name)
        {
            // Corrected Console.WriteLine statement
            Console.WriteLine($"Age: {age}, Name: {name}");
            return "success";
        }

        [HttpPost]
        [Route("DownloadCode")]
        public async Task<List<GitHubContent>> DownloadCode(string owner, string repo, string token)
        {
            var gitHubRepository = new GitHubRepository(token);
            var jsonContents = await gitHubRepository.DownloadCodeAsync(owner, repo);
            List<GitHubContent> contents = new List<GitHubContent>(); 
            //Console.WriteLine(jsonContents);
            foreach(var content in jsonContents)
            {

                Console.WriteLine($"\n\nfile : {content.Path}\nContent: \n{content.Content}");
                contents.Add(new GitHubContent {name = content.Name, path = content.Path, content = content.Content });
            }
            return contents;
        }
    }
}
