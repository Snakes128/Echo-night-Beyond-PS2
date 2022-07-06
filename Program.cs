using System.Text;

namespace EchoNightBeyondTool
{
    internal class Program
    {
        static string extensionFMG = ".fmg";
        static string extensionTXT = ".txt";
        static string exportTable = "export.tbl";
        static string importTable = "import.tbl";

        static bool isModeTest = false;
        static string testFileFMG = "menu.fmg";
        //static string testFileFMG = "m_101-01-00.fmg"; 
        //static string testFileTXT = "menu.txt";
        static string testFileTXT = "m_101-01-00.txt"; 

        public struct rangeStruct
        {
            public rangeStruct(int _start, int _end)
            {
                start = _start;
                end = _end;
                count = end - start + 1;
                pointers = new List<pointerStruct>(count);

                for(int i=0; i < count; ++i)
                    pointers.Add(new pointerStruct(start+i, ""));
            }

            public rangeStruct(int _start, int _end, List<string> text)
            {
                start = _start;
                end = _end;
                count = end - start + 1;
                pointers = new List<pointerStruct>(count);

                for (int i = 0; i < count; ++i)
                    pointers.Add(new pointerStruct(start + i, text[i]));
            }

            public int start;
            public int end;
            public int count;
            public List<pointerStruct> pointers;
        }

        public struct pointerStruct
        {
            public pointerStruct(int _id, string _pointer)
            {
                id = _id;
                pointer = _pointer;
            }

            public int id;
            public string pointer;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Programa creado por Snake128 (SwitchCord Traducciones).");
            Console.WriteLine("Herramienta del juego Echo Night Beyond para PS2.");
            Console.WriteLine("\n");

            if (args.Length == 0)
            {
                isModeTest = true;
                FileInfo fileInfo = new FileInfo(testFileTXT);

                extractBinaryToTxt(fileInfo);
                //importTxtToBinary(fileInfo);
                return;
            }

            if (File.Exists(args[0]))
            {
                FileInfo fileInfo = new FileInfo(args[0]);

                if (fileInfo.Extension.CompareTo(extensionFMG) == 0)
                    extractBinaryToTxt(fileInfo);
                else if (fileInfo.Extension.CompareTo(extensionTXT) == 0)
                    importTxtToBinary(fileInfo);
            }

        }

        static void extractBinaryToTxt(FileInfo file)
        {
            //primero de todo cargamos el binario y leemos 

            //4 bytes no me hacen falta
            //4 bytes, tamaño total, tendremos que cambiarlo si es diferente, recordar que el tamaño total tiene que ser múltiplo de 32
            //4 bytes vacios??
            //4 bytes numero de rangos

            //4 bytes numero de punteros
            //4 bytes posición donde empiezan los punteros
            //4 bytes vacios??
            //4 bytes vacios??

            //Estructura de rangos, normalmente solo hay 1 rango por lo tanto sera:
            //4 bytes inicio del rango == 0
            //4 bytes fin rango == numero de punteros - 1

            //Ahora ya podemos extraer los punteros.
            //Una vez extraidos, procedemos a extraer los textos hasta que haya un 0, que significa fin del texto/puntero y pasamos a otro. Si el 
            //puntero es 0 lo obviamos, pero lo necesitamos para reconstruir todo.

            //Vale una vez todo estudiado, procedemos a guardar el tamaño del archivo y el binario en formato64, para extraerlo todo sin problema.

            Console.WriteLine("Extracting " + file.Name);

            //cargamos la tabla para usar luego
            //Dictionary<string, string> tableExport;
            //if (File.Exists(exportTable))
            var tableExport = Utils.loadExportTable(exportTable);

            string newTxt;
            if (isModeTest)
                newTxt = "test.txt";
            else
                newTxt = file.FullName.Substring(0, (int)file.FullName.Length - extensionFMG.Length) + extensionTXT;

            StreamWriter fileTxt = new StreamWriter(newTxt);

            FileStream Stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read);
            BinaryReader Reader = new BinaryReader(Stream);

            //leemos todos los punteros necesarios
            Stream.Position = 4;
            int totalSize = Reader.ReadInt32();
            Stream.Position += 4;
            int totalRange = Reader.ReadInt32();
            int totalPointers = Reader.ReadInt32();
            int posInitPointers = Reader.ReadInt32();


            //aquí creamos los distintos rangos y los guardamos en una list, para usar luego.
            //nos ponemos donde empiezan los rangos que es en 0x20
            Stream.Position = 32;

            List<rangeStruct> ranges = new List<rangeStruct>();

