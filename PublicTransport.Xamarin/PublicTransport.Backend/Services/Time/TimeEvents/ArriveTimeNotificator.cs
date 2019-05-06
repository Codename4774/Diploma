using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GTFS.Entities;

namespace PublicTransport.Backend.Services.Time.TimeEvents
{
    public class ArriveTimeNotificator : IArriveTimeNotificator
    {
        private bool _needToProcess;

        private int _minutesBefore;

        private int _minutesAfter;

        private bool _calculateNearestArrives;

        private TimeOfDay _nextArriveTime;

        private int _minutesToNextArrive;

        private List<TimeOfDay> _times;

        private List<TimeOfDay> _nearestArriveTimes;

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

        public IEnumerable<TimeOfDay> NearestArrivesTimes
        {
            get
            {
                return _nearestArriveTimes;
            }
        }

        public event EventHandler NextArriveTimeChanged;
        public event EventHandler NearestArrivesChanged;
        public event EventHandler NoArrivesToday;
        public event EventHandler MinutesToNextArriveChanged;

        private static char[] _seperator = new char[] { ':' };

        private ArriveTimeNotificator(int minutesBefore, int minutesAfter, bool calculateNearestArrives)
        {
            _minutesBefore = minutesBefore;
            _minutesAfter = minutesAfter;
            _calculateNearestArrives = calculateNearestArrives;
            _needToProcess = true;
        }

        public ArriveTimeNotificator(IEnumerable<string> times, int minutesBefore, int minutesAfter, bool calculateNearestArrives) 
            : this(minutesBefore, minutesAfter, calculateNearestArrives)
        {
            _times = times.Select(timeStr =>
            {
                TimeOfDay timeOfDay = new TimeOfDay();
                string[] values = timeStr.Split(_seperator, StringSplitOptions.RemoveEmptyEntries);
                timeOfDay.Hours = Convert.ToInt32(values[0]);
                timeOfDay.Minutes = Convert.ToInt32(values[1]);

                return timeOfDay;
            }).ToList();

            _nextArriveTime = _times.FirstOrDefault();
            _minutesToNextArrive = MinutesDiffWithCurrent(DateTime.Now, _nextArriveTime);
            UpdateState(DateTime.Now);
        }

        public ArriveTimeNotificator(IEnumerable<TimeOfDay> times, int minutesBefore, int minutesAfter, bool calculateNearestArrives)
            : this(minutesBefore, minutesAfter, calculateNearestArrives)
        {
            _times = times.ToList();
            _nextArriveTime = _times.FirstOrDefault();
        }


        public void UpdateState(DateTime dateTime)
        {
            int currentHour = dateTime.Hour;
            int currentMinutes = dateTime.Minute;

            ProcessNextArriveTime(currentHour, currentMinutes);

            if (_calculateNearestArrives)
            {
                ProcessNearestArriveTimes(currentHour, currentMinutes);
            }
        }

        private void ProcessNearestArriveTimes(int currentHour, int currentMinutes)
        {
            if (_calculateNearestArrives)
            {
                int totalMinutes = currentHour * 60 + currentMinutes;

                if (_needToProcess)
                {
                    IEnumerable<TimeOfDay> timesToAdd = _times.Where(time =>
                    {
                        int timeDiffMin = totalMinutes - (time.Hours * 60 + time.Minutes);

                        return timeDiffMin > -_minutesBefore && timeDiffMin < _minutesAfter;
                    });

                    if (timesToAdd.Count() != 0)
                    {
                        _nearestArriveTimes = timesToAdd.ToList();
                        if (NearestArrivesChanged != null)
                        {
                            NearestArrivesChanged(this, new EventArgs());
                        }
                    }
                }
            }
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

        private void ProcessNextArriveTime(int currentHour, int currentMinutes)
        {
            if (_needToProcess)
            {
                _minutesToNextArrive = MinutesDiffWithCurrent(DateTime.Now, _nextArriveTime);

                if (MinutesToNextArriveChanged != null)
                {
                    MinutesToNextArriveChanged(this, new EventArgs());
                }

                if (MinutesDiff(currentHour, currentMinutes, _nextArriveTime.Hours, _nextArriveTime.Minutes) < 0)
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
                                                                && time.Minutes == _nextArriveTime.Minutes)];


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
