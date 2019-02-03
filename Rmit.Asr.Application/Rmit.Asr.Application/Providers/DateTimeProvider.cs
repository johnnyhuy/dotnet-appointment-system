using System;

namespace Rmit.Asr.Application.Providers
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private DateTime? _now;

        public DateTimeProvider()
        {
        }

        public DateTimeProvider(DateTime? now)
        {
            _now = now;
        }
        
        public DateTime Now()
        {
            return _now ?? DateTime.Now;
        }
    }
}