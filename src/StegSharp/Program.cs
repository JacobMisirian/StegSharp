using System;

namespace StegSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            new StegSharpArgumentParser().Parse(args).Execute();
        }
    }
}
