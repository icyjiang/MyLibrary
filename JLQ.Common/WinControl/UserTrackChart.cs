using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace JLQ.Common
{
    public partial class UserTrackChart : UserControl
    {
        #region 变量
        Padding _offset;//坐标的内缩
        string _xText;//X轴文字
        string _yText;//Y轴文字

        int _trackPointRadius;//点半径
        Size _trackPeriodSize;//矩形
        bool _hasAxisArrow;//是否画坐标箭头
        string _noticeWhenNoData;//无数据的提示

        Color _colorTrackPoint;
        Color _colorTrackPeriod;
        Color _colorTrackPeriodOver;

        int _shadowOffset = 5;//阴影
        List<TrackItem> _items = null;
        Rectangle? _recUseB4Order = null;
        Rectangle? _recOrders = null;

        Rectangle? _recUseB4OrderTip = null;
        Rectangle? _recOrdersTip = null;

        MouseStatus _mouseStatus = MouseStatus.None;
        #endregion

        //坐标原点
        Point _zeroPoint
        {
            get { return new Point(this._offset.Left, this.Height - this._offset.Bottom); }
        }
        [Browsable(false)]
        public List<TrackItem> Items
        {
            set
            {
                _items = value;

                //这里清空缓存
                _recUseB4Order = null;
                _recOrders = null;
                _recUseB4OrderTip = null;
                _recOrdersTip = null;

                this.Invalidate();//赋值时重绘
            }
        }

        public UserTrackChart()
        {
            InitializeComponent();
            _offset = new Padding(30);
            _xText = "时间";
            _yText = "进度";

            _trackPointRadius = 10;
            _trackPeriodSize = new Size(30, 20);
            _hasAxisArrow = true;
            _noticeWhenNoData = "当前用户无相关数据";

            _colorTrackPoint = Color.Blue;
            _colorTrackPeriod = Color.Red;
            _colorTrackPeriodOver = Color.Pink;
        }

        #region 前台属性
        [Description("色块的阴影位移"), Browsable(true), DefaultValue(5), Category("UserTrackChart")]
        public int ShadowOffset
        {
            get { return _shadowOffset; }
            set { if (!_shadowOffset.Equals(value)) { _shadowOffset = value; } }
        }

        [Description("图像区域离控件边缘的距离"), Browsable(true), DefaultValue(typeof(Padding), "30, 30, 30, 30"), Category("UserTrackChart")]
        public Padding Offset
        {
            get { return _offset; }
            set
            {
                if (!_offset.Equals(value))
                {
                    _offset = value;
                    this.Invalidate();
                }
            }
        }

        [Description("X轴的描述文字"), Browsable(true), DefaultValue("时间"), Category("UserTrackChart")]
        public string XText
        {
            get { return _xText; }
            set
            {
                if (!_xText.Equals(value))
                {
                    _xText = value;
                    this.Invalidate();
                }
            }
        }

        [Description("Y轴的描述文字"), Browsable(true), DefaultValue("进度"), Category("UserTrackChart")]
        public string YText
        {
            get { return _yText; }
            set
            {
                if (!_yText.Equals(value))
                {
                    _yText = value;
                    this.Invalidate();
                }
            }
        }

        [Description("时间点的圆半径"), Browsable(true), DefaultValue(10), Category("UserTrackChart")]
        public int TrackPointRadius
        {
            get { return _trackPointRadius; }
            set { if (!_trackPointRadius.Equals(value)) _trackPointRadius = value; }
        }

        [Description("时间段的矩形尺寸"), Browsable(true), DefaultValue(typeof(Size), "30, 20"), Category("UserTrackChart")]
        public Size TrackPeriodSize
        {
            get { return _trackPeriodSize; }
            set { if (!_trackPeriodSize.Equals(value)) _trackPeriodSize = value; }
        }

        [Description("坐标轴是否绘制箭头"), Browsable(true), DefaultValue(true), Category("UserTrackChart")]
        public bool HasAxisArrow
        {
            get { return _hasAxisArrow; }
            set
            {
                if (!_hasAxisArrow.Equals(value))
                {
                    _hasAxisArrow = value;
                    this.Invalidate();
                }
            }
        }

        [Description("查询结果报错时的绘制文字"), Browsable(true), DefaultValue("当前用户无相关数据"), Category("UserTrackChart")]
        public string NoticeWhenNoData
        {
            get { return _noticeWhenNoData; }
            set
            {
                if (!_noticeWhenNoData.Equals(value))
                {
                    _noticeWhenNoData = value;
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region Event
        /// <summary>
        /// 点击“购买前试用情况”触发
        /// </summary>
        [Description("点击“购买前试用情况”触发")]
        public event EventHandler<UserTrackEventArgs> UseBeforeOrderClick;
        /// <summary>

        /// <summary>
        /// 点击“订单统计”触发
        /// </summary>
        /// </summary>
        [Description("点击“订单统计”触发")]
        public event EventHandler<UserTrackEventArgs> OrderAnalyseClick;
        #endregion

        #region OnPaint
        private void UserTrackChart_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawAxis(e.Graphics);
            if (_items == null || _items.Count <= 0 || _items.FirstOrDefault(x => x.TrackStyle == TrackStyle.Regist) == null)
            {
                DrawNoData(e.Graphics);
                return;
            }

            var count = _items.Count;
            var xOff = (this.Size.Width - _offset.Left - _offset.Right) / (count + 1);//x轴间距
            var yOff = (this.Size.Height - _offset.Top - _offset.Bottom) / (count + 1);//y轴间距
            Point[] ps = Enumerable.Range(0, _items.Count).Select(x => new Point(_zeroPoint.X + (x + 1) * xOff, _zeroPoint.Y - (x + 1) * yOff)).ToArray();
            if (ps.Length >= 2) e.Graphics.DrawLines(Pens.Gray, ps);
            int i = 0;
            foreach (var item in _items.OrderBy(x => x.TrackStyle))
            {
                var centerX = _zeroPoint.X + (i + 1) * xOff;
                var centerY = _zeroPoint.Y - (i + 1) * yOff;
                if (item.TrackSharp == TrackSharp.TimePoint) DrawTrackPoint(e.Graphics, item, centerX, centerY);
                else if (item.TrackSharp == TrackSharp.TimePeriod) DrawTrackPeriod(e.Graphics, item, centerX, centerY);
                i++;
            }

            //画Tip
            if (_mouseStatus == MouseStatus.UseBeforeOrderArea || _mouseStatus == MouseStatus.OrderAnalyseArea)
            {
                if (_mouseStatus == MouseStatus.UseBeforeOrderArea && _recUseB4OrderTip.HasValue)
                {
                    var item = _items.FirstOrDefault(x => x.TrackStyle == TrackStyle.UseB4Order);
                    if (item != null && !string.IsNullOrWhiteSpace(item.Note))
                        DrawTip(e.Graphics, item.Note, _recUseB4OrderTip.Value, 10, Color.Gray, Color.LightGoldenrodYellow, this.Font, Color.Black);
                }
                else if (_mouseStatus == MouseStatus.OrderAnalyseArea && _recOrdersTip.HasValue)
                {
                    var item = _items.FirstOrDefault(x => x.TrackStyle == TrackStyle.Orders);
                    if (item != null && !string.IsNullOrWhiteSpace(item.Note))
                        DrawTip(e.Graphics, item.Note, _recOrdersTip.Value, 10, Color.Gray, Color.LightGoldenrodYellow, this.Font, Color.Black);
                }
            }
        }

        //画坐标轴(无刻度)
        void DrawAxis(Graphics g)
        {
            var yTopPoint = new Point(_zeroPoint.X, _offset.Top);
            var xRightPoint = new Point(this.Width - _offset.Right, _zeroPoint.Y);
            g.DrawLines(Pens.Black, new Point[] { yTopPoint, _zeroPoint, xRightPoint });
            if (_hasAxisArrow)//带箭头
            {
                g.DrawLines(Pens.Black, new Point[] { new Point(yTopPoint.X - 3, yTopPoint.Y + 10), yTopPoint, new Point(yTopPoint.X + 3, yTopPoint.Y + 10) });
                g.DrawLines(Pens.Black, new Point[] { new Point(xRightPoint.X - 10, xRightPoint.Y - 3), xRightPoint, new Point(xRightPoint.X - 10, xRightPoint.Y + 3) });
            }

            if (!string.IsNullOrWhiteSpace(_xText))
                g.DrawString(_xText, this.Font, Brushes.Black, xRightPoint.X - 20, xRightPoint.Y + 10);
            if (!string.IsNullOrWhiteSpace(_yText))
                g.DrawString(_yText, this.Font, Brushes.Black, yTopPoint.X - 20, yTopPoint.Y - 15);
        }

        //无数据的提示
        void DrawNoData(Graphics g)
        {
            //放在大概居中的位置
            var x = _zeroPoint.X + 50;
            var y = _offset.Top + (this.Height - _offset.Top - _offset.Bottom) / 2;
            using (var font = new Font("黑体", 15, FontStyle.Bold))
            {
                g.DrawString(_noticeWhenNoData, font, Brushes.LightGray, x, y);
            }
        }

        //画时间点(包括X轴的刻度)
        void DrawTrackPoint(Graphics g, TrackItem item, int x, int y)
        {
            DrawTrackGradation(g, item, x, y);
            Rectangle rec = new Rectangle(x - _trackPointRadius, y - _trackPointRadius, _trackPointRadius * 2, _trackPointRadius * 2);
            Rectangle recShadow = rec;
            recShadow.Offset(_shadowOffset, _shadowOffset);
            var pathShadow = GetGraphicsPath(recShadow, _trackPointRadius * 2);

            using (var brush = new SolidBrush(_colorTrackPoint))
            using (var brushShadow = new PathGradientBrush(pathShadow) { CenterColor = Color.Black, SurroundColors = new Color[] { SystemColors.ButtonFace } })
            {
                g.FillPath(brushShadow, pathShadow);//阴影
                g.FillEllipse(brush, rec);
            }
        }

        //画时间段(包括X轴的刻度)
        void DrawTrackPeriod(Graphics g, TrackItem item, int x, int y)
        {
            DrawTrackGradation(g, item, x, y);

            Brush brush = new SolidBrush(_colorTrackPeriod);
            try
            {
                if ((item.TrackStyle == TrackStyle.UseB4Order && _mouseStatus == MouseStatus.UseBeforeOrderArea) ||
                    (item.TrackStyle == TrackStyle.Orders && _mouseStatus == MouseStatus.OrderAnalyseArea))
                    brush = new SolidBrush(_colorTrackPeriodOver);
                Rectangle rec = new Rectangle(x - _trackPeriodSize.Width / 2, y - _trackPeriodSize.Height / 2, _trackPeriodSize.Width, _trackPeriodSize.Height);

                Rectangle recShadow = rec;
                recShadow.Offset(_shadowOffset, _shadowOffset);
                var pathShadow = GetGraphicsPath(recShadow, 1);
                using (var brushShadow = new PathGradientBrush(pathShadow) { CenterColor = Color.Black, SurroundColors = new Color[] { SystemColors.ButtonFace } })
                    g.FillPath(brushShadow, pathShadow);//阴影

                g.FillRectangle(brush, rec);

                if (item.TrackStyle == TrackStyle.UseB4Order && !string.IsNullOrWhiteSpace(item.Note))// && _recUseB4OrderTip == null)
                {
                    var size = TextRenderer.MeasureText(item.Note, this.Font);
                    var width = size.Width + 30;//留边
                    var height = size.Height + 30;
                    _recUseB4OrderTip = new Rectangle(x, y + _trackPeriodSize.Height / 2, width, height);
                }
                else if (item.TrackStyle == TrackStyle.Orders && !string.IsNullOrWhiteSpace(item.Note))// && _recOrdersTip == null)
                {
                    var size = TextRenderer.MeasureText(item.Note, this.Font);
                    var width = size.Width + 30;//留边
                    var height = size.Height + 30;
                    _recOrdersTip = new Rectangle(x, y + _trackPeriodSize.Height / 2, width, height);
                }


                //对敏感区域赋值
                if (item.TrackStyle == TrackStyle.UseB4Order) _recUseB4Order = rec;
                if (item.TrackStyle == TrackStyle.Orders) _recOrders = rec;
            }
            finally { brush.Dispose(); }
        }

        //画刻度
        void DrawTrackGradation(Graphics g, TrackItem item, int x, int y)
        {
            var size = TextRenderer.MeasureText(item.YText, this.Font);//居中
            g.DrawString(item.YText, this.Font, Brushes.Black, x - size.Width / 2, y - _trackPointRadius - size.Height - 6);

            var size2 = TextRenderer.MeasureText(item.XText, this.Font);//居中
            g.DrawLine(Pens.Black, x, _zeroPoint.Y, x, _zeroPoint.Y - 3);
            g.DrawString(item.XText, this.Font, Brushes.Black, x - size2.Width / 2, _zeroPoint.Y + 6);

            using (var pen = new Pen(Brushes.Gray) { DashStyle = System.Drawing.Drawing2D.DashStyle.Custom, DashPattern = new float[] { 2, 2 } })//
                g.DrawLine(pen, x, y, x, _zeroPoint.Y);
        }

        //画提示
        void DrawTip(Graphics g, string text, Rectangle rec, int radius, Color borderColor, Color fillColor, Font font, Color textColor)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            GraphicsPath path = GetGraphicsPath(rec, radius);

            using (Pen penBorder = new Pen(borderColor))
            using (var brushBtn = new SolidBrush(fillColor))
            using (var brushTxt = new SolidBrush(textColor))
            {
                g.DrawPath(penBorder, path);
                g.FillPath(brushBtn, path);
                g.DrawString(text, font, brushTxt, rec.X + 12, rec.Y + 12);
            }
        }

        //长方形+导角半径，得到圆角矩形
        GraphicsPath GetGraphicsPath(Rectangle rc, int r)
        {
            int x = rc.X, y = rc.Y, w = rc.Width, h = rc.Height;
            GraphicsPath path = new GraphicsPath();
            path.AddArc(x, y, r, r, 180, 90);				//Upper left corner
            path.AddArc(x + w - r, y, r, r, 270, 90);			//Upper right corner
            path.AddArc(x + w - r, y + h - r, r, r, 0, 90);		//Lower right corner
            path.AddArc(x, y + h - r, r, r, 90, 90);			//Lower left corner
            path.CloseFigure();
            return path;
        }
        #endregion

        #region 重写事件
        protected override void OnMouseUp(MouseEventArgs e)//单击后触发事件
        {
            if (_recUseB4Order.HasValue && _recUseB4Order.Value.Contains(e.X, e.Y))
            {
                var item = _items.FirstOrDefault(x => x.TrackStyle == TrackStyle.UseB4Order);
                OnUseBeforeOrderClick(this, new UserTrackEventArgs(item));
                return;
            }
            if (_recOrders.HasValue && _recOrders.Value.Contains(e.X, e.Y))
            {
                var item = _items.FirstOrDefault(x => x.TrackStyle == TrackStyle.Orders);
                OnOrderAnalyseClick(this, new UserTrackEventArgs(item));
                return;
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)//移动时变手指,且变色
        {
            base.OnMouseMove(e);
            if (_recUseB4Order.HasValue && _recUseB4Order.Value.Contains(e.X, e.Y))//移进来
            {
                if (this.Cursor != Cursors.Hand)
                    this.Cursor = Cursors.Hand;
                if (_mouseStatus != MouseStatus.UseBeforeOrderArea)//刚刚移过来
                {
                    if (_recUseB4OrderTip.HasValue)
                        this.Invalidate(new Rectangle(_recUseB4Order.Value.Left, _recUseB4Order.Value.Top,
                            _recUseB4OrderTip.Value.Right - _recUseB4Order.Value.Left + 2, _recUseB4OrderTip.Value.Bottom - _recUseB4Order.Value.Top + 2));
                    else this.Invalidate(_recUseB4Order.Value);
                }
                _mouseStatus = MouseStatus.UseBeforeOrderArea;
            }
            else if (_recOrders.HasValue && _recOrders.Value.Contains(e.X, e.Y))
            {
                if (this.Cursor != Cursors.Hand)
                    this.Cursor = Cursors.Hand;
                if (_mouseStatus != MouseStatus.OrderAnalyseArea)
                {
                    if (_recOrdersTip.HasValue)
                        this.Invalidate(new Rectangle(_recOrders.Value.Left, _recOrders.Value.Top,
                            _recOrdersTip.Value.Right - _recOrders.Value.Left + 2, _recOrdersTip.Value.Bottom - _recOrders.Value.Top + 2));
                    else this.Invalidate(_recOrders.Value);
                }
                _mouseStatus = MouseStatus.OrderAnalyseArea;
            }
            else
            {
                if (this.Cursor == Cursors.Hand)
                    this.Cursor = Cursors.Default;
                if (_mouseStatus == MouseStatus.UseBeforeOrderArea)
                {
                    if (_recUseB4OrderTip.HasValue)
                        this.Invalidate(new Rectangle(_recUseB4Order.Value.Left, _recUseB4Order.Value.Top,
                            _recUseB4OrderTip.Value.Right - _recUseB4Order.Value.Left + 2, _recUseB4OrderTip.Value.Bottom - _recUseB4Order.Value.Top + 2));
                    else this.Invalidate(_recUseB4Order.Value);
                }
                if (_mouseStatus == MouseStatus.OrderAnalyseArea)
                {
                    if (_recOrdersTip.HasValue)
                        this.Invalidate(new Rectangle(_recOrders.Value.Left, _recOrders.Value.Top,
                            _recOrdersTip.Value.Right - _recOrders.Value.Left + 2, _recOrdersTip.Value.Bottom - _recOrders.Value.Top + 2));
                    else this.Invalidate(_recOrders.Value);
                }
                _mouseStatus = MouseStatus.OtherArea;
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _mouseStatus = MouseStatus.OtherArea;
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_mouseStatus == MouseStatus.UseBeforeOrderArea)
            {
                if (_recUseB4OrderTip.HasValue)
                    this.Invalidate(new Rectangle(_recUseB4Order.Value.Left, _recUseB4Order.Value.Top,
                        _recUseB4OrderTip.Value.Right - _recUseB4Order.Value.Left + 2, _recUseB4OrderTip.Value.Bottom - _recUseB4Order.Value.Top + 2));
                else this.Invalidate(_recUseB4Order.Value);
            }
            if (_mouseStatus == MouseStatus.OrderAnalyseArea)
            {
                if (_recOrdersTip.HasValue)
                    this.Invalidate(new Rectangle(_recOrders.Value.Left, _recOrders.Value.Top,
                        _recOrdersTip.Value.Right - _recOrders.Value.Left + 2, _recOrdersTip.Value.Bottom - _recOrders.Value.Top + 2));
                else this.Invalidate(_recOrders.Value);
            }
            _mouseStatus = MouseStatus.None;
        }

        //自定事件(点击控件上的两个可展开数据的Rectangle)
        protected virtual void OnUseBeforeOrderClick(object sender, UserTrackEventArgs e)
        {
            if (UseBeforeOrderClick != null)
                UseBeforeOrderClick(sender, e);
        }
        protected virtual void OnOrderAnalyseClick(object sender, UserTrackEventArgs e)
        {
            if (OrderAnalyseClick != null)
                OrderAnalyseClick(sender, e);
        }

        //尺寸变化后重绘
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }
        #endregion
    }

    public class TrackItem
    {
        public TrackSharp TrackSharp;
        public TrackStyle TrackStyle;
        public string XText;
        public string YText;
        public string Note;

        public TrackItem(TrackSharp trackSharp, TrackStyle trackStyle, string xText, string yText, string note = "")
        {
            this.TrackSharp = trackSharp;
            this.TrackStyle = trackStyle;
            this.XText = xText;
            this.YText = yText;
            this.Note = note;
        }
    }

    public class UserTrackEventArgs : EventArgs
    {
        public TrackItem Item;
        public UserTrackEventArgs(TrackItem item)
        {
            Item = item;//事件传递数据
        }
    }

    public enum TrackSharp
    {
        TimePoint,//时间点
        TimePeriod,//时间段
    }

    public enum TrackStyle
    {
        Regist, //注册
        FirstLogin,//首次登录
        UseB4Order,//买前试用
        FirstOrder,//第一次订单
        Orders,//所有订单
        LastLogin,//最后一次登录
    }

    //鼠标位置
    enum MouseStatus
    {
        None = 1 << 0,//控件以外
        UseBeforeOrderArea = 1 << 1,//导入
        OrderAnalyseArea = 1 << 2,//导出
        OtherArea = 1 << 3,//非导入导出的控件以内
    }
}
