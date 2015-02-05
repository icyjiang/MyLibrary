﻿using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace JLQ.Common
{
    /// <summary>
    /// 水晶按钮。有倒影效果。可用。
    /// </summary>
    public class CrystalButton : Button
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.Container components = null;
        private bool XiaCen = false;
        private bool mouseMove = false;
        private Color backColor;
        public CrystalButton()
        {
            // 该调用是 Windows.Forms 窗体设计器所必需的。
            InitializeComponent();

            // TODO: 在 InitComponent 调用后添加任何初始化
            backColor = this.BackColor;
            //this.Text=this.ShowFocusCues.ToString();
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码
        /// <summary>
        /// 设计器支持所需的方法 - 不要使用代码编辑器 
        /// 修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // UserControl1
            // 
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseUp);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.UserControl1_Paint);
            this.MouseEnter += new System.EventHandler(this.UserControl1_MouseEnter);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.UserControl1_KeyUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserControl1_KeyDown);
            this.BackColorChanged += new System.EventHandler(this.UserControl1_BackColorChanged);
            this.MouseLeave += new System.EventHandler(this.UserControl1_MouseLeave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UserControl1_MouseDown);

        }
        #endregion



        protected GraphicsPath GetGraphicsPath(Rectangle rect)
        {
            GraphicsPath ClientPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (rect.Width <= 0)
            {
                rect.Width = 1;
            }
            if (rect.Height <= 0)
            {
                rect.Height = 1;
            }

            ClientPath.AddArc(rect.Left, rect.Top, rect.Height, rect.Height, 90f, 180f);
            ClientPath.AddArc(rect.Right - rect.Height, rect.Top, rect.Height, rect.Height, 270f, 180f);
            ClientPath.CloseFigure();
            return ClientPath;
        }

        protected GraphicsPath GetGraphicsPath1(Rectangle rect)
        {
            GraphicsPath ClientPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (rect.Width <= 0)
            {
                rect.Width = 1;
            }
            if (rect.Height <= 0)
            {
                rect.Height = 1;
            }

            ClientPath.AddArc(rect.Left, rect.Top, rect.Height, rect.Height, 190f, 80f);
            ClientPath.AddArc(rect.Right - rect.Height, rect.Top, rect.Height, rect.Height, 270f, 80f);
            ClientPath.CloseFigure();
            return ClientPath;
        }

        private void DrawYinYing(Graphics gr, bool xiacen)
        {
            Rectangle rect = this.ClientRectangle;
            rect.Inflate(-(rect.Width / 10), -(rect.Height) / 4);
            float bf1 = rect.Width / 100f;
            float bf2 = rect.Height / 100f;

            rect.Y = rect.Y + this.ClientRectangle.Height / 4;
            if (xiacen)
            {
                rect.Y = rect.Y + 4;
            }
            GraphicsPath path;

            for (int a = 1; a < 33; a++)
            {
                float bf3 = bf1 * a;
                float bf4 = bf2 * a;
                Rectangle rect1 = rect;
                rect1.Inflate(-(int)bf3, -(int)bf4);
                path = GetGraphicsPath(rect1);

                int r = backColor.R;
                int g = backColor.G;
                int b = backColor.B;
                r = r + 3 * a;
                g = g + 3 * a;
                b = b + 3 * a;
                if (r > 255) r = 255;
                if (g > 255) g = 255;
                if (b > 255) b = 255;
                gr.FillPath(new SolidBrush(Color.FromArgb(r, g, b)), path);
            }
        }

        private void DrawGaoLiang(Graphics g, bool xiacen)
        {
            Rectangle rect = this.ClientRectangle;
            rect.Inflate(-4, -4);

            if (xiacen)
            {
                rect.Y = rect.Y + 4;
            }
            GraphicsPath path = GetGraphicsPath1(rect);
            RectangleF rect1 = path.GetBounds();
            rect1.Height = rect1.Height + 1;
            g.FillPath(new LinearGradientBrush(rect1,
             Color.FromArgb(0xff, 0xff, 0xff, 0xff),
             Color.FromArgb(0xff, backColor), LinearGradientMode.Vertical), path);
        }

        private void DrawText(Graphics g, bool xiacen)
        {
            Rectangle rect = this.ClientRectangle;
            Rectangle rect1 = this.ClientRectangle;
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            rect.Y = this.ClientRectangle.Height / 5;
            if (xiacen)
            {
                rect.Y = rect.Y + 4;
                rect1.Y = rect1.Y + 4;
            }

            Font font = this.Font;


            if (mouseMove)
            {
                font = new Font(this.Font, FontStyle.Underline);
            }

            g.DrawString(this.Text, font,
             new SolidBrush(Color.FromArgb(0x66, backColor)), rect, stringFormat);
            g.DrawString(this.Text, font, new SolidBrush(this.ForeColor), rect1, stringFormat);
        }

        private void UserControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (XiaCen == false)
            {
                XiaCen = true;
                this.Refresh();
            }
        }

        private void UserControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (XiaCen == true)
            {
                XiaCen = false;
                this.Refresh();
            }
        }

        private void UserControl1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {


        }

        protected override void OnPaint(PaintEventArgs e)
        {

            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(backColor), 0, 0, this.Width, this.Height);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            GraphicsPath ClientPath = GetGraphicsPath(rect);
            e.Graphics.FillPath(new SolidBrush(backColor), ClientPath);
            this.Region = new System.Drawing.Region(ClientPath);
            DrawYinYing(e.Graphics, XiaCen);
            DrawGaoLiang(e.Graphics, XiaCen);
            DrawText(e.Graphics, XiaCen);

            if (this.Focused)
            {
                e.Graphics.DrawPath(new Pen(Color.FromArgb(0x22, 0xff, 0xff, 0xff), 3), ClientPath);
            }

        }





        private void UserControl1_BackColorChanged(object sender, System.EventArgs e)
        {
            int r = BackColor.R;
            int g = BackColor.G;
            int b = BackColor.B;
            r = r + 0x22;
            g = g + 0x22;
            b = b + 0x22;
            if (r > 255) r = 255;
            if (g > 255) g = 255;
            if (b > 255) b = 255;
            backColor = Color.FromArgb(r, g, b);
        }

        private void UserControl1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (XiaCen == false && e.KeyCode == Keys.Space)
            {
                XiaCen = true;
                this.Refresh();
            }
        }

        private void UserControl1_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (XiaCen == true && e.KeyCode == Keys.Space)
            {
                XiaCen = false;
                this.Refresh();
            }
        }

        private void UserControl1_MouseEnter(object sender, System.EventArgs e)
        {
            if (mouseMove == false)
            {
                mouseMove = true;
                this.Refresh();
            }
        }

        private void UserControl1_MouseLeave(object sender, System.EventArgs e)
        {
            if (mouseMove == true)
            {
                mouseMove = false;
                this.Refresh();
            }
        }

    }
}