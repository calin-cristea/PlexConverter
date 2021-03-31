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
            muxerInfo.Arguments = $@"-add ""{outVideoStream.StreamPath}""#{outVideoStream.ID + 1}:hdlr=vide:name=VideoHandler";
            foreach (AudioStream audioStream in _mediaFile.AudioStreams)
            {
                var outAudioStream = new OutAudioStream(audioStream, _mediaFile.MediaPath);
                muxerInfo.Arguments += $@" -add ""{outAudioStream.StreamPath}""#{outAudioStream.ID + 1}:hdlr=soun:name=AudioHandler:lang={outAudioStream.Lang}:group=1";
                if(!outAudioStream.IsDefault)
                {
                    muxerInfo.Arguments += $@":disable";
                }
            }
            foreach (SubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            {
                var outSubtitleStream = new OutSubtitleStream(subtitleStream, _mediaFile.MediaPath);
                var handler = outSubtitleStream.Codec == SubtitleCodec.Vobsub ? "subp" : "sbtl";
                muxerInfo.Arguments += $@" -add ""{outSubtitleStream.StreamPath}""#{outSubtitleStream.ID + 1}:hdlr={handler}:name=SubtitleHandler:lang={outSubtitleStream.Lang}:group=2:disable";
            }
            muxerInfo.Arguments += $@" -fps {outVideoStream.Framerate} ""{outputPath}""";
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
            var outputPath = Path.Combine(Path.GetDirectoryName(_mediaFile.MediaPath), $"{Path.GetFileNameWithoutExtension(_mediaFile.MediaPath)}-new.mkv");
            File.Delete(outputPath);
            var muxerInfo = new ProcessStartInfo();
            muxerInfo.FileName = ToolsConfig.MkvMergePath;
            muxerInfo.Arguments = $@"-o ""{outputPath}""";
            var outVideoStream = new OutVideoStream(_mediaFile.VideoStreams.First(), _mediaFile.MediaPath);
            muxerInfo.Arguments += $@" --video-tracks {outVideoStream.ID} --no-audio --no-subtitles --no-buttons --no-track-tags --no-chapters --no-attachments --no-global-tags ""{outVideoStream.StreamPath}""";
            foreach (AudioStream audioStream in _mediaFile.AudioStreams)
            {
                var outAudioStream = new OutAudioStream(audioStream, _mediaFile.MediaPath);
                muxerInfo.Arguments += $@" --audio-tracks {outAudioStream.ID} --language {outAudioStream.ID}:{outAudioStream.Lang}";
                if (outAudioStream.IsDefault)
                {
                    muxerInfo.Arguments += $@" --default-track {outAudioStream.ID}:1";
                }
                muxerInfo.Arguments += $@" --no-video --no-subtitles --no-buttons --no-track-tags --no-chapters --no-attachments --no-global-tags ""{outAudioStream.StreamPath}""";
            }
            foreach (SubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            {
                var outSubtitleStream = new OutSubtitleStream(subtitleStream, _mediaFile.MediaPath);
                muxerInfo.Arguments += $@" --subtitle-tracks {outSubtitleStream.ID} --language {outSubtitleStream.ID}:{outSubtitleStream.Lang} --default-track {outSubtitleStream.ID}:0 --no-video --no-audio --no-buttons --no-track-tags --no-chapters --no-attachments --no-global-tags ""{outSubtitleStream.StreamPath}""";
            }
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
    }
}
