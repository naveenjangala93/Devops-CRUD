using CodeAnalysis.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        [Route("PerfromStaticCodeAnalysis")]
        public async Task<bool> PerfromStaticCodeAnalysis(string owner, string repo, string token)
        {
            var gitHubRepository = new GitHubRepository(token);
            var rawContents = await gitHubRepository.DownloadCodeAsync(owner, repo);

            List<SchemaGitHubContent> contents = new List<SchemaGitHubContent>(); 

            StaticAnalysisRules staticAnalyser = new StaticAnalysisRules();
            List<SchemaStaticAnalysisResult> staticAnalysisResult = new List<SchemaStaticAnalysisResult>();


            foreach(var content in rawContents)
            {

                //Console.WriteLine($"\nFILE : {content.Name}\nFILE PATH : {content.Path}\nCONTENT : \n{content.Content}");
                //Console.WriteLine("------------------------------------------------------------------------------");
                //Console.WriteLine($"{JsonConvert.SerializeObject(content,Formatting.Indented)}");

                contents.Add(new SchemaGitHubContent {
                    name = content.Name,
                    path = content.Path, 
                    content = content.Content 
                });
            }

            //ANALYSE THE CODE
            foreach(SchemaGitHubContent content in contents)
            {
                SchemaStaticAnalysisResult result = staticAnalyser.analyseCode(content);
                //PRINT REPORT
                Console.WriteLine("SchemaStaticAnalysisResult:");
                Console.WriteLine($"Name: {result.Name}");
                Console.WriteLine($"Path: {result.Path}");
                Console.WriteLine($"Message: {result.Message}");
                Console.WriteLine($"Length: {result.Length}");
                Console.WriteLine($"NumberOfMethods: {result.NumberOfMethods}");
                Console.WriteLine($"NumberOfComments: {result.NumberOfComments}");
                Console.WriteLine($"Complexity: {result.Complexity}");
                Console.WriteLine($"FollowsNamingConventions: {result.FollowsNamingConventions}");
                Console.WriteLine($"HasErrorHandling: {result.HasErrorHandling}");
                Console.WriteLine($"HasCodeDuplication: {result.HasCodeDuplication}");
                Console.WriteLine($"HasGoodComments: {result.HasGoodComments}");
                Console.WriteLine($"HasConsistentFormatting: {result.HasConsistentFormatting}");
                Console.WriteLine($"HasPrivateKeys: {result.HasPrivateKeys}");
                Console.WriteLine($"FollowsFileNameConventions: {result.FollowsFileNameConventions}");
                Console.WriteLine($"Score: {result.Score}\n\n\n");
                staticAnalysisResult.Add(result);
            }

            int avgScore = 0, noOfResults=0;
            bool criticalError = false;
            foreach (var result in staticAnalysisResult)
            {
                avgScore += result.Score;
                noOfResults++;
                if (result.HasPrivateKeys) criticalError = true;
            }

            avgScore = avgScore / noOfResults;

            return !(criticalError || avgScore < 60);

            

        }
    }
}
