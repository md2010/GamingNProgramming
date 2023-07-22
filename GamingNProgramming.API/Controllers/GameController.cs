using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Service;
using GamingNProgramming.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/game/")]
    [ApiController]
    public class GameController : ControllerBase
    {
        protected IGameService GameService { get; set; }

        public GameController(IGameService service)
        {
            this.GameService = service;
        }

        [Authorize]
        [HttpPost]
        [Route("save-map")]
        public async Task<IActionResult> SaveMap([FromBody] MapModel model)
        { 
            if(model == null)
            {
                return BadRequest();
            }
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var map = MapMap(model);
            var result = await this.GameService.AddMapAsync(Helper.TransformGuid(id), map);

            return Ok();

        }

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

        public class MapModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Path { get; set; }
            public bool IsVisible { get; set; }
            public List<LevelModel> Levels { get; set; }
        }

        public class LevelModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public List<AssignmentModel> Assignments { get; set; }
        }

        public class AssignmentModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public bool IsCoding { get; set; }
            public bool IsMultiSelect { get; set; } = false;
            public bool IsTimeMeasured { get; set; } = false;
            public bool HasBadge { get; set; } = false; 
            public bool HasArgs { get; set; } = false;
            public int Points { get; set; }
            public string InitialCode { get; set; } = "";
            public int Seconds { get; set; } = 0;
            public string BadgeId { get; set; } = "";
            public List<TestCaseModel> TestCases { get; set; } = null;
            public List<AnswerModel> Answers { get; set; } = null;
        }

        public class TestCaseModel
        {
            public string Input { get; set; }
            public string Output { get; set; }
        }

        public class AnswerModel
        {
            public string OfferedAnswer { get; set; }
            public bool IsCorrect { get; set; }
        }

        #endregion

        #region Mapping
        public Map MapMap(MapModel model)
        {
            var map = new Map();
            map.Title = model.Title;
            map.Description = model.Description;

            List<Level> levels = new List<Level>();
            foreach(var level in model.Levels)
            {
                var l = new Level();

                List<Assignment> assignments = new List<Assignment>();
                foreach (var task in level.Assignments)
                {
                    var assignment = new Assignment();
                    assignment.Points = task.Points;
                    assignment.Title = task.Title;
                    assignment.Description = task.Description;
                    assignment.IsMultiSelect = task.IsMultiSelect;
                    assignment.HasArgs = task.HasArgs;
                    assignment.HasBadge = task.HasBadge;
                    assignment.BadgeId = string.IsNullOrEmpty(task.BadgeId) ? null : Helper.TransformGuid(task.BadgeId);
                    assignment.IsCoding = task.IsCoding;
                    assignment.InitialCode = task.InitialCode;
                    assignment.Seconds = task.Seconds;

                    List<TestCase> testCases = new List<TestCase>();
                    foreach (var testCase in task.TestCases)
                    {
                        var tCase = new TestCase();
                        tCase.Input = testCase.Input;
                        tCase.Output = testCase.Output;
                        testCases.Add(tCase);
                    }

                    List<Answer> answers = new List<Answer>();
                    foreach (var answer in task.Answers)
                    {
                        var a = new Answer();
                        a.OfferedAnswer = answer.OfferedAnswer;
                        a.IsCorrect = answer.IsCorrect;
                        answers.Add(a);
                    }

                    assignment.TestCases = testCases;
                    assignment.Answers = answers;
                    assignments.Add(assignment);
                }

                l.Assignments = assignments;
                levels.Add(l);
            }

            map.Levels = levels;
            return map;
        }

        #endregion
    }
}
