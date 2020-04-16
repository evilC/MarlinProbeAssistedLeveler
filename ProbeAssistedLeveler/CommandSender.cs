using System;
using System.Collections.Generic;
using System.Numerics;
using ProbeAssistedLeveler.ExtensionMethods;

namespace ProbeAssistedLeveler
{
    public class CommandSender
    {
        private readonly SerialPortHandler _serialPortHandler;

        public CommandSender(SerialPortHandler serialPortHandler)
        {
            _serialPortHandler = serialPortHandler;
        }

        public Vector3 AutoHome()
        {
            /*
            SENT: G28
            RECV: echo:busy: processing
            RECV: X:138.00 Y:140.00 Z:12.75 E:-40.00 Count X:11040 Y:11200 Z:5100
            RECV: ok
             */
            var response = _serialPortHandler.Send("G28")[0]
                .Split(new[] {"Count "}, StringSplitOptions.None)[0];
            var homeCoords = response
                .ToDictionary()
                .ToVector3();
            return homeCoords;
        }

        public float GetProbeZOffset()
        {
            /*
            SENT: M851
            RECV: Probe Offset X-31.00 Y-35.00 Z-2.75
            RECV: ok
            */
            var response = _serialPortHandler.Send("M851");
            var coordStr = response[0].Split(new[] {"Probe Offset "}, StringSplitOptions.RemoveEmptyEntries)[0];
            var coords = coordStr
                .SplitXyz()
                .ToVector3();
            return coords.Z;
        }

        public void Move(MoveMode moveMode, float? x = null, float? y = null, float? z = null, int? speed = null, bool waitForFinish = true)
        {
            /*
            SENT: G90 // Absolute
            SENT: G91 // Relative
            RECV: ok
            SENT: G0 X1 Y1 Z1
            RECV: ok
            */
            _serialPortHandler.Send(moveMode == MoveMode.Absolute ? "G90" : "G91");
            var parameters = BuildOptionalXyz(x, y, z, speed);
            var command = $"G0 {parameters}";
            _serialPortHandler.Send(command);
            if (waitForFinish) _serialPortHandler.Send("M400");
        }

        public void EmergencyStop()
        {
            // No "ok" response :(
            _serialPortHandler.Send("M112");
        }

        private string BuildOptionalXyz(float? x = null, float? y = null, float? z = null, int? speed = null)
        {
            var parameters = new List<string>();
            if (x != null) parameters.Add($"X{x}");
            if (y != null) parameters.Add($"Y{y}");
            if (z != null) parameters.Add($"Z{z}");
            if (speed != null) parameters.Add($"F{speed}");
            return BuildCommandParts(parameters);
        }

        public Vector3 GetCurrentPosition()
        {
            /*
            SENT: M114
            RECV: X:138.00 Y:140.00 Z:12.75 E:0.00 Count X:11040 Y:11200 Z:5100
            RECV: ok
            */
            var response = _serialPortHandler.Send("M114");
            var coordStr = response[0].Split(new[] {"Count "}, StringSplitOptions.RemoveEmptyEntries)[0];
            var coords = coordStr
                .ToDictionary()
                .ToVector3();
            return coords;
        }

        public void DeployProbe()
        {
            /*
            SENT: M280 P0 S10
            RECV: ok
            */
            _serialPortHandler.Send("M280 P0 S10");
        }

        public void ResetProbeAlarm()
        {
            /*
            SENT: M280 P0 S160
            RECV: ok
            */
            _serialPortHandler.Send("M280 P0 S160");
        }

        public void RetractProbe()
        {
            /*
            SENT: M280 P0 S90
            RECV: ok
            */
            _serialPortHandler.Send("M280 P0 S90");
        }

        public float DoSingleProbe()
        {
            /*
            SENT: G30 S1
            RECV: echo:busy: processing
            RECV: Bed X: 107.00 Y: 105.00 Z: 0.48
            RECV: X:138.00 Y:140.00 Z:12.75 E:-40.00 Count X:11040 Y:11200 Z:5100
            RECV: ok
            */
            var responses = _serialPortHandler.Send("G30 S1");
            if (responses[0] == "Error:Probing Failed") throw new Exception("Probing failed");
            var probedZ = responses[0]
                .Substring(4)
                .ToDictionary()
                .ToVector3()
                .Z;
            return probedZ;
        }

        public bool IsProbeTriggered()
        {
            /*
            https://marlinfw.org/docs/gcode/M119.html
            SENT: M119
            RECV: Reporting endstop status
            RECV: x_min: open
            RECV: y_min: open
            RECV: z_min: TRIGGERED
            RECV: ok
            */
            var response = _serialPortHandler.Send("M119");
            response.RemoveAt(0);
            var dict = response.ToDictionary();
            if (!dict.TryGetValue("z_min", out var zMin))
                throw new Exception($"Response does not contain z_min value");
            return zMin == "TRIGGERED";
        }

        public List<string> ReportSettings()
        {
            return _serialPortHandler.Send("M503");
        }

        private static string BuildCommand(string command, List<string> paramList)
        {
            var commandStr = $"{command} {BuildCommandParts(paramList)}";
            return commandStr;
        }
        private static string BuildCommandParts(List<string> paramList)
        {
            var commandStr = string.Empty;
            var firstPart = true;

            foreach (var parameter in paramList)
            {
                if (!firstPart) commandStr += " ";
                commandStr += parameter;
                firstPart = false;
            }

            return commandStr;
        }
    }
}
