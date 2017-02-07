using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SyncCycle
{
    enum co2Data { Prevented };
    class GreenData : BikeData
    {
        //Buffer to hold strings displayed on page. Not interacted with directly.
        string display1;

        //Variables to update
        float co2Prevented;

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
        

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void update(co2Data enumeration, float value)
        {
            switch (enumeration)
            {
                case co2Data.Prevented:
                    co2Prevented = value;
                    Display1 = "Emissions Prevented : " + co2Prevented;
                    break;
            }

        }

        public GreenData() : base(ViewType.Green, "Environmental Data")
        {
            update(co2Data.Prevented, 0);
        }
    }
}
