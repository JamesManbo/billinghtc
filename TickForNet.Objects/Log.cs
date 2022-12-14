using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TikForNet.Objects
{
    /// <summary>
    /// /log
    /// </summary>
    [TikEntity("/log", IsReadOnly = true)]
    public class Log
    {
        /// <summary>
        /// Row .id property.
        /// </summary>
        [TikProperty(".id", IsReadOnly = true, IsMandatory = true)]
        public string Id { get; private set; }

        /// <summary>
        /// Row message property.
        /// </summary>
        [TikProperty("message", IsReadOnly = true, IsMandatory = true)]
        public string Message { get; private set; }

        /// <summary>
        /// Row time property.
        /// </summary>
        [TikProperty("time", IsReadOnly = true, IsMandatory = true)]
        public string Time { get; private set; }

        /// <summary>
        /// Row topics property.
        /// </summary>
        [TikProperty("topics", IsReadOnly = true, IsMandatory = true)]
        public string Topics { get; private set; }

        #region -- static methods --

        /// <summary>
        /// Writes debug message into mikrotik log.
        /// </summary>
        public static void Debug(ITikConnection connection, string message)
        {
            LogConnectionExtensions.LogDebug(connection, message);
        }

        /// <summary>
        /// Writes info message into mikrotik log.
        /// </summary>
        public static void Info(ITikConnection connection, string message)
        {
            LogConnectionExtensions.LogInfo(connection, message);
        }

        /// <summary>
        /// Writes warning message into mikrotik log.
        /// </summary>
        public static void Warning(ITikConnection connection, string message)
        {
            LogConnectionExtensions.LogWarning(connection, message);
        }

        /// <summary>
        /// Writes error message into mikrotik log.
        /// </summary>
        public static void WriteErrorMessage(ITikConnection connection, string message)
        {
            LogConnectionExtensions.LogError(connection, message);
        }
        #endregion
    }

    /// <summary>
    /// Connection extension class for <see cref="Log"/>
    /// </summary>
    public static class LogConnectionExtensions
    {
        private static void WriteToLog(ITikConnection connection, string message, string logLevelCommandSufix)
        {
            var cmd = connection.CreateCommand("/log/" + logLevelCommandSufix,
                connection.CreateParameter("message", message));
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Writes debug message into mikrotik log.
        /// </summary>
        public static void LogDebug(this ITikConnection connection, string message)
        {
            WriteToLog(connection, message, "debug");
        }

        /// <summary>
        /// Writes info message into mikrotik log.
        /// </summary>
        public static void LogInfo(this ITikConnection connection, string message)
        {
            WriteToLog(connection, message, "info");
        }

        /// <summary>
        /// Writes warning message into mikrotik log.
        /// </summary>
        public static void LogWarning(this ITikConnection connection, string message)
        {
            WriteToLog(connection, message, "warning");
        }

        /// <summary>
        /// Writes error message into mikrotik log.
        /// </summary>
        public static void LogError(this ITikConnection connection, string message)
        {
            WriteToLog(connection, message, "error");
        }
    }
}
