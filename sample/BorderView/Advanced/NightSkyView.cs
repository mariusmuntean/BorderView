using System;
using System.Diagnostics;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace BorderView.Sample.Advanced
{
    public class NightSkyView : SKCanvasView
    {
        private object _lock = new object();

        private const string _starSvgData = "M480,50L423.8,182.6L280,194.8L389.2,289.4L356.4,430L480,355.4L480,355.4L603.6,430L570.8,289.4L680,194.8L536.2,182.6Z";

        private SKPaint _starPaint = new SKPaint()
        {
            Color = SKColors.Yellow,
            Style = SKPaintStyle.Fill
        };

        private StarDataGenerator _starDataGenerator;
        private StarData[] _starData;
        private (float, float) _scaleBounds;
        private (float, float) _scaleSpeedBounds;
        private (float, float) _rotationSpeedBounds;
        private int _starCount;
        private bool _isShowingStars;
        private float _orbitSpeedDegSec;
        private float _orbitPosition;

        private SKPath _starTemplate;
        private float _edgeLength;

        public NightSkyView()
        {
            _starDataGenerator = new StarDataGenerator();

            OrbitSpeedDegSec = 0.0f;
            StarCount = 50;
            IsSpinning = false;
            IsPulsing = false;
            IsShowingStars = false;

            _scaleBounds = (0.3f, 1.8f);
            _scaleSpeedBounds = (-0.01f, 0.03f);
            _rotationSpeedBounds = (-2.0f, 2.0f);

            _orbitPosition = 0.0f;

            _edgeLength = 10.0f;

            _starTemplate = new SKPath();
            _starTemplate.MoveTo(0.0f, -_edgeLength / 2.0f);
            _starTemplate.LineTo(_edgeLength / 6.0f, -_edgeLength / 6.0f);
            _starTemplate.LineTo(_edgeLength / 2.0f, 0.0f);
            _starTemplate.LineTo(_edgeLength / 6.0f, _edgeLength / 6.0f);
            _starTemplate.LineTo(0.0f, _edgeLength / 2.0f);
            _starTemplate.LineTo(-_edgeLength / 6.0f, _edgeLength / 6.0f);
            _starTemplate.LineTo(-_edgeLength / 2.0f, 0.0f);
            _starTemplate.LineTo(-_edgeLength / 6.0f, -_edgeLength / 6.0f);
            _starTemplate.Close();

            Animate();
        }

        public float OrbitSpeedDegSec
        {
            get => _orbitSpeedDegSec;
            set
            {
                lock (_lock)
                {
                    _orbitSpeedDegSec = value;
                    Device.BeginInvokeOnMainThread(() => InvalidateSurface());
                }
            }
        }

        public int StarCount
        {
            get => _starCount;
            set
            {
                lock (_lock)
                {
                    _starCount = value;
                    _starData = null;

                    Device.BeginInvokeOnMainThread(() => InvalidateSurface());
                }
            }
        }

        public bool IsSpinning { get; set; }
        public bool IsPulsing { get; set; }

        public bool IsShowingStars
        {
            get => _isShowingStars;
            set
            {
                lock (_lock)
                {
                    _isShowingStars = value;
                    _starData = null;

                    Device.BeginInvokeOnMainThread(() => InvalidateSurface());
                }
            }
        }

        public void Animate()
        {
            var sw = new Stopwatch();
            sw.Start();
            var cycleLengthMillis = 2000;

            Device.StartTimer(TimeSpan.FromMilliseconds(1000.0f / 60), () =>
                {
                    var progress = ((float) (sw.ElapsedMilliseconds % cycleLengthMillis)) / cycleLengthMillis;
                    Tick(progress);

                    return true;
                }
            );
        }

        private void Tick(float progress)
        {
            lock (_lock)
            {
                if (_starData == null)
                {
                    return;
                }

                foreach (var starData in _starData)
                {
                    if (IsPulsing)
                    {
                        if (progress <= 0.5f)
                        {
                            starData.RelativeScale += starData.ScalingSpeed;
                        }
                        else
                        {
                            starData.RelativeScale -= starData.ScalingSpeed;
                        }
                    }

                    if (IsSpinning)
                    {
                        starData.Rotation += starData.RotationSpeed;
                    }

                    _orbitPosition = (_orbitPosition + OrbitSpeedDegSec) % 360.0f;
                }

                Device.BeginInvokeOnMainThread(() => InvalidateSurface());
            }
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;

            canvas.Clear(SKColors.Black);

            if (!IsShowingStars)
            {
                return;
            }

            // Rotate to "orbit"
            canvas.Save();
            canvas.RotateDegrees(_orbitPosition, e.Info.Size.Width / 2.0f, e.Info.Size.Height / 2.0f);


            lock (_lock)
            {
                // Draw stars
                _starData = _starData ?? _starDataGenerator.GenerateStarData(StarCount, e.Info.Rect, _scaleBounds, _scaleSpeedBounds, _rotationSpeedBounds);


                for (var i = 0; i < _starData.Length; i++)
                {
                    canvas.Save();

                    var currentStarData = _starData[i];
                    canvas.Translate(currentStarData.Center.X, currentStarData.Center.Y);
                    canvas.RotateDegrees(currentStarData.Rotation);
                    canvas.Scale(currentStarData.RelativeScale);

                    var currentStarPath = new SKPath(_starTemplate);
                    canvas.DrawPath(currentStarPath, _starPaint);

                    canvas.Restore();
                }
            }

            canvas.Restore();
        }
    }

    class StarDataGenerator
    {
        public StarData[] GenerateStarData(int amountOfStars,
            SKRectI region,
            (float minScale, float maxScale) scaleBounds,
            (float minScaleSpeed, float maxScaleSpeed) scaleSpeedBounds,
            (float minRotationSpeed, float maxRotationSpeed) rotationSpeedBounds)
        {
            var rand = new Random();
            var starData = new StarData[amountOfStars];

            for (int i = 0; i < amountOfStars; i++)
            {
                var center = new SKPoint(
                    (float) (region.Left + (rand.NextDouble() * region.Width)),
                    (float) (region.Top + (rand.NextDouble() * region.Height))
                );

                var scaleRange = scaleBounds.maxScale - scaleBounds.minScale;
                var relativeScale = scaleBounds.minScale + (rand.NextDouble() * scaleRange);

                var scaleSpeedRange = scaleSpeedBounds.maxScaleSpeed - scaleSpeedBounds.minScaleSpeed;
                var scalingSpeed = scaleSpeedBounds.minScaleSpeed + (rand.NextDouble() * scaleSpeedRange);

                var rotation = rand.NextDouble() * 360.0f;

                var rotationSpeedRange = rotationSpeedBounds.maxRotationSpeed - rotationSpeedBounds.minRotationSpeed;
                var rotationSpeed = rotationSpeedBounds.minRotationSpeed + (rand.NextDouble() * rotationSpeedRange);

                starData[i] = new StarData(center, (float) relativeScale, (float) scalingSpeed, (float) rotation, (float) rotationSpeed);
            }

            return starData;
        }
    }

    class StarData
    {
        public SKPoint Center { get; set; }
        public float RelativeScale { get; set; }
        public float ScalingSpeed { get; set; }
        public float Rotation { get; set; }
        public float RotationSpeed { get; set; }

        public StarData(SKPoint center, float relativeScale, float scalingSpeed, float rotation, float rotationSpeed)
        {
            Center = center;
            RelativeScale = relativeScale;
            ScalingSpeed = scalingSpeed;
            Rotation = rotation;
            RotationSpeed = rotationSpeed;
        }
    }
}