using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SyncCycle
{
    class TabbedPageWrapper : TabbedPage
    {
        public TabbedPageWrapper(ArrayList list)
        {
            foreach(ContentPage page in list)
            {
                Children.Add(page);
            }
        }
    }
}
