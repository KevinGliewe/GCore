using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace GCore.Extensions.StringShEx {
    public static class StringShExtensions {

        public class ReturnCodeException : Exception {
            public int ReturnCode { get; private set; }

            public string Command { get; private set; }

            public string Output { get; private set; }

            public ReturnCodeException(int returnCode, string command, string output) {
                ReturnCode = returnCode;
                Command = command;
                Output = output;
            }
        }

        public static string Sh(this string cmd, string workingDirectory = ".", bool redirectStandardError = true, bool throwOnError = false) {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var fileName = "/bin/bash";
            var arguments = $"-c \"{escapedArgs}\"";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                fileName = "cmd.exe";
                arguments = $"/C \"{escapedArgs}\"";
            }

            var process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = redirectStandardError,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
            var stdOut = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if(throwOnError && process.ExitCode != 0) {
                throw new ReturnCodeException(process.ExitCode, cmd, stdOut);
            }

            return stdOut;
        }

        public static void Sh2(this string cmd, out Process process, string workingDirectory = ".", bool redirectStandardError = true) {
            var escapedArgs = cmd.Replace("\"", "\\\"");

            var fileName = "/bin/bash";
            var arguments = $"-c \"{escapedArgs}\"";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                fileName = "cmd.exe";
                arguments = $"/C \"{escapedArgs}\"";
            }

            process = new Process() {
                StartInfo = new ProcessStartInfo {
                    FileName = fileName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = redirectStandardError,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
        }



        public static int Sh2(this string cmd, out string stdOut, string workingDirectory = ".", bool redirectStandardError = true) {
            Process process;
            cmd.Sh2(out process, workingDirectory, redirectStandardError);
            stdOut = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return process.ExitCode;
        }

        public static int Sh2(this string cmd, Action<string> lineCallback = null, string workingDirectory = ".", bool redirectStandardError = true) {
            Process process;
            cmd.Sh2(out process, workingDirectory, redirectStandardError);
            string line;
            if (lineCallback != null)
                while ((line = process.StandardOutput.ReadLine()) != null)
                    lineCallback(line);
            process.WaitForExit();
            return process.ExitCode;
        }

        public static int Sh2(this string cmd, string workingDirectory = ".", bool redirectStandardError = true) {
            Process process;
            cmd.Sh2(out process, workingDirectory, redirectStandardError);
            string line;
            while ((line = process.StandardOutput.ReadLine()) != null)
                GCore.Logging.Log.Info($"Process {process.Id}: {line}");
            process.WaitForExit();
            return process.ExitCode;
        }


        public static Version ExtractVersion(this string self) {
            var match = Regex.Match(self, @"^.*(\d+)\.(\d+)\.(\d+).*$");
            if (match != null)
                return new Version(
                    int.Parse(match.Groups[1].Value),
                    int.Parse(match.Groups[2].Value),
                    int.Parse(match.Groups[3].Value));
            throw new Exception($"String '{self}' does not contain a version number");
        }
    }
}