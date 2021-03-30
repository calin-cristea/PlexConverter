using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MediaInfo;
using MediaInfo.Model;

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
        public string Lang { get => _stream.Language; }
        public OutSubtitleStream(SubtitleStream subtitleStream, string path)
        {
            _stream = subtitleStream;
            _streamPath = path;
            _streamID = _stream.Id;
            _needsProcessing = !_subtitleCodecs.Contains(_stream.Codec);
            Encode();
        }
        private void Encode()
        {
            Directory.CreateDirectory(ToolsConfig.TempPath);
            var outputPath = Path.Combine(ToolsConfig.TempPath, $"{_stream.StreamNumber}-subtitle.srt");
            var encoderInfo = new ProcessStartInfo();
            encoderInfo.FileName = ToolsConfig.FFmpegPath;
            if (_needsProcessing)
            {
                encoderInfo.Arguments = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s srt ""{outputPath}""";
            }
            else
            {
                encoderInfo.Arguments = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:s copy ""{outputPath}""";
            }
            try
            {
                using (Process encoder = Process.Start(encoderInfo))
                {
                    encoder.WaitForExit();
                }
            }
            catch
            {
                // errors
            }
            _streamPath = outputPath;
            _streamID = 1;
        }
    }
}
