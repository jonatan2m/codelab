using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeneralResources.Utils;
using Xunit;

namespace GeneralResources.AsyncAwait
{


    public class MakeTea
    {
        [Fact]
        public async Task Deferred_Await_Scenario()
        {
            var result = await MakeTeaAsync();
            Assert.True(true);
        }

        public async Task<string> MakeTeaAsync()
        {
            var boilingWater = BoilerWaterAsync();
            "take the cups out".Dump();
            "put tea in cups".Dump();

            var water = await boilingWater;

            var tea = $"pour {water} in cups".Dump();

            return tea;
        }

        public async Task<string> BoilerWaterAsync()
        {
            "Start the kettle".Dump();
            "waiting for the kettle".Dump();
            await Task.Delay(2000);
            "kettle finished boiling".Dump();

            return "water";
        }

    }
}