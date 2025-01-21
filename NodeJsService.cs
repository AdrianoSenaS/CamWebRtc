using System.Diagnostics;

namespace CamWebRtc
{
    public class NodeJsService
    {
        private Process? _nodeProcess;

        public void StartNodeServer(string nodeScriptPath)
        {
            if (_nodeProcess != null && !_nodeProcess.HasExited)
                return; // O servidor já está rodando

            var startInfo = new ProcessStartInfo
            {
                FileName = "node", // Comando Node.js
                Arguments = nodeScriptPath, // Caminho para o arquivo server.js
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            _nodeProcess = new Process { StartInfo = startInfo };
            _nodeProcess.OutputDataReceived += (sender, args) => Console.WriteLine(args.Data);
            _nodeProcess.ErrorDataReceived += (sender, args) => Console.WriteLine("Error: " + args.Data);
            _nodeProcess.Start();
            _nodeProcess.BeginOutputReadLine();
            _nodeProcess.BeginErrorReadLine();

            Console.WriteLine("Node.js server started.");
        }

        public void StopNodeServer()
        {
            if (_nodeProcess != null && !_nodeProcess.HasExited)
            {
                _nodeProcess.Kill(); // Finaliza o processo Node.js
                _nodeProcess.Dispose();
                _nodeProcess = null;
                Console.WriteLine("Node.js server stopped.");
            }
        }
    }
}
