using MediaInfo.Model;
using System;
using System.IO;
using System.Linq;

namespace PlexConverter
{
    public class OutSubtitleStream : IOutStream
    {
        private readonly string[] _subtitleCodecs = { "VobSub", "TextUtf8" };
        private SubtitleStream _stream;
        private string _streamPath;
        private int _streamID;
        private bool _needsProcessing;
        public string Codec { get => _stream.Format; }
        public string StreamPath { get => _streamPath; }
        public int ID { get => _streamID; }
        public string Lang { get => LanguageMirror.GetCode(_stream.Language); }
        public OutSubtitleStream(SubtitleStream subtitleStream, string path)
        {
            _stream = subtitleStream;
            _streamPath = path;
            _streamID = _stream.StreamNumber;
            _needsProcessing = !_subtitleCodecs.Contains(_stream.Format);
            Encode();
        }
        private void Encode()
        {
            Directory.CreateDirectory(ToolsConfig.TempPath);
            var outputPath = Path.Combine(ToolsConfig.TempPath, $"{_stream.StreamNumber}-subtitle.srt");
            var encoder = new Converter();
            encoder.Path = ToolsConfig.FFmpegPath;
            if(Codec == "PGS" || Codec == "VobSub")
            {
                outputPath = Path.ChangeExtension(outputPath, "mks");
            }
            if (_needsProcessing)
            {
                if (Codec == "PGS")
                {
                    encoder.Args = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s dvdsub -f matroska ""{outputPath}""";
                }
                else
                {
                    encoder.Args = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s srt -f srt ""{outputPath}""";
                }
            }
            else
            {
                if (Codec == "VobSub")
                {
                    encoder.Args = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s copy -f matroska ""{outputPath}""";
                }
                else
                {
                    encoder.Args = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s copy -f srt ""{outputPath}""";
                }
            }
            encoder.Convert(true, $"Processing subtitle {_stream.StreamPosition + 1}...");
            if (Codec == "PGS" || Codec == "VobSub")
            {
                Demux(outputPath);
                outputPath = Path.ChangeExtension(outputPath, "idx");
            }
            _streamPath = outputPath;
            _streamID = 0;
        }
        private void Demux(string inputPath)
        {
            var demuxer = new Converter();
            demuxer.Path = ToolsConfig.MkvExtractPath;
            var outputPath = Path.ChangeExtension(inputPath, "idx");
            demuxer.Args = $@"""{inputPath}"" tracks 0:""{outputPath}""";
            demuxer.Convert(true, $"Extract processed subtitle...");
        }
    }
}
