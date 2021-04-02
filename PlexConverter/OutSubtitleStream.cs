using MediaInfo.Model;
using System;
using System.IO;
using System.Linq;

namespace PlexConverter
{
    public class OutSubtitleStream : IOutStream
    {
        private readonly SubtitleCodec[] _subtitleCodecs = { SubtitleCodec.Vobsub, SubtitleCodec.TextUtf8 };
        private SubtitleStream _stream;
        private string _streamPath;
        private int _streamID;
        private bool _needsProcessing;
        public SubtitleCodec Codec { get => _stream.Codec; }
        public string StreamPath { get => _streamPath; }
        public int ID { get => _streamID; }
        public string Lang { get => LanguageMirror.GetCode(_stream.Language); }
        public OutSubtitleStream(SubtitleStream subtitleStream, string path)
        {
            _stream = subtitleStream;
            _streamPath = path;
            _streamID = _stream.StreamNumber;
            _needsProcessing = !_subtitleCodecs.Contains(_stream.Codec);
            Encode();
        }
        private void Encode()
        {
            Directory.CreateDirectory(ToolsConfig.TempPath);
            var outputPath = Path.Combine(ToolsConfig.TempPath, $"{_stream.StreamNumber}-subtitle.srt");
            var encoder = new Converter();
            encoder.Path = ToolsConfig.FFmpegPath;
            if (_needsProcessing)
            {
                encoder.Args = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s srt ""{outputPath}""";
            }
            else
            {
                encoder.Args = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s copy ""{outputPath}""";
            }
            encoder.Convert(true, $"Processing subtitle {_stream.StreamPosition}...");
            _streamPath = outputPath;
            _streamID = 0;
        }
    }
}
