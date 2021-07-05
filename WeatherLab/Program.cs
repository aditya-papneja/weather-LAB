using System;
using System.Linq;


namespace WeatherLab
{
    public class DegreeData
    {
        public int Year { get; set; }
        public int HDD { get; set; }
        public int CDD { get; set; }
    }

    class Program
    {
        static string dbfile = @".\data\climate.db";

        static void Main(string[] args)
        {
            var measurements = new WeatherSqliteContext(dbfile).Weather;

            var dRF = measurements.Where(x => x.year == 2020).Sum(x => x.precipitation);

            var total_2020_precipitation = dRF / measurements.Where(x => x.year == 2020).Count();
            Console.WriteLine($"Total precipitation in 2020: {total_2020_precipitation} mm\n");

            //
            // Heating Degree days have a mean temp of < 18C
            //   see: https://en.wikipedia.org/wiki/Heating_degree_day
            //

            var data = measurements.GroupBy(x => x.year).Select(x => new
            DegreeData()
            {
                Year = x.Key,
                HDD = x.Where(x => x.meantemp < 18).Count(),
                CDD = x.Where(x => x.meantemp >= 18).Count()
            });

            // ?? TODO ??

            //
            // Cooling degree days have a mean temp of >=18C
            //

            // ?? TODO ??

            //
            // Most Variable days are the days with the biggest temperature
            // range. That is, the largest difference between the maximum and
            // minimum temperature
            //
            // Oh: and number formatting to zero pad.
            // 
            // For example, if you want:
            //      var x = 2;
            // To display as "0002" then:
            //      $"{x:d4}"
            //

            var vData = measurements.OrderByDescending(x => x.meantemp).Take(5).ToList();

            Console.WriteLine("Year\tHDD\tCDD");
            foreach (var x in data)
            {
                Console.WriteLine($"\n{x.Year}\t{x.HDD}\t{x.CDD}");
            }

            // ?? TODO ??

            Console.WriteLine("\nTop 5 Most Variable Days");
            Console.WriteLine("YYYY-MM-DD\tDelta");
            foreach (var x in vData)
            {
                Console.WriteLine($"\n{x.year}-{x.month:d2}-{x.day:d2}\t{x.meantemp}");
            }


            // ?? TODO ??
        }
    }
}
