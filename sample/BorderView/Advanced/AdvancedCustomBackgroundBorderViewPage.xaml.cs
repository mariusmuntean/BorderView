using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BorderView.Sample.Advanced
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdvancedCustomBackgroundBorderViewPage : ContentPage
    {
        public AdvancedCustomBackgroundBorderViewPage()
        {
            InitializeComponent();
        }

        private void Thickness_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            BorderView.BorderThickness = (float) e.NewValue;
            ThicknessLbl.Text = $"Border Thickness: {e.NewValue}";
        }

        private void Radius_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            BorderView.CornerRadius = (float) e.NewValue;
            RadiusLbl.Text = $"Corner Radius: {e.NewValue}";
        }

        private void OrbitSpeed_OnValueChanged(object sender, ValueChangedEventArgs e)
        {
            NightSky.OrbitSpeedDegSec = (float) e.NewValue;
            OrbitSpeedLbl.Text = $"Orbit Speed: {e.NewValue}";
        }

        private void IsShowingStarts_OnToggled(object sender, ToggledEventArgs e)
        {
            NightSky.IsShowingStars = e.Value;
        }

        private void FewerStar_OnClicked(object sender, EventArgs e)
        {
            NightSky.StarCount = Math.Max(0, NightSky.StarCount - 30);
        }

        private void MoreStar_OnClicked(object sender, EventArgs e)
        {
            NightSky.StarCount = NightSky.StarCount + 30;
        }

        private void IsSpinning_OnToggled(object sender, ToggledEventArgs e)
        {
            NightSky.IsSpinning = e.Value;
        }

        private void IsPulsing_OnToggled(object sender, ToggledEventArgs e)
        {
            NightSky.IsPulsing = e.Value;
        }
    }
}