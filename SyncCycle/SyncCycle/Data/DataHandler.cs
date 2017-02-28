using OxyPlot;
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
        string rideID;

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

            if(rideID != "")
            {
                getRideFromID(rideID);
            }
            else
            {
                askForID();
            }
        }

        internal KinematicsData KinematicsData
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal EnergyData EnergyData
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        internal GreenData GreenData
        {
            get
            {
                throw new System.NotImplementedException();
            }

            set
            {
            }
        }

        public bool askForID()
        {
            DateTime date = DateTime.Today;

            //TO DO
            //send request over bluetooth to get an ID
            //bluetooth should respond at some point pls.

            //asynchronously populate the ID?

            return true;
        }

        public bool getRideFromID(string ID)
        {
            //TO DO
            //send request over bluetooth to get ride by ID
            //bluetooth should return with json objects
            //**Maybe we should just pull stat objects, and the graph objects can be a separate pull

            //create EnergyData, GreenData, and KinematicsData from json objects

            //true = success, false = failed somewhere along the way
            return true;
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
            Console.WriteLine("GETTING PLOT " + sensorEnum.ToString());
            Console.WriteLine("GETTING PLOT " + sensorEnum.ToString());

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
