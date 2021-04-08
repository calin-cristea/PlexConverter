using MediaInfo.Model;
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
        }
        private void ConvertToMP4()
        {
            var outputPath = Path.Combine(Path.GetDirectoryName(_mediaFile.MediaPath), $"{Path.GetFileNameWithoutExtension(_mediaFile.MediaPath)}-new.m4v");
            File.Delete(outputPath);
            var muxer = new Converter();
            muxer.Path = ToolsConfig.Mp4boxPath;
            var outVideoStream = new OutVideoStream(_mediaFile.VideoStreams.First(), _mediaFile.MediaPath);
            muxer.Args = $@"-add ""{outVideoStream.StreamPath}""#{outVideoStream.ID + 1}:hdlr=vide:name=VideoHandler";
            foreach (AudioStream audioStream in _mediaFile.AudioStreams)
            {
                var outAudioStream = new OutAudioStream(audioStream, _mediaFile.MediaPath);
                muxer.Args += $@" -add ""{outAudioStream.StreamPath}""#{outAudioStream.ID + 1}:hdlr=soun:name=AudioHandler:lang={outAudioStream.Lang}:group=1";
                if(!outAudioStream.IsDefault)
                {
                    muxer.Args += $@":disable";
                }
            }
            foreach (SubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            {
                var outSubtitleStream = new OutSubtitleStream(subtitleStream, _mediaFile.MediaPath);
                var handler = outSubtitleStream.Codec == "VobSub" || outSubtitleStream.Codec == "PGS" ? "subp" : "sbtl";
                muxer.Args += $@" -add ""{outSubtitleStream.StreamPath}""#{outSubtitleStream.ID + 1}:hdlr={handler}:name=SubtitleHandler:lang={outSubtitleStream.Lang}:group=2:disable";
            }
            muxer.Args += $@" -fps {outVideoStream.Framerate} ""{outputPath}""";
            muxer.Convert(false, "Muxing to MP4...");
        }
        private void ConvertToMatroska()
        {
            var outputPath = Path.Combine(Path.GetDirectoryName(_mediaFile.MediaPath), $"{Path.GetFileNameWithoutExtension(_mediaFile.MediaPath)}-new.mkv");
            File.Delete(outputPath);
            var muxer = new Converter();
            muxer.Path = ToolsConfig.MkvMergePath;
            muxer.Args = $@"-o ""{outputPath}""";
            var outVideoStream = new OutVideoStream(_mediaFile.VideoStreams.First(), _mediaFile.MediaPath);
            muxer.Args += $@" --video-tracks {outVideoStream.ID} --no-audio --no-subtitles --no-buttons --no-track-tags --no-chapters --no-attachments --no-global-tags ""{outVideoStream.StreamPath}""";
            foreach (AudioStream audioStream in _mediaFile.AudioStreams)
            {
                var outAudioStream = new OutAudioStream(audioStream, _mediaFile.MediaPath);
                muxer.Args += $@" --audio-tracks {outAudioStream.ID} --language {outAudioStream.ID}:{outAudioStream.Lang}";
                if (outAudioStream.IsDefault)
                {
                    muxer.Args += $@" --default-track {outAudioStream.ID}:1";
                }
                muxer.Args += $@" --no-video --no-subtitles --no-buttons --no-track-tags --no-chapters --no-attachments --no-global-tags ""{outAudioStream.StreamPath}""";
            }
            foreach (SubtitleStream subtitleStream in _mediaFile.SubtitleStreams)
            {
                var outSubtitleStream = new OutSubtitleStream(subtitleStream, _mediaFile.MediaPath);
                muxer.Args += $@" --subtitle-tracks {outSubtitleStream.ID} --language {outSubtitleStream.ID}:{outSubtitleStream.Lang} --default-track {outSubtitleStream.ID}:0 --no-video --no-audio --no-buttons --no-track-tags --no-chapters --no-attachments --no-global-tags ""{outSubtitleStream.StreamPath}""";
            }
            muxer.Convert(false, "Muxing to matroska...");
        }
    }
}
