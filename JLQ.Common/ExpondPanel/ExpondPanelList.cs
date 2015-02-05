using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace JLQ.Common
{
    #region Class ExpondPanelList
    [Designer(typeof(ExpondPanelListDesigner)),
    DesignTimeVisibleAttribute(true)]
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
    public partial class ExpondPanelList : ScrollableControl
    {
        #region Events
        [Description("The PanelStyleChanged event occurs when PanelStyle flags have been changed.")]
        public event EventHandler<PanelStyleChangeEventArgs> PanelStyleChanged;
        [Description("The CaptionStyleChanged event occurs when CaptionStyle flags have been changed.")]
        public event EventHandler<EventArgs> CaptionStyleChanged;
        [Description("The ColorSchemeChanged event occurs when ColorScheme flags have been changed.")]
        public event EventHandler<ColorSchemeChangeEventArgs> ColorSchemeChanged;
        [Description("Occurs when the value of the CaptionHeight property changes.")]
        public event EventHandler<EventArgs> CaptionHeightChanged;
        #endregion

        #region FieldsPrivate

        private bool _isShowBorder;
        private bool _isShowGradientBackground;
        private bool _isShowExpandIcon;
        private bool _isShowCloseIcon;
        private int _captionHeight;
        private LinearGradientMode _linearGradientMode;
        private Color _colorGradientBackground;
        private CaptionStyle _captionStyle;
        private PanelStyle _panelStyle;
        private ColorScheme _colorScheme;
        private ExpondPanelCollection _expondPanels;
        private PanelColors _panelColors;

        #endregion

        #region Properties
        [RefreshProperties(RefreshProperties.Repaint), Category("Collections"), Browsable(true)]
        [Description("Collection containing all the ExpondPanels for the expondpanellist.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Editor(typeof(ExpondPanelCollectionEditor), typeof(UITypeEditor))]
        public ExpondPanelCollection ExpondPanels
        {
            get { return this._expondPanels; }
        }
        [Description("Specifies the style of the expondpanels in this expondpanellist.")]
        [DefaultValue(PanelStyle.Default), Category("Appearance")]
        public PanelStyle PanelStyle
        {
            get { return this._panelStyle; }
            set
            {
                if (value != this._panelStyle)
                {
                    this._panelStyle = value;
                    OnPanelStyleChanged(this, new PanelStyleChangeEventArgs(this._panelStyle));
                }
            }
        }
        public PanelColors PanelColors
        {
            get { return this._panelColors; }
            set { this._panelColors = value; }
        }
        [Description("The colorscheme of the expondpanels in the expondpanellist")]
        [DefaultValue(ColorScheme.Professional), Category("Appearance")]
        public ColorScheme ColorScheme
        {
            get { return this._colorScheme; }
            set
            {
                if (value != this._colorScheme)
                {
                    this._colorScheme = value;
                    OnColorSchemeChanged(this, new ColorSchemeChangeEventArgs(this._colorScheme));
                }
            }
        }
        [Description("The style of the captionbar."), Category("Appearance")]
        public CaptionStyle CaptionStyle
        {
            get { return this._captionStyle; }
            set
            {
                this._captionStyle = value;
                OnCaptionStyleChanged(this, EventArgs.Empty);
            }
        }

        [Description("LinearGradientMode of the background in the expondpanellist")]
        [DefaultValue(LinearGradientMode.Vertical), Category("Appearance")]
        public LinearGradientMode LinearGradientMode
        {
            get { return this._linearGradientMode; }
            set
            {
                if (value != this._linearGradientMode)
                {
                    this._linearGradientMode = value;
                    this.Invalidate(false);
                }
            }
        }
        [Description("Gets or sets a value indicating whether a expondpanellist's gradient background is shown.")]
        [DefaultValue(false), Category("Appearance")]
        public bool ShowGradientBackground
        {
            get { return this._isShowGradientBackground; }
            set
            {
                if (value != this._isShowGradientBackground)
                {
                    this._isShowGradientBackground = value;
                    this.Invalidate(false);
                }
            }
        }

        [Description("Gets or sets a value indicating whether a expondpanellist's border is shown")]
        [DefaultValue(true), Category("Appearance")]
        public bool ShowBorder
        {
            get { return this._isShowBorder; }
            set
            {
                if (value != this._isShowBorder)
                {
                    this._isShowBorder = value;
                    foreach (ExpondPanel expondPanel in this.ExpondPanels)
                    {
                        expondPanel.ShowBorder = this._isShowBorder;
                    }
                    this.Invalidate(false);
                }
            }
        }

        [Description("Gets or sets a value indicating whether the expand icon of the expondpanels in this expondpanellist are visible.")]
        [DefaultValue(false), Category("Appearance")]
        public bool ShowExpandIcon
        {
            get { return this._isShowExpandIcon; }
            set
            {
                if (value != this._isShowExpandIcon)
                {
                    this._isShowExpandIcon = value;
                    foreach (ExpondPanel expondPanel in this.ExpondPanels)
                    {
                        expondPanel.ShowExpandIcon = this._isShowExpandIcon;
                    }
                }
            }
        }

        [Description("Gets or sets a value indicating whether the close icon of the expondpanels in this expondpanellist are visible.")]
        [DefaultValue(false), Category("Appearance")]
        public bool ShowCloseIcon
        {
            get { return this._isShowCloseIcon; }
            set
            {
                if (value != this._isShowCloseIcon)
                {
                    this._isShowCloseIcon = value;
                    foreach (ExpondPanel expondPanel in this.ExpondPanels)
                    {
                        expondPanel.ShowCloseIcon = this._isShowCloseIcon;
                    }
                }
            }
        }

        [Description("Gradientcolor background in this expondpanellist"), DefaultValue(false), Category("Appearance")]
        public System.Drawing.Color GradientBackground
        {
            get { return this._colorGradientBackground; }
            set
            {
                if (value != this._colorGradientBackground)
                {
                    this._colorGradientBackground = value;
                    this.Invalidate(false);
                }
            }
        }

        [Description("Gets or sets the height of the ExpondPanels in this ExpondPanelList. "), DefaultValue(25), Category("Appearance")]
        public int CaptionHeight
        {
            get { return _captionHeight; }
            set
            {
                if (value < StaticResource.CaptionMinHeight)
                {
                    throw new InvalidOperationException(
                        string.Format(
                        System.Globalization.CultureInfo.CurrentUICulture,
                        StaticResource.IDS_InvalidOperationExceptionInteger, value, "CaptionHeight", StaticResource.CaptionMinHeight));
                }
                this._captionHeight = value;
                OnCaptionHeightChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        #region MethodsPublic
        public ExpondPanelList()
        {
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.ResizeRedraw, false);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            InitializeComponent();

            this._expondPanels = new ExpondPanelCollection(this);

            this.ShowBorder = true;
            this.PanelStyle = PanelStyle.Default;
            this.LinearGradientMode = LinearGradientMode.Vertical;
            this.CaptionHeight = 25;
        }
       
        public void Expand(BasePanel panel)
        {
            if (panel == null)
            {
                throw new ArgumentNullException("panel",
                    string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    StaticResource.IDS_ArgumentException,
                    "panel"));
            }

            ExpondPanel expondPanel = panel as ExpondPanel;
            if (expondPanel != null)
            {
                foreach (ExpondPanel tmpExpondPanel in this._expondPanels)
                {
                    if (tmpExpondPanel.Equals(expondPanel) == false)
                    {
                        tmpExpondPanel.Expand = false;
                    }
                }
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(expondPanel)["Expand"];
                if (propertyDescriptor != null)
                {
                    propertyDescriptor.SetValue(expondPanel, true);
                }
            }
        }
        #endregion

        #region MethodsProtected
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            if (this._isShowGradientBackground == true)
            {
                Rectangle rectangle = new Rectangle(0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
                using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(
                    rectangle,
                    this.BackColor,
                    this.GradientBackground,
                    this.LinearGradientMode))
                {
                    pevent.Graphics.FillRectangle(linearGradientBrush, rectangle);
                }
            }
        }
      
        protected override void OnControlAdded(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlAdded(e);
            ExpondPanel expondPanel = e.Control as ExpondPanel;
            if (expondPanel != null)
            {
                if (expondPanel.Expand == true)
                {
                    foreach (ExpondPanel tmpExpondPanel in this.ExpondPanels)
                    {
                        if (tmpExpondPanel != expondPanel)
                        {
                            tmpExpondPanel.Expand = false;
                            tmpExpondPanel.Height = expondPanel.CaptionHeight;
                        }
                    }
                }
                expondPanel.Parent = this;
                expondPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
                expondPanel.Left = this.Padding.Left;
                expondPanel.Width = this.ClientRectangle.Width
                    - this.Padding.Left
                    - this.Padding.Right;
                expondPanel.PanelStyle = this.PanelStyle;
                expondPanel.ColorScheme = this.ColorScheme;
                if (this.PanelColors != null)
                {
                    expondPanel.SetPanelProperties(this.PanelColors);
                }
                expondPanel.ShowBorder = this.ShowBorder;
                expondPanel.ShowCloseIcon = this._isShowCloseIcon;
                expondPanel.ShowExpandIcon = this._isShowExpandIcon;
                expondPanel.CaptionStyle = this._captionStyle;
                expondPanel.Top = this.GetTopPosition();
                expondPanel.PanelStyleChanged += new EventHandler<PanelStyleChangeEventArgs>(ExpondPanelPanelStyleChanged);
                expondPanel.ExpandClick += new EventHandler<EventArgs>(this.ExpondPanelExpandClick);
                expondPanel.CloseClick += new EventHandler<EventArgs>(this.ExpondPanelCloseClick);
            }
            else
            {
                throw new InvalidOperationException("Can only add ExpondPanel");
            }
        }
       
        protected override void OnControlRemoved(System.Windows.Forms.ControlEventArgs e)
        {
            base.OnControlRemoved(e);

            ExpondPanel expondPanel =
                e.Control as ExpondPanel;

            if (expondPanel != null)
            {
                expondPanel.PanelStyleChanged -= new EventHandler<PanelStyleChangeEventArgs>(ExpondPanelPanelStyleChanged);
                expondPanel.ExpandClick -= new EventHandler<EventArgs>(this.ExpondPanelExpandClick);
                expondPanel.CloseClick -= new EventHandler<EventArgs>(this.ExpondPanelCloseClick);
            }
        }
        
        protected override void OnResize(System.EventArgs e)
        {
            base.OnResize(e);
            int iExpondPanelCaptionHeight = 0;

            if (this._expondPanels != null)
            {
                foreach (ExpondPanel expondPanel in this._expondPanels)
                {
                    expondPanel.Width = this.ClientRectangle.Width
                        - this.Padding.Left
                        - this.Padding.Right;
                    if (expondPanel.Visible == false)
                    {
                        iExpondPanelCaptionHeight -= expondPanel.CaptionHeight;
                    }
                    iExpondPanelCaptionHeight += expondPanel.CaptionHeight;
                }

                foreach (ExpondPanel expondPanel in this._expondPanels)
                {
                    if (expondPanel.Expand == true)
                    {
                        expondPanel.Height =
                            this.Height
                            - iExpondPanelCaptionHeight
                            - this.Padding.Top
                            - this.Padding.Bottom
                            + expondPanel.CaptionHeight;
                        return;
                    }
                }
            }
        }
       
        protected virtual void OnPanelStyleChanged(object sender, PanelStyleChangeEventArgs e)
        {
            PanelStyle panelStyle = e.PanelStyle;
            this.Padding = new System.Windows.Forms.Padding(0);

            foreach (ExpondPanel expondPanel in this.ExpondPanels)
            {
                PropertyDescriptorCollection propertyDescriptorCollection = TypeDescriptor.GetProperties(expondPanel);
                if (propertyDescriptorCollection.Count > 0)
                {
                    PropertyDescriptor propertyDescriptorPanelStyle = propertyDescriptorCollection["PanelStyle"];
                    if (propertyDescriptorPanelStyle != null)
                    {
                        propertyDescriptorPanelStyle.SetValue(expondPanel, panelStyle);
                    }
                    PropertyDescriptor propertyDescriptorLeft = propertyDescriptorCollection["Left"];
                    if (propertyDescriptorLeft != null)
                    {
                        propertyDescriptorLeft.SetValue(expondPanel, this.Padding.Left);
                    }
                    PropertyDescriptor propertyDescriptorWidth = propertyDescriptorCollection["Width"];
                    if (propertyDescriptorWidth != null)
                    {
                        propertyDescriptorWidth.SetValue(
                            expondPanel,
                            this.ClientRectangle.Width
                            - this.Padding.Left
                            - this.Padding.Right);
                    }

                }
            }
            if (this.PanelStyleChanged != null)
            {
                this.PanelStyleChanged(sender, e);
            }
        }
        
        protected virtual void OnColorSchemeChanged(object sender, ColorSchemeChangeEventArgs e)
        {
            ColorScheme eColorScheme = e.ColorSchema;
            foreach (ExpondPanel expondPanel in this.ExpondPanels)
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(expondPanel)["ColorScheme"];
                if (propertyDescriptor != null)
                {
                    propertyDescriptor.SetValue(expondPanel, eColorScheme);
                }
            }
            if (this.ColorSchemeChanged != null)
            {
                this.ColorSchemeChanged(sender, e);
            }
        }
       
        protected virtual void OnCaptionHeightChanged(object sender, EventArgs e)
        {
            foreach (ExpondPanel expondPanel in this.ExpondPanels)
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(expondPanel)["CaptionHeight"];
                if (propertyDescriptor != null)
                {
                    propertyDescriptor.SetValue(expondPanel, this._captionHeight);
                }
            }
            if (this.CaptionHeightChanged != null)
            {
                this.CaptionHeightChanged(sender, e);
            }
        }
       
        protected virtual void OnCaptionStyleChanged(object sender, EventArgs e)
        {
            foreach (ExpondPanel expondPanel in this.ExpondPanels)
            {
                PropertyDescriptor propertyDescriptor = TypeDescriptor.GetProperties(expondPanel)["CaptionStyle"];
                if (propertyDescriptor != null)
                {
                    propertyDescriptor.SetValue(expondPanel, this._captionStyle);
                }
            }
            if (this.CaptionStyleChanged != null)
            {
                this.CaptionStyleChanged(sender, e);
            }
        }
        #endregion

        #region MethodsPrivate
        private void ExpondPanelExpandClick(object sender, EventArgs e)
        {
            ExpondPanel expondPanel = sender as ExpondPanel;
            if (expondPanel != null)
            {
                this.Expand(expondPanel);
            }
        }

        private void ExpondPanelCloseClick(object sender, EventArgs e)
        {
            ExpondPanel expondPanel = sender as ExpondPanel;
            if (expondPanel != null)
            {
                this.Controls.Remove(expondPanel);
            }
        }

        private void ExpondPanelPanelStyleChanged(object sender, PanelStyleChangeEventArgs e)
        {
            PanelStyle panelStyle = e.PanelStyle;
            if (panelStyle != this._panelStyle)
            {
                this.PanelStyle = panelStyle;
            }
        }

        private int GetTopPosition()
        {
            int iTopPosition = this.Padding.Top;
            int iNextTopPosition = 0;

            IEnumerator enumerator = this.ExpondPanels.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ExpondPanel expondPanel = (ExpondPanel)enumerator.Current;

                if (expondPanel.Visible == true)
                {
                    if (iNextTopPosition == this.Padding.Top)
                    {
                        iTopPosition = this.Padding.Top;
                    }
                    else
                    {
                        iTopPosition = iNextTopPosition;
                    }
                    iNextTopPosition = iTopPosition + expondPanel.Height;
                }
            }
            return iTopPosition;
        }
        #endregion
    }

    #endregion

    #region Class ExpondPanelListDesigner
    internal class ExpondPanelListDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        private Pen m_borderPen = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark));
        private ExpondPanelList m_expondPanelList;

        #region MethodsPublic
        public ExpondPanelListDesigner()
        {
            this.m_borderPen.DashStyle = DashStyle.Dash;
        }
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
            this.m_expondPanelList = (ExpondPanelList)this.Control;
            this.m_expondPanelList.AutoScroll = false;
        }
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                // Create action list collection
                DesignerActionListCollection actionLists = new DesignerActionListCollection();

                // Add custom action list
                actionLists.Add(new ExpondPanelListDesignerActionList(this.Component));

                // Return to the designer action service
                return actionLists;
            }
        }

        #endregion

        #region MethodsProtected
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (this.m_borderPen != null)
                    {
                        this.m_borderPen.Dispose();
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        protected override void OnPaintAdornments(PaintEventArgs e)
        {
            base.OnPaintAdornments(e);
            e.Graphics.DrawRectangle(this.m_borderPen, 0, 0, this.m_expondPanelList.Width - 2, this.m_expondPanelList.Height - 2);
        }

        #endregion
    }
    #endregion

    #region Class ExpondPanelListDesignerActionList
    public class ExpondPanelListDesignerActionList : DesignerActionList
    {
        #region Properties
        [Editor(typeof(ExpondPanelCollectionEditor), typeof(UITypeEditor))]
        public ExpondPanelCollection ExpondPanels
        {
            get { return this.ExpondPanelList.ExpondPanels; }
        }
        public PanelStyle PanelStyle
        {
            get { return this.ExpondPanelList.PanelStyle; }
            set { SetProperty("PanelStyle", value); }
        }
        public ColorScheme ColorScheme
        {
            get { return this.ExpondPanelList.ColorScheme; }
            set { SetProperty("ColorScheme", value); }
        }
        public CaptionStyle CaptionStyle
        {
            get { return this.ExpondPanelList.CaptionStyle; }
            set { SetProperty("CaptionStyle", value); }
        }
        public bool ShowBorder
        {
            get { return this.ExpondPanelList.ShowBorder; }
            set { SetProperty("ShowBorder", value); }
        }
        public bool ShowExpandIcon
        {
            get { return this.ExpondPanelList.ShowExpandIcon; }
            set { SetProperty("ShowExpandIcon", value); }
        }
        public bool ShowCloseIcon
        {
            get { return this.ExpondPanelList.ShowCloseIcon; }
            set { SetProperty("ShowCloseIcon", value); }
        }
        #endregion

        #region MethodsPublic
        public ExpondPanelListDesignerActionList(System.ComponentModel.IComponent component)
            : base(component)
        {
            base.AutoShow = true;
        }
        public override DesignerActionItemCollection GetSortedActionItems()
        {
            // Create list to store designer action items
            DesignerActionItemCollection actionItems = new DesignerActionItemCollection();

            actionItems.Add(
              new DesignerActionMethodItem(
                this,
                "ToggleDockStyle",
                GetDockStyleText(),
                "Design",
                "Dock or undock this control in it's parent container.",
                true));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowBorder",
                "Show Border",
                GetCategory(this.ExpondPanelList, "ShowBorder")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowExpandIcon",
                "Show ExpandIcon",
                GetCategory(this.ExpondPanelList, "ShowExpandIcon")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowCloseIcon",
                "Show CloseIcon",
                GetCategory(this.ExpondPanelList, "ShowCloseIcon")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "PanelStyle",
                "Select PanelStyle",
                GetCategory(this.ExpondPanelList, "PanelStyle")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ColorScheme",
                "Select ColorScheme",
                GetCategory(this.ExpondPanelList, "ColorScheme")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "CaptionStyle",
                "Select CaptionStyle",
                GetCategory(this.ExpondPanelList, "CaptionStyle")));

            actionItems.Add(
              new DesignerActionPropertyItem(
                "ExpondPanels",
                "Edit ExpondPanels",
                GetCategory(this.ExpondPanelList, "ExpondPanels")));

            return actionItems;
        }
        public void ToggleDockStyle()
        {
            // Toggle ClockControl's Dock property
            if (this.ExpondPanelList.Dock != DockStyle.Fill)
            {
                SetProperty("Dock", DockStyle.Fill);
            }
            else
            {
                SetProperty("Dock", DockStyle.None);
            }
        }

        #endregion

        #region MethodsPrivate
        private string GetDockStyleText()
        {
            if (this.ExpondPanelList.Dock == DockStyle.Fill)
            {
                return "Undock in parent container";
            }
            else
            {
                return "Dock in parent container";
            }
        }

        private ExpondPanelList ExpondPanelList
        {
            get { return (ExpondPanelList)this.Component; }
        }

        private void SetProperty(string propertyName, object value)
        {
            System.ComponentModel.PropertyDescriptor property
                = System.ComponentModel.TypeDescriptor.GetProperties(this.ExpondPanelList)[propertyName];
            property.SetValue(this.ExpondPanelList, value);
        }
        private static string GetCategory(object source, string propertyName)
        {
            System.Reflection.PropertyInfo property = source.GetType().GetProperty(propertyName);
            CategoryAttribute attribute = (CategoryAttribute)property.GetCustomAttributes(typeof(CategoryAttribute), false)[0];
            if (attribute == null)
            {
                return null;
            }
            else
            {
                return attribute.Category;
            }
        }

        #endregion
    }

    #endregion

    #region Class ExpondPanelCollection
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1010:CollectionsShouldImplementGenericInterface")]
    public sealed class ExpondPanelCollection : IList, ICollection, IEnumerable
    {
        private ExpondPanelList _expondPanelList;
        private Control.ControlCollection _controlCollection;

        internal ExpondPanelCollection(ExpondPanelList expondPanelList)
        {
            this._expondPanelList = expondPanelList;
            this._controlCollection = this._expondPanelList.Controls;
        }

        #region Properties
        public ExpondPanel this[int index]
        {
            get { return (ExpondPanel)this._controlCollection[index] as ExpondPanel; }
        }
        #endregion

        #region MethodsPublic
        public bool Contains(ExpondPanel expondPanel)
        {
            return this._controlCollection.Contains(expondPanel);
        }
        public void Add(ExpondPanel expondPanel)
        {
            this._controlCollection.Add(expondPanel);
            this._expondPanelList.Invalidate();

        }
        public void Remove(ExpondPanel expondPanel)
        {
            this._controlCollection.Remove(expondPanel);
        }
        public void Clear()
        {
            this._controlCollection.Clear();
        }
        public int Count
        {
            get { return this._controlCollection.Count; }
        }
        public bool IsReadOnly
        {
            get { return this._controlCollection.IsReadOnly; }
        }
        public IEnumerator GetEnumerator()
        {
            return this._controlCollection.GetEnumerator();
        }
        public int IndexOf(ExpondPanel expondPanel)
        {
            return this._controlCollection.IndexOf(expondPanel);
        }
        public void RemoveAt(int index)
        {
            this._controlCollection.RemoveAt(index);
        }
        public void Insert(int index, ExpondPanel expondPanel)
        {
            ((IList)this).Insert(index, (object)expondPanel);
        }
        public void CopyTo(ExpondPanel[] expondPanels, int index)
        {
            this._controlCollection.CopyTo(expondPanels, index);
        }
        #endregion

        #region Interface ICollection
        int ICollection.Count
        {
            get { return this.Count; }
        }
        bool ICollection.IsSynchronized
        {
            get { return ((ICollection)this._controlCollection).IsSynchronized; }
        }
        object ICollection.SyncRoot
        {
            get { return ((ICollection)this._controlCollection).SyncRoot; }
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this._controlCollection).CopyTo(array, index);
        }

        #endregion

        #region Interface IList
        object IList.this[int index]
        {
            get { return this._controlCollection[index]; }
            set { }
        }
        int IList.Add(object value)
        {
            ExpondPanel expondPanel = value as ExpondPanel;
            if (expondPanel == null)
            {
                throw new ArgumentException(string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(ExpondPanel).Name));
            }
            this.Add(expondPanel);
            return this.IndexOf(expondPanel);
        }
        bool IList.Contains(object value)
        {
            return this.Contains(value as ExpondPanel);
        }
        int IList.IndexOf(object value)
        {
            return this.IndexOf(value as ExpondPanel);
        }
        void IList.Insert(int index, object value)
        {
            if ((value is ExpondPanel) == false)
            {
                throw new ArgumentException(
                    string.Format(System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(ExpondPanel).Name));
            }
        }
        void IList.Remove(object value)
        {
            this.Remove(value as ExpondPanel);
        }
        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }
        bool IList.IsReadOnly
        {
            get { return this.IsReadOnly; }
        }
        bool IList.IsFixedSize
        {
            get { return ((IList)this._controlCollection).IsFixedSize; }
        }

        #endregion
    }
    #endregion

    #region Class ExpondPanelCollectionEditor
    internal class ExpondPanelCollectionEditor : CollectionEditor
    {
        private CollectionForm _collectionForm;

        #region MethodsPublic
        public ExpondPanelCollectionEditor(Type type)
            : base(type)
        {
        }
        #endregion

        #region MethodsProtected
        protected override CollectionForm CreateCollectionForm()
        {
            this._collectionForm = base.CreateCollectionForm();
            return this._collectionForm;
        }
        protected override Object CreateInstance(Type ItemType)
        {
            ExpondPanel expondPanel = (ExpondPanel)base.CreateInstance(ItemType);
            if (this.Context.Instance != null)
            {
                expondPanel.Expand = true;
            }
            return expondPanel;
        }
        #endregion
    }
    #endregion
}
