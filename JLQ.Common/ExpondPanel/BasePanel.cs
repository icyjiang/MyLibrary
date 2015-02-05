using System;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace JLQ.Common
{
    [DesignTimeVisibleAttribute(false)]
    public class BasePanel : ScrollableControl, IPanel
    {
        #region StaticResource
        public const int CaptionSpacing = 6;
        #endregion

        #region Events
        [Description("Occurs when the close icon in the caption of the panel or expondpanel is clicked.")]
        public event EventHandler<EventArgs> CloseClick;
        [Description("Occurs when the expand icon in the caption of the panel or expondpanel is clicked.")]
        public event EventHandler<EventArgs> ExpandClick;
        [Description("Occurs when the panel or expondpanel expands.")]
        public event EventHandler<ExpondStateChangeEventArgs> PanelExpanding;
        [Description("Occurs when the panel or expondpanel collapse.")]
        public event EventHandler<ExpondStateChangeEventArgs> PanelCollapsing;
        [Description("The PanelStyleChanged event occurs when PanelStyle flags have been changed.")]
        public event EventHandler<PanelStyleChangeEventArgs> PanelStyleChanged;
        [Description("The ColorSchemeChanged event occurs when ColorScheme flags have been changed.")]
        public event EventHandler<ColorSchemeChangeEventArgs> ColorSchemeChanged;
        [Description("Occurs when the value of the CustomColors property changes.")]
        public event EventHandler<EventArgs> CustomColorsChanged;
        [Description("Occurs when the value of the CaptionHeight property changes.")]
        public event EventHandler<EventArgs> CaptionHeightChanged;
        [Description("Occurs when the value of the CaptionBar HoverState changes.")]
        public event EventHandler<HoverStateChangeEventArgs> CaptionBarHoverStateChanged;
        [Description("Occurs when the value of the CloseIcon HoverState changes.")]
        protected event EventHandler<HoverStateChangeEventArgs> CloseIconHoverStateChanged;
        [Description("Occurs when the value of the ExpandIcon HoverState changes.")]
        protected event EventHandler<HoverStateChangeEventArgs> ExpandIconHoverStateChanged;
        #endregion

        #region FieldsPrivate

        private int _captionHeight;
        private Font _captionFont;
        private Rectangle _imageRectangle;
        private bool _isShowBorder;
        private bool _isExpand;
        private Size _imageSize;
        private ColorScheme _colorScheme;
        private PanelColors _panelColors;
        private PanelStyle _panelStyle;
        private Image _image;
        private HoverState _hoverStateCaptionBar;
        private HoverState _hoverStateExpandIcon;
        private string _strToolTipTextExpandIconPanelExpanded;
        private string _strToolTipTextExpandIconPanelCollapsed;
        private HoverState _hoverStateCloseIcon;
        private string _strToolTipTextCloseIcon;
        private bool _isShowExpandIcon;
        private bool _isShowCloseIcon;
        private System.Windows.Forms.ToolTip _toolTip;

        #endregion

        #region FieldsProtected
        protected Rectangle RectangleExpandIcon = Rectangle.Empty;
        protected Rectangle RectangleCloseIcon = Rectangle.Empty;
        #endregion

        #region Properties
        [Description("Style of the panel"), DefaultValue(PanelStyle.Default), Category("Appearance")]
        public virtual PanelStyle PanelStyle
        {
            get { return this._panelStyle; }
            set
            {
                if (value.Equals(this._panelStyle) == false)
                {
                    this._panelStyle = value;
                    OnPanelStyleChanged(this, new PanelStyleChangeEventArgs(this._panelStyle));
                }
            }
        }

        [Description("Gets or sets the image that is displayed on a Panels caption.")]
        [Category("Appearance")]
        public Image Image
        {
            get { return this._image; }
            set
            {
                if (value != this._image)
                {
                    this._image = value;
                    this.Invalidate(this.CaptionRectangle);
                }
            }
        }

        [Description("ColorScheme of the Panel")]
        [DefaultValue(ColorScheme.Professional)]
        [Browsable(true), Category("Appearance")]
        public virtual ColorScheme ColorScheme
        {
            get { return this._colorScheme; }
            set
            {
                if (value.Equals(this._colorScheme) == false)
                {
                    this._colorScheme = value;
                    OnColorSchemeChanged(this, new ColorSchemeChangeEventArgs(this._colorScheme));
                }
            }
        }

        [Description("Gets or sets the height of the panels caption.")]
        [DefaultValue(20), Category("Appearance")]
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

        [Description("Gets or sets the font of the text displayed on the caption.")]
        [DefaultValue(typeof(Font), "Microsoft Sans Serif; 8,25pt; style=Bold")]
        [Category("Appearance")]
        public Font CaptionFont
        {
            get { return this._captionFont; }
            set
            {
                if (value != null)
                {
                    if (value.Equals(this._captionFont) == false)
                    {
                        this._captionFont = value;
                        this.Invalidate(this.CaptionRectangle);
                    }
                }
            }
        }

        [Description("Expand the panel or expondpanel")]
        [DefaultValue(false), Category("Appearance"), RefreshProperties(RefreshProperties.Repaint)]
        public virtual bool Expand
        {
            get { return this._isExpand; }
            set
            {
                if (value.Equals(this._isExpand) == false)
                {
                    this._isExpand = value;
                    if (this._isExpand == true)
                    {
                        OnPanelExpanding(this, new ExpondStateChangeEventArgs(this._isExpand));
                    }
                    else
                    {
                        OnPanelCollapsing(this, new ExpondStateChangeEventArgs(this._isExpand));
                    }
                }
            }
        }

        [Description("Gets or sets a value indicating whether the control shows a border")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(true), Browsable(false), Category("Appearance")]
        public virtual bool ShowBorder
        {
            get { return this._isShowBorder; }
            set
            {
                if (value.Equals(this._isShowBorder) == false)
                {
                    this._isShowBorder = value;
                    this.Invalidate(false);
                }
            }
        }

        [Description("Gets or sets a value indicating whether the expand icon in a Panel or ExpondPanel is visible.")]
        [DefaultValue(false), Category("Appearance")]
        public virtual bool ShowExpandIcon
        {
            get { return this._isShowExpandIcon; }
            set
            {
                if (value.Equals(this._isShowExpandIcon) == false)
                {
                    this._isShowExpandIcon = value;
                    this.Invalidate(false);
                }
            }
        }

        [Description("Specifies the text to show on a ToolTip when the mouse moves over the closeicon.")]
        [Category("Behavior")]
        public virtual string ToolTipTextCloseIcon
        {
            get { return this._strToolTipTextCloseIcon; }
            set { this._strToolTipTextCloseIcon = value; }
        }

        [Description("Specifies the text to show on a ToolTip when the mouse moves over the expandicon and the panel is collapsed.")]
        [Category("Behavior")]
        public virtual string ToolTipTextExpandIconPanelCollapsed
        {
            get { return this._strToolTipTextExpandIconPanelCollapsed; }
            set { this._strToolTipTextExpandIconPanelCollapsed = value; }
        }

        [Description("Specifies the text to show on a ToolTip when the mouse moves over the expandicon and the panel is expanded.")]
        [Category("Behavior")]
        public virtual string ToolTipTextExpandIconPanelExpanded
        {
            get { return this._strToolTipTextExpandIconPanelExpanded; }
            set { this._strToolTipTextExpandIconPanelExpanded = value; }
        }

        [Description("Gets or sets a value indicating whether the close icon in a Panel or ExpondPanel is visible.")]
        [DefaultValue(false), Category("Appearance")]
        public virtual bool ShowCloseIcon
        {
            get { return this._isShowCloseIcon; }
            set
            {
                if (value.Equals(this._isShowCloseIcon) == false)
                {
                    this._isShowCloseIcon = value;
                    this.Invalidate(false);
                }
            }
        }

        protected PanelColors PanelColors
        {
            get { return _panelColors; }
        }

        internal HoverState HoverStateCaptionBar
        {
            get { return this._hoverStateCaptionBar; }
            set { this._hoverStateCaptionBar = value; }
        }

        internal HoverState HoverStateCloseIcon
        {
            get { return this._hoverStateCloseIcon; }
            set { this._hoverStateCloseIcon = value; }
        }

        internal HoverState HoverStateExpandIcon
        {
            get { return this._hoverStateExpandIcon; }
            set { this._hoverStateExpandIcon = value; }
        }

        internal Size ImageSize
        {
            get { return this._imageSize; }
            set { this._imageSize = value; }
        }

        internal Rectangle CaptionRectangle
        {
            get { return new Rectangle(0, 0, this.ClientRectangle.Width, this.CaptionHeight); }
        }

        internal Rectangle ImageRectangle
        {
            get
            {
                if (this._imageRectangle == Rectangle.Empty)
                {
                    this._imageRectangle = new Rectangle(
                        CaptionSpacing,
                        this.CaptionHeight,
                        this._imageSize.Width,
                        this._imageSize.Height);
                }
                return this._imageRectangle;
            }
        }
        #endregion

        #region MethodsPublic
        public virtual void SetPanelProperties(PanelColors panelColors)
        {
            if (panelColors == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    "panelColors"));
            }
            this._panelColors = panelColors;
            this.ColorScheme = ColorScheme.Professional;
            this.Invalidate(true);
        }
        #endregion

        #region MethodsProtected
        protected BasePanel()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.ContainerControl, true);
            this.CaptionFont = new Font(SystemFonts.CaptionFont.FontFamily, SystemFonts.CaptionFont.SizeInPoints - 1.0F, FontStyle.Bold);
            this.CaptionHeight = 25;
            this.PanelStyle = PanelStyle.Default;
            this._panelColors = new PanelColors(this);
            this._imageSize = new Size(16, 16);
            this._imageRectangle = Rectangle.Empty;
            this._toolTip = new System.Windows.Forms.ToolTip();

        }
        
        protected override void OnTextChanged(EventArgs e)
        {
            this.Invalidate(this.CaptionRectangle);
            base.OnTextChanged(e);
        }
       
        protected virtual void OnColorSchemeChanged(object sender, ColorSchemeChangeEventArgs e)
        {
            this.PanelColors.Clear();
            this.Invalidate(false);
            if (this.ColorSchemeChanged != null)
            {
                this.ColorSchemeChanged(sender, e);
            }
        }
       
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if ((this.ShowExpandIcon == true) && (this.RectangleExpandIcon.Contains(e.X, e.Y) == true))
            {
                OnExpandClick(this, EventArgs.Empty);
            }
            if ((this.ShowCloseIcon == true) && (this.RectangleCloseIcon.Contains(e.X, e.Y) == true))
            {
                OnCloseClick(this, EventArgs.Empty);
            }
            base.OnMouseUp(e);
        }
       
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.CaptionRectangle.Contains(e.X, e.Y) == true)
            {
                if (this._hoverStateCaptionBar == HoverState.None)
                {
                    this._hoverStateCaptionBar = HoverState.Hover;
                    OnCaptionBarHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateCaptionBar));
                }
            }
            else
            {
                if (this._hoverStateCaptionBar == HoverState.Hover)
                {
                    this._hoverStateCaptionBar = HoverState.None;
                    OnCaptionBarHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateCaptionBar));
                }
            }

            if ((this.ShowExpandIcon == true) || (this.ShowCloseIcon == true))
            {
                if (this.RectangleExpandIcon.Contains(e.X, e.Y) == true)
                {
                    if (this._hoverStateExpandIcon == HoverState.None)
                    {
                        this._hoverStateExpandIcon = HoverState.Hover;
                        OnExpandIconHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateExpandIcon));
                    }
                }
                else
                {
                    if (this._hoverStateExpandIcon == HoverState.Hover)
                    {
                        this._hoverStateExpandIcon = HoverState.None;
                        OnExpandIconHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateExpandIcon));
                    }
                }
                if (this.RectangleCloseIcon.Contains(e.X, e.Y) == true)
                {
                    if (this._hoverStateCloseIcon == HoverState.None)
                    {
                        this._hoverStateCloseIcon = HoverState.Hover;
                        OnCloseIconHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateCloseIcon));
                    }
                }
                else
                {
                    if (this._hoverStateCloseIcon == HoverState.Hover)
                    {
                        this._hoverStateCloseIcon = HoverState.None;
                        OnCloseIconHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateCloseIcon));
                    }
                }
            }
            base.OnMouseMove(e);
        }
       
        protected override void OnMouseLeave(EventArgs e)
        {
            if (this._hoverStateCaptionBar == HoverState.Hover)
            {
                this._hoverStateCaptionBar = HoverState.None;
                OnCaptionBarHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateCaptionBar));
            }
            if (this._hoverStateExpandIcon == HoverState.Hover)
            {
                this._hoverStateExpandIcon = HoverState.None;
                OnExpandIconHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateExpandIcon));
            }
            if (this._hoverStateCloseIcon == HoverState.Hover)
            {
                this._hoverStateCloseIcon = HoverState.None;
                OnCloseIconHoverStateChanged(this, new HoverStateChangeEventArgs(this._hoverStateCloseIcon));
            }
            base.OnMouseLeave(e);
        }
       
        protected virtual void OnPanelExpanding(object sender, ExpondStateChangeEventArgs e)
        {
            if (this.PanelExpanding != null)
            {
                this.PanelExpanding(sender, e);
            }
        }
       
        protected virtual void OnPanelCollapsing(object sender, ExpondStateChangeEventArgs e)
        {
            if (this.PanelCollapsing != null)
            {
                this.PanelCollapsing(sender, e);
            }
        }
        
        protected virtual void OnPanelStyleChanged(object sender, PanelStyleChangeEventArgs e)
        {
            PanelStyle panelStyle = e.PanelStyle;
            switch (panelStyle)
            {
                default:
                case PanelStyle.Default:
                    _panelColors = new PanelColors(this);
                    break;
                case PanelStyle.Office2007:
                    _panelColors = new PanelColorsOffice2007Blue(this);
                    break;
                case PanelStyle.Black:
                    _panelColors = new PanelColorsBlack(this);
                    break;
                case PanelStyle.Blue:
                    _panelColors = new PanelColorsBlue(this);
                    break;
                case PanelStyle.Office2007Black:
                    _panelColors = new PanelColorsOffice2007Black(this);
                    break;
                case PanelStyle.Office2007Silver:
                    _panelColors = new PanelColorsOffice2007Silver(this);
                    break;
            }

            //switch (panelStyle)
            //{
            //    case PanelStyle.Default:
            //        _panelColors = new PanelColors(this);
            //        break;
            //    case PanelStyle.Office2007:
            //        _panelColors = new PanelColorsOffice2007Blue(this);
            //        break;
            //}
            Invalidate(true);
            if (this.PanelStyleChanged != null)
            {
                this.PanelStyleChanged(sender, e);
            }
        }
        
        protected virtual void OnCloseClick(object sender, EventArgs e)
        {
            if (this.CloseClick != null)
            {
                this.CloseClick(sender, e);
            }
        }
        
        protected virtual void OnExpandClick(object sender, EventArgs e)
        {
            this.Invalidate(false);
            if (this.ExpandClick != null)
            {
                this.ExpandClick(sender, e);
            }
        }
       
        protected virtual void OnExpandIconHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            if (e.HoverState == HoverState.Hover)
            {
                if (this.Cursor != Cursors.Hand)
                {
                    this.Cursor = Cursors.Hand;
                    if (this.Expand == true)
                    {
                        if (this is PanelEx)
                        {
                            if (string.IsNullOrEmpty(this._strToolTipTextExpandIconPanelExpanded) == false)
                            {
                                this._toolTip.SetToolTip(this, this._strToolTipTextExpandIconPanelExpanded);
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(this._strToolTipTextExpandIconPanelCollapsed) == false)
                        {
                            this._toolTip.SetToolTip(this, this._strToolTipTextExpandIconPanelCollapsed);
                        }
                    }
                }
            }
            else
            {
                if (this.Cursor == Cursors.Hand)
                {
                    this._toolTip.SetToolTip(this, string.Empty);
                    this._toolTip.Hide(this);
                    this.Cursor = Cursors.Default;
                }
            }
            if (this.ExpandIconHoverStateChanged != null)
            {
                this.ExpandIconHoverStateChanged(sender, e);
            }
        }
        
        protected virtual void OnCaptionHeightChanged(object sender, EventArgs e)
        {
            OnLayout(new LayoutEventArgs(this, null));
            this.Invalidate(false);
            if (this.CaptionHeightChanged != null)
            {
                this.CaptionHeightChanged(sender, e);
            }
        }
        
        protected virtual void OnCaptionBarHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            if (this is ExpondPanel)
            {
                if (e.HoverState == HoverState.Hover)
                {
                    if ((this.ShowCloseIcon == false) && (this.ShowExpandIcon == false))
                    {
                        if (this.Cursor != Cursors.Hand)
                        {
                            this.Cursor = Cursors.Hand;
                        }
                    }
                }
                else
                {
                    if (this.Cursor == Cursors.Hand)
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
                this.Invalidate(CaptionRectangle);
            }
            if (this.CaptionBarHoverStateChanged != null)
            {
                this.CaptionBarHoverStateChanged(sender, e);
            }
        }
        
        protected virtual void OnCloseIconHoverStateChanged(object sender, HoverStateChangeEventArgs e)
        {
            if (e.HoverState == HoverState.Hover)
            {
                if (this.Cursor != Cursors.Hand)
                {
                    this.Cursor = Cursors.Hand;
                }
                if (string.IsNullOrEmpty(this._strToolTipTextCloseIcon) == false)
                {
                    this._toolTip.SetToolTip(this, this._strToolTipTextCloseIcon);
                }
            }
            else
            {
                if (this.Cursor == Cursors.Hand)
                {
                    this._toolTip.SetToolTip(this, string.Empty);
                    this._toolTip.Hide(this);
                    this.Cursor = Cursors.Default;
                }
            }
            if (this.CloseIconHoverStateChanged != null)
            {
                this.CloseIconHoverStateChanged(sender, e);
            }
        }
       
        protected virtual void OnCustomColorsChanged(object sender, EventArgs e)
        {
            if (this.ColorScheme == ColorScheme.Custom)
            {
                this.PanelColors.Clear();
                this.Invalidate(false);
            }
            if (this.CustomColorsChanged != null)
            {
                this.CustomColorsChanged(sender, e);
            }
        }
       
        protected static void DrawString(
            Graphics graphics,
            RectangleF layoutRectangle,
            Font font,
            Color fontColor,
            string strText,
            RightToLeft rightToLeft,
            StringAlignment stringAlignment)
        {
            if (graphics == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }

            using (SolidBrush stringBrush = new SolidBrush(fontColor))
            {
                using (StringFormat stringFormat = new StringFormat())
                {
                    stringFormat.FormatFlags = StringFormatFlags.NoWrap;
                    if (rightToLeft == RightToLeft.Yes)
                    {
                        stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
                    }
                    stringFormat.Trimming = StringTrimming.EllipsisCharacter;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    stringFormat.Alignment = stringAlignment;
                    graphics.DrawString(strText, font, stringBrush, layoutRectangle, stringFormat);
                }
            }
        }
        
        protected static void DrawIcon(Graphics graphics, Image imgPanelIcon, Rectangle imageRectangle, Color foreColorImage, int iconPositionY)
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

            int iconPositionX = imageRectangle.Left;
            int iconWidth = imgPanelIcon.Width;
            int iconHeight = imgPanelIcon.Height;

            Rectangle rectangleIcon = new Rectangle(
                iconPositionX + (iconWidth / 2) - 1,
                iconPositionY + (iconHeight / 2) - 1,
                imgPanelIcon.Width,
                imgPanelIcon.Height - 1);

            using (System.Drawing.Imaging.ImageAttributes imageAttribute = new System.Drawing.Imaging.ImageAttributes())
            {
                imageAttribute.SetColorKey(Color.Magenta, Color.Magenta);
                System.Drawing.Imaging.ColorMap colorMap = new System.Drawing.Imaging.ColorMap();
                colorMap.OldColor = Color.FromArgb(0, 60, 166);
                colorMap.NewColor = foreColorImage;
                imageAttribute.SetRemapTable(new System.Drawing.Imaging.ColorMap[] { colorMap });

                graphics.DrawImage(imgPanelIcon, rectangleIcon, 0, 0, iconWidth, iconHeight, GraphicsUnit.Pixel, imageAttribute);
            }
        }
        
        protected static void DrawImage(Graphics graphics, Image image, Rectangle imageRectangle)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            if (image != null)
            {
                graphics.DrawImage(image, imageRectangle);
            }
        }
        
        protected static void DrawImagesAndText(
            Graphics graphics,
            Rectangle captionRectangle,
            int iSpacing,
            Rectangle imageRectangle,
            Image image,
            RightToLeft rightToLeft,
            Font fontCaption,
            Color captionForeColor,
            string strCaptionText)
        {
            //DrawImages
            int iTextPositionX1 = iSpacing;
            int iTextPositionX2 = captionRectangle.Right - iSpacing;

            imageRectangle.Y = (captionRectangle.Height - imageRectangle.Height) / 2;

            if (rightToLeft == RightToLeft.No)
            {
                if (image != null)
                {
                    DrawImage(graphics, image, imageRectangle);
                    iTextPositionX1 += imageRectangle.Width + iSpacing;
                }
            }
            // Draw Caption text
            Rectangle textRectangle = captionRectangle;
            textRectangle.X = iTextPositionX1;
            textRectangle.Width -= iTextPositionX1 + iSpacing;
            if (rightToLeft == RightToLeft.Yes)
            {
                if (image != null)
                {
                    Rectangle imageRectangleRight = imageRectangle;
                    imageRectangleRight.X = iTextPositionX2 - imageRectangle.Width;
                    DrawImage(graphics, image, imageRectangleRight);
                    iTextPositionX2 = imageRectangleRight.X - iSpacing;
                }
            }
            textRectangle.Width = iTextPositionX2 - iTextPositionX1;
            DrawString(graphics, textRectangle, fontCaption, captionForeColor, strCaptionText, rightToLeft, StringAlignment.Near);

        }
        
        protected static void DrawImagesAndText(
            Graphics graphics,
            Rectangle captionRectangle,
            int iSpacing,
            Rectangle imageRectangle,
            Image image,
            RightToLeft rightToLeft,
            bool bIsClosable,
            bool bShowCloseIcon,
            Image imageClosePanel,
            Color foreColorCloseIcon,
            ref Rectangle rectangleImageClosePanel,
            bool bShowExpandIcon,
            Image imageExandPanel,
            Color foreColorExpandIcon,
            ref Rectangle rectangleImageExandPanel,
            Font fontCaption,
            Color captionForeColor,
            string strCaptionText)
        {
            //DrawImages
            int iTextPositionX1 = iSpacing;
            int iTextPositionX2 = captionRectangle.Right - iSpacing;

            imageRectangle.Y = (captionRectangle.Height - imageRectangle.Height) / 2;

            if (rightToLeft == RightToLeft.No)
            {
                if (image != null)
                {
                    DrawImage(graphics, image, imageRectangle);
                    iTextPositionX1 += imageRectangle.Width + iSpacing;
                    iTextPositionX2 -= iTextPositionX1;
                }
            }
            else
            {
                if ((bShowCloseIcon == true) && (imageClosePanel != null))
                {
                    rectangleImageClosePanel = imageRectangle;
                    rectangleImageClosePanel.X = imageRectangle.X;
                    if (bIsClosable == true)
                    {
                        DrawIcon(graphics, imageClosePanel, rectangleImageClosePanel, foreColorCloseIcon, imageRectangle.Y);
                    }
                    iTextPositionX1 = rectangleImageClosePanel.X + rectangleImageClosePanel.Width;
                }
                if ((bShowExpandIcon == true) && (imageExandPanel != null))
                {
                    rectangleImageExandPanel = imageRectangle;
                    rectangleImageExandPanel.X = imageRectangle.X;
                    if ((bShowCloseIcon == true) && (imageClosePanel != null))
                    {
                        rectangleImageExandPanel.X = iTextPositionX1 + (iSpacing / 2);
                    }
                    DrawIcon(graphics, imageExandPanel, rectangleImageExandPanel, foreColorExpandIcon, imageRectangle.Y);
                    iTextPositionX1 = rectangleImageExandPanel.X + rectangleImageExandPanel.Width;
                }
            }
            // Draw Caption text
            RectangleF textRectangle = captionRectangle;
            textRectangle.X = iTextPositionX1;
            textRectangle.Width -= iTextPositionX1 + iSpacing;
            if (rightToLeft == RightToLeft.No)
            {
                if ((bShowCloseIcon == true) && (imageClosePanel != null))
                {
                    rectangleImageClosePanel = imageRectangle;
                    rectangleImageClosePanel.X = captionRectangle.Right - iSpacing - imageRectangle.Width;
                    if (bIsClosable == true)
                    {
                        DrawIcon(graphics, imageClosePanel, rectangleImageClosePanel, foreColorCloseIcon, imageRectangle.Y);
                    }
                    iTextPositionX2 = rectangleImageClosePanel.X;
                }
                if ((bShowExpandIcon == true) && (imageExandPanel != null))
                {
                    rectangleImageExandPanel = imageRectangle;
                    rectangleImageExandPanel.X = captionRectangle.Right - iSpacing - imageRectangle.Width;
                    if ((bShowCloseIcon == true) && (imageClosePanel != null))
                    {
                        rectangleImageExandPanel.X = iTextPositionX2 - (iSpacing / 2) - imageRectangle.Width;
                    }
                    DrawIcon(graphics, imageExandPanel, rectangleImageExandPanel, foreColorExpandIcon, imageRectangle.Y);
                    iTextPositionX2 = rectangleImageExandPanel.X;
                }
                if ((bShowCloseIcon == true)
                    && (imageClosePanel != null)
                    && (bShowExpandIcon == true)
                    && (imageExandPanel != null))
                {
                    iTextPositionX2 -= iSpacing;
                }
            }
            else
            {
                if (image != null)
                {
                    Rectangle imageRectangleRight = imageRectangle;
                    imageRectangleRight.X = iTextPositionX2 - imageRectangle.Width;
                    DrawImage(graphics, image, imageRectangleRight);
                    iTextPositionX2 = imageRectangleRight.X - iSpacing;
                }
            }
            textRectangle.Width = iTextPositionX2 - iTextPositionX1;
            textRectangle.Y = (float)(captionRectangle.Height - fontCaption.Height) / 2 + 1;
            textRectangle.Height = fontCaption.Height;
            DrawString(graphics, textRectangle, fontCaption, captionForeColor, strCaptionText, rightToLeft, StringAlignment.Near);

            //if the ExpondPanel not closable then the RectangleCloseIcon must be empty
            if (bIsClosable == false)
            {
                rectangleImageClosePanel = Rectangle.Empty;
            }
        }
       
        protected static void DrawImagesAndText(
            Graphics graphics,
            DockStyle dockStyle,
            int iSpacing,
            Rectangle captionRectangle,
            Rectangle panelRectangle,
            Rectangle imageRectangle,
            Image image,
            RightToLeft rightToLeft,
            bool bShowCloseIcon,
            Image imageClosePanel,
            Color foreColorCloseIcon,
            ref Rectangle rectangleImageClosePanel,
            bool bShowExpandIcon,
            bool bIsExpanded,
            Image imageExandPanel,
            Color foreColorExpandPanel,
            ref Rectangle rectangleImageExandPanel,
            Font fontCaption,
            Color captionForeColor,
            Color collapsedForeColor,
            string strCaptionText)
        {
            switch (dockStyle)
            {
                case DockStyle.Left:
                case DockStyle.Right:
                    if (bIsExpanded == true)
                    {
                        DrawImagesAndText(
                            graphics,
                            captionRectangle,
                            iSpacing,
                            imageRectangle,
                            image,
                            rightToLeft,
                            true,
                            bShowCloseIcon,
                            imageClosePanel,
                            foreColorCloseIcon,
                            ref rectangleImageClosePanel,
                            bShowExpandIcon,
                            imageExandPanel,
                            foreColorExpandPanel,
                            ref rectangleImageExandPanel,
                            fontCaption,
                            captionForeColor,
                            strCaptionText);
                    }
                    else
                    {
                        rectangleImageClosePanel = Rectangle.Empty;
                        DrawVerticalImagesAndText(
                            graphics,
                            captionRectangle,
                            panelRectangle,
                            imageRectangle,
                            dockStyle,
                            image,
                            rightToLeft,
                            imageExandPanel,
                            foreColorExpandPanel,
                            ref rectangleImageExandPanel,
                            fontCaption,
                            collapsedForeColor,
                            strCaptionText);
                    }
                    break;
                case DockStyle.Top:
                case DockStyle.Bottom:
                    DrawImagesAndText(
                        graphics,
                        captionRectangle,
                        iSpacing,
                        imageRectangle,
                        image,
                        rightToLeft,
                        true,
                        bShowCloseIcon,
                        imageClosePanel,
                        foreColorCloseIcon,
                        ref rectangleImageClosePanel,
                        bShowExpandIcon,
                        imageExandPanel,
                        foreColorExpandPanel,
                        ref rectangleImageExandPanel,
                        fontCaption,
                        captionForeColor,
                        strCaptionText);
                    break;
            }
        }
        
        protected static void RenderDoubleBackgroundGradient(Graphics graphics, Rectangle bounds, Color beginColor, Color middleColor, Color endColor, LinearGradientMode linearGradientMode, bool flipHorizontal)
        {
            int iUpperHeight = bounds.Height / 2;
            int iLowerHeight = bounds.Height - iUpperHeight;

            RenderDoubleBackgroundGradient(
                graphics,
                bounds,
                beginColor,
                middleColor,
                endColor,
                iUpperHeight,
                iLowerHeight,
                linearGradientMode,
                flipHorizontal);
        }
        
        protected static void RenderBackgroundGradient(Graphics graphics, Rectangle bounds, Color beginColor, Color endColor, LinearGradientMode linearGradientMode)
        {
            if (graphics == null)
            {
                throw new ArgumentException(
                    string.Format(
                    System.Globalization.CultureInfo.CurrentUICulture,
                    StaticResource.IDS_ArgumentException,
                    typeof(Graphics).Name));
            }
            if (IsZeroWidthOrHeight(bounds))
            {
                return;
            }
            using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(bounds, beginColor, endColor, linearGradientMode))
            {
                graphics.FillRectangle(linearGradientBrush, bounds);
            }
        }
        
        protected static void RenderButtonBackground(Graphics graphics, Rectangle bounds, Color colorGradientBegin, Color colorGradientMiddle, Color colorGradientEnd)
        {
            RectangleF upperRectangle = bounds;
            upperRectangle.Height = bounds.Height * 0.4f;

            using (LinearGradientBrush upperLinearGradientBrush = new LinearGradientBrush(
                    upperRectangle,
                    colorGradientBegin,
                    colorGradientMiddle,
                    LinearGradientMode.Vertical))
            {
                if (upperLinearGradientBrush != null)
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0.0F, 1.0F };
                    blend.Factors = new float[] { 0.0F, 0.6F };
                    upperLinearGradientBrush.Blend = blend;
                    graphics.FillRectangle(upperLinearGradientBrush, upperRectangle);
                }
            }

            RectangleF lowerRectangle = bounds;
            lowerRectangle.Y = upperRectangle.Height;
            lowerRectangle.Height = bounds.Height - upperRectangle.Height;

            using (LinearGradientBrush lowerLinearGradientBrush = new LinearGradientBrush(
                    lowerRectangle,
                    colorGradientMiddle,
                    colorGradientEnd,
                    LinearGradientMode.Vertical))
            {
                if (lowerLinearGradientBrush != null)
                {
                    graphics.FillRectangle(lowerLinearGradientBrush, lowerRectangle);
                }
            }
            //At some captionheights there are drawing errors. This is the correction
            RectangleF correctionRectangle = lowerRectangle;
            correctionRectangle.Y -= 1;
            correctionRectangle.Height = 2;
            using (SolidBrush solidBrush = new SolidBrush(colorGradientMiddle))
            {
                graphics.FillRectangle(solidBrush, correctionRectangle);
            }
        }
        
        protected static void RenderFlatButtonBackground(Graphics graphics, Rectangle bounds, Color colorGradientBegin, Color colorGradientEnd, bool bHover)
        {
            using (LinearGradientBrush gradientBrush = GetFlatGradientBackBrush(bounds, colorGradientBegin, colorGradientEnd, bHover))
            {
                if (gradientBrush != null)
                {
                    graphics.FillRectangle(gradientBrush, bounds);
                }
            }
        }

        protected static GraphicsPath GetPath(Rectangle bounds, int radius)
        {
            int x = bounds.X;
            int y = bounds.Y;
            int width = bounds.Width;
            int height = bounds.Height;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(x, y, radius, radius, 180, 90);				                    //Upper left corner
            graphicsPath.AddArc(x + width - radius, y, radius, radius, 270, 90);			    //Upper right corner
            graphicsPath.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);//Lower right corner
            graphicsPath.AddArc(x, y + height - radius, radius, radius, 90, 90);			                    //Lower left corner
            graphicsPath.CloseFigure();
            return graphicsPath;
        }
     
        protected static GraphicsPath GetUpperBackgroundPath(Rectangle bounds, int radius)
        {
            int x = bounds.X;
            int y = bounds.Y;
            int width = bounds.Width;
            int height = bounds.Height;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddLine(x, y + height, x, y - radius);                 //Left Line
            graphicsPath.AddArc(x, y, radius, radius, 180, 90);                 //Upper left corner
            graphicsPath.AddArc(x + width - radius, y, radius, radius, 270, 90);//Upper right corner
            graphicsPath.AddLine(x + width, y + radius, x + width, y + height); //Right Line
            graphicsPath.CloseFigure();
            return graphicsPath;
        }
       
        protected static GraphicsPath GetBackgroundPath(Rectangle bounds, int radius)
        {
            int x = bounds.X;
            int y = bounds.Y;
            int width = bounds.Width;
            int height = bounds.Height;
            GraphicsPath graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(x, y, radius, radius, 180, 90);				                    //Upper left corner
            graphicsPath.AddArc(x + width - radius, y, radius, radius, 270, 90);			    //Upper right corner
            graphicsPath.AddArc(x + width - radius, y + height - radius, radius, radius, 0, 90);//Lower right corner
            graphicsPath.AddArc(x, y + height - radius, radius, radius, 90, 90);			    //Lower left corner
            graphicsPath.CloseFigure();
            return graphicsPath;
        }
        
        protected static LinearGradientBrush GetFlatGradientBackBrush(Rectangle bounds, Color colorGradientBegin, Color colorGradientEnd, bool bHover)
        {
            LinearGradientBrush linearGradientBrush = null;
            Blend blend = new Blend();
            blend.Positions = new float[] { 0.0F, 0.2F, 0.3F, 0.4F, 0.5F, 0.6F, 0.7F, 0.8F, 1.0F };
            if (bHover == false)
            {
                blend.Factors = new float[] { 0.0F, 0.0F, 0.2F, 0.4F, 0.6F, 0.4F, 0.2F, 0.0F, 0.0F };
            }
            else
            {
                blend.Factors = new float[] { 0.4F, 0.5F, 0.6F, 0.8F, 1.0F, 0.8F, 0.6F, 0.5F, 0.4F };
            }
            linearGradientBrush = linearGradientBrush = new LinearGradientBrush(bounds, colorGradientBegin, colorGradientEnd, LinearGradientMode.Horizontal);
            if (linearGradientBrush != null)
            {
                linearGradientBrush.Blend = blend;
            }
            return linearGradientBrush;
        }
        
        protected static bool IsZeroWidthOrHeight(Rectangle rectangle)
        {
            if (rectangle.Width != 0)
            {
                return (rectangle.Height == 0);
            }
            return true;
        }
        #endregion

        #region MethodsPrivate

        private static void RenderDoubleBackgroundGradient(Graphics graphics, Rectangle bounds, Color beginColor, Color middleColor, Color endColor, int firstGradientWidth, int secondGradientWidth, LinearGradientMode mode, bool flipHorizontal)
        {
            if ((bounds.Width != 0) && (bounds.Height != 0))
            {
                Rectangle rectangle1 = bounds;
                Rectangle rectangle2 = bounds;
                bool flag1 = true;
                if (mode == LinearGradientMode.Horizontal)
                {
                    if (flipHorizontal)
                    {
                        Color color1 = endColor;
                        endColor = beginColor;
                        beginColor = color1;
                    }
                    rectangle2.Width = firstGradientWidth;
                    rectangle1.Width = secondGradientWidth + 1;
                    rectangle1.X = bounds.Right - rectangle1.Width;
                    flag1 = bounds.Width > (firstGradientWidth + secondGradientWidth);
                }
                else
                {
                    rectangle2.Height = firstGradientWidth;
                    rectangle1.Height = secondGradientWidth + 1;
                    rectangle1.Y = bounds.Bottom - rectangle1.Height;
                    flag1 = bounds.Height > (firstGradientWidth + secondGradientWidth);
                }
                if (flag1)
                {
                    using (Brush brush1 = new SolidBrush(middleColor))
                    {
                        graphics.FillRectangle(brush1, bounds);
                    }
                    using (Brush brush2 = new LinearGradientBrush(rectangle2, beginColor, middleColor, mode))
                    {
                        graphics.FillRectangle(brush2, rectangle2);
                    }
                    using (LinearGradientBrush brush3 = new LinearGradientBrush(rectangle1, middleColor, endColor, mode))
                    {
                        if (mode == LinearGradientMode.Horizontal)
                        {
                            rectangle1.X++;
                            rectangle1.Width--;
                        }
                        else
                        {
                            rectangle1.Y++;
                            rectangle1.Height--;
                        }
                        graphics.FillRectangle(brush3, rectangle1);
                        return;
                    }
                }
                using (Brush brush4 = new LinearGradientBrush(bounds, beginColor, endColor, mode))
                {
                    graphics.FillRectangle(brush4, bounds);
                }
            }
        }

        private static void DrawVerticalImagesAndText(
            Graphics graphics,
            Rectangle captionRectangle,
            Rectangle panelRectangle,
            Rectangle imageRectangle,
            DockStyle dockStyle,
            Image image,
            RightToLeft rightToLeft,
            Image imageExandPanel,
            Color foreColorExpandPanel,
            ref Rectangle rectangleImageExandPanel,
            Font captionFont,
            Color collapsedCaptionForeColor,
            string strCaptionText)
        {
            imageRectangle.Y = (captionRectangle.Height - imageRectangle.Height) / 2;

            if (imageExandPanel != null)
            {
                rectangleImageExandPanel = imageRectangle;
                rectangleImageExandPanel.X = (panelRectangle.Width - imageRectangle.Width) / 2;
                DrawIcon(graphics, imageExandPanel, rectangleImageExandPanel, foreColorExpandPanel, imageRectangle.Y);
            }

            int iTextPositionY1 = CaptionSpacing;
            int iTextPositionY2 = panelRectangle.Height - CaptionSpacing;

            if (image != null)
            {
                imageRectangle.Y = iTextPositionY2 - imageRectangle.Height;
                imageRectangle.X = (panelRectangle.Width - imageRectangle.Width) / 2;
                DrawImage(graphics, image, imageRectangle);
                iTextPositionY1 += imageRectangle.Height + CaptionSpacing;
            }

            iTextPositionY2 -= captionRectangle.Height + iTextPositionY1;

            Rectangle textRectangle = new Rectangle(
                iTextPositionY1,
                panelRectangle.Y,
                iTextPositionY2,
                captionRectangle.Height);

            using (SolidBrush textBrush = new SolidBrush(collapsedCaptionForeColor))
            {
                if (dockStyle == DockStyle.Left)
                {
                    graphics.TranslateTransform(0, panelRectangle.Height);
                    graphics.RotateTransform(-90);

                    DrawString(
                        graphics,
                        textRectangle,
                        captionFont,
                        collapsedCaptionForeColor,
                        strCaptionText,
                        rightToLeft,
                        StringAlignment.Center);

                    graphics.ResetTransform();
                }
                if (dockStyle == DockStyle.Right)
                {
                    graphics.TranslateTransform(panelRectangle.Width, 0);
                    graphics.RotateTransform(90);

                    DrawString(
                        graphics,
                        textRectangle,
                        captionFont,
                        collapsedCaptionForeColor,
                        strCaptionText,
                        rightToLeft,
                        StringAlignment.Center);

                    graphics.ResetTransform();
                }
            }
        }
        #endregion
    }
}
