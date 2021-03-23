﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

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

            var mediaFile = new MediaFile(inputPath);

            DisplayWriter.DisplayInfo(mediaFile);

            var muxer = new Muxer(mediaFile);
            muxer.MuxerType = outputContainer;
            muxer.Convert();
            Console.WriteLine(ToolsConfig.FFmpegPath);
            Console.WriteLine(ToolsConfig.NVEncCPath);
            DisplayWriter.DisplayMessage("Press any key to exit ...", ConsoleColor.Green);
            Console.ReadKey();
        }
    }
}
