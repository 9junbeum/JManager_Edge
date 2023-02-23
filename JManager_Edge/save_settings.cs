using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;
using System.Net;

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

        //저장할 데이터
        List<string> save_ip = new List<string>();
        List<string> save_name = new List<string>();
        List<int> save_kind = new List<int>();
        List<string> save_id = new List<string>();
        List<string> save_pw = new List<string>();


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
            List<Device> devices = deviceData.Devices;

            try
            {
                save_ip.Clear();
                save_name.Clear();
                save_kind.Clear();
                save_id.Clear();
                save_pw.Clear();

                for (int i = 0; i < devices.Count; i++)
                {
                    save_ip.Add(devices[i].IP);
                    save_name.Add(devices[i].Name);
                    save_kind.Add(devices[i].Kind);
                    save_id.Add(devices[i].ID);
                    save_pw.Add(devices[i].PW);
                }
                JObject newjobj = new JObject(
                    new JProperty("obj_ip", save_ip),
                    new JProperty("obj_name", save_name),
                    new JProperty("obj_kind", save_kind),
                    new JProperty("obj_id", save_id),
                    new JProperty("obj_pw", save_pw)
                    );
                File.WriteAllText(save_path, newjobj.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show(ex.Message);
            }
        }

        public void LOAD_devices()
        {
            try
            {
                string json = File.ReadAllText(save_path);
                JObject jobj = JObject.Parse(json);
                List<Device> devices = deviceData.Devices;

                for (int i = 0; i < devices.Count; i++)
                {
                    this.deviceData.Devices[i].IP = jobj["obj_ip"][i].ToString();
                    this.deviceData.Devices[i].Name = jobj["obj_name"][i].ToString();
                    this.deviceData.Devices[i].Kind = int.Parse(jobj["obj_kind"][i].ToString());
                    this.deviceData.Devices[i].ID = jobj["obj_id"][i].ToString();
                    this.deviceData.Devices[i].PW = jobj["obj_pw"][i].ToString();

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
