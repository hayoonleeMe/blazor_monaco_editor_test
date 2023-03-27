using Microsoft.AspNetCore.Mvc;
using MonacoTest.Shared;
using static System.Net.WebRequestMethods;
using System.Text.Json;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MonacoTest.Server.Controllers {
    [Route("[controller]")]
    [ApiController]
    public class RunCodeController : ControllerBase {

        // POST api/<RunCode>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] JudgeRequest request) {
            Console.WriteLine("#문제 번호 : " + request.TaskId);
            Console.WriteLine("#언어 : " + request.Language);
            Console.WriteLine("#코드 : " + request.Code);

            // to Judge Server
            HttpClient Http = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Http.PostAsync("https://localhost:7109/Judge", content);

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("Data posted successfully");

                JudgeResult? judgeResult = await response.Content.ReadFromJsonAsync<JudgeResult>();
                if (judgeResult != null) {
                    Console.WriteLine("IsCorrect : " + (judgeResult.IsCorrect ? "true" : "false"));
                    Console.WriteLine("ExecutionTime (ms) : " + judgeResult.ExecutionTime);
                    Console.WriteLine("MemoryUsage (KB) : " + judgeResult.MemoryUsage);
                } else {
                    Console.WriteLine("Result is null");
                }

                return Ok(judgeResult);
            } else {
                Console.WriteLine("Failed to post data " + response.StatusCode);

                return Ok();
            }
        }
    }
}
