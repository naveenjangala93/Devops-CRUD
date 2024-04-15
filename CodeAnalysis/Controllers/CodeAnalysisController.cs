using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace CodeAnalysis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CodeAnalysisController : ControllerBase
    {   
        [HttpGet]
        [Route("GetName")]
        public string getSample()
        {
            return "AzureAmigos"; 
        }

        [HttpPost]
        [Route("Post")]
        public string ValidateData(string age, string name)
        {
            Console.WriteLine(age, name);
            return "success";
        }
    }
}
