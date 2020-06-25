using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BikeShop
{
    public class Car : Notifier
    {
        private double speed;
        private Color color;
        public double Speed 
        { 
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                OnPropertyChanged("Speed"); // 속성값이 변경되는 것을 클라이언트 시스템에 통보해줌
            }
        }
        public Color Color 
        {
            get 
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        public Human Driver 
        { get; set; }
    }
    public class Human
    {
        public string FirstName { get; set; }
        public Boolean HasDrivingLicense { get; set; }
        
    }
}
