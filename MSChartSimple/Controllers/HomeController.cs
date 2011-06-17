using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

using System.Drawing;
using System.Data.OleDb;


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
            ViewBag.Message = this.Server.MapPath("\\data");

            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        private void AddInSeries(Chart chart)
        {
            string path = this.Server.MapPath("\\data");
            var series = chart.Series.Add("In");
            series.Color = Color.Blue;
            series.ChartType = SeriesChartType.Line;
            series.XValueType = ChartValueType.DateTime;
            //  series.MarkerStyle = MarkerStyle.Circle;

            OleDbDataReader myReader = DataProvider.GetData(path);
            //  series.ChartArea = "0";
            series.Points.DataBindXY(myReader, myReader.GetName(1), myReader, myReader.GetName(2));
            myReader.Close();
            //for (int i = 0; i < Today.Count; i++)
            //{
            //    double y = double.NaN;
            //    if (i < InValues.Count)
            //        y = InValues[i];
            //    series.Points.AddXY(Today[i], y);
            //}
        }


        private void AddOutSeries(Chart chart)
        {
            var series = chart.Series.Add("Out");
            series.Color = Color.FromArgb(0, 201, 0);
            series.ChartType = SeriesChartType.Area;
            series.XValueType = ChartValueType.DateTime;
            OleDbDataReader myReader = DataProvider.GetData(this.Server.MapPath("\\data"));
            series.Points.DataBindXY(myReader, myReader.GetName(1), myReader, myReader.GetName(4));
            myReader.Close();

            //for (int i = 0; i < Today.Count; i++)
            //{
            //    double y = double.NaN;
            //    if (i < OutValues.Count)
            //        y = OutValues[i];
            //    series.Points.AddXY(Today[i], y);
            //}

        }

        /// <summary>
        /// 添加Title
        /// </summary>
        /// <param name="chart"></param>
        private void AddTitle(Chart chart)
        {
            var t = new Title("Utilization and Status graph", Docking.Top,
                    new Font("Trebuchet MS", 14, FontStyle.Bold), Color.FromArgb(26, 59, 105));
            chart.Titles.Add(t);
        }

        /// <summary>
        /// 添加说明
        /// </summary>
        /// <param name="chart"></param>
        private void AddLengend(Chart chart)
        {
            var legend = chart.Legends.Add("Legend");
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;

            // Add Color column
            LegendCellColumn firstColumn = new LegendCellColumn();
            firstColumn.ColumnType = LegendCellColumnType.SeriesSymbol;
            firstColumn.HeaderText = "Color";
            firstColumn.HeaderBackColor = Color.WhiteSmoke;
            legend.CellColumns.Add(firstColumn);

            // Add Legend Text column
            LegendCellColumn secondColumn = new LegendCellColumn();
            secondColumn.ColumnType = LegendCellColumnType.Text;
            secondColumn.HeaderText = "Name";
            secondColumn.Text = "#LEGENDTEXT";
            secondColumn.HeaderBackColor = Color.WhiteSmoke;
            legend.CellColumns.Add(secondColumn);

            // Add AVG cell column
            LegendCellColumn avgColumn = new LegendCellColumn();
            avgColumn.Text = "#AVG{N2}";
            avgColumn.HeaderText = "Avg";
            avgColumn.Name = "AvgColumn";
            avgColumn.HeaderBackColor = Color.WhiteSmoke;
            legend.CellColumns.Add(avgColumn);

            // Set Min cell column attributes
            LegendCellColumn minColumn = new LegendCellColumn();
            minColumn.Text = "#MAX{N1}";
            minColumn.HeaderText = "Max";
            minColumn.Name = "MAXColumn";
            minColumn.HeaderBackColor = Color.WhiteSmoke;
            legend.CellColumns.Add(minColumn);


        }

        /// <summary>
        /// 设置X轴
        /// </summary>
        /// <param name="axisX"></param>
        private void SetAxisX(Axis axisX)
        {
            axisX.LabelStyle.Format = "HH:mm";
            axisX.ArrowStyle = AxisArrowStyle.Triangle;
            axisX.IsMarginVisible = false;
            //axisX.IsStartedFromZero = false;
            axisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //axisX.Interval = 1;
            axisX.IntervalType = DateTimeIntervalType.Hours;
        }

        /// <summary>
        /// 设置Y轴
        /// </summary>
        /// <param name="axisY"></param>
        private void SetAxisY(Axis axisY)
        {
            axisY.ArrowStyle = AxisArrowStyle.Triangle;
        }


        private void AddChartArea(Chart chart)
        {
            var chartArea = chart.ChartAreas.Add("chart");

            //设置X轴
            SetAxisX(chartArea.AxisX);

            //设置Y轴
            SetAxisY(chartArea.AxisY);

            AddOutSeries(chart);
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

            AddTitle(chart);


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

}
