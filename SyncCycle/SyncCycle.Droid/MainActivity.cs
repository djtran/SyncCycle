using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Plugin.Permissions;

namespace SyncCycle.Droid
{
	[Activity (MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
	{

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

            Xamarin.Forms.DependencyService.Register<BluetoothLE.Core.IAdapter, BluetoothLE.Droid.Adapter>();
            global::Xamarin.Forms.Forms.Init (this, bundle);
            OxyPlot.Xamarin.Forms.Platform.Android.PlotViewRenderer.Init();
            global::Xamarin.Forms.Forms.Init(this, bundle);
            global::Xamarin.FormsMaps.Init(this, bundle);

            LoadApplication (new SyncCycle.App ());
		}
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}

