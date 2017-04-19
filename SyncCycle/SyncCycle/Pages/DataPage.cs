using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Xamarin.Forms;
using OxyPlot.Xamarin.Forms;
using System.ComponentModel;
using System.Timers;

namespace SyncCycle
{
    class DataPage : ContentPage
    {      
        
        public DataHandler data;
        public string id;

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
            
            id = rideID;
            if(data == null)
            {
                data = new DataHandler(rideID);
            }
            //Create data views
            createTable();

            var returnButton = new Button()
            {
                Text = "Return to list"
            };
            returnButton.Clicked += (s, e) =>
            {
                this.Navigation.PopAsync();
            };

            //plot = data.getPlot(DataHandler.Sensor.speedometer);

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
            if(App.BluetoothHandler.connected != null)
            {                
                App.BluetoothHandler.subscribe.ValueUpdated -= Subscribe_ValueUpdated;
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if(App.BluetoothHandler.connected != null)
            {
                App.BluetoothHandler.subscribe.ValueUpdated += Subscribe_ValueUpdated;

                await App.BluetoothHandler.subscribe.StartUpdatesAsync();

            }
        }
        
        private void Subscribe_ValueUpdated(object sender, Plugin.BLE.Abstractions.EventArgs.CharacteristicUpdatedEventArgs e)
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


    }
}
