using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class AppLib
    {
        public static string WriteLogState = "On";
        public static string WriteLogFileName = "NUBEDC_log";

        public static void WriteLog(String str)
        {
            if (WriteLogState.ToLower() != "off")
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(Path.GetTempPath() + WriteLogFileName + ".txt", true))
                    {
                        writer.WriteLine(str);
                    }
                }
                catch (Exception) { }
            }

        }

        public static void WriteLog(String str, params object[] args)
        {
            if (WriteLogState.ToLower() != "off")
            {
                try
                {
                    WriteLog(string.Format(str, args));
                }
                catch (Exception ex) { WriteLog(ex); }
            }
        }


        public static void WriteLogDT(String str)
        {
            if (WriteLogState.ToLower() != "off")
            {
                try
                {
                    WriteLog(string.Format("{0:dd/MM/yyyy hh:mm:ss} => {1}", DateTime.Now, str));                    
                }
                catch (Exception) { }
            }

        }

        
        public static void WriteLogDT(String str, params object[] args)
        {
            if (WriteLogState.ToLower() != "off")
            {
                try
                {
                    WriteLogDT(string.Format(str, args));
                }
                catch (Exception ex) { WriteLog(ex); }
            }
        }


        public static void WriteLog(Exception ex)
        {
            WriteLogDT(string.Format("Error=> ExMessage:{0},StackTrace:{1}", ex.Message, ex.StackTrace));
        }

        public static void DisplayMsg(int Left, int Top, string Msg)
        {
            Console.CursorLeft = Left;
            Console.CursorTop = Top;
            Console.WriteLine(Msg);
        }
        public static string ReadMsg(int Left, int Top)
        {
            Console.CursorLeft = Left;
            Console.CursorTop = Top;
            return Console.ReadLine();
        }
        public static void DisplayClear()
        {
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();
        }

    }

    public struct DateTimeSpan
    {
        private readonly int years;
        private readonly int months;
        private readonly int days;
        private readonly int hours;
        private readonly int minutes;
        private readonly int seconds;
        private readonly int milliseconds;

        public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            this.years = years;
            this.months = months;
            this.days = days;
            this.hours = hours;
            this.minutes = minutes;
            this.seconds = seconds;
            this.milliseconds = milliseconds;
        }

        public int Years { get { return years; } }
        public int Months { get { return months; } }
        public int Days { get { return days; } }
        public int Hours { get { return hours; } }
        public int Minutes { get { return minutes; } }
        public int Seconds { get { return seconds; } }
        public int Milliseconds { get { return milliseconds; } }

        enum Phase { Years, Months, Days, Done }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            DateTimeSpan span = new DateTimeSpan();
            int officialDay = current.Day;

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                current = current.AddDays(officialDay - current.Day);
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }
    }
}
