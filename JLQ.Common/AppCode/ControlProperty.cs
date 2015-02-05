using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace JLQ.Common
{
    public class ControlProperty
    {
        //设置控件的深度缓存。避免刷屏的效果
        public static void SetDoubleBuffered(dynamic obj)
        {
            obj.GetType().GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic).SetValue(obj, true, null);
        }

        //public static void SetOptimizedDoubleBuffer(dynamic obj)
        //{
        //    obj.GetType().GetProperty("OptimizedDoubleBuffer", System.Reflection.BindingFlags.Instance
        //        | System.Reflection.BindingFlags.NonPublic).SetValue(obj, true, null);
        //}

        //public static void SetAllPaintingInWmPaint(dynamic obj)
        //{
        //    obj.GetType().GetProperty("AllPaintingInWmPaint", System.Reflection.BindingFlags.Instance
        //        | System.Reflection.BindingFlags.NonPublic).SetValue(obj, true, null);
        //}


        //public static void SetDoubleBuffered(Control control)
        //{
        //    // set instance non-public property with name "DoubleBuffered" to true
        //    typeof(Control).InvokeMember("DoubleBuffered",
        //        BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
        //        null, control, new object[] { true });
        //}
    }
}