using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace ArduinoStandWPF
{
    public class Arduino
    {
        private string name;
        private string iconPath;
        public Dictionary<string, Port> ports = new Dictionary<string, Port>();
        
        public Arduino()
        {
            string arduinoPath = @"..\..\..\Arduino";
            string[] res = Directory.GetFiles(arduinoPath);
            string line;

            if (res.Length == 3 && res.Contains(arduinoPath + "\\Arduino_UNO.png") &&
                res.Contains(arduinoPath + "\\Arduino_UNO_ports.txt") &&
                res.Contains(arduinoPath + "\\Arduino_UNO_ports_render.txt"))
            {
                name = "Arduino_UNO";
                iconPath = arduinoPath + "\\Arduino_UNO.png";

                StreamReader sr;

                sr = new StreamReader(arduinoPath + "\\Arduino_UNO_ports.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] port = line.Split(' ');
                    Port p = new Port("Arduino_UNO_" + port[0], port[1]);
                    ports.Add("Arduino_UNO_" + port[0], p);
                    line = sr.ReadLine();
                }
                sr.Close();

                sr = new StreamReader(arduinoPath + "\\Arduino_UNO_ports_render.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] port = line.Split(' ');
                    ports["Arduino_UNO_" + port[0]].SetCoordX(Convert.ToInt32(port[1]));
                    ports["Arduino_UNO_" + port[0]].SetCoordY(Convert.ToInt32(port[2]));
                    line = sr.ReadLine();
                }
                sr.Close();
            }
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