            for (int i = 0; i < totalRange; ++i)
            {
                int start = Reader.ReadInt32();
                int end = Reader.ReadInt32();

                ranges.Add(new rangeStruct(start, end));

                if (Stream.Position != posInitPointers)
                    Reader.ReadInt32();
            }

            //ahora nos situamos en la primera posición de los punteros y empezamos a guardar (si es 0 lo guardamos igual)
            Stream.Position = posInitPointers;

            List<int> pointers = new List<int>();
            for (int i = 0; i < totalPointers; ++i)
                pointers.Add(Reader.ReadInt32());

            //una vez guardamos los punteros los recorremos y empezamos a extraer los textos, en un principio al extraer los punteros ahora nos
            //deberíamos situar en el primera posición del texto, de todas formas moveremos el stream según los punteros.
            int countPointersInRanges = 0;
            for (int r = 0; r < totalRange; ++r)
            {
                rangeStruct range = ranges[r];
                int countPointers = 0;
                for (int i = countPointersInRanges; i < pointers.Count; ++i)
                {
                    if (countPointers >= range.count)
                    {
                        countPointersInRanges = i;
                        break;
                    }

                    Stream.Position = (long)pointers[i];

                    //como no podemos saber el último tamaño de texto, tengo que recorrerlo hasta que sea 0.
                    StringBuilder text = new StringBuilder();
                    text.Append(range.pointers[countPointers].id + "=");

                    if (pointers[i] == 0)
                    {
                        countPointers++;
                        countPointersInRanges = i;
                        fileTxt.WriteLine(text);
                        continue;
                    }

                    countPointers++;
                    countPointersInRanges = i;

                    while (true)
                    {
                        byte[] currentByte = Reader.ReadBytes(1);

                        if (currentByte[0] != 0/* && currentByte[0] != 10*/)
                        {
                            if (currentByte[0] > 127)
                            {
                                Stream.Position -= 1;
                                currentByte = Reader.ReadBytes(2);
                            }

                            string newChar;
                            string newStr = BitConverter.ToString(currentByte);

                            if (newStr.IndexOf("-") > -1)
                            {
                                int index = newStr.IndexOf("-");
                                newStr = newStr.Remove(index, 1);
                            }

                            tableExport.TryGetValue(newStr, out newChar);
                            if (newChar != null)
                                text.Append(newChar);
                        }
                        else if (currentByte[0] == 0)
                        {
                            //termina el texto, comprobamos que el byte es el último y añadimos la cadena a el txt.
                            fileTxt.WriteLine(text);
                            //fileTxt.Flush();
                            break;
                        }
                    }

                }
            }

            fileTxt.Close();
        }

