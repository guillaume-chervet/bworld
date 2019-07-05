using JsonDiffPatchDotNet;
using Newtonsoft.Json.Linq;
using System;
using Xunit;

namespace Demo.History.Data.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var jdp = new JsonDiffPatch();
            //var left = JToken.Parse(@"{ ""key"": false, ""text"" :""The program '[3572] dotnet.exe: Program Trace' has exited with code 0 (0x0)."" }");

            var left = JToken.Parse(@"{}");
            var right = JToken.Parse(@"{ ""key"": true, ""text"" :""The program dotnet '[3572] : Program Traces' has exited with code 0 (0x0)."" }");

            JToken patch = jdp.Diff(left, right);

            Console.WriteLine(patch.ToString());

            var output = jdp.Patch(left, patch);

            Console.WriteLine(output.ToString());
        }

        /*[Fact]
        public void Test2()
        {
            var left =
                    {
                      "id": 100,
                      "revision": 5,
                      "items": [
                        "car",
                        "bus"
                      ],
                      "tagline": "I can't do it. This text is too long for me to handle! Please help me JsonDiffPatch!",
                      "author": "wbish"
                    }

                        var right =
                        {
                      "id": 100,
                      "revision": 6,
                      "items": [
                        "bike",
                        "bus",
                        "car"
                      ],
                      "tagline": "I can do it. This text is not too long. Thanks JsonDiffPatch!",
                      "author": {
                        "first": "w",
                        "last": "bish"
                      }
                    }

                    var jdp = new JsonDiffPatch();
                    var output = jdp.Diff(left, right);

        }*/
    }
}
