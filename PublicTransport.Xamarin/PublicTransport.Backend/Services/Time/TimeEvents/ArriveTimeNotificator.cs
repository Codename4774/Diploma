using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTFS.Entities;
using PublicTransport.Backend.Models;

namespace PublicTransport.Backend.Services.Time.TimeEvents
{
    public class ArriveTimeNotificator : IArriveTimeNotificator
    {
        private bool _needToProcess;

        private TimeOfDay _nextArriveTime;

        private int _minutesToNextArrive;

        private List<TimeOfDay> _times;

        public TimeOfDay NextArriveTime
        {
            get
            {
                return _nextArriveTime;
            }
        }

        public int MinutesToNextArrive
        {
            get
            {
                return - _minutesToNextArrive;
            }
        }

        public event EventHandler NextArriveTimeChanged;
        public event EventHandler NoArrivesToday;
        public event EventHandler MinutesToNextArriveChanged;

        private static char[] _seperator = new char[] { ':' };

        private ArriveTimeNotificator()
        {
            _needToProcess = true;
        }

        public ArriveTimeNotificator(IEnumerable<string> times) 
            : this()
        {
            _times = times.Select(timeStr =>
            {
                TimeOfDay timeOfDay = new TimeOfDay();
                string[] values = timeStr.Split(_seperator, StringSplitOptions.RemoveEmptyEntries);
                timeOfDay.Hours = Convert.ToInt32(values[0]);
                timeOfDay.Minutes = Convert.ToInt32(values[1]);

                return timeOfDay;
            }).ToList();

            DateTime dateTime = DateTime.Now;

            _nextArriveTime = SetNearestArrive(dateTime.Hour, dateTime.Minute);
            _minutesToNextArrive = MinutesDiffWithCurrent(DateTime.Now, _nextArriveTime);
            UpdateState(dateTime);
        }

        public ArriveTimeNotificator(IEnumerable<TimeOfDay> times)
            : this()
        {
            _times = times.ToList();

            DateTime dateTime = DateTime.Now;

            _nextArriveTime = SetNearestArrive(dateTime.Hour, dateTime.Minute);
            _minutesToNextArrive = MinutesDiffWithCurrent(DateTime.Now, _nextArriveTime);
            UpdateState(dateTime);
        }


        public void UpdateState(DateTime dateTime)
        {
            int currentHour = dateTime.Hour;
            int currentMinutes = dateTime.Minute;

            ProcessNextArriveTime(currentHour, currentMinutes);
        }

        private int MinutesDiffWithCurrent(DateTime current, TimeOfDay timeOfDay)
        {
            return (current.Hour * 60 + current.Minute - timeOfDay.Hours * 60 - timeOfDay.Minutes);
        }

        private int MinutesDiff(int hours1, int minutes1, int hours2, int minutes2)
        {
            return (hours1 * 60 + minutes1 - hours2 * 60 - minutes2);
        }

        //private ProcessNearestTimesToAdd(IEnumerable<TimeOfDay> timeToAdd)
        //{
        //    foreach (TimeOfDay timeOfDay in timeToAdd)
        //    {
        //        foreach (TimeOfDay nearestTimeOfDay in _nearestArriveTimes)
        //        {
        //            if (timeOfDay.Hours != _nextArriveTime.Hours || timeOfDay.Minutes != _nextArriveTime.Minutes)
        //            {
        //            }
        //        }

        //    }
        //}

        private TimeOfDay SetNearestArrive(int currentHour, int currentMinutes)
        {
            IEnumerable<TimeOfDay> result = _times.Where(time => currentHour * 60 + currentMinutes - time.Hours * 60 - time.Minutes < 0);

            if (result.Count() == 0)
            {
                _needToProcess = false;
                if (NoArrivesToday != null)
                {
                    NoArrivesToday(this, new EventArgs());
                }

                return default(TimeOfDay);
            }
            else
            {
                return result.First();
            }
        }

        private void ProcessNextArriveTime(int currentHour, int currentMinutes)
        {
            if (_needToProcess)
            {
                _minutesToNextArrive = MinutesDiffWithCurrent(DateTime.Now, _nextArriveTime);

                if (MinutesToNextArriveChanged != null)
                {
                    MinutesToNextArriveChanged(this, new EventArgs());
                }

                if (MinutesDiff(currentHour, currentMinutes, _nextArriveTime.Hours, _nextArriveTime.Minutes) > 0)
                {
                    TimeOfDay last = _times.Last();

                    if (last.Hours == _nextArriveTime.Hours && last.Minutes == _nextArriveTime.Minutes)
                    {
                        _needToProcess = false;
                        if (NoArrivesToday != null)
                        {
                            NoArrivesToday(this, new EventArgs());
                        }
                    }
                    else
                    {
                        _nextArriveTime = _times[_times.FindIndex(time => time.Hours == _nextArriveTime.Hours
                                                                && time.Minutes == _nextArriveTime.Minutes) + 1];


                        if (NextArriveTimeChanged != null)
                        {
                            NextArriveTimeChanged(this, new EventArgs());
                        }
                    }
                }
            }
        }
    }
}
