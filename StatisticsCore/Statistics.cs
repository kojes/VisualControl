using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using MathNet.Numerics;


namespace VueExample.StatisticsCore
{

    public class Statistics
    {
        public string ExpectedValue { get; set; }
        public string Maximum { get; set; }
        public string Minimum { get; set; }
        public string StandartDeviation { get; set; }
        public string StatisticsName { get; set; }
        public string Unit { get; set; }
        public string Median { get; set; }
        public int ParameterID { get; set; }
        public List<double> FullList { get; set; }

      

        public Statistics()
        {

        }

        public List<Statistics> GetStatistics(List<string> xList, List<List<string>> commonYList, VueExample.Models.SRV6.Graphic graphics, double divider)
        {
            var thisType = GetType();
            if (String.IsNullOrEmpty(graphics.StatisticsFunction))
                return new List<Statistics>();
            var theMethod = thisType.GetMethod(graphics.StatisticsFunction, BindingFlags.Instance | BindingFlags.NonPublic);
            if (theMethod == null)
            {
                return new List<Statistics>();
            }
            var st = theMethod.Invoke(this, new object[] { xList, commonYList, divider }) as List<Statistics>;
            return st;
        }


        public List<Statistics> GetStatisticsHistogram(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {
            var thisType = GetType();
            if (String.IsNullOrEmpty(graphics.StatisticsFunction))
                return new List<Statistics>();
            var theMethod = thisType.GetMethod(graphics.StatisticsFunction, BindingFlags.Instance | BindingFlags.NonPublic);
            if (theMethod == null)
            {
                return new List<Statistics>();
            }
            var st = theMethod.Invoke(this, new object[] { valueList, graphics }) as List<Statistics>;
            return st;
        }

        public List<Statistics> GetStatistics(List<double> xList, List<List<double>> commonYList, VueExample.Models.SRV6.Graphic graphics, double divider, string type)
        {
            var thisType = GetType();
            if (String.IsNullOrEmpty(graphics.StatisticsFunction))
                return new List<Statistics>();
            var theMethod = thisType.GetMethod(graphics.StatisticsFunction, BindingFlags.Instance | BindingFlags.NonPublic);
            if (theMethod == null)
            {
                return new List<Statistics>();
            }
            var st = theMethod.Invoke(this, new object[] { xList, commonYList, divider, type }) as List<Statistics>;
            return st;
        }

        public string GetStatistics(List<double> xList, List<List<double>> commonYList, string statisticsfunction, string type, Dictionary<string,double> points )
        {
            var thisType = GetType();
            if (String.IsNullOrEmpty(statisticsfunction))
                return String.Empty;
            var theMethod = thisType.GetMethod(statisticsfunction, BindingFlags.Instance | BindingFlags.NonPublic);
            if (theMethod == null)
            {
                return String.Empty;
            }
            var st = theMethod.Invoke(this, new object[] { xList, commonYList, type, points }) as String;
            return st;
        }


        public string GetStatistics(List<double> xList, Dictionary<string, List<List<double>>> commonYList, string statisticsfunction, Dictionary<string,string> type, Dictionary<string, double> points)
        {
            var thisType = GetType();
            if (String.IsNullOrEmpty(statisticsfunction))
                return String.Empty;
            var theMethod = thisType.GetMethod(statisticsfunction, BindingFlags.Instance | BindingFlags.NonPublic);
            if (theMethod == null)
            {
                return String.Empty;
            }
            var st = theMethod.Invoke(this, new object[] { xList, commonYList, type, points }) as String;
            return st;
        }




        public static Statistics GetStatisticsAverageFull(List<Statistics> list)
        {

            var diffstat = new Statistics
            {
                Minimum = GetAverageFromFullStatistics(list, "Minimum"),
                ExpectedValue = GetAverageFromFullStatistics(list, "ExpectedValue"),
                Maximum = GetAverageFromFullStatistics(list, "Maximum"),
                StandartDeviation = GetAverageFromFullStatistics(list, "StandartDeviation"),
                Median = GetAverageFromFullStatistics(list, "Median")
            };
            return diffstat;
        }

        public static Statistics GetStatisticsSDFull(List<Statistics> list)
        {

            var diffstat = new Statistics
            {
                Minimum = GetSDFromFullStatistics(list, "Minimum"),
                ExpectedValue = GetSDFromFullStatistics(list, "ExpectedValue"),
                Maximum = GetSDFromFullStatistics(list, "Maximum"),
                StandartDeviation = GetSDFromFullStatistics(list, "StandartDeviation"),
                Median = GetSDFromFullStatistics(list, "Median")
            };
            return diffstat;
        }

      

      

        public List<Statistics> GetStatistics(List<string> list, VueExample.Models.SRV6.Graphic graphics, VueExample.Models.SRV6.StatisticParameter statisticParameter = null)
        {
            var thisType = GetType();
            var theMethod = thisType.GetMethod("GetSingleStatistics", BindingFlags.Instance | BindingFlags.NonPublic);
            if (theMethod == null)
            {
                return new List<Statistics>();
            }
            var st = theMethod.Invoke(this, new object[] { list, graphics, statisticParameter }) as List<Statistics>;
            return st;
        }

        public static Statistics GetDifferenceBetweenTwoStatistics(Statistics main, Statistics second)
        {
            var diffstat = new Statistics
            {
                Minimum = GetDifferenceBetweenTwoStatisticsParams(main.Minimum, second.Minimum),
                ExpectedValue = GetDifferenceBetweenTwoStatisticsParams(main.ExpectedValue, second.ExpectedValue),
                Maximum = GetDifferenceBetweenTwoStatisticsParams(main.Maximum, second.Maximum),
                StandartDeviation = GetDifferenceBetweenTwoStatisticsParams(main.StandartDeviation, second.StandartDeviation),
                Median = GetDifferenceBetweenTwoStatisticsParams(main.Median, second.Median)
            };
            return diffstat;
        }

        private static string GetDifferenceBetweenTwoStatisticsParams(string main, string second)
        {
            var returnstring = String.Empty;
            var difference = double.Parse(main) - double.Parse(second);
            if (difference < 0)
            {
                returnstring = "<br><span class=\"differencespan\" style=\"display:none\">" + "<font color=\"#238F23\">" + ToStringD(Math.Abs(difference)) + "↑" + "</font>" + "</span></br>";
            }
            else
            {
                returnstring = "<br><span class=\"differencespan\" style=\"display:none\">" + "<font color=\"#8F2323\">" + ToStringD(Math.Abs(difference)) + "↓" + "</font>" + "</span></br>";
            }
            return returnstring;

        }

        private static string GetAverageFromFullStatistics(IEnumerable<Statistics> list, string propertyname)
        {

            return CalculateExpectedValue(list.Select(statisticse => double.Parse(Convert.ToString(statisticse.GetType().GetProperty(propertyname).GetValue(statisticse, null)))).ToList());


        }

        private static string GetSDFromFullStatistics(IEnumerable<Statistics> list, string propertyname)
        {

            return CalculateStandartDeviation(list.Select(statisticse => double.Parse(Convert.ToString(statisticse.GetType().GetProperty(propertyname).GetValue(statisticse, null)))).ToList());


        }

        
        private static string CalculateExpectedValue(IEnumerable<double> list)
        {
            var enumerable = list as IList<double> ?? list.ToList();
            if (enumerable.Count == 0)
            {
                return String.Empty;
            }
            var average = enumerable.Average();
            if ((Math.Abs(average) >= 10000 || Math.Abs(average) < 1E-2) && Math.Abs(average - 0) > 1E-20)
            {
                return average.ToString("0.00E0");
            }
            return average.ToString("0.000");
        }

        private static string ToStringD(double d)
        {
            if ((Math.Abs(d) >= 10000 || Math.Abs(d) < 1E-2) && Math.Abs(d - 0) > 1E-20)
            {
                return d.ToString("0.00E0");
            }
            return d.ToString("0.000");
        }

        private static string CalculateMedian(IEnumerable<double> list)
        {
            var enumerable = list as IList<double> ?? list.ToList();
            var median = MathNet.Numerics.Statistics.Statistics.Median(enumerable);
            if ((Math.Abs(median) >= 10000 || Math.Abs(median) < 1E-2) && Math.Abs(median - 0) > 1E-20)
            {
                return median.ToString("0.00E0");
            }
            return median.ToString("0.000");
        }

        private static string CalculateStandartDeviation(List<double> list)
        {

            var enumerable = list as IList<double> ?? list.ToList();
            if (enumerable.Count == 0)
            {
                return String.Empty;
            }
            var avg = list.Average();
            //var standartdeviation = Math.Sqrt(list.Average(v => Math.Pow(v - avg, 2)));
            var standartdeviation = MathNet.Numerics.Statistics.Statistics.StandardDeviation(list);
            if (Double.IsNaN(standartdeviation))
            {
                standartdeviation = 0.0;
            }
            if ((Math.Abs(standartdeviation) >= 10000 || Math.Abs(standartdeviation) < 1E-2) && Math.Abs(standartdeviation - 0) > 1E-20)
            {
                return standartdeviation.ToString("0.00E0");
            }
            return standartdeviation.ToString("0.000");


        }

        private static string CalculateMinimum(IEnumerable<double> list)
        {


            var enumerable = list as IList<double> ?? list.ToList();
            if (enumerable.Count == 0)
            {
                return String.Empty;
            }
            var minimum = enumerable.Min();
            if ((Math.Abs(minimum) >= 10000 || Math.Abs(minimum) < 1E-2) && Math.Abs(minimum - 0) > 1E-20)
            {
                return minimum.ToString("0.00E0");
            }
            return minimum.ToString("0.000");
        }

        private static string CalculateMaximum(IEnumerable<double> list)
        {
            var enumerable = list as IList<double> ?? list.ToList();
            if (enumerable.Count == 0)
            {
                return String.Empty;
            }
            var maximum = enumerable.Max();
            if ((Math.Abs(maximum) >= 10000 || Math.Abs(maximum) < 1E-2) && Math.Abs(maximum - 0) > 1E-20)
            {
                return maximum.ToString("0.00E0");
            }
            return maximum.ToString("0.000");
        }


        private Statistics GetFullStatisticsFromList(List<double> list, string statisticsname, string unit, int parameterid = 0)
        {
            var denanlist = DeNanList(list);
            var statisitics = new Statistics
                {
                    Maximum = CalculateMaximum(denanlist),
                    Minimum = CalculateMinimum(denanlist),
                    StandartDeviation = CalculateStandartDeviation(denanlist),
                    StatisticsName = statisticsname,
                    ExpectedValue = CalculateExpectedValue(denanlist),
                    Unit = unit,
                    Median = CalculateMedian(denanlist),
                    FullList = list.ToList(),
                    ParameterID = parameterid
                };
            return statisitics;
        }

        private static List<double> DeNanList(IEnumerable<double> list)
        {
            return list.Where(d => !double.IsNaN(d)).ToList();
        }

        private List<Statistics> GetIdssAndUgsoff(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.0 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.5 - item)).FirstOrDefault());
            var idssList = new List<double>();
            var id05List = new List<double>();
            var ugsoffwithinterpolationList = new List<double>();
            var ugsoffwithinterpolation100List = new List<double>();
            var ugsoffList = new List<double>();
            var ugsoffminList = new List<double>();
            var unit = "A";
            if (Math.Abs(divider - 1) > 1E-6)
            {
                unit = "A/мм";
            }
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var isugsoff = false;
                idssList.Add(yListdouble[zeroIndex]);
                id05List.Add(yListdouble[fiveIndex]);
                ugsoffminList.Add(xListdouble[yListdouble.IndexOf(yListdouble.Min())]);
                for (int i = yListdouble.Count - 1; i > -1; i--)
                {

                    if (yListdouble[i] < yListdouble[zeroIndex] / 1000)
                    {
                        if (i < yListdouble.Count - 1)
                        {
                            var xtwopointList = new List<double> {xListdouble[i], xListdouble[i + 1]};
                            var ytwopointList = new List<double> {yListdouble[i], yListdouble[i + 1]};
                            var interpolationmethod = Interpolate.Linear(ytwopointList, xtwopointList);
                            ugsoffwithinterpolationList.Add(interpolationmethod.Interpolate(yListdouble[zeroIndex] / 1000));
                        
                            ugsoffList.Add((xListdouble[i] + xListdouble[i + 1]) / 2);
                            isugsoff = true;
                        }
                        else
                        {
                            ugsoffwithinterpolationList.Add(xListdouble[i]);
                            ugsoffList.Add(xListdouble[i]);
                            isugsoff = true;
                        }

                        i = -1;
                    }

                }

