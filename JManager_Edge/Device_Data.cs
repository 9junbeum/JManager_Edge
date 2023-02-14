using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JManager_Edge
{
    internal class Device_Data
    {
        static Device_Data _instance = null; //Singleton Pattern

        public Device[] Devices = new Device[60]; //장치들 담는 배열 선언

        public static Device_Data instance
        {
            get
            {
                if (_instance == null) _instance = new Device_Data();
                return _instance;
            }
        }

        public Device_Data()
        {

        }

        public void save_Device(Device device_to_save)
        {

        }
        public bool ValidateIPv4(string ipString)
        {
            //IP주소 형식이 정확한지 판별하는 함수
            //어디서든 접근가능하도록 전역변수 Device_Data에 저장

            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }

    }
}
