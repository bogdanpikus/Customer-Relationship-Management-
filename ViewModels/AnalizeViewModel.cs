using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Windows.Input;
using CRM.Commands;
using CRM.Models;
using CRM.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CRM.ViewModels
{
    // REFUCTOR: rule SRP, вынос построение графиков в отдельные файлы в папку ChartsBuilder
    public class AnalizeViewModel : NotifyPropertyChange
    {
        private readonly SQLService _sqlService = new SQLService(); // сервис запросов SQL
        public ObservableCollection<RangeWithOrders> OrdersInRange { get; } = new();
        public ObservableCollection<PriceByMonth> PriceMonthCollection { get; } = new();
        public ObservableCollection<SourseCount> OrdersSourseCount { get; } = new();
        public ObservableCollection<YearIncome> YearIncomes { get; } = new();
        public ObservableCollection<YearSpendings> YearSpendings { get; } = new();
        public PlotModel WeekPlotModel { get; set; }
        public PlotModel PieGraff { get; set; }
        public PlotModel IncomeByMonthBar { get; set; }
        public PlotModel SpendingAnalize { get; set; }

        public ICommand PreviousWeek { get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand TodayWeekGraff { get; set; }
        public ICommand GoBackPerSource { get; set; }
        public ICommand GoForwardPerSource { get; set; }
        public ICommand ReloadPage { get; set; }

        private int? _maxOrdersValue;
        private int? maxOrdersValue
        {
            get => _maxOrdersValue;
            set
            {
                if (_maxOrdersValue != value)
                {
                    _maxOrdersValue = value;
                    OnPropertyChange(nameof(maxOrdersValue));
                }
            }
        }
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
                if (_today != value)
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
                if (_todayOrdersCount != value)
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
                if (_todayOrdersIncome != value)
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
                if (_monthOrderCount != value)
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
                if (_monthCancelledCount != value)
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
                if (_monthIncome != value)
                {
                    _monthIncome = value;
                    OnPropertyChange(nameof(MonthIncome));
                }
            }
        }
        private string? _monthSuccessCount;
        public string? MonthSuccessCount
        {
            get => _monthSuccessCount;
            set
            {
                if (_monthSuccessCount != value)
                {
                    _monthSuccessCount = value;
                    OnPropertyChange(nameof(MonthSuccessCount));
                }
            }
        }
        private string? _monthInProcessCount;
        public string? MonthInProcessCount
        {
            get => _monthInProcessCount;
            set
            {
                if (_monthInProcessCount != value)
                {
                    _monthInProcessCount = value;
                    OnPropertyChange(nameof(MonthInProcessCount));
                }
            }
        }
        private string? _monthDeliverCount;
        public string? MonthDeliverCount
        {
            get => _monthDeliverCount;
            set
            {
                if (_monthDeliverCount != value)
                {
                    _monthDeliverCount = value;
                    OnPropertyChange(nameof(MonthDeliverCount));
                }
            }
        }

        public AnalizeViewModel()
        {
            PreviousWeek = new RelayCommand(Click => GoToPreviousWeek());
            NextWeek = new RelayCommand(Click => GoToNextWeek());
            TodayWeekGraff = new RelayCommand(Click => BackToTodayWeekGraff());
            GoBackPerSource = new RelayCommand(Click => GoBackPercentSours());
            GoForwardPerSource = new RelayCommand(Click => GoForwardPercentSource());
            ReloadPage = new RelayCommand(Click => ReloadAnaliticsPage());

            WeekPlotModel = new PlotModel();
            PieGraff = new PlotModel();
            IncomeByMonthBar = new PlotModel();
            SpendingAnalize = new PlotModel();
            FieldTodayData();
            FieldMonthData();
            LoadSourseToDataGrid();
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
            PieModelGraff();
            IncomeBarGraff();
            SpendingAnalizeBar();
            RejectionPercent();
        }
        private void ReloadAnaliticsPage()
        {
            BackToTodayWeekGraff();
            ReloadPie();
            ReloadSourseDataGrid();
            FieldTodayData();
            FieldMonthData();
        }
        private void FieldMonthData()
        {
            var monthOrders = _sqlService.LoadMonthOrdersData(_Today.Month, _Today.Year);
            foreach (var order in monthOrders)
            {
                MonthOrderCount = $"Заказов за месяц: {order.MonthOrderCount}";
                MonthSuccessCount = $"Успешных за месяц: {order.MonthSuccessCount} ";
                MonthInProcessCount = $"Находятся в обработке: {order.MonthInProcessCount} ";
                MonthDeliverCount = $"Доставляются: {order.MonthDeliverCount}";
                MonthCancelledCount = $"Отказов за месяц: {order.MonthOrderRejections}";
                MonthIncome = $"Прибыль за месяц: {order.MonthIncome}";
            }
        }
        private void FieldTodayData()
        {
            var todayOrders = _sqlService.LoadTodayOrdersData();
            foreach (var item in todayOrders)
            {
                TodayOrdersCount = $"Заказов за сегодня: {item.TodayOrdersCount}";
                TodayOrdersIncame = $"Прибыль за сегодня: {item.TodayIncome}";
            }
        }

        private void CalculateYAxisAmount()
        {
            maxOrdersValue = OrdersInRange.Any() ? OrdersInRange.Max(x => x.Count) : 0;
            if (YAxisHeight < maxOrdersValue)
            {
                YAxisHeight = maxOrdersValue + 1;
            }
            else
            {
                YAxisHeight = 5;
            }
        }
        private void ExtractDataFromDatabase()
        {
            OrdersInRange.Clear();
            var ordersInRange = _sqlService.SelectDataToWeekGraff(CenterOfWeek.AddDays(-3), CenterOfWeek.AddDays(3));
            foreach (var order in ordersInRange)
            {
                OrdersInRange.Add(order);
            }
        }
        private void WeekPlotModelGraff()
        {
            // REFUCTOR: TrackerFormatString не работает
            WeekPlotModel.Series.Clear();
            CalculateYAxisAmount();

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
            foreach (var order in OrdersInRange)
            {
                int x = (int)((order.Date - CenterOfWeek.AddDays(-3)).TotalDays + 1);
                double y = order.Count;
                lineSeries.Points.Add(new DataPoint(x + 1, y));
            }
        }
        private void UpdateTitle()
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

        private void LoadPriceMonthPie()
        {
            var priceList = _sqlService.SelectAllPriceByMonth();
            foreach (var price in priceList)
            {
                PriceMonthCollection.Add(price);
            }
        }
        private void PieModelGraff()
        {
            PieGraff.Series.Clear();
            LoadPriceMonthPie();

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

            foreach (var month in PriceMonthCollection)
            {
                pieSeries.Slices.Add(new PieSlice(month.Month, month.SumOfPriceByMonth) { Fill = OxyColors.WhiteSmoke });
            }

            PieGraff.Series.Add(pieSeries);
        }
        private void ReloadPie()

        {
            PieGraff.Series.Clear();
            PriceMonthCollection.Clear();
            PieModelGraff();
            PieGraff.InvalidatePlot(true);
        }

        private void LoadSourseToDataGrid()
        {
            OrdersSourseCount.Clear();
            var sourseList = _sqlService.LoadSourseCountToDataGtid(_Today.Month, _Today.Year);
            foreach (var sourse in sourseList)
            {
                OrdersSourseCount.Add(sourse);
            }
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
            FieldMonthData();
        }
        private void GoForwardPercentSource()
        {
            _Today = _Today.AddMonths(1);
            RejectionPercent();
            LoadSourseToDataGrid();
            FieldMonthData();
        }
        private void ReloadSourseDataGrid()
        {
            DateTime _staticTodayMonth = DateTime.Now;
            _Today = _staticTodayMonth;
            LoadSourseToDataGrid();
            RejectionPercent();
        }

        private void SQLExtractYearIncome()
        {
            var yearIncome = _sqlService.ExtractYearIncome(); //Авг : 0 Окт: 1600 Нояб: 100 Дек: 0
            foreach (var month in yearIncome)
            {
                YearIncomes.Add(month);
            }
        }
        private void IncomeBarGraff()
        {
            // REFUCTOR: сделано через жопу просто чтобы перейти на следующий этап из-за дедлайна (100% переделать)
            SQLExtractYearIncome();
            IncomeByMonthBar.Series.Clear();
            IncomeByMonthBar.Axes.Clear();
            IncomeByMonthBar.Title = $"Столбиковая диаграмма прибыли по месяцам за {_Today.Year}";

            var month = YearIncomes.Select(m => m.Month).ToList();
            var income = YearIncomes.Select(m => new BarItem { Value = (double)m.MonthIncome }).ToList();

            IncomeByMonthBar.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = month
            });

            IncomeByMonthBar.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
            });

            var barSeries = new BarSeries
            {
                ItemsSource = income,
                StrokeThickness = 2,
                TrackerFormatString = "X = {0}, Y = {0}"
            };

            IncomeByMonthBar.Series.Add(barSeries);
        }

        private void SQLExtractYearSpendings()
        {
            var yearSpendings = _sqlService.ExtractYearSpendings();
            foreach(var month in yearSpendings)
            {
                YearSpendings.Add(month);
            }
        }
        private void SpendingAnalizeBar()
        {
            // REFUCTOR: сделано через жопу просто чтобы перейти на следующий этап из-за дедлайна (100% переделать)
            SQLExtractYearSpendings();
            SpendingAnalize.Series.Clear();
            SpendingAnalize.Axes.Clear();
            SpendingAnalize.Title = "Анализ расходов по месяцам";

            var month = YearSpendings.Select(m => m.Month).ToList();
            var spendings = YearSpendings.Select(m => new BarItem { Value = (double)m.MonthSpendings }).ToList();

            SpendingAnalize.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Left,
                ItemsSource = month
            });

            SpendingAnalize.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 1000
            });

            var series = new BarSeries
            {
                Title = "Расходы",
                StrokeThickness = 2,
                ItemsSource = spendings
            };

            SpendingAnalize.Series.Add(series);
        }
    }
}
