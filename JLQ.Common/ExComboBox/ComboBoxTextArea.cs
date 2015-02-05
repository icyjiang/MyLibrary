namespace JLQ.Common
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;

    internal class ComboBoxTextArea : TextBox
    {
        private IContainer components;
        private bool firstMouseDown;
        private MultiLineComboBox multiLineComboBox;
        private int saveSelLength;
        internal bool setByUserProgram;
        private bool startMouseDownAllSelect;

        public ComboBoxTextArea()
        {
            this.InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        [DllImport("User32.dll")]
        private static extern bool HideCaret(IntPtr hwnd);
        private void InitializeComponent()
        {
            base.SuspendLayout();
            this.Multiline = true;
            base.ResumeLayout(false);
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            base.SelectAll();
            this.firstMouseDown = true;
            this.multiLineComboBox._RaiseEnter(e);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (base.ReadOnly)
            {
                HideCaret(base.Handle);
            }
            this.multiLineComboBox._RaiseGotFocus(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            this.multiLineComboBox._RaiseKeyDown(e);
            if (!e.Handled)
            {
                base.OnKeyDown(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            this.multiLineComboBox._RaiseKeyPress(e);
            if (!e.Handled)
            {
                base.OnKeyPress(e);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            this.multiLineComboBox._RaiseKeyUp(e);
            if (!e.Handled)
            {
                base.OnKeyUp(e);
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);
            base.SelectionStart = 0;
            this.SelectionLength = 0;
            base.ScrollToCaret();
            this.multiLineComboBox._RaiseLeave(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            this.multiLineComboBox._RaiseLostFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (this.firstMouseDown)
            {
                if (this.saveSelLength != this.Text.Length)
                {
                    base.SelectAll();
                    this.startMouseDownAllSelect = true;
                }
                this.firstMouseDown = false;
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseEventArgs args = new MouseEventArgs(e.Button, e.Clicks, e.X + base.Left, e.Y + base.Top, e.Delta);
            this.multiLineComboBox._RaiseMouseMove(args);
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            if (this.startMouseDownAllSelect)
            {
                this.startMouseDownAllSelect = false;
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (base.Modified || this.setByUserProgram)
            {
                base.Modified = false;
                this.setByUserProgram = false;
                int index = this.multiLineComboBox.FindStringExact(this.Text);
                this.multiLineComboBox._SetSelectedIndexOnly(index, true);
            }
            else
            {
                this.multiLineComboBox._RaiseTextChanged(e);
            }
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
        }

        protected override void WndProc(ref Message m)
        {
            if (!this.startMouseDownAllSelect || (m.Msg != 0x200))
            {
                if (m.Msg == 0x201)
                {
                    this.saveSelLength = this.SelectionLength;
                }
                base.WndProc(ref m);
            }
        }

        public MultiLineComboBox MultiLineComboBox
        {
            set
            {
                this.multiLineComboBox = value;
            }
        }
    }
}

