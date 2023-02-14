using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace JManager_Edge
{
    internal class Device
    {
        public string IP;  //IP주소
        public string Name;//사용자 지정 이름
        public int Kind;   //장치 종류 0 - 스피커 / 1 - 카메라 / 2 - 기타 / 3 - 등록안됨

        private string ID;
        private string PW;

        public Device(string ip, string name, int kind, string id, string pw)
        {
            //생성자
            try
            {
                this.IP = ip;
                this.Name = name;
                this.Kind = kind;
                this.ID = id;
                this.PW = pw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
