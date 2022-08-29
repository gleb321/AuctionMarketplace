using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Timers;

namespace AuctionLiveService.Services {
    public class TimerService {
        private readonly Timer _timer;
        private readonly ConcurrentDictionary<string, Action> _auctionTimeEvents;

        /// <summary>
        /// Конструктор класса TimerService
        /// </summary>
        /// <param name="minutes">Временной интервал раз в который происходит запуск аукцинов</param>
        /// <param name="auctionTimeEvents">Словарь, хранящий в себя методы запуска и остановки аукционов</param>
        public TimerService(double minutes, ConcurrentDictionary<string, Action> auctionTimeEvents) {
            _timer = new Timer();
            _timer.Interval = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            _timer.Elapsed += TimerEventHandler;
            _auctionTimeEvents = auctionTimeEvents;
        }

        private void TimerEventHandler(object? sender, ElapsedEventArgs elapsedEventArgs) {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            if (_auctionTimeEvents.ContainsKey(time)) {
                _auctionTimeEvents[time].Invoke();
                _auctionTimeEvents.Remove(time, out var unused);
            }
        }
    }
}