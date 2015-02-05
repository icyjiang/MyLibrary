using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;

namespace JLQ.Common
{
    /// <summary>
    /// DataGridView行合并.请对属性MergeColumnNames 赋值既可
    /// </summary>
    public partial class DataGridViewEx : DataGridView
    {
        #region 私有变量 & 构造函数
        Rectangle _buttonRectangle;//按钮区间
        Rectangle _tipRectangle;//提示区间

        Rectangle _rePaintRectangle;//重绘区间
        Rectangle _importRectangle;//导入区间
        Rectangle _exportRectangle;//导出区间
        bool _containColReadOnly = false;
        bool _containRowHead = false;
        bool _showExcelButton = true;
        MouseStatus _ms = MouseStatus.None;

        Point _excelButtonPoint = Point.Empty;

        public List<string> FormatColumns;//这里是需要格式化的列名称集合。如果为空则所有的格式化。
        public DataGridViewEx()
        {
            InitializeComponent();
            _containColReadOnly = false;
            _containRowHead = false;
            _showExcelButton = true;//显示excel数据操作的按钮
            _ms = MouseStatus.None;

            FormatColumns = null;
            CalcRectangle();
        }
        #endregion

        #region Event
        /// <summary>
        /// 点击从Excel导入时触发(需自行编写导入数据代码)
        /// </summary>
        [Description("点击从Excel导入时触发(需自行编写导入数据代码)")]
        public event EventHandler<EventArgs> ImportClick;
        /// <summary>
        ///  /// <summary>
        /// 点击从Excel导入时触发(需自行编写导入数据代码)
        /// </summary>
        /// </summary>
        [Description("点击导出到Excel时触发((需自行编写导出数据代码))")]
        public event EventHandler<EventArgs> ExportClick;
        #endregion

