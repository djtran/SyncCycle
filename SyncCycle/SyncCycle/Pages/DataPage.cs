using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Xamarin.Forms;
using OxyPlot.Xamarin.Forms;
using System.ComponentModel;
using Akavache;
using System.Timers;

namespace SyncCycle
{
    class DataPage : ContentPage
    {      
        
        public DataHandler data;
        public string id;
        Timer checkRide;
        Timer subscribeTimer;
        //private PlotView plot;

        StackLayout container = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Spacing = 5
        };

        public DataPage(string rideID)
        {
            NavigationPage.SetHasNavigationBar(this, false);
            Title = "Data";
            BackgroundColor = Color.FromRgb(29,17,96);
            Padding = 10;

            //BlobCache.LocalMachine.GetObject<DataHandler>(rideID).Subscribe(x => data = x, ex => data = new DataHandler(rideID));
            
            id = rideID;
            if(data == null)
            {
                data = new DataHandler(rideID);
            }
            //Create data views
            createTable();

            var debugButton = new Button()
            {
                Text = "'Receive' some dummy data",
            };
            debugButton.Clicked += debugData;

            var returnButton = new Button()
            {
                Text = "Return to list"
            };
            returnButton.Clicked += (s, e) =>
            {
                this.Navigation.PopAsync();
            };

            //plot = data.getPlot(DataHandler.Sensor.speedometer);



            container.Children.Add(debugButton);
            container.Children.Add(returnButton);
            //container.Children.Add(plot);
            

            Content = container;

            //whenever you change data, remember to invalidate.
            //data.receiveData(1, 2, DataHandler.Sensor.speedometer);
            //data.receiveData(2, 5, DataHandler.Sensor.speedometer);
            //data.receiveData(3, 15, DataHandler.Sensor.speedometer);
            //data.receiveData(4, 20, DataHandler.Sensor.speedometer);
            //data.receiveData(5, 15, DataHandler.Sensor.speedometer);
            //data.receiveData(6, 5, DataHandler.Sensor.speedometer);
            //data.receiveData(7, 2, DataHandler.Sensor.speedometer);
            //plot.Model.InvalidatePlot(true);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            BlobCache.LocalMachine.InsertObject<DataHandler>(id, data);
            if(checkRide != null)
            {
                checkRide.Stop();
                checkRide = null;

                App.BluetoothHandler.readRide.ValueUpdated -= ReadRide_ValueUpdated;
                App.BluetoothHandler.subscribe.ValueUpdated -= Subscribe_ValueUpdated;
            }
            if(subscribeTimer != null)
            {
                subscribeTimer.Stop();
                subscribeTimer = null;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(App.BluetoothHandler.connected != null)
            {
                checkRide = new Timer(200);
                checkRide.Elapsed += checkCurrentRide;
                checkRide.Enabled = true;
            }
        }

        void checkCurrentRide(object sender, ElapsedEventArgs args)
        {
            App.BluetoothHandler.readRide.Read();
            App.BluetoothHandler.readRide.ValueUpdated += ReadRide_ValueUpdated;
        }

        private void ReadRide_ValueUpdated(object sender, BluetoothLE.Core.Events.CharacteristicReadEventArgs e)
        {
            Console.WriteLine("DataPage ReadRide updated");
            Console.WriteLine("Comparing " + e.Characteristic.StringValue.ToUpperInvariant() + " and " + data.rideID.ToUpperInvariant());
            if(data.rideID.ToUpperInvariant() == e.Characteristic.StringValue.ToUpperInvariant())
            {
                if(checkRide != null)
                {
                    checkRide.Stop();
                    checkRide = null;
                }
                Console.WriteLine("Instantiating the subscribe timer");
                subscribeTimer = new Timer(1000);
                subscribeTimer.Elapsed += checkSubscribe;
                subscribeTimer.Start();
                subscribeTimer.Enabled = true;
                App.BluetoothHandler.subscribe.ValueUpdated += Subscribe_ValueUpdated;

                Console.WriteLine(subscribeTimer.ToString() + " " + subscribeTimer.Enabled);
            }
        }

        private void checkSubscribe(object sender, ElapsedEventArgs e)
        {
            App.BluetoothHandler.subscribe.ValueUpdated += Subscribe_ValueUpdated;
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            App.BluetoothHandler.subscribe.Read();
            Console.WriteLine("Checking subscribe characteristic!");
        }

        private void Subscribe_ValueUpdated(object sender, BluetoothLE.Core.Events.CharacteristicReadEventArgs e)
        {
            Console.WriteLine("Subscribe ValueUpdated! " + e.Characteristic.StringValue);
            if (e.Characteristic.StringValue != null && e.Characteristic.StringValue != "")
            {
                data.subscribeDataBuffer(e.Characteristic.StringValue);
            }
        }

        private void createTable()
        {
            var tableView = new TableView()
            {
                HasUnevenRows = true,
                Intent = TableIntent.Data,
                Root = new TableRoot("Bike Diagnostics"),
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            var tableData = data.getData();

            bool alternate = true;
            foreach (BikeData dataCell in tableData)
            {
                DataViewCell temp = new DataViewCell(dataCell.dataType, alternate);
                temp.BindingContext = dataCell;
                alternate = !alternate;

                var sect = new TableSection(dataCell.title)
                {
                    temp
                };
                tableView.Root.Add(sect);
            }

            var tableFrame = new Frame()
            {
                BackgroundColor = Color.FromRgb(0, 121, 193),
                Content = tableView,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };

            container.Children.Add(tableFrame);
        }



        private void debugData(object sender, EventArgs args)
        {
            Console.WriteLine(subscribeTimer.ToString());
            Console.WriteLine("Subscribe properties : " + App.BluetoothHandler.subscribe.CanRead + App.BluetoothHandler.subscribe.CanWrite + App.BluetoothHandler.subscribe.CanUpdate);
            App.BluetoothHandler.subscribe.ValueUpdated += Subscribe_ValueUpdated;
            App.BluetoothHandler.subscribe.Read();
        }

    }
}
