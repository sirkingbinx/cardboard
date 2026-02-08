using System;
using System.IO;

namespace Cardboard.Utils
{
    /// <summary>
    /// Class for writing to logs.
    /// </summary>
    public class CardboardLog
    {
        private readonly StreamWriter _currentWriter;

        /// <summary>
        /// Log the current line into the log.
        /// </summary>
        public void Log(string text, string ending = "\n") => _currentWriter.Write($"{text}{ending}");

        /// <summary>
        /// Log the current line into the log with the logLevel string.
        /// </summary>
        public void Log(string text, LogLevel logLevel) => Log($"[{logLevel} | {DateTime.Now.ToLongTimeString()}]: {text}");

        /// <summary>
        /// Log an error into the log.
        /// </summary>
        public void LogError(string text) => Log("error", $"{text}");

        /// <summary>
        /// Log a warning into the log.
        /// </summary>
        public void LogWarning(string text) => Log("warning", $"{text}");

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
        /// <param name="outputFolder">The folder that your mod's logs will be collected in. By default, this is (GT)/BepInEx/CardboardLogs/(uuid) which will be used if outputFolder = "BepInEx".</param>
        public CardboardLog(string uuid, string outputFolder = "BepInEx")
        {
            var logsFolder = outputFolder == "BepInEx" ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx", "CardboardLogs", uuid) : outputFolder;

            if (!Directory.Exists(logsFolder))
                Directory.CreateDirectory(logsFolder);
            
            var logsFile = $"log_{DateTime.Now.ToShortDateString().Replace("/", "-")}_{DateTime.Now.ToLongTimeString().Replace(":", "-").Replace(" ", "-")}.txt";

            _currentWriter = new StreamWriter(logsFile);
            _currentWriter.AutoFlush = true;
        }
    }
}
