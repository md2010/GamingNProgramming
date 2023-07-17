using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/game/")]
    [ApiController]
    public class GameController : ControllerBase
    {
        [Authorize]
        [HttpPost]
        [Route("submit-code")]
        public async Task<IActionResult> SubmitCode([FromBody] RunCodeModel model)
        {
            var compileResult = await CompileCode(model.Code);

            if (!String.IsNullOrEmpty(compileResult.Error))
            {
                return Ok(compileResult);
            }
            else
            {
                var inputs = new[] { "2 5", "1 2", "0 3" };
                var results = new List<RunTestCasesResultModel>();

                foreach (var input in inputs)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.FileName = @"C:\Windows\System32\cmd.exe";
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    startInfo.WorkingDirectory = @"C:\Users\Martina\Documents\GamingNProgramming\GamingNProgramming.API\codeFiles";
                    startInfo.Arguments = "/c a.exe " + (string.IsNullOrEmpty(model.Inputs) ? "" : model.Inputs);

                    var result = "";
                    var error = "";

                    try
                    {
                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();
                            using (StreamReader reader = exeProcess.StandardOutput)
                            {
                                result = reader.ReadToEnd();
                            }
                            using (StreamReader reader = exeProcess.StandardError)
                            {
                                error = reader.ReadToEnd();
                            }
                            results.Add(new RunTestCasesResultModel { Inputs = input, Result = result, Error = error });
                        }
                    }
                    catch { }
                }
                return Ok(results);
            }

        }

        [Authorize]
        [HttpPost]
        [Route("run-code")]
        public async Task<IActionResult> RunCode([FromBody] RunCodeModel model)
        {
            var compileResult = await CompileCode(model.Code);

            if (!String.IsNullOrEmpty(compileResult.Error))
            {
                return Ok(compileResult);
            }
            else
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = false;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.FileName = @"C:\Windows\System32\cmd.exe";
                startInfo.WindowStyle = ProcessWindowStyle.Normal;
                startInfo.WorkingDirectory = @"C:\Users\Martina\Documents\GamingNProgramming\GamingNProgramming.API\codeFiles";
                startInfo.Arguments = "/c a.exe " + (string.IsNullOrEmpty(model.Inputs) ? "" : model.Inputs);

                var result = "";
                var error = "";

                try
                {
                    using (Process exeProcess = Process.Start(startInfo))
                    {
                        exeProcess.WaitForExit();
                        using (StreamReader reader = exeProcess.StandardOutput)
                        {
                            result = reader.ReadToEnd();
                        }
                        using (StreamReader reader = exeProcess.StandardError)
                        {
                            error = reader.ReadToEnd();
                        }
                    }
                }
                catch { }

                var data = new RunCodeResultModel { Result = result, Error = error };
                return Ok(data);
            }
        }

        private async Task<RunCodeResultModel> CompileCode(string code)
        {
            string folder = @"C:\Users\Martina\Documents\GamingNProgramming\GamingNProgramming.API\codeFiles\";
            string fileName = "file.c";
            string fullPath = folder + fileName;
            System.IO.File.WriteAllLines(fullPath, new[] { code });

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = @"C:\Windows\System32\cmd.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.WorkingDirectory = @"C:\Users\Martina\Documents\GamingNProgramming\GamingNProgramming.API\codeFiles";
            startInfo.Arguments = "/c gcc file.c";

            var result = "";
            var error = "";

            try
            {
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    using (StreamReader reader = exeProcess.StandardOutput)
                    {
                        result = reader.ReadToEnd();
                    }
                    using (StreamReader reader = exeProcess.StandardError)
                    {
                        error = reader.ReadToEnd();
                    }
                }
            }
            catch { }

            var data = new RunCodeResultModel { Result = result, Error = error };
            return data;
        }

        #region Classes

        public class RunCodeModel
        {
            public string Code { get; set; }

            public string Inputs { get; set; } = "";
        }

        public class RunCodeResultModel
        {
            public string Result { get; set; }

            public string Error { get; set; }
        }

        public class RunTestCasesResultModel
        {
            public string Inputs { get; set; }
            public string Result { get; set; }

            public string Error { get; set; }
        }
        #endregion
    }
}
