using BluetoothLE.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using BluetoothLE.Core.Events;

namespace SyncCycle
{
    /// <summary>
    ///// ******* OVERVIEW *******
    /////
    ///// NuGet library         -   Purpose
    ///
    ///// Xamarin.Forms.Maps    -   Cross-platform maps using native APIs
    ///// Xamarin.BluetoothLE   -   Bluetooth LE Connectivity and communications.
    ///// OxyPlot               -   Data Visualization - Charts and Plots
    /// 
    /// App.cs      -       Purely for visuals. No logic should be done here.
    /// 
    /// </summary>

    public class App : Application
	{
        private DataPage defaultPage;
        private static readonly IAdapter _bluetoothAdapter;
        public static IAdapter BluetoothAdapter { get { return _bluetoothAdapter; } }

        static App()
        {
            _bluetoothAdapter = DependencyService.Get<IAdapter>();

            _bluetoothAdapter.ScanTimeout = TimeSpan.FromSeconds(10);
            _bluetoothAdapter.ConnectionTimeout = TimeSpan.FromSeconds(10);
        }

        public App ()
		{

            List<BikeData> dummy = new List<BikeData>();
            dummy.Add(new BikeData());

            defaultPage = new DataPage(dummy);

            //delegate instance method to the event
            _bluetoothAdapter.DeviceDiscovered += defaultPage.DeviceDiscovered;
            
            // The root page of your application
            MainPage = defaultPage;




        }

		protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
            foreach(BikeData dataCell in defaultPage.Data)
            {
                dataCell.tmr.Dispose();
            }
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
