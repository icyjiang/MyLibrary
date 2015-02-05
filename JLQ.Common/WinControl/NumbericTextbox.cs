using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Drawing;

namespace JLQ.Common
{
    public class NumbericTextbox : TextBox
    {
        #region " Private Const and Variables "
        const int WM_SETFOCUS = 7;
        const int WM_KILLFOCUS = 8;
        const int WM_PAINT = 15;
        const int WM_PASTE = 770;//0x0302

        bool _hasDot = true;
        double _maxValue = -1;
        string _defaultText = String.Empty;
        Color _defaultTextColor = SystemColors.GrayText;
        Font _defaultTextFont = null;

        string regex = @"^\d+(\.\d*)?$|^$";//数字或空 
        bool drawDefault = true;
        #endregion

        #region " Constructor "
        public NumbericTextbox()
        {
            HasDot = true;
            MaxValue = -1;
            DefaultText = "";
            DefaultTextColor = Color.LightGray;
            DefaultTextFont = this.Font;
        }
        #endregion

        #region " Overrided Methods "
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //小数点
            if (e.KeyChar == '.' && _hasDot) e.Handled = this.Text.IndexOf('.') != -1;
            //数字键，back
            else if (Char.IsNumber(e.KeyChar) || e.KeyChar == (char)8/* || e.KeyChar == (Char)Keys.Delete 无效*/)
                e.Handled = false;
            //复制、粘贴、剪切
            else if (e.KeyChar == 3 || e.KeyChar == 22 || e.KeyChar == 24)
            {
                //如果是粘贴
                if (e.KeyChar == 22)
                    e.Handled = !Regex.IsMatch(Clipboard.GetText(), regex);
                else e.Handled = false;
            }
            else e.Handled = true;

            if (!e.Handled) this.Tag = this.Text;//记录最后一次正确输入 
            base.OnKeyPress(e);
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (!Regex.IsMatch((this).Text, regex))
            {
                int index = (this).SelectionStart;
                (this).Text = (this).Tag as string;
                (this).SelectionStart = index;
            }

            //最大值控制
            if (_maxValue != -1)
            {
                if (!string.IsNullOrWhiteSpace(this.Text))
                {
                    try
                    {
                        double curValue = double.Parse(this.Text);
                        if (curValue > _maxValue)
                        {
                            this.Text = _maxValue.ToString();
                            MessageBox.Show("最大值为：" + _maxValue);
                            (this).SelectionStart = this.Text.Length;
                        }
                    }
                    catch (FormatException err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }
        }

        //文字对齐方式改变时，重绘控件
        protected override void OnTextAlignChanged(EventArgs e)
        {
            base.OnTextAlignChanged(e);
            this.Invalidate();
        }

        //控制的核心。重绘、判断能否粘贴、获取焦点、失去焦点等事件都从这里开始。
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_SETFOCUS:
                    drawDefault = false;
                    break;

                case WM_KILLFOCUS:
                    drawDefault = true;
                    break;

                case WM_PASTE:
                    if (!Regex.IsMatch(Clipboard.GetText(), regex))
                        return;
                    break;
            }
            base.WndProc(ref m);

            // 只有当消息为WM_PAINT，且需要画，且当前的text为空，重画
            if (m.Msg == WM_PAINT && drawDefault && this.Text.Length == 0 && !this.GetStyle(ControlStyles.UserPaint))
                DrawTextDefault();
        }

        protected virtual void DrawTextDefault()
        {
            using (Graphics g = this.CreateGraphics())
            {
                DrawTextDefault(g);
            }
        }

        //重绘的方法
        protected virtual void DrawTextDefault(Graphics g)
        {
            TextFormatFlags flags = TextFormatFlags.NoPadding | TextFormatFlags.Top | TextFormatFlags.EndEllipsis;
            Rectangle rect = this.ClientRectangle;

            switch (this.TextAlign)
            {
                case HorizontalAlignment.Center:
                    flags = flags | TextFormatFlags.HorizontalCenter;
                    rect.Offset(0, 1);
                    break;
                case HorizontalAlignment.Left:
                    flags = flags | TextFormatFlags.Left;
                    rect.Offset(1, 1);
                    break;
                case HorizontalAlignment.Right:
                    flags = flags | TextFormatFlags.Right;
                    rect.Offset(0, 1);
                    break;
            }

            TextRenderer.DrawText(g, _defaultText, _defaultTextFont, rect, _defaultTextColor, this.BackColor, flags);
        }
        #endregion

        #region " Private Methods "
        //获取float的值
        public float GetSingleValue()
        {
            if (string.IsNullOrWhiteSpace(Text)) return 0;
            try
            {
                return Convert.ToSingle(Text);
            }
            catch { }
            return 0;
        }

        //获取int的值
        public int GetIntValue()
        {
            if (string.IsNullOrWhiteSpace(Text)) return 0;
            try
            {
                return Convert.ToInt32(Text);
            }
            catch { }
            return 0;
        }
        #endregion

        #region " Fields and Properties "
        /// <summary>
        /// 数值能否含有小数点
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(bool), "True"), Category("自定义属性"), Description("数值能否含有小数点")]
        public bool HasDot
        {
            get { return _hasDot; }
            set
            {
                if (!_hasDot.Equals(value))
                {
                    _hasDot = value;
                    if (IsHandleCreated)
                        Invalidate();
                }
            }
        }

        /// <summary>
        /// 数值的最大值。-1表示没有最大值。
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(double), "-1"), Category("自定义属性"), Description("数值的最大范围。-1表示没有限制。")]
        public double MaxValue
        {
            get { return _maxValue; }
            set
            {
                if (!_maxValue.Equals(value))
                {
                    _maxValue = value;
                    if (IsHandleCreated)
                        Invalidate();
                }
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(string), ""), Category("自定义属性"), Description("默认显示的提示文字")]
        public string DefaultText
        {
            get { return _defaultText; }
            set
            {
                if (!_defaultText.Equals(value))
                {
                    _defaultText = value.Trim();
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                }
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(typeof(Color), "LightGray"), Category("自定义属性"), Description("提示文字的颜色")]
        public Color DefaultTextColor
        {
            get { return _defaultTextColor; }
            set
            {
                if (_defaultTextColor != value)
                {
                    _defaultTextColor = value;
                    if (IsHandleCreated)
                    {
                        Invalidate();
                    }
                }
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("自定义属性"), Description("提示文字的字体等其他信息")]
        public Font DefaultTextFont
        {
            get { return _defaultTextFont; }
            set { _defaultTextFont = value; this.Invalidate(); }
        }
        #endregion
    }
}
