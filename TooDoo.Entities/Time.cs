using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TooDoo.Entities
{
    public class Time
    {
        /// <summary>
        /// The total time all tasks will take to finnish
        /// </summary>
        public string TotalTime { get; set; }

        /// <summary>
        /// The time when all tasks will be finished
        /// </summary>
        public string TimeWhenFinished { get; set; }

        /// <summary>
        /// Returns how long all tasks will take to finish
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string GetTotalTime(int minutes)
        {
            var timeSpan = TimeSpan.FromMinutes(minutes);

            return string.Format("{0} dagar, {1} timmar, {2} minuter",
                timeSpan.Days,
                timeSpan.Hours,
                timeSpan.Minutes);
        }

        /// <summary>
        /// Returns the time when all tasks will be finished
        /// </summary>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static string GetTimeWhenFinished(int minutes)
        {
            var timeSpan = TimeSpan.FromMinutes(minutes);

            // If tasks take more than a day show date and time
            if (timeSpan.Days > 0)
            {
                return string.Format(
                    DateTime.Now
                    .AddDays(timeSpan.Days)
                    .AddHours(timeSpan.Hours)
                    .AddMinutes(timeSpan.Minutes).ToString("yyyy-MM-dd HH:mm"));
            }
            //...otherwise only show the time
            else
            {
                return string.Format(
                    DateTime.Now
                    .AddHours(timeSpan.Hours)
                    .AddMinutes(timeSpan.Minutes).ToString("HH:mm"));
            }
        }
    }
}
