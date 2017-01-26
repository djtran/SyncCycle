using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace SyncCycle
{
    //Dummy data generated to be displayed on data page
    class BikeData : INotifyPropertyChanged
    {
        //Buffer to hold strings to be displayed on the page
        //not interacted with directly
        string display1 = "In honor of Francis the dog";
        string display2 = "woof";
        string display3 = "woof woof";

        public string imgURI { get; set; } = "Icon.png";
        public string Display1
        {
            get
            {
                return display1;
            }
            set
            {
                if(display1 != value)
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
                if(display3 != value)
                {
                    display3 = value;
                    OnPropertyChanged("Display3");
                }
            }
        }

        //Actual variables to write to for specific metrics
        public float metric1 = 0;
        public float metric2 = 0;
        public float metric3 = 0;
        public Timer tmr;

        //Update the viewcell bound to this data object
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propName)
        {
            var changed = PropertyChanged;
            if(changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


        public BikeData()
        {
            //tmr = new Timer(new TimerCallback(updateData), this, 1000, 1000);
        }

        /// <summary>
        /// Dummy methods below for debug purposes.
        /// </summary>
        void constructDisplayStrings()
        {
            Display1 = "Speed : " + metric1;
            Display2 = "Emissions saved : " + metric2;
            Display3 = "Some other metrics, roadkill avoided i guess : " + metric3;
        }

        void updateData(Object state)
        {
            metric1++;
            metric2 += .05f;
            metric3 += 2f;

            Console.WriteLine("updating state : {0} {1} {2}", metric1, metric2, metric3);

            constructDisplayStrings();
        }
    }
}
