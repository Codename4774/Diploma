using GTFS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PublicTransport.Backend.Models
{
    public class TimeItem
    {
        public string FormattedTime { get; set; }

        public TimeItem(IEnumerable<StopTime> stopTimes)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(stopTimes.Count() > 0 ? stopTimes.First().ArrivalTime.Value.Hours.ToString("00") + ": " : "");

            var orderedStopTimes = stopTimes.OrderBy(stopTime => stopTime.ArrivalTime.Value.Minutes);

            foreach (StopTime stopTime in orderedStopTimes)
            {
                stringBuilder.Append(stopTime.ArrivalTime.Value.Minutes.ToString("00") + " ");
            }

            FormattedTime = stringBuilder.ToString();
        }
    }
}
