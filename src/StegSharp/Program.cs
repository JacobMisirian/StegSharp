using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace StegSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var steg = new Steganography();
            switch (args[0])
            {
                case "-d":
                case "--decode":
                    Console.WriteLine(ASCIIEncoding.ASCII.GetString(steg.Decode((Bitmap)Bitmap.FromFile(args[1]), (Bitmap)Bitmap.FromFile(args[2]))));
                    break;
                case "-e":
                case "--encode":
                    steg.Encode((Bitmap)Bitmap.FromFile(args[1]), File.ReadAllBytes(args[2])).Save(args[3], ImageFormat.Png);
                    break;
            }
        }
    }
}
