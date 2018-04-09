using System;
using Agora.DynamicKey;
using Agora.DynamicKey.Extersions;
namespace Agora.DynamicKeyUnitTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var vendor_key = "970ca35de60c44645bbae8a215061b33";
            var sign_key = "7d72365eb983485397e3e3f9d460bdda";
            //int expiredTs =DateTime.Now.AddSeconds(Duration-TotalTime).ToTimeStamp();//服务截止时间戳（2小时）
            int expiredTs = DateTime.Now.AddHours(2).ToTimeStamp();//服务截止时间戳（2小时）
            int unixTs = DateTime.Now.ToTimeStamp();//本次请求时间戳
            int randomInt = new Random().Next() * 100000000;
            var identifier = 0;
            var channelId = 0;

            // Generates Key for user to join Channel
            String media_channel_key = DynamicKey4.generateMediaChannelKey(vendor_key,
                                                                            sign_key,
                                                                            channelId.ToString(),
                                                                            unixTs,
                                                                            randomInt,
                                                                            identifier,
                                                                            expiredTs);
            Console.WriteLine($"channel key:{media_channel_key}");
        
        }

    }
}
