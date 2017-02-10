﻿using System;
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
        public DataHandler data;

        //private PlotView plot;

        StackLayout container = new StackLayout()
        {
            VerticalOptions = LayoutOptions.FillAndExpand,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            Spacing = 5
        };

        public DataPage(string rideID)
        {
            Title = "Data";
            BackgroundColor = Color.FromHex("#0079c2");
            Padding = 10;
            data = new DataHandler(rideID);

            //Create data views
            createTable();

            var debugButton = new Button()
            {
                Text = "Send some dummy data",
            };
            debugButton.Clicked += debugData;
            
            //plot = data.getPlot(DataHandler.Sensor.speedometer);

            

            container.Children.Add(debugButton);
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

            container.Children.Add(tableView);
        }



        private void debugData(object sender, EventArgs args)
        {
            Random rnd = new Random();
            data.energy.update(eData.Save, rnd.Next(1, 30));
            data.green.update(co2Data.Prevented, rnd.Next(1, 20));
            data.kinematics.update(kData.dTraveled, rnd.Next(1, 20));
        }

    }
}