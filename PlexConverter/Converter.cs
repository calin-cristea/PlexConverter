﻿using System;
using System.Diagnostics;

namespace PlexConverter
{
    public class Converter
    {
        public string Path { get;  set; }
        public string Args { get; set; }
        public void Convert(bool redirect, string message)
        {
            try
            {
                using (Process process = new Process())
                {
                    DisplayWriter.DisplayMessage(message, ConsoleColor.Green);
                    process.StartInfo.FileName = Path;
                    process.StartInfo.Arguments = Args;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = redirect;
                    //process.StartInfo.RedirectStandardInput = redirect;
                    //process.StartInfo.RedirectStandardOutput = redirect;
                    process.Start();
                    process.WaitForExit();
                    if (process.ExitCode > 0)
                    {
                        DisplayWriter.DisplayMessage($"Error {process.ExitCode}", ConsoleColor.Red);
                    }
                    else
                    {
                        DisplayWriter.DisplayMessage("Done!", ConsoleColor.Green);
                    }
                }
            }
            catch
            {
                // errors
            }
        }
    }
}
