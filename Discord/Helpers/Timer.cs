using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Discord.Helpers
{
    internal class Timer
    {
        private System.Threading.Timer? timer;
        public void SetUpTimer(TimeSpan alertTime,object passingObject, Action<object> method)
        {
            DateTime current = DateTime.Now;
            TimeSpan timeToGo = alertTime - current.TimeOfDay;
            if (timeToGo < TimeSpan.Zero)
            {
                return;//time already passed
            }
            this.timer = new System.Threading.Timer(x =>
            {
                method.Invoke(passingObject);
            }, null, timeToGo, Timeout.InfiniteTimeSpan);
        }
    }
}
