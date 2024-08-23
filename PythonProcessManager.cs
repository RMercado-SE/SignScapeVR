using UnityEngine;
using System.Diagnostics;

public static class PythonProcessManager
{
    private static Process pythonProcess;

    public static void StartPythonScript()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo("python", "\"E:\\VR-BSL\\VR-BSL\\HandReader.py\"")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true
        };

        pythonProcess = Process.Start(startInfo);
    }

    public static void StopPythonScript()
    {
        if (pythonProcess != null && !pythonProcess.HasExited)
        {
            pythonProcess.Kill();
            pythonProcess.Dispose();
            pythonProcess = null;
        }
    }
}
