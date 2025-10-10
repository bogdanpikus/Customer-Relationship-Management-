using System;
using System.Collections.ObjectModel;
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
        public ObservableCollection<OrdersPerDay> ordersPerDay { get; } = new(); // принимает вытягиваемые данные из базы данных в модель дата | колличество
        public PlotModel WeekPlotModel { get; set; } 
        public ICommand PreviousWeek {  get; set; }
        public ICommand NextWeek { get; set; }


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

            WeekPlotModel = new PlotModel();
            ExtractDataFromDatabase();
            WeekPlotModelGraff();
        }
        private void ExtractDataFromDatabase()
        {
            _db.SelectDataToWeekGraff(ordersPerDay);
        }
        private void WeekPlotModelGraff() //самый верхний левый графф
        {
            //WeekPlotModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)")); // пример создания графика
            WeekPlotModel.Series.Add(new LineSeries
            {
                Title = "Линия",
                Color = OxyPlot.OxyColors.Black,
                StrokeThickness = 2, 
                Points =
                {
                    // функция if, смотрим сколько за определенную дату заказов и подставляем значения (сколько заказов за дату, за какое число)
                    new DataPoint(0,0),
                    new DataPoint(1,1),
                    new DataPoint(2,3),
                }
            });

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
            WeekPlotModel.Axes.Add(yAxis);
            WeekPlotModel.Axes.Add(xAxis);
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
            UpdateTitle();
        }
        private void GoToNextWeek()
        {
            CenterOfWeek = CenterOfWeek.AddDays(1);
            UpdateTitle();
        }
    }
}
