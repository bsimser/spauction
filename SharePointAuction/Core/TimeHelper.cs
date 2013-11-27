using System;
using System.Text;

namespace SharePointAuction.Core
{
    public static class TimeHelper
    {
        public static bool AuctionHasEnded(DateTime endDate)
        {
            return DateTime.Now > endDate;
        }

        public static bool AuctionHasStarted(DateTime startDate)
        {
            return DateTime.Now > startDate;
        }

        public static string GetTimeLeft(DateTime endTime)
        {
            var sb = new StringBuilder();
            var now = DateTime.Now;

            if (endTime >= now)
            {
                var timeLeft = endTime - now;
                var days = timeLeft.Days;
                var hours = timeLeft.Hours;
                var minutes = timeLeft.Minutes;
                var seconds = timeLeft.Seconds;

                if (days > 0)
                {
                    sb.AppendFormat("{0}d ", days);
                }
                if (hours > 0)
                {
                    sb.AppendFormat("{0}h ", hours);
                }
                if (minutes > 0)
                {
                    sb.AppendFormat("{0}m ", minutes);
                }
                if (seconds > 0)
                {
                    sb.AppendFormat("{0}s", seconds);
                }
            }
            else
            {
                sb.Append("Ended");
            }

            return sb.ToString();
        }
    }
}