using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace BorderView
{
    public partial class CustomBackgroundBorderView : ContentView
    {
        private Grid _myContentView;
        private Frame _myOuterFrame;
        private Frame _myInnerFrame;
        private ContentPresenter _myContentPresenter;

        public CustomBackgroundBorderView()
        {
            InitializeComponent();
            PropertyChanged += OnPropertyChanged;
            _myContentView = ViewHelper.GetTemplateChild<Grid>(this, "MyContentView");
            _myOuterFrame = ViewHelper.GetTemplateChild<Frame>(this, "MyOuterFrame");
            _myInnerFrame = ViewHelper.GetTemplateChild<Frame>(this, "MyInnerFrame");
            _myContentPresenter = ViewHelper.GetTemplateChild<ContentPresenter>(this, "MyContentPresenter");

            _myContentPresenter.PropertyChanged += MyContentPresenterOnBindingContextChanged;
        }

        private void MyContentPresenterOnBindingContextChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ContentProperty.PropertyName)
            {
                if (_myContentPresenter.Content != null)
                {
                    _myContentPresenter.Content.BindingContextChanged += ContentBindingContextChanged;
                }
            }
        }

        private void ContentBindingContextChanged(object sender, EventArgs e)
        {
            var childrenToUpdate = _myContentView.Children.Where(element => element != _myInnerFrame);
            foreach (var child in childrenToUpdate)
            {
                child.BindingContext = _myContentPresenter.Content.BindingContext;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        public static readonly BindableProperty BorderViewProperty = BindableProperty.Create(
            nameof(BorderView),
            typeof(View),
            typeof(CustomBackgroundBorderView),
            new Grid(),
            propertyChanged: OnBorderViewPropertyChanged
        );

        private static void OnBorderViewPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (newvalue != oldvalue
                && bindable is CustomBackgroundBorderView customBackgroundBorderView
                && newvalue is View newCustomBackgroundView)
            {
                // Remove all children of MyContentView that are not MyInnerFrame
                var childrenToRemove = customBackgroundBorderView._myContentView.Children.Where(element => element != customBackgroundBorderView._myInnerFrame);
                foreach (var child in childrenToRemove)
                {
                    customBackgroundBorderView._myContentView.Children.Remove(child);
                }

                // Add the new custom background view in from of the inner Frame
                customBackgroundBorderView._myContentView.Children.Insert(0, newCustomBackgroundView);
            }
        }

        public View BorderView
        {
            get => (View) GetValue(BorderViewProperty);
            set => SetValue(BorderViewProperty, value);
        }

        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(
            nameof(BorderThickness),
            typeof(float),
            typeof(CustomBackgroundBorderView),
            10.0f,
            propertyChanged: OnBorderThicknessChanged);

        private static void OnBorderThicknessChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is CustomBackgroundBorderView simpleBorderView)
            {
                simpleBorderView._myInnerFrame.Margin = new Thickness(simpleBorderView.BorderThickness);
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
            typeof(CustomBackgroundBorderView),
            5.0f,
            propertyChanged: OnCornerRadiusChanged);

        private static void OnCornerRadiusChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is CustomBackgroundBorderView simpleBorderView)
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
            typeof(CustomBackgroundBorderView),
            true,
            propertyChanged: OnHasShadowChanged
        );

        private static void OnHasShadowChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (newValue != oldValue && bindable is CustomBackgroundBorderView simpleBorderView)
            {
                simpleBorderView._myOuterFrame.HasShadow = simpleBorderView.HasShadow;
            }
        }


        public bool HasShadow
        {
            get => (bool) GetValue(HasShadowProperty);
            set => SetValue(HasShadowProperty, value);
        }

        private static float RecomputeOuterCornerRadius(CustomBackgroundBorderView simpleBorderView)
        {
            return simpleBorderView._myInnerFrame.CornerRadius switch
            {
                0.0f => 0.0f,
                _ => simpleBorderView._myInnerFrame.CornerRadius + simpleBorderView.BorderThickness
            };
        }
    }
}