using MediaInfo;
using MediaInfo.Model;
using System.Collections.Generic;
using System.IO;

namespace PlexConverter
{
    public class MediaFile
    {
        public string MediaPath { get; }
        public string MediaContainer { get; }
        public IList<VideoStream> VideoStreams { get; }
        public IList<AudioStream> AudioStreams { get; }
        public IList<SubtitleStream> SubtitleStreams { get; }
        public MediaFile(string path)
        {
            var mediaInfo = new MediaInfoWrapper(path);
            MediaPath = path;
            MediaContainer = Path.GetExtension(path).TrimStart('.');
            VideoStreams = mediaInfo.VideoStreams;
            AudioStreams = mediaInfo.AudioStreams;
            SubtitleStreams = mediaInfo.Subtitles;
        }
    }
}
