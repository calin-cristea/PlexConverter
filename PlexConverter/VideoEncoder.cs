using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

namespace PlexConverter
{
    public class VideoEncoder
    {
        private IVideoStream _videoStream;
        public VideoEncoder(IVideoStream videoStream)
        {
            _videoStream = videoStream;
        }
        public IVideoStream Encode()
        {
            var tempDir = Path.Combine(Path.GetDirectoryName(_videoStream.Path), ".temp");
            Directory.CreateDirectory(tempDir);
            var outputPath = Path.Combine(tempDir, "0-video.h265");
            var checker = new VideoChecker(_videoStream);
            if(checker.NeedsProcessing)
            {
                var encoderInfo = new ProcessStartInfo();
                encoderInfo.FileName = ToolsConfig.NVEncCPath;
                // --dhdr10-info copy
                encoderInfo.Arguments = @$"--avhw --input {_videoStream.Path} --output-res {checker.Resolution}x-2 --sar 1:1 --vpp-resize lanczos --fps {checker.Framerate} --avsync cfr --codec h265 --profile main --level 5.1 --tier high --vbrhq {checker.Bitrate} --preset quality --output-depth 8 --max-bitrate {checker.Bitrate + (checker.Bitrate / 3)} --qp-init 1 --vbr-quality 1 --lookahead 32 --gop-len {(int)checker.Framerate * 10} --bframes 3 --ref 3 --weightp --nonrefp --bref-mode middle --aq --aq-temporal --mv-precision Q-pel --colorrange auto --colormatrix auto --colorprim auto --transfer auto --chromaloc auto --max-cll copy --master-display copy --atc-sei auto --aud --pic-struct --output {outputPath}";
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
                return newMediaFile.VideoStreams.First();
            }
            return _videoStream;
        }
    }
}
