using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace SyncCycle
{

    /// <summary>
    /// Base class for specialized EnergyData, GreenData, and KinematicsData
    /// </summary>
    
    class BikeData : INotifyPropertyChanged
    {
        public ViewType dataType;
        public string title;
        
        public BikeData(ViewType type, string title)
        {
            this.title = title;
            dataType = type;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}
