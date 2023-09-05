using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VideoToJson
{
    internal class Watch
    {
        Stopwatch sw;
        internal Watch() { sw = Stopwatch.StartNew(); }

        public void Start()
        {
            sw.Start();
        }
        public string StopResult()
        {
            sw.Stop();
            string asdf = sw.Elapsed.ToString();
            sw.Reset();
            return asdf;
        }
    }
}
