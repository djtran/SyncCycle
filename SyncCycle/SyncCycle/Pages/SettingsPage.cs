using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SyncCycle
{
    public class SettingsPage : ContentPage
    {

        void OnemergencyContactClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("tel://911"));
        }

        void OncontactHubClicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("tel://7742397587"));
        }

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

        public SettingsPage()
        {

            Title = "Contact Us";
            BackgroundColor = Color.FromRgb(48,48,48);
           

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (container.Children.Count < 4)
            {
                Button emergencyContact = new Button
                {
                    Text = "Emergency Contact",
                    HorizontalOptions = LayoutOptions.Center
                };
                emergencyContact.Clicked += OnemergencyContactClicked;

                Button contactHub = new Button
                {
                    Text = "Contact Sustainability Hub",
                    HorizontalOptions = LayoutOptions.Center
                };
                contactHub.Clicked += OncontactHubClicked;
                
                var branding = createBranding();

                container.Children.Add(branding);
                container.Children.Add(searchWrap);
                container.Children.Add(emergencyContact);
                container.Children.Add(contactHub);
            }
            Content = container;
           
        }

        private View createBranding()
        {
            StackLayout branding = new StackLayout()
            {
                BackgroundColor = Color.FromRgb(29, 17, 96),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Fill,
                Spacing = 0,
                Padding = 25,
            };

            var logo = new Image()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Source = "settings_logo.png"
            };

            var company = new Label()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.StartAndExpand,
                Text = "nationalgrid",
                FontSize = 1.75*Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                FontAttributes = FontAttributes.Bold,
            };

            branding.Children.Add(logo);
            branding.Children.Add(company);

            return branding;
        }
        

        public void updateSearchBox(string words)
        {
            Device.BeginInvokeOnMainThread(() => {
                search.Children.Add(new Label { Text = words });
                searchWrap.Content = search;
            });

        }

        public void updateSearchBox(View visual)
        {
            Device.BeginInvokeOnMainThread(() => {
                search.Children.Add(visual);
                searchWrap.Content = search;
            });
        }

        public void updateContainer(View visual)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                container.Children.Add(visual);
                Content = container;
            });
        }
    }
}
