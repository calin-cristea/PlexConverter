using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;

namespace PlexConverter
{
    public class MediaFile
    {
        public string MediaPath { get; }
        public string MediaContainer { get; }
        public IEnumerable<IVideoStream> VideoStreams { get; }
        public IEnumerable<IAudioStream> AudioStreams { get; }
        public IEnumerable<ISubtitleStream> SubtitleStreams { get; }
        public MediaFile(string path)
        {
            var ffmpegPath = Path.Combine(Environment.CurrentDirectory, "FFmpeg");
            if(!Directory.Exists(ffmpegPath)) Directory.CreateDirectory(ffmpegPath);
            FFmpeg.SetExecutablesPath(ffmpegPath);
            FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, ffmpegPath).Wait();
            MediaPath = path;
            MediaContainer = Path.GetExtension(path).TrimStart('.');
            VideoStreams = FFmpeg.GetMediaInfo(path).Result.VideoStreams;
            AudioStreams = FFmpeg.GetMediaInfo(path).Result.AudioStreams;
            SubtitleStreams = FFmpeg.GetMediaInfo(path).Result.SubtitleStreams;
        }
    }
}
