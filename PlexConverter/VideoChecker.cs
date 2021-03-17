using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

namespace PlexConverter
{
    public class VideoChecker : IChecker
    {
        private readonly string[] _videoCodecs = { "h264", "hevc" };
        private IVideoStream _stream;
        private bool _needsProcessing;
        
        public bool NeedsProcessing { get => _needsProcessing; }
        public int Resolution { get; }
        public double Framerate { get => _stream.Framerate; }
        public long Bitrate { get; }

        public VideoChecker(IVideoStream videoStream)
        {
            _stream = videoStream;
            _needsProcessing = !_videoCodecs.Contains(_stream.Codec);
            Resolution = CheckResolution();
            Bitrate = CheckBitrate();
        }

        private int CheckResolution()
        {
            if (_stream.Width <= 1280)
            {
                if(_stream.Width < 1280) _needsProcessing = true;
                return 1280;
            }
            if (_stream.Width <= 1920) 
            {
                if (_stream.Width < 1920) _needsProcessing = true;
                return 1920;
            }
            if (_stream.Width < 3840) _needsProcessing = true;
            return 3840;
        }
        private long CheckBitrate()
        {
            if(_stream.Codec == "h264")
            {
                if(Resolution == 1280)
                {
                    if (_stream.Bitrate < 4000000 || _stream.Bitrate > 8000000) _needsProcessing = true;
                    return 3000;
                }
                if(Resolution == 1920)
                {
                    if (_stream.Bitrate < 8000000 || _stream.Bitrate > 16000000) _needsProcessing = true;
                    return 6000;
                }
                _needsProcessing = true;
                return 24000;
            }
            if(_stream.Codec == "hevc")
            {
                if (Resolution == 1280)
                {
                    if (_stream.Bitrate < 2000000 || _stream.Bitrate > 4000000) _needsProcessing = true;
                    return 3000;
                }
                if (Resolution == 1920)
                {
                    if (_stream.Bitrate < 4000000 || _stream.Bitrate > 8000000) _needsProcessing = true;
                    return 6000;
                }
                if (_stream.Bitrate < 16000000 || _stream.Bitrate > 32000000) _needsProcessing = true;
                return 24000;
            }
            if (Resolution == 1280) return 3000;
            if (Resolution == 1920) return 6000;
            return 24000;
        }
    }
}
