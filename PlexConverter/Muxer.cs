using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

namespace PlexConverter
{
    public class Muxer
    {
        private MediaFile _mediaFile;
        public string MuxerType { get; set; }
        public Muxer(MediaFile mediaInfo)
        {
            _mediaFile = mediaInfo;
        }

        public void Convert()
        {
            var muxerFormat = MuxerType == "mkv" ? "mkv" : "m4v";
            var outputPath = Path.Combine(Path.GetDirectoryName(_mediaFile.MediaPath), $"{Path.GetFileNameWithoutExtension(_mediaFile.MediaPath)}-new.{muxerFormat}");
            File.Delete(outputPath);
            var convert = FFmpeg.Conversions.New();
            convert.SetOutputFormat(MuxerType);
            if (MuxerType == "mkv") convert.SetOutputFormat(Format.matroska);
            else convert.SetOutputFormat(Format.mp4);
            convert.SetOutput(outputPath);
            convert.SetVideoSyncMethod(VideoSyncMethod.cfr);
            if (MuxerType == "mp4") convert.AddParameter($"-movflags +faststart");
            var videoStream = _mediaFile.VideoStreams.First();
            videoStream = new VideoEncoder(videoStream).Encode();
            videoStream.SetCodec(VideoCodec.copy);
            convert.AddStream(videoStream);
            foreach(IAudioStream audioStream in _mediaFile.AudioStreams)
            {
                var audioEncodedStream = new AudioEncoder(audioStream).Encode();
                audioEncodedStream.SetCodec(AudioCodec.copy);
                convert.AddStream(audioEncodedStream);
            }
            foreach (ISubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            {
                subtitleStream.SetCodec(Xabe.FFmpeg.Streams.SubtitleStream.SubtitleCodec.copy);
                convert.AddStream(subtitleStream);
            }
            convert.OnProgress += async (sender, args) =>
            {
                if (args.Percent % 25 == 0) await Console.Out.WriteLineAsync($"[{args.Percent}%]");
            };
            convert.Start().Wait();
        }
    }
}
