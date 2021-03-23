using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xabe.FFmpeg;

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
        private static void DisplayVideoInfo(IVideoStream videoStream)
        {
            DisplayMessage("Video:", ConsoleColor.Yellow);
            DisplayMessage($"   codec:      {videoStream.Codec}");
            DisplayMessage($"   resolution: {videoStream.Width}x{videoStream.Height}");
            DisplayMessage($"   framerate:  {videoStream.Framerate}");
            DisplayMessage($"   bitrate:    {videoStream.Bitrate / 1000} kbps");
        }
        private static void DisplayAudioInfo(IAudioStream audioStream)
        {
            DisplayMessage($"   codec:      {audioStream.Codec}");
            DisplayMessage($"   lang:       {audioStream.Language}");
            DisplayMessage($"   channels:   {audioStream.Channels}");
            DisplayMessage($"   samplerate: {audioStream.SampleRate} Hz");
            DisplayMessage($"   bitrate:    {audioStream.Bitrate / 1000} kbps");
        }
        private static void DisplaySubtitleInfo(ISubtitleStream subtitleStream)
        {
            DisplayMessage($"   codec:      {subtitleStream.Codec}");
            DisplayMessage($"   lang:       {subtitleStream.Language}");
        }
    }
}
