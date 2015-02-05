namespace JLQ.Common
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class DropDownListForm : Form
    {
        private IContainer components;
        private int dropDownAnimCount;
        private static int dropDownAnimFrameNum = 10;
        private static int dropDownAnimOpenTime = 100;
        private Timer dropDownAnimTimer;
        private DropDownDirection dropDownDir;
        internal ListBox dropDownListBox;
        private Point dropDownOrigin;
        private ToolTip dropDownToolTip;
        private DropDownListFormFilter filter;
        private MultiLineComboBox multiLineComboBox;

        public DropDownListForm()
        {
            this.InitializeComponent();
            this.filter = new DropDownListFormFilter(this);
        }

        public int CalcDropDownHeight()
        {
            int preferredHeight = 0;
            if (this.dropDownListBox.Items.Count > this.multiLineComboBox.MaxDropDownItems)
            {
                for (int i = 0; i < this.multiLineComboBox.MaxDropDownItems; i++)
                {
                    preferredHeight += this.dropDownListBox.GetItemHeight(i);
                }
            }
            else
            {
                preferredHeight = this.dropDownListBox.PreferredHeight;
            }
            if (preferredHeight > this.multiLineComboBox.DropDownHeight)
            {
                preferredHeight = this.multiLineComboBox.DropDownHeight;
            }
            if (preferredHeight == 0)
            {
                preferredHeight = this.dropDownListBox.ItemHeight + (this.dropDownListBox.ItemHeight / 2);
            }
            return preferredHeight;
        }

        private void CancelDropDown()
        {
            this.multiLineComboBox.CancelDropDown();
        }

        public void CloseDropDown()
        {
            Application.RemoveMessageFilter(this.filter);
            this.dropDownAnimTimer.Enabled = false;
            base.Visible = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void DropDownAnimation()
        {
            this.dropDownAnimCount++;
            double num = ((double) this.dropDownAnimCount) / ((double) dropDownAnimFrameNum);
            if (this.dropDownDir == DropDownDirection.upward)
            {
                int height = (int) (this.dropDownListBox.Height * num);
                int y = this.dropDownOrigin.Y - height;
                base.SetBounds(this.dropDownOrigin.X, y, base.Width, height);
            }
            else
            {
                int num4 = (int) (this.dropDownListBox.Height * num);
                base.SetBounds(this.dropDownOrigin.X, this.dropDownOrigin.Y, base.Width, num4);
                this.dropDownListBox.Location = new Point(0, num4 - this.dropDownListBox.Height);
            }
            if (base.Height >= this.dropDownListBox.Height)
            {
                this.dropDownAnimCount = 0;
                this.dropDownAnimTimer.Enabled = false;
            }
            else if (!this.dropDownAnimTimer.Enabled)
            {
                this.dropDownAnimTimer.Enabled = true;
            }
        }

        private void dropDownAnimTimer_Tick(object sender, EventArgs e)
        {
            this.DropDownAnimation();
        }

        private void dropDownListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            this.multiLineComboBox._RaiseDrawItem(e);
        }

        private void dropDownListBox_Format(object sender, ListControlConvertEventArgs e)
        {
            this.multiLineComboBox._RaiseFormat(e);
        }

        private void dropDownListBox_MouseClick(object sender, MouseEventArgs e)
        {
            int num = this.dropDownListBox.IndexFromPoint(e.X, e.Y);
            if (this.dropDownListBox.SelectedIndex != num)
            {
                this.dropDownListBox.SelectedIndex = num;
            }
            if (this.dropDownListBox.Items.Count > 0)
            {
                this.multiLineComboBox.CommitDropDown();
            }
            else
            {
                this.multiLineComboBox.CancelDropDown();
            }
        }

        private void dropDownListBox_MouseMove(object sender, MouseEventArgs e)
        {
            int index = this.dropDownListBox.IndexFromPoint(e.X, e.Y);
            if (this.dropDownListBox.SelectedIndex != index)
            {
                this.dropDownListBox.SelectedIndex = index;
                if ((index >= 0) && this.multiLineComboBox.CalcIsShowToolTip(index, this.dropDownListBox.Text))
                {
                    this.dropDownToolTip.Active = true;
                    this.dropDownToolTip.SetToolTip(this.dropDownListBox, this.dropDownListBox.Text);
                }
                else
                {
                    this.dropDownToolTip.Active = false;
                }
            }
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.dropDownListBox = new ListBox();
            this.dropDownAnimTimer = new Timer(this.components);
            this.dropDownToolTip = new ToolTip(this.components);
            base.SuspendLayout();
            this.dropDownListBox.BorderStyle = BorderStyle.FixedSingle;
            this.dropDownListBox.ItemHeight = 12;
            this.dropDownListBox.Location = new Point(0, 0);
            this.dropDownListBox.Margin = new Padding(0);
            this.dropDownListBox.Name = "dropDownListBox";
            this.dropDownListBox.Size = new Size(0x79, 0x7a);
            this.dropDownListBox.TabIndex = 0;
            this.dropDownListBox.TabStop = false;
            this.dropDownListBox.DrawItem += new DrawItemEventHandler(this.dropDownListBox_DrawItem);
            this.dropDownListBox.MouseClick += new MouseEventHandler(this.dropDownListBox_MouseClick);
            this.dropDownListBox.MouseMove += new MouseEventHandler(this.dropDownListBox_MouseMove);
            this.dropDownListBox.Format += new ListControlConvertEventHandler(this.dropDownListBox_Format);
            this.dropDownAnimTimer.Interval = 1;
            this.dropDownAnimTimer.Tick += new EventHandler(this.dropDownAnimTimer_Tick);
            this.dropDownToolTip.ShowAlways = true;
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            base.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = SystemColors.Window;
            base.ClientSize = new Size(0x79, 0x7a);
            base.ControlBox = false;
            base.Controls.Add(this.dropDownListBox);
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "DropDownListForm";
            base.StartPosition = FormStartPosition.Manual;
            this.Text = "DropDownListForm";
            base.TopMost = true;
            base.ResumeLayout(false);
        }

        public bool IsOutSide(Point scrn_cursor)
        {
            Point point = base.PointToClient(scrn_cursor);
            return ((((point.X < 0) || (point.X >= base.Width)) || ((point.Y < 0) || (point.Y >= base.Height))) && !this.multiLineComboBox.InButtonArea(this.multiLineComboBox.PointToClient(scrn_cursor)));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.dropDownListBox.Width = base.Width;
        }

        public void ShowDropDown()
        {
            this.multiLineComboBox._RaiseDropDown(new EventArgs());
            Application.AddMessageFilter(this.filter);
            Point p = new Point(0, this.multiLineComboBox.Height);
            p = this.multiLineComboBox.PointToScreen(p);
            if (((p.Y + base.Height) > Screen.GetBounds(this).Height) && (p.Y > ((Screen.GetBounds(this).Height + this.multiLineComboBox.Height) / 2)))
            {
                this.dropDownDir = DropDownDirection.upward;
                p.Offset(0, -this.multiLineComboBox.Height);
            }
            else
            {
                this.dropDownDir = DropDownDirection.downward;
            }
            if ((base.Width < Screen.GetBounds(this).Width) && ((p.X + base.Width) > Screen.PrimaryScreen.WorkingArea.Width))
            {
                p.X = Screen.GetBounds(this).Width - base.Width;
            }
            if ((p.X < 0) && (base.Width <= Screen.GetBounds(this).Width))
            {
                p.X = 0;
            }
            this.dropDownListBox.Height = this.CalcDropDownHeight() + 2;
            this.dropDownOrigin = p;
            if (ExAppearanceManager.GetAppearanceManager().DoSlideOpenComboBox() && (this.multiLineComboBox.SlidingDropDown == MultiLineComboBoxEffect.Standard))
            {
                dropDownAnimFrameNum = 10;
            }
            else
            {
                dropDownAnimFrameNum = 1;
            }
            this.dropDownAnimTimer.Interval = dropDownAnimOpenTime / dropDownAnimFrameNum;
            this.DropDownAnimation();
            base.Visible = true;
            this.dropDownListBox.Height = this.CalcDropDownHeight() + 2;
            this.dropDownAnimCount = 0;
            this.DropDownAnimation();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if ((m.Msg == 0x1c) && base.Visible)
            {
                this.CancelDropDown();
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Style = 0x40000000;
                createParams.ExStyle = 0x8000008;
                if (ExAppearanceManager.GetAppearanceManager().DoDropShadowComboBox())
                {
                    createParams.ClassStyle |= 0x20000;
                }
                createParams.Parent = IntPtr.Zero;
                return createParams;
            }
        }

        public string ItemText
        {
            get
            {
                return this.dropDownListBox.Text;
            }
        }

        public MultiLineComboBox MultiLineComboBox
        {
            set
            {
                this.multiLineComboBox = value;
            }
        }

        private enum DropDownDirection
        {
            downward,
            upward
        }

        private class DropDownListFormFilter : IMessageFilter
        {
            private DropDownListForm owner;

            public DropDownListFormFilter(DropDownListForm owner)
            {
                this.owner = owner;
            }

            public bool PreFilterMessage(ref Message m)
            {
                switch (m.Msg)
                {
                    case 0x201:
                    case 0x204:
                    case 0x207:
                    case 0xa1:
                    case 0xa4:
                    case 0xa7:
                        if (this.owner.IsOutSide(Cursor.Position))
                        {
                            this.owner.CancelDropDown();
                        }
                        break;
                }
                return false;
            }
        }
    }
}

