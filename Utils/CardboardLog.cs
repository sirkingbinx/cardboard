using System;
using System.IO;
using System.Text;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cardboard.Utils
{
    /// <summary>
    /// Class for writing to logs.
    /// </summary>
    public class CardboardLog
    {
        private StreamWriter currentWriter;

        /// <summary>
        /// Log the current line into the log.
        /// </summary>
        public void Log(string text, string ending = "\n") => currentWriter.Write($"{text}{ending}");

        /// <summary>
        /// Log the current line into the log with the logLevel string.
        /// </summary>
        public void Log(string logLevel, string text) => Log($"[{logLevel.ToUpper()} @ {DateTime.Now.ToLongTimeString()}]: {text}");

        /// <summary>
        /// Log an error into the log.
        /// </summary>
        public void LogError(string text) => Log("error", $"{text}");

        /// <summary>
        /// Log a warning into the log.
        /// </summary>
        public void LogWarning(string text) => Log("warning", $"{text}");

        /// <summary>
        /// Represents if the CardboardLog is writable and enabled. If this is false, do not write to the log.
        /// </summary>
        public bool Active => (currentWriter.BaseStream != null);

        /// <summary>
        /// Close the CardboardLog and disable writing.
        /// </summary>
        public void Dispose()
        {
            if (Active) currentWriter.Dispose();
        }

        /// <summary>
        /// Create a new CardboardLog.
        /// </summary>
        /// <param name="uuid">The UUID of your mod.</param>
        /// <param name="logs_folder">The folder that your mod's logs will be collected in. By default, this is (GT)/BepInEx/CardboardLogs/(uuid).</param>
        public CardboardLog(string uuid, string? outputFolder)
        {
            var logsFolder = outputFolder ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BepInEx", "CardboardLogs", uuid);

            if (!Directory.Exists(logsFolder))
                Directory.Create(logsFolder);
            
            var logsFile = $"log_{DateTime.Now.ToShortDateString().Replace("/", "-")}_{DateTime.Now.ToLongTimeString().Replace(":", "-").Replace(" ", "-")}.txt";

            currentWriter = new StreamWriter(logsFile);
            currentWriter.AutoFlush = true;
        }
    }
}
