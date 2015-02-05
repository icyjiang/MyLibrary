using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.ComponentModel.Design;

namespace JLQ.Common
{
    #region Class ExpondPanel
    [Designer(typeof(ExpondPanelDesigner))]
    [DesignTimeVisible(false)]
    public partial class ExpondPanel : BasePanel
    {
        #region EventsPublic
        [Description("The CaptionStyleChanged event occurs when CaptionStyle flags have been changed.")]
        public event EventHandler<EventArgs> CaptionStyleChanged;
        #endregion

        #region FieldsPrivate
        private System.Drawing.Image _imageChevron;
        private System.Drawing.Image _imageChevronUp;
        private System.Drawing.Image _imageChevronDown;
        private CustomExpondPanelColors _customColors;
        private System.Drawing.Image _imageClosePanel;
        private bool _isIsClosable = true;
        private CaptionStyle _captionStyle;

        #endregion

        #region Properties
        [Description("Gets or sets a value indicating whether the expand icon in a ExpondPanel is visible.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false), Browsable(false), Category("Appearance")]
        public override bool ShowExpandIcon
        {
            get
            {
                return base.ShowExpandIcon;
            }
            set
            {
                base.ShowExpandIcon = value;
            }
        }

        [Description("Gets or sets a value indicating whether the close icon in a ExpondPanel is visible.")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(false), Browsable(false), Category("Appearance")]
        public override bool ShowCloseIcon
        {
            get
            {
                return base.ShowCloseIcon;
            }
            set
            {
                base.ShowCloseIcon = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The custom colors which are used for the ExpondPanel."), Category("Appearance")]
        public CustomExpondPanelColors CustomColors
        {
            get { return this._customColors; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public CaptionStyle CaptionStyle
        {
            get { return this._captionStyle; }
            set
            {
                if (value.Equals(this._captionStyle) == false)
                {
                    this._captionStyle = value;
                    OnCaptionStyleChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("Gets or sets a value indicating whether this ExpondPanel is closable.")]
        [DefaultValue(true), Category("Appearance")]
        public bool IsClosable
        {
            get { return this._isIsClosable; }
            set
            {
                if (value.Equals(this._isIsClosable) == false)
                {
                    this._isIsClosable = value;
                    this.Invalidate(false);
                }
            }
        }

        [Browsable(false)]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }
        #endregion

        #region MethodsPublic
        public ExpondPanel()
        {
            InitializeComponent();

            this.BackColor = Color.Transparent;
            this.CaptionStyle = CaptionStyle.Normal;
            this.ForeColor = SystemColors.ControlText;
            this.Height = this.CaptionHeight;
            this.ShowBorder = true;
            this._customColors = new CustomExpondPanelColors();
            this._customColors.CustomColorsChanged += OnCustomColorsChanged;

        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                Padding padding = this.Padding;

                Rectangle displayRectangle = new Rectangle(
                    padding.Left + StaticResource.BorderThickness,
                    padding.Top + this.CaptionHeight,
                    this.ClientRectangle.Width - padding.Left - padding.Right - (2 * StaticResource.BorderThickness),
                    this.ClientRectangle.Height - this.CaptionHeight - padding.Top - padding.Bottom);

                if (this.Controls.Count > 0)
                {
                    ExpondPanelList expondPanelList = this.Controls[0] as ExpondPanelList;
                    if ((expondPanelList != null) && (expondPanelList.Dock == DockStyle.Fill))
                    {
                        displayRectangle = new Rectangle(
                            padding.Left,
                            padding.Top + this.CaptionHeight,
                            this.ClientRectangle.Width - padding.Left - padding.Right,
                            this.ClientRectangle.Height - this.CaptionHeight - padding.Top - padding.Bottom - StaticResource.BorderThickness);
                    }
                }
                return displayRectangle;
            }
        }
        #endregion

        #region MethodsProtected
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            base.BackColor = Color.Transparent;
            Color backColor = this.PanelColors.ExpondPanelBackColor;
            if ((backColor != Color.Empty) && backColor != Color.Transparent)
            {
                Rectangle rectangle = new Rectangle(
                    0,
                    this.CaptionHeight,
                    this.ClientRectangle.Width,
                    this.ClientRectangle.Height - this.CaptionHeight);

                using (SolidBrush backgroundBrush = new SolidBrush(backColor))
                {
                    pevent.Graphics.FillRectangle(backgroundBrush, rectangle);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsZeroWidthOrHeight(this.CaptionRectangle) == true)
            {
                return;
            }

            using (UseAntiAlias antiAlias = new UseAntiAlias(e.Graphics))
            {
                Graphics graphics = e.Graphics;
                using (UseClearTypeGridFit clearTypeGridFit = new UseClearTypeGridFit(graphics))
                {
                    bool bExpand = this.Expand;
                    bool bShowBorder = this.ShowBorder;
                    Color borderColor = this.PanelColors.BorderColor;
                    Rectangle borderRectangle = this.ClientRectangle;

                    //switch (this.PanelStyle)
                    //{
                    //case PanelStyle.Default:
                    //case PanelStyle.Office2007:
                    //default:
                    DrawCaptionbar(graphics, bExpand, bShowBorder, this.PanelStyle);
                    CalculatePanelHeights();
                    DrawBorders(graphics, this);
                    //        break;
                    //}
                }
            }
        }

        protected override void OnPanelExpanding(object sender, ExpondStateChangeEventArgs e)
        {
            bool bExpand = e.Expand;
            if (bExpand == true)
            {
                this.Expand = bExpand;
                this.Invalidate(false);
            }
            base.OnPanelExpanding(sender, e);
        }

        protected virtual void OnCaptionStyleChanged(object sender, EventArgs e)
        {
            this.Invalidate(this.CaptionRectangle);
            if (this.CaptionStyleChanged != null)
            {
                this.CaptionStyleChanged(sender, e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (this.CaptionRectangle.Contains(e.X, e.Y) == true)
            {
                if ((this.ShowCloseIcon == false) && (this.ShowExpandIcon == false))
                {
                    OnExpandClick(this, EventArgs.Empty);
                }
                else if ((this.ShowCloseIcon == true) && (this.ShowExpandIcon == false))
                {
                    if (this.RectangleCloseIcon.Contains(e.X, e.Y) == false)
                    {
                        OnExpandClick(this, EventArgs.Empty);
                    }
                }
                if (this.ShowExpandIcon == true)
                {
                    if (this.RectangleExpandIcon.Contains(e.X, e.Y) == true)
                    {
                        OnExpandClick(this, EventArgs.Empty);
                    }
                }
                if ((this.ShowCloseIcon == true) && (this._isIsClosable == true))
                {
                    if (this.RectangleCloseIcon.Contains(e.X, e.Y) == true)
                    {
                        OnCloseClick(this, EventArgs.Empty);
                    }
                }
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (this.DesignMode == true)
            {
                return;
            }
            if (this.Visible == false)
            {
                if (this.Expand == true)
                {
                    this.Expand = false;
                    foreach (Control control in this.Parent.Controls)
                    {
                        ExpondPanel expondPanel = control as ExpondPanel;

                        if (expondPanel != null)
                        {
                            if (expondPanel.Visible == true)
                            {
                                expondPanel.Expand = true;
                                return;
                            }
                        }
                    }
                }
            }
#if DEBUG
            //System.Diagnostics.Trace.WriteLine("Visibility: " + this.Name + this.Visible);
#endif
            CalculatePanelHeights();
        }

        #endregion

        #region MethodsPrivate

        private void DrawCaptionbar(Graphics graphics, bool bExpand, bool bShowBorder, PanelStyle panelStyle)
        {
            Rectangle captionRectangle = this.CaptionRectangle;
            Color colorGradientBegin = this.PanelColors.ExpondPanelCaptionGradientBegin;
            Color colorGradientEnd = this.PanelColors.ExpondPanelCaptionGradientEnd;
            Color colorGradientMiddle = this.PanelColors.ExpondPanelCaptionGradientMiddle;
            Color colorText = this.PanelColors.ExpondPanelCaptionText;
            Color foreColorCloseIcon = this.PanelColors.ExpondPanelCaptionCloseIcon;
            Color foreColorExpandIcon = this.PanelColors.ExpondPanelCaptionExpandIcon;
            bool bHover = this.HoverStateCaptionBar == HoverState.Hover ? true : false;

            if (this._imageClosePanel == null)
            {
                this._imageClosePanel = StaticResource.ClosePanel;
            }
            if (this._imageChevronUp == null)
            {
                this._imageChevronUp = StaticResource.ChevronUp;
            }
            if (this._imageChevronDown == null)
            {
                this._imageChevronDown = StaticResource.ChevronDown;
            }

            this._imageChevron = this._imageChevronDown;
            if (bExpand == true)
            {
                this._imageChevron = this._imageChevronUp;
            }

            if (this._captionStyle == CaptionStyle.Normal)
            {
                if (bHover == true)
                {
                    colorGradientBegin = this.PanelColors.ExpondPanelSelectedCaptionBegin;
                    colorGradientEnd = this.PanelColors.ExpondPanelSelectedCaptionEnd;
                    colorGradientMiddle = this.PanelColors.ExpondPanelSelectedCaptionMiddle;
                    if (bExpand == true)
                    {
                        colorGradientBegin = this.PanelColors.ExpondPanelPressedCaptionBegin;
                        colorGradientEnd = this.PanelColors.ExpondPanelPressedCaptionEnd;
                        colorGradientMiddle = this.PanelColors.ExpondPanelPressedCaptionMiddle;
                    }
                    colorText = this.PanelColors.ExpondPanelSelectedCaptionText;
                    foreColorCloseIcon = colorText;
                    foreColorExpandIcon = colorText;
                }
                else
                {
                    if (bExpand == true)
                    {
                        colorGradientBegin = this.PanelColors.ExpondPanelCheckedCaptionBegin;
                        colorGradientEnd = this.PanelColors.ExpondPanelCheckedCaptionEnd;
                        colorGradientMiddle = this.PanelColors.ExpondPanelCheckedCaptionMiddle;
                        colorText = this.PanelColors.ExpondPanelSelectedCaptionText;
                        foreColorCloseIcon = colorText;
                        foreColorExpandIcon = colorText;
                    }
                }
                if (panelStyle != PanelStyle.Office2007)
                {
                    RenderDoubleBackgroundGradient(
                    graphics,
                    captionRectangle,
                    colorGradientBegin,
                    colorGradientMiddle,
                    colorGradientEnd,
                    LinearGradientMode.Vertical,
                    false);
                }
                else
                {
                    RenderButtonBackground(
                        graphics,
                        captionRectangle,
                        colorGradientBegin,
                        colorGradientMiddle,
                        colorGradientEnd);
                }
            }
            else
            {
                Color colorFlatGradientBegin = this.PanelColors.ExpondPanelFlatCaptionGradientBegin;
                Color colorFlatGradientEnd = this.PanelColors.ExpondPanelFlatCaptionGradientEnd;
                Color colorInnerBorder = this.PanelColors.InnerBorderColor;
                colorText = this.PanelColors.ExpondPanelCaptionText;
                foreColorExpandIcon = colorText;

                RenderFlatButtonBackground(graphics, captionRectangle, colorFlatGradientBegin, colorFlatGradientEnd, bHover);
                DrawInnerBorders(graphics, this);
            }

            DrawImagesAndText(
                graphics,
                captionRectangle,
                CaptionSpacing,
                this.ImageRectangle,
                this.Image,
                this.RightToLeft,
                this._isIsClosable,
                this.ShowCloseIcon,
                this._imageClosePanel,
                foreColorCloseIcon,
                ref this.RectangleCloseIcon,
                this.ShowExpandIcon,
                this._imageChevron,
                foreColorExpandIcon,
                ref this.RectangleExpandIcon,
                this.CaptionFont,
                colorText,
                this.Text);
        }

        private static void DrawBorders(Graphics graphics, ExpondPanel expondPanel)
        {
            if (expondPanel.ShowBorder == true)
            {
                using (GraphicsPath graphicsPath = new GraphicsPath())
                {
                    using (Pen borderPen = new Pen(expondPanel.PanelColors.BorderColor, StaticResource.BorderThickness))
                    {
                        Rectangle captionRectangle = expondPanel.CaptionRectangle;
                        Rectangle borderRectangle = captionRectangle;

                        if (expondPanel.Expand == true)
                        {
                            borderRectangle = expondPanel.ClientRectangle;

                            graphics.DrawLine(
                                borderPen,
                                captionRectangle.Left,
                                captionRectangle.Top + captionRectangle.Height - StaticResource.BorderThickness,
                                captionRectangle.Left + captionRectangle.Width,
                                captionRectangle.Top + captionRectangle.Height - StaticResource.BorderThickness);
                        }

                        ExpondPanelList expondPanelList = expondPanel.Parent as ExpondPanelList;
                        if ((expondPanelList != null) && (expondPanelList.Dock == DockStyle.Fill))
                        {
                            PanelEx panel = expondPanelList.Parent as PanelEx;
                            ExpondPanel parentExpondPanel = expondPanelList.Parent as ExpondPanel;
                            if (((panel != null) && (panel.Padding == new Padding(0))) ||
                                ((parentExpondPanel != null) && (parentExpondPanel.Padding == new Padding(0))))
                            {
                                if (expondPanel.Top != 0)
                                {
                                    graphicsPath.AddLine(
                                        borderRectangle.Left,
                                        borderRectangle.Top,
                                        borderRectangle.Left + captionRectangle.Width,
                                        borderRectangle.Top);
                                }

                                // Left vertical borderline
                                graphics.DrawLine(borderPen,
                                    borderRectangle.Left,
                                    borderRectangle.Top,
                                    borderRectangle.Left,
                                    borderRectangle.Top + borderRectangle.Height);

                                // Right vertical borderline
                                graphics.DrawLine(borderPen,
                                    borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                    borderRectangle.Top,
                                    borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                    borderRectangle.Top + borderRectangle.Height);
                            }
                            else
                            {
                                // Upper horizontal borderline only at the top expondPanel
                                if (expondPanel.Top == 0)
                                {
                                    graphicsPath.AddLine(
                                        borderRectangle.Left,
                                        borderRectangle.Top,
                                        borderRectangle.Left + borderRectangle.Width,
                                        borderRectangle.Top);
                                }

                                // Left vertical borderline
                                graphicsPath.AddLine(
                                    borderRectangle.Left,
                                    borderRectangle.Top,
                                    borderRectangle.Left,
                                    borderRectangle.Top + borderRectangle.Height);

                                //Lower horizontal borderline
                                graphicsPath.AddLine(
                                    borderRectangle.Left,
                                    borderRectangle.Top + borderRectangle.Height - StaticResource.BorderThickness,
                                    borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                    borderRectangle.Top + borderRectangle.Height - StaticResource.BorderThickness);

                                // Right vertical borderline
                                graphicsPath.AddLine(
                                    borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                    borderRectangle.Top,
                                    borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                    borderRectangle.Top + borderRectangle.Height);
                            }
                        }
                        else
                        {
                            // Upper horizontal borderline only at the top expondPanel
                            if (expondPanel.Top == 0)
                            {
                                graphicsPath.AddLine(
                                    borderRectangle.Left,
                                    borderRectangle.Top,
                                    borderRectangle.Left + borderRectangle.Width,
                                    borderRectangle.Top);
                            }

                            // Left vertical borderline
                            graphicsPath.AddLine(
                                borderRectangle.Left,
                                borderRectangle.Top,
                                borderRectangle.Left,
                                borderRectangle.Top + borderRectangle.Height);

                            //Lower horizontal borderline
                            graphicsPath.AddLine(
                                borderRectangle.Left,
                                borderRectangle.Top + borderRectangle.Height - StaticResource.BorderThickness,
                                borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                borderRectangle.Top + borderRectangle.Height - StaticResource.BorderThickness);

                            // Right vertical borderline
                            graphicsPath.AddLine(
                                borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                borderRectangle.Top,
                                borderRectangle.Left + borderRectangle.Width - StaticResource.BorderThickness,
                                borderRectangle.Top + borderRectangle.Height);
                        }
                    }
                    using (Pen borderPen = new Pen(expondPanel.PanelColors.BorderColor, StaticResource.BorderThickness))
                    {
                        graphics.DrawPath(borderPen, graphicsPath);
                    }
                }
            }
        }


        private static void DrawInnerBorders(Graphics graphics, ExpondPanel expondPanel)
        {
            if (expondPanel.ShowBorder == true)
            {
                using (GraphicsPath graphicsPath = new GraphicsPath())
                {
                    Rectangle captionRectangle = expondPanel.CaptionRectangle;
                    ExpondPanelList expondPanelList = expondPanel.Parent as ExpondPanelList;
                    if ((expondPanelList != null) && (expondPanelList.Dock == DockStyle.Fill))
                    {
                        PanelEx panel = expondPanelList.Parent as PanelEx;
                        ExpondPanel parentExpondPanel = expondPanelList.Parent as ExpondPanel;
                        if (((panel != null) && (panel.Padding == new Padding(0))) ||
                            ((parentExpondPanel != null) && (parentExpondPanel.Padding == new Padding(0))))
                        {
                            //Left vertical borderline
                            graphicsPath.AddLine(captionRectangle.X, captionRectangle.Y + captionRectangle.Height, captionRectangle.X, captionRectangle.Y + StaticResource.BorderThickness);
                            if (expondPanel.Top == 0)
                            {
                                //Upper horizontal borderline
                                graphicsPath.AddLine(captionRectangle.X, captionRectangle.Y, captionRectangle.X + captionRectangle.Width, captionRectangle.Y);
                            }
                            else
                            {
                                //Upper horizontal borderline
                                graphicsPath.AddLine(captionRectangle.X, captionRectangle.Y + StaticResource.BorderThickness, captionRectangle.X + captionRectangle.Width, captionRectangle.Y + StaticResource.BorderThickness);
                            }
                        }
                    }
                    else
                    {
                        //Left vertical borderline
                        graphicsPath.AddLine(captionRectangle.X + StaticResource.BorderThickness, captionRectangle.Y + captionRectangle.Height, captionRectangle.X + StaticResource.BorderThickness, captionRectangle.Y);
                        if (expondPanel.Top == 0)
                        {
                            //Upper horizontal borderline
                            graphicsPath.AddLine(captionRectangle.X + StaticResource.BorderThickness, captionRectangle.Y + StaticResource.BorderThickness, captionRectangle.X + captionRectangle.Width - StaticResource.BorderThickness, captionRectangle.Y + StaticResource.BorderThickness);
                        }
                        else
                        {
                            //Upper horizontal borderline
                            graphicsPath.AddLine(captionRectangle.X + StaticResource.BorderThickness, captionRectangle.Y, captionRectangle.X + captionRectangle.Width - StaticResource.BorderThickness, captionRectangle.Y);
                        }
                    }

                    using (Pen borderPen = new Pen(expondPanel.PanelColors.InnerBorderColor))
                    {
                        graphics.DrawPath(borderPen, graphicsPath);
                    }
                }
            }
        }

        private void CalculatePanelHeights()
        {
            if (this.Parent == null)
            {
                return;
            }

            int iPanelHeight = this.Parent.Padding.Top;

            foreach (Control control in this.Parent.Controls)
            {
                ExpondPanel expondPanel = control as ExpondPanel;

                if ((expondPanel != null) && (expondPanel.Visible == true))
                {
                    iPanelHeight += expondPanel.CaptionHeight;
                }
            }

            iPanelHeight += this.Parent.Padding.Bottom;

            foreach (Control control in this.Parent.Controls)
            {
                ExpondPanel expondPanel = control as ExpondPanel;

                if (expondPanel != null)
                {
                    if (expondPanel.Expand == true)
                    {
                        expondPanel.Height = this.Parent.Height
                            + expondPanel.CaptionHeight
                            - iPanelHeight;
                    }
                    else
                    {
                        expondPanel.Height = expondPanel.CaptionHeight;
                    }
                }
            }

            int iTop = this.Parent.Padding.Top;
            foreach (Control control in this.Parent.Controls)
            {
                ExpondPanel expondPanel = control as ExpondPanel;

                if ((expondPanel != null) && (expondPanel.Visible == true))
                {
                    expondPanel.Top = iTop;
                    iTop += expondPanel.Height;
                }
            }
        }

        #endregion
    }
    #endregion

    #region Class ExpondPanelDesigner
    internal class ExpondPanelDesigner : System.Windows.Forms.Design.ScrollableControlDesigner
    {
        #region FieldsPrivate

        private Pen m_borderPen = new Pen(Color.FromKnownColor(KnownColor.ControlDarkDark));
        private System.Windows.Forms.Design.Behavior.Adorner m_adorner;

        #endregion

        #region MethodsPublic
        public ExpondPanelDesigner()
        {
            this.m_borderPen.DashStyle = DashStyle.Dash;
        }
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            ExpondPanel expondPanel = Control as ExpondPanel;
            if (expondPanel != null)
            {
                this.m_adorner = new System.Windows.Forms.Design.Behavior.Adorner();
                BehaviorService.Adorners.Add(this.m_adorner);
                this.m_adorner.Glyphs.Add(new ExpondPanelCaptionGlyph(BehaviorService, expondPanel));
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
                    if (this.m_adorner != null)
                    {
                        if (BehaviorService != null)
                        {
                            BehaviorService.Adorners.Remove(this.m_adorner);
                        }
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
            e.Graphics.DrawRectangle(
                this.m_borderPen,
                0,
                0,
                this.Control.Width - 2,
                this.Control.Height - 2);
        }
        protected override void PostFilterProperties(IDictionary properties)
        {
            base.PostFilterProperties(properties);
            properties.Remove("AccessibilityObject");
            properties.Remove("AccessibleDefaultActionDescription");
            properties.Remove("AccessibleDescription");
            properties.Remove("AccessibleName");
            properties.Remove("AccessibleRole");
            properties.Remove("AllowDrop");
            properties.Remove("Anchor");
            properties.Remove("AntiAliasing");
            properties.Remove("AutoScroll");
            properties.Remove("AutoScrollMargin");
            properties.Remove("AutoScrollMinSize");
            properties.Remove("BackColor");
            properties.Remove("BackgroundImage");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("CausesValidation");
            properties.Remove("ContextMenuStrip");
            properties.Remove("Dock");
            properties.Remove("GenerateMember");
            properties.Remove("ImeMode");
            properties.Remove("Location");
            properties.Remove("MaximumSize");
            properties.Remove("MinimumSize");
        }

        #endregion
    }
    #endregion

    #region Class ExpondPanelCaptionGlyph
    internal class ExpondPanelCaptionGlyph : System.Windows.Forms.Design.Behavior.Glyph
    {
        #region FieldsPrivate

        private ExpondPanel m_expondPanel;
        private System.Windows.Forms.Design.Behavior.BehaviorService m_behaviorService;

        #endregion

        #region Properties
        public override Rectangle Bounds
        {
            get
            {
                Point edge = this.m_behaviorService.ControlToAdornerWindow(this.m_expondPanel);
                Rectangle bounds = new Rectangle(
                    edge.X,
                    edge.Y,
                    this.m_expondPanel.Width,
                    this.m_expondPanel.CaptionHeight);

                return bounds;
            }
        }
        #endregion

        #region MethodsPublic
        public ExpondPanelCaptionGlyph(System.Windows.Forms.Design.Behavior.BehaviorService behaviorService, ExpondPanel expondPanel)
            :
            base(new ExpondPanelCaptionClickBehavior(expondPanel))
        {
            this.m_behaviorService = behaviorService;
            this.m_expondPanel = expondPanel;
        }
        public override Cursor GetHitTest(Point p)
        {
            if ((this.m_expondPanel != null) && (this.m_expondPanel.Expand == false) && (Bounds.Contains(p)))
            {
                return Cursors.Hand;
            }

            return null;
        }
        public override void Paint(PaintEventArgs pe)
        {
        }

        #endregion
    }

    #endregion

    #region Class ExpondPanelCaptionClickBehavior
    internal class ExpondPanelCaptionClickBehavior : System.Windows.Forms.Design.Behavior.Behavior
    {
        #region FieldsPrivate
        private ExpondPanel m_expondPanel;
        #endregion

        #region MethodsPublic
        public ExpondPanelCaptionClickBehavior(ExpondPanel expondPanel)
        {
            this.m_expondPanel = expondPanel;
        }
        public override bool OnMouseDown(System.Windows.Forms.Design.Behavior.Glyph g, MouseButtons button, Point mouseLoc)
        {
            if ((this.m_expondPanel != null) && (this.m_expondPanel.Expand == false))
            {
                ExpondPanelList expondPanelList = this.m_expondPanel.Parent as ExpondPanelList;
                if (expondPanelList != null)
                {
                    expondPanelList.Expand(this.m_expondPanel);
                    this.m_expondPanel.Invalidate(false);
                }
            }
            return true; // indicating we processed this event.
        }
        #endregion
    }
    #endregion
}