                for (int i = yListdouble.Count - 1; i > -1; i--)
                {

                    if (yListdouble[i] < yListdouble[zeroIndex] / 100)
                    {
                        if (i < yListdouble.Count - 1)
                        {
                            var xtwopointList = new List<double> { xListdouble[i], xListdouble[i + 1] };
                            var ytwopointList = new List<double> { yListdouble[i], yListdouble[i + 1] };
                            var interpolationmethod100 = Interpolate.Linear(ytwopointList, xtwopointList);
                            ugsoffwithinterpolation100List.Add(interpolationmethod100.Interpolate(yListdouble[zeroIndex] / 100));

                            
                          
                        }
                        else
                        {
                            ugsoffwithinterpolation100List.Add(xListdouble[i]);
                          
                        }

                        i = -1;
                    }

                }
                if (!isugsoff)
                {
                    ugsoffList.Add(double.NaN);
                    ugsoffwithinterpolationList.Add(double.NaN);
                }



            }
            if (ugsoffList.Count(x => Double.IsNaN(x)) == ugsoffList.Count())
            {
                foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
                {
                    ugsoffList.Clear();
                    var isugsoff = false;
                    for (int i = yListdouble.Count - 1; i > -1; i--)
                    {

                        if (yListdouble[i] < yListdouble[zeroIndex] / 100)
                        {
                            if (i < yListdouble.Count - 1)
                            {
                                ugsoffList.Add((xListdouble[i] + xListdouble[i + 1]) / 2);
                                var xtwopointList = new List<double> { xListdouble[i], xListdouble[i + 1] };
                                var ytwopointList = new List<double> { yListdouble[i], yListdouble[i + 1] };
                                var interpolationmethod = Interpolate.Linear(ytwopointList, xtwopointList);
                                ugsoffwithinterpolationList.Add(interpolationmethod.Interpolate(yListdouble[zeroIndex] / 100));
                                isugsoff = true;
                            }
                            else
                            {
                                ugsoffwithinterpolationList.Add(xListdouble[i]);
                                ugsoffList.Add(xListdouble[i]);
                                isugsoff = true;
                            }

                            i = -1;
                        }
                    }
                    if (!isugsoff)
                    {
                        ugsoffList.Add(double.NaN);
                        ugsoffwithinterpolationList.Add(double.NaN);
                    }



                }

                var returnexList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "I<sub>DSS(3V)</sub> (начальный ток стока)", unit, 3),
                    GetFullStatisticsFromList(ugsoffwithinterpolationList, "U<sub>GS(off)</sub> (напряжение отсечки при Idss/100) !Транзисторы не закрываются!", "В", 4),
                    GetFullStatisticsFromList(id05List, "Id<sub>max</sub> (ток при Vgs=0.5В)", "А"),
                    GetFullStatisticsFromList(ugsoffminList, "U<sub>GS(min)</sub> (напряжение отсечки при Imin)", "В"),

                };
                return returnexList;

            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "I<sub>DSS(3V)</sub> (начальный ток стока)", unit, 3),
                    GetFullStatisticsFromList(ugsoffwithinterpolationList, "U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", "В", 4),
                    GetFullStatisticsFromList(id05List, "Id<sub>max</sub> (ток при Vgs=0.5В)", "А"),
                    GetFullStatisticsFromList(ugsoffwithinterpolation100List, "U<sub>GS-100(off)</sub> (напряжение отсечки при Idss/100)", "В"),
                    GetFullStatisticsFromList(ugsoffminList, "U<sub>GS(min)</sub> (напряжение при Imin)", "В"),

                };
            return returnList;
        }

        private List<Statistics> GetIdssAndUgsoff_Progress(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.0 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.5 - item)).FirstOrDefault());
            var idssList = new List<double>();
            var id05List = new List<double>();
            var ugsoffwithinterpolationList = new List<double>();
            var ugsoffwithinterpolation100List = new List<double>();
            var ugsoffList = new List<double>();
            var ugsoffminList = new List<double>();
            var unit = "A";
            if (Math.Abs(divider - 1) > 1E-6)
            {
                unit = "A/мм";
            }

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {

                idssList.Add(yListdouble[zeroIndex] * 1.1 / divider);
               
            }

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var isugsoff = false;
              
                id05List.Add(yListdouble[fiveIndex]);
                ugsoffminList.Add(xListdouble[yListdouble.IndexOf(yListdouble.Min())]);
                for (int i = yListdouble.Count - 1; i > -1; i--)
                {

                    if (yListdouble[i] < yListdouble[zeroIndex] / 1000)
                    {
                        if (i < yListdouble.Count - 1)
                        {
                            var xtwopointList = new List<double> { xListdouble[i], xListdouble[i + 1] };
                            var ytwopointList = new List<double> { yListdouble[i], yListdouble[i + 1] };
                            var interpolationmethod = Interpolate.Linear(ytwopointList, xtwopointList);
                            ugsoffwithinterpolationList.Add(interpolationmethod.Interpolate(yListdouble[zeroIndex] / 1000));

                            ugsoffList.Add((xListdouble[i] + xListdouble[i + 1]) / 2);
                            isugsoff = true;
                        }
                        else
                        {
                            ugsoffwithinterpolationList.Add(xListdouble[i]);
                            ugsoffList.Add(xListdouble[i]);
                            isugsoff = true;
                        }

                        i = -1;
                    }

                }

                for (int i = yListdouble.Count - 1; i > -1; i--)
                {

                    if (yListdouble[i] < yListdouble[zeroIndex] / 100)
                    {
                        if (i < yListdouble.Count - 1)
                        {
                            var xtwopointList = new List<double> { xListdouble[i], xListdouble[i + 1] };
                            var ytwopointList = new List<double> { yListdouble[i], yListdouble[i + 1] };
                            var interpolationmethod100 = Interpolate.Linear(ytwopointList, xtwopointList);
                            ugsoffwithinterpolation100List.Add(interpolationmethod100.Interpolate(yListdouble[zeroIndex] / 100));



                        }
                        else
                        {
                            ugsoffwithinterpolation100List.Add(xListdouble[i]);

                        }

                        i = -1;
                    }

                }
                if (!isugsoff)
                {
                    ugsoffList.Add(double.NaN);
                    ugsoffwithinterpolationList.Add(double.NaN);
                }



            }
            if (ugsoffList.Count(x => Double.IsNaN(x)) == ugsoffList.Count())
            {
                foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
                {
                    ugsoffList.Clear();
                    var isugsoff = false;
                    for (int i = yListdouble.Count - 1; i > -1; i--)
                    {

                        if (yListdouble[i] < yListdouble[zeroIndex] / 100)
                        {
                            if (i < yListdouble.Count - 1)
                            {
                                ugsoffList.Add((xListdouble[i] + xListdouble[i + 1]) / 2);
                                var xtwopointList = new List<double> { xListdouble[i], xListdouble[i + 1] };
                                var ytwopointList = new List<double> { yListdouble[i], yListdouble[i + 1] };
                                var interpolationmethod = Interpolate.Linear(ytwopointList, xtwopointList);
                                ugsoffwithinterpolationList.Add(interpolationmethod.Interpolate(yListdouble[zeroIndex] / 100));
                                isugsoff = true;
                            }
                            else
                            {
                                ugsoffwithinterpolationList.Add(xListdouble[i]);
                                ugsoffList.Add(xListdouble[i]);
                                isugsoff = true;
                            }

                            i = -1;
                        }
                    }
                    if (!isugsoff)
                    {
                        ugsoffList.Add(double.NaN);
                        ugsoffwithinterpolationList.Add(double.NaN);
                    }



                }

                var returnexList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "I<sub>DSS(3V)</sub> (начальный ток стока)", unit, 3),
                    GetFullStatisticsFromList(ugsoffwithinterpolationList, "U<sub>GS(off)</sub> (напряжение отсечки при Idss/100) !Транзисторы не закрываются!", "В", 4),
                    GetFullStatisticsFromList(id05List, "Id<sub>max</sub> (ток при Vgs=0.5В)", "А"),
                    GetFullStatisticsFromList(ugsoffminList, "U<sub>GS(min)</sub> (напряжение отсечки при Imin)", "В"),

                };
                return returnexList;

            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "I<sub>DSS(3V)</sub> (начальный ток стока)", unit, 3),
                    GetFullStatisticsFromList(ugsoffwithinterpolationList, "U<sub>GS(off)</sub> (напряжение отсечки при Idss/1000)", "В", 4),
                    GetFullStatisticsFromList(id05List, "Id<sub>max</sub> (ток при Vgs=0.5В)", "А"),
                    GetFullStatisticsFromList(ugsoffwithinterpolation100List, "U<sub>GS-100(off)</sub> (напряжение отсечки при Idss/100)", "В"),
                    GetFullStatisticsFromList(ugsoffminList, "U<sub>GS(min)</sub> (напряжение при Imin)", "В"),

                };
            return returnList;
        }

        private List<Statistics> GetLR(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.05 - item)).FirstOrDefault());
            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>ind</sub> при U=0.05В (сопротивление катушки)", "Ом"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetHarmony3(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();
            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(minList, "Harmony_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "Harmony_DELTA", "дБм"),

                };
            return returnList;
        }

        private List<Statistics> GetHarmony(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();
            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList, "Harmony_MAX", "дБм"),
                    GetFullStatisticsFromList(deltaList, "Harmony_DELTA", "дБм"),

                };
            return returnList;
        }

        private List<Statistics> GetALFA1(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();


            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList.Select(Math.Abs).ToList(), "ALFA1_MAX", "дБм", 48),
                    GetFullStatisticsFromList(minList.Select(Math.Abs).ToList(), "ALFA1_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "ALFA1_DELTA", "дБм"),

                };
            return returnList;
        }

        private List<Statistics> GetALFA2(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();
            
            
            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList.Select(Math.Abs).ToList(), "ALFA2_MAX", "дБм", 46),
                    GetFullStatisticsFromList(minList.Select(Math.Abs).ToList(), "ALFA2_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "ALFA2_DELTA", "дБм"),

                };
            return returnList;
        }

        private List<Statistics> GetALFA3(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();
          
            
            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList.Select(Math.Abs).ToList(), "ALFA3_MAX", "дБм", 49),
                    GetFullStatisticsFromList(minList.Select(Math.Abs).ToList(), "ALFA3_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "ALFA3_DELTA", "дБм"),

                };
            return returnList;
        }


        private List<Statistics> GetALFA4(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();
            
            

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList.Select(Math.Abs).ToList(), "ALFA4_MAX", "дБм", 47),
                    GetFullStatisticsFromList(minList.Select(Math.Abs).ToList(), "ALFA4_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "ALFA4_DELTA", "дБм"),

                };
            return returnList;
        }

        private List<Statistics> GetALFA5(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();


            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList.Select(Math.Abs).ToList(), "ALFA5_MAX", "дБм"),
                    GetFullStatisticsFromList(minList.Select(Math.Abs).ToList(), "ALFA5_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "ALFA5_DELTA", "дБм"),

                };
            return returnList;
        }


        private List<Statistics> GetCL(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var maxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var minList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Min()).ToList();
            var deltaList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => Math.Abs(yListdouble.Max() - yListdouble.Min())).ToList();


            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(maxList, "ConversionLoss_MAX", "дБм", 45),
                    GetFullStatisticsFromList(minList, "ConversionLoss_MIN", "дБм"),
                    GetFullStatisticsFromList(deltaList, "ConversionLoss_DELTA", "дБм"),

                };
            return returnList;
        }



        private List<Statistics> GetIdssAndUgsoffX5(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.0 - item)).FirstOrDefault());
            var ugsoffList = new List<double>();


            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var isugsoff = false;
                for (int i = yListdouble.Count - 1; i > -1; i--)
                {

                    if (yListdouble[i] < yListdouble[zeroIndex] / 1000)
                    {
                        if (i < yListdouble.Count - 1)
                        {
                            ugsoffList.Add((xListdouble[i] + xListdouble[i + 1]) / 2);
                            isugsoff = true;
                        }
                        else
                        {
                            ugsoffList.Add(xListdouble[i]);
                            isugsoff = true;
                        }

                        i = -1;
                    }
                }
                if (!isugsoff)
                {
                    ugsoffList.Add(double.NaN);
                }



            }
            if (ugsoffList.Count(x => Double.IsNaN(x)) == ugsoffList.Count())
            {
                foreach (
                    var yListdouble in
                        commonYList.Select(
                            yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)/divider).ToList()))
                {
                    var isugsoff = false;
                    for (int i = yListdouble.Count - 1; i > -1; i--)
                    {
                        if (yListdouble[i] < yListdouble[zeroIndex]/10)
                        {
                            if (i < yListdouble.Count - 1)
                            {
                                ugsoffList.Add((xListdouble[i] + xListdouble[i + 1])/2);
                                isugsoff = true;
                            }
                            else
                            {
                                ugsoffList.Add(xListdouble[i]);
                                isugsoff = true;
                            }

                            i = -1;
                        }

                    }
                    if (!isugsoff)
                    {
                        ugsoffList.Add(double.NaN);
                    }


                }
                var returnexList = new List<Statistics>
                {
                   
                    GetFullStatisticsFromList(ugsoffList, "Все транзисторы не закрываются! Расчет отсечки при Idss/10", "", 4),
                 

                };
                return returnexList;

            }

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(ugsoffList, "U<sub>gs(off)</sub>", "В", 4)
                };
            return returnList;
        }


        private List<Statistics> GetSingleStatistics(List<string> list, VueExample.Models.SRV6.Graphic graphics, VueExample.Models.SRV6.StatisticParameter statisticParameter)
        {

            var listdouble = list.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var statname = graphics.Ordinate;
            var statID = 0;
            if (statisticParameter != null)
            {
                statname = $"{statisticParameter.HTMLName} ({statisticParameter.Description})";
                statID = statisticParameter.Id;
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, statname, graphics.OrdinateUnit, statID),
                };
            return returnList;
        }

        private List<Statistics> GetRDivided01(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {

            var listdouble = valueList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)*10).ToList();
            var statname = graphics.Ordinate;
            var statID = 0;

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, "Удельное " + statname, graphics.OrdinateUnit +  "/кв", statID),
                };
            return returnList;
        }

        private List<Statistics> GetRDivided01_1DOT1(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {

            var listdouble = valueList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) * 10 / 1.1).ToList();
            var statname = graphics.Ordinate;
            var statID = 0;

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, "Удельное " + statname, graphics.OrdinateUnit +  "/кв", statID),
                };
            return returnList;
        }

        private List<Statistics> GetRDivided_NO(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {

            var listdouble = valueList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var statname = graphics.Ordinate;
            var statID = 0;

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, "Удельное " + statname, graphics.OrdinateUnit +  "/кв", statID),
                };
            return returnList;
        }

       

        private List<Statistics> GetRDivided10(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {

            var listdouble = valueList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / 10).ToList();
            var statname = graphics.Ordinate;
            var statID = 0;

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, "Удельное " + statname, graphics.OrdinateUnit +  "/кв", statID),
                };
            return returnList;
        }

        private List<Statistics> GetRDivided10_1DOT23(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {

            var listdouble = valueList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / 10 / 1.23).ToList();
            var statname = graphics.Ordinate;
            var statID = 0;

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, "Удельное " + statname, graphics.OrdinateUnit + "/кв", statID),
                };
            return returnList;
        }

        private List<Statistics> GetRDivided10_1DOT22(List<string> valueList, VueExample.Models.SRV6.Graphic graphics)
        {

            var listdouble = valueList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / 10 / 1.22).ToList();
            var statname = graphics.OrdinateUnit;
            var statID = 0;

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(listdouble, "Удельное " + statname, graphics.OrdinateUnit + "/кв", statID),
                };
            return returnList;
        }




        private List<Statistics> GetIdss(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {

            int zeroIndex = xList.IndexOf("0");
            var idssList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "I<sub>dss</sub> (ток при Uзи = 0В, Uси = 10В)", "А", 3),
                   
                };
            return returnList;
        }


        private List<Statistics> GetMongoFirstPoint(List<double> xListdouble, List<List<double>> commonYList, double divider, string type)
        {

            
            var idssList = commonYList.Select(yList => yList.Select(x => x / divider).ToList()).Select(yListdouble => yListdouble[0]).ToList();

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "FirstPoint " + type , "", 3),

                };
            return returnList;
        }

        private List<Statistics> GetIdssVknee(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {

            int maxIndex = xList.IndexOf("0");
            var idssList = new List<double>();
            var ronList = new List<double>();
            var vkneeList = new List<double>();
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.5 - item)).FirstOrDefault());
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                idssList.Add(yListdouble.Max());
                vkneeList.Add(xListdouble[yListdouble.IndexOf(yListdouble.Max())]);
                ronList.Add(xListdouble[tenIndex]/yListdouble[tenIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(idssList, "Idss", "А", 3),
                    GetFullStatisticsFromList(vkneeList, "Vknee", "В"),
                    GetFullStatisticsFromList(ronList, "Ron", "Ом", 12)
                   
                };
            return returnList;
        }


        private List<Statistics> GetRon(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            int zeroIndex = 0;
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            for (int index = 0; index < xListdouble.Count; index++)
            {
                double d = xListdouble[index];
                if (Math.Abs(d - 0.02) < 0.005)
                {
                    zeroIndex = index;
                }

            }
          
            var ronList = new List<double>();
            if (divider < 1)
            {
                divider = 1 / divider;
            }
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                ronList.Add(yListdouble[zeroIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ronList, "r<sub>DS(on)</sub> (сопротивление открытого канала при Uси = 0.02В)", "Ом*мм", 12),
                };
            return returnList;
        }

        private List<Statistics> GetRchain(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            int zeroIndex = 0;
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            for (int index = 0; index < xListdouble.Count; index++)
            {
                double d = xListdouble[index];
                if (Math.Abs(d - 0.3) < 0.005)
                {
                    zeroIndex = index;
                }

            }

            var ronList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                ronList.Add(yListdouble[zeroIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ronList, "R<sub>chain</sub> (сопротивление при U=0.3В)", "Ом"),
                };
            return returnList;
        }

        private List<Statistics> GetRind(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            int zeroIndex = 0;
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            for (int index = 0; index < xListdouble.Count; index++)
            {
                double d = xListdouble[index];
                if (Math.Abs(d - 0.05) < 0.003)
                {
                    zeroIndex = index;
                }

            }

            var ronList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                ronList.Add(yListdouble[zeroIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ronList, "R<sub>ind</sub> (сопротивление при U=0.5В)", "Ом"),
                };
            return returnList;
        }

        private List<Statistics> GetRchain01(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            int zeroIndex = 0;
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            for (int index = 0; index < xListdouble.Count; index++)
            {
                double d = xListdouble[index];
                if (Math.Abs(d - 0.1) < 0.005)
                {
                    zeroIndex = index;
                }

            }

            var ronList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                ronList.Add(yListdouble[zeroIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ronList, "R<sub>chain</sub> (сопротивление при U=0.1В)", "Ом"),
                };
            return returnList;
        }

        private List<Statistics> GetGmax(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var unit = "См";
            if (Math.Abs(divider - 1.0) > 1E-6)
            {
                unit = "См/мм";
            }
            var gmaxList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.Max()).ToList();
            var vpeakList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => xListdouble[yListdouble.IndexOf(yListdouble.Max())]).ToList();
            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(gmaxList, "g<sub>max</sub> (максимум крутизны)", unit, 6),
                    GetFullStatisticsFromList(vpeakList, "V<sub>GMmpeak</sub> (напряжение при Gmax)", "В")
                };
            return returnList;
        }


        private List<Statistics> GetVbr(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var vbrList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                var indexes = new List<int>();
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (Math.Abs(d) > 4.5E-4)
                    {
                        if (index < xListdouble.Count)
                        {
                            indexes.Add(index);
                        }

                    }
                }
                vbrList.Add(indexes.Count == 0
                                ? xListdouble.Last()
                                : indexes.Select(index => xListdouble[index]).ToList().Min());
            }
            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(vbrList, "U<sub>BR</sub> (напряжение пробоя сток-исток)", "В", 15),
                };
            return returnList;
        }

        private List<Statistics> GetVbrCherry(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var vbrList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                var indexes = new List<int>();
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (Math.Abs(d) > 9.9E-4)
                    {
                        if (index < xListdouble.Count)
                        {
                            indexes.Add(index);
                        }

                    }
                }
                vbrList.Add(indexes.Count == 0
                                ? xListdouble.Last()
                                : indexes.Select(index => xListdouble[index]).ToList().Min());
            }
            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(vbrList, "V<sub>br</sub> (напряжение пробоя сток-исток)", "В", 15),
                };
            return returnList;
        }



        private List<Statistics> GetLeak(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10.0 - item)).FirstOrDefault());
            var fiftyIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(50.0 - item)).FirstOrDefault());
            var hundredIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(100.0 - item)).FirstOrDefault());

            var ig10List = new List<double>();
            var ig50List = new List<double>();
            var ig100List = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                ig10List.Add(tenIndex < 0 ? yListdouble[0] : yListdouble[tenIndex]);
                ig50List.Add(fiftyIndex < 0 ? yListdouble[0] : yListdouble[fiftyIndex]);
                ig100List.Add(hundredIndex < 0 ? yListdouble[0] : yListdouble[hundredIndex]);
            }

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(ig10List, "I(V=10В) ток утечки мезы", "А"),
                    GetFullStatisticsFromList(ig50List, "I(V=50В) ток утечки мезы", "А"),
                    GetFullStatisticsFromList(ig100List, "I(V=100В) ток утечки мезы", "А")
                };
            return returnList;
        }

       

        private List<Statistics> GetRLine(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.3 - item)).FirstOrDefault());
            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>16</sub> при R=0.3В (гальваника)", "Ом"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetRLine05(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.5 - item)).FirstOrDefault());
            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>line</sub> при R=0.5В ", "Ом"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetR02(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.2 - item)).FirstOrDefault());
            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R(V=0.2В)", "Ом"),
                  
                };

            return returnList;
        }

        private List<Statistics> GetRgate(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.2 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>6</sub> при U=0.3B (затвор)", "Ом"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetRchain1(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.2 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>11</sub>(при U=0.3B)", "Ом"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetRchain2(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.2 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>12</sub>(при U=0.3B)", "Ом"),
                  
                };
            return returnList;
        }
        private List<Statistics> GetRTFR(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.2 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>TFR</sub> при U=0.3B (TFR-резистор)", "Ом"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetRisol(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var sixIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6 - item)).FirstOrDefault());
            
            var tenList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => tenIndex < 0 ? 10 / yListdouble[0] * 1E-9 : 10 / yListdouble[tenIndex] * 1E-9).ToList();
            var sixList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => sixIndex < 0 ? 6 / yListdouble[0] * 1E-9 : 6 / yListdouble[sixIndex] * 1E-9).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(sixList, "R<sub>isol</sub>(V=6В)", "ГОм"),
                    GetFullStatisticsFromList(tenList, "R<sub>isol</sub>(V=10В)", "ГОм")
                    
                  
                };
            return returnList;
        }

        private List<Statistics> GetTFRLong(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.3 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "R<sub>14</sub> при R=0.6В (змейка TFR)", "кОм"),
                  
                };

            return returnList;
        }

        private List<Statistics> GetCAPPCM(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.0 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "C при U=0В", "Ф"),
                    GetFullStatisticsFromList(zeroList.Select(x=>x*1E14).ToList(), "C<sub>MIM</sub>при U=0B (удельная ёмкость МДМ-конденсатора)", "пФ/мм²", 37),
                  
                };
            return returnList;
        }

        private List<Statistics> GetCMIM(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.06 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "C при U=0.06В", "Ф"),
                    GetFullStatisticsFromList(zeroList.Select(x=>x*1E12/0.09).ToList(), "C<sub>MIM</sub> при U=0.06B (удельная ёмкость МДМ-конденсатора)", "пФ/мм²", 37)
                  
                };
            return returnList;
        }


        private List<Statistics> GetCMIM003(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.06 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(zeroList, "C при U=0.06В", "Ф"),
                    GetFullStatisticsFromList(zeroList.Select(x=>x*1E12/0.03).ToList(), "C<sub>MIM</sub> при U=0.06B (удельная ёмкость МДМ-конденсатора)", "пФ/мм²", 37)

                };
            return returnList;
        }

        private List<Statistics> GetCMIM0025(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.06 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(zeroList, "C при U=0.06В", "Ф"),
                    GetFullStatisticsFromList(zeroList.Select(x=>x*1E12/0.0025).ToList(), "C<sub>MIM</sub> при U=0.06B (удельная ёмкость МДМ-конденсатора)", "пФ/мм²", 37)

                };
            return returnList;
        }

        private List<Statistics> GetI(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.2 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();
            
            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "I(V=0.2В)", "А"),
                  
                };
            return returnList;
        }


      

        private List<Statistics> GetCapLeaks(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(20.0 - item)).FirstOrDefault());


            var zeroList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => zeroIndex < 0 ? yListdouble[0] : yListdouble[zeroIndex]).ToList();

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList(zeroList, "I(V=20В)", "А"),
                  
                };
            return returnList;
        }

      

        private List<Statistics> GetIgss(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(-10 - item)).FirstOrDefault());
            var threeIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(-3 - item)).FirstOrDefault());
            var ig3List = new List<double>();
            var ig10List = new List<double>();
            var vbdgList = new List<double>();
            var unit = "A";
            if (Math.Abs(divider - 1) > 1E-6)
            {
                unit = "A/мм";
            }
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                ig3List.Add(yListdouble[threeIndex]);
                ig10List.Add(tenIndex < 0 ? yListdouble[0] : yListdouble[tenIndex]);
                var indexes = new List<int>();
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (Math.Abs(d) >= 0.0001)
                    {
                        if (index < xListdouble.Count)
                        {
                            indexes.Add(index);
                        }

                    }
                }
              
                vbdgList.Add(indexes.Count == 0
                                ? xListdouble[0]
                                : indexes.Select(index => xListdouble[index]).ToList().Max());
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ig3List, "I<sub>GSS(-3V)</sub> (ток утечки затвора при Uзи=-3В)", unit, 7),
                    GetFullStatisticsFromList(ig10List, "I<sub>GSS(-10V)</sub> (ток утечки затвора при Uзи=-10В)", unit, 8)
                    
                };
            return returnList;
        }

        private List<Statistics> GetIgss10V(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {

            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(-10 - item)).FirstOrDefault());
            var ig10List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                ig10List.Add(tenIndex < 0 ? yListdouble[0] : yListdouble[tenIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ig10List, "I(V=-10В)", "А")
                };
            return returnList;
        }

        private List<Statistics> GetIgss1E6(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {

            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var ig10List = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble.IndexOf(yListdouble.OrderBy(item => Math.Abs(1E-6 - item)).FirstOrDefault())).Select(tenIndex => xListdouble[tenIndex]).ToList();

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ig10List, "V(I=1E-6В)", "А")
                };
            return returnList;
        }

        private List<Statistics> GetS21OFFMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(20 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>21OFF(1GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>21OFF(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>21OFF(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>21OFF(20GHz)</sub>", "дБ"),
                   
                };
            return returnList;
        }

        private List<Statistics> GetFilterMarkersORION010(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index3 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3.0 - item)).FirstOrDefault());
            var index11 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(11.0 - item)).FirstOrDefault());

            var id3List = new List<double>();
            var id11List = new List<double>();
            var A0List = new List<double>();

            var bandfnList = new List<double>();
            var bandfbList = new List<double>();
            var bandfdleftList = new List<double>();
            var bandfdrightList = new List<double>();


            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id3List.Add(yListdouble[index3]);
                id11List.Add(yListdouble[index11]);

                var maxIndex = yListdouble.IndexOf(yListdouble.Max());
                A0List.Add(yListdouble.Max());

                var leftsideList = yListdouble.Take(maxIndex + 1).ToList();
                var rightsideList = yListdouble.Skip(maxIndex + 1).ToList();
                var leftIndex =
                    leftsideList.IndexOf(
                        leftsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 1 - item)).FirstOrDefault());
                var rightIndex =
                    rightsideList.IndexOf(
                        rightsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 1 - item)).FirstOrDefault()) +
                    maxIndex + 1;

                bandfnList.Add(xListdouble[leftIndex]);
                bandfbList.Add(xListdouble[rightIndex]);
                //bandfdleftList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(xListdouble[leftIndex] - 0.5 - item)).FirstOrDefault())]));
                //bandfdrightList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(xListdouble[rightIndex] + 0.5 - item)).FirstOrDefault())]));


            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id3List, "Потери на 3ГГц", "дБ"),
                    GetFullStatisticsFromList(id11List, "Потери на 11ГГц", "дБ"),
                    GetFullStatisticsFromList(A0List, "A0", "дБ"),
                    GetFullStatisticsFromList(bandfnList, "Fn", "ГГц"),
                    GetFullStatisticsFromList(bandfbList, "Fb", "ГГц"),
                    //GetFullStatisticsFromList(bandfdleftList, "FD_left", "ГГц", 55),
                    //GetFullStatisticsFromList(bandfdrightList, "FD_right", "ГГц", 56)

                };
            return returnList;
        }


        private List<Statistics> GetFilterMarkers15(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.5 - item)).FirstOrDefault());
            var id15List = new List<double>();
            var bandfnList = new List<double>();
            var bandfbList = new List<double>();
            var bandfdleftList = new List<double>();
            var bandfdrightList = new List<double>();


            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id15List.Add(yListdouble[onehalfIndex]);
                var maxIndex = yListdouble.IndexOf(yListdouble.Max());
                var leftsideList = yListdouble.Take(maxIndex + 1).ToList();
                var rightsideList = yListdouble.Skip(maxIndex + 1).ToList();
                var leftIndex =
                    leftsideList.IndexOf(
                        leftsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 3 - item)).FirstOrDefault());
                var rightIndex =
                    rightsideList.IndexOf(
                        rightsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 3 - item)).FirstOrDefault()) +
                    maxIndex + 1;
                bandfnList.Add(xListdouble[leftIndex]);
                bandfbList.Add(xListdouble[rightIndex]);
                bandfdleftList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(xListdouble[leftIndex] - 0.5 - item)).FirstOrDefault())]));
                bandfdrightList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(xListdouble[rightIndex] + 0.5 - item)).FirstOrDefault())]));


            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id15List, "Потери на 1.5ГГц", "дБ", 51),
                    GetFullStatisticsFromList(bandfnList, "Fn", "ГГц", 53),
                    GetFullStatisticsFromList(bandfbList, "Fb", "ГГц", 54),
                    GetFullStatisticsFromList(bandfdleftList, "FD_left", "ГГц", 55),
                    GetFullStatisticsFromList(bandfdrightList, "FD_right", "ГГц", 56)

                };
            return returnList;
        }


        private List<Statistics> GetFilterMarkers2000_1000(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.075 - item)).FirstOrDefault());
            var id15List = new List<double>();
            var bandfnList = new List<double>();
            var bandfbList = new List<double>();
            var bandfdleftList = new List<double>();
            var bandfdrightList = new List<double>();


            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id15List.Add(yListdouble[onehalfIndex]);
                var maxIndex = yListdouble.IndexOf(yListdouble.Max());
                var leftsideList = yListdouble.Take(maxIndex + 1).ToList();
                var rightsideList = yListdouble.Skip(maxIndex + 1).ToList();
                var leftIndex =
                    leftsideList.IndexOf(
                        leftsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 3 - item)).FirstOrDefault());
                var rightIndex =
                    rightsideList.IndexOf(
                        rightsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 3 - item)).FirstOrDefault()) +
                    maxIndex + 1;
                bandfnList.Add(xListdouble[leftIndex]);
                bandfbList.Add(xListdouble[rightIndex]);
                bandfdleftList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(Math.Abs(xListdouble[leftIndex] - 0.5 - item))).FirstOrDefault())]));
                bandfdrightList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(Math.Abs(xListdouble[rightIndex] + 0.5 - item))).FirstOrDefault())]));


            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id15List, "Потери на 2ГГц", "дБ", 52),
                    GetFullStatisticsFromList(bandfnList, "Fn", "ГГц", 53),
                    GetFullStatisticsFromList(bandfbList, "Fb", "ГГц", 54),
                    GetFullStatisticsFromList(bandfdleftList, "FD_left", "ГГц", 55),
                    GetFullStatisticsFromList(bandfdrightList, "FD_right", "ГГц", 56)

                };
            return returnList;
        }


        private List<Statistics> GetFilterMarkers2000_900(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.075 - item)).FirstOrDefault());
            var id15List = new List<double>();
            var bandfnList = new List<double>();
            var bandfbList = new List<double>();
            var bandfdleftList = new List<double>();
            var bandfdrightList = new List<double>();


            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id15List.Add(yListdouble[onehalfIndex]);
                var maxIndex = yListdouble.IndexOf(yListdouble.Max());
                var leftsideList = yListdouble.Take(maxIndex + 1).ToList();
                var rightsideList = yListdouble.Skip(maxIndex + 1).ToList();
                var leftIndex =
                    leftsideList.IndexOf(
                        leftsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 3 - item)).FirstOrDefault());
                var rightIndex =
                    rightsideList.IndexOf(
                        rightsideList.OrderBy(item => Math.Abs(yListdouble.Max() - 3 - item)).FirstOrDefault()) +
                    maxIndex + 1;
                bandfnList.Add(xListdouble[leftIndex]);
                bandfbList.Add(xListdouble[rightIndex]);
                bandfdleftList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(Math.Abs(xListdouble[leftIndex] - 0.5 - item))).FirstOrDefault())]));
                bandfdrightList.Add(Math.Abs(yListdouble[xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(Math.Abs(xListdouble[rightIndex] + 0.5 - item))).FirstOrDefault())]));


            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id15List, "Потери на 2ГГц", "дБ", 52),
                    GetFullStatisticsFromList(bandfnList, "Fn", "ГГц", 53),
                    GetFullStatisticsFromList(bandfbList, "Fb", "ГГц", 54),
                    GetFullStatisticsFromList(bandfdleftList, "FD_left", "ГГц", 55),
                    GetFullStatisticsFromList(bandfdrightList, "FD_right", "ГГц", 56)

                };
            return returnList;
        }

        private List<Statistics> GetFilterMarkers20S21S22(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.075 - item)).FirstOrDefault());

            var id15List = new List<double>();



            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id15List.Add(yListdouble[onehalfIndex]);

            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id15List, "Потери на 1.5ГГц", "дБ")

                };
            return returnList;
        }

        private List<Statistics> GetFilterMarkers15S21S22(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.5 - item)).FirstOrDefault());

            var id15List = new List<double>();
           


            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id15List.Add(yListdouble[onehalfIndex]);
              
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id15List, "Потери на 1.5ГГц", "дБ")
                  
                };
            return returnList;
        }

        private List<Statistics> GetS21ONMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(20 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>21ON(1GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>21ON(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>21ON(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>21ON(20GHz)</sub>", "дБ"),
                   
                };
            return returnList;
        }

        private List<Statistics> GetS22OFFMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>22OFF(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>22OFF(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>22OFF(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>22OFF(18GHz)</sub>", "дБ"),
                   
                };
            return returnList;
        }
        private List<Statistics> GetS22ONMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>22ON(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>22ON(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>22ON(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>22ON(18GHz)</sub>", "дБ"),
                   
                };
            return returnList;
        }

        private List<Statistics> GetS11OFFMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>11OFF(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>11OFF(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>11OFF(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>11OFF(18GHz)</sub>", "дБ"),
                   
                };
            return returnList;
        }
        private List<Statistics> GetS11ONMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>11ON(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>11ON(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>11ON(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>11ON(18GHz)</sub>", "дБ"),
                   
                };
            return returnList;
        }

        private List<Statistics> GetS21X5(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var s21List = new List<double>();
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());


            List<double> id5List = new List<double>();
            List<double> id10List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                s21List.Add(yListdouble[0]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(s21List, "S<sub>21(минимальная частота)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S21<sub>(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S21<sub>(10GHz)</sub>", "дБ")

                };
            return returnList;
        }

        private List<Statistics> GetS11X5(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var s21List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                s21List.Add(yListdouble[0]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(s21List, "S<sub>21(минимальная частота)</sub>", "дБ"),


                };
            return returnList;
        }


        private List<Statistics> GetS22X5(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var s21List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                s21List.Add(yListdouble[0]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(s21List, "S<sub>21(минимальная частота)</sub>", "дБ"),


                };
            return returnList;
        }


        private List<Statistics> GetS12X5(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var s21List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                s21List.Add(yListdouble[0]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(s21List, "S<sub>21(минимальная частота)</sub>", "дБ"),


                };
            return returnList;
        }


        private List<Statistics> GetIgss025(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twoIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(-2 - item)).FirstOrDefault());
            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(-8 - item)).FirstOrDefault());
            var ig2List = new List<double>();
            var ig8List = new List<double>();
            var vbdgList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                ig2List.Add(twoIndex < 0 ? yListdouble[0] : yListdouble[twoIndex]);
                ig8List.Add(eightIndex < 0 ? yListdouble[0] : yListdouble[eightIndex]);
                var indexes = new List<int>();
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (Math.Abs(d) >= 0.0001)
                    {
                        if (index < xListdouble.Count)
                        {
                            indexes.Add(index);
                        }

                    }
                }
                vbdgList.Add(indexes.Count == 0
                                ? xListdouble[0]
                                : indexes.Select(index => xListdouble[index]).ToList().Max());
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(ig2List, "I<sub>g2V</sub> (ток при Uзи=-2В)", "А"),
                    GetFullStatisticsFromList(ig8List, "I<sub>g8V</sub> (ток при Uзи=-8В)", "А")
                  
                };
            return returnList;
        }

        private List<Statistics> GetCV(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var zeroIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0 - item)).FirstOrDefault());
            var cvList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                cvList.Add(yListdouble[zeroIndex]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(cvList, "С (емкость при V=0В)", "Ф"),
                  
                };
            return returnList;
        }

        private List<Statistics> GetVbrdg(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var vbrdgList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                var hundredList = new List<int>();
                foreach (var d in yListdouble.Where(item => Math.Abs(100E-6 - item) < 5E-6).ToList())
                {
                    hundredList.Add(yListdouble.IndexOf(d));
                }
                vbrdgList.Add(xListdouble[hundredList.OrderBy(x=>x).FirstOrDefault()]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(vbrdgList, "V<sub>(br)dg</sub> (напряжение при Ig=100мкА)", "В"),

                };
            return returnList;
        }

        private List<Statistics> GetIdVd_Progress(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var ocIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.15 - item)).FirstOrDefault());
            var oneIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.0 - item)).FirstOrDefault());
            var twoIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.0 - item)).FirstOrDefault());
            var threeIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3.0 - item)).FirstOrDefault());
            var fourIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5.0 - item)).FirstOrDefault());
            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.5 - item)).FirstOrDefault());

            var id2List = new List<double>();
            var id3List = new List<double>();
            var id4List = new List<double>();
            var id5List = new List<double>();
            var s31List = new List<double>();
            var s51List = new List<double>();
            var sfList = new List<double>();
            var ocList = new List<double>();
            var enumerable = commonYList as IList<List<string>> ?? commonYList.ToList();
            foreach (List<double> yListdouble in enumerable.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var interpolationmethod = Interpolate.Linear(xListdouble, yListdouble);
                var tempList = xListdouble.Where(x => x >= xListdouble[onehalfIndex]).Select(interpolationmethod.Differentiate).ToList();
                sfList.Add(Math.Abs(tempList.Max() - tempList.Min()) * 1000);

                s31List.Add(100 - (yListdouble[onehalfIndex] / yListdouble[threeIndex]) * 100);
                s51List.Add(100 - (yListdouble[onehalfIndex] / yListdouble[fiveIndex]) * 100);



            }

            foreach (List<double> yListdouble in enumerable.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                ocList.Add((xListdouble[ocIndex] / yListdouble[ocIndex] - 1.1) * divider);
                id2List.Add(yListdouble[twoIndex] * 1.11 / divider);
                id3List.Add(yListdouble[threeIndex] * 1.11 / divider);
                id5List.Add(yListdouble[fiveIndex] * 1.11 / divider);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id2List, "I<sub>dss(2V)</sub> (ток при Uси=2В)", "А", 28),
                    GetFullStatisticsFromList(id3List, "I<sub>dss(3V)</sub> (ток при Uси=3В)", "А", 29),
                    GetFullStatisticsFromList(id5List, "I<sub>dss(5V)</sub> (ток при Uси=5В)", "А", 30),
                    GetFullStatisticsFromList(ocList, "R<sub>ds(on)</sub> (сопротивление открытого канала)", "Ом", 12),
                    GetFullStatisticsFromList(s31List, "S<sub>3-1.5</sub> (критерий S-образности)", "%"),
                    GetFullStatisticsFromList(s51List, "S<sub>5-1.5</sub> (критерий S-образности)", "%"),
                    GetFullStatisticsFromList(sfList,  "S<sub>f</sub> (критерий S-образности)", "мСм")
                };
            return returnList;
        }


        private List<Statistics> GetIdVd(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var ocIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.05 - item)).FirstOrDefault());
            var oneIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.0 - item)).FirstOrDefault());
            var twoIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.0 - item)).FirstOrDefault());
            var threeIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3.0 - item)).FirstOrDefault());
            var fourIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5.0 - item)).FirstOrDefault());
            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.5 - item)).FirstOrDefault());
            
            var id2List = new List<double>();
            var id3List = new List<double>();
            var id4List = new List<double>();
            var id5List = new List<double>();
            var s31List = new List<double>();
            var s51List = new List<double>();
            var sfList = new List<double>();
            var ocList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var interpolationmethod = Interpolate.Linear(xListdouble, yListdouble);
                var tempList = xListdouble.Where(x => x >= xListdouble[onehalfIndex]).Select(interpolationmethod.Differentiate).ToList();
                sfList.Add(Math.Abs(tempList.Max() - tempList.Min()) * 1000);
                id2List.Add(yListdouble[twoIndex]);
                id3List.Add(yListdouble[threeIndex]);
                id4List.Add(yListdouble[fourIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                s31List.Add(100 - (yListdouble[onehalfIndex] / yListdouble[threeIndex]) * 100);
                s51List.Add(100 - (yListdouble[onehalfIndex] / yListdouble[fiveIndex]) * 100);
                ocList.Add(xListdouble[ocIndex] / yListdouble[ocIndex]);


            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id2List, "I<sub>dss(2V)</sub> (ток при Uси=2В)", "А", 28),
                    GetFullStatisticsFromList(id3List, "I<sub>dss(3V)</sub> (ток при Uси=3В)", "А", 29),
                    GetFullStatisticsFromList(id5List, "I<sub>dss(5V)</sub> (ток при Uси=5В)", "А", 30),
                    GetFullStatisticsFromList(ocList, "R<sub>ds(on)</sub> (сопротивление открытого канала)", "Ом", 12),
                    GetFullStatisticsFromList(s31List, "S<sub>3-1.5</sub> (критерий S-образности)", "%"),
                    GetFullStatisticsFromList(s51List, "S<sub>5-1.5</sub> (критерий S-образности)", "%"),
                    GetFullStatisticsFromList(sfList,  "S<sub>f</sub> (критерий S-образности)", "мСм")
                };
            return returnList;
        }

        private List<Statistics> GetIdVd025(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var ocIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.15 - item)).FirstOrDefault());
            var twoIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.0 - item)).FirstOrDefault());
            var threeIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3.0 - item)).FirstOrDefault());
            var fourIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5.0 - item)).FirstOrDefault());
            var onehalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.5 - item)).FirstOrDefault());


            var id2List = new List<double>();
            var id3List = new List<double>();
            var id4List = new List<double>();
            var id5List = new List<double>();
            var s31List = new List<double>();
            var s51List = new List<double>();
            var sfList = new List<double>();
            var ocList = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var interpolationmethod = Interpolate.Linear(xListdouble, yListdouble);
                var tempList = xListdouble.Where(x => x >= xListdouble[onehalfIndex]).Select(interpolationmethod.Differentiate).ToList();
                sfList.Add(Math.Abs(tempList.Max() - tempList.Min()) * 1000);
                id2List.Add(yListdouble[twoIndex]);
                id3List.Add(yListdouble[threeIndex]);
                id4List.Add(yListdouble[fourIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                s31List.Add(100 - (yListdouble[onehalfIndex] / yListdouble[threeIndex]) * 100);
                s51List.Add(100 - (yListdouble[onehalfIndex] / yListdouble[fiveIndex]) * 100);
                ocList.Add(xListdouble[ocIndex]/yListdouble[ocIndex]);

            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id2List, "I<sub>dss(2V)</sub> (ток при Uси=2В)", "А", 28),
                    GetFullStatisticsFromList(id3List, "I<sub>dss(3V)</sub> (ток при Uси=3В)", "А", 29),
                    GetFullStatisticsFromList(id5List, "I<sub>dss(5V)</sub> (ток при Uси=5В)", "А", 30),
                    GetFullStatisticsFromList(ocList, "R<sub>ds(on)</sub> (сопротивление открытого канала)", "Ом", 12),
                    GetFullStatisticsFromList(s31List, "S<sub>3-1.5</sub> (критерий S-образности)", "%"),
                    GetFullStatisticsFromList(s51List, "S<sub>5-1.5</sub> (критерий S-образности)", "%"),
                    GetFullStatisticsFromList(sfList, "S<sub>f</sub> (критерий S-образности)", "мСм")
                };
            return returnList;
        }

       

        private List<Statistics> GetS21Markers025(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S21<sub>(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S21<sub>(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S21<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S21<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList;
        }

     

        private List<Statistics> GetS12MarkersORION002(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S12<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S12<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S12<sub>(6GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS22MarkersORION002(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S22<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S22<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S22<sub>(6GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS11MarkersORION002(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S11<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S11<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S11<sub>(6GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS21MarkersORION002(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S21<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S21<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S21<sub>(6GHz)</sub>", "дБ"),

                };
            return returnList;
        }


        private List<Statistics> GetS21MarkersORION006(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S21<sub>(2GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S21<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S21<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList;
        }



       

        private List<Statistics> GetS22MarkersORION006(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S22<sub>(2GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S22<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S22<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS11MarkersORION006(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2 - item)).FirstOrDefault());

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();

            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);

                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S11<sub>(2GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S11<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S11<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS22Markers025(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S22<sub>(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S22<sub>(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S22<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S22<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList;
        }


        private List<Statistics> GetS12Markers_ORION003(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
           
            var index06 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.6 - item)).FirstOrDefault());
            var index225 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.25 - item)).FirstOrDefault());
           
            var id06List = new List<double>();
            var id225List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

               
                id06List.Add(yListdouble[index06]);
                id225List.Add(yListdouble[index225]);



            }

            var returnList = new List<Statistics>
                {
                  
                    GetFullStatisticsFromList( id06List, "S12<sub>(0.6GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id225List, "S12<sub>(2.25GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS22Markers_ORION003(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index06 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.6 - item)).FirstOrDefault());
            var index225 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.25 - item)).FirstOrDefault());

            var id06List = new List<double>();
            var id225List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id06List.Add(yListdouble[index06]);
                id225List.Add(yListdouble[index225]);



            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id06List, "S22<sub>(0.6GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id225List, "S22<sub>(2.25GHz)</sub>", "дБ"),

                };
            return returnList;
        }


        private List<Statistics> GetS21Markers_ORION003(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index06 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.6 - item)).FirstOrDefault());
            var index225 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.25 - item)).FirstOrDefault());

            var id06List = new List<double>();
            var id225List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id06List.Add(yListdouble[index06]);
                id225List.Add(yListdouble[index225]);



            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id06List, "S21<sub>(0.6GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id225List, "S21<sub>(2.25GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS21Markers_ORION001(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index4 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var index6 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6.0 - item)).FirstOrDefault());
            
            var id4List = new List<double>();
            var id6List = new List<double>();
            var deltaList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id4List.Add(yListdouble[index4]);
                id6List.Add(yListdouble[index6]);
                var y46List = new List<double>();
                for (var i = index4; i < index6 + 1; i++)
                {
                    y46List.Add(yListdouble[i]);
                }
                deltaList.Add(Math.Abs(y46List.Max() - y46List.Min()));
                


            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id4List, "S21<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id6List, "S21<sub>(6GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( deltaList, "S21_delta<sub>(4-6GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS12Markers_ORION001(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index4 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var index6 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6.0 - item)).FirstOrDefault());

            var id4List = new List<double>();
            var id6List = new List<double>();
            var deltaList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id4List.Add(yListdouble[index4]);
                id6List.Add(yListdouble[index6]);
             



            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id4List, "S12<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id6List, "S12<sub>(6GHz)</sub>", "дБ"),
                   

                };
            return returnList;
        }

        private List<Statistics> GetS22Markers_ORION001(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index4 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var index6 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6.0 - item)).FirstOrDefault());

            var id4List = new List<double>();
            var id6List = new List<double>();
            var deltaList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id4List.Add(yListdouble[index4]);
                id6List.Add(yListdouble[index6]);
              



            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id4List, "S22<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id6List, "S22<sub>(6GHz)</sub>", "дБ"),
                   

                };
            return returnList;
        }

        private List<Statistics> GetS11Markers_ORION001(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index4 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.0 - item)).FirstOrDefault());
            var index6 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(6.0 - item)).FirstOrDefault());

            var id4List = new List<double>();
            var id6List = new List<double>();
            var deltaList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id4List.Add(yListdouble[index4]);
                id6List.Add(yListdouble[index6]);
              



            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id4List, "S11<sub>(4GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id6List, "S11<sub>(6GHz)</sub>", "дБ"),
                  

                };
            return returnList;
        }
        private List<Statistics> GetS11Markers_ORION003(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var index06 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.6 - item)).FirstOrDefault());
            var index225 = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.25 - item)).FirstOrDefault());

            var id06List = new List<double>();
            var id225List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {


                id06List.Add(yListdouble[index06]);
                id225List.Add(yListdouble[index225]);



            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList( id06List, "S11<sub>(0.6GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList( id225List, "S11<sub>(2.25GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS12Markers025(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S12<sub>(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S12<sub>(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S12<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S12<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList; 
        }

        private List<Statistics> GetS11Markers025(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S11<sub>(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S11<sub>(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S11<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S11<sub>(18GHz)</sub>", "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetS21Markers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S21<sub>(2.5GHz)</sub>(коэффициент передачи)", "дБ", 18),
                    GetFullStatisticsFromList(id5List,  "S21<sub>(5GHz)</sub>(коэффициент передачи)", "дБ", 19),
                    GetFullStatisticsFromList(id10List, "S21<sub>(10GHz)</sub>(коэффициент передачи)", "дБ", 20),
                    GetFullStatisticsFromList(id18List, "S21<sub>(18GHz)</sub>(коэффициент передачи)", "дБ", 21),

                };
            return returnList;
        }

        private List<Statistics> GetATTMongo(List<double> xListdouble, List<List<double>> commonYList, double divider, string type)
        {

            var twelveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(8 - item)).FirstOrDefault());
            var s21sigmaList = new List<double>();
            var s21at10List = new List<double>();
           
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x / divider).ToList()))
            {

                s21sigmaList.Add(Math.Abs(yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Max() - yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Min()));
                s21at10List.Add(yListdouble[tenIndex]);
              
            }

            var returnList = new List<Statistics>
                {
                 
                    GetFullStatisticsFromList(s21at10List, "S21<sub>(10GHz)</sub> - type: " + type, "дБ"),
                    GetFullStatisticsFromList(s21sigmaList, "S21<sub>sigma(8GHz-12GHz)</sub> - type: " + type, "дБ"),

                };
            return returnList;
        }

        private string GetATTMongoPoint(List<double> xListdouble, List<List<double>> commonYList, string type, Dictionary<string, double> points)
        {

            var index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(points["SinglePoint"] - item)).FirstOrDefault());
            var s21at10List = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x).ToList()))
            {
                s21at10List.Add(yListdouble[index]);

            }
            
            return GetFullStatisticsFromList(s21at10List, "S21<sub>(10GHz)</sub>", "дБ").ExpectedValue;
        }

       

        private string GetATTMongoSigma(List<double> xListdouble, List<List<double>> commonYList, string type, Dictionary<string, double> points)
        {

            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(points["StartRangeGatePoint"] - item)).FirstOrDefault());
            var twelveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(points["EndRangeGatePoint"] - item)).FirstOrDefault());
            var s21sigmaList = new List<double>();
           
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x).ToList()))
            {

                s21sigmaList.Add(Math.Abs(yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Max() - yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Min()));
              
            }
            
            return GetFullStatisticsFromList(s21sigmaList, "S21<sub>sigma(8GHz-12GHz)</sub>", "дБ").ExpectedValue;
        }

        private string GetATTMongoPointPair(List<double> xListdouble, Dictionary<string, List<List<double>>> commonYList, Dictionary<string,string> type, Dictionary<string, double> points)
        {

            var index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(points["SinglePoint"] - item)).FirstOrDefault());

            var firstslash = commonYList[type["FirstSlashType"]].Select(yList => yList.Select(x => x).ToList()).Select(yListdouble => yListdouble[index]).ToList();
            var secondslash = commonYList[type["SecondSlashType"]].Select(yList => yList.Select(x => x).ToList()).Select(yListdouble => yListdouble[index]).ToList();

            return GetFullStatisticsFromList(firstslash, "S21<sub>(10GHz)</sub> - type: ", "дБ").ExpectedValue + "/" + GetFullStatisticsFromList(secondslash, "S21<sub>(10GHz)</sub> - type: ", "дБ").ExpectedValue;
        }

        private List<Statistics> GetPSMongo(List<double> xListdouble, List<List<double>> commonYList, double divider, string type)
        {

            var twelveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(8 - item)).FirstOrDefault());
            var s21sigmaList = new List<double>();
            var s21at10List = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x / divider).ToList()))
            {

                s21sigmaList.Add(Math.Abs(yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Max() - yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Min()));
                s21at10List.Add(yListdouble[tenIndex]);

            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(s21at10List, "S21<sub>(10GHz)</sub> - type: " + type, "дБ"),
                    GetFullStatisticsFromList(s21sigmaList, "S21<sub>sigma(8GHz-12GHz)</sub> - type: " + type, "дБ"),

                };
            return returnList;
        }



        private List<Statistics> GetATTMongoPhase(List<double> xListdouble, List<List<double>> commonYList, double divider, string type)
        {

            var twelveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(8 - item)).FirstOrDefault());
            var s21sigmaList = new List<double>();
            var s21at10List = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x / divider).ToList()))
            {

                s21sigmaList.Add(Math.Abs(yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Max() - yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Min()));
                s21at10List.Add(yListdouble[tenIndex]);

            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(s21at10List, "Phase_S21<sub>(10GHz)</sub> - type: " + type, "дБ"),
                    GetFullStatisticsFromList(s21sigmaList, "Phase_S21<sub>sigma(8GHz-12GHz)</sub> - type: " + type, "дБ"),

                };
            return returnList;
        }

        private List<Statistics> GetPSMongoPhase(List<double> xListdouble, List<List<double>> commonYList, double divider, string type)
        {

            var twelveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(8 - item)).FirstOrDefault());
            var s21sigmaList = new List<double>();
            var s21at10List = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x / divider).ToList()))
            {

                s21sigmaList.Add(
                    Math.Abs(yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Max() -
                             yListdouble.Skip(eightIndex).Take(twelveIndex - eightIndex).Min()));
                s21at10List.Add(yListdouble[tenIndex]);

            }

            var returnList = new List<Statistics>
            {

                GetFullStatisticsFromList(s21at10List, "Phase_S21<sub>(10GHz)</sub> - type: " + type, "дБ"),
                GetFullStatisticsFromList(s21sigmaList, "Phase_S21<sub>sigma(8GHz-12GHz)</sub> - type: " + type, "дБ"),

            };
            return returnList;
        }

        private List<Statistics> GetATTMongoS21(List<double> xListdouble, List<List<double>> commonYList, double divider, string type)
        {

            var twelveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(8 - item)).FirstOrDefault());

            var s21at8List = new List<double>();
            var s21at10List = new List<double>();
            var s21at12List = new List<double>();

            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => x / divider).ToList()))
            {
                s21at8List.Add(yListdouble[eightIndex]);
                s21at10List.Add(yListdouble[tenIndex]);
                s21at12List.Add(yListdouble[twelveIndex]);
            }

            var returnList = new List<Statistics>
                {

                    GetFullStatisticsFromList(s21at8List, type + "<sub>(8GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(s21at10List, type + "<sub>(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(s21at12List, type + "<sub>(12GHz)</sub>", "дБ")
                 

                };
            return returnList;
        }

        private List<Statistics> GetS21MarkersFilter(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)

        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.85 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.22 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.77 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.1 - item)).FirstOrDefault());
            var eightteen2Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());
            var eightteen1Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(7.1 - item)).FirstOrDefault());
            var a0Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.7 - item)).FirstOrDefault());
            var a45Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(4.5 - item)).FirstOrDefault());
            var a078Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.78 - item)).FirstOrDefault());
            var id085List = new List<double>();
            var id122List = new List<double>();
            var id177List = new List<double>();
            var id21List = new List<double>();
            var id3List = new List<double>();
            var id71List = new List<double>();
            var a0List = new List<double>();
            var a45List = new List<double>();
            List<double> a078List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id085List.Add(yListdouble[twohalfIndex]);
                id122List.Add(yListdouble[fiveIndex]);
                id177List.Add(yListdouble[tenIndex]);
                id21List.Add(yListdouble[eightteenIndex]);
                id3List.Add(yListdouble[eightteen2Index]);
                id71List.Add(yListdouble[eightteen1Index]);
                a0List.Add(yListdouble[a0Index]);
                a45List.Add(yListdouble[a45Index]);
                a078List.Add(yListdouble[a078Index]);


            }

           
            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(a0List, "A0/F0<sub>(1.7GHz)</sub>", "дБ", 42),
                    GetFullStatisticsFromList(a45List, "A4.5ГГц<sub>(4.5GHz)</sub>", "дБ", 43),
                    GetFullStatisticsFromList(a078List, "A0.78ГГц<sub>(0.78GHz)</sub>", "дБ", 44),
                    GetFullStatisticsFromList(id085List, "S21<sub>(0.85GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id122List,  "S21<sub>(1.22GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id177List, "S21<sub>(1.77GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id21List, "S21<sub>(2.1GHz)</sub>", "дБ"), 
                    GetFullStatisticsFromList(id3List, "S21<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id71List, "S21<sub>(7.1GHz)</sub>", "дБ")

                   
                };
            return returnList;
        }

        private List<Statistics> GetS11MarkersFilter(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.85 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.22 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.77 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.1 - item)).FirstOrDefault());
            var eightteen2Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());
            var eightteen1Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(7.1 - item)).FirstOrDefault());
            var id085List = new List<double>();
            var id122List = new List<double>();
            var id177List = new List<double>();
            var id21List = new List<double>();
            var id3List = new List<double>();
            var id71List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id085List.Add(yListdouble[twohalfIndex]);
                id122List.Add(yListdouble[fiveIndex]);
                id177List.Add(yListdouble[tenIndex]);
                id21List.Add(yListdouble[eightteenIndex]);
                id3List.Add(yListdouble[eightteen2Index]);
                id71List.Add(yListdouble[eightteen1Index]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id085List, "S11<sub>(0.85GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id122List,  "S11<sub>(1.22GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id177List, "S11<sub>(1.77GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id21List, "S11<sub>(2.1GHz)</sub>", "дБ"), 
                    GetFullStatisticsFromList(id3List, "S11<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id71List, "S11<sub>(7.1GHz)</sub>", "дБ")

                   
                };
            return returnList;
        }

        private List<Statistics> GetS22MarkersFilter(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(0.85 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.22 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(1.77 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.1 - item)).FirstOrDefault());
            var eightteen2Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(3 - item)).FirstOrDefault());
            var eightteen1Index = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(7.1 - item)).FirstOrDefault());
            var id085List = new List<double>();
            var id122List = new List<double>();
            var id177List = new List<double>();
            var id21List = new List<double>();
            var id3List = new List<double>();
            var id71List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id085List.Add(yListdouble[twohalfIndex]);
                id122List.Add(yListdouble[fiveIndex]);
                id177List.Add(yListdouble[tenIndex]);
                id21List.Add(yListdouble[eightteenIndex]);
                id3List.Add(yListdouble[eightteen2Index]);
                id71List.Add(yListdouble[eightteen1Index]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id085List, "S22<sub>(0.85GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id122List,  "S22<sub>(1.22GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id177List, "S22<sub>(1.77GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id21List, "S22<sub>(2.1GHz)</sub>", "дБ"), 
                    GetFullStatisticsFromList(id3List, "S22<sub>(3GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id71List, "S22<sub>(7.1GHz)</sub>", "дБ")

                   
                };
            return returnList;
        }

       

        
       

       


        private List<Statistics> GetS11Markers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>11(2.5GHz)</sub>", "дБ", 22),
                    GetFullStatisticsFromList(id5List,  "S<sub>11(5GHz)</sub>", "дБ", 23),
                    GetFullStatisticsFromList(id10List, "S<sub>11(10GHz)</sub>", "дБ", 24),
                    GetFullStatisticsFromList(id18List, "S<sub>11(18GHz)</sub>", "дБ", 25)
                   
                };
            return returnList;
        }


        private List<Statistics> GetNoiseMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList,
            double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var firstIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(9.5 - item)).FirstOrDefault());
            var secondIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var id95List = new List<double>();
            var id12List = new List<double>();
            foreach (
                List<double> yListdouble in
                commonYList.Select(
                    yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                id95List.Add(yListdouble[firstIndex]);
                id12List.Add(yListdouble[secondIndex]);
            }

            var returnList = new List<Statistics>
            {
                GetFullStatisticsFromList(id95List, "Noise(9.5GHz)</sub>", "дБ"),
                GetFullStatisticsFromList(id12List, "Noise(12GHz)</sub>", "дБ")
            };
            return returnList;
        }


        private List<Statistics> GetGainMarkers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList,
            double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var firstIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(9.5 - item)).FirstOrDefault());
            var secondIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(12 - item)).FirstOrDefault());
            var id95List = new List<double>();
            var id12List = new List<double>();
            foreach (
                List<double> yListdouble in
                commonYList.Select(
                    yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                id95List.Add(yListdouble[firstIndex]);
                id12List.Add(yListdouble[secondIndex]);
            }

            var returnList = new List<Statistics>
            {
                GetFullStatisticsFromList(id95List, "Gain(9.5GHz)</sub>", "дБ"),
                GetFullStatisticsFromList(id12List, "Gain(12GHz)</sub>", "дБ")
            };
            return returnList;
        }

        private List<Statistics> GetS22Markers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>22(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>22(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>22(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>22(18GHz)</sub>", "дБ")

                };
            return returnList;
        }

        private List<Statistics> GetS12Markers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var twohalfIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(2.5 - item)).FirstOrDefault());
            var fiveIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(5 - item)).FirstOrDefault());
            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(10 - item)).FirstOrDefault());
            var eightteenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(18 - item)).FirstOrDefault());
            var id25List = new List<double>();
            var id5List = new List<double>();
            var id10List = new List<double>();
            var id18List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {

                id25List.Add(yListdouble[twohalfIndex]);
                id5List.Add(yListdouble[fiveIndex]);
                id10List.Add(yListdouble[tenIndex]);
                id18List.Add(yListdouble[eightteenIndex]);



            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(id25List, "S<sub>12(2.5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id5List,  "S<sub>12(5GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id10List, "S<sub>12(10GHz)</sub>", "дБ"),
                    GetFullStatisticsFromList(id18List, "S<sub>12(18GHz)</sub>", "дБ")

                };
            return returnList;
        }

        private List<Statistics> GetZ11Markers(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var enumerable = xList as IList<string> ?? xList.ToList();
            List<double> xListdouble = enumerable.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
           
            var z11List = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                z11List.Add(yListdouble[0]);
            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(z11List, "Z<sub>11(" + enumerable.FirstOrDefault() + "GHz)" + "</sub>", "дБ")
                  
                };
            return returnList;
        }

        private List<Statistics> GetCAP_LEAK(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var vbrList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                var indexes = new List<int>();
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (d >= 1E-6)
                    {
                        if (index < xListdouble.Count)
                        {
                            indexes.Add(index);
                        }

                    }
                } 
                vbrList.Add(indexes.Count == 0
                                ? xListdouble.Last()
                                : indexes.Select(index => xListdouble[index]).ToList().Min());
            }
            var twentyIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(20 - item)).FirstOrDefault());
            var twentyList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)/divider).ToList()).Select(yListdouble => yListdouble[twentyIndex]).ToList();

            var returnList = new List<Statistics>
                {
                   
                    GetFullStatisticsFromList(twentyList, "Утечка на 20В", "А"),
                    GetFullStatisticsFromList(vbrList, "U<sub>BRC</sub> (пробивное напряжение МДМ-конденсатора)", "В", 50),
                   
                };
            return returnList;
        }

        private List<Statistics> GetVbrx5(IEnumerable<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {

            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var vbrList = new List<double>();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                var indexes = new List<int>();
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (Math.Abs(d) >= 5E-3 - 1E-6 )
                    { 
                        if (index < xListdouble.Count)
                        {
                            indexes.Add(index);
                        }

                    } 
                }
                vbrList.Add(indexes.Count == 0
                                ? double.NaN
                                : indexes.Select(index => Math.Abs(xListdouble[index])).ToList().Min());
            }
            if (vbrList.Count(x => Double.IsNaN(x)) == vbrList.Count())
            {
                var badvbrList = vbrList.Select(d => 0.0).ToList();
                var badreturnList = new List<Statistics>
                {
                   
                   GetFullStatisticsFromList(badvbrList, "V<sub>brc</sub> (напряжение пробоя !не найдено! )", "В", 15),
                   
                };
                return badreturnList;
            }
          

            var returnList = new List<Statistics>
                {
                   
                   GetFullStatisticsFromList(vbrList, "V<sub>brc</sub> (напряжение пробоя, ограничение I=10мА )", "В", 15),

                   
                };
            return returnList;
        }

        private List<Statistics> GetPinPout(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();

            var tenIndex = xListdouble.IndexOf(xListdouble.OrderBy(item => Math.Abs(-10 - item)).FirstOrDefault());
            var pinList = new List<double>();
            var poutList = new List<double>();
            var pexit = new List<double>();
            var mainindex = 0;
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var interpolationmethod = Interpolate.Linear(xListdouble.Take(tenIndex).ToList(), yListdouble.Take(tenIndex).ToList());
                var interpolatedList = xListdouble.Select(interpolationmethod.Interpolate).ToList();
                for (int index = 0; index < interpolatedList.Count; index++)
                {
                    double d = interpolatedList[index];
                    if (Math.Abs(d - yListdouble[index]) > 1)
                    {
                        mainindex = index;
                        break;
                    }
                }

                pinList.Add(xListdouble[mainindex]);
                poutList.Add(yListdouble[mainindex]);
                pexit.Add(yListdouble[yListdouble.Count - 1]);


            }

            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(pinList, "Р<sub>лин вх(Рвых1дБ)</sub>", "дБм"),
                    GetFullStatisticsFromList(poutList,  "Р<sub>вых1дБ</sub> (Рвых при компрессии Ку=1дБ)", "дБм"),
                    GetFullStatisticsFromList(pexit,  "Р<sub>выхнас</sub> (Рвых в режиме насыщения)", "дБм"),
                    
                   
                };
            return returnList;
        }

        private List<Statistics> GetS21PinMax(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var poutList = commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()).Select(yListdouble => yListdouble[0]).ToList();
            var returnList = new List<Statistics>
                {
                     GetFullStatisticsFromList(poutList, "Ку в малосигнальном режиме", "дБ"),
                   
                };
            return returnList;
        }

        private List<Statistics> GetS21Pin(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            var pinList = new List<double>();
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                var interpolation = MathNet.Numerics.Interpolate.Linear(xListdouble.Take(15), yListdouble.Take(15));
                var interpolationList = xListdouble.Select(x => interpolation.Interpolate(x)).ToList();
                var pList = new List<double>();
                for (int i = 0; i < xListdouble.Count; i++)
                {
                    if (Math.Abs(interpolationList[i] - yListdouble[i]) > 1)
                    {
                        pList.Add(yListdouble[i]);
                    }
                }

                pinList.Add(pList.FirstOrDefault());

            }
          

            var returnList = new List<Statistics>
                {
                     GetFullStatisticsFromList(pinList, "Р<sub>вых1дБ</sub> (Рвых при компрессии Ку=1дБ)", "дБм"),

                };
            return returnList;
        }

        private List<Statistics> GetS21PinGain(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = xList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList();
            var poutList = new List<double>();
            var pinList = new List<double>();
            var mainindex = 0;
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture) / divider).ToList()))
            {
                for (int index = 0; index < yListdouble.Count; index++)
                {
                    double d = yListdouble[index];
                    if (Math.Abs(d - yListdouble[0]) > 1)
                    {
                        mainindex = index;
                        break;
                    }
                }
                pinList.Add(yListdouble[0]);
                poutList.Add(xListdouble[mainindex]);


            }
            
            var returnList = new List<Statistics>
                {
                     GetFullStatisticsFromList(pinList, "Ку в малосигнальном режиме", "дБ"),
                     GetFullStatisticsFromList(poutList,  "Р<sub>вх1дБ</sub> (Рвх при компрессии Ку=1дБ)", "дБм")
                };
            return returnList;
        }




        private List<Statistics> GetVboandN(List<string> xList, IEnumerable<List<string>> commonYList, double divider)
        {
            List<double> xListdouble = new List<double>();
            List<int> indexesList = new List<int>();

            List<double> xListdoubleforn = new List<double>();
            List<int> indexesListforn = new List<int>();

            List<double> xListdoublefornn = new List<double>();
            List<int> indexesListfornn = new List<int>();


            for (int index = 0; index < xList.Count; index++)
            {
                var x = double.Parse(xList[index], CultureInfo.InvariantCulture);
                if (0.15 <= x && x <= 0.2)
                {
                    xListdouble.Add(x);
                    indexesList.Add(index);
                }
                if (0.2 <= x && x <= 0.75)
                {
                    xListdoubleforn.Add(x);
                    indexesListforn.Add(index);
                }
                if (0.2 <= x && x <= 0.75)
                {
                    xListdoublefornn.Add(x);
                    indexesListfornn.Add(index);
                }

            }

            var vboList = new List<double>();
            var nList = new List<double>();
            var n1List = new List<double>();

          

           
            foreach (List<double> yListdouble in commonYList.Select(yList => yList.Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToList()))
            {
                var yListdoubletemp = new List<double>();
                var yListdoubletempforn = new List<double>();
                var yListdoubletempfornn = new List<double>();
                for (int index = indexesList.FirstOrDefault(); index < indexesList.Last() + 1; index++)
                {
                    var ydouble = yListdouble[index];
                    yListdoubletemp.Add(ydouble);
                }

                for (int index = indexesListforn.FirstOrDefault(); index < indexesListforn.Last() + 1; index++)
                {
                    var ydouble = yListdouble[index];
                    yListdoubletempforn.Add(ydouble);
                }

                for (int index = indexesListfornn.FirstOrDefault(); index < indexesListfornn.Last() + 1; index++)
                {
                    var ydouble = yListdouble[index];
                    yListdoubletempfornn.Add(ydouble);
                }

                var yListdoublelog10 = yListdoubletemp.Select(Math.Log10).ToList();
                var yListdoublelog10Forn = yListdoubletempforn.Select(Math.Log10).ToList();
            //    var yListdoublelog10Fornn = yListdoubletempfornn.Select(Math.Log10).ToList();
                var interpolationmethodvpo = Interpolate.Linear(xListdouble, yListdoublelog10);
                var interpolationmethodvpoforn = Interpolate.Linear(xListdoubleforn, yListdoublelog10Forn);
             //  var interpolationmethodvpofornn = Interpolate.Linear(xListdoublefornn, yListdoublelog10Fornn);
                var Is = interpolationmethodvpo.Interpolate(0);
                var nnList = new List<double>();
              
                
             //   var dn = interpolationmethodvpofornn.Differentiate(0.6);
                const double contactArea = 7.5E-9;
                const double A = 8.17E+4;//постоянная Ричардсона
                const int T = 295;
                const double k = 8.617332E-5;
                var k2 = 1.380648E-23;//постоянная Больцмана
                var e = 1.60218E-19;
                var vbo = k * T * Math.Log(10) * (Math.Log10(contactArea * A * Math.Pow(T, 2)) - Is);
                for (double i = 0.2; i < 0.75; i = i + 0.0025)
                {
                    var a = 1/Math.Log(10)*(e/(k2*T))*(1/interpolationmethodvpoforn.Differentiate(i));
                    if (!Double.IsNaN(a))
                    {
                        nnList.Add(a);
                    }
                    
                }
               // var n1 = (1 / Math.Log(10)) * (e / (k2 * T)) * (1 / dn);
                vboList.Add(vbo);
                var newList = nnList.Where(d => !Double.IsNaN(d) && !Double.IsInfinity(d) && Math.Abs(d) > 1.1).Select(Math.Abs).ToList();
             //   var t = MathNet.Numerics.Statistics.Statistics.MovingAverage(newList, newList.Count).ToList().Select(Math.Abs);
                if (newList.Count != 0)
                {
                     nList.Add(newList.Min());
                }
              
             // n1List.Add(n1);
            }

            
            var returnList = new List<Statistics>
                {
                    GetFullStatisticsFromList(vboList, "Vbo (высота барьера Шоттки)", "эВ", 13),
                    GetFullStatisticsFromList(nList, "n (коэффициент неидеальности)", " ", 16),
                   
                 
                };
            return returnList;
        }

        public bool UseScietificNotation(IEnumerable<List<string>> commonYList)
        {
            var boolList = new List<bool>();
            foreach (var list in commonYList)
            {
                var boolsinglelistList = new List<bool>();
                foreach (string s in list)
                {

                    if (Math.Abs(double.Parse(s, CultureInfo.InvariantCulture)) > 1E-3 && Math.Abs(double.Parse(s, CultureInfo.InvariantCulture)) < 1000)
                    {
                        boolsinglelistList.Add(false);
                    }
                    else
                    {
                        boolsinglelistList.Add(true);
                    }

                }
                var boolcount = boolsinglelistList.Count(x => !x);
                boolList.Add(boolcount > 0);
            }
            var boolcountmain = boolList.Count(x => x);
            return !(boolcountmain > 0);
        }





    }


}