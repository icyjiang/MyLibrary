namespace JLQ.Common
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    internal class ComboBoxListTextArea : Label
    {
        private bool _hilight;
        private bool _wordwrap = true;
        private IContainer components;
        private MultiLineComboBox multiLineComboBox;
        private Color saveForeColor;

        public ComboBoxListTextArea()
        {
            this.InitializeComponent();
            base.BackColor = Color.Transparent;
            this.saveForeColor = base.ForeColor;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if ((this.multiLineComboBox != null) && (this.multiLineComboBox.DrawMode != DrawMode.Normal))
            {
                this.multiLineComboBox._RaiseDrawItem(new DrawItemEventArgs(e.Graphics, this.Font, e.ClipRectangle, this.multiLineComboBox.SelectedIndex, DrawItemState.ComboBoxEdit, this.ForeColor, this.BackColor));
            }
            else
            {
                if (this.WordWrap)
                {
                    base.OnPaint(e);
                }
                else
                {
                    base.OnPaintBackground(e);
                    Color foreColor = this.ForeColor;
                    if (!base.Enabled)
                    {
                        foreColor = SystemColors.GrayText;
                    }
                    ExAppearanceManager.GetAppearanceManager().DrawText(this.Text, this.Font, this.multiLineComboBox.TextAlign, this.multiLineComboBox.TextVerticalAlign, false, e.Graphics, new Rectangle(0, 0, base.Width, base.Height), foreColor);
                }
                if (this.Hilight && this.ShowFocusCues)
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, base.ClientRectangle, Color.Black, this.BackColor);
                }
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                m.Result = (IntPtr) (-1);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
            }
        }

        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
                this.saveForeColor = value;
            }
        }

        internal bool Hilight
        {
            get
            {
                return this._hilight;
            }
            set
            {
                if (this._hilight != value)
                {
                    this._hilight = value;
                    if (this.multiLineComboBox.GetStyle() == ExAppearanceStyle.Vista)
                    {
                        this.ForeColor = this.saveForeColor;
                    }
                    else if (this._hilight)
                    {
                        this.saveForeColor = this.ForeColor;
                        base.BackColor = SystemColors.Highlight;
                        base.ForeColor = SystemColors.HighlightText;
                    }
                    else
                    {
                        base.BackColor = Color.Transparent;
                        base.ForeColor = this.saveForeColor;
                    }
                }
            }
        }

        public MultiLineComboBox MultiLineComboBox
        {
            set
            {
                this.multiLineComboBox = value;
            }
        }

        public bool WordWrap
        {
            get
            {
                return this._wordwrap;
            }
            set
            {
                if (this._wordwrap != value)
                {
                    this._wordwrap = value;
                    base.Invalidate();
                }
            }
        }
    }
}

