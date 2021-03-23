using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

namespace PlexConverter
{
    public class AudioEncoder
    {
        private IAudioStream _stream;
        public AudioEncoder(IAudioStream audioStream)
        {
            _stream = audioStream;
        }

        public IAudioStream Encode()
        {
            var tempDir = Path.Combine(Path.GetDirectoryName(_stream.Path), ".temp");
            Directory.CreateDirectory(tempDir);
            var outputPath = Path.Combine(tempDir, $"{_stream.Index}-audio.mp4");
            var outputPathAAC = Path.Combine(tempDir, $"{_stream.Index}-audio.aac");
            var outputPathDDP = Path.Combine(tempDir, $"{_stream.Index}-audio.ec3");
            var checker = new AudioChecker(_stream);
            if (checker.NeedsProcessing)
            {
                var encoderInfo = new ProcessStartInfo();
                encoderInfo.FileName = ToolsConfig.FFmpegPath;
                encoderInfo.Arguments = @$"-i {_stream.Path} -map 0:{_stream.Index} -codec:a aac -b:a 256k -ac 2 -ar {_stream.SampleRate} {outputPath}";
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
                var newMediaFile = new MediaFile(outputPath);
                return newMediaFile.AudioStreams.First();
            }
            return _stream;
        }
    }
}
