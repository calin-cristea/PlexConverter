using MediaInfo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PlexConverter
{
    public class OutVideoStream : IOutStream
    {
        private readonly VideoCodec[] _videoCodecs = { VideoCodec.Mpeg4IsoAvc, VideoCodec.MpeghIsoHevc };
        private VideoStream _stream;
        private string _streamPath;
        private int _streamID;
        private bool _needsProcessing;
        private int _resolution;
        private int _bitrate;
        public string StreamPath { get => _streamPath; }
        public int ID { get => _streamID; }
        public double Framerate { get => _stream.FrameRate; }
        public OutVideoStream(VideoStream videoStream, string path)
        {
            _stream = videoStream;
            _streamPath = path;
            _streamID = _stream.StreamNumber;
            _needsProcessing = !_videoCodecs.Contains(_stream.Codec);
            _resolution = CheckResolution();
            _bitrate = CheckBitrate();
            Encode();
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
        private int CheckBitrate()
        {
            if(_stream.Codec == VideoCodec.Mpeg4IsoAvc)
            {
                if(_resolution == 1280)
                {
                    if (_stream.Bitrate < 4000000 || _stream.Bitrate > 8000000) _needsProcessing = true;
                    return 3000;
                }
                if(_resolution == 1920)
                {
                    if (_stream.Bitrate < 8000000 || _stream.Bitrate > 16000000) _needsProcessing = true;
                    return 6000;
                }
                _needsProcessing = true;
                return 24000;
            }
            if(_stream.Codec == VideoCodec.MpeghIsoHevc)
            {
                if (_resolution == 1280)
                {
                    if (_stream.Bitrate < 2000000 || _stream.Bitrate > 4000000) _needsProcessing = true;
                    return 3000;
                }
                if (_resolution == 1920)
                {
                    if (_stream.Bitrate < 4000000 || _stream.Bitrate > 8000000) _needsProcessing = true;
                    return 6000;
                }
                if (_stream.Bitrate < 16000000 || _stream.Bitrate > 32000000) _needsProcessing = true;
                return 24000;
            }
            if (_resolution == 1280) return 3000;
            if (_resolution == 1920) return 6000;
            return 24000;
        }
        private void Encode()
        {
            Directory.CreateDirectory(ToolsConfig.TempPath);
            var outputPath = Path.Combine(ToolsConfig.TempPath, "0-video.h265");
            if (_needsProcessing)
            {
                var encoderInfo = new ProcessStartInfo();
                encoderInfo.FileName = ToolsConfig.NVEncCPath;
                // --dhdr10-info copy
                encoderInfo.Arguments = $@"--avhw --input ""{StreamPath}"" --output-res {_resolution}x-2 --sar 1:1 --vpp-resize lanczos --fps {_stream.FrameRate} --avsync forcecfr --codec h265 --profile main --level 5.1 --tier high --vbrhq {_bitrate} --preset quality --output-depth 8 --max-bitrate {_bitrate + (_bitrate / 3)} --qp-init 1 --vbr-quality 1 --lookahead 32 --gop-len {_stream.FrameRate * 10} --bframes 3 --ref 3 --weightp --nonrefp --bref-mode middle --aq --aq-temporal --mv-precision Q-pel --colorrange auto --colormatrix auto --colorprim auto --transfer auto --chromaloc auto --max-cll copy --master-display copy --atc-sei auto --aud --pic-struct --output ""{outputPath}""";
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
                _streamID = 0;
            }
        }
    }
}
