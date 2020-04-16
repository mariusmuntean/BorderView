using System.ComponentModel;
using Xamarin.Forms;

namespace BorderView
{
    public partial class SimpleBorderView : ContentView
    {
        private ContentView _myContentView;
        private Frame _myOuterFrame;
        private Frame _myInnerFrame;
        private ContentPresenter _myContentPresenter;

        public SimpleBorderView()
        {
            InitializeComponent();
            PropertyChanged += OnPropertyChanged;

            _myContentView = ViewHelper.GetTemplateChild<ContentView>(this, "MyContentView");
            _myOuterFrame = ViewHelper.GetTemplateChild<Frame>(this, "MyOuterFrame");
            _myInnerFrame = ViewHelper.GetTemplateChild<Frame>(this, "MyInnerFrame");
            _myContentPresenter = ViewHelper.GetTemplateChild<ContentPresenter>(this, "MyContentPresenter");
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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
                simpleBorderView._myContentView.Padding = new Thickness(simpleBorderView.BorderThickness);
                simpleBorderView._myOuterFrame.CornerRadius = RecomputeOuterCornerRadius(simpleBorderView);
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
                simpleBorderView._myInnerFrame.CornerRadius = simpleBorderView.CornerRadius;
                simpleBorderView._myOuterFrame.CornerRadius = RecomputeOuterCornerRadius(simpleBorderView);
            }
        }

        public float CornerRadius
        {
            get => (float) GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(
            nameof(HasShadow),
            typeof(bool),
            typeof(SimpleBorderView),
            true,
            propertyChanged: OnHasShadowChanged
        );

        private static void OnHasShadowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is SimpleBorderView simpleBorderView)
            {
                simpleBorderView._myOuterFrame.HasShadow = simpleBorderView.HasShadow;
            }
        }


        public bool HasShadow
        {
            get => (bool) GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(
            nameof(BorderColor),
            typeof(Color),
            typeof(SimpleBorderView),
            Color.White,
            propertyChanged: OnBorderColorChanged);

        private static void OnBorderColorChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is SimpleBorderView simpleBorderView)
            {
                simpleBorderView._myContentView.BackgroundColor = simpleBorderView.BorderColor;
            }
        }

        public Color BorderColor
        {
            get => (Color) GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        private static float RecomputeOuterCornerRadius(SimpleBorderView simpleBorderView)
        {
            return simpleBorderView._myInnerFrame.CornerRadius switch
            {
                0.0f => 0.0f,
                _ => simpleBorderView._myInnerFrame.CornerRadius + simpleBorderView.BorderThickness
            };
        }
    }
}