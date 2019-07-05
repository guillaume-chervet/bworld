using System;
using Demo.Data.History;
using Demo.Data.History.Models;
using Demo.Queue;
using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace Demo.History.Core
{
    public class History
    {
        private readonly IHistoryService _historyService;

        public History(IHistoryService historyService, IQueue queue)
        {
            _historyService = historyService;
        }

        public async System.Threading.Tasks.Task SaveAsync(string content, string id, string userId, CheckPointType checkPointType)
        {
            var history = await _historyService.GetHistoryAsync(id);

            var jsonDiffPatch = new JsonDiffPatch();
            var left = JToken.Parse("{}");

            if (history == null)
            {
                history = new HistoryDbModel()
                {
                    CheckPoints = new List<CheckPoint>(),
                    ElementId = id,
                };
            }
            else
            {
                foreach (var checkpoint in history.CheckPoints)
                {
                    var patch = JToken.Parse(checkpoint.Patch);
                    left = jsonDiffPatch.Patch(left, patch);
                }
            }

            var checkPoint = new CheckPoint()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                CheckPointType = checkPointType,
                Hash = Hash(content)
            };

            var lastRight = JToken.Parse(content);
            checkPoint.Patch = jsonDiffPatch.Diff(left, lastRight).ToString();
            await _historyService.AddAsync(id, checkPoint);
        }

        private string Hash(string message)
        {
            //convert the string into an array of Unicode bytes.
            var ue = new UnicodeEncoding();
            //Convert the string into an array of bytes.
            var messageBytes = ue.GetBytes(message);
            //Create a new instance of the SHA1Managed class to create 
            //the hash value.
            var sHhash = new SHA1Managed();
            //Create the hash value from the array of bytes.
            var hashValue = sHhash.ComputeHash(messageBytes);
            //Display the hash value to the console. 
            return hashValue.ToString();
        }


    }
}
