using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Blind
{
    class Translator
    {
        int mode;
        FileInfo path;

        public Translator(int mode, FileInfo path)
        {
            if (mode > 1)
                throw new IndexOutOfRangeException(mode + " must not be bigger than 1");
            this.mode = mode;
            this.path = path;
        }

        public void Save()
        {
            File.WriteAllBytes(path.FullName + TranslateExtension(), Translate());
        }

        private string TranslateExtension()
        {
            if (mode == 0)
                return ".png";
            return ".txt";
        }

        public byte[] Translate()
        {
            switch (mode)
            {
                case 0: //.txt
                    return BitmapToByteArray(FromText());
                case 1: //.png,.jpg...
                    return Encoding.UTF8.GetBytes(FromImage());
            }
            return new byte[0];
        }

        private Bitmap FromText()
        {
            List<string> lines = File.ReadAllLines(path.FullName).ToList();
            int width = lines[0].Length;
            lines.RemoveAt(0);
            List<int[]> flines = Group(lines);
            int height = flines.Count / width;
            Bitmap b = new Bitmap(width, height);
            int i = 0;
            foreach (int y in Enumerable.Range(0, b.Height))
                foreach (int x in Enumerable.Range(0, b.Width))
                {
                    int[] o = flines[i];
                    Color c = Color.FromArgb(o[3],o[0],o[1],o[2]);
                    b.SetPixel(x, y, c);
                    i++;
                }
            return b;
        }

        private string FromImage()
        {
            Bitmap b = new Bitmap(path.FullName);
            int lenght = b.Size.Width;
            List<string> l = new List<string>();
            l.Add(GetWhites(lenght));
            foreach (int y in Enumerable.Range(0, b.Height))
                foreach (int x in Enumerable.Range(0, b.Width))
                {
                    Color c = b.GetPixel(x, y);
                    l.Add(GetWhites(c.R));
                    l.Add(GetWhites(c.G));
                    l.Add(GetWhites(c.B));
                    l.Add(GetWhites(c.A));
                }
            return string.Join("\n", l.ToArray());
        }

        private List<int[]> Group(List<string> l)
        {
            List<int[]> tr = new List<int[]>();
            int sum = 0;
            int[] actual = new int[4];
            foreach(string n in l)
            {
                actual[sum] = n.Length;
                sum++;
                if (sum == 4)
                {
                    tr.Add(actual);
                    sum = 0;
                    actual = new int[4];
                }
            }
            return tr;
        }

        private string GetWhites(int lenght)
        {
            string whites = "";
            foreach (int w in Enumerable.Range(0, lenght))
                whites += " ";
            return whites;
        }

        private byte[] BitmapToByteArray(Bitmap m)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(m, typeof(byte[]));
        }
    }
}
