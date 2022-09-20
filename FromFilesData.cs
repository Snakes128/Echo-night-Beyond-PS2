namespace EchoNightBeyondTool
{
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

    internal class FromFilesData
    {
        public FromFilesData(int _start, int _end, List<string>? text = null)
        {
            start = _start;
            end = _end;
            count = end - start + 1;
            pointers = new List<pointerStruct>(count);

            for (int i = 0; i < count; ++i)
            {
                string newText = "";
                if (text != null)
                    newText = text[i];

                pointers.Add(new pointerStruct(start + i, newText));
            }
        }

        public int start;
        public int end;
        public int count;
        public List<pointerStruct> pointers;
    }
}
