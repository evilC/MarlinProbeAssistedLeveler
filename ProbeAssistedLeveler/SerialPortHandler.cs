using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProbeAssistedLeveler
{
    public class SerialPortHandler : IDisposable
    {
        private readonly SerialPort _port;
        private readonly AutoResetEvent _waitHandle;
        private volatile List<string> _response;
        private static List<string> _ignoreLines = new List<string>
        {
            string.Empty,
            "processing",
            "echo:busy: processing"
        };

        public SerialPortHandler(string port, int baudRate)
        {
            _waitHandle = new AutoResetEvent(false);
            _port = new SerialPort(port, baudRate);
            _port.DataReceived += OnDataReceived;
            _port.Open();
            _port.ReadExisting(); // clear buffer
        }

        public void Dispose()
        {
            _port.Close();
        }

        public List<string> Send(string command)
        {
            _response = new List<string>();
            _port.WriteLine(command);
            _waitHandle.WaitOne();
            return new List<string>(_response);
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var lines = new List<string>();
            while (_port.BytesToRead > 0)
            {
                var line = _port.ReadLine();
                if (_ignoreLines.Contains(line))
                {
                    continue;
                }
                lines.Add(line);
            }
            _response.AddRange(lines);
            if (_response.Count > 0 && _response.Last() == "ok")
            {
                _response.RemoveAt(_response.Count - 1);
                _waitHandle.Set();
            }

            /*
            var response = _port.ReadExisting();
            var lines = new List<string>();
            var inLines = response.Split('\n').ToList();
            foreach (var inLine in inLines)
            {
                if (_ignoreLines.Contains(inLine))
                {
                    continue;
                }
                lines.Add(inLine);
            }
            _response.AddRange(lines);
            if (_response.Count > 0 && _response.Last() == "ok")
            {
                _response.RemoveAt(_response.Count - 1);
                _waitHandle.Set();
            }
            */
        }
    }
}
