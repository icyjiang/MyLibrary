using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace JLQ.Common
{
    public struct FlagsBitArray
    {
        int _length;
        BitArray _array;
        public int Length { get { return _length; } }

        //构造函数
        public FlagsBitArray(int length)
        {
            _length = length;
            _array = new BitArray(length);
        }
        //构造函数
        public FlagsBitArray(int[] arr)
        {
            if (arr == null)
            {
                _length = 0;
                _array = new BitArray(0);
            }
            else
            {
                var length = arr.Max();
                _length = length;
                _array = new BitArray(length);
                AddFlag(arr);
            }
        }

        void ModifyFlag(int[] arr, bool value)
        {
            if (arr == null) return;
            var max = arr.Max();
            if (max > _length - 1)//如果提交的数字有超出当前索引范围的
            {
                _array.Length = max + 1;
                _length = max + 1;
            }
            foreach (var x in arr)
            {
                if (x >= 0) _array[x] = value;
                else throw new ArgumentException("负值无效");
            }
        }

        //加法
        public void AddFlag(params int[] newArr) { ModifyFlag(newArr, true); }

        //移除
        public void RemoveFlag(params int[] newArr) { ModifyFlag(newArr, false); }

        //判断有没有
        public bool HasFlag(int which)
        {
            //超出范围，返回false
            if (which < 0 || which > Length - 1) return false;
            return _array[which];
        }

        //如果Length很大的时候，而且string需要存储到数据库时，可以考虑压缩算法。
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(this.Length) { Length = this.Length };
            for (int i = 0; i < this.Length; i++)
                sb[i] = this._array[i] ? '1' : '0';
            return sb.ToString();
        }

        //FlagsBitArray与String的转换
        public static implicit operator string(FlagsBitArray bitArr)
        {
            return bitArr.ToString();
        }

        //String与FlagsBitArray的隐式转换
        public static implicit operator FlagsBitArray(string str)
        {
            FlagsBitArray res = new FlagsBitArray(str.Length);
            for (int i = 0; i < str.Length; i++)
                res._array[i] = str[i] == '1';
            return res;
        }
    }
}
