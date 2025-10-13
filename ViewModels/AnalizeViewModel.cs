using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using CRM.Commands;
using CRM.Models;
using CRM.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CRM.ViewModels
{
    public class AnalizeViewModel: NotifyPropertyChange
    {
        private readonly DuckDatabase _db = DatabaseFactory.Instance;
        public ObservableCollection<OrdersPerDay> ordersPerDay { get; } = new(); // принимает вытягиваемые данные из базы данных в модель (дата | колличество заказов)
        public PlotModel WeekPlotModel { get; set; }
        public ICommand PreviousWeek {  get; set; }
        public ICommand NextWeek { get; set; }
        public ICommand TodayWeekGraff { get; set; }
        public ICommand ReloadWeekGraff { get; set; }


        private int _yAxisHeight = 5; // изначально высота оси Y = 5 с шагом 1
        private int YAxisHeight // присваеваем значение 10, если заказов становится или было вытянуто 5 или > 5 и т.д
        {
            get => _yAxisHeight;
            set
            {
                if(_yAxisHeight != value)
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
                if(_centerOfWeek != value)
                {
                    _centerOfWeek = value;
                    OnPropertyChange(nameof(CenterOfWeek));
                }
            }
        }

        public AnalizeViewModel()
        {
            PreviousWeek = new RelayCommand(Click => GoToPreviousWeek());
            NextWeek = new RelayCommand(Click => GoToNextWeek());
            TodayWeekGraff = new RelayCommand(Click => BackToTodayWeekGraff());
            ReloadWeekGraff = new RelayCommand(Click => ReloadGraffWeek());

            WeekPlotModel = new PlotModel();
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
        }

        private void ExtractDataFromDatabase()
        {
            ordersPerDay.Clear();
            _db.SelectDataToWeekGraff(ordersPerDay, CenterOfWeek.AddDays(-3), CenterOfWeek.AddDays(3)); // формирование данных типо: (дата | колличество заказов), в диапазоне []
        }
        private void WeekPlotModelGraff()
        {
            WeekPlotModel.Series.Clear();
            int maxOrdersValue = ordersPerDay.Max(x => x.Count);
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
                TrackerFormatString = "X = {2:0}, Y = {4:0}"
            };

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = YAxisHeight,
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
            foreach (var order in ordersPerDay)
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
            WeekPlotModel.Title = $"{startDate.ToString("dd.MM.yyy")} - {CenterOfWeek.ToString("d MMMM")} - {endDate.ToString("dd.MM.yyy")}";
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
        private void ReloadGraffWeek()
        {
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
            WeekPlotModel.InvalidatePlot(true);
        }
    }
}
