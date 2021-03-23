using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

namespace PlexConverter
{
    public class AudioChecker : IChecker
    {
        private readonly string[] _audioCodecs = { "aac", "eac3", "ac3" };
        private IAudioStream _stream;
        private bool _needsProcessing;

        public bool NeedsProcessing { get => _needsProcessing; }
        public string Codec { get; }
        public string Lang { get; }
        public int Channels { get; }
        public long Bitrate { get; }

        public AudioChecker(IAudioStream audioStream)
        {
            _stream = audioStream;
            _needsProcessing = !_audioCodecs.Contains(_stream.Codec);
            Codec = CheckCodec();
            Lang = CheckLang();
            Channels = CheckChannels();
            Bitrate = CheckBitrate();
        }
        private string CheckCodec()
        {
            if (_audioCodecs.Contains(_stream.Codec)) return "copy";
            else
            {
                if (CheckChannels() == 6) return "eac3";
                else return "aac";
            }
        }
        private string CheckLang()
        {
            return _stream.Language;
        }
        private int CheckChannels()
        {
            if(_stream.Channels >= 6) return 6;
            else return 2;
        }
        private long CheckBitrate()
        {
            if (Codec == "eac3") return 768000;
            if (Codec == "aac") return 256000;
            else return _stream.Bitrate;
        }
    }
}
