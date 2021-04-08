using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PlexConverter
{
    public static class ToolsConfig
    {
        public static string TempPath { get; set; }
        public static string FFmpegPath { get => GetPath("ffmpeg"); }
        public static string NVEncCPath { get => GetPath("NVEncC64"); }
        public static string Mp4boxPath { get => GetPath("mp4box"); }
        public static string MkvMergePath { get => GetPath("mkvmerge"); }
        public static string MkvExtractPath { get => GetPath("mkvextract"); }
        public static string Bdsup2subPath { get => GetPath("bdsup2sub++"); }
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
