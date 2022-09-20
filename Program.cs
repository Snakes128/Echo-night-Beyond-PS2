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

            bool removePause = false;

            if (args.Length <= 0)
            {
                Utils.showConsoleText("Es necesario pasarle un archivo fmg o txt para exportar/importar");
            }
            else if (File.Exists(args[0]))
            {
                FileInfo fileInfo = new FileInfo(args[0]);

                if (fileInfo.Extension.CompareTo(FromFilesConvert.extensionFMG) == 0)
                    extractBinaryToTxt(fileInfo);
                else if (fileInfo.Extension.CompareTo(FromFilesConvert.extensionTXT) == 0)
                    importTxtToBinary(fileInfo);

                if (args.Length == 2 && args[1].CompareTo("true") == 0)
                    removePause = true;
            }

            if(!removePause)
                Console.Read();
        }

        static void extractBinaryToTxt(FileInfo file)
        {
            Utils.showConsoleText("Extrayendo binario a txt...");

            bool result = FromFilesConvert.extractFile(file);

            if(!result)
                Utils.showConsoleText("Ha habido un error en la extracción del archivo, resuelva el problema y vuelva a intentarlo.");
            else
                Utils.showConsoleText("Archivo extraido correctamente");
        }

        static void importTxtToBinary(FileInfo file)
        {
            Utils.showConsoleText("Creando binario de un txt...");

            bool result = FromFilesConvert.importFile(file);

            if (!result)
                Utils.showConsoleText("Ha habido un error en la creación del archivo, resuelva el problema y vuelva a intentarlo.");
            else
                Utils.showConsoleText("Archivo creado correctamente");
        }
    }
}