using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using BluetoothLE.Core;
using BluetoothLE.Core.Events;
using System.Collections;

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
    /// 
    /// </summary>

    public class App : Application
	{
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
            SettingsPage page = new SettingsPage();
            //delegate instance method to the event
            _bluetoothAdapter.DeviceDiscovered += page.b.DeviceDiscovered;

            ArrayList pages = new ArrayList();
            pages.Add(new NavigationPage());
            pages.Add(new DataPage(""));
            pages.Add(page);
            
            // The root page of your application
            MainPage = new TabbedPageWrapper(pages);
        }

       

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps

		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
