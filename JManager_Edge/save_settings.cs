using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;

namespace JManager_Edge
{
    internal class save_settings
    {
        //세이브 파일 저장할 경로
        string save_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\JManager_Edge"; //기본 저장 경로
        //Json 객체 생성
        JObject save_obj = new JObject();
        //Device_Data
        Device_Data deviceData = Device_Data.instance;


        public bool is_savefile_exist()
        {
            //세이브 파일이 존재하는지 확인 
            if (File.Exists(save_path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void set_path(string path)
        {
            save_path = path;
        }

        public void SAVE_devices()
        {
            Device[] devices = deviceData.Devices;

            for(int i = 0; i < devices.Length ;i++)
            {
                save_obj.Add(devices[i]);
            }

            File.WriteAllText(save_path, save_obj.ToString());

        }



    }
}
