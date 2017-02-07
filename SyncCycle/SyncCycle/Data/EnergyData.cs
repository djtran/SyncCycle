using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SyncCycle
{
    enum eData { Used, Equiv, Save};

    class EnergyData : BikeData
    {
        //Buffer to hold strings displayed on page. Not interacted with directly.
        string display1, display2, display3;

        //Variables to update
        float eUsed, eEquiv, eSave;

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


        public void update(eData enumeration, float value)
        {
            switch(enumeration)
            {
                case eData.Used:
                    eUsed = value;
                    Display1 = "Used : " + eUsed;
                    break;
                case eData.Equiv:
                    eEquiv = value;
                    Display2 = "Car Equivalent : " + eEquiv;
                    break;
                case eData.Save:
                    eSave = value;
                    Display3 = "Saved : " + eSave;
                    break;

            }
        
        }

        public EnergyData() : base (ViewType.Energy, "Energy Data")
        {
            update(eData.Used, 0);
            update(eData.Save, 0);
            update(eData.Equiv, 0);
        }
    }
}
