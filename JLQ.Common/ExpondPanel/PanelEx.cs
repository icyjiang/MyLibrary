using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

namespace JLQ.Common
{
    #region Class PanelEx
    [Designer(typeof(PanelDesigner)),
    DesignTimeVisibleAttribute(true)]
    [DefaultEvent("CloseClick")]
    [ToolboxBitmap(typeof(System.Windows.Forms.Panel))]
    public partial class PanelEx : BasePanel
    {
        #region FieldsPrivate
        private Rectangle _restoreBounds;
        private bool _isShowTransparentBackground;
        private bool _isShowExpondPanelProfessionalStyle;
        private bool _isShowCaptionbar;
        private LinearGradientMode _linearGradientMode;
        private Image _imageClosePanel;
        private CustomPanelColors _customColors;
        private Image _imgHoverBackground;
        private Splitter _associatedSplitter;
        #endregion

        #region Properties
        [Description("The associated Splitter."), Category("Behavior")]
        public virtual System.Windows.Forms.Splitter AssociatedSplitter
        {
            get { return this._associatedSplitter; }
            set { this._associatedSplitter = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Description("The custom colors which are used for the panel."), Category("Appearance")]
        public CustomPanelColors CustomColors
        {
            get { return this._customColors; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override bool Expand
        {
            get
            {
                return base.Expand;
            }
            set
            {
                base.Expand = value;
            }
        }

        [Description("LinearGradientMode of the Panels background")]
        [DefaultValue(1), Category("Appearance")]
        public LinearGradientMode LinearGradientMode
        {
            get { return this._linearGradientMode; }
            set
            {
                if (value.Equals(this._linearGradientMode) == false)
                {
                    this._linearGradientMode = value;
                    this.Invalidate(false);
                }
            }
        }

        [Description("A value indicating whether the panels captionbar is displayed.")]
        [DefaultValue(true), Category("Behavior")]
        public bool ShowCaptionbar
        {
            get { return this._isShowCaptionbar; }
            set
            {
                if (value.Equals(this._isShowCaptionbar) == false)
                {
                    this._isShowCaptionbar = value;
                    this.Invalidate(true);
                }
            }
        }

        [Description("Gets or sets a value indicating whether the controls background is transparent")]
        [DefaultValue(true), Category("Behavior")]
        public bool ShowTransparentBackground
        {
            get { return this._isShowTransparentBackground; }
            set
            {
                if (value.Equals(this._isShowTransparentBackground) == false)
                {
                    this._isShowTransparentBackground = value;
                    this.Invalidate(false);
                }
            }
        }

        [Description("Gets or sets a value indicating whether the controls caption professional colorscheme is the same then the ExpondPanels")]
        [DefaultValue(false), Category("Behavior")]
        public bool ShowExpondPanelProfessionalStyle
        {
            get { return this._isShowExpondPanelProfessionalStyle; }
            set
            {
                if (value.Equals(this._isShowExpondPanelProfessionalStyle) == false)
                {
                    this._isShowExpondPanelProfessionalStyle = value;
                    this.Invalidate(false);
                }
            }
        }

        [Browsable(false)]
        public Rectangle RestoreBounds
        {
            get { return this._restoreBounds; }
        }
        #endregion

        #region MethodsPublic
        /// <summary>
        /// Initializes a new instance of the Panel class.
        /// </summary>
        public PanelEx()
        {
            InitializeComponent();

            this.CaptionFont = new Font(SystemFonts.CaptionFont.FontFamily, SystemFonts.CaptionFont.SizeInPoints + 2.75F, FontStyle.Bold);
            this.BackColor = Color.Transparent;
            this.ForeColor = SystemColors.ControlText;
            this.ShowTransparentBackground = true;
            this.ShowExpondPanelProfessionalStyle = false;
            this.ColorScheme = ColorScheme.Professional;
            this.LinearGradientMode = LinearGradientMode.Vertical;
            this.Expand = true;
            this.CaptionHeight = 20;
            this.ImageSize = new Size(18, 18);
            this._isShowCaptionbar = true;
            this._customColors = new CustomPanelColors();
            this._customColors.CustomColorsChanged += OnCustomColorsChanged;
        }
        /// <summary>
        /// Sets the PanelProperties for the Panel
        /// </summary>
        /// <param name="panelColors">The PanelColors table</param>
        public override void SetPanelProperties(PanelColors panelColors)
        {
            this._imgHoverBackground = null;
            base.SetPanelProperties(panelColors);
        }
        /// <summary>
        /// Gets the rectangle that represents the display area of the Panel.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override Rectangle DisplayRectangle
        {
            get
            {
                Padding padding = this.Padding;
                Rectangle displayRectangle = new Rectangle(
                    this.ClientRectangle.Left + padding.Left,
                    this.ClientRectangle.Top + padding.Top,
                    this.ClientRectangle.Width - padding.Left - padding.Right,
                    this.ClientRectangle.Height - padding.Top - padding.Bottom);

                if (this._isShowCaptionbar == true)
                {
                    if (this.Controls.Count > 0)
                    {
                        ExpondPanelList expondPanelList = this.Controls[0] as ExpondPanelList;
                        if ((expondPanelList != null) && (expondPanelList.Dock == DockStyle.Fill))
                        {
                            displayRectangle = new Rectangle(
                                padding.Left,
                                this.CaptionHeight + padding.Top + StaticResource.BorderThickness,
                                this.ClientRectangle.Width - padding.Left - padding.Right,
                                this.ClientRectangle.Height - this.CaptionHeight - padding.Top - padding.Bottom - (2 * StaticResource.BorderThickness));
                        }
                        else
                        {
                            displayRectangle = new Rectangle(
                                padding.Left + StaticResource.BorderThickness,
                                this.CaptionHeight + padding.Top + StaticResource.BorderThickness,
                                this.ClientRectangle.Width - padding.Left - padding.Right - (2 * StaticResource.BorderThickness),
                                this.ClientRectangle.Height - this.CaptionHeight - padding.Top - padding.Bottom - (2 * StaticResource.BorderThickness));
                        }
                    }
                }
                return displayRectangle;
            }
        }
        #endregion

        #region MethodsProtected
        protected override void OnExpandClick(object sender, EventArgs e)
        {
            this.Expand = !this.Expand;
            base.OnExpandClick(sender, e);
        }

        protected override void OnExpandIconHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            Invalidate(this.RectangleExpandIcon);
            base.OnExpandIconHoverStateChanged(sender, e);
        }

        protected override void OnCloseIconHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            Invalidate(this.RectangleCloseIcon);
            base.OnCloseIconHoverStateChanged(sender, e);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            if (this.ShowTransparentBackground == true)
            {
                this.BackColor = Color.Transparent;
            }
            else
            {
                Rectangle rectangleBounds = this.ClientRectangle;
                if (this._isShowCaptionbar == true)
                {
                    this.BackColor = Color.Transparent;
                    rectangleBounds = new Rectangle(
                        this.ClientRectangle.Left,
                        this.ClientRectangle.Top + this.CaptionHeight,
                        this.ClientRectangle.Width,
                        this.ClientRectangle.Height - this.CaptionHeight);
                }
                RenderBackgroundGradient(
                    pevent.Graphics,
                    rectangleBounds,
                    this.PanelColors.PanelContentGradientBegin,
                    this.PanelColors.PanelContentGradientEnd,
                    this.LinearGradientMode);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PanelStyle panelStyle = this.PanelStyle;
            if (this._isShowCaptionbar == false)
            {
                return;
            }

            using (UseAntiAlias antiAlias = new UseAntiAlias(e.Graphics))
            {
                Graphics graphics = e.Graphics;
                using (UseClearTypeGridFit clearTypeGridFit = new UseClearTypeGridFit(graphics))
                {
                    Rectangle captionRectangle = this.CaptionRectangle;
                    Color colorGradientBegin = this.PanelColors.PanelCaptionGradientBegin;
                    Color colorGradientEnd = this.PanelColors.PanelCaptionGradientEnd;
                    Color colorGradientMiddle = this.PanelColors.PanelCaptionGradientMiddle;
                    Color colorText = this.PanelColors.PanelCaptionText;
                    bool bShowExpondPanelProfessionalStyle = this.ShowExpondPanelProfessionalStyle;
                    ColorScheme colorSchema = this.ColorScheme;

                    if ((bShowExpondPanelProfessionalStyle == true)
                        && (colorSchema == ColorScheme.Professional)
                        && (panelStyle != PanelStyle.Office2007))
                    {
                        colorGradientBegin = this.PanelColors.ExpondPanelCaptionGradientBegin;
                        colorGradientEnd = this.PanelColors.ExpondPanelCaptionGradientEnd;
                        colorGradientMiddle = this.PanelColors.ExpondPanelCaptionGradientMiddle;
                        colorText = this.PanelColors.ExpondPanelCaptionText;
                    }

                    Image image = this.Image;
                    RightToLeft rightToLeft = this.RightToLeft;
                    Font captionFont = this.CaptionFont;
                    Rectangle clientRectangle = this.ClientRectangle;
                    string strText = this.Text;
                    DockStyle dockStyle = this.Dock;
                    bool bExpand = this.Expand;
                    if (this._imageClosePanel == null)
                    {
                        this._imageClosePanel = StaticResource.ClosePanel;
                    }
                    Color colorCloseIcon = this.PanelColors.PanelCaptionCloseIcon;
                    if (colorCloseIcon == Color.Empty)
                    {
                        colorCloseIcon = colorText;
                    }
                    bool bShowExpandIcon = this.ShowExpandIcon;
                    bool bShowCloseIcon = this.ShowCloseIcon;

                    //switch (panelStyle)
                    //{
                    //    case PanelStyle.Default:
                    //    case PanelStyle.Office2007:
                    //    default:
                    DrawStyleDefault(graphics,
                        captionRectangle,
                        colorGradientBegin,
                        colorGradientEnd,
                        colorGradientMiddle);
                    //        break;
                    //}

                    DrawBorder(
                        graphics,
                        clientRectangle,
                        captionRectangle,
                        this.PanelColors.BorderColor,
                        this.PanelColors.InnerBorderColor);

                    if ((dockStyle == DockStyle.Fill) || (dockStyle == DockStyle.None) ||
                        ((bShowExpandIcon == false) && (bShowCloseIcon == false)))
                    {
                        DrawImagesAndText(
                            graphics,
                            captionRectangle,
                            CaptionSpacing,
                            this.ImageRectangle,
                            image,
                            rightToLeft,
                            captionFont,
                            colorText,
                            strText);

                        return;
                    }
                    if ((bShowExpandIcon == true) || (bShowCloseIcon == true))
                    {
                        Image imageExpandPanel = GetExpandImage(dockStyle, bExpand);

                        DrawImagesAndText(
                            graphics,
                            dockStyle,
                            CaptionSpacing,
                            captionRectangle,
                            clientRectangle,
                            this.ImageRectangle,
                            image,
                            rightToLeft,
                            bShowCloseIcon,
                            this._imageClosePanel,
                            colorCloseIcon,
                            ref this.RectangleCloseIcon,
                            bShowExpandIcon,
                            bExpand,
                            imageExpandPanel,
                            colorText,
                            ref this.RectangleExpandIcon,
                            captionFont,
                            colorText,
                            this.PanelColors.PanelCollapsedCaptionText,
                            strText);

                        if (this._imgHoverBackground == null)
                        {
                            this._imgHoverBackground = GetPanelIconBackground(
                                graphics,
                                this.ImageRectangle,
                                this.PanelColors.PanelCaptionSelectedGradientBegin,
                                this.PanelColors.PanelCaptionSelectedGradientEnd);
                        }
                        if (this._imgHoverBackground != null)
                        {
                            Rectangle rectangleCloseIcon = this.RectangleCloseIcon;
                            if (rectangleCloseIcon != Rectangle.Empty)
                            {
                                if (this.HoverStateCloseIcon == HoverState.Hover)
                                {
                                    graphics.DrawImage(this._imgHoverBackground, rectangleCloseIcon);
                                    DrawIcon(graphics, this._imageClosePanel, rectangleCloseIcon, colorCloseIcon, rectangleCloseIcon.Y);
                                }
                            }
                            Rectangle rectangleExpandIcon = this.RectangleExpandIcon;
                            if (rectangleExpandIcon != Rectangle.Empty)
                            {
                                if (this.HoverStateExpandIcon == HoverState.Hover)
                                {
                                    graphics.DrawImage(this._imgHoverBackground, rectangleExpandIcon);
                                    DrawIcon(graphics, imageExpandPanel, rectangleExpandIcon, colorText, rectangleExpandIcon.Y);
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnPanelCollapsing(object sender, ExpondStateChangeEventArgs e)
        {
            if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
            {
                foreach (Control control in this.Controls)
                {
                    control.Hide();
                }
            }

            if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
            {
                if (this.ClientRectangle.Width > this.CaptionHeight)
                {
                    this._restoreBounds = this.ClientRectangle;
                }
                this.Width = this.CaptionHeight;
            }

            if ((this.Dock == DockStyle.Top) || (this.Dock == DockStyle.Bottom))
            {
                if (this.ClientRectangle.Height > this.CaptionHeight)
                {
                    this._restoreBounds = this.ClientRectangle;
                }
                this.Height = this.CaptionHeight;
            }

            base.OnPanelCollapsing(sender, e);
        }

        protected override void OnPanelExpanding(object sender, ExpondStateChangeEventArgs e)
        {
            if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
            {
                foreach (Control control in this.Controls)
                {
                    control.Show();
                }
                //When ClientRectangle.Width > CaptionHeight the panel size has changed
                //otherwise the captionclick event was executed
                if (this.ClientRectangle.Width == this.CaptionHeight)
                {
                    this.Width = this._restoreBounds.Width;
                }
            }
            if ((this.Dock == DockStyle.Top) || (this.Dock == DockStyle.Bottom))
            {
                this.Height = this._restoreBounds.Height;
            }

            base.OnPanelExpanding(sender, e);
        }

        protected override void OnPanelStyleChanged(object sender, PanelStyleChangeEventArgs e)
        {
            OnLayout(new LayoutEventArgs(this, null));
            base.OnPanelStyleChanged(sender, e);

        }

        protected override void OnCreateControl()
        {
            this._restoreBounds = this.ClientRectangle;
            this.MinimumSize = new Size(this.CaptionHeight, this.CaptionHeight);
            base.OnCreateControl();
        }

        protected override void OnResize(EventArgs e)
        {
            if (this.ShowExpandIcon == true)
            {
                if (this.Expand == false)
                {
                    if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
                    {
                        if (this.Width > this.CaptionHeight)
                        {
                            this.Expand = true;
                        }
                    }
                    if ((this.Dock == DockStyle.Top) || (this.Dock == DockStyle.Bottom))
                    {
                        if (this.Height > this.CaptionHeight)
                        {
                            this.Expand = true;
                        }
                    }
                }
                else
                {
                    if ((this.Dock == DockStyle.Left) || (this.Dock == DockStyle.Right))
                    {
                        if (this.Width == this.CaptionHeight)
                        {
                            this.Expand = false;
                        }
                    }
                    if ((this.Dock == DockStyle.Top) || (this.Dock == DockStyle.Bottom))
                    {
                        if (this.Height == this.CaptionHeight)
                        {
                            this.Expand = false;
                        }
                    }
                }
            }
            base.OnResize(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            System.Windows.Forms.Splitter associatedSplitter = this.AssociatedSplitter;
            if (associatedSplitter != null)
            {
                associatedSplitter.Visible = this.Visible;
            }
            base.OnVisibleChanged(e);
        }
        #endregion

        #region MethodsPrivate
        private static Image GetPanelIconBackground(Graphics graphics, Rectangle rectanglePanelIcon, Color backgroundColorBegin, Color backgroundColorEnd)
        {
            Rectangle rectangle = rectanglePanelIcon;
            rectangle.X = 0;
            rectangle.Y = 0;
            Image image = new Bitmap(rectanglePanelIcon.Width, rectanglePanelIcon.Height, graphics);
            using (Graphics imageGraphics = Graphics.FromImage(image))
            {
                RenderBackgroundGradient(
                    imageGraphics,
                    rectangle,
                    backgroundColorBegin,
                    backgroundColorEnd,
                    LinearGradientMode.Vertical);
            }
            return image;
        }

        private static void DrawStyleDefault(Graphics graphics,
            Rectangle captionRectangle,
            Color colorGradientBegin,
            Color colorGradientEnd,
            Color colorGradientMiddle)
        {
            RenderDoubleBackgroundGradient(
                graphics,
                captionRectangle,
                colorGradientBegin,
                colorGradientMiddle,
                colorGradientEnd,
                LinearGradientMode.Vertical,
                true);
        }

        private static void DrawBorder(
            Graphics graphics,
            Rectangle panelRectangle,
            Rectangle captionRectangle,
            Color borderColor,
            Color innerBorderColor)
        {
            using (Pen borderPen = new Pen(borderColor))
            {
                // Draws the innerborder around the captionbar
                Rectangle innerBorderRectangle = captionRectangle;
                innerBorderRectangle.Width -= StaticResource.BorderThickness;
                innerBorderRectangle.Offset(StaticResource.BorderThickness, StaticResource.BorderThickness);
                ControlPaint.DrawBorder(
                    graphics,
                    innerBorderRectangle,
                    innerBorderColor,
                    ButtonBorderStyle.Solid);

                // Draws the outer border around the captionbar
                ControlPaint.DrawBorder(
                    graphics,
                    panelRectangle,
                    borderColor,
                    ButtonBorderStyle.Solid);

                // Draws the line below the captionbar
                graphics.DrawLine(
                    borderPen,
                    captionRectangle.X,
                    captionRectangle.Y + captionRectangle.Height,
                    captionRectangle.Width,
                    captionRectangle.Y + captionRectangle.Height);

                if (panelRectangle.Height == captionRectangle.Height)
                {
                    return;
                }

                // Draws the border lines around the whole panel
                Rectangle panelBorderRectangle = panelRectangle;
                panelBorderRectangle.Y = captionRectangle.Height;
                panelBorderRectangle.Height -= captionRectangle.Height + (int)borderPen.Width;
                panelBorderRectangle.Width -= (int)borderPen.Width;
                Point[] points =
                    {
                        new Point(panelBorderRectangle.X, panelBorderRectangle.Y),
                        new Point(panelBorderRectangle.X, panelBorderRectangle.Y + panelBorderRectangle.Height),
                        new Point(panelBorderRectangle.X + panelBorderRectangle.Width ,panelBorderRectangle.Y + panelBorderRectangle.Height),
                        new Point(panelBorderRectangle.X + panelBorderRectangle.Width ,panelBorderRectangle.Y)
                    };
                graphics.DrawLines(borderPen, points);
            }
        }

        private static Image GetExpandImage(DockStyle dockStyle, bool bIsExpanded)
        {
            Image image = null;
            if ((dockStyle == DockStyle.Left) && (bIsExpanded == true))
            {
                image = StaticResource.ChevronLeft;
            }
            else if ((dockStyle == DockStyle.Left) && (bIsExpanded == false))
            {
                image = StaticResource.ChevronRight;
            }
            else if ((dockStyle == DockStyle.Right) && (bIsExpanded == true))
            {
                image = StaticResource.ChevronRight;
            }
            else if ((dockStyle == DockStyle.Right) && (bIsExpanded == false))
            {
                image = StaticResource.ChevronLeft;
            }
            else if ((dockStyle == DockStyle.Top) && (bIsExpanded == true))
            {
                image = StaticResource.ChevronUp;
            }
            else if ((dockStyle == DockStyle.Top) && (bIsExpanded == false))
            {
                image = StaticResource.ChevronDown;
            }
            else if ((dockStyle == DockStyle.Bottom) && (bIsExpanded == true))
            {
                image = StaticResource.ChevronDown;
            }
            else if ((dockStyle == DockStyle.Bottom) && (bIsExpanded == false))
            {
                image = StaticResource.ChevronUp;
            }

            return image;
        }

        #endregion

    }

    #endregion

    #region Class PanelDesigner
    internal class PanelDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        #region MethodsPublic
        public PanelDesigner()
        {
        }
        public override void Initialize(System.ComponentModel.IComponent component)
        {
            base.Initialize(component);
        }
        public override DesignerActionListCollection ActionLists
        {
            get
            {
                // Create action list collection
                DesignerActionListCollection actionLists = new DesignerActionListCollection();

                // Add custom action list
                actionLists.Add(new PanelDesignerActionList(this.Component));

                // Return to the designer action service
                return actionLists;
            }
        }
        #endregion

        #region MethodsProtected
        protected override void OnPaintAdornments(PaintEventArgs e)
        {
            base.OnPaintAdornments(e);
        }

        #endregion
    }

    #endregion

    #region Class ExpondPanelListDesignerActionList
    public class PanelDesignerActionList : DesignerActionList
    {
        #region Properties
        public bool ShowCaptionbar
        {
            get { return this.Panel.ShowCaptionbar; }
            set { SetProperty("ShowCaptionbar", value); }
        }

        public bool ShowTransparentBackground
        {
            get { return this.Panel.ShowTransparentBackground; }
            set { SetProperty("ShowTransparentBackground", value); }
        }

        public bool ShowExpondPanelProfessionalStyle
        {
            get { return this.Panel.ShowExpondPanelProfessionalStyle; }
            set { SetProperty("ShowExpondPanelProfessionalStyle", value); }
        }

        public bool ShowExpandIcon
        {
            get { return this.Panel.ShowExpandIcon; }
            set { SetProperty("ShowExpandIcon", value); }
        }

        public bool ShowCloseIcon
        {
            get { return this.Panel.ShowCloseIcon; }
            set { SetProperty("ShowCloseIcon", value); }
        }

        public PanelStyle PanelStyle
        {
            get { return this.Panel.PanelStyle; }
            set { SetProperty("PanelStyle", value); }
        }

        public ColorScheme ColorScheme
        {
            get { return this.Panel.ColorScheme; }
            set { SetProperty("ColorScheme", value); }
        }
        #endregion

        #region MethodsPublic
        public PanelDesignerActionList(System.ComponentModel.IComponent component)
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
                "ShowTransparentBackground",
                "Show transparent backcolor",
                GetCategory(this.Panel, "ShowTransparentBackground")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowExpondPanelProfessionalStyle",
                "Show the ExpondPanels professional colorscheme",
                GetCategory(this.Panel, "ShowExpondPanelProfessionalStyle")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowCaptionbar",
                "Show the captionbar on top of the panel",
                GetCategory(this.Panel, "ShowCaptionbar")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowExpandIcon",
                "Show the expand panel icon (not at DockStyle.None or DockStyle.Fill)",
                GetCategory(this.Panel, "ShowExpandIcon")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "ShowCloseIcon",
                "Show the close panel icon (not at DockStyle.None or DockStyle.Fill)",
                GetCategory(this.Panel, "ShowCloseIcon")));

            actionItems.Add(
                new DesignerActionPropertyItem(
                "PanelStyle",
                "Select PanelStyle",
                GetCategory(this.Panel, "PanelStyle")));

            actionItems.Add(
               new DesignerActionPropertyItem(
               "ColorScheme",
               "Select ColorScheme",
               GetCategory(this.Panel, "ColorScheme")));

            return actionItems;
        }

        public void ToggleDockStyle()
        {

            // Toggle ClockControl's Dock property
            if (this.Panel.Dock != DockStyle.Fill)
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
            if (this.Panel.Dock == DockStyle.Fill)
            {
                return "Undock in parent container";
            }
            else
            {
                return "Dock in parent container";
            }
        }

        private PanelEx Panel
        {
            get { return (PanelEx)this.Component; }
        }

        private void SetProperty(string propertyName, object value)
        {
            // Get property
            System.ComponentModel.PropertyDescriptor property
                = System.ComponentModel.TypeDescriptor.GetProperties(this.Panel)[propertyName];
            // Set property value
            property.SetValue(this.Panel, value);
        }

        private static string GetCategory(object source, string propertyName)
        {
            System.Reflection.PropertyInfo property = source.GetType().GetProperty(propertyName);
            CategoryAttribute attribute = (CategoryAttribute)property.GetCustomAttributes(typeof(CategoryAttribute), false)[0];
            if (attribute == null) return null;
            return attribute.Category;
        }

        #endregion
    }
    #endregion
}
