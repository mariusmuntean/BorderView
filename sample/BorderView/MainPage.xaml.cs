using System;
using System.ComponentModel;
using BorderView.Sample.Advanced;
using Xamarin.Forms;

namespace BorderView.Sample
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Simple_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SimpleBorderViewPage(), true);
        }

        private void Custom_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CustomBackgroundBorderViewPage(), true);
        }
        
        private void Advanced_OnClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdvancedCustomBackgroundBorderViewPage(), true);
        }
    }
}