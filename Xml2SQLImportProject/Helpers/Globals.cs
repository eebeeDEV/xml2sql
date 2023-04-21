using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xml2SqlImport.Helpers
{
    public static class Globals
    {

        public static bool isForTest { get; set; }

        public static int calcFieldSize(int maxSize)
        {
            if (maxSize == 0) return 5;
            if (maxSize < 10) return 10;
            if (maxSize < 20) return 20;
            if (maxSize < 40) return 40;
            if (maxSize < 60) return 60;
            if (maxSize < 100) return 100;
            if (maxSize < 200) return 200;
            if (maxSize < 400) return 400;
            if (maxSize < 600) return 600;
            if (maxSize < 1000) return 1000;
            if (maxSize < 2000) return 2000;
            if (maxSize < 4000) return 4000;
            return maxSize;
        }
    }
}
