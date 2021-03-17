using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlexConverter
{
    public class ContainerChecker
    {
        private readonly string[] _mediaContainers = { "m4v", "mkv", "mp4" };

        public bool NeedsProcessing { get; }

        public ContainerChecker(MediaFile mediaInfo)
        {
            NeedsProcessing = !_mediaContainers.Contains(mediaInfo.MediaContainer);
        }
    }
}
