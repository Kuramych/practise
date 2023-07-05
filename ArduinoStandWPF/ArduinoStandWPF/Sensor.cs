using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoStandWPF
{
    public class Sensor
    {
        private string name;
        private string iconPath;
        public Dictionary<string, Port> ports = new Dictionary<string, Port>();

        public Sensor(string name, string iconPath)
        {
            this.name = name;
            this.iconPath = iconPath;
        }

        public void AddPort(string portName, Port p)
        {
            ports.Add(portName, p);
        }

        public string GetName()
        {
            return name;
        }

        public void SetPortCoordX(string portName, int X)
        {
            ports[portName].SetCoordX(X);
        }

        public void SetPortCoordY(string portName, int Y)
        {
            ports[portName].SetCoordY(Y);
        }

        public string GetIconPath()
        {
            return iconPath;
        }

        public Port GetPort(string name)
        {
            if (ports.ContainsKey(name))
                return ports[name];
            else
                return null;
        }

    }
}
