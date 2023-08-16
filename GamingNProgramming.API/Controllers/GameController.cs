using GamingNProgramming.Common;
using GamingNProgramming.Model;
using GamingNProgramming.Repository;
using GamingNProgramming.Service;
using GamingNProgramming.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GamingNProgramming.WebAPI.Controllers
{
    [Route("api/game/")]
    [ApiController]
    public class GameController : ControllerBase
    {
        protected IGameService GameService { get; set; }
        protected IPlayerService PlayerService { get; set; }

        public GameController(IGameService service, IPlayerService playerService)
        {
            this.GameService = service;
            PlayerService = playerService;
        }

        [Authorize]
        [HttpGet]
        [Route("map/{id}")]
        public async Task<IActionResult> GetMap(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await this.GameService.GetAsync(Helper.TransformGuid(id));

            return Ok(result);

        }

        [Authorize]
        [HttpGet]
        [Route("task/{id}")]
        public async Task<IActionResult> GetTask(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await this.GameService.GetTaskAsync(Helper.TransformGuid(id));

            return Ok(result);

        }

        [Authorize]
        [HttpGet]
        [Route("get-map-edit/{professorId}")]
        public async Task<IActionResult> GetMapForEditing(string professorId)
        {
            if (professorId == null)
            {
                return BadRequest();
            }

            var result = await this.GameService.GetMapByProfessorIdForEditingAsync(Helper.TransformGuid(professorId));

            return Ok(result);

        }

        [Authorize]
        [HttpGet]
        [Route("get-map/{professorId}")]
        public async Task<IActionResult> GetMaps(string professorId)
        {
            if (professorId == null)
            {
                return BadRequest();
            }

            var result = await this.GameService.GetMapByProfessorIdAsync(Helper.TransformGuid(professorId));

            return Ok(result);

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
        [Route("update-map")]
        public async Task<IActionResult> UpdateMap([FromBody] MapModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var map = MapMap(model);
            var result = await this.GameService.UpdateMapAsync(Helper.TransformGuid(id), map);

            return Ok();

        }


        [Authorize]
        [HttpPost]
        [Route("insert-player-task")]
        public async Task<IActionResult> InsertPlayerTask([FromBody] InsertPlayerTaskModel model)
        {
            var playerTask = MapPlayerTask(model);
            var result = await GameService.InsertPlayerTask(playerTask, model.IsDefaultMap, Helper.TransformGuid(model.LevelId));
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpPost]
        [Route("update-scored-points")]
        public async Task<IActionResult> UpdateScoredPoints([FromBody] UpdateScoredPointsModel model)
        {
            if(model.PlayerTaskId == null)
            {
                return BadRequest();
            }
            var result = await GameService.UpdateScoredPoints(Helper.TransformGuid(model.PlayerTaskId), model.NewPoints);
            
            if (result)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet]
        [Route("player-task/{playerId}/{mapId}")]
        public async Task<IActionResult> GetPlayerTask(string playerId, string mapId, String? taskId = "")
        {
            if (playerId == null || mapId == null)
            {
                return BadRequest();
            }

            var result = await this.PlayerService.GetPlayerTask(Helper.TransformGuid(playerId), Helper.TransformGuid(mapId), String.IsNullOrEmpty(taskId) ? null : Helper.TransformGuid(taskId));

            return Ok(result);

        }
       
        [Authorize]
        [HttpPost]
        [Route("insert-battle/{player2Id}")]
        public async Task<IActionResult> InsertBattle(string player2Id)
        {
            if (player2Id == null)
            {
                return BadRequest();
            }

            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await this.GameService.InsertBattle(Helper.TransformGuid(id), Helper.TransformGuid(player2Id));

            return Ok(result);

        }

        [Authorize]
        [HttpGet]
        [Route("battle/{id}")]
        public async Task<IActionResult> GetBattle(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await this.GameService.GetBattleAsync(Helper.TransformGuid(id));

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("battles/{id}")]
        public async Task<IActionResult> FindBattles(string id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var result = await this.GameService.FindBattlesAsync(Helper.TransformGuid(id));

            return Ok(result);

        }

        [Authorize]
        [HttpPut]
        [Route("battle/{id}")]
        public async Task<IActionResult> UpdateBattle(string id, UpdateBattleModel model)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var entity = await GameService.GetBattleAsync(Helper.TransformGuid(id));
            MapBattle(model, entity);

            await GameService.UpdateBattleAsync(entity);

            return Ok();

        }

        [Authorize]
        [HttpPost]
        [Route("submit-code")]
        public async Task<IActionResult> SubmitCode([FromBody] SubmitCodeModel model)
        {
            var compileResult = await CompileCode(model.Code);

            if (!String.IsNullOrEmpty(compileResult.Error))
            {
                return Ok(compileResult);
            }
            else
            {
                var results = new List<RunTestCasesResultModel>();
                var scoredPoints = 0;
                var passedTestCases = 0;
                List<long> times = new List<long>();

                foreach (var testCase in model.TestCases)
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = false;
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.FileName = @"C:\Windows\System32\cmd.exe";
                    startInfo.WindowStyle = ProcessWindowStyle.Normal;
                    startInfo.WorkingDirectory = @"C:\Users\Martina\Documents\GamingNProgramming\GamingNProgramming.API\codeFiles";
                    startInfo.Arguments = "/c a.exe " + testCase.Input;

                    var result = "";
                    var error = "";

                    try
                    {
                        Stopwatch stopwatch = new Stopwatch();
                        stopwatch.Start();

                        using (Process exeProcess = Process.Start(startInfo))
                        {
                            exeProcess.WaitForExit();

                            stopwatch.Stop();
                            long ts = stopwatch.ElapsedMilliseconds;
                            times.Add(ts);

                            using (StreamReader reader = exeProcess.StandardOutput)
                            {
                                result = reader.ReadToEnd();
                            }
                            using (StreamReader reader = exeProcess.StandardError)
                            {
                                error = reader.ReadToEnd();
                            }

                            if(result == testCase.Output)
                            {
                                passedTestCases++;
                            }

                            results.Add(new RunTestCasesResultModel { Inputs = testCase.Input, Result = result, ExpectedOutput = testCase.Output, Error = error });
                        }
                    }
                    catch { }
                }
                var bestTime = times.Min();
                scoredPoints = Helper.GetPoints(passedTestCases, model.TestCases.Count, model.Points);
                return Ok(new SubmitCodeResultModel { Points = scoredPoints, ExecutionTime = bestTime, Results = results});
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

        public class SubmitCodeModel
        {
            public string Code { get; set; }

            public int Points { get; set; }

            public List<TestCase> TestCases { get; set; } = null;
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
            public string ExpectedOutput { get; set; }
            public string Error { get; set; }
        }

        public class SubmitCodeResultModel
        {
           public int Points { get; set; }
            public long ExecutionTime { get; set; }
           public List<RunTestCasesResultModel> Results { get; set; }
        }

        public class UpdateScoredPointsModel
        {
            public string PlayerTaskId { get; set; }
            public int NewPoints { get; set; }
        }

        public class InsertPlayerTaskModel
        {
            public bool IsDefaultMap { get; set; }
            public string PlayerId { get; set; }
            public string MapId { get; set; }
            public string AssignmentId { get; set; }
            public string LevelId { get; set; }
            public int ScoredPoints { get; set; }
            public double Percentage { get; set; }
            public string PlayersCode { get; set; } = "";
            public string Answers { get; set; } = "";
            public long ExecutionTime { get; set; } = 0;

            public string BadgeId { get; set; } = "";
        }

        public class MapModel
        {
            public string Id { get; set; } = "";
            public string Title { get; set; }
            public string Description { get; set; }
            public string Path { get; set; }
            public bool IsVisible { get; set; }
            public List<LevelModel> Levels { get; set; }
        }

        public class LevelModel
        {
            public string Id { get; set; } = "";
            public string MapId { get; set; } = "";
            public string Title { get; set; }
            public string Description { get; set; }
            public List<AssignmentModel> Assignments { get; set; }
        }

        public class AssignmentModel
        {
            public string Id { get; set; } = "";
            public string LevelId { get; set; } = "";
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
            public String? BadgeId { get; set; } = null;
            public List<TestCaseModel> TestCases { get; set; } = null;
            public List<AnswerModel> Answers { get; set; } = null;
        }

        public class TestCaseModel
        {
            public string AssignmentId { get; set; } = "";
            public string Id { get; set; } = "";
            public string Input { get; set; }
            public string Output { get; set; }
        }

        public class AnswerModel
        {
            public string AssignmentId { get; set; } = "";
            public string Id { get; set; } = "";
            public string OfferedAnswer { get; set; }
            public bool IsCorrect { get; set; }
        }

        public class UpdateBattleModel
        {
            public int Player1Points { get; set; } = 0;

            public int Player2Points { get; set; } = 0;

            public double Player1Time { get; set; } = 0;

            public double Player2Time { get; set; } = 0;
        }
        #endregion

        #region Mapping
        private Map MapMap(MapModel model, Map map = null)
        {
            if (map == null)
            {
                map = new Map();
            }
            map.Id = string.IsNullOrEmpty(model.Id) ? Guid.Empty : Helper.TransformGuid(model.Id);
            map.Title = model.Title;
            map.Description = model.Description;
            map.Path = model.Path;
            map.IsVisible = model.IsVisible;

            List<Level> levels = new List<Level>();
            foreach(var level in model.Levels)
            {
                var l = new Level();
                l.Id = string.IsNullOrEmpty(level.Id) ? Guid.Empty : Helper.TransformGuid(level.Id);
                l.MapId = string.IsNullOrEmpty(level.MapId) ? Guid.Empty : Helper.TransformGuid(level.MapId);
                l.Title = level.Title;
                l.Description = level.Description;

                List<Assignment> assignments = new List<Assignment>();
                foreach (var task in level.Assignments)
                {
                    var assignment = new Assignment();
                    assignment.Id = string.IsNullOrEmpty(task.Id) ? Guid.Empty : Helper.TransformGuid(task.Id);
                    assignment.LevelId = string.IsNullOrEmpty(task.LevelId) ? Guid.Empty : Helper.TransformGuid(task.LevelId);
                    assignment.Points = task.Points;
                    assignment.Title = task.Title;
                    assignment.Description = task.Description;
                    assignment.IsMultiSelect = task.IsMultiSelect;
                    assignment.HasArgs = task.HasArgs;
                    assignment.HasBadge = task.HasBadge;
                    assignment.BadgeId = (string.IsNullOrEmpty(task.BadgeId) || task.BadgeId == null) ? null : Helper.TransformGuid(task.BadgeId);
                    assignment.IsCoding = task.IsCoding;
                    assignment.InitialCode = task.InitialCode;
                    assignment.Seconds = task.Seconds;

                    List<TestCase> testCases = new List<TestCase>();
                    foreach (var testCase in task.TestCases)
                    {
                        var tCase = new TestCase();
                        tCase.Id = string.IsNullOrEmpty(testCase.Id) ? Guid.Empty : Helper.TransformGuid(testCase.Id);
                        tCase.AssignmentId = string.IsNullOrEmpty(testCase.AssignmentId) ? Guid.Empty : Helper.TransformGuid(testCase.AssignmentId);
                        tCase.Input = testCase.Input;
                        tCase.Output = testCase.Output;
                        testCases.Add(tCase);
                    }

                    List<Answer> answers = new List<Answer>();
                    foreach (var answer in task.Answers)
                    {
                        var a = new Answer();
                        a.Id = string.IsNullOrEmpty(answer.Id) ? Guid.Empty : Helper.TransformGuid(answer.Id);
                        a.AssignmentId = string.IsNullOrEmpty(answer.AssignmentId) ? Guid.Empty : Helper.TransformGuid(answer.AssignmentId);
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

        private PlayerTask MapPlayerTask(InsertPlayerTaskModel model)
        {
            PlayerTask entity = new PlayerTask();
            entity.PlayerId = Helper.TransformGuid(model.PlayerId);
            entity.AssignmentId = Helper.TransformGuid(model.AssignmentId);
            entity.ScoredPoints = model.ScoredPoints;
            entity.Percentage = Math.Round(model.Percentage, 2);
            entity.Answers = model.Answers;
            entity.MapId = Helper.TransformGuid(model.MapId);
            entity.PlayersCode = model.PlayersCode;
            entity.ExecutionTime = model.ExecutionTime;
            entity.BadgeId = string.IsNullOrEmpty(model.BadgeId) ? null : Helper.TransformGuid(model.BadgeId);

            return entity;
        }

        private void MapBattle(UpdateBattleModel model, Battle entity)
        {
            if(model.Player1Time != 0 && model.Player1Points != 0)
            {
                entity.Player1Time = model.Player1Time!;
                entity.Player1Points = model.Player1Points;
            }
            if(model.Player2Time != 0 && model.Player2Points != 0)
            {
                entity.Player2Points = model.Player2Points;
                entity.Player2Time = model.Player2Time;
            }

            return;
        }

        #endregion
    }
}
