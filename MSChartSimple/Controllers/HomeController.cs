using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.DataVisualization.Charting;
using System.IO;

namespace MSChartSimple.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        private Chart GenChart()
        {
            List<int> data = StaticModel.createStaticData();
            var chart = new Chart();
            chart.Width = 800;
            chart.Height = 400;
            chart.RenderType = System.Web.UI.DataVisualization.Charting.RenderType.ImageTag;
            chart.Palette = ChartColorPalette.BrightPastel;
            Title t = new Title("IMG source streamed from Controller", Docking.Top, new System.Drawing.Font("Trebuchet MS", 14, System.Drawing.FontStyle.Bold), System.Drawing.Color.FromArgb(26, 59, 105));
            chart.Titles.Add(t);
            chart.ChartAreas.Add("Series 1");
            // create a couple of series   
            chart.Series.Add("Series 1");
            chart.Series.Add("Series 2");
            // add points to series 1   
            foreach (int value in data)
            {
                chart.Series["Series 1"].Points.AddY(value);
            }
            // add points to series 2   
            foreach (int value in data)
            {
                chart.Series["Series 2"].Points.AddY(value + 1);
            }
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.BorderlineWidth = 2;
            chart.BorderColor = System.Drawing.Color.Black;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderWidth = 2;
            chart.Legends.Add("Legend1");

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
