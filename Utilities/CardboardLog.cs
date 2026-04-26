using System;
using System.IO;
using UnityEngine;

namespace Cardboard.Utilities
{
    /// <summary>
    /// Class for writing to logs.
    /// </summary>
    public class CardboardLog
    {
        private readonly StreamWriter _currentWriter;

        /// <summary>
        /// A divider used to seperate notable messages and text from other log messages.
        /// </summary>
        public const string Divider = "======================================================";

        /// <summary>
        /// Log the current line into the log.
        /// </summary>
        public void Log(string text, string ending)
        {
            var fmt = $"{text}{ending}";

            _currentWriter.Write(fmt);
            Debug.Log(fmt);
        }

        /// <summary>
        /// Log the current line into the log with the logLevel string.
        /// </summary>
        public void Log(string text, LogLevel logLevel = LogLevel.Info) => Log($"[{logLevel} | DT:{Time.deltaTime}]: {text}", "\n");

        /// <summary>
        /// Log an exception to the file. This logs the message, source, and stack trace of the exception.
        /// </summary>
        public void Log(Exception ex)
        {
            Log($"{Divider}\nAn exception occured!", "\n");
            Log($"  Message:   {ex.Message}", "\n");
            Log($"  Source:    {ex.Source}", "\n");
            Log( "  Stack:", "\n");
            Log(ex.StackTrace.Replace("\n", "\n\t").Trim().RemoveEnd("\n"), "\n");
            Log(Divider, "\n");
        }

        /// <summary>
        /// Log an error into the log.
        /// </summary>
        public void LogError(string text) => Log($"{text}", LogLevel.Error);

        /// <summary>
        /// Log a warning into the log.
        /// </summary>
        public void LogWarning(string text) => Log($"{text}", LogLevel.Warning);

        /// <summary>
        /// Close the CardboardLog and disable writing.
        /// </summary>
        public void Dispose()
        {
            _currentWriter.Dispose();
        }

        /// <summary>
        /// Create a new CardboardLog.
        /// </summary>
        /// <param name="uuid">The UUID of your mod.</param>
        /// <param name="outputFolder">The folder that your mod's logs will be collected in. By default, this is (GT)/Cardboard/logs/(uuid) which will be used if outputFolder = "BepInEx".</param>
        public CardboardLog(string uuid, string outputFolder = null)
        {
            var logsFolder = outputFolder ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cardboard", "logs", uuid);

            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);
            
            var logsFile = $"log_{DateTime.Now.ToShortDateString().Replace("/", "-")}_{DateTime.Now.ToLongTimeString().Replace(":", "-").Replace(" ", "-")}.txt";

            _currentWriter = new StreamWriter(Path.Combine(logsFolder, logsFile));
            _currentWriter.AutoFlush = true;
        }
    }
}
