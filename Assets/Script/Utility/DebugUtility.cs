using System;
using System.IO;
using System.Text;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using UnityEngine;
public static class DebugUtility
{
    const string LogFileName = "DebugUtilityLog.txt";
    private static string m_logFilePath;
    private static string m_folderPath;

    private static readonly StringBuilder m_sb = new StringBuilder();
    private static readonly ConcurrentQueue<string> m_logQueue = new ConcurrentQueue<string>();
    private static bool m_isWriting = false;
    public static void SpecifyLogFolderPath(string folderPath)
    {
        m_folderPath = folderPath;
        SetUpFilePath();
    }

    private static void SetUpFilePath()
    {
        string directory = string.IsNullOrWhiteSpace(m_folderPath)
            ? Application.persistentDataPath
            : m_folderPath;

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        m_logFilePath = Path.Combine(directory, LogFileName);
    }
    public static void PrintLogMessage(string message, Action callBack = null)
    {
        if (string.IsNullOrEmpty(m_logFilePath)) 
            SetUpFilePath();

        string logEntry;

        lock (m_sb)
        {
            m_sb.Clear();
            m_sb.Append(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                .Append(": ").Append(message).Append(Environment.NewLine);
            logEntry = m_sb.ToString();
        }

        m_logQueue.Enqueue(logEntry);
        TriggerWrite();

        callBack?.Invoke();
    }

    private static void TriggerWrite()
    {
        if (m_isWriting) 
            return;

        lock (m_logQueue)
        {
            if (m_isWriting) 
                return;
            m_isWriting = true;
            WriteLogsToFileAsync();
        }
    }
    private static async void WriteLogsToFileAsync()
    {
        m_isWriting = true;

        try
        {
            await Task.Run(() =>
            {
                using (FileStream sourceStream = new FileStream(m_logFilePath,
                    FileMode.Append, FileAccess.Write, FileShare.ReadWrite,
                    bufferSize: 4096, useAsync: true))
                {
                    using (StreamWriter writer = new StreamWriter(sourceStream, Encoding.UTF8))
                    {
                        while (m_logQueue.TryDequeue(out string message))
                        {
                            writer.Write(message);
                        }
                    }
                }
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"Log Writing Error: {ex.Message}");
        }
        finally
        {
            m_isWriting = false;
            if (!m_logQueue.IsEmpty) 
                TriggerWrite();
        }
    }

    public static void ClearLogFile()
    {
        if (string.IsNullOrEmpty(m_logFilePath)) 
            SetUpFilePath();
        try
        {
            if (File.Exists(m_logFilePath)) 
                File.WriteAllText(m_logFilePath, string.Empty);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Clear Log Error: {ex.Message}");
        }
    }
}
