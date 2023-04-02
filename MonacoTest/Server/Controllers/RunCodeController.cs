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
            // to Judge Server
            HttpClient Http = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await Http.PostAsync("https://localhost:7135/Judge", content);

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("Data posted successfully");

                JudgeResult? judgeResult = await response.Content.ReadFromJsonAsync<JudgeResult>();
                if (judgeResult != null) {
                    switch (judgeResult.Result) {
                        case JudgeResult.JResult.Accepted:
                            Console.WriteLine("맞았습니다.");
                            Console.WriteLine("실행 시간(ms) : " + judgeResult.ExecutionTime);
                            Console.WriteLine("메모리 사용량(KB) : " + judgeResult.MemoryUsage);
                            break;
                        case JudgeResult.JResult.WrongAnswer:
                            Console.WriteLine("틀렸습니다.");
                            break;
                        case JudgeResult.JResult.CompileError:
                            Console.WriteLine("컴파일 에러");
                            break;
                        case JudgeResult.JResult.RuntimeError:
                            Console.WriteLine("런타임 에러");
                            break;
                        case JudgeResult.JResult.TimeLimitExceeded:
                            Console.WriteLine("시간 초과");
                            break;
                        case JudgeResult.JResult.MemoryLimitExceeded:
                            Console.WriteLine("메모리 초과");
                            break;
                        case JudgeResult.JResult.PresentationError:
                            Console.WriteLine("출력 형식 에러");
                            break;
                        case JudgeResult.JResult.OutputLimitExceeded:
                            Console.WriteLine("출력 한도 초과");
                            break;
                        case JudgeResult.JResult.JudgementFailed:
                            Console.WriteLine("채점 실패");
                            break;
                        case JudgeResult.JResult.Pending:
                            Console.WriteLine("채점 미완료");
                            break;
                    }
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
