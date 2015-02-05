namespace JLQ.Common
{
    using System;
    using System.Collections;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Windows.Forms;
    using System.Windows.Forms.VisualStyles;

    [DefaultEvent("SelectedIndexChanged"), /*LicenseProvider(typeof(TsControlsLicenseProvider)), */DefaultProperty("Items"), ToolboxBitmap(typeof(System.Windows.Forms.ComboBox))]
    public class MultiLineComboBox : ContainerControl
    {
        private ExComboBoxState _combostate;
        private int _dropDownHeight = 0x6a;
        private MultiLineComboBoxStyle _dropDownStyle;
        private int _dropDownWidth = 0x79;
        private MultiLineComboBoxEffect _fadingEffect;
        private ObjectCollection _items;
        private int _leftmergin;
        private int _maxDropDownItems = 8;
        private int _selectedIndex = -1;
        private object _selectedItem;
        private object _selectedValue;
        private bool _showDropDownToolTip;
        private MultiLineComboBoxEffect _slidingDropDown;
        private MultiLineComboBoxAppearance _style;
        private string _text = "";
        private int _topmergin;
        private VerticalAlignment _vertAlign;
        private Rectangle buttonArea = new Rectangle(0x67, 1, 0x11, 0x20);
        private ComboBoxListTextArea comboBoxListTextArea;
        internal ComboBoxTextArea comboBoxTextArea;
        private IContainer components;
        //internal static bool didLicenseChecked = false;
        internal DropDownListForm dropDownListForm;
        private Timer fadeAnimTimer;
        private static int fadeFrameNum = 10;
        private static int fadeInTime = 200;
        private static int fadeOutTime = 600;
        private bool in_transaction;
        private Bitmap lastBitmap;
        //private License license;
        private Bitmap overlayBitmap;
        private float overlayRate;
        private bool overlaySwaped;
        private bool stopRaiseTextChangeEvent;
        private const int WM_SETFOCUS = 7;

        [ExDescription("MlcbDesc_DataSourceChanged"), Category("Property Changed")]
        public event EventHandler DataSourceChanged;

        [ExDescription("MlcbDesc_DisplayMemberChanged"), Category("Property Changed")]
        public event EventHandler DisplayMemberChanged;

        [ExDescription("MlcbDesc_DrawItem"), Category("Behavior")]
        public event DrawItemEventHandler DrawItem;

        [Category("Behavior"), ExDescription("MlcbDesc_DropDown")]
        public event EventHandler DropDown;

        [Category("Behavior"), ExDescription("MlcbDesc_DropDownClosed")]
        public event EventHandler DropDownClosed;

        [Category("Behavior"), ExDescription("MlcbDesc_DropDownStyleChanged")]
        public event EventHandler DropDownStyleChanged;

        [ExDescription("MlcbDesc_Format"), Category("Property Changed")]
        public event ListControlConvertEventHandler Format;

        [ExDescription("MlcbDesc_FormatInfoChanged"), Category("Property Changed")]
        public event EventHandler FormatInfoChanged;

        [ExDescription("MlcbDesc_FormatStringChanged"), Category("Property Changed")]
        public event EventHandler FormatStringChanged;

        [Category("Property Changed"), ExDescription("MlcbDesc_FormattingEnabledChanged")]
        public event EventHandler FormattingEnabledChanged;

        [Category("Property Changed"), ExDescription("MlcbDesc_SelectedIndexChanged")]
        public event EventHandler SelectedIndexChanged;

        [Category("Property Changed"), ExDescription("MlcbDesc_SelectedValueChanged")]
        public event EventHandler SelectedValueChanged;

        [Category("Property Changed"), ExDescription("MlcbDesc_SelectionChangeCommitted")]
        public event EventHandler SelectionChangeCommitted;

        [ExDescription("MlcbDesc_TextAlignChanged"), Category("Property Changed")]
        public event EventHandler TextAlignChanged;

        [ExDescription("MlcbDesc_ValueMemberChanged"), Category("Property Changed")]
        public event EventHandler ValueMemberChanged;

        public MultiLineComboBox()
        {
            ExAppearanceManager.GetAppearanceManager();
            //if (!didLicenseChecked)
            //{
            //    try
            //    {
            //        this.license = LicenseManager.Validate(typeof(MultiLineComboBox), this);
            //    }
            //    catch (LicenseException)
            //    {
            //        new RegistrationForm().ShowDialog();
            //    }
            //    didLicenseChecked = true;
            //}
            this.dropDownListForm = new DropDownListForm();
            this.dropDownListForm.Hide();
            this._items = new ObjectCollection(this);
            this.InitializeComponent();
            this.dropDownListForm.MultiLineComboBox = this;
            this.comboBoxTextArea.MultiLineComboBox = this;
            this.comboBoxListTextArea.MultiLineComboBox = this;
            this.DoubleBuffered = true;
            this.BackColor = SystemColors.Window;
            this.overlayBitmap = null;
            this.lastBitmap = null;
            this.overlayRate = 0f;
            this.overlaySwaped = false;
        }

        private void _OnKeyDown(KeyEventArgs e)
        {
            if (!this.IsDropDownShowing())
            {
                if ((e.KeyCode == Keys.F4) || ((e.Modifiers == Keys.Alt) && ((e.KeyCode == Keys.Down) || (e.KeyCode == Keys.Up))))
                {
                    this.SetComboState(ExComboBoxState.DropDown);
                    this.ShowDropDown();
                    e.Handled = true;
                }
                else if ((this.comboBoxTextArea.ReadOnly || (this.comboBoxTextArea.SelectionLength == this.comboBoxTextArea.Text.Length)) && (e.Modifiers == Keys.None))
                {
                    if (e.KeyCode == Keys.Up)
                    {
                        if (this._selectedIndex > 0)
                        {
                            this._SetSelectedIndex(this._selectedIndex - 1, true);
                        }
                        this.comboBoxTextArea.SelectAll();
                        e.Handled = true;
                        return;
                    }
                    if (e.KeyCode == Keys.Down)
                    {
                        if (this._selectedIndex < (this._items.Count - 1))
                        {
                            this._SetSelectedIndex(this._selectedIndex + 1, true);
                        }
                        this.comboBoxTextArea.SelectAll();
                        e.Handled = true;
                    }
                }
                return;
            }
            int selectedIndex = this.dropDownListForm.dropDownListBox.SelectedIndex;
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (e.Modifiers != Keys.Alt)
                    {
                        if (selectedIndex > 0)
                        {
                            this._SetSelectedIndex(selectedIndex - 1, true);
                        }
                        break;
                    }
                    this.CancelDropDown();
                    break;

                case Keys.Down:
                    if (e.Modifiers != Keys.Alt)
                    {
                        if (selectedIndex < (this._items.Count - 1))
                        {
                            this._SetSelectedIndex(selectedIndex + 1, true);
                        }
                    }
                    else
                    {
                        this.CancelDropDown();
                    }
                    this.comboBoxTextArea.SelectAll();
                    goto Label_00B9;

                case Keys.F4:
                case Keys.Escape:
                    this.CancelDropDown();
                    goto Label_00B9;

                default:
                    goto Label_00B9;
            }
            this.comboBoxTextArea.SelectAll();
        Label_00B9:
            e.Handled = true;
        }

        internal void _RaiseDrawItem(DrawItemEventArgs e)
        {
            this.OnDrawItem(e);
        }

        internal void _RaiseDropDown(EventArgs e)
        {
            this.OnDropDown(e);
        }

        internal void _RaiseDropDownClosed(EventArgs e)
        {
            this.OnDropDownClosed(e);
        }

        internal void _RaiseEnter(EventArgs e)
        {
            this.OnEnterTextbox(e);
        }

        internal void _RaiseFormat(ListControlConvertEventArgs e)
        {
            this.OnFormat(e);
        }

        internal void _RaiseGotFocus(EventArgs e)
        {
            this.OnGotFocusTextbox(e);
        }

        internal void _RaiseKeyDown(KeyEventArgs e)
        {
            this.OnKeyDown(e);
        }

        internal void _RaiseKeyPress(KeyPressEventArgs e)
        {
            this.OnKeyPress(e);
        }

        internal void _RaiseKeyUp(KeyEventArgs e)
        {
            this.OnKeyUp(e);
        }

        internal void _RaiseLeave(EventArgs e)
        {
            this.OnLeaveTextbox(e);
        }

        internal void _RaiseLostFocus(EventArgs e)
        {
            this.OnLostFocusTextbox(e);
        }

        internal void _RaiseMouseMove(MouseEventArgs e)
        {
            this.OnMouseMove(e);
        }

        internal void _RaiseSelectedIndexChanged(EventArgs e)
        {
            this.OnSelectedIndexChanged(e);
        }

        internal void _RaiseTextChanged(EventArgs e)
        {
            if (!this.stopRaiseTextChangeEvent)
            {
                this.OnTextChanged(e);
            }
        }

        internal void _SetSelectedIndex(int index, bool user_commited)
        {
            if ((index < -1) || (index >= this.Items.Count))
            {
                throw new ArgumentOutOfRangeException();
            }
            int num = this._selectedIndex;
            object obj2 = this._selectedValue;
            this._selectedIndex = index;
            if (this._selectedIndex != -1)
            {
                this._selectedItem = this._items[this._selectedIndex];
                PropertyInfo info = this._selectedItem.GetType().GetProperty("Item", typeof(object), new System.Type[] { typeof(string) });
                if ((info != null) && (this.DisplayMember != ""))
                {
                    this._text = info.GetValue(this._selectedItem, new object[] { this.DisplayMember }).ToString();
                }
                else
                {
                    this._text = this._selectedItem.ToString();
                }
                if ((info != null) && (this.ValueMember != ""))
                {
                    this._selectedValue = info.GetValue(this._selectedItem, new object[] { this.ValueMember });
                }
                else
                {
                    this._selectedValue = this._selectedItem.ToString();
                }
            }
            else
            {
                this._text = "";
                this._selectedItem = null;
                this._selectedValue = null;
            }
            if (user_commited && (this._selectedIndex != num))
            {
                this.in_transaction = true;
                this.OnSelectionChangedCommited(new EventArgs());
                this.in_transaction = false;
            }
            if (!this.in_transaction)
            {
                this.dropDownListForm.dropDownListBox.SelectedIndex = this._selectedIndex;
                this.comboBoxTextArea.Text = this._text;
                this.comboBoxListTextArea.Text = this._text;
                if ((((this._selectedValue == null) && (obj2 != null)) || ((this._selectedValue != null) && (obj2 == null))) || (((this._selectedValue != null) || (obj2 != null)) && ((this._selectedValue.GetType() != obj2.GetType()) || !this._selectedValue.Equals(obj2))))
                {
                    this.OnSelectedValueChanged(new EventArgs());
                }
                if (this._selectedIndex != num)
                {
                    this.OnSelectedIndexChanged(new EventArgs());
                }
            }
        }

        internal void _SetSelectedIndexOnly(int index, bool raiseTextChange)
        {
            int num = this._selectedIndex;
            object obj2 = this._selectedValue;
            if (this._selectedIndex != index)
            {
                this._selectedIndex = index;
            }
            if (this._selectedIndex != -1)
            {
                this._selectedItem = this._items[this._selectedIndex];
                PropertyInfo info = this._selectedItem.GetType().GetProperty("Item", typeof(object), new System.Type[] { typeof(string) });
                if ((info != null) && (this.ValueMember != ""))
                {
                    this._selectedValue = info.GetValue(this._selectedItem, new object[] { this.ValueMember });
                }
                else
                {
                    this._selectedValue = this._selectedItem.ToString();
                }
            }
            else
            {
                this._selectedItem = null;
                this._selectedValue = null;
            }
            this._text = this.comboBoxListTextArea.Text;
            if (!this.in_transaction)
            {
                this.dropDownListForm.dropDownListBox.SelectedIndex = this._selectedIndex;
                if (raiseTextChange)
                {
                    this.OnTextChanged(new EventArgs());
                }
                if ((((this._selectedValue == null) && (obj2 != null)) || ((this._selectedValue != null) && (obj2 == null))) || (((this._selectedValue != null) || (obj2 != null)) && ((this._selectedValue.GetType() != obj2.GetType()) || !this._selectedValue.Equals(obj2))))
                {
                    this.OnSelectedValueChanged(new EventArgs());
                }
                if (this._selectedIndex != num)
                {
                    this.OnSelectedIndexChanged(new EventArgs());
                }
            }
        }

        internal void AdjustDropDownListUpdated()
        {
            if (base.InvokeRequired)
            {
                base.Invoke(new VoidProc(this.AdjustDropDownListUpdated));
            }
            else if (this._selectedItem != null)
            {
                int index = this.FindStringExact(this._text);
                if (index >= 0)
                {
                    this._SetSelectedIndex(index, false);
                }
                else
                {
                    this._SetSelectedIndexOnly(-1, false);
                }
            }
        }

        public virtual void BeginUpdate()
        {
            this.dropDownListForm.dropDownListBox.BeginUpdate();
        }

        internal bool CalcIsShowToolTip(int index, string text)
        {
            return this.IsShowToolTip(index, text);
        }

        internal void CancelDropDown()
        {
            this.dropDownListForm.CloseDropDown();
            this.dropDownListForm.dropDownListBox.SelectedIndex = this._selectedIndex;
            this.OnDropDownClosed(new EventArgs());
            this.comboBoxTextArea.SelectAll();
            this.comboBoxListTextArea.Hilight = true;
            Point p = base.PointToClient(Cursor.Position);
            if (this.InButtonArea(p))
            {
                this.SetComboState(ExComboBoxState.FocusedOnButton);
            }
            else
            {
                this.SetComboState(ExComboBoxState.Focused);
            }
        }

        public virtual void ClearUndo()
        {
            this.comboBoxTextArea.ClearUndo();
        }

        internal void CommitDropDown()
        {
            this.dropDownListForm.CloseDropDown();
            this._SetSelectedIndex(this.dropDownListForm.dropDownListBox.SelectedIndex, true);
            this.comboBoxTextArea.SelectAll();
            this.comboBoxListTextArea.Hilight = true;
            Point p = base.PointToClient(Cursor.Position);
            if (this.InButtonArea(p))
            {
                this.SetComboState(ExComboBoxState.FocusedOnButton);
            }
            else
            {
                this.SetComboState(ExComboBoxState.Focused);
            }
        }

        public virtual void Copy()
        {
            this.comboBoxTextArea.Copy();
        }

        public virtual void Cut()
        {
            this.comboBoxTextArea.Cut();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            if (disposing)// && (this.license != null))
            {
                this.SelectedValueChanged = null;
                this.SelectedIndexChanged = null;
                this.SelectionChangeCommitted = null;
                this.DataSourceChanged = null;
                this.DataSource = null;
                //this.license.Dispose();
                //this.license = null;
            }
            base.Dispose(disposing);
        }

        internal void DropDownListChanged(object sender, ListChangedEventArgs e)
        {
            this.AdjustDropDownListUpdated();
        }

        public virtual void EndUpdate()
        {
            this.dropDownListForm.dropDownListBox.EndUpdate();
        }

        private void fadeAnimTimer_Tick(object sender, EventArgs e)
        {
            this.overlayRate -= 0.1f;
            if (this.overlayRate <= 0f)
            {
                this.overlayRate = 0f;
                this.fadeAnimTimer.Enabled = false;
            }
            base.Invalidate(false);
        }

        public virtual int FindString(string s)
        {
            return this.dropDownListForm.dropDownListBox.FindString(s);
        }

        public virtual int FindString(string s, int startIndex)
        {
            return this.dropDownListForm.dropDownListBox.FindString(s, startIndex);
        }

        public virtual int FindStringExact(string s)
        {
            return this.dropDownListForm.dropDownListBox.FindStringExact(s);
        }

        public virtual int FindStringExact(string s, int startIndex)
        {
            return this.dropDownListForm.dropDownListBox.FindStringExact(s, startIndex);
        }

        private System.Drawing.ContentAlignment GetContentAlignment(HorizontalAlignment h, VerticalAlignment v)
        {
            switch (h)
            {
                case HorizontalAlignment.Left:
                    switch (v)
                    {
                        case VerticalAlignment.Top:
                            return System.Drawing.ContentAlignment.TopLeft;

                        case VerticalAlignment.Center:
                            return System.Drawing.ContentAlignment.MiddleLeft;

                        case VerticalAlignment.Bottom:
                            return System.Drawing.ContentAlignment.BottomLeft;
                    }
                    break;

                case HorizontalAlignment.Right:
                    switch (v)
                    {
                        case VerticalAlignment.Top:
                            return System.Drawing.ContentAlignment.TopRight;

                        case VerticalAlignment.Center:
                            return System.Drawing.ContentAlignment.MiddleRight;

                        case VerticalAlignment.Bottom:
                            return System.Drawing.ContentAlignment.BottomRight;
                    }
                    break;

                case HorizontalAlignment.Center:
                    switch (v)
                    {
                        case VerticalAlignment.Top:
                            return System.Drawing.ContentAlignment.TopCenter;

                        case VerticalAlignment.Center:
                            return System.Drawing.ContentAlignment.MiddleCenter;

                        case VerticalAlignment.Bottom:
                            return System.Drawing.ContentAlignment.BottomCenter;
                    }
                    break;
            }
            return System.Drawing.ContentAlignment.TopLeft;
        }

        public virtual int GetItemHeight(int index)
        {
            return this.dropDownListForm.dropDownListBox.GetItemHeight(index);
        }

        public virtual string GetItemText(object item)
        {
            return this.dropDownListForm.dropDownListBox.GetItemText(item);
        }

        internal ExAppearanceStyle GetStyle()
        {
            if (this.Style == MultiLineComboBoxAppearance.Classic)
            {
                return ExAppearanceManager.GetAppearanceManager().GetStyle(ExAppearanceStyle.Classic);
            }
            if (this.Style == MultiLineComboBoxAppearance.XP)
            {
                return ExAppearanceManager.GetAppearanceManager().GetStyle(ExAppearanceStyle.XP);
            }
            return ExAppearanceManager.GetAppearanceManager().GetStyle(ExAppearanceStyle.Standard);
        }

        internal bool InButtonArea(Point p)
        {
            if (this.DropDownStyle == MultiLineComboBoxStyle.DropDownList)
            {
                return ((((p.X >= 0) && (p.X < base.Width)) && (p.Y >= 0)) && (p.Y < base.Height));
            }
            return ((((p.X >= this.buttonArea.X) && (p.X < (this.buttonArea.X + this.buttonArea.Width))) && (p.Y >= this.buttonArea.Y)) && (p.Y < (this.buttonArea.Y + this.buttonArea.Height)));
        }

        internal bool InComboBoxArea(Point p)
        {
            return ((((p.X >= 0) && (p.X < base.Width)) && (p.Y >= 0)) && (p.Y < base.Height));
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            this.comboBoxListTextArea = new ComboBoxListTextArea();
            this.comboBoxTextArea = new ComboBoxTextArea();
            this.fadeAnimTimer = new Timer(this.components);
            base.SuspendLayout();
            this.comboBoxListTextArea.BackColor = Color.Transparent;
            this.comboBoxListTextArea.Location = new Point(3, 3);
            this.comboBoxListTextArea.Name = "comboBoxListTextArea";
            this.comboBoxListTextArea.Size = new Size(0x62, 0x1c);
            this.comboBoxListTextArea.TabIndex = 2;
            this.comboBoxListTextArea.Visible = false;
            this.comboBoxListTextArea.WordWrap = true;
            this.comboBoxTextArea.BorderStyle = BorderStyle.None;
            this.comboBoxTextArea.Cursor = Cursors.IBeam;
            this.comboBoxTextArea.Location = new Point(3, 3);
            this.comboBoxTextArea.Margin = new Padding(0);
            this.comboBoxTextArea.Multiline = true;
            this.comboBoxTextArea.Name = "comboBoxTextArea";
            this.comboBoxTextArea.Size = new Size(0x62, 0x1c);
            this.comboBoxTextArea.TabIndex = 1;
            this.fadeAnimTimer.Tick += new EventHandler(this.fadeAnimTimer_Tick);
            this.BackColor = SystemColors.Control;
            base.Controls.Add(this.comboBoxListTextArea);
            base.Controls.Add(this.comboBoxTextArea);
            base.Size = new Size(0x79, 0x22);
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        internal bool IsDropDownShowing()
        {
            return this.dropDownListForm.Visible;
        }

        protected override bool IsInputKey(Keys keyData)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Up:
                case Keys.Down:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected virtual bool IsShowToolTip(int index, string text)
        {
            if (!this.ShowDropDownToolTip)
            {
                return false;
            }
            return (text.Contains("\n") || (text.Contains("\r") || ((TextRenderer.MeasureText(Graphics.FromHwnd(this.dropDownListForm.Handle), text, this.DropDownFont).Width + 0x12) > this.dropDownListForm.Width)));
        }

        protected virtual void OnDataSourceChanged(EventArgs e)
        {
            if (this.DataSourceChanged != null)
            {
                this.DataSourceChanged(this, e);
            }
        }

        protected virtual void OnDisplayMemberChanged(EventArgs e)
        {
            if (this.DisplayMemberChanged != null)
            {
                this.DisplayMemberChanged(this, e);
            }
        }

        protected virtual void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.DrawItem != null)
            {
                this.DrawItem(this, e);
            }
        }

        protected virtual void OnDropDown(EventArgs e)
        {
            if (this.DropDown != null)
            {
                this.DropDown(this, e);
            }
        }

        protected virtual void OnDropDownClosed(EventArgs e)
        {
            if (this.DropDownClosed != null)
            {
                this.DropDownClosed(this, e);
            }
        }

        protected virtual void OnDropDownStyleChanged(EventArgs e)
        {
            if (this.DropDownStyleChanged != null)
            {
                this.DropDownStyleChanged(this, e);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            this.comboBoxListTextArea.Hilight = true;
            Point p = base.PointToClient(Cursor.Position);
            if (this.InButtonArea(p))
            {
                this.SetComboState(ExComboBoxState.FocusedOnButton);
            }
            else
            {
                this.SetComboState(ExComboBoxState.Focused);
            }
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDownList)
            {
                base.OnEnter(e);
            }
            else
            {
                this.comboBoxTextArea.Focus();
            }
        }

        protected virtual void OnEnterTextbox(EventArgs e)
        {
            this.comboBoxListTextArea.Hilight = true;
            Point p = base.PointToClient(Cursor.Position);
            if (this.InButtonArea(p))
            {
                this.SetComboState(ExComboBoxState.FocusedOnButton);
            }
            else
            {
                this.SetComboState(ExComboBoxState.Focused);
            }
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDown)
            {
                base.OnEnter(e);
            }
        }

        protected virtual void OnFormat(ListControlConvertEventArgs e)
        {
            if (this.Format != null)
            {
                this.Format(this, e);
            }
        }

        protected virtual void OnFormatInfoChanged(EventArgs e)
        {
            if (this.FormatInfoChanged != null)
            {
                this.FormatInfoChanged(this, e);
            }
        }

        protected virtual void OnFormatStringChanged(EventArgs e)
        {
            if (this.FormatStringChanged != null)
            {
                this.FormatStringChanged(this, e);
            }
        }

        protected virtual void OnFormattingEnabledChanged(EventArgs e)
        {
            if (this.FormattingEnabledChanged != null)
            {
                this.FormattingEnabledChanged(this, e);
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDownList)
            {
                base.OnGotFocus(e);
            }
        }

        protected virtual void OnGotFocusTextbox(EventArgs e)
        {
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDown)
            {
                base.OnGotFocus(e);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (!e.Handled)
            {
                this._OnKeyDown(e);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (!e.Handled && this.IsDropDownShowing())
            {
                if (e.KeyChar == Convert.ToChar(13))
                {
                    this.CommitDropDown();
                }
                else
                {
                    int index = this.FindString(new string(e.KeyChar, 1), this._selectedIndex);
                    if (index >= 0)
                    {
                        this._SetSelectedIndex(index, true);
                    }
                }
                e.Handled = true;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            this.comboBoxListTextArea.Hilight = false;
            if (this.IsDropDownShowing())
            {
                this.CancelDropDown();
            }
            Point p = base.PointToClient(Cursor.Position);
            if (this.InComboBoxArea(p))
            {
                if (this.InButtonArea(p))
                {
                    this.SetComboState(ExComboBoxState.HotOnButton);
                }
                else
                {
                    this.SetComboState(ExComboBoxState.Hot);
                }
            }
            else
            {
                this.SetComboState(ExComboBoxState.Normal);
            }
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDownList)
            {
                base.OnLeave(e);
            }
        }

        protected virtual void OnLeaveTextbox(EventArgs e)
        {
            this.comboBoxListTextArea.Hilight = false;
            if (this.IsDropDownShowing())
            {
                this.CancelDropDown();
            }
            Point p = base.PointToClient(Cursor.Position);
            if (this.InComboBoxArea(p))
            {
                if (this.InButtonArea(p))
                {
                    this.SetComboState(ExComboBoxState.HotOnButton);
                }
                else
                {
                    this.SetComboState(ExComboBoxState.Hot);
                }
            }
            else
            {
                this.SetComboState(ExComboBoxState.Normal);
            }
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDown)
            {
                base.OnLeave(e);
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDownList)
            {
                base.OnLostFocus(e);
            }
        }

        protected virtual void OnLostFocusTextbox(EventArgs e)
        {
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDown)
            {
                base.OnLostFocus(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDownList)
            {
                base.Select();
            }
            else
            {
                this.comboBoxTextArea.Select();
            }
            if (((e.Button == MouseButtons.Left) && (this.comboBoxTextArea.Focused || this.Focused)) && this.InButtonArea(new Point(e.X, e.Y)))
            {
                if (this.IsDropDownShowing())
                {
                    this.SetComboState(ExComboBoxState.Normal);
                    this.CancelDropDown();
                }
                else
                {
                    this.SetComboState(ExComboBoxState.PressButton);
                    this.ShowDropDown();
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!this.IsDropDownShowing())
            {
                if (this.Focused)
                {
                    this.SetComboState(ExComboBoxState.Focused);
                }
                else
                {
                    Point p = base.PointToClient(Cursor.Position);
                    if (!this.InComboBoxArea(p))
                    {
                        this.SetComboState(ExComboBoxState.Normal);
                    }
                }
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.InButtonArea(new Point(e.X, e.Y)))
            {
                if (!this.IsDropDownShowing())
                {
                    if (this.Focused)
                    {
                        this.SetComboState(ExComboBoxState.FocusedOnButton);
                    }
                    else
                    {
                        this.SetComboState(ExComboBoxState.HotOnButton);
                    }
                }
            }
            else if ((this._combostate != ExComboBoxState.PressButton) && !this.IsDropDownShowing())
            {
                if (this.Focused)
                {
                    this.SetComboState(ExComboBoxState.Focused);
                }
                else
                {
                    this.SetComboState(ExComboBoxState.Hot);
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X, e.Y);
                if (this.InButtonArea(p))
                {
                    if (this.IsDropDownShowing())
                    {
                        this.SetComboState(ExComboBoxState.DropDown);
                    }
                    else
                    {
                        this.SetComboState(ExComboBoxState.HotOnButton);
                    }
                }
                else if (this.InComboBoxArea(p))
                {
                    this.SetComboState(ExComboBoxState.Hot);
                }
                else
                {
                    this.SetComboState(ExComboBoxState.Normal);
                }
            }
            base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            Rectangle borderArea = new Rectangle(0, 0, base.Width, base.Height);
            if (this.GetStyle() == ExAppearanceStyle.Vista)
            {
                Rectangle buttonArea = new Rectangle(this.buttonArea.X + 1, this.buttonArea.Y - 1, this.buttonArea.Width, this.buttonArea.Height + 2);
                ExAppearanceManager appearanceManager = ExAppearanceManager.GetAppearanceManager();
                if (base.Enabled)
                {
                    appearanceManager.DrawComboBoxVista(base.Handle, e.Graphics, borderArea, buttonArea, this._dropDownStyle, this._combostate, this.DrawBorder, ref this.overlayBitmap, this.overlayRate, ref this.lastBitmap);
                }
                else
                {
                    appearanceManager.DrawComboBoxVista(base.Handle, e.Graphics, borderArea, buttonArea, this._dropDownStyle, ExComboBoxState.Disabled, this.DrawBorder, ref this.overlayBitmap, this.overlayRate, ref this.lastBitmap);
                }
                this.overlaySwaped = false;
            }
            else if (this.GetStyle() == ExAppearanceStyle.XP)
            {
                ExAppearanceManager manager2 = ExAppearanceManager.GetAppearanceManager();
                if (base.Enabled)
                {
                    manager2.DrawComboBoxXP(base.Handle, e.Graphics, borderArea, this.buttonArea, this._combostate, this.DrawBorder);
                }
                else
                {
                    manager2.DrawComboBoxXP(base.Handle, e.Graphics, borderArea, this.buttonArea, ExComboBoxState.Disabled, this.DrawBorder);
                }
                this.overlaySwaped = false;
            }
            else
            {
                if (this.DrawBorder)
                {
                    ControlPaint.DrawBorder3D(e.Graphics, borderArea, Border3DStyle.Sunken);
                }
                Rectangle rectangle = new Rectangle(this.buttonArea.X - 1, this.buttonArea.Y + 1, this.buttonArea.Width, this.buttonArea.Height - 2);
                if (base.Enabled)
                {
                    ButtonState pushed;
                    if (this._combostate == ExComboBoxState.PressButton)
                    {
                        pushed = ButtonState.Pushed;
                    }
                    else
                    {
                        pushed = ButtonState.Normal;
                    }
                    ControlPaint.DrawComboButton(e.Graphics, rectangle, pushed);
                }
                else
                {
                    ControlPaint.DrawComboButton(e.Graphics, rectangle, ButtonState.Inactive);
                }
            }
            base.OnPaint(e);
        }

        protected virtual void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedIndexChanged != null)
            {
                this.SelectedIndexChanged(this, e);
            }
            Binding binding = base.DataBindings["SelectedIndex"];
            if ((binding != null) && (binding.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged))
            {
                binding.WriteValue();
            }
        }

        protected virtual void OnSelectedValueChanged(EventArgs e)
        {
            if (this.SelectedValueChanged != null)
            {
                this.SelectedValueChanged(this, e);
            }
            Binding binding = base.DataBindings["SelectedValue"];
            if ((binding != null) && (binding.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged))
            {
                binding.WriteValue();
            }
        }

        protected virtual void OnSelectionChangedCommited(EventArgs e)
        {
            if (this.SelectionChangeCommitted != null)
            {
                this.SelectionChangeCommitted(this, e);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.buttonArea.X = (base.Width - this.buttonArea.Width) - 1;
            this.buttonArea.Height = base.Height - 2;
            this.comboBoxListTextArea.Width = this.comboBoxTextArea.Width = (this.buttonArea.X - 4) - this._leftmergin;
            this.comboBoxListTextArea.Height = this.comboBoxTextArea.Height = (base.Height - 6) - this._topmergin;
            if (this._dropDownWidth > base.Width)
            {
                this.dropDownListForm.Width = this._dropDownWidth;
            }
            else
            {
                this.dropDownListForm.Width = base.Width;
            }
            base.Invalidate();
        }

        protected virtual void OnTextAlignChanged(EventArgs e)
        {
            if (this.TextAlignChanged != null)
            {
                this.TextAlignChanged(this, e);
            }
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Binding binding = base.DataBindings["Text"];
            if ((binding != null) && (binding.DataSourceUpdateMode == DataSourceUpdateMode.OnPropertyChanged))
            {
                binding.WriteValue();
            }
        }

        protected override void OnValidated(EventArgs e)
        {
            for (int i = 0; i < base.DataBindings.Count; i++)
            {
                if (base.DataBindings[i].DataSourceUpdateMode == DataSourceUpdateMode.OnValidation)
                {
                    base.DataBindings[i].WriteValue();
                }
            }
            base.OnValidated(e);
        }

        protected virtual void OnValueMemberChanged(EventArgs e)
        {
            if (this.ValueMemberChanged != null)
            {
                this.ValueMemberChanged(this, e);
            }
        }

        private void Paste()
        {
            this.comboBoxTextArea.Paste();
        }

        public void ScrollToCaret()
        {
            this.comboBoxTextArea.ScrollToCaret();
        }

        protected override void Select(bool directed, bool forward)
        {
            if (this._dropDownStyle == MultiLineComboBoxStyle.DropDown)
            {
                base.SelectNextControl(null, true, true, true, false);
            }
            else
            {
                base.Select(directed, forward);
            }
        }

        public virtual void Select(int start, int length)
        {
            this.comboBoxTextArea.Select(start, length);
        }

        public virtual void SelectAll()
        {
            this.comboBoxTextArea.SelectAll();
        }

        private void SetComboState(ExComboBoxState state)
        {
            if (this._combostate != state)
            {
                if (!this.overlaySwaped)
                {
                    Bitmap overlayBitmap = this.overlayBitmap;
                    this.overlayBitmap = this.lastBitmap;
                    this.lastBitmap = overlayBitmap;
                }
                if (((this._combostate == ExComboBoxState.PressButton) || (state == ExComboBoxState.PressButton)) || ((state == ExComboBoxState.DropDown) || (this.FadeEffect == MultiLineComboBoxEffect.Off)))
                {
                    this.overlayRate = 0f;
                }
                else
                {
                    this.overlayRate = 1f - (1f / ((float) fadeFrameNum));
                    if (((state == ExComboBoxState.Normal) || (this._combostate == ExComboBoxState.HotOnButton)) || (this._combostate == ExComboBoxState.FocusedOnButton))
                    {
                        this.fadeAnimTimer.Interval = fadeOutTime / fadeFrameNum;
                    }
                    else
                    {
                        this.fadeAnimTimer.Interval = fadeInTime / fadeFrameNum;
                    }
                    this.fadeAnimTimer.Enabled = true;
                }
                this.overlaySwaped = true;
                this._combostate = state;
                base.Invalidate(true);
            }
        }

        private bool ShouldSerializeBackColor()
        {
            return !this.BackColor.Equals(SystemColors.Window);
        }

        private bool ShouldSerializeDropDownBackColor()
        {
            return !this.DropDownBackColor.Equals(SystemColors.Window);
        }

        private bool ShouldSerializeDropDownFont()
        {
            return !this.DropDownFont.Equals(SystemFonts.DefaultFont);
        }

        private bool ShouldSerializeDropDownForeColor()
        {
            return !this.DropDownForeColor.Equals(SystemColors.WindowText);
        }

        private bool ShouldSerializeItems()
        {
            return (this._items.Count != 0);
        }

        internal void ShowDropDown()
        {
            this.dropDownListForm.ShowDropDown();
            if (this._dropDownStyle != MultiLineComboBoxStyle.DropDownList)
            {
                this.comboBoxTextArea.Focus();
            }
            else
            {
                base.Focus();
            }
            this.comboBoxListTextArea.Hilight = false;
        }

        public void Undo()
        {
            this.comboBoxTextArea.Undo();
        }

        protected override void WndProc(ref Message m)
        {
            if ((m.Msg == 7) && (this._dropDownStyle == MultiLineComboBoxStyle.DropDown))
            {
                base.SelectNextControl(null, true, true, true, false);
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        [DefaultValue(false), Category("Behavior"), ExDescription("MlcbDesc_AcceptsReturn")]
        public virtual bool AcceptsReturn
        {
            get
            {
                return this.comboBoxTextArea.AcceptsReturn;
            }
            set
            {
                this.comboBoxTextArea.AcceptsReturn = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), ExDescription("MlcbDesc_AcceptsTab")]
        public virtual bool AcceptsTab
        {
            get
            {
                return this.comboBoxTextArea.AcceptsTab;
            }
            set
            {
                this.comboBoxTextArea.AcceptsTab = value;
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
                base.BackColor = value;
                this.comboBoxTextArea.BackColor = value;
            }
        }

        [DefaultValue(0x11), Category("Appearance"), ExDescription("MlcbDesc_ButtonWidth")]
        public virtual int ButtonWidth
        {
            get
            {
                return this.buttonArea.Width;
            }
            set
            {
                if (value <= 0)
                {
                    value = 0x11;
                }
                else if (value > (base.Width - 6))
                {
                    value = base.Width - 6;
                }
                this.buttonArea.Width = value;
                this.OnSizeChanged(new EventArgs());
            }
        }

        [AttributeProvider(typeof(IListSource)), Category("Data"), DefaultValue((string) null), ExDescription("MlcbDesc_DataSource")]
        public virtual object DataSource
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.DataSource;
            }
            set
            {
                object dataSource = this.dropDownListForm.dropDownListBox.DataSource;
                if (this.dropDownListForm.dropDownListBox.DataSource != null)
                {
                    if (dataSource is DataTable)
                    {
                        ((DataTable) dataSource).DefaultView.ListChanged -= new ListChangedEventHandler(this.DropDownListChanged);
                    }
                    else if (dataSource is IBindingList)
                    {
                        ((IBindingList) dataSource).ListChanged -= new ListChangedEventHandler(this.DropDownListChanged);
                    }
                    else if (dataSource is IBindingListView)
                    {
                        ((IBindingList) dataSource).ListChanged -= new ListChangedEventHandler(this.DropDownListChanged);
                    }
                }
                this.dropDownListForm.dropDownListBox.DataSource = value;
                if (value != null)
                {
                    if (value is DataTable)
                    {
                        ((DataTable) value).DefaultView.ListChanged += new ListChangedEventHandler(this.DropDownListChanged);
                    }
                    else if (value is IBindingList)
                    {
                        ((IBindingList) value).ListChanged += new ListChangedEventHandler(this.DropDownListChanged);
                    }
                    else if (value is IBindingListView)
                    {
                        ((IBindingList) value).ListChanged += new ListChangedEventHandler(this.DropDownListChanged);
                    }
                }
                this.AdjustDropDownListUpdated();
                if ((((value == null) && (dataSource != null)) || ((value != null) && (dataSource == null))) || (((value != null) || (dataSource != null)) && ((value.GetType() != dataSource.GetType()) || !value.Equals(dataSource))))
                {
                    this.OnDataSourceChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(""), Category("Data"), ExDescription("MlcbDesc_DisplayMember")]
        public virtual string DisplayMember
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.DisplayMember;
            }
            set
            {
                if (this.dropDownListForm.dropDownListBox.DisplayMember != value)
                {
                    bool flag = false;
                    this.dropDownListForm.dropDownListBox.DisplayMember = value;
                    this.dropDownListForm.dropDownListBox.SelectedIndex = this._selectedIndex;
                    if ((this._selectedIndex != -1) && (this.comboBoxTextArea.Text != this.dropDownListForm.dropDownListBox.Text))
                    {
                        flag = true;
                        this.stopRaiseTextChangeEvent = true;
                        this.comboBoxTextArea.Text = this.dropDownListForm.dropDownListBox.Text;
                        this.comboBoxListTextArea.Text = this.dropDownListForm.dropDownListBox.Text;
                        this.stopRaiseTextChangeEvent = false;
                    }
                    this.OnDisplayMemberChanged(new EventArgs());
                    if (flag)
                    {
                        this.OnTextChanged(new EventArgs());
                    }
                }
            }
        }

        protected virtual bool DrawBorder
        {
            get
            {
                return true;
            }
        }

        [DefaultValue(0), ExDescription("MlcbDesc_DrawMode"), Category("Behavior")]
        public virtual System.Windows.Forms.DrawMode DrawMode
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.DrawMode;
            }
            set
            {
                this.dropDownListForm.dropDownListBox.DrawMode = value;
            }
        }

        [Category("Appearance"), ExDescription("MlcbDesc_DropDownBackColor")]
        public virtual Color DropDownBackColor
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.BackColor;
            }
            set
            {
                this.dropDownListForm.BackColor = value;
                this.dropDownListForm.dropDownListBox.BackColor = value;
            }
        }

        [Category("Appearance"), ExDescription("MlcbDesc_DropDownFont")]
        public virtual Font DropDownFont
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.Font;
            }
            set
            {
                this.dropDownListForm.dropDownListBox.Font = value;
            }
        }

        [ExDescription("MlcbDesc_DropDownForeColor"), Category("Appearance")]
        public virtual Color DropDownForeColor
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.ForeColor;
            }
            set
            {
                this.dropDownListForm.ForeColor = value;
                this.dropDownListForm.dropDownListBox.ForeColor = value;
            }
        }

        [Category("Behavior"), ExDescription("MlcbDesc_DropDownHeight"), DefaultValue(0x6a)]
        public virtual int DropDownHeight
        {
            get
            {
                return this._dropDownHeight;
            }
            set
            {
                this._dropDownHeight = value;
            }
        }

        [ExDescription("MlcbDesc_DropDownStyle"), Category("Appearance"), DefaultValue(0)]
        public virtual MultiLineComboBoxStyle DropDownStyle
        {
            get
            {
                return this._dropDownStyle;
            }
            set
            {
                if (this._dropDownStyle != value)
                {
                    if (value == MultiLineComboBoxStyle.DropDownList)
                    {
                        this.comboBoxListTextArea.Text = this.comboBoxTextArea.Text;
                        this.comboBoxListTextArea.Visible = true;
                        this.comboBoxTextArea.Visible = false;
                    }
                    else
                    {
                        this.comboBoxTextArea.Visible = true;
                        this.comboBoxListTextArea.Visible = false;
                    }
                    this._dropDownStyle = value;
                    this.OnDropDownStyleChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(0x79), ExDescription("MlcbDesc_DropDownWidth"), Category("Behavior")]
        public virtual int DropDownWidth
        {
            get
            {
                return this._dropDownWidth;
            }
            set
            {
                this._dropDownWidth = value;
                if (this._dropDownWidth > base.Width)
                {
                    this.dropDownListForm.Width = this._dropDownWidth;
                }
                else
                {
                    this.dropDownListForm.Width = base.Width;
                }
            }
        }

        [Browsable(false), DefaultValue(false)]
        public virtual bool DroppedDown
        {
            get
            {
                return this.IsDropDownShowing();
            }
            set
            {
                if (this.IsDropDownShowing())
                {
                    if (!value)
                    {
                        this.CancelDropDown();
                    }
                }
                else if (value)
                {
                    this.ShowDropDown();
                }
            }
        }

        [ExDescription("MlcbDesc_FadeEffect"), Category("Behavior"), DefaultValue(0)]
        public virtual MultiLineComboBoxEffect FadeEffect
        {
            get
            {
                return this._fadingEffect;
            }
            set
            {
                this._fadingEffect = value;
            }
        }

        public override bool Focused
        {
            get
            {
                if (this._dropDownStyle == MultiLineComboBoxStyle.DropDownList)
                {
                    return base.Focused;
                }
                return this.comboBoxTextArea.Focused;
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
                this.comboBoxListTextArea.ForeColor = value;
                this.comboBoxTextArea.ForeColor = value;
                base.ForeColor = value;
            }
        }

        [DefaultValue((string) null), Browsable(false)]
        public virtual IFormatProvider FormatInfo
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.FormatInfo;
            }
            set
            {
                if (this.dropDownListForm.dropDownListBox.FormatInfo != value)
                {
                    this.dropDownListForm.dropDownListBox.FormatInfo = value;
                    this.OnFormatInfoChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(""), ExDescription("MlcbDesc_FormatString")]
        public virtual string FormatString
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.FormatString;
            }
            set
            {
                if (this.dropDownListForm.dropDownListBox.FormatString != value)
                {
                    this.dropDownListForm.dropDownListBox.FormatString = value;
                    this.OnFormatStringChanged(new EventArgs());
                }
            }
        }

        [ExDescription("MlcbDesc_FormattingEnabled"), DefaultValue(false)]
        public virtual bool FormattingEnabled
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.FormattingEnabled;
            }
            set
            {
                if (this.dropDownListForm.dropDownListBox.FormattingEnabled != value)
                {
                    this.dropDownListForm.dropDownListBox.FormattingEnabled = value;
                    this.OnFormattingEnabledChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(12), ExDescription("MlcbDesc_ItemHeight"), Category("Behavior")]
        public virtual int ItemHeight
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.ItemHeight;
            }
            set
            {
                this.dropDownListForm.dropDownListBox.ItemHeight = value;
            }
        }

        [Editor("System.Windows.Forms.Design.StringCollectionEditor,System.Design", "System.Drawing.Design.UITypeEditor,System.Drawing"), Category("Data"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), ExDescription("MlcbDesc_Items")]
        public virtual ObjectCollection Items
        {
            get
            {
                return this._items;
            }
        }

        [DefaultValue(8), Category("Behavior"), ExDescription("MlcbDesc_MaxDropDownItems")]
        public virtual int MaxDropDownItems
        {
            get
            {
                return this._maxDropDownItems;
            }
            set
            {
                this._maxDropDownItems = value;
            }
        }

        [Category("Behavior"), ExDescription("MlcbDesc_MaxLength"), DefaultValue(0x7fff)]
        public virtual int MaxLength
        {
            get
            {
                return this.comboBoxTextArea.MaxLength;
            }
            set
            {
                this.comboBoxTextArea.MaxLength = value;
            }
        }

        [Browsable(false)]
        public virtual int PreferredHeight
        {
            get
            {
                return (this.comboBoxTextArea.PreferredHeight + 6);
            }
        }

        [ExDescription("MlcbDesc_ReadOnly"), Category("Behavior"), DefaultValue(false)]
        public virtual bool ReadOnly
        {
            get
            {
                return this.comboBoxTextArea.ReadOnly;
            }
            set
            {
                this.comboBoxTextArea.ReadOnly = value;
            }
        }

        [Bindable(true), ExDescription("MlcbDesc_SelectedIndex"), Browsable(false), DefaultValue(-1)]
        public virtual int SelectedIndex
        {
            get
            {
                return this._selectedIndex;
            }
            set
            {
                this._SetSelectedIndex(value, false);
            }
        }

        [Browsable(false), DefaultValue((string) null)]
        public virtual object SelectedItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                for (int i = 0; i < this._items.Count; i++)
                {
                    object obj2 = this._items[i];
                    if (obj2 != null)
                    {
                        if (obj2.Equals(value))
                        {
                            this._SetSelectedIndex(i, false);
                            return;
                        }
                    }
                    else if (value == null)
                    {
                        this._SetSelectedIndex(i, false);
                        return;
                    }
                }
                if (value != null)
                {
                    throw new IndexOutOfRangeException(string.Format(Properties.Resources.MlcbExpt_CntFindItem, value.ToString(), value.GetType().ToString()));
                }
                this._SetSelectedIndex(-1, false);
            }
        }

        [DefaultValue(""), Browsable(false)]
        public virtual string SelectedText
        {
            get
            {
                return this.comboBoxTextArea.SelectedText;
            }
            set
            {
                this.comboBoxTextArea.SelectedText = value;
            }
        }

        [Bindable(true), DefaultValue((string) null), Browsable(false)]
        public virtual object SelectedValue
        {
            get
            {
                return this._selectedValue;
            }
            set
            {
                for (int i = 0; i < this._items.Count; i++)
                {
                    object obj2 = this._items[i];
                    if ((obj2 != null) && (value != null))
                    {
                        PropertyInfo info = obj2.GetType().GetProperty("Item", typeof(object), new System.Type[] { typeof(string) });
                        if ((info == null) || (this.ValueMember == ""))
                        {
                            if (obj2.ToString() == value.ToString())
                            {
                                this._SetSelectedIndex(i, false);
                                return;
                            }
                        }
                        else if (info.GetValue(obj2, new object[] { this.ValueMember }).Equals(value))
                        {
                            this._SetSelectedIndex(i, false);
                            return;
                        }
                    }
                    else if ((value == null) && (obj2 == null))
                    {
                        this._SetSelectedIndex(i, false);
                        return;
                    }
                }
                if (value != null)
                {
                    throw new IndexOutOfRangeException(string.Format(Properties.Resources.MlcbExpt_CntFindValue, value.ToString(), value.GetType().ToString()));
                }
                this._SetSelectedIndex(-1, false);
            }
        }

        [DefaultValue(0), Browsable(false)]
        public virtual int SelectionLength
        {
            get
            {
                return this.comboBoxTextArea.SelectionLength;
            }
            set
            {
                this.comboBoxTextArea.SelectionLength = value;
            }
        }

        [Browsable(false), DefaultValue(0)]
        public virtual int SelectionStart
        {
            get
            {
                return this.comboBoxTextArea.SelectionStart;
            }
            set
            {
                this.comboBoxTextArea.SelectionStart = value;
            }
        }

        [DefaultValue(true), Category("Behavior"), ExDescription("MlcbDesc_ShortcutsEnabled")]
        public virtual bool ShortcutsEnabled
        {
            get
            {
                return this.comboBoxTextArea.ShortcutsEnabled;
            }
            set
            {
                this.comboBoxTextArea.ShortcutsEnabled = value;
            }
        }

        [Category("Behavior"), DefaultValue(false), ExDescription("MlcbDesc_ShowDropDownToolTip")]
        public virtual bool ShowDropDownToolTip
        {
            get
            {
                return this._showDropDownToolTip;
            }
            set
            {
                this._showDropDownToolTip = true;
            }
        }

        [Category("Behavior"), ExDescription("MlcbDesc_SlidingDropDown"), DefaultValue(0)]
        public virtual MultiLineComboBoxEffect SlidingDropDown
        {
            get
            {
                return this._slidingDropDown;
            }
            set
            {
                this._slidingDropDown = value;
            }
        }

        [ExDescription("MlcbDesc_Sorted"), Category("Behavior"), DefaultValue(false)]
        public virtual bool Sorted
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.Sorted;
            }
            set
            {
                if (this.dropDownListForm.dropDownListBox.Sorted != value)
                {
                    this.dropDownListForm.dropDownListBox.Sorted = value;
                    this._SetSelectedIndex(-1, false);
                }
            }
        }

        [Category("Appearance"), DefaultValue(0), ExDescription("MlcbDesc_Style")]
        public virtual MultiLineComboBoxAppearance Style
        {
            get
            {
                return this._style;
            }
            set
            {
                this._style = value;
                base.Invalidate();
            }
        }

        public override string Text
        {
            get
            {
                if (this.in_transaction)
                {
                    return this._text;
                }
                return this.comboBoxTextArea.Text;
            }
            set
            {
                if (this.in_transaction)
                {
                    if (this._text != value)
                    {
                        this._text = value;
                        this._SetSelectedIndexOnly(-1, true);
                    }
                }
                else if (this.comboBoxTextArea.Text != value)
                {
                    this._text = value;
                    this.comboBoxTextArea.setByUserProgram = true;
                    this.comboBoxTextArea.Text = value;
                    this.comboBoxListTextArea.Text = value;
                }
            }
        }

        [DefaultValue(0), ExDescription("MlcbDesc_TextAlign"), Category("Appearance")]
        public virtual HorizontalAlignment TextAlign
        {
            get
            {
                return this.comboBoxTextArea.TextAlign;
            }
            set
            {
                if (this.comboBoxTextArea.TextAlign != value)
                {
                    this.comboBoxTextArea.TextAlign = value;
                    this.comboBoxListTextArea.TextAlign = this.GetContentAlignment(value, this._vertAlign);
                    this.OnTextAlignChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(0), Browsable(false)]
        public virtual int TextLeftMargin
        {
            get
            {
                return this._leftmergin;
            }
            set
            {
                if (this._leftmergin != value)
                {
                    this.comboBoxTextArea.SetBounds(3 + value, 0, (base.Width - 0x17) - value, 0, BoundsSpecified.Width | BoundsSpecified.X);
                    this.comboBoxListTextArea.SetBounds(3 + value, 0, (base.Width - 0x17) - value, 0, BoundsSpecified.Width | BoundsSpecified.X);
                    this._leftmergin = value;
                }
            }
        }

        [DefaultValue(0), Browsable(false)]
        public virtual int TextTopMargin
        {
            get
            {
                return this._topmergin;
            }
            set
            {
                if (this._topmergin != value)
                {
                    this.comboBoxTextArea.SetBounds(0, 3 + value, 0, (base.Height - 6) - value, BoundsSpecified.Height | BoundsSpecified.Y);
                    this.comboBoxListTextArea.SetBounds(0, 3 + value, 0, (base.Height - 6) - value, BoundsSpecified.Height | BoundsSpecified.Y);
                    this._topmergin = value;
                }
            }
        }

        [DefaultValue(0), ExDescription("MlcbDesc_TextVerticalAlign"), Category("Appearance")]
        public virtual VerticalAlignment TextVerticalAlign
        {
            get
            {
                return this._vertAlign;
            }
            set
            {
                if (this._vertAlign != value)
                {
                    this._vertAlign = value;
                    this.comboBoxListTextArea.TextAlign = this.GetContentAlignment(this.TextAlign, value);
                    this.OnTextAlignChanged(new EventArgs());
                }
            }
        }

        [Category("Data"), ExDescription("MlcbDesc_ValueMember"), DefaultValue("")]
        public virtual string ValueMember
        {
            get
            {
                return this.dropDownListForm.dropDownListBox.ValueMember;
            }
            set
            {
                if (this.dropDownListForm.dropDownListBox.ValueMember != value)
                {
                    this.dropDownListForm.dropDownListBox.ValueMember = value;
                    this.OnValueMemberChanged(new EventArgs());
                }
            }
        }

        [DefaultValue(true), Category("Behavior"), ExDescription("MlcbDesc_WordWrap")]
        public virtual bool WordWrap
        {
            get
            {
                return this.comboBoxTextArea.WordWrap;
            }
            set
            {
                this.comboBoxTextArea.WordWrap = value;
                this.comboBoxListTextArea.WordWrap = value;
            }
        }

        public class ObjectCollection : IList, ICollection, IEnumerable
        {
            private ListBox listbox;
            private MultiLineComboBox parent;

            internal ObjectCollection(MultiLineComboBox parent)
            {
                this.parent = parent;
                this.listbox = parent.dropDownListForm.dropDownListBox;
            }

            public int Add(object item)
            {
                int num = this.listbox.Items.Add(item);
                this.parent.AdjustDropDownListUpdated();
                return num;
            }

            public void AddRange(object[] items)
            {
                this.listbox.Items.AddRange(items);
                this.parent.AdjustDropDownListUpdated();
            }

            public virtual void Clear()
            {
                this.listbox.Items.Clear();
                this.parent.AdjustDropDownListUpdated();
            }

            public bool Contains(object value)
            {
                return this.listbox.Items.Contains(value);
            }

            public void CopyTo(object[] destination, int arrayIndex)
            {
                this.listbox.Items.CopyTo(destination, arrayIndex);
            }

            public IEnumerator GetEnumerator()
            {
                return this.listbox.Items.GetEnumerator();
            }

            public int IndexOf(object value)
            {
                return this.listbox.Items.IndexOf(value);
            }

            public void Insert(int index, object item)
            {
                this.listbox.Items.Insert(index, item);
                this.parent.AdjustDropDownListUpdated();
            }

            public void Remove(object value)
            {
                this.listbox.Items.Remove(value);
                this.parent.AdjustDropDownListUpdated();
            }

            public void RemoveAt(int index)
            {
                this.listbox.Items.RemoveAt(index);
                this.parent.AdjustDropDownListUpdated();
            }

            void ICollection.CopyTo(Array destination, int index)
            {
                ((ICollection) this.listbox.Items).CopyTo(destination, index);
            }

            public int Count
            {
                get
                {
                    return this.listbox.Items.Count;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return this.listbox.Items.IsReadOnly;
                }
            }

            public virtual object this[int index]
            {
                get
                {
                    return this.listbox.Items[index];
                }
                set
                {
                    this.listbox.Items[index] = value;
                    this.parent.AdjustDropDownListUpdated();
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return ((ICollection) this.listbox.Items).IsSynchronized;
                }
            }

            object ICollection.SyncRoot
            {
                get
                {
                    return ((ICollection) this.listbox.Items).SyncRoot;
                }
            }

            bool IList.IsFixedSize
            {
                get
                {
                    return ((IList) this.listbox.Items).IsFixedSize;
                }
            }
        }

        private delegate void VoidProc();
    }
}