        static void importTxtToBinary(FileInfo file)
        {

            Console.WriteLine("Import " + file);

            //cargamos la tabla para usar luego
            Dictionary<string, string> table;
            if (File.Exists(importTable))
                table = Utils.loadImportTable(importTable);
            else //if (File.Exists(exportTable))
                table = Utils.loadExportInversTable(exportTable);

            string[] allLines = File.ReadAllLines(file.FullName);

            //despues de leer la lineas tengo que separarlo en comentarios // y lineas de rangos
            //una vez hecho eso me pondré a montar el binario nuevo, los primeros 32bytes son fijos y a partir de ahí empezaré a 
            //meter los rangos, antes de eso los rangos los meteré en distintas estructuras como he hecho en la exportación

            List<string> listComments = new List<string>();
            List<string> listPointers = new List<string>();

            int magicNumber = 65536; // 00-00-01-00
            int totalSize = -1;
            int totalRanges = -1;
            int totalPointers = -1;
            int posInitPointers = - 1;

            foreach (var line in allLines)
            {
                if (line.IndexOf("//") != -1)
                    listComments.Add(line);
                else
                    listPointers.Add(line);
            }

            totalPointers = listPointers.Count;

            List<rangeStruct> listRangesStruct = new List<rangeStruct>();
            List<string> listPointersText = new List<string>();

            int rangeInit = -1;
            int rangeCurrent = -1;

            for(int i=0; i < listPointers.Count; ++i)
            {
                string line = listPointers[i];
                int index = line.IndexOf("=")+1;
                string rangeStr = line.Substring(0, index-1);
                int rangeInt = int.Parse(rangeStr);

                if (rangeInit < 0)
                    rangeInit = rangeInt;

                if (rangeCurrent < 0)
                {
                    rangeCurrent = rangeInt;
                    listPointersText.Add(line.Substring(index, line.Length - index));
                }
                else if (rangeInt == rangeCurrent + 1)
                {
                    listPointersText.Add(line.Substring(index, line.Length - index));
                    rangeCurrent = rangeInt;
                }
                else if (rangeInt != rangeCurrent + 1)
                {
                    //nuevo rango
                    listRangesStruct.Add(new rangeStruct(rangeInit, rangeCurrent, listPointersText));
                    listPointersText.Clear();
                    rangeInit = -1;
                    rangeCurrent = -1;
                    i--;
                }

            }

            if(listPointersText.Count > 0)
                listRangesStruct.Add(new rangeStruct(rangeInit, rangeCurrent, listPointersText));

            totalRanges = listRangesStruct.Count;

            string newFMGFile;
            if (isModeTest)
                newFMGFile = "test.fmg";
            else
                newFMGFile = file.FullName.Substring(0, (int)file.FullName.Length - extensionFMG.Length) + extensionFMG;

            if (File.Exists(newFMGFile))
                File.Delete(newFMGFile);

            File.Create(newFMGFile).Close();

            FileStream Stream = new FileStream(newFMGFile, FileMode.Open, FileAccess.Write);
            BinaryWriter Writer = new BinaryWriter(Stream);

            //escribimos los primeros 32bytes a 0
            byte[] init = new byte[32];
            Writer.Write(init);

            int countRange = 0;
            //empezamos a escribir los rangos y su diferencia
            for(int i=0; i < listRangesStruct.Count ;++i)
            {
                var range = listRangesStruct[i];

                Writer.Write(range.start);
                Writer.Write(range.end);

                if (i + 1 < listRangesStruct.Count)
                {
                    countRange += range.count;
                    Writer.Write(countRange); 
                }
            }

            posInitPointers = (int)Stream.Position;

            //ahora metemos tantos punteros como tengan todos los rangos
            for (int i = 0; i < totalPointers; ++i)
                Writer.Write((int)0);

            List<int> posAllPointers = new List<int>();

            //ahora metemos los textos, recuerda que necesito la tabla de conversión.
            for (int i = 0; i < listRangesStruct.Count; ++i)
            {
                var range = listRangesStruct[i];

                for (int j = 0; j < range.pointers.Count; ++j)
                {
                    string text = range.pointers[j].pointer;

                    if (text.Length == 0)
                        posAllPointers.Add((int)0);
                    else
                        posAllPointers.Add((int)Stream.Position);

                    if (text.Length != 0)
                    {
                        //aquí tengo que recorrer todo el text, convirtiendo los carácteres con su table, cuadno haya un \n tengo que agregar el A0
                        //y cuando termine añadir un 00 para saber que termina el texto.

                        //StringBuilder newText = new StringBuilder();
                        List<byte> newText = new List<byte>();

                        for (int k=0; k < text.Length; ++k)
                        {    
                            string newChar;
                            string newStr = text[k].ToString();

                            if (newStr.CompareTo("\\") == 0)
                            {
                                k++;
                                newStr = "\\n";
                            }

                            if (newStr.CompareTo("‡") == 0)
                            {
                                k++;
                                newStr = "‡U";
                            }                    

                            table.TryGetValue(newStr, out newChar);
                            if (newChar != null)
                            {
                                if(newChar.Length <= 2)
                                    newText.Add(Convert.ToByte(newChar, 16));
                                else
                                {
                                    //con esto resuelvo los valores japoneses
                                    byte[] newb = new byte[2];
                                    newb[0] = Convert.ToByte(newChar.Substring(0, 2), 16);
                                    newb[1] = Convert.ToByte(newChar.Substring(2, 2), 16);

                                    newText.Add(newb[0]);
                                    newText.Add(newb[1]);
                                }
                            }

                        }

                        Writer.Write(newText.ToArray());
                        //salto de linea
                        Writer.Write((byte)0);
                    }
                }
            }

            totalSize = (int)Stream.Position;

            //comprobamos si el texto es múltiplo de 32, si no es así le añadimos caracteres hasta que lo sea
            int result = totalSize % 32;

            if (result != 0)
            {
                result = 32 - result;
                for (int i = 0; i < result; ++i)
                    Writer.Write((byte)0);

                totalSize = (int)Stream.Position;
            }

            //ahora podemos escribir los primeros 32 bytes
            Stream.Position = 0;
            Writer.Write(magicNumber);
            Writer.Write(totalSize);
            Writer.Write((int)0);
            Writer.Write(totalRanges);
            Writer.Write(totalPointers);
            Writer.Write(posInitPointers);

            Stream.Position = posInitPointers;
            //ahora con todas las posiciones de los punteros, podemos ir al inicio y empezar a ponerlos.
            for (int i = 0; i < posAllPointers.Count; ++i)
                Writer.Write(posAllPointers[i]);


            Writer.Close();

        }
    }
}