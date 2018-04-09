using System;
using System.Collections.Generic;
using System.Text;

namespace Agora.DynamicKey.Extersions
{
    public static class DateTimeExtersion
    {
        /// <summary>
        /// 转换成时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int ToTimeStamp(this System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
    }
}
