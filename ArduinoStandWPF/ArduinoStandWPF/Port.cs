using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArduinoStandWPF
{
    public class Port
    {
        private string name;
        private string address;
        private List<string> nextPort;
        private string portType;
        private int coordX;
        private int coordY;

        public Port()
        {
            name = "";
            address = "";
            nextPort = new List<string>();
            portType = "";
            coordX = -1;
            coordY = -1;
        }

        ~Port()
        {
        }

        public Port(string valueName, string valueAddress)
        {
            name = valueName;
            address = valueAddress;
            nextPort = new List<string>();
            portType = "";
            coordX = -1;
            coordY = -1;
        }

        public Port(string valueName, string valuePortType, string valueAddress)
        {
            name = valueName;
            address = valueAddress;
            nextPort = new List<string>();
            portType = valuePortType;
            coordX = -1;
            coordY = -1;
        }

        public string GetName()
        {
            return name;
        }
        public string GetAddress()
        {
            return address;
        }
        public List<string> GetNextPort()
        {
            return nextPort;
        }
        public string GetPortType()
        {
            return portType;
        }
        public int GetCoordX()
        {
            return coordX;
        }
        public int GetCoordY()
        {
            return coordY;
        }
        public void SetName(string name)
        {
            this.name = name;
        }
        public void SetAddress(string address)
        {
            this.address = address;
        }

        public void SetNextPort(List<string> nextPort)
        {
            this.nextPort = nextPort;
        }

        public void SetType(string portType)
        {
            this.portType = portType;
        }
        public void SetCoordX(int coordX)
        {
            this.coordX = coordX;
        }
        public void SetCoordY(int coordY)
        {
            this.coordY = coordY;
        }
    }
}
