﻿using MediaInfo.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace PlexConverter
{
    public class OutAudioStream : IOutStream
    {
        private readonly AudioCodec[] _audioCodecs = { AudioCodec.Aac, AudioCodec.Eac3, AudioCodec.Ac3 };
        private AudioStream _stream;
        private string _streamPath;
        private int _streamID;
        private bool _needsProcessing;
        private string _codec;
        private int _channels;
        private int _bitrate;
        public string StreamPath { get => _streamPath; }
        public int ID { get => _streamID; }
        public string Lang { get => LanguageMirror.GetCode(_stream.Language); }
        public OutAudioStream(AudioStream audioStream, string path)
        {
            _stream = audioStream;
            _streamPath = path;
            _streamID = _stream.Id;
            _needsProcessing = !_audioCodecs.Contains(_stream.Codec);
            _codec = CheckCodec();
            _channels = CheckChannels();
            _bitrate = CheckBitrate();
            Encode();
        }
        private string CheckCodec()
        {
            if (CheckChannels() == 6) return "eac3";
            else return "aac";
        }
        private int CheckChannels()
        {
            if(_stream.Channel >= 6) return 6;
            else return 2;
        }
        private int CheckBitrate()
        {
            if (_codec == "eac3") return 768;
            if (_codec == "aac") return 256;
            else return (int)(_stream.Bitrate / 1000);
        }
        private void Encode()
        {
            Directory.CreateDirectory(ToolsConfig.TempPath);
            var outputPath = Path.Combine(ToolsConfig.TempPath, $"{_stream.StreamNumber}-audio.mp4");
            var outputPathAAC = Path.Combine(ToolsConfig.TempPath, $"{ID}-audio.aac");
            var outputPathDDP = Path.Combine(ToolsConfig.TempPath, $"{ID}-audio.ec3");
            if (_needsProcessing)
            {
                var encoderInfo = new ProcessStartInfo();
                encoderInfo.FileName = ToolsConfig.FFmpegPath;
                encoderInfo.Arguments = $@"-i ""{_streamPath}"" -map 0:{_stream.StreamNumber} -codec:a {_codec} -b:a {_bitrate}k -ac {_channels} -ar {_stream.SamplingRate} ""{outputPath}""";
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
}
