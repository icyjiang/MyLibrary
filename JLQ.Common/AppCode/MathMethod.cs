using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLQ.Common
{
    //C#写的数学方法，求因数、质数等，此类算法用F#算会更快。
    public class MathMethod4CSharp
    {
        /// <summary>
        /// 分解质因素。
        /// </summary>
        /// <param name="compositeNum"></param>
        /// <param name="res"></param>
        public static void GetSeparate(int compositeNum, ref List<int> res)
        {
            if (IsPrime(compositeNum))
            //if (FSharp.Library.MathMethod.IsPrime(compositeNum))
            {
                res.Add(compositeNum);
                return;
            }
            double sqrt = Math.Sqrt(compositeNum);
            for (int i = 2; i <= sqrt; i++)
            {
                bool isPrime =IsPrime(i);
                //bool isPrime = FSharp.Library.MathMethod.IsPrime(i);
                if (isPrime && compositeNum % i == 0)
                {
                    res.Add(i);
                    GetSeparate(compositeNum / i, ref res);
                    break;
                }
            }
        }

        /// <summary>
        /// 获取某个数值的因数，质数则至返回自己
        /// </summary>
        /// <param name="input"></param>
        /// <returns>List(因数)</returns>
        public static List<int> GetDivisor(int input)
        {
            List<int> res = new List<int>();
            if (IsPrime(input))
            //if (FSharp.Library.MathMethod.IsPrime(input))
            {
                res.Add(input);
                return res;
            }
            double sqrt = Math.Sqrt(input);
            for (int i = 1; i <= sqrt; i++)
            {
                if (input % i == 0)
                {
                    res.Add(input / i);
                    res.Add(i);
                }
            }
            return res.Distinct().OrderBy(x => x).ToList();
        }

        /// <summary>
        /// 判断是否为质数。注意执行一定次数之后会溢出堆栈，测试时为11619次。建议调用FSharp接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPrime(int input)
        {
            Func<int, bool> ToDo = null;
            ToDo = x =>
            {
                if (x > input / 2f) return true;
                if (input % x == 0) return false;
                else
                    return ToDo(x + 1);
            };
            return ToDo(2);
        }
    }
}
