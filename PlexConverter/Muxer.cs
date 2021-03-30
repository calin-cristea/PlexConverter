using MediaInfo.Model;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
            if (MuxerType == "mkv")
            {
                ConvertToMatroska();
            }
            else
            {
                ConvertToMP4();
            }
            Directory.Delete(ToolsConfig.TempPath, true);
        }
        private void ConvertToMP4()
        {
            var outputPath = Path.Combine(Path.GetDirectoryName(_mediaFile.MediaPath), $"{Path.GetFileNameWithoutExtension(_mediaFile.MediaPath)}-new.m4v");
            File.Delete(outputPath);
            var outVideoStream = new OutVideoStream(_mediaFile.VideoStreams.First(), _mediaFile.MediaPath);
            var muxerInfo = new ProcessStartInfo();
            muxerInfo.FileName = ToolsConfig.Mp4boxPath;
            muxerInfo.Arguments = $@"-add ""{outVideoStream.StreamPath}""#video:hdlr=vide:name=VideoHandler";
            foreach (AudioStream audioStream in _mediaFile.AudioStreams)
            {
                var outAudioStream = new OutAudioStream(audioStream, _mediaFile.MediaPath);
                muxerInfo.Arguments += $@" -add ""{outAudioStream.StreamPath}""#{outAudioStream.ID}:hdlr=soun:name=AudioHandler:group=1";
                if(audioStream.Id != 1)
                {
                    muxerInfo.Arguments += $@":disable";
                }
            }
            foreach (SubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            {
                var outSubtitleStream = new OutSubtitleStream(subtitleStream, _mediaFile.MediaPath);
                var handler = outSubtitleStream.Codec == SubtitleCodec.Vobsub ? "subp" : "sbtl";
                muxerInfo.Arguments += $@" -add ""{outSubtitleStream.StreamPath}""#{outSubtitleStream.ID}:hdlr={handler}:name=SubtitleHandler:lang={subtitleStream.Language}:group=2";
                if (subtitleStream.Id != 1)
                {
                    muxerInfo.Arguments += $@":disable";
                }
            }
            muxerInfo.Arguments += $@" ""{outputPath}""";
            try
            {
                using (Process muxer = Process.Start(muxerInfo))
                {
                    muxer.WaitForExit();
                }
            }
            catch
            {
                // errors
            }
        }
        private void ConvertToMatroska()
        {
            //    var outputPath = Path.Combine(Path.GetDirectoryName(_mediaFile.MediaPath), $"{Path.GetFileNameWithoutExtension(_mediaFile.MediaPath)}-new.mkv");
            //    File.Delete(outputPath);
            //    var convert = FFmpeg.Conversions.New();
            //    convert.SetOutputFormat(Format.matroska);
            //    convert.SetOutput(outputPath);
            //    convert.SetVideoSyncMethod(VideoSyncMethod.cfr);
            //    var videoEncodedStream = new VideoEncoder(_mediaFile.VideoStreams.First()).Encode();
            //    videoEncodedStream.SetCodec(VideoCodec.copy);
            //    convert.AddStream(videoEncodedStream);
            //    foreach (IAudioStream audioStream in _mediaFile.AudioStreams)
            //    {
            //        var audioEncodedStream = new AudioEncoder(audioStream).Encode();
            //        audioEncodedStream.SetCodec(AudioCodec.copy);
            //        convert.AddStream(audioEncodedStream);
            //    }
            //    foreach (ISubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            //    {
            //        subtitleStream.SetCodec(Xabe.FFmpeg.Streams.SubtitleStream.SubtitleCodec.copy);
            //        convert.AddStream(subtitleStream);
            //    }
            //    convert.OnProgress += async (sender, args) =>
            //    {
            //        if (args.Percent % 25 == 0) await Console.Out.WriteLineAsync($"[{args.Percent}%]");
            //    };
            //    convert.Start().Wait();
        }
    }
}
