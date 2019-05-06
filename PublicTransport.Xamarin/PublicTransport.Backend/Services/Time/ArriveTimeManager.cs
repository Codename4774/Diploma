using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GTFS.Entities;
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

        private IBackendConfiguration backendConfiguration;

        private Timer _managerTimer;

        private int _timerPeriod;

        private List<IArriveTimeNotificator> _arriveTimeNotificators;

        private DateTime _prevDateTime;

        public ArriveTimeManager(IBackendConfiguration backendConfiguration, bool calculateNearestArrives)
        {
            _arriveTimeNotificators = new List<IArriveTimeNotificator>();
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
            }

            _prevDateTime = currentDateTime;
        }

        public event EventHandler DayChanged;

        public IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<string> times)
        {
            IArriveTimeNotificator result = new ArriveTimeNotificator(times, _minutesBefore, _minutesAfter, _calculateNearestArrives);

            _arriveTimeNotificators.Add(result);

            return result;
        }

        public IArriveTimeNotificator GetArriveTimeNotificator(IEnumerable<TimeOfDay> times)
        {
            IArriveTimeNotificator result = new ArriveTimeNotificator(times, _minutesBefore, _minutesAfter, _calculateNearestArrives);

            _arriveTimeNotificators.Add(result);

            return result;
        }

        public void Dispose()
        {
            _managerTimer.Dispose();
        }
    }
}
