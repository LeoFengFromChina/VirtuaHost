using System;
using System.IO;

namespace StandardFeature
{
    public class LogHelper
    {

        public static void LogError(string typeName, string msg)
        {
            WriteTextFileText(typeName, "Error", msg);
        }
        public static void LogError(string typeName, Exception ex)
        {
            WriteTextFileText(typeName, "Error", ex.Message);
        }

        public static void LogDebug(string typeName, string msg)
        {
            WriteTextFileText(typeName, "Debug", msg);
        }

        static string directory = AppDomain.CurrentDomain.BaseDirectory + "Logs";
        static string path = string.Empty;
        static FileStream fs = null;
        static StreamWriter sw = null;

        public static bool WriteTextFileText(string typeName, string logLevel, string content, bool autoShowTime = true)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            path = directory + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            try
            {
                fs = new FileStream(path, FileMode.OpenOrCreate);
                fs.Position = fs.Length;
                sw = new StreamWriter(fs);
                sw.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff") + " [" + typeName + "] " + "[" + logLevel + "] " + content);
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                sw.Close();
                fs.Close();
            }

            return true;
        }

    }
}
