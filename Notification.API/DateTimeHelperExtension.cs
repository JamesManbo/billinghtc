using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notification.API
{
    public static class DateTimeHelperExtension
    {
        //public static string ToReadableTime(this DateTime value)
        //{
        //    var ts = new TimeSpan(DateTime.Now.Ticks - value.Ticks);
        //    double delta = ts.TotalSeconds;
        //    if (delta < 60)
        //    {
        //        return "Vài giây trước";
        //    }
        //    if (delta < 120)
        //    {
        //        return "Một phút trước";
        //    }
        //    if (delta < 2700) // 45 * 60
        //    {
        //        return ts.Minutes + " phút trước";
        //    }
        //    if (delta < 5400) // 90 * 60
        //    {
        //        return "Một giờ trước";
        //    }
        //    if (delta < 86400) // 24 * 60 * 60
        //    {
        //        return ts.Hours + " giờ trước";
        //    }
        //    if (delta < 172800) // 48 * 60 * 60
        //    {
        //        return "Hôm qua lúc " + value.ToString("HH:mm");
        //    }
        //    if (delta < 2592000) // 30 * 24 * 60 * 60
        //    {
        //        return ts.Days + " ngày trước";
        //    }
        //    if (delta < 31104000) // 12 * 30 * 24 * 60 * 60
        //    {
        //        int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
        //        return months <= 1 ? "Một tháng trước" : months + " tháng trước";
        //    }
        //    var years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
        //    return years <= 1 ? "Một năm trước" : years + " năm trước";
        //}
    }
}
