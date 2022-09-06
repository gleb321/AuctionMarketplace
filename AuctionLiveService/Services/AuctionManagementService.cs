using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Timers;

namespace AuctionLiveService.Services {
    public class AuctionManagementService {
        private readonly System.Timers.Timer _timer;
        private AuctionAlertService _alertService;
        public readonly ConcurrentDictionary<string, Func<Task>> AuctionTimeEvents;

        /// <summary>
        /// Конструктор класс AuctionManagementService
        /// </summary>
        /// <param name="minutes">Временной интервал раз в который происходит запуск аукцинов</param>
        /// <param name="alertService">Сервис оповещения</param>
        public AuctionManagementService(AuctionAlertService alertService, int minutes) {
            AuctionTimeEvents = new ConcurrentDictionary<string, Func<Task>>();
            _alertService = alertService;
            _timer = new System.Timers.Timer();
            _timer.Interval = TimeSpan.FromMinutes(minutes).TotalMilliseconds;
            _timer.Elapsed += TimerEventHandler;
            _timer.Start();
        }
        
        private async void TimerEventHandler(object? sender, ElapsedEventArgs elapsedEventArgs) {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            Console.WriteLine(time);
            if (AuctionTimeEvents.ContainsKey(time)) {
                //TODO Разобраться с exception(сейчас обрабатывается только последний метод в Invokation list)
                await AuctionTimeEvents[time].Invoke();
                AuctionTimeEvents.TryRemove(time, out var unused);
            }
        }

        public void Add(int id, string time, bool activityStatus) {
            Func<Task> sendAlert = async () => {
                await _alertService.SendAuctionActivationAlert(id, activityStatus);
            };

            if (!AuctionTimeEvents.ContainsKey(time)) {
                AuctionTimeEvents.TryAdd(time, sendAlert);
            } else {
                AuctionTimeEvents[time] += sendAlert;
            }
        }
    }
}