using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CRM.Commands;
using CRM.Models;
using CRM.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CRM.ViewModels
{
    public class AnalizeViewModel : NotifyPropertyChange
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;
        public ObservableCollection<RangeWithOrders> rangeWithOrders { get; } = new(); // принимает вытягиваемые данные из базы данных в модель (дата | колличество заказов)
        public ObservableCollection<PriceByMonth> priceByMonths { get; } = new();
        public ObservableCollection<SourseCount> OrdersSourseCount { get; } = new();
        public PlotModel WeekPlotModel { get; set; }
        public PlotModel PieGraff { get; set; }
        public PlotModel IncomeByMonthBar { get; set; }
        public PlotModel SpendingAnalize { get; set; }

        public ICommand PreviousWeek { get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand TodayWeekGraff { get; set; }
        public ICommand ReloadPieGraff { get; set; }
        public ICommand GoBackPerSource { get; set; }
        public ICommand GoForwardPerSource { get; set; }

        private int? maxOrdersValue { get; set; }
        private int? _yAxisHeight = 5; // изначально высота оси Y = 5 с шагом 1
        private int? YAxisHeight // присваеваем значение 10, если заказов становится или было вытянуто 5 или > 5 и т.д
        {
            get => _yAxisHeight;
            set
            {
                if (_yAxisHeight != value)
                {
                    _yAxisHeight = value;
                    OnPropertyChange(nameof(YAxisHeight));
                }
            }
        }
        private DateTime _centerOfWeek = DateTime.Now;
        private DateTime CenterOfWeek //сегодняшняя | центральная дата 
        {
            get => _centerOfWeek;
            set
            {
                if (_centerOfWeek != value)
                {
                    _centerOfWeek = value;
                    OnPropertyChange(nameof(CenterOfWeek));
                }
            }
        }
        private DateTime _today = DateTime.Now;
        private DateTime _Today
        {
            get => _today;
            set
            {
                if(_today != value)
                {
                    _today = value;
                    OnPropertyChange(nameof(_Today));
                }
            }
        }

        private string? _percentageOfRejections;
        public string? PercentageOfRejections
        {
            get => _percentageOfRejections;
            set
            {
                if (_percentageOfRejections != value)
                {
                    _percentageOfRejections = value;
                    OnPropertyChange(nameof(PercentageOfRejections));
                }
            }
        }
        private string? _todayOrdersCount;
        public string? TodayOrdersCount 
        { 
            get => _todayOrdersCount;
            set
            {
                if(_todayOrdersCount != value)
                {
                    _todayOrdersCount = value;
                    OnPropertyChange(nameof(TodayOrdersCount));
                }
            }
        }
        private string? _todayOrdersIncome;
        public string? TodayOrdersIncame
        {
            get => _todayOrdersIncome;
            set
            {
                if(_todayOrdersIncome != value)
                {
                    _todayOrdersIncome = value;
                    OnPropertyChange(nameof(TodayOrdersIncame));
                }
            }
        }
        private string? _monthOrderCount;
        public string? MonthOrderCount
        {
            get => _monthOrderCount;
            set
            {
                if(_monthOrderCount != value)
                {
                    _monthOrderCount = value;
                    OnPropertyChange(nameof(MonthOrderCount));
                }
            }
        }
        private string? _monthCancelledCount;
        public string? MonthCancelledCount
        {
            get => _monthCancelledCount;
            set
            {
                if(_monthCancelledCount != value)
                {
                    _monthCancelledCount = value;
                    OnPropertyChange(nameof(MonthCancelledCount));
                }
            }
        }
        private string? _monthIncome;
        public string? MonthIncome
        {
            get => _monthIncome;
            set
            {
                if(_monthIncome != value)
                {
                    _monthIncome = value;
                    OnPropertyChange(nameof(MonthIncome));
                }
            }
        }

        public AnalizeViewModel()
        {
            PreviousWeek = new RelayCommand(Click => GoToPreviousWeek());
            NextWeek = new RelayCommand(Click => GoToNextWeek());
            TodayWeekGraff = new RelayCommand(Click => BackToTodayWeekGraff());
            ReloadPieGraff = new RelayCommand(Click => ReloadPie());
            GoBackPerSource = new RelayCommand(Click => GoBackPercentSours());
            GoForwardPerSource = new RelayCommand(Click => GoForwardPercentSource());

            WeekPlotModel = new PlotModel();
            PieGraff = new PlotModel();
            IncomeByMonthBar = new PlotModel();
            SpendingAnalize = new PlotModel();
            LoadTodayData();
            LoadMonthData();
            LoadSourseToDataGrid();
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
            PieModelGraff();
            IncomeBarGraff();
            SpendingAnalizeGraff();
            RejectionPercent();
        }
        private void LoadMonthData()
        {
            MonthOrderCount = $"Заказов за месяц: 300";
            MonthCancelledCount = $"Отказов за месяц: 3";
            MonthIncome = $"Прибыль за месяц: 20000";
        }
        private void LoadTodayData()
        {
            TodayOrdersCount = $"Заказов за сегодня: 10";
            TodayOrdersIncame = $"Прибыль за сегодня: 50000";
        }
        private void LoadSourseToDataGrid()
        {
            OrdersSourseCount.Clear();
            _db.LoadSourseCountToDataGtid(OrdersSourseCount, _Today.Month);
        }
        private void ExtractDataFromDatabase()
        {
            rangeWithOrders.Clear();
            _db.SelectDataToWeekGraff(rangeWithOrders, CenterOfWeek.AddDays(-3), CenterOfWeek.AddDays(3)); // формирование данных типо: (дата | колличество заказов), в диапазоне []
        }
        private void WeekPlotModelGraff()
        {
            WeekPlotModel.Series.Clear();
            maxOrdersValue = rangeWithOrders.Max(x => x.Count);
            if (YAxisHeight < maxOrdersValue)
            {
                YAxisHeight = maxOrdersValue + 1;
            }

            var lineSeries = new LineSeries
            {
                Title = "Линия",
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyPlot.OxyColors.IndianRed,
                Color = OxyPlot.OxyColors.Black,
                LineStyle = LineStyle.Solid,
                StrokeThickness = 2,
                TrackerFormatString = "X = {2}, Y = {4}"
            };

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = (double)YAxisHeight,
                MajorStep = 1
            };
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 8,
                MajorStep = 1,
                LabelFormatter = value => // формат оси X
                {
                    string[] weekDays = new string[7] {
                        CenterOfWeek.AddDays(-3).DayOfWeek.ToString(),
                        CenterOfWeek.AddDays(-2).DayOfWeek.ToString(),
                        CenterOfWeek.AddDays(-1).DayOfWeek.ToString(),
                        CenterOfWeek.DayOfWeek.ToString(), // сегодняшний день недели
                        CenterOfWeek.AddDays(1).DayOfWeek.ToString(),
                        CenterOfWeek.AddDays(2).DayOfWeek.ToString(),
                        CenterOfWeek.AddDays(3).DayOfWeek.ToString()};
                    if (value >= 1 && value <= 7)
                    {
                        return weekDays[(int)value - 1];
                    }

                    return "";
                }
            };

            UpdateTitle();
            CalculateWeekGraffPosition(lineSeries);
            WeekPlotModel.Axes.Add(yAxis);
            WeekPlotModel.Axes.Add(xAxis);
            WeekPlotModel.Series.Add(lineSeries);
        }
        private void CalculateWeekGraffPosition(LineSeries lineSeries)
        {
            foreach (var order in rangeWithOrders)
            {
                int x = (int)((order.Date - CenterOfWeek.AddDays(-3)).TotalDays + 1);
                double y = order.Count;
                lineSeries.Points.Add(new DataPoint(x + 1, y));
            }
        }
        private void UpdateTitle() // обновления UI Plot Title при переключении +1 -1 от сегодняшнего дня
        {
            var startDate = CenterOfWeek.AddDays(-3);
            var endDate = CenterOfWeek.AddDays(3);
            WeekPlotModel.Title = $"Колличество заказов в диапазоне: {startDate.ToString("dd.MM.yyy")} - {CenterOfWeek.ToString("d MMMM")} - {endDate.ToString("dd.MM.yyy")}";
            WeekPlotModel.InvalidatePlot(true);
        }
        private void GoToPreviousWeek()
        {
            CenterOfWeek = CenterOfWeek.AddDays(-1);
            ExtractDataFromDatabase(); // каждый раз должен? вытягивает свой диапазон [дат]
            WeekPlotModelGraff(); //перерисовка графика
            UpdateTitle(); // обновляем title
        }
        private void GoToNextWeek()
        {
            CenterOfWeek = CenterOfWeek.AddDays(1);
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
            UpdateTitle();
        }
        private void BackToTodayWeekGraff()
        {
            CenterOfWeek = DateTime.Now;
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
            UpdateTitle();
        }

        private void PieModelGraff()
        {
            _db.SelectAllPriceByMonth(priceByMonths);
           //Debug.WriteLine($"{orders.Month}: {orders.SumOfPriceByMonth}"); // Октябрь: 2481, Ноябрь: 601, Декабрь: 3000

            PieGraff.Series.Clear();
            PieGraff.Title = "Оборот по месяцам за год";

            var pieSeries = new PieSeries
            {
                Stroke = OxyColors.Black,
                StrokeThickness = 2,
                TickHorizontalLength = 0,
                TickRadialLength = 0,
                InsideLabelPosition = 0.7,
                AngleSpan = 360,
                StartAngle = 0,
                OutsideLabelFormat = "",
                InsideLabelColor = OxyPlot.OxyColors.Black,
                InsideLabelFormat = "{1}",
                FontSize = 20
            };

            pieSeries.Slices.Clear();

            foreach(var month in priceByMonths)
            {
                pieSeries.Slices.Add(new PieSlice(month.Month, month.SumOfPriceByMonth) { Fill = OxyColors.WhiteSmoke });
            }

            PieGraff.Series.Add(pieSeries);
        }
        private void ReloadPie()

        {
            PieGraff.Series.Clear();
            priceByMonths.Clear();
            PieModelGraff();
            PieGraff.InvalidatePlot(true);
        }
        
        private void IncomeBarGraff()
        {
            IncomeByMonthBar.Series.Clear();
            IncomeByMonthBar.Title = "Столбиковая диаграмма прибыли по месяцам";
            var barSeries = new BarSeries
            {
                ItemsSource = new List<BarItem>(new[]
                {
                    new BarItem{ Value = 100 },
                    new BarItem{ Value = 200 },
                    new BarItem{ Value = 300 },
                    new BarItem{ Value = 400 },
                    new BarItem{ Value = 500 }  
                }),
                BarWidth = 100
            };
           

            IncomeByMonthBar.Series.Add(barSeries);
        }

        private void RejectionPercent()
        {
            PercentageOfRejections = $"{_Today.ToString("MMMM yyy")}";
        }
        private void GoBackPercentSours()
        {
            _Today = _Today.AddMonths(-1);
            RejectionPercent();
            LoadSourseToDataGrid();
        }
        private void GoForwardPercentSource()
        {
            _Today = _Today.AddMonths(1);
            RejectionPercent();
            LoadSourseToDataGrid();
        }

        private void SpendingAnalizeGraff()
        {
            SpendingAnalize.Series.Clear();
            SpendingAnalize.Title = "Анализ расходов по месяцам";
            var series = new BarSeries
            {
                BarWidth = 100
            };

            SpendingAnalize.Series.Add(series);
        }
    }
}
