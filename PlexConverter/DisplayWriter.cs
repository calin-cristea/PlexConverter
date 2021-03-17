using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlexConverter
{
    public static class DisplayWriter
    {
        public static void DisplayMessage(string message)
        {
            Console.WriteLine($" {message}");
        }
        public static void DisplayMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine($" {message}");
            Console.ResetColor();
        }
        public static void DisplayInfo(MediaFile mediaInfo)
        {
            DisplayMessage(mediaInfo.MediaPath);
            DisplayMessage(" INFO:", ConsoleColor.Green);
            DisplayMessage("Container:", ConsoleColor.Yellow);
            DisplayMessage($"   format:     {mediaInfo.MediaContainer}");
            DisplayMessage("Video:", ConsoleColor.Yellow);
            DisplayMessage($"   codec:      {mediaInfo.VideoStreams.First().Codec}");
            DisplayMessage($"   resolution: {mediaInfo.VideoStreams.First().Width}x{mediaInfo.VideoStreams.First().Height}");
            DisplayMessage($"   framerate:  {mediaInfo.VideoStreams.First().Framerate}");
            DisplayMessage($"   bitrate:    {mediaInfo.VideoStreams.First().Bitrate / 1000} kbps");
            for (int i = 0; i < mediaInfo.AudioStreams.Count(); i++)
            {
                DisplayMessage($"Audio {i + 1}:", ConsoleColor.Yellow);
                DisplayMessage($"   codec:      {mediaInfo.AudioStreams.ElementAt(i).Codec}");
                DisplayMessage($"   lang:       {mediaInfo.AudioStreams.ElementAt(i).Language}");
                DisplayMessage($"   channels:   {mediaInfo.AudioStreams.ElementAt(i).Channels}");
                DisplayMessage($"   samplerate: {mediaInfo.AudioStreams.ElementAt(i).SampleRate} Hz");
                DisplayMessage($"   bitrate:    {mediaInfo.AudioStreams.ElementAt(i).Bitrate / 1000} kbps");
            }
            for (int i = 0; i < mediaInfo.SubtitleStreams.Count(); i++)
            {
                DisplayMessage($"Subtitle {i + 1}:", ConsoleColor.Yellow);
                DisplayMessage($"   codec:      {mediaInfo.SubtitleStreams.ElementAt(i).Codec}");
                DisplayMessage($"   lang:       {mediaInfo.SubtitleStreams.ElementAt(i).Language}");
            }
        }
    }
}
