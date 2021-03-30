using System;
using System.Collections.Generic;
using System.Text;

namespace PlexConverter
{
    public interface IOutStream
    {
        string StreamPath { get; }
        int ID { get; }
    }
}
