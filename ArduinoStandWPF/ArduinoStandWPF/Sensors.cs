using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace ArduinoStandWPF
{
    public class Sensors
    {
        public Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();

        public Sensors()
        {
            string[] files = Directory.GetDirectories(@"..\..\..\Sensors");

            foreach (string file in files)
            {
                string[] res = Directory.GetFiles(file);
                string sensName = file.Substring(file.LastIndexOf('\\') + 1);
                string iconPath = file + "\\" + sensName + ".png";
                string line;
                if (res.Length == 3 && res.Contains(file + "\\" + sensName + ".png") &&
                    res.Contains(file + "\\" + sensName + "_ports.txt") &&
                    res.Contains(file + "\\" + sensName + "_ports_render.txt"))
                {
                    Sensor s = new Sensor(sensName, iconPath);
                    StreamReader sr;

                    sr = new StreamReader(file + "\\" + sensName + "_ports.txt");
                    line = sr.ReadLine();

                    while (line != null)
                    {
                        string[] port = line.Split(' ');
                        Trace.WriteLine(line);
                        Port p = new Port(sensName + "_" + port[0], port[1], port[2]);

                        s.AddPort(sensName + "_" + port[0], p);
                        line = sr.ReadLine();
                    }
                    sensors.Add(sensName, s);
                    sr.Close();


                    sr = new StreamReader(file + "\\" + sensName + "_ports_render.txt");
                    line = sr.ReadLine();
                    while (line != null)
                    {
                        string[] port = line.Split(' ');
                        sensors[sensName].SetPortCoordX(sensName + "_" + port[0], Convert.ToInt32(port[1]));
                        sensors[sensName].SetPortCoordY(sensName + "_" + port[0], Convert.ToInt32(port[2]));
                        line = sr.ReadLine();
                    }
                    sr.Close();
                }
            }
        }

        public Port GetPort(string name)
        {
            string[] words = name.Split('_');
            if(words.Length == 2 && sensors.ContainsKey(words[0]))
            {
                Port p = sensors[words[0]].GetPort(name);
                if (p != null)
                    return p;
            }

            return null;
        }
    }


}
