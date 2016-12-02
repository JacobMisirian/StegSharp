using System;
using System.Drawing;
using System.IO;

namespace StegSharp
{
    public class StegSharpConfig
    {
        public string EncryptedImageFile { get; set; }
        public string OriginalImageFile { get; set; }
        public string OutputFile { get; set; }
        public string InputFile { get; set; }
        public StegSharpMode StegSharpMode { get; set; }

        public StegSharpConfig()
        {
            StegSharpMode = StegSharpMode.None;
        }

        public void Execute()
        {
            verifyConfig();

            Steganography steganography = new Steganography();
            switch (StegSharpMode)
            {
                case StegSharpMode.Decrypt:
                    File.WriteAllBytes(OutputFile, steganography.DecryptBitmap((Bitmap)Bitmap.FromFile(EncryptedImageFile), (Bitmap)Bitmap.FromFile(OriginalImageFile)));
                    break;
                case StegSharpMode.Encrypt:
                    steganography.EncryptBitmap((Bitmap)Bitmap.FromFile(OriginalImageFile), File.ReadAllBytes(InputFile)).Save(OutputFile);
                    break;
            }
        }

        private void verifyConfig()
        {
            if (StegSharpMode == StegSharpMode.None)
                die("No mode specified! Run StegSharp --help for help.");
            if (OriginalImageFile == string.Empty || OriginalImageFile == null)
                die("No image file specified!");
            if ((EncryptedImageFile == string.Empty || EncryptedImageFile == null) && StegSharpMode == StegSharpMode.Decrypt)
                die("No encrypted image file specified!");
            if ((InputFile == string.Empty || InputFile == null) && StegSharpMode == StegSharpMode.Encrypt)
                die("No input file specified!");

            if (OutputFile == string.Empty || OutputFile == null)
                if (StegSharpMode == StegSharpMode.Decrypt)
                    OutputFile = string.Format("{0}.bin", Path.GetFileNameWithoutExtension(EncryptedImageFile));
                else
                    OutputFile = string.Format("{0}.png", Path.GetFileName(OriginalImageFile));
        }

        private void die(string msg = "")
        {
            Console.WriteLine(msg);
            Environment.Exit(0);
        }
    }

    public enum StegSharpMode
    {
        Decrypt,
        Encrypt,
        None
    }
}

