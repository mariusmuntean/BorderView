using Xamarin.Forms;

namespace BorderView
{
    public partial class SimpleBorderView : ContentView
    {
        public SimpleBorderView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(
            nameof(BorderThickness),
            typeof(float),
            typeof(SimpleBorderView),
            10.0f,
            propertyChanged: OnBorderThicknessChanged);

        private static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is SimpleBorderView simpleBorderView)
            {
                simpleBorderView.MyContentView.Padding = new Thickness(simpleBorderView.BorderThickness);
                simpleBorderView.MyOuterFrame.CornerRadius = simpleBorderView.MyInnerFrame.CornerRadius + simpleBorderView.BorderThickness;
            }
        }

        public float BorderThickness
        {
            get => (float) GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }


        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            nameof(CornerRadius),
            typeof(float),
            typeof(SimpleBorderView),
            5.0f,
            propertyChanged: OnCornerRadiusChanged);

        private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is SimpleBorderView simpleBorderView)
            {
                simpleBorderView.MyInnerFrame.CornerRadius = simpleBorderView.CornerRadius;
                simpleBorderView.MyOuterFrame.CornerRadius = simpleBorderView.MyInnerFrame.CornerRadius + simpleBorderView.BorderThickness;
            }
        }

        public float CornerRadius
        {
            get => (float) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
    }
}