using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JLQ.Common
{
    public class Setting
    {
        //设置当前窗口。主要用于单实例且不阻挡的窗口
        public static Action<IntPtr> SetForegroundWindow = null;
    }
}
