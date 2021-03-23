using System.Collections.Generic;
using System.IO;
using Xabe.FFmpeg;

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
            FFmpeg.SetExecutablesPath(Path.GetDirectoryName(ToolsConfig.FFmpegPath));
            MediaPath = path;
            MediaContainer = Path.GetExtension(path).TrimStart('.');
            VideoStreams = FFmpeg.GetMediaInfo(path).Result.VideoStreams;
            AudioStreams = FFmpeg.GetMediaInfo(path).Result.AudioStreams;
            SubtitleStreams = FFmpeg.GetMediaInfo(path).Result.SubtitleStreams;
        }
    }
}
