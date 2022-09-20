using System.Text;

namespace EchoNightBeyondTool
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Programa creado por Snake128 (SwitchCord Traducciones).");
            Console.WriteLine("Herramienta del juego Echo Night Beyond para PS2.");
            Console.WriteLine("\n");

            if (File.Exists(args[0]))
            {
                FileInfo fileInfo = new FileInfo(args[0]);

                if (fileInfo.Extension.CompareTo(FromFilesConvert.extensionFMG) == 0)
                    extractBinaryToTxt(fileInfo);
                else if (fileInfo.Extension.CompareTo(FromFilesConvert.extensionTXT) == 0)
                    importTxtToBinary(fileInfo);
            }

            Console.Read();

        }

        static void extractBinaryToTxt(FileInfo file)
        {
            Utils.showConsoleText("Extrayendo binario a txt...");

            FromFilesConvert.extractFile(file);

            Utils.showConsoleText("Archivo extraido correctamente");
        }

        static void importTxtToBinary(FileInfo file)
        {
            Utils.showConsoleText("Creando binario de un txt...");

            FromFilesConvert.importFile(file);

            Utils.showConsoleText("Archivo creado correctamente");
        }
    }
}