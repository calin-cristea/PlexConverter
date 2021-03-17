using System;
using System.Collections.Generic;
using System.Text;

namespace PlexConverter
{
    public interface IChecker
    {
        bool NeedsProcessing { get; }
    }
}
