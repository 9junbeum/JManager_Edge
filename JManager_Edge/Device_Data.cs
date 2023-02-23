using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace JManager_Edge
{
    internal class Device_Data
    {
        static Device_Data _instance = null; //Singleton Pattern

        public List<Device> Devices = new List<Device>(); //장치들 담는 배열 선언
        public string[] D_GrData = new string[15]; //장치 관리하는 그룹 데이터 변수(15칸) 설정.

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

        public bool ValidateIPv4(string ipString)
        {
            //IP주소 형식이 정확한지 판별하는 함수
            //어디서든 접근가능하도록 전역변수 Device_Data에 저장

            if (string.IsNullOrWhiteSpace(ipString))
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


        public string Send_Url(string url, int arr_num)
        {

            string responseText = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UseDefaultCredentials = true;
            request.PreAuthenticate = true;
            
            System.Net.NetworkCredential netCredential = new System.Net.NetworkCredential(Devices[arr_num].ID, Devices[arr_num].PW, Devices[arr_num].IP);
            request.Credentials = netCredential;

            using (HttpWebResponse resp = (HttpWebResponse)request.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;
                if (status == HttpStatusCode.OK)
                {
                    Stream respStream = resp.GetResponseStream();
                    using (StreamReader sr = new StreamReader(respStream))
                    {
                        responseText = sr.ReadToEnd();
                        return responseText;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public bool is_exist(string ip)
        {
            for (int i = 0; i < Devices.Count; i++)
            {
                if (Devices[i] != null)
                {
                    if (Devices[i].IP == ip)
                    {
                        return true;
                        //같은게 이미 등록되어있다는 의미
                    }
                }
            }
            return false;
        }
    }
}
