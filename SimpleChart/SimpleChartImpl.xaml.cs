namespace SimpleChart
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Windows.Media;
    using System.Windows.Threading;
    using LiveCharts;
    using LiveCharts.Configurations;
    using LiveCharts.Wpf;

    internal partial class SimpleChartImpl : ISimpleChart
    {
        string ISimpleChart.Title
        {
            get => Dispatcher.Invoke(() => Title);
            set => Dispatcher.Invoke(() => Title = value);
        }

        double ISimpleChart.MinValueAxisY
        {
            get => Dispatcher.Invoke(() => LiveChart.AxisY[0].MinValue);
            set => Dispatcher.Invoke(() => LiveChart.AxisY[0].MinValue = value);
        }

        private int _pointCountRescaleAxisX = 1000;
        int ISimpleChart.PointCountRescaleAxisX
        {
            get => _pointCountRescaleAxisX;
            set => _pointCountRescaleAxisX = value;
        }

        bool ISimpleChart.Animate
        {
            get => Dispatcher.Invoke(() => !LiveChart.DisableAnimations);
            set => Dispatcher.Invoke(() => LiveChart.DisableAnimations = !value);
        }

        private readonly IChartValues _chartValues = new ChartValues<PointModel>();

        internal SimpleChartImpl()
        {
            InitializeComponent();

            Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            LiveChart.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            LiveChart.DisableAnimations = true;
            LiveChart.Hoverable = false;
            LiveChart.DataTooltip = null;

            var mapper = Mappers.Xy<PointModel>()
                .X(model => model.Id)
                .Y(model => model.Value);

            Charting.For<PointModel>(mapper);

            LiveChart.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = _chartValues,
                    PointGeometry = DefaultGeometries.None,
                }
            };

            Separator BackgroundGrid() => new Separator
            {
                Stroke = Brushes.LightGray,
                StrokeThickness = 0.5
            };

            LiveChart.AxisY.Add(new Axis { Separator = BackgroundGrid(), MinValue = 0 });
            LiveChart.AxisX.Add(new Axis { Separator = BackgroundGrid() });
        }

        private int _seenPointsCount;
        private int _takeEveryNth = 1;

        void ISimpleChart.SetSeries(IReadOnlyCollection<double> points)
        {
            _seenPointsCount = points.Count;
            _takeEveryNth = points.Count < _pointCountRescaleAxisX ? 1 : points.Count / _pointCountRescaleAxisX;
            var sparsePoints = points.Where((p, idx) => idx % _takeEveryNth == 0);
            var pointModels = sparsePoints.Select((v, idx) => new PointModel((idx + 1) * _takeEveryNth, v)).ToList();
            _chartValues.Clear();
            _chartValues.AddRange(pointModels);
        }

        void ISimpleChart.AddPoint(double value)
        {
            _seenPointsCount++;
            if (_seenPointsCount % _takeEveryNth != 0) return;

            if (_chartValues.Count >= _pointCountRescaleAxisX)
            {
                RescaleChart();
                return;
            }

            _chartValues.Add(new PointModel(_seenPointsCount, value));
        }

        private void RescaleChart()
        {
            var result = _chartValues.Cast<object>().Where((t, i) => i % 2 == 0).ToList();
            _chartValues.Clear();
            _chartValues.AddRange(result);
            _takeEveryNth *= 2;
        }

        void ISimpleChart.Clear(bool forceRedraw)
        {
            _chartValues.Clear();
            if (forceRedraw) Dispatcher.Invoke(() => LiveChart.Update(false, true));
            _seenPointsCount = 0;
            _takeEveryNth = 1;
        }

        void ISimpleChart.Close() => Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(Close));

        private class PointModel
        {
            public readonly int Id;
            public readonly double Value;

            public PointModel(int id, double value)
            {
                Id = id;
                Value = value;
            }
        }
    }
}
