using System;
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
        /// <summary>
        /// Create a new CardboardLog.
        /// </summary>
        public static CardboardLog Create(string modName)
        {
            Debug.Log(modName);
        }

        /// <summary>
        /// Log text into the log.
        /// </summary>
        public void Log(string text)
        {
            // placeholder for now
            Debug.Log(text);
        }

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
    }
}
