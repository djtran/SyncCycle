using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SyncCycle
{
    class DataPage : ContentPage
    {
       

        public DataPage(List<BikeData> data)
        {
            Padding = 20;

            var tableView = new TableView()
            {
                HasUnevenRows = true,
                Intent = TableIntent.Data,
                Root = new TableRoot("Bike Diagnostics")
            };

            foreach (BikeData dataCell in data)
            {
                DataViewCell temp = new DataViewCell();
                temp.BindingContext = dataCell;

                var sect = new TableSection("Beep Boop Section")
                {
                    temp
                };
                tableView.Root.Add(sect);
            }

            Content = new StackLayout
            {
                Children = {
                    tableView
                },
                Spacing = 10
            };
        }

    }
}
