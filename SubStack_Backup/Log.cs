using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text;

namespace SubStack_Backup
{
    class Log
    {
        // property
        public string savePath { get; set; }
        public string timeFormat { get; set; }
        private string dt
        {
            get;
            set
            {
                DateTime.Now.ToString(timeFormat);
            }
        }

        // 建構
        public Log()
        {
            savePath = @"‪C:\Users\waynejin\Downloads\Log.txt";
        }

        public Log(string path)
        {
            savePath = path;
        }

        // Method
        public void Record(string msg)
        {
            using(StreamWriter sw = new StreamWriter(savePath, true, Encoding.UTF8))
            {
                sw.WriteLine(msg);
                // TODO: 前面加上格式化的時間資訊
            }
        }
    }
}
