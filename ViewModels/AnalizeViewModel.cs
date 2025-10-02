using System;
using OxyPlot;
using OxyPlot.Series;

namespace CRM.ViewModels
{
    public class AnalizeViewModel
    {
        public PlotModel MyPlotModel { get; set; }
        public AnalizeViewModel()
        {
            MyPlotModel = new PlotModel { Title = "Test" };
            MyPlotModel.Series.Add(new LineSeries
            {
                ItemsSource = new List<DataPoint>
            {
                new DataPoint(0,0),
                new DataPoint(1,2),
                new DataPoint(2,4)
            }
            });
        }
    }
}
