﻿using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace SyncCycle
{

    class DataHandler
    {
        //One DataHandler per ride
        public string rideID;

        //Creating graphs and the like
        PlotModel speedometer = new PlotModel { Title = "Speedometer" };
        PlotModel sensor2 = new PlotModel { Title = "Sensor2" };
        PlotModel sensor3 = new PlotModel { Title = "Sensor3" };

        //Data Modules
        public EnergyData energy = new EnergyData();
        public GreenData green = new GreenData();
        public KinematicsData kinematics = new KinematicsData();

        public enum Sensor
        {
            speedometer,
            sensor2,
            sensor3
        };
        
        public DataHandler(string rideID)
        {
            initAxes(speedometer);
            initAxes(sensor2);
            initAxes(sensor3);
            initDataPlot(speedometer);
            initDataPlot(sensor2);
            initDataPlot(sensor3);

            this.rideID = rideID;

        }

        private void initAxes(PlotModel model)
        {
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom });
            model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Maximum = 25, Minimum = 0 });
        }

        private void initDataPlot(PlotModel model)
        {
            var series = new LineSeries
            {
                MarkerType = OxyPlot.MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyPlot.OxyColors.White
            };
            series.Points.Add(new DataPoint(0, 0));

            model.Series.Add(series);
            model.Background = OxyColor.FromRgb(255, 255, 255);
        }

        public PlotView getPlot(Sensor sensorEnum)
        {

            PlotView graph = new PlotView()
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                //Currently must make a width/height request in order for view to show up on screen. sucks we cant dynamically arrange based on screensize
                WidthRequest = 100,
                HeightRequest = 100,
            };
            switch(sensorEnum)
            {
                case Sensor.speedometer:
                    Console.WriteLine("Adding speedometer model to plotview");
                    graph.Model = speedometer;
                    break;
                case Sensor.sensor2:
                    graph.Model = sensor2;
                    break;
                case Sensor.sensor3:
                    graph.Model = sensor3;
                    break;
                default:
                    graph.Model = null;
                    break;
            }
            graph.Model.InvalidatePlot(true);
            return graph;
        }

        public List<BikeData> getData()
        {
            List<BikeData> list = new List<BikeData>();
            list.Add(energy);
            list.Add(green);
            list.Add(kinematics);

            return list;
        }

        public void subscribeDataBuffer(string received)
        {
            string[] halves = received.Split(':');
            try
            {
                float val = float.Parse(halves[1]);

                switch(halves[0])
                {
                    case "energyUsed":
                        energy.update(eData.Used, val);
                        break;
                    case "energyEquiv":
                        energy.update(eData.Equiv, val);
                        break;
                    case "energySav":
                        energy.update(eData.Save, val);
                        break;
                    case "carbonEm":
                        green.update(co2Data.Prevented, val);
                        break;
                    case "speedAvg":
                        kinematics.update(kData.vAvg, val);
                        break;
                    case "speedTop":
                        kinematics.update(kData.vTop, val);
                        break;
                    case "distanceTra":
                        kinematics.update(kData.dTraveled, val);
                        break;
                    case "timeEla":
                        kinematics.update(kData.tElapsed, val);
                        break;
                    default:
                        break;
                }
            }
            catch (FormatException f)
            {
                Console.WriteLine("woops");
            }

        }

        public void receiveData(double time, double value, Sensor sensorEnum)
        {
            DataPoint point = new DataPoint(time, value);

            switch (sensorEnum)
            {
                case Sensor.speedometer:
                    foreach (LineSeries s in speedometer.Series)
                    {
                        s.Points.Add(point);
                    }
                    speedometer.InvalidatePlot(true);
                    break;
                case Sensor.sensor2:
                    foreach (LineSeries s in sensor2.Series)
                    {
                        s.Points.Add(point);
                    }
                    sensor2.InvalidatePlot(true);
                    break;
                case Sensor.sensor3:
                    foreach (LineSeries s in sensor3.Series)
                    {
                        s.Points.Add(point);
                    }
                    sensor3.InvalidatePlot(true);
                    break;

            }

        }
    }
}
