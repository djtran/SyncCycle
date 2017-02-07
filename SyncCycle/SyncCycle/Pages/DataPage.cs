using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using Xamarin.Forms;
using SyncCycle.DataVisuals;
using OxyPlot.Xamarin.Forms;
using System.ComponentModel;

namespace SyncCycle
{
    class DataPage : ContentPage
    {      
        
        public List<BikeData> Data = new List<BikeData>();
        public BTDeviceButton b;
        public DataHandler data;

        private PlotView plot;


        StackLayout search = new StackLayout()
        {
            VerticalOptions = LayoutOptions.End,
        };

        ScrollView searchWrap = new ScrollView()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            HeightRequest = 100,
        };

        StackLayout container = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Spacing = 5
        };

        public DataPage()
        {
            Title = "Data";
            BackgroundColor = Color.FromHex("#0079c2");
            Padding = 10;
            data = new DataHandler();

            var tableView = new TableView()
            {
                BackgroundColor = Color.FromRgb(8, 93, 159),
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

            b = new BTDeviceButton(this);
            
            //plot = data.getPlot(DataHandler.Sensor.speedometer);

            container.Children.Add(tableView);
            //container.Children.Add(plot);
            container.Children.Add(searchWrap);
            container.Children.Add(b);

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

            App.BluetoothAdapter.ScanTimeoutElapsed += timedout;
        }

        private void timedout(object sender, EventArgs e)
        {
            updateSearchBox("Search ended.");   
        }

        public void updateSearchBox(string words)
        {
            search.Children.Add(new Label { Text = words });
            searchWrap.Content = search;
        }

        public void updateSearchBox(View visual)
        {
            search.Children.Add(visual);
            searchWrap.Content = search;
        }

        public void updateContainer(View visual)
        {
            container.Children.Add(visual);
        }

    }
}
