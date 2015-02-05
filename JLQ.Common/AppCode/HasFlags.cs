using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Numerics;

namespace JLQ.Common
{
    public static class Flags
    {
        const int min = 0, max32 = 31, max64 = 63, maxBigInt = 1000;//长度302，所以数据库准备字段长度最好定为350字长

        #region Int32
        public static int GetFlag32(params int[] arr)
        {
            return 0.FlagWith32(arr);
        }

        public static int FlagWith32(this int old, params int[] newArr)
        {
            int res = old;
            foreach (int read in newArr)
            {
                if (read >= min && read <= max32)
                    res |= 1 << read;
                else throw new ArgumentException(string.Format("值范围是[0-{0}]", max32));
            }
            return res;
        }

        public static bool HasFlag32(this int source, int witch)
        {
            return (source | (1 << witch)) == source;
        }

        public static int RemoveFlag32(this int source, int witch)
        {
            if (!source.HasFlag32(witch)) return source;
            return source - (1 << witch);
        }


        public static List<int> GetList32(this int source)
        {
            List<int> res = new List<int>();
            for (int i = min; i <= max32; i++)
            {
                if (source.HasFlag32(i))
                    res.Add(i);
            }
            return res;
        }
        #endregion

        #region Int64
        const long I = 1;
        public static long GetFlag64(params int[] arr)
        {
            return ((long)0).FlagWith64(arr);
        }

        public static long FlagWith64(this long old, params int[] newArr)
        {
            long res = old;
            foreach (int read in newArr)
            {
                if (read >= min && read <= max64)
                    res |= I << read;
                else throw new ArgumentException(string.Format("值范围是[0-{0}]", max64));
            }
            return res;
        }

        public static bool HasFlag64(this long source, int witch)
        {
            return (source | (I << witch)) == source;
        }

        public static long RemoveFlag64(this long source, int witch)
        {
            if (!source.HasFlag64(witch)) return source;
            return source - (I << witch);
        }

        public static List<int> GetList64(this long source)
        {
            List<int> res = new List<int>();
            for (int i = min; i <= max64; i++)
            {
                if (source.HasFlag64(i))
                    res.Add(i);
            }
            return res;
        }
        #endregion

        #region BigInteger
        static BigInteger bigI = 1;
        public static BigInteger GetFlagBigInt(params int[] arr)
        {
            return ((BigInteger)0).FlagWithBigInt(arr);
        }

        public static BigInteger FlagWithBigInt(this BigInteger old, params int[] newArr)
        {
            var res = old;
            foreach (int read in newArr)
            {
                if (read >= min && read <= maxBigInt)
                    res |= bigI << read;
                else throw new ArgumentException(string.Format("值范围是[0-{0}]", maxBigInt));
            }
            return res;
        }

        public static bool HasFlagBigInt(this BigInteger source, int witch)
        {
            return (source | (bigI << witch)) == source;
        }

        public static BigInteger RemoveFlagBigInt(this BigInteger source, int witch)
        {
            if (!source.HasFlagBigInt(witch)) return source;
            return source - (bigI << witch);
        }

        public static List<int> GetListBigInt(this BigInteger source, int max = maxBigInt)
        {
            if (max > maxBigInt) max = maxBigInt;
            List<int> res = new List<int>();
            for (int i = min; i <= max; i++)
            {
                if (source.HasFlagBigInt(i))
                    res.Add(i);
            }
            return res;
        }
        #endregion

    }


    public class Class1
    {
        public static void ddd()
        {
            var list = new List<int>() { 0, 1, 2, 3, 5, 8, 25, 30 };
            var i = 0;
            foreach (var read in list)
                i = i.FlagWith32(read);
            var b = i.HasFlag32(22);
            b = i.HasFlag32(30);
            var dd = i.GetList32();

            //Flags dd = Flags.A | Flags.D;
            //bool has = dd.HasFlag(Flags.D);
            //has = !has;
        }
    }
}


//public enum Flags : long
//{
//    A = I << 0,
//    B = I << 1,
//    C = I << 2,
//    D = I << 63,
//}