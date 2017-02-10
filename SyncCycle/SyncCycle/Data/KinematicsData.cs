using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SyncCycle
{

    enum kData { vAvg, vTop, dTraveled, tElapsed }
    class KinematicsData : BikeData
    {
        //Buffer to hold strings displayed on page. Not interacted with directly.
        string display1, display2, display3, display4;

        //Variables to update
        float vAvg, vTop, dTraveled, tElapsed;

        //Bound to DataViewCell.cs//
        public string imgURI { get; set; } = "Icon.png";
        public string Display1
        {
            get
            {
                return display1;
            }
            set
            {
                if (display1 != value)
                {
                    display1 = value;
                    OnPropertyChanged("Display1");
                }
            }
        }
        public string Display2
        {
            get
            {
                return display2;
            }
            set
            {
                if (display2 != value)
                {
                    display2 = value;
                    OnPropertyChanged("Display2");
                }
            }
        }
        public string Display3
        {
            get
            {
                return display3;
            }
            set
            {
                if (display3 != value)
                {
                    display3 = value;
                    OnPropertyChanged("Display3");
                }
            }
        }
        public string Display4
        {
            get
            {
                return display4;
            }
            set
            {
                if (display4 != value)
                {
                    display4 = value;
                    OnPropertyChanged("Display4");
                }
            }
        }

        public void update(kData enumeration, float value)
        {
            switch (enumeration)
            {
                case kData.vAvg:
                    vAvg = value;
                    Display1 = "Average Speed : " + vAvg;
                    break;
                case kData.vTop:
                    vTop = value;
                    Display2 = "Top Speed : " + vTop;
                    break;
                case kData.dTraveled:
                    dTraveled = value;
                    Display3 = "Distance Traveled : " + dTraveled;
                    break;
                case kData.tElapsed:
                    tElapsed = value;
                    Display4 = "Duration of Trip : " + tElapsed;
                    break;

            }

        }

        public KinematicsData() : base(ViewType.Kinematics, "Kinematic Data")
        {
            update(kData.vAvg, 0);
            update(kData.vTop, 0);
            update(kData.dTraveled, 0);
            update(kData.tElapsed, 0);
        }
    }
}
