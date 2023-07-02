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
    }
}
