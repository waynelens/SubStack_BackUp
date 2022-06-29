using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace SubStack_Backup
{
    class TimeMesaure : IDisposable
    {
        // property
        private readonly Stopwatch sw = new Stopwatch();
        private readonly string _title;
        private static bool Enable = false;
        private Log log = new Log();

        // 建構
        public TimeMesaure(string title, bool enable)
        {
            Enable = enable;
            if (!Enable) { return; }
            _title = title;
            sw.Start();
        }

        // Method
        public void Dispose()
        {
            if (!Enable) { return; }
            sw.Stop();
            // Log... ...
        }
    }
}
