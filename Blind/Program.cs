using System;
using System.IO;

namespace Blind
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Title = "Blind";
                Console.Write("Path: ");
                string path = Console.ReadLine();
                FileInfo f = new FileInfo(path);
                if (!f.Exists)
                    throw new FileNotFoundException("File not found in: " + path);
                int mode = 2;
                switch (f.Extension)
                {
                    case ".png":
                        mode = 1;
                        break;
                    case ".jpg":
                        mode = 1;
                        break;
                    case ".jpeg":
                        mode = 1;
                        break;
                    case ".gif":
                        mode = 1;
                        break;
                    case ".bmp":
                        mode = 1;
                        break;
                    case ".ico":
                        mode = 1;
                        break;
                    case ".txt":
                        mode = 0;
                        break;
                    default:
                        throw new IOException("Unknow extension");
                }
                Translator t = new Translator(mode,f);
                t.Save();
            }
        }
    }
}
