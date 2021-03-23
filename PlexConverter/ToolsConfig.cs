﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PlexConverter
{
    public static class ToolsConfig
    {
        public static string FFmpegPath { get => GetPath("ffmpeg"); }
        public static string NVEncCPath { get => GetPath("NVEncC64"); }
        private static string GetPath(string bin)
        {
            var binName = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{bin}.exe" : bin;
            var paths = Environment.GetEnvironmentVariable("PATH");
            foreach(var path in paths.Split(Path.PathSeparator))
            {
                var fullPath = Path.Combine(path, binName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }
            return null;
        }
    }
}