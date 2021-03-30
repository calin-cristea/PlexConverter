using MediaInfo.Model;
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
        public static void DisplayInfo(MediaFile mediaFile)
        {
            DisplayMessage(mediaFile.MediaPath);
            DisplayMessage(" INFO:", ConsoleColor.Green);
            DisplayMessage("Container:", ConsoleColor.Yellow);
            DisplayMessage($"   format:     {mediaFile.MediaContainer}");
            DisplayVideoInfo(mediaFile.VideoStreams.First());
            for (int i = 0; i < mediaFile.AudioStreams.Count(); i++)
            {
                DisplayMessage($"Audio {i + 1}:", ConsoleColor.Yellow);
                DisplayAudioInfo(mediaFile.AudioStreams.ElementAt(i));
            }
            for (int i = 0; i < mediaFile.SubtitleStreams.Count(); i++)
            {
                DisplayMessage($"Subtitle {i + 1}:", ConsoleColor.Yellow);
                DisplaySubtitleInfo(mediaFile.SubtitleStreams.ElementAt(i));
            }
        }
        private static void DisplayVideoInfo(VideoStream videoStream)
        {
            DisplayMessage("Video:", ConsoleColor.Yellow);
            DisplayMessage($"   codec:      {videoStream.Format}");
            DisplayMessage($"   resolution: {videoStream.Width}x{videoStream.Height}");
            DisplayMessage($"   framerate:  {videoStream.FrameRate}");
            DisplayMessage($"   bitrate:    {(int)(videoStream.Bitrate / 1000)} kbps");
        }
        private static void DisplayAudioInfo(AudioStream audioStream)
        {
            DisplayMessage($"   codec:      {audioStream.Codec}");
            DisplayMessage($"   lang:       {audioStream.Language}");
            DisplayMessage($"   channels:   {audioStream.AudioChannelsFriendly}");
            DisplayMessage($"   samplerate: {audioStream.SamplingRate} Hz");
            DisplayMessage($"   bitrate:    {(int)(audioStream.Bitrate / 1000)} kbps");
        }
        private static void DisplaySubtitleInfo(MediaInfo.Model.SubtitleStream subtitleStream)
        {
            DisplayMessage($"   codec:      {subtitleStream.Codec}");
            DisplayMessage($"   lang:       {subtitleStream.Language}");
        }
    }
}
