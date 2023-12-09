// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

using System;
using System.IO;
using System.Diagnostics;
using UnityEngine;

namespace Tests
{
    public sealed class TemporaryFile : IDisposable
    {
        public TemporaryFile() :
          this(Path.GetTempPath())
        { }

        public TemporaryFile(string directory)
        {
            Create(Path.Combine(directory, Path.GetRandomFileName()));
        }

        ~TemporaryFile()
        {
            Delete();
        }

        public void Dispose()
        {
            Delete();
            GC.SuppressFinalize(this);
        }

        public string FilePath { get; private set; }

        private void Create(string path)
        {
            FilePath = path;
            using (File.Create(FilePath)) { };
        }

        private void Delete()
        {
            if (FilePath == null) return;
            File.Delete(FilePath);
            FilePath = null;
        }
    }

    public class Protoscope
    {
        public static byte[] Run(string protoscopeProgram)
        {
            using (var tempFile = new TemporaryFile())
            {
                // Add the protoscope spec to the temp file
                using (StreamWriter outputFile = new StreamWriter(tempFile.FilePath))
                    outputFile.Write(protoscopeProgram);

                // Run protoscope on the temporary file
                return RunFile(tempFile.FilePath);
            }
        }

        public static byte[] RunFile(string fileName)
        {
            Process cmdProcess = new Process();
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo();
            cmdStartInfo.FileName = "protoscope";

            cmdStartInfo.RedirectStandardError = true;
            cmdStartInfo.RedirectStandardOutput = true;
            cmdStartInfo.RedirectStandardInput = false;
            cmdStartInfo.UseShellExecute = false;
            cmdStartInfo.CreateNoWindow = true;

            cmdStartInfo.Arguments = $"-s {fileName}";

            cmdProcess.EnableRaisingEvents = true;
            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.Start();

            // Prepare to read each alignment (binary)
            var ms = new MemoryStream();
            cmdProcess.StandardOutput.BaseStream.CopyTo(ms);
            return ms.ToArray();
        }
    }
}