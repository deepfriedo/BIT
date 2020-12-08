using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBITProject
{
    public class FileLogger : LogBase
    {
        public string filePath = "fileLogger.txt";
        public override void Log(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.WriteLine(DateTime.Now + message);
                streamWriter.Close();
            }
        }
    }
}
