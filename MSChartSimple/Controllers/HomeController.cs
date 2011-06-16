using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Drawing;

namespace MSChartSimple.Controllers
{
    public class HomeController : Controller
    {
        private IList<DateTime> Today
        {
            get
            {
                var today = DateTime.Today;
                var dateTimes = new List<DateTime>();
                for (int i = 0; i < 24; i++)
                {
                    dateTimes.Add(new DateTime(today.Year, today.Month, today.Day, i, 0, 0));
                }
                return dateTimes;
            }
        }

        private IList<double> InValues
        {
            get
            {
                return new[] { 2.2, 2.3, 2.4, 2, 1.5, 1.8, 2.1, 2.0, 2.8 };
            }
        }
        private IList<double> OutValues
        {
            get
            {
                return new[] { 1.0, 1.3, 2.9, 1.4, 2.2, 2.3, 2.4, 2, 1.5 };
            }
        }
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        private void AddInSeries(Chart chart)
        {
            var series = chart.Series.Add("In");
            series.Color = Color.Blue;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            series.MarkerStyle = MarkerStyle.Circle;


            for (int i = 0; i < Today.Count; i++)
            {
                double y = double.NaN;
                if (i < InValues.Count)
                    y = InValues[i];
                series.Points.AddXY(Today[i], y);
            }


        }


        private void AddOutSeries(Chart chart)
        {
            var series = chart.Series.Add("Out");
            series.Color = Color.FromArgb(0, 201, 0);
            series.ChartType = SeriesChartType.Area;
            series.XValueType = ChartValueType.DateTime;
            for (int i = 0; i < Today.Count; i++)
            {
                double y = double.NaN;
                if (i < OutValues.Count)
                    y = OutValues[i];
                series.Points.AddXY(Today[i], y);
            }

        }

        private void AddTitle(Chart chart)
        {
            var t = new Title("IMG source streamed from Controller", Docking.Top,
                    new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(t);
        }

      
        private void AddLengend(Chart chart)
        {
            chart.Legends.Add("Legend");
        }


        private void AddChartArea(Chart chart)
        {
            var chartArea = chart.ChartAreas.Add("chart");
            var axisX = chartArea.AxisX;
            axisX.LabelStyle.Format = "HH:mm";
            axisX.ArrowStyle = AxisArrowStyle.Triangle;
            axisX.IsMarginVisible = false;
            //axisX.IsStartedFromZero = false;
            axisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //axisX.Interval = 1;
            axisX.IntervalType = DateTimeIntervalType.Hours;

            //chartArea.AxisX.IsLabelAutoFit = false;
            // chartArea.AxisX.IntervalOffsetType = DateTimeIntervalType.Minutes;

            // AddOutSeries(chart);
            AddInSeries(chart);
            AddLengend(chart);
        }

        private Chart GenChart()
        {
            var chart = new Chart();
            chart.Width = 800;
            chart.Height = 400;
            chart.RenderType = RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineWidth = 2;
            chart.BorderWidth = 2;
            chart.BackColor = Color.FromArgb(211, 223, 240);
            chart.BackGradientStyle = GradientStyle.TopBottom;
            chart.BorderColor = System.Drawing.Color.Black;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;

            AddChartArea(chart);

           
            return chart;
        }


        public FileResult GetChart()
        {
            var chart = GenChart();
            var imageStream = new MemoryStream();
            chart.SaveImage(imageStream, ChartImageFormat.Png);
            imageStream.Position = 0;
            return new FileStreamResult(imageStream, "image/png");
        }
    }

    public class StaticModel
    {
        public static List<int> createStaticData()
        {
            List<int> c_data = new List<int>();
            c_data.Add(1);
            c_data.Add(6);
            c_data.Add(4);
            c_data.Add(3);
            return c_data;
        }
    }
}
