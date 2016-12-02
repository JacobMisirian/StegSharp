using System;

namespace StegSharp
{
    public class StegSharpArgumentParser
    {
        private string[] args;
        private int position;

        public StegSharpConfig Parse(string[] args)
        {
            if (args.Length <= 0)
                displayHelp();

            this.args = args;
            var config = new StegSharpConfig();

            for (position = 0; position < args.Length; position++)
            {
                switch (args[position].ToLower())
                {
                    case "-d":
                    case "--decrypt":
                        config.StegSharpMode = StegSharpMode.Decrypt;
                        break;
                    case "-e":
                    case "--encrypt":
                        config.StegSharpMode = StegSharpMode.Encrypt;
                        break;
                    case "-f":
                    case "--file":
                        config.InputFile = expectData("[FILE]");
                        break;
                    case "-h":
                    case "--help":
                        displayHelp();
                        break;
                    case "-i":
                    case "--image":
                        config.EncryptedImageFile = expectData("[IMAGE]");
                        break;
                    case "-o":
                    case "--output":
                        config.OutputFile = expectData("[FILE]");
                        break;
                    case "-s":
                    case "--source":
                        config.OriginalImageFile = expectData("[IMAGE]");
                        break;
                }
            }
            return config;
        }

        private void displayHelp()
        {
            Console.WriteLine("Syntax: StegSharp [MODE] [FILES]");
            Console.WriteLine("-h --help              Displays this help and exits.");

            Console.WriteLine("\nMODES");
            Console.WriteLine("-d --decrypt           Turns on decrypt mode.");
            Console.WriteLine("-e --encrypt           Turns on encrypt mode.");

            Console.WriteLine("\nFILES");
            Console.WriteLine("-f --file   [FILE]     Specifies the input file for encryption.");
            Console.WriteLine("-i --input  [IMAGE]    Specifies the input image for decryption.");
            Console.WriteLine("-s --source [IMAGE]    Specifies the source unencrypted image.");

            die();
        }

        private string expectData(string type)
        {
            if (args[++position].StartsWith("-"))
                die("Expected data of type {0}, instead got flag {1}!", type, args[position]);
            return args[position];
        }

        private void die(string msg = "", params string[] args)
        {
            Console.WriteLine(msg, args);
            Environment.Exit(0);
        }
    }
}

