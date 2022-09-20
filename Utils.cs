using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoNightBeyondTool
{
    static public class Utils
    {

        public static Dictionary<string, string> loadExportTable(string tablePath)
        {
            Dictionary<string, string> tableData = new Dictionary<string, string>();

            foreach (string line in File.ReadLines(tablePath, Encoding.UTF8))
            {
                if (line.Length > 0)
                {
                    int index = line.IndexOf("=") + 1;
                    string key = line.Substring(0, index-1);
                    string value = line.Substring(index, line.Length-index);

                    tableData.Add(key, value);
                }
            }

            return tableData;
        }

        public static Dictionary<string, string> loadExportInversTable(string tablePath)
        {
            Dictionary<string, string> tableData = new Dictionary<string, string>();

            foreach (string line in File.ReadLines(tablePath, Encoding.UTF8))
            {
                if (line.Length > 0)
                {
                    int index = line.IndexOf("=") + 1;
                    string key = line.Substring(0, index - 1);
                    string value = line.Substring(index, line.Length - index);

                    //if(!tableData.ContainsKey(value))
                        tableData.Add(value, key);
                }
            }

            return tableData;
        }

        public static Dictionary<string, string> loadImportTable(string tablePath)
        {
            Dictionary<string, string> tableData = new Dictionary<string, string>();
            //Con esto leemos el fichero de la tabla y sacamos primero el byte a string y el char.
            foreach (string line in File.ReadLines(tablePath, Encoding.UTF8))
            {
                if (line.Length > 0)
                {
                    int index = line.IndexOf("=") + 1;
                    string key = line.Substring(0, index - 1);
                    string value = line.Substring(index, line.Length - index);
                    tableData.Add(value, key);
                }
            }

            return tableData;
        }

        public static void showConsoleText(string text, bool lineJumpExtra = false)
        {
            Console.WriteLine(text);

            if (lineJumpExtra)
                Console.WriteLine("\n");
        }
    }
}
