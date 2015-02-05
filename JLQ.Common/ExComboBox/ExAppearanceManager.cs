namespace JLQ.Common
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    internal class ExAppearanceManager
    {
        private static ExAppearanceManager apprMgr;
        private const int CBB_DISABLED = 4;
        private const int CBB_FOCUSED = 3;
        private const int CBB_HOT = 2;
        private const int CBB_NORMAL = 1;
        private const int CBCB_DISABLED = 4;
        private const int CBCB_HOT = 2;
        private const int CBCB_NORMAL = 1;
        private const int CBCB_PRESSED = 3;
        private const int CBRO_DISABLED = 4;
        private const int CBRO_HOT = 2;
        private const int CBRO_NORMAL = 1;
        private const int CBRO_PRESSED = 3;
        private const int CBTBS_DISABLED = 3;
        private const int CBTBS_FOCUSED = 4;
        private const int CBTBS_HOT = 2;
        private const int CBTBS_NORMAL = 1;
        private const int CBXS_DISABLED = 4;
        private const int CBXS_HOT = 2;
        private const int CBXS_NORMAL = 1;
        private const int CBXS_PRESSED = 3;
        private const int CBXSL_DISABLED = 4;
        private const int CBXSL_HOT = 2;
        private const int CBXSL_NORMAL = 1;
        private const int CBXSL_PRESSED = 3;
        private const int CBXSR_DISABLED = 4;
        private const int CBXSR_HOT = 2;
        private const int CBXSR_NORMAL = 1;
        private const int CBXSR_PRESSED = 3;
        private const int CP_BACKGROUND = 2;
        private const int CP_BORDER = 4;
        private const int CP_CUEBANNER = 8;
        private const int CP_DROPDOWNBUTTON = 1;
        private const int CP_DROPDOWNBUTTONLEFT = 7;
        private const int CP_DROPDOWNBUTTONRIGHT = 6;
        private const int CP_READONLY = 5;
        private const int CP_TRANSPARENTBACKGROUND = 3;
        internal const int DropDownButtonWidth = 0x11;
        private const int EP_EDITTEXT = 1;
        private const int ETS_ASSIST = 7;
        private const int ETS_DISABLED = 4;
        private const int ETS_FOCUSED = 5;
        private const int ETS_HOT = 2;
        private const int ETS_NORMAL = 1;
        private const int ETS_READONLY = 6;
        private const int ETS_SELECTED = 3;
        private const uint GBF_COPY = 2;
        private const uint GBF_DIRECT = 1;
        private static ExOSVersion osver;
        private const int TMT_DIBDATA = 2;
        private const int TMT_GLYPHDIBDATA = 8;

        public ExAppearanceManager()
        {
            OperatingSystem oSVersion = Environment.OSVersion;
            switch (oSVersion.Platform)
            {
                case PlatformID.Win32Windows:
                    osver = ExOSVersion.Classic;
                    return;

                case PlatformID.Win32NT:
                    if (oSVersion.Version.Major != 4)
                    {
                        if (oSVersion.Version.Major == 5)
                        {
                            switch (oSVersion.Version.Minor)
                            {
                                case 0:
                                    osver = ExOSVersion.Classic;
                                    return;

                                case 1:
                                    osver = ExOSVersion.XP;
                                    return;

                                case 2:
                                    osver = ExOSVersion.Classic;
                                    return;
                            }
                            return;
                        }
                        if (oSVersion.Version.Major != 6)
                        {
                            break;
                        }
                        switch (oSVersion.Version.Minor)
                        {
                            case 0:
                                osver = ExOSVersion.Vista;
                                return;

                            case 1:
                                osver = ExOSVersion.Seven;
                                return;
                        }
                        return;
                    }
                    osver = ExOSVersion.Classic;
                    return;

                default:
                    return;
            }
        }

        [DllImport("UxTheme.dll")]
        private static extern int CloseThemeData(IntPtr hTheme);
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        internal bool DoDropShadowComboBox()
        {
            return (IsThemeActive() && (osver >= ExOSVersion.Vista));
        }

        internal bool DoSlideOpenComboBox()
        {
            return SystemInformation.IsComboBoxAnimationEnabled;
        }

        internal void DrawComboBoxVista(IntPtr hWnd, Graphics g, Rectangle borderArea, Rectangle buttonArea, MultiLineComboBoxStyle cbstyle, ExComboBoxState cbstate, bool drawborder, ref Bitmap overlayBitmap, float overlayRate, ref Bitmap resultBitmap)
        {
            int num5;
            int num6;
            IntPtr ptr5;
            int width = borderArea.Width;
            int height = borderArea.Height;
            if ((resultBitmap != null) && ((resultBitmap.Width != width) || (resultBitmap.Height != height)))
            {
                resultBitmap.Dispose();
                resultBitmap = null;
            }
            if (resultBitmap == null)
            {
                resultBitmap = new Bitmap(width, height);
            }
            IntPtr hdc = g.GetHdc();
            Graphics graphics = Graphics.FromImage(resultBitmap);
            IntPtr hDC = graphics.GetHdc();
            IntPtr zero = IntPtr.Zero;
            if (cbstyle != MultiLineComboBoxStyle.DropDownList)
            {
                switch (cbstate)
                {
                    case ExComboBoxState.Normal:
                        num5 = 1;
                        num6 = 1;
                        goto Label_020E;

                    case ExComboBoxState.Hot:
                        num5 = 2;
                        num6 = 1;
                        goto Label_020E;

                    case ExComboBoxState.HotOnButton:
                        num5 = 2;
                        num6 = 2;
                        goto Label_020E;

                    case ExComboBoxState.Focused:
                        num5 = 3;
                        num6 = 1;
                        goto Label_020E;

                    case ExComboBoxState.FocusedOnButton:
                        num5 = 3;
                        num6 = 2;
                        goto Label_020E;

                    case ExComboBoxState.PressButton:
                        num5 = 3;
                        num6 = 3;
                        goto Label_020E;

                    case ExComboBoxState.DropDown:
                        num5 = 3;
                        num6 = 3;
                        goto Label_020E;

                    case ExComboBoxState.Disabled:
                        num5 = 4;
                        num6 = 4;
                        goto Label_020E;
                }
                num5 = 1;
                num6 = 1;
            }
            else
            {
                int num3;
                int num4;
                switch (cbstate)
                {
                    case ExComboBoxState.Normal:
                        num3 = 1;
                        break;

                    case ExComboBoxState.Hot:
                    case ExComboBoxState.HotOnButton:
                    case ExComboBoxState.Focused:
                    case ExComboBoxState.FocusedOnButton:
                        num3 = 2;
                        break;

                    case ExComboBoxState.PressButton:
                    case ExComboBoxState.DropDown:
                        num3 = 3;
                        break;

                    case ExComboBoxState.Disabled:
                        num3 = 4;
                        break;

                    default:
                        num3 = 1;
                        break;
                }
                if (cbstate != ExComboBoxState.Disabled)
                {
                    num4 = 1;
                }
                else
                {
                    num4 = 4;
                }
                IntPtr hTheme = OpenThemeData(hWnd, "ComboBox");
                if (hTheme != IntPtr.Zero)
                {
                    RECT pRect = new RECT(borderArea.Left, borderArea.Top, borderArea.Right, borderArea.Bottom);
                    if (IsThemeBackgroundPartiallyTransparent(hTheme, 5, num3))
                    {
                        DrawThemeParentBackground(hTheme, hdc, ref pRect);
                    }
                    DrawThemeBackground(hTheme, hdc, 5, num3, ref pRect, IntPtr.Zero);
                    GetThemeBackgroundRegion(hTheme, hdc, 5, num3, ref pRect, ref zero);
                    DrawThemeBackground(hTheme, hDC, 5, num3, ref pRect, IntPtr.Zero);
                    pRect = new RECT(buttonArea.Left, buttonArea.Top, buttonArea.Right, buttonArea.Bottom);
                    DrawThemeBackground(hTheme, hDC, 6, num4, ref pRect, IntPtr.Zero);
                    CloseThemeData(hTheme);
                }
                goto Label_02E6;
            }
        Label_020E:
            ptr5 = OpenThemeData(hWnd, "ComboBox");
            if (ptr5 != IntPtr.Zero)
            {
                RECT rect2 = new RECT(borderArea.Left, borderArea.Top, borderArea.Right, borderArea.Bottom);
                if (IsThemeBackgroundPartiallyTransparent(ptr5, 4, num5))
                {
                    DrawThemeParentBackground(ptr5, hdc, ref rect2);
                }
                DrawThemeBackground(ptr5, hdc, 4, num5, ref rect2, IntPtr.Zero);
                GetThemeBackgroundRegion(ptr5, hdc, 4, num5, ref rect2, ref zero);
                DrawThemeBackground(ptr5, hDC, 4, num5, ref rect2, IntPtr.Zero);
                rect2 = new RECT(buttonArea.Left, buttonArea.Top, buttonArea.Right, buttonArea.Bottom);
                DrawThemeBackground(ptr5, hDC, 6, num6, ref rect2, IntPtr.Zero);
                CloseThemeData(ptr5);
            }
        Label_02E6:
            g.ReleaseHdc();
            graphics.ReleaseHdc();
            if ((overlayBitmap != null) && ((overlayBitmap.Width != width) || (overlayBitmap.Height != height)))
            {
                overlayBitmap.Dispose();
                overlayBitmap = null;
            }
            if ((overlayRate > 0f) && (overlayBitmap != null))
            {
                ColorMatrix newColorMatrix = new ColorMatrix
                {
                    Matrix00 = 1f,
                    Matrix11 = 1f,
                    Matrix22 = 1f,
                    Matrix33 = overlayRate,
                    Matrix44 = 1f
                };
                ImageAttributes imageAttr = new ImageAttributes();
                imageAttr.SetColorMatrix(newColorMatrix);
                graphics.DrawImage(overlayBitmap, new Rectangle(0, 0, width, height), 0, 0, width, height, GraphicsUnit.Pixel, imageAttr);
            }
            Region region = Region.FromHrgn(zero);
            Region clip = g.Clip;
            g.Clip = region;
            g.DrawImage(resultBitmap, 0, 0);
            g.Clip = clip;
            region.Dispose();
            DeleteObject(zero);
        }

        internal void DrawComboBoxXP(IntPtr hWnd, Graphics g, Rectangle borderArea, Rectangle buttonArea, ExComboBoxState cbstate, bool drawborder)
        {
            int num;
            int num2;
            IntPtr ptr2;
            switch (cbstate)
            {
                case ExComboBoxState.Normal:
                    num = 1;
                    num2 = 1;
                    break;

                case ExComboBoxState.Hot:
                    num = 2;
                    num2 = 1;
                    break;

                case ExComboBoxState.HotOnButton:
                    num = 2;
                    num2 = 2;
                    break;

                case ExComboBoxState.Focused:
                    num = 5;
                    num2 = 1;
                    break;

                case ExComboBoxState.FocusedOnButton:
                    num = 5;
                    num2 = 2;
                    break;

                case ExComboBoxState.PressButton:
                    num = 5;
                    num2 = 3;
                    break;

                case ExComboBoxState.DropDown:
                    num = 5;
                    num2 = 1;
                    break;

                case ExComboBoxState.Disabled:
                    num = 4;
                    num2 = 4;
                    break;

                default:
                    num = 1;
                    num2 = 1;
                    break;
            }
            IntPtr hdc = g.GetHdc();
            if (drawborder)
            {
                ptr2 = OpenThemeData(hWnd, "Edit");
                if (ptr2 != IntPtr.Zero)
                {
                    RECT pRect = new RECT(borderArea.Left, borderArea.Top, borderArea.Right, borderArea.Bottom);
                    if (IsThemeBackgroundPartiallyTransparent(ptr2, 4, num))
                    {
                        DrawThemeParentBackground(ptr2, hdc, ref pRect);
                    }
                    DrawThemeBackground(ptr2, hdc, 1, num, ref pRect, IntPtr.Zero);
                    CloseThemeData(ptr2);
                }
            }
            ptr2 = OpenThemeData(hWnd, "ComboBox");
            if (ptr2 != IntPtr.Zero)
            {
                RECT rect2 = new RECT(buttonArea.Left, buttonArea.Top, buttonArea.Right, buttonArea.Bottom);
                if ((osver == ExOSVersion.Vista) || (osver == ExOSVersion.Seven))
                {
                    rect2.left++;
                }
                DrawThemeBackground(ptr2, hdc, 1, num2, ref rect2, IntPtr.Zero);
                CloseThemeData(ptr2);
            }
            g.ReleaseHdc();
        }

        internal unsafe void DrawText(string text, Font font, HorizontalAlignment halign, VerticalAlignment valign, bool wordwrap, Graphics g, Rectangle range, Color fcolor)
        {
            Brush brush = new SolidBrush(fcolor);
            if (!wordwrap)
            {
                int num3;
                string[] strArray = text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                Point[] pointArray = new Point[strArray.Length];
                float num = 0f;
                for (int i = 0; i < strArray.Length; i++)
                {
                    SizeF ef = g.MeasureString(strArray[i], font);
                    if (halign == HorizontalAlignment.Center)
                    {
                        pointArray[i].X = ((range.Width - ((int)ef.Width)) / 2) + range.Left;
                    }
                    else if (halign == HorizontalAlignment.Right)
                    {
                        pointArray[i].X = (range.Width - ((int)ef.Width)) + range.Left;
                    }
                    else
                    {
                        pointArray[i].X = range.Left;
                    }
                    pointArray[i].Y = ((int)(ef.Height * i)) + range.Top;
                    num += ef.Height;
                }
                if (valign == VerticalAlignment.Center)
                {
                    num3 = (range.Height - ((int)num)) / 2;
                }
                else if (valign == VerticalAlignment.Bottom)
                {
                    num3 = range.Height - ((int)num);
                }
                else
                {
                    num3 = 0;
                }
                for (int j = 0; j < strArray.Length; j++)
                {
                    pointArray[j].Y += num3;
                    g.DrawString(strArray[j], font, brush, pointArray[j]);//* ((PointF*)&(pointArray[j])));
                }
            }
            else
            {
                StringFormat format = (StringFormat)StringFormat.GenericDefault.Clone();
                switch (halign)
                {
                    case HorizontalAlignment.Left:
                        format.Alignment = StringAlignment.Near;
                        break;

                    case HorizontalAlignment.Right:
                        format.Alignment = StringAlignment.Far;
                        break;

                    case HorizontalAlignment.Center:
                        format.Alignment = StringAlignment.Center;
                        break;
                }
                switch (valign)
                {
                    case VerticalAlignment.Top:
                        format.LineAlignment = StringAlignment.Near;
                        break;

                    case VerticalAlignment.Center:
                        format.LineAlignment = StringAlignment.Center;
                        break;

                    case VerticalAlignment.Bottom:
                        format.LineAlignment = StringAlignment.Far;
                        break;
                }
                g.DrawString(text, font, brush, range, format);
            }
        }

        [DllImport("UxTheme.dll")]
        private static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hDC, int iPartId, int iStateId, ref RECT pRect, ref RECT pClipRect);
        [DllImport("UxTheme.dll")]
        private static extern int DrawThemeBackground(IntPtr hTheme, IntPtr hDC, int iPartId, int iStateId, ref RECT pRect, IntPtr pClipRect);
        [DllImport("UxTheme.dll")]
        private static extern int DrawThemeParentBackground(IntPtr hTheme, IntPtr hDC, ref RECT pRect);
        public static ExAppearanceManager GetAppearanceManager()
        {
            if (apprMgr == null)
            {
                apprMgr = new ExAppearanceManager();
            }
            return apprMgr;
        }

        internal ExAppearanceStyle GetStyle(ExAppearanceStyle userStyle)
        {
            if ((userStyle == ExAppearanceStyle.Classic) || !IsThemeActive())
            {
                return ExAppearanceStyle.Classic;
            }
            if (userStyle == ExAppearanceStyle.XP)
            {
                return ExAppearanceStyle.XP;
            }
            if (userStyle != ExAppearanceStyle.Standard)
            {
                return ExAppearanceStyle.Classic;
            }
            if (osver == ExOSVersion.XP)
            {
                return ExAppearanceStyle.XP;
            }
            if ((osver != ExOSVersion.Vista) && (osver != ExOSVersion.Seven))
            {
                return ExAppearanceStyle.Classic;
            }
            return ExAppearanceStyle.Vista;
        }

        [DllImport("UxTheme.dll")]
        private static extern int GetThemeBackgroundRegion(IntPtr hTheme, IntPtr hdc, int iPartId, int iStateId, ref RECT pRect, ref IntPtr pRegion);
        [DllImport("UxTheme.dll")]
        private static extern bool IsThemeActive();
        [DllImport("UxTheme.dll")]
        private static extern bool IsThemeBackgroundPartiallyTransparent(IntPtr hTheme, int iPartId, int iStateId);
        [DllImport("UxTheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern IntPtr OpenThemeData(IntPtr hWnd, string pszClassList);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
            public RECT(int l, int t, int r, int b)
            {
                this.left = l;
                this.top = t;
                this.right = r;
                this.bottom = b;
            }
        }
    }
}

