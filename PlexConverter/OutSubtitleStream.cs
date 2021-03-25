using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

namespace PlexConverter
{
    public class OutSubtitleStream : IChecker
    {
        private readonly string[] _subtitleCodecs = { "dvd_subtitle", "mov_text" };
        private ISubtitleStream _stream;
        private bool _needsProcessing;
        public bool NeedsProcessing { get => _needsProcessing; }
        public string Codec { get => _stream.Codec; }
        public OutSubtitleStream(ISubtitleStream subtitleStream)
        {
            _stream = subtitleStream;
            _needsProcessing = !_subtitleCodecs.Contains(_stream.Codec);
        }
    }
}
