using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.OleDb;
using System.Data;
using System.Collections;

namespace MSChartSimple.Controllers
{
    public class DataProvider
    {

        static string file = "DataFile.csv";



        public static OleDbDataReader GetData(string path)
        {

         //   path=@"D:\Workspace\MSChartSimple\MSChartSimple\data";
            // Create a select statement and a connection string.
            string mySelectQuery = "Select * from " + file;
            string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=\"Text;HDR=No;FMT=Delimited\"";

            OleDbConnection myConnection = new OleDbConnection(ConStr);

            // create a database command on the connection using query
            OleDbCommand myCommand = new OleDbCommand(mySelectQuery, myConnection);

            // open the connection
            myCommand.Connection.Open();
            // create a database reader
            OleDbDataReader myReader = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            //myReader.Close();
            // myConnection.Close();

            return myReader;
        }

        public static IList<InOutModel> InValues
        {
            get
            {
                var ins = new[] { 2.2, 2.3, 2.4, 2, 1.5, 1.8, 2.1, 2.0, 2.8 };
                var inValues = new List<InOutModel>();
                for (int i = 0; i < Today.Count; i++)
                {
                    double y = double.NaN;
                    if (i < ins.Count())
                        y = ins[i];
                    inValues.Add(new InOutModel()
                    {
                        DateTime = Today[i],
                        Value = y
                    });
                }
                return inValues;
            }
        }


        private static IList<DateTime> Today
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
    }
}