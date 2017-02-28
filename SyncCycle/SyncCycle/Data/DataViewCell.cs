using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SyncCycle
{
    //EnergyData - Energy Usage
    //GreenData - How you're helping the planet.
    //KinematicsData - Speed, Distance, Time, etc. Physics equations.
    enum ViewType { Energy, Green, Kinematics }

    class DataViewCell : ViewCell
    {
        public DataViewCell(ViewType which, bool imgLeft)
        {

            var image = new Image
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                Margin = 5,

            };
            image.SetBinding(Image.SourceProperty, new Binding("imgURI"));
            image.HeightRequest = image.WidthRequest = 60;

            StackLayout labelLayout;

            switch (which)
            {
                case ViewType.Energy:
                    labelLayout = CreateLabelLayout(3);
                    break;
                case ViewType.Green:
                    labelLayout = CreateLabelLayout(1);
                    break;
                case ViewType.Kinematics:
                    labelLayout = CreateLabelLayout(4);
                    break;
                default:
                    Console.WriteLine("[Warning] DataViewCell creating default label list");
                    labelLayout = CreateLabelLayout(3);
                    break;

            }
            StackLayout viewLayout = new StackLayout()
            {
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
            };

            if(imgLeft)
            {
                viewLayout.Children.Add(image);
                viewLayout.Children.Add(labelLayout);
            }
            else
            {
                viewLayout.Children.Add(labelLayout);
                viewLayout.Children.Add(image);
            }
            View = viewLayout;
        }

        static StackLayout CreateLabelLayout(int number)
        {

            var nameLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Orientation = StackOrientation.Vertical,
                
            };

            for(int i = 1; i <= number; i++)
            {
                var label = new Label {
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                    FontAttributes = FontAttributes.Bold
                };

                label.SetBinding(Label.TextProperty, new Binding("Display" + i.ToString()));

                nameLayout.Children.Add(label);
            }
          
            return nameLayout;
        }
    }
}
