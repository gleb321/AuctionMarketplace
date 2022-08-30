using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace AuctionLiveService.Services {
    public class TimerService {
        private readonly System.Timers.Timer _timer;
        private readonly ConcurrentDictionary<string, Func<Task>> _auctionTimeEvents;

        /// <summary>
        /// Конструктор класса TimerService
        /// </summary>
        /// <param name="minutes">Временной интервал раз в который происходит запуск аукцинов</param>
        /// <param name="auctionTimeEvents">Словарь, хранящий в себя методы запуска и остановки аукционов</param>
        public TimerService(double minutes, ConcurrentDictionary<string, Func<Task>> auctionTimeEvents) {
            Console.WriteLine("Timer created");
            _timer = new System.Timers.Timer();
            _timer.Interval = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            _timer.Elapsed += TimerEventHandler;
            _timer.Start();
            _auctionTimeEvents = auctionTimeEvents;
        }

        private async void TimerEventHandler(object? sender, ElapsedEventArgs elapsedEventArgs) {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            Console.WriteLine(time);
            if (_auctionTimeEvents.ContainsKey(time)) {
                await _auctionTimeEvents[time].Invoke();
                _auctionTimeEvents.Remove(time, out var unused);
            }
        }
    }
}