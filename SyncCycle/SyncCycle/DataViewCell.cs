using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SyncCycle
{
    class DataViewCell : ViewCell
    {
        public DataViewCell()
        {
           
            var image = new Image
            {
                HorizontalOptions = LayoutOptions.Start
            };
            image.SetBinding(Image.SourceProperty, new Binding("imgURI"));
            image.WidthRequest = image.HeightRequest = 50;

            var nameLayout = CreateNameLayout();
            var viewLayout = new StackLayout()
            {
                Padding = 10,
                Orientation = StackOrientation.Horizontal,
                Children = { image, nameLayout }
            };
            View = viewLayout;
        }

        static StackLayout CreateNameLayout()
        {
            var nameLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            nameLabel.SetBinding(Label.TextProperty, new Binding("Display1"));

            var twitterLabel = new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            twitterLabel.SetBinding(Label.TextProperty, "Display2");

            var thirdLabel= new Label
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            thirdLabel.SetBinding(Label.TextProperty, "Display3");

            var nameLayout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { nameLabel, twitterLabel, thirdLabel }
            };
            return nameLayout;
        }
    }
}