        #region 重写的事件
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (_showExcelButton)//显示按钮为大前提
            {
                if (_importRectangle.Contains(e.X, e.Y))
                {
                    OnImportClick(this, EventArgs.Empty);
                    return;
                }
                if (_exportRectangle.Contains(e.X, e.Y))
                {
                    OnExportClick(this, EventArgs.Empty);
                    return;
                }
            }
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_showExcelButton) return;//显示按钮为大前提
            if (_exportRectangle.Contains(e.X, e.Y))
            {
                if (this.Cursor != Cursors.Hand)
                    this.Cursor = Cursors.Hand;
                if (_ms != MouseStatus.ExportArea)
                    this.Invalidate(_tipRectangle);
                _ms = MouseStatus.ExportArea;
            }
            else if (_importRectangle.Contains(e.X, e.Y))
            {
                if (this.Cursor != Cursors.Hand)
                    this.Cursor = Cursors.Hand;
                if (_ms != MouseStatus.ImportArea)
                    this.Invalidate(_tipRectangle);
                _ms = MouseStatus.ImportArea;
            }
            else
            {
                if (this.Cursor == Cursors.Hand)
                    this.Cursor = Cursors.Default;
                if (_ms != MouseStatus.OtherArea)
                    this.Invalidate(_tipRectangle);
                _ms = MouseStatus.OtherArea;
            }
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!_showExcelButton) return;//显示按钮为大前提
            if (!MouseStatus.HasMouse.HasFlag(_ms))
                _ms = MouseStatus.HasMouse;
            this.Invalidate(_rePaintRectangle);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_showExcelButton) return;//显示按钮为大前提
            _ms = MouseStatus.None;
            this.Invalidate(_rePaintRectangle);
        }
        protected override void OnScroll(ScrollEventArgs e)
        {
            base.OnScroll(e);
            if (!_showExcelButton) return;//显示按钮为大前提
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
                this.Invalidate(_rePaintRectangle);
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            if (_ms == MouseStatus.None) return;
            if (!_showExcelButton) return;
            //鼠标停留在控件的时候，画出导入导出按钮
            var graphic = pe.Graphics;
            var exportImg = StaticResource.ChevronUp;
            var importImg = StaticResource.ChevronDown;
            DrawImages(graphic, this.ClientRectangle, _excelButtonPoint.X, _excelButtonPoint.Y, exportImg, importImg, Color.AliceBlue);

            if (_ms == MouseStatus.ExportArea || _ms == MouseStatus.ImportArea)
            {
                var tipText = _ms == MouseStatus.ExportArea ? "数据导出到excel文件" : "导入excel数据到表格";
                using (var font = new Font("宋体", 9))
                {
                    var size = TextRenderer.MeasureText(tipText, font);
                    DrawTip(graphic, tipText, _excelButtonPoint.X + 44, _excelButtonPoint.Y + 1, size.Width + 24, size.Height + 10, 3,
                        Color.Black, Color.AliceBlue, font, Color.Black);
                }
            }
        }
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 && e.ColumnIndex > -1)
                {
                    DrawCell(e);
                }
                else
                {
                    //二维表头
                    if (e.RowIndex == -1)
                    {
                        if (SpanRows.ContainsKey(e.ColumnIndex)) //被合并的列
                        {
                            //画边框
                            Graphics g = e.Graphics;
                            e.Paint(e.CellBounds, DataGridViewPaintParts.Background | DataGridViewPaintParts.Border);

                            int left = e.CellBounds.Left, top = e.CellBounds.Top + 2,
                            right = e.CellBounds.Right, bottom = e.CellBounds.Bottom;

                            switch (SpanRows[e.ColumnIndex].Position)
                            {
                                case 1:
                                    left += 2;
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    right -= 2;
                                    break;
                            }

                            //画上半部分底色
                            g.FillRectangle(new SolidBrush(this._mergecolumnheaderbackcolor), left, top,
                            right - left, (bottom - top) / 2);

                            //画中线
                            g.DrawLine(new Pen(this.GridColor), left, (top + bottom) / 2,
                            right, (top + bottom) / 2);

                            //写小标题
                            StringFormat sf = new StringFormat();
                            sf.Alignment = StringAlignment.Center;
                            sf.LineAlignment = StringAlignment.Center;

                            g.DrawString(e.Value + "", e.CellStyle.Font, Brushes.Black,
                            new Rectangle(left, (top + bottom) / 2, right - left, (bottom - top) / 2), sf);
                            left = this.GetColumnDisplayRectangle(SpanRows[e.ColumnIndex].Left, true).Left - 2;

                            if (left < 0) left = this.GetCellDisplayRectangle(-1, -1, true).Width;
                            right = this.GetColumnDisplayRectangle(SpanRows[e.ColumnIndex].Right, true).Right - 2;
                            if (right < 0) right = this.Width;

                            g.DrawString(SpanRows[e.ColumnIndex].Text, e.CellStyle.Font, Brushes.Black,
                            new Rectangle(left, top, right - left, (bottom - top) / 2), sf);
                            e.Handled = true;
                        }
                    }
                }
                base.OnCellPainting(e);
            }
            catch
            { }
        }
        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            base.OnCellClick(e);
        }

        //自定事件
        protected virtual void OnImportClick(object sender, EventArgs e)
        {
            if (ImportClick != null)
                ImportClick(sender, e);
        }
        protected virtual void OnExportClick(object sender, EventArgs e)
        {
            if (ExportClick != null)
                ExportClick(sender, e);
        }

        //画按钮
        protected static void DrawImages(Graphics graphics, Rectangle conRectangle, int x, int y, Image imageExport, Image imageImport,
            Color backColor)//, ref Rectangle rectangleBack)
        {
            //rectangleBack.X = conRectangle.Left + 1;
            //rectangleBack.Y = conRectangle.Top + 1;
            using (var brush = new SolidBrush(backColor))
            {
                graphics.DrawRectangle(Pens.Black, new Rectangle(x + 1, y + 1, 38, 22));
                graphics.FillRectangle(brush, new Rectangle(x + 1, y + 1, 38, 22));
            }

            if (imageImport != null)
                DrawIcon(graphics, imageImport, x + 6 + 1, y + 6 + 1);

            if (imageExport != null)
                DrawIcon(graphics, imageExport, x + 12 + 1 + imageImport.Width, y + 6 + 1);

        }

        protected static void DrawIcon(Graphics graphics, Image imgPanelIcon, int x, int y)
        {
            if (graphics == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            if (imgPanelIcon == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(Image).Name));
            }

            int iconWidth = imgPanelIcon.Width;
            int iconHeight = imgPanelIcon.Height;
            Rectangle rectangleIcon = new Rectangle(x, y, iconWidth, iconHeight);

            using (System.Drawing.Imaging.ImageAttributes imageAttribute = new System.Drawing.Imaging.ImageAttributes())
            {
                imageAttribute.SetColorKey(Color.Magenta, Color.Magenta);//颜色改为透明
                System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();
                colorMap.OldColor = Color.FromArgb(0, 60, 166);//改颜色
                colorMap.NewColor = Color.Black;
                imageAttribute.SetRemapTable(new System.Drawing.Imaging.ColorMap[] { colorMap });

                graphics.DrawImage(imgPanelIcon, rectangleIcon, 0, 0, iconWidth, iconHeight, GraphicsUnit.Pixel, imageAttribute);
            }
        }

        //画提示
        protected static void DrawTip(Graphics graphics, string text, int x, int y, int width, int height, int radius,
            Color borderColor, Color fillColor, Font font, Color textColor)
        {
            int shadowOffset = 5;
            if (graphics == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            if (string.IsNullOrWhiteSpace(text)) return;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            Rectangle rec = new Rectangle(x, y, width, height);//图像
            GraphicsPath path = GetGraphicsPath(rec, radius);

            Rectangle rec2 = rec;//阴影
            rec2.Offset(shadowOffset, shadowOffset);
            GraphicsPath pathShadow = GetGraphicsPath(rec2, radius);

            using (Pen penBorder = new Pen(borderColor))
            using (var brushBtn = new LinearGradientBrush(new Point(0, 0), new Point(0, height + 6), Color.White, fillColor))
            using (var brushTxt = new SolidBrush(textColor))
            using (var brushShadow = new PathGradientBrush(pathShadow) { CenterColor = Color.Black, SurroundColors = new Color[] { SystemColors.ButtonFace } })
            {
                graphics.FillPath(brushShadow, pathShadow);
                graphics.DrawPath(penBorder, path);
                graphics.FillPath(brushBtn, path);
                graphics.DrawString(text, font, brushTxt, x + 12, y + 5);
            }
        }

        //长方形+导角半径，得到圆角矩形
        static GraphicsPath GetGraphicsPath(Rectangle rc, int r)
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

        #region 自定义方法
        //重算各个区间的域值
        void CalcRectangle()
        {
            _buttonRectangle = new Rectangle(_excelButtonPoint.X + 1, _excelButtonPoint.Y + 1, 38, 22);//按钮的区域
            _tipRectangle = new Rectangle(_excelButtonPoint.X + 44, _excelButtonPoint.Y + 1, 180, 30);//tip的区域，重绘
            _rePaintRectangle = new Rectangle(_excelButtonPoint.X, _excelButtonPoint.Y, this.ClientRectangle.Width, _buttonRectangle.Height + 6);
            _importRectangle = new Rectangle(
                    _buttonRectangle.X,
                    _buttonRectangle.Y,
                    _buttonRectangle.Width - _buttonRectangle.Width / 2,
                    _buttonRectangle.Height);
            _exportRectangle = new Rectangle(
                    _buttonRectangle.X + _buttonRectangle.Width / 2,
                    _buttonRectangle.Y,
                    _buttonRectangle.Width / 2,
                    _buttonRectangle.Height);
        }
        /// <summary>
        /// 画单元格
        /// </summary>
        /// <param name="e"></param>
        private void DrawCell(DataGridViewCellPaintingEventArgs e)
        {
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.NotSet)
            {
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            Brush gridBrush = new SolidBrush(this.GridColor);
            SolidBrush backBrush = new SolidBrush(e.CellStyle.BackColor);
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int cellwidth;
            //上面相同的行数
            int UpRows = 0;
            //下面相同的行数
            int DownRows = 0;
            //总行数
            int count = 0;
            if (this.MergeColumnNames.Contains(this.Columns[e.ColumnIndex].Name) && e.RowIndex != -1)
            {
                cellwidth = e.CellBounds.Width;
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = e.Value == null ? "" : e.Value.ToString().Trim();
                string curSelected = this.CurrentRow.Cells[e.ColumnIndex].Value == null ? "" : this.CurrentRow.Cells[e.ColumnIndex].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(curValue))
                {
                    #region 获取下面的行数
                    for (int i = e.RowIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;

                            DownRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    #region 获取上面的行数
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.Rows[i].Cells[e.ColumnIndex].Value.ToString().Equals(curValue))
                        {
                            //this.Rows[i].Cells[e.ColumnIndex].Selected = this.Rows[e.RowIndex].Cells[e.ColumnIndex].Selected;
                            UpRows++;
                            if (e.RowIndex != i)
                            {
                                cellwidth = cellwidth < this.Rows[i].Cells[e.ColumnIndex].Size.Width ? cellwidth : this.Rows[i].Cells[e.ColumnIndex].Size.Width;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    #endregion
                    count = DownRows + UpRows - 1;
                    if (count < 2)
                    {
                        return;
                    }
                }
                if (this.Rows[e.RowIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }
                //以背景色填充
                e.Graphics.FillRectangle(backBrush, e.CellBounds);
                //画字符串
                PaintingFont(e, cellwidth, UpRows, DownRows, count);
                if (DownRows == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    count = 0;
                }
                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);

                e.Handled = true;
            }
        }
        /// <summary>
        /// 画字符串
        /// </summary>
        /// <param name="e"></param>
        /// <param name="cellwidth"></param>
        /// <param name="UpRows"></param>
        /// <param name="DownRows"></param>
        /// <param name="count"></param>
        private void PaintingFont(System.Windows.Forms.DataGridViewCellPaintingEventArgs e, int cellwidth, int UpRows, int DownRows, int count)
        {
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int fontheight = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Height;
            int fontwidth = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Width;
            int cellheight = e.CellBounds.Height;

            //这里留意，可能前台有格式化过，所以这里画格式化的值
            if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomCenter)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomLeft)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.BottomRight)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y + cellheight * DownRows - fontheight);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleCenter)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleLeft)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.MiddleRight)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopCenter)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopLeft)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else if (e.CellStyle.Alignment == DataGridViewContentAlignment.TopRight)
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + cellwidth - fontwidth, e.CellBounds.Y - cellheight * (UpRows - 1));
            }
            else
            {
                e.Graphics.DrawString((String)e.FormattedValue, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (UpRows - 1) + (cellheight * count - fontheight) / 2);
            }
        }
        #endregion

        #region 属性
        /// <summary>
        /// 设置或获取合并列的集合
        /// </summary>
        [MergableProperty(false)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Visible)]
        [Localizable(true)]
        [Description("设置或获取合并列的集合"), Browsable(true), Category("单元格合并")]
        public List<string> MergeColumnNames
        {
            get
            {
                return _mergecolumnname;
            }
            set
            {
                _mergecolumnname = value;
            }
        }

        public void MergeColumnByIndex(int index)
        {
            MergeColumnNames.Add(this.Columns[index].Name);
        }

        private List<string> _mergecolumnname = new List<string>();

        //20131128.暂时写定为false.这个值主要是表现在导入导出数据时应该导出那些数据，
        [Description("导入导出excel时，是否包含只读列"), Browsable(false), DefaultValue(false), Category("Appearance")]
        public bool ContainColReadOnly
        {
            get { return this._containColReadOnly; }
            set
            {
                if (!value.Equals(this._containColReadOnly))
                { this._containColReadOnly = value; }
            }
        }

        //20131128.暂时写定为false.这个值主要是表现在导入导出数据时应该导出那些数据，
        [Description("导入导出excel时，是否有标题栏"), Browsable(false), DefaultValue(false), Category("Appearance")]
        public bool ContainRowHead
        {
            get { return this._containRowHead; }
            set
            {
                if (!value.Equals(this._containRowHead))
                { this._containRowHead = value; }
            }
        }

        [Description("是否显示与excel数据交互工具"), DefaultValue(true), Category("Appearance")]
        public bool ShowExcelButton
        {
            get { return this._showExcelButton; }
            set
            {
                if (!value.Equals(this._showExcelButton))
                { this._showExcelButton = value; }
            }
        }

        [Description("excel交互按钮的位置"), Category("Appearance")]
        public Point ExcelButtonPoint
        {
            get { return _excelButtonPoint; }
            set
            {
                if (!value.Equals(this._excelButtonPoint))
                {
                    this._excelButtonPoint = value;
                    CalcRectangle();
                }
            }
        }
        #endregion

        #region 二维表头
        private struct SpanInfo //表头信息
        {
            public SpanInfo(string Text, int Position, int Left, int Right)
            {
                this.Text = Text;
                this.Position = Position;
                this.Left = Left;
                this.Right = Right;
            }

            public string Text; //列主标题
            public int Position; //位置，1:左，2中，3右
            public int Left; //对应左行
            public int Right; //对应右行
        }
        private Dictionary<int, SpanInfo> SpanRows = new Dictionary<int, SpanInfo>();//需要2维表头的列
        /// <summary>
        /// 合并列
        /// </summary>
        /// <param name="ColIndex">列的索引</param>
        /// <param name="ColCount">需要合并的列数</param>
        /// <param name="Text">合并列后的文本</param>
        public void AddSpanHeader(int ColIndex, int ColCount, string Text)
        {
            if (ColCount < 2)
            {
                throw new Exception("行宽应大于等于2，合并1列无意义。");
            }
            //将这些列加入列表
            int Right = ColIndex + ColCount - 1; //同一大标题下的最后一列的索引
            SpanRows[ColIndex] = new SpanInfo(Text, 1, ColIndex, Right); //添加标题下的最左列
            SpanRows[Right] = new SpanInfo(Text, 3, ColIndex, Right); //添加该标题下的最右列
            for (int i = ColIndex + 1; i < Right; i++) //中间的列
            {
                SpanRows[i] = new SpanInfo(Text, 2, ColIndex, Right);
            }
        }
        /// <summary>
        /// 清除合并的列
        /// </summary>
        public void ClearSpanInfo()
        {
            SpanRows.Clear();
            //ReDrawHead();
        }
        private void DataGridViewEx_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)// && e.Type == ScrollEventType.EndScroll)
            {
                timer1.Enabled = false; timer1.Enabled = true;
            }
        }
        //刷新显示表头
        public void ReDrawHead()
        {
            foreach (int si in SpanRows.Keys)
            {
                try
                {
                    this.Invalidate(this.GetCellDisplayRectangle(si, -1, true));
                }
                catch { }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            ReDrawHead();
        }
        /// <summary>
        /// 二维表头的背景颜色
        /// </summary>
        [Description("二维表头的背景颜色"), Browsable(true), Category("二维表头")]
        public Color MergeColumnHeaderBackColor
        {
            get { return this._mergecolumnheaderbackcolor; }
            set { this._mergecolumnheaderbackcolor = value; }
        }
        private Color _mergecolumnheaderbackcolor = System.Drawing.SystemColors.Control;
        #endregion

        //鼠标位置
        [Flags]
        enum MouseStatus
        {
            None = 1 << 0,//控件以外
            ImportArea = 1 << 1,//导入
            ExportArea = 1 << 2,//导出
            OtherArea = 1 << 3,//非导入导出的控件以内
            HasMouse = ImportArea | ExportArea | OtherArea,//控件以内
        }
    }
}