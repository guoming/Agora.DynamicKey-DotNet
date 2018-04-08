using System;
using System.Security.Cryptography;
using System.Text;
using DotAgoraDynamicKey.Extersions;
/// <summary>
/// 声网服务端集成
/// 作者：郭明
/// 日期：2016年8月16日
/// </summary>
namespace DotAgora.DynamicKey
{

    class DynamicKeyUtil
    {



        //数据签名
        public static string HMACSHA1Encrypt(string EncryptKey, string EncryptText)
        {
            HMACSHA1 myHMACSHA1 = new HMACSHA1(Encoding.UTF8.GetBytes(EncryptKey));
            byte[] RstRes = myHMACSHA1.ComputeHash(Encoding.UTF8.GetBytes(EncryptText));
            return bytesToHex(RstRes);
        }


        public static string bytesToHex(byte[] bytes)
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2").PadLeft(2, '0'));
            }

            return builder.ToString();
        }
    }

    /// <summary>
    /// 动态秘钥
    /// </summary>
    public class DynamicKey4
    {

        private static string RECORDING_SERVICE = "ARS";
        private static string MEDIA_CHANNEL_SERVICE = "ACS";


        private static string generateSignature4(string vendorKey,
            string signKey,
            string channelName,
            string unixTsStr,
            string randomIntStr,
            string uidStr,
            string expiredTsStr,
            string serviceType)
        {

            StringBuilder builder = new StringBuilder();

            builder.Append(serviceType);
            builder.Append(vendorKey);
            builder.Append(unixTsStr);
            builder.Append(randomIntStr);
            builder.Append(channelName);
            builder.Append(uidStr);
            builder.Append(expiredTsStr);
            string sign = DynamicKeyUtil.HMACSHA1Encrypt(signKey, builder.ToString());
            return sign;
            //return DynamicKeyUtil.bytesToHex(sign.getBytes());
        }

        private static string doGenerate(String vendorKey,
            String signKey,
            String channelName,
            int unixTs,
            int randomInt,
            long uid,
            int expiredTs,
            String serviceType)
        {
            String version = "004";
            String unixTsStr = ("0000000000" + unixTs.ToString()).Substring(unixTs.ToString().Length);
            String randomIntStr = ("00000000" + randomInt.ToString().toHexString()).Substring(randomInt.ToString().toHexString().Length).ToLower();
            uid = uid & 0xFFFFFFFFL;
            String uidStr = ("0000000000" + uid.ToString()).Substring(uid.ToString().Length);
            String expiredTsStr = ("0000000000" + expiredTs.ToString()).Substring(expiredTs.ToString().Length);
            String signature = generateSignature4(vendorKey, signKey, channelName, unixTsStr, randomIntStr, uidStr, expiredTsStr, serviceType);



            if (version.Length != 3)
                throw new Exception("version 3");

            if (signature.Length != 40)
                throw new Exception("signature 长度不是40");

            if (vendorKey.Length != 32)
                throw new Exception("vendorKey 长度不是32");


            if (unixTsStr.Length != 10)
                throw new Exception("vendorKey 长度不是10");

            if (randomIntStr.Length != 8)
                throw new Exception("randomIntStr 长度不是8");

            if (expiredTsStr.Length != 10)
                throw new Exception("randomIntStr 长度不是10");



            string result = string.Format("{0}{1}{2}{3}{4}{5}", version, signature, vendorKey, unixTsStr, randomIntStr, expiredTsStr);

            if (result.Length == 103)
            {
                return result;
            }
            else
            {
                throw new Exception("Key无效，长度必须是103");
            }

        }


        /**
         * Generate Dynamic Key for recording service
         * @param vendorKey Vendor key assigned by Agora
         * @param signKey Sign key assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs should be 0
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static String generateRecordingKey(String vendorKey,
            String signKey,
            String channelName,
            int unixTs,
            int randomInt,
            long uid,
            int expiredTs)
        {
            return doGenerate(vendorKey, signKey, channelName, unixTs, randomInt, uid, expiredTs, RECORDING_SERVICE);
        }

        /**
         * Generate Dynamic Key for media channel service
         * @param vendorKey Vendor key assigned by Agora
         * @param signKey Sign key assigned by Agora
         * @param channelName name of channel to join, limited to 64 bytes and should be printable ASCII characters
         * @param unixTs unix timestamp in seconds when generating the Dynamic Key
         * @param randomInt salt for generating dynamic key
         * @param uid user id, range from 0 - max uint32
         * @param expiredTs service expiring timestamp. After this timestamp, user will not be able to stay in the channel.
         * @return String representation of dynamic key
         * @throws Exception if any error occurs
         */
        public static String generateMediaChannelKey(String vendorKey,
            String signKey,
            String channelName,
            int unixTs,
            int randomInt,
            long uid,
            int expiredTs)
        {
            return doGenerate(vendorKey, signKey, channelName, unixTs, randomInt, uid, expiredTs, MEDIA_CHANNEL_SERVICE);
        }


    }
}
