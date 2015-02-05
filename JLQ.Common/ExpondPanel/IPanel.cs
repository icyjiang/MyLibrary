using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace JLQ.Common
{
    public interface IPanel
    {
        PanelStyle PanelStyle { get; set;}
        ColorScheme ColorScheme { get; set;}
        bool ShowBorder { get; set;}
		bool ShowExpandIcon { get; set;}
		bool ShowCloseIcon  { get; set;}
        bool Expand { get; set; }
    }
}
