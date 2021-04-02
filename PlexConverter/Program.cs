using System;
using System.IO;

namespace PlexConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputPath = args[0];
            string outputContainer;
            if (args.Length == 2) outputContainer = args[1].TrimStart('-');
            else outputContainer = "mp4";
            ToolsConfig.TempPath = Path.Combine(Path.GetDirectoryName(inputPath), ".temp");

            var mediaFile = new MediaFile(inputPath);

            DisplayWriter.DisplayInfo(mediaFile);

            var muxer = new Muxer(mediaFile);
            muxer.MuxerType = outputContainer;
            muxer.Convert();
            DisplayWriter.DisplayMessage("Press any key to exit ...", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}
