using System;
using System.Diagnostics;

namespace Assistant.Engine.Executor;

class Terminal
{
    public static void execCmd(string cmd, string[] args) {
        Process process = new Process();

        process.StartInfo.FileName = "cmd.exe";
        process.StartInfo.Arguments = "cd";
        process.StartInfo.UseShellExecute = false; // 不使用操作系统shell启动
        process.StartInfo.RedirectStandardOutput = true; // 重定向输出流
        process.StartInfo.RedirectStandardError = true;  // 重定向错误流
        process.StartInfo.CreateNoWindow = true; // 不显示新的窗口
    }
}