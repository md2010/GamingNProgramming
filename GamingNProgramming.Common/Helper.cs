using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingNProgramming.Common
{
    public static class Helper
    {
        public static Guid TransformGuid(string guid)
        {
            return Guid.Parse(guid);
        }

        public static int GetPoints(int passed, int count, int points)
        {
            var percentage = (decimal)passed / count;
            var scored = (decimal)percentage * points;
            return (int)Math.Round(scored, 0, MidpointRounding.AwayFromZero);
        }
    }
}
