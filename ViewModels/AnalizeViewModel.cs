using System;
using CRM.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace CRM.ViewModels
{
    public class AnalizeViewModel
    {
        public PlotModel WeekPlotModel { get; set; } 

        public AnalizeViewModel()
        {
            WeekPlotModel = new PlotModel { Title = "Заказы по деням за неделю" };

            var yAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 10,
                MajorStep = 1
            };
            var xAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                Minimum = 0,
                Maximum = 7,
                Title = "Дни недели",
                MajorStep = 1
            };

            WeekPlotModel.Axes.Add(yAxis);
            WeekPlotModel.Axes.Add(xAxis);
        }
    }
}
