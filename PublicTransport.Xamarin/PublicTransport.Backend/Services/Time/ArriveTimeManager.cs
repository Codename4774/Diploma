using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GTFS.Entities;
using PublicTransport.Backend.Models;
using PublicTransport.Backend.Services.Configuration;
using PublicTransport.Backend.Services.Time.TimeEvents;

namespace PublicTransport.Backend.Services.Time
{
    public class ArriveTimeManager : IArriveTimeManager, IDisposable
    {
        private const int _timerPeriodMillisec = 10000;

        private int _minutesBefore;

        private int _minutesAfter;

        private bool _calculateNearestArrives;

        private IBackendConfiguration _backendConfiguration;

        private Timer _managerTimer;

        private int _timerPeriod;

        private ICollection<NearestArriveTimeModel> _nearestArriveTimeModels;

        private List<IArriveTimeNotificator> _arriveTimeNotificators;

        private ICollection<NearestArriveTimeModel> _currentDisplayedItems;

        private DateTime _prevDateTime;

        public ArriveTimeManager(IBackendConfiguration backendConfiguration, bool calculateNearestArrives)
        {
            _arriveTimeNotificators = new List<IArriveTimeNotificator>();
            _nearestArriveTimeModels = new List<NearestArriveTimeModel>();
            _currentDisplayedItems = new List<NearestArriveTimeModel>();
            _managerTimer = new Timer(OnTimer, null, 0, _timerPeriodMillisec);
            _minutesAfter = Convert.ToInt32(backendConfiguration.GetProperty("MinutesAfter"));
            _minutesBefore = Convert.ToInt32(backendConfiguration.GetProperty("MinutesBefore"));
            _calculateNearestArrives = calculateNearestArrives;
        }

        private void OnTimer(object state)
        {
            DateTime currentDateTime = DateTime.Now;

            if (_prevDateTime.DayOfWeek != currentDateTime.DayOfWeek)
            {
                if (DayChanged != null)
                {
                    DayChanged(this, new EventArgs());
                }
            }
            else
            {
                foreach (IArriveTimeNotificator notificator in _arriveTimeNotificators)
                {
                    notificator.UpdateState(currentDateTime);
                }

                if (_calculateNearestArrives)
                {
                    ProcessNearestArriveTimes(currentDateTime);
                }              
            }

            _prevDateTime = currentDateTime;
            if (OnTick != null)
            {
                OnTick(this, new EventArgs());
            }
        }

        private void ProcessNearestArriveTimes(DateTime current)
        {
            lock (_nearestArriveTimeModels)
            {
                int totalMinutes = current.Hour * 60 + current.Minute;

                NearestArriveTimeModel[] timesToAdd = _nearestArriveTimeModels.Where(time =>
                {
                    int timeDiffMin = (time.ArriveTime.Hours * 60 + time.ArriveTime.Minutes) - totalMinutes;

                    return timeDiffMin > -_minutesBefore && timeDiffMin < _minutesAfter;
                }).ToArray();


                foreach (NearestArriveTimeModel nearestArriveTimeModel in _nearestArriveTimeModels)
                {
                    nearestArriveTimeModel.MinutesToArrive = -MinutesDiffWithCurrent(current, nearestArriveTimeModel.ArriveTime);
                }

                if (timesToAdd.Length != 0)
                {
                    var temp = _currentDisplayedItems;

                    _currentDisplayedItems = timesToAdd;

                    for (int i = 0; i < timesToAdd.Length; i++)
                    {
                        if (!temp.Contains(timesToAdd[i]))
                        {
                            if (OnNearestArriveTimeShow != null)
                            {
                                OnNearestArriveTimeShow(this, timesToAdd[i]);
                            }
                        }
                    }

                    for (int i = 0; i < temp.Count; i++)
                    {
                        var element = temp.ElementAt(i);
                        if (!timesToAdd.Contains(element))
                        {
                            if (OnNearestArriveTimeHide != null)
                            {
                                OnNearestArriveTimeHide(this, element);
                            }
                        }
                    }
                }
            }
        }

        public event EventHandler DayChanged;
        public event EventHandler OnTick;
        public event EventHandler<NearestArriveTimeModel> OnNearestArriveTimeShow;
        public event EventHandler<NearestArriveTimeModel> OnNearestArriveTimeHide;

        public IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<string> times)
        {
            IArriveTimeNotificator result = new ArriveTimeNotificator(times);

            _arriveTimeNotificators.Add(result);

            return result;
        }

        public IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<TimeOfDay> times)
        {
            IArriveTimeNotificator result = new ArriveTimeNotificator(times);

            _arriveTimeNotificators.Add(result);

            return result;
        }

        public void Dispose()
        {
            _managerTimer.Dispose();
        }

        public void RemoveArriveTimeNotificator(IArriveTimeNotificator item)
        {
            _arriveTimeNotificators.Remove(item);
        }

        public IEnumerable<NearestArriveTimeModel> AddNearestArriveTimesToProcessing(IEnumerable<NearestArriveTimeModel> nearestArriveTimeModels)
        {
            foreach(NearestArriveTimeModel nearestArriveTimeModel in nearestArriveTimeModels)
            {
                _nearestArriveTimeModels.Add(nearestArriveTimeModel);
            }

            return InitNearestArriveTimesToShow(DateTime.Now);
        }


        private IEnumerable<NearestArriveTimeModel> InitNearestArriveTimesToShow(DateTime current)
        {
            lock (_nearestArriveTimeModels)
            {
                int totalMinutes = current.Hour * 60 + current.Minute;

                IEnumerable<NearestArriveTimeModel> timesToAdd = _nearestArriveTimeModels.Where(time =>
                {
                    int timeDiffMin = (time.ArriveTime.Hours * 60 + time.ArriveTime.Minutes) - totalMinutes;

                    return timeDiffMin > -_minutesBefore && timeDiffMin < _minutesAfter;
                });

                foreach (NearestArriveTimeModel nearestArriveTimeModel in timesToAdd)
                {
                    nearestArriveTimeModel.MinutesToArrive = -MinutesDiffWithCurrent(current, nearestArriveTimeModel.ArriveTime);
                }

                _currentDisplayedItems = timesToAdd.ToList();

                return timesToAdd;
            }
            //if (timesToAdd.Count() != 0)
            //{
            //    _nearestArriveTimes = timesToAdd.ToList();
            //    if (NearestArrivesChanged != null)
            //    {
            //        NearestArrivesChanged(this, new EventArgs());
            //    }
            //}

        }

        private int MinutesDiffWithCurrent(DateTime current, TimeOfDay timeOfDay)
        {
            return (current.Hour * 60 + current.Minute - timeOfDay.Hours * 60 - timeOfDay.Minutes);
        }

        private int MinutesDiff(int hours1, int minutes1, int hours2, int minutes2)
        {
            return (hours1 * 60 + minutes1 - hours2 * 60 - minutes2);
        }
    }
}
