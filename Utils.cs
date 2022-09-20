using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoNightBeyondTool
{
    static public class Utils
    {
        public static byte[] getPattern(int Offset)
        {
            while (Offset >= 65536)
                Offset -= 65536;

            string Hex = "";

            //como el pattern tiene que ser de dos valores, si vemos que es pequeño, el otro valor siempre es cero, por eso se lo rellenamos.
            if (Offset < 10)
                Hex += System.Convert.ToString(0, 16);

            Hex += System.Convert.ToString(Offset, 16);

            byte[] Pattern = new byte[2];
            Pattern[0] = System.Convert.ToByte(Hex.Substring(Hex.Length - 2, 2), 16);
            if ((Hex.Length - 2) > 0)
                Pattern[1] = System.Convert.ToByte(Hex.Substring(0, Hex.Length - 2), 16);

            return Pattern;
        }

        public static void loadTable(string tablePath, ref Dictionary<string, string> tableData, ref Dictionary<string, byte> tableDataInverse)
        {
            Dictionary<string, string> tempTableDataOut = new Dictionary<string, string>();
            Dictionary<string, byte> tempTableDataInverseOut = new Dictionary<string, byte>();

            //Con esto leemos el fichero de la tabla y sacamos primero el byte a string y el char.
            foreach (string line in File.ReadLines(tablePath, Encoding.Latin1))
            {
                if (line.Length > 0)
                {
                    string first = line.Substring(0, 2);
                    string second = line.Substring(3, 1);
                    byte firstByte = Convert.ToByte(first, 16);

                    tableData.Add(first, second);
                    tableDataInverse.TryAdd(second, firstByte);
                }
            }
        }

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

        public static Dictionary<string, byte> loadTableSpanish(string tablePath)
        {
            Dictionary<string, byte> tableData = new Dictionary<string, byte>();
            //Con esto leemos el fichero de la tabla y sacamos primero el byte a string y el char.
            foreach (string line in File.ReadLines(tablePath, Encoding.Latin1))
            {
                if (line.Length > 0)
                {
                    string s = line.Substring(2, 2);
                    byte b = byte.Parse(s);
                    tableData.Add(line.Substring(0, 1), b);
                }
            }

            return tableData;
        }

        public static Dictionary<string, int> loadSizeChars(string tablePath)
        {
            Dictionary<string, int> tableData = new Dictionary<string, int>();
            //Con esto leemos el fichero de la tabla y sacamos primero el byte a string y el char.
            foreach (string line in File.ReadLines(tablePath, Encoding.Latin1))
            {
                if (line.Length > 0)
                {
                    int Index = line.LastIndexOf("=");
                    string s = line.Substring(Index+1, (line.Length-1) - Index);
                    int size = int.Parse(s);
                    tableData.Add(line.Substring(0, Index), size);
                }
            }

            return tableData;
        }

        public static void swapBytes(ref byte[] bytes)
        {
            byte[] newBytes = new byte[2];

            for (int i = 0; i < bytes.Length; i += 2)
            {
                Array.Copy(bytes, i, newBytes, 0, 2);
                Array.Reverse(newBytes);
                Array.Copy(newBytes, 0, bytes, i, 2);
            }
        }

        public static bool compareByteArrays(byte[] firstArray, byte[] secondArray)
        {
            return firstArray.SequenceEqual(secondArray);
        }

        public static void showConsoleText(string text, bool lineJumpExtra = false)
        {
            Console.WriteLine(text);

            if (lineJumpExtra)
                Console.WriteLine("\n");
        }
    }
}
