using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace JLQ.Common
{
    #region CollapsiblePanel class
    //可收缩的面板
    /// <summary>
    /// An extended <see cref="System.Windows.Forms.Panel">Panel</see> that provides collapsible panels like those provided in Windows XP.
    /// </summary>
    public class CollapsiblePanel : System.Windows.Forms.Panel
    {
        #region Events
        /// <summary>
        /// A <see cref="PanelState">PanelState</see> changed event.
        /// </summary>
        [Category("State"),
        Description("Raised when panel state has changed.")]
        public event PanelStateChangedEventHandler PanelStateChanged;
        #endregion

        #region Private class data
        private System.Drawing.Imaging.ColorMatrix grayMatrix;
        private System.Drawing.Imaging.ImageAttributes grayAttributes;
        private PanelState state = PanelState.Collapsed;
        private int panelHeight;
        private int imageIndex = 0;
        private const int minTitleHeight = 24;
        private const int iconBorder = 2;
        private const int expandBorder = 4;
        private System.Drawing.Color startColour = Color.White;
        private System.Drawing.Color endColour = Color.FromArgb(199, 212, 247);
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Label labelTitle;
        private System.Drawing.Image image;
        private System.Windows.Forms.ImageList imageList;
        #endregion

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CollapsiblePanel));
            this.labelTitle = new System.Windows.Forms.Label();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelTitle.ForeColor = System.Drawing.Color.Black;
            this.labelTitle.Location = new System.Drawing.Point(114, 17);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(200, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Title";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelTitle.Paint += new System.Windows.Forms.PaintEventHandler(this.labelTitle_Paint);
            this.labelTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseMove);
            this.labelTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.labelTitle_MouseUp);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Collapse.png");
            this.imageList.Images.SetKeyName(1, "Expand.png");
            // 
            // CollapsiblePanel
            // 
            this.Controls.Add(this.labelTitle);
            this.ResumeLayout(false);

        }
        #endregion

        #region Public Constructors
        /// <summary>
        /// Initialises a new instance of <a cref="Salamander.Windows.Forms.CollapsiblePanel">CollapsiblePanel</a>.
        /// </summary>
        public CollapsiblePanel()
            : base()
        {
            this.components = new System.ComponentModel.Container();

            InitializeComponent();

            // Set the background colour to ControlLightLight
            this.BackColor = System.Drawing.SystemColors.Control;

            // Store the current panelHeight
            this.panelHeight = this.Height;

            // Setup the ColorMatrix and ImageAttributes for grayscale images.
            this.grayMatrix = new ColorMatrix();
            this.grayMatrix.Matrix00 = 1 / 3f;
            this.grayMatrix.Matrix01 = 1 / 3f;
            this.grayMatrix.Matrix02 = 1 / 3f;
            this.grayMatrix.Matrix10 = 1 / 3f;
            this.grayMatrix.Matrix11 = 1 / 3f;
            this.grayMatrix.Matrix12 = 1 / 3f;
            this.grayMatrix.Matrix20 = 1 / 3f;
            this.grayMatrix.Matrix21 = 1 / 3f;
            this.grayMatrix.Matrix22 = 1 / 3f;
            this.grayAttributes = new ImageAttributes();
            this.grayAttributes.SetColorMatrix(this.grayMatrix, ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/sets the <see cref="PanelState">PanelState</see>.
        /// </summary>
        [Browsable(true)]
        [Category("Title")]
        public PanelState PanelState
        {
            get
            {
                return this.state;
            }
            set
            {
                PanelState oldState = this.state;
                this.state = value;
                if (oldState != this.state)
                {
                    // State has changed to update the display
                    UpdateDisplayedState();
                }
            }
        }

        /// <summary>
        /// Gets/sets the text displayed as the panel title.
        /// </summary>
        [Category("Title"),
        Description("The text contained in the title bar.")]
        public string TitleText
        {
            get
            {
                return this.labelTitle.Text;
            }
            set
            {
                this.labelTitle.Text = value;
            }
        }

        /// <summary>
        /// Gets/sets the foreground colour used for the title bar.
        /// </summary>
        [Category("Title"),
        Description("The foreground colour used to display the title text.")]
        public Color TitleFontColour
        {
            get
            {
                return this.labelTitle.ForeColor;
            }
            set
            {
                this.labelTitle.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets/sets the font used for the title bar text.
        /// </summary>
        [Category("Title"),
        Description("The font used to display the title text.")]
        public Font TitleFont
        {
            get
            {
                return this.labelTitle.Font;
            }
            set
            {
                this.labelTitle.Font = value;
            }
        }

        /// <summary>
        /// Gets/sets the image list used for the expand/collapse image.
        /// </summary>
        [Category("Title"),
        Description("The image list to get the images displayed for expanding/collapsing the panel.")]
        public ImageList ImageList
        {
            get
            {
                return this.imageList;
            }
            set
            {
                this.imageList = value;
                if (null != this.imageList)
                {
                    if (this.imageList.Images.Count > 0)
                    {
                        this.imageIndex = 0;
                    }
                }
                else
                {
                    this.imageIndex = -1;
                }
            }
        }

        /// <summary>
        /// Gets/sets the starting colour for the background gradient of the header.
        /// </summary>
        [Category("Title"),
        Description("The colour used at the start of the colour gradient displayed as the background of the title bar.")]
        public Color StartColour
        {
            get
            {
                return this.startColour;
            }
            set
            {
                this.startColour = value;
                this.labelTitle.Invalidate();
            }
        }

        /// <summary>
        /// Gets/sets the ending colour for the background gradient of the header.
        /// </summary>
        [Category("Title"),
        Description("The colour used at the end of the colour gradient displayed as the background of the title bar.")]
        public Color EndColour
        {
            get
            {
                return this.endColour;
            }
            set
            {
                this.endColour = value;
                this.labelTitle.Invalidate();
            }
        }

        /// <summary>
        /// Gets/sets the image displayed in the header of the title bar.
        /// </summary>
        [Category("Title"),
        Description("The image that will be displayed on the left hand side of the title bar.")]
        public Image Image
        {
            get
            {
                return this.image;
            }
            set
            {
                this.image = value;
                if (null != value)
                {
                    // Update the height of the title label
                    this.labelTitle.Height = this.image.Height + (2 * CollapsiblePanel.iconBorder);
                    if (this.labelTitle.Height < minTitleHeight)
                    {
                        this.labelTitle.Height = minTitleHeight;
                    }
                }
                this.labelTitle.Invalidate();
            }
        }
        #endregion

        #region Private Helper functions
        // <feature>Expand/Collapse functionality updated as per Windows XP. Whole of title bar is active
        // <version>1.3</version>
        // <date>23-Oct-2002</date>
        // </feature>
        /// <summary>
        /// Helper function to determine if the mouse is currently over the title bar.
        /// </summary>
        /// <param name="xPos">The x-coordinate of the mouse position.</param>
        /// <param name="yPos">The y-coordinate of the mouse position.</param>
        /// <returns></returns>
        private bool IsOverTitle(int xPos, int yPos)
        {
            // Get the dimensions of the title label
            Rectangle rectTitle = this.labelTitle.Bounds;
            // Check if the supplied coordinates are over the title label
            if (rectTitle.Contains(xPos, yPos))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Helper function to update the displayed state of the panel.
        /// </summary>
        private void UpdateDisplayedState()
        {
            switch (this.state)
            {
                case PanelState.Collapsed:
                    // Entering collapsed state, so store the current height.
                    this.panelHeight = this.Height;
                    // Collapse the panel
                    this.Height = labelTitle.Height;
                    // Update the image.
                    this.imageIndex = 1;
                    break;
                case PanelState.Expanded:
                    // Entering expanded state, so expand the panel.
                    this.Height = this.panelHeight;
                    // Update the image.
                    this.imageIndex = 0;
                    break;
                default:
                    // Ignore
                    break;
            }
            this.labelTitle.Invalidate();

            OnPanelStateChanged(new PanelEventArgs(this));
        }
        #endregion

        #region Event handlers
        /// <summary>
        /// Event handler for the <see cref="CollapsiblePanel.PanelStateChanged">PanelStateChanged</see> event.
        /// </summary>
        /// <param name="e">A <see cref="Salamander.Windows.Forms.PanelEventArgs">PanelEventArgs</see> that contains the event data.</param>
        protected virtual void OnPanelStateChanged(PanelEventArgs e)
        {
            if (PanelStateChanged != null)
            {
                PanelStateChanged(this, e);
            }
        }

        private void labelTitle_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            const int diameter = 14;
            int radius = diameter / 2;
            Rectangle bounds = labelTitle.Bounds;
            int offsetY = 0;
            if (null != this.image)
            {
                offsetY = this.labelTitle.Height - CollapsiblePanel.minTitleHeight;
                if (offsetY < 0)
                {
                    offsetY = 0;
                }
                bounds.Offset(0, offsetY);
                bounds.Height -= offsetY;
            }

            e.Graphics.Clear(this.Parent.BackColor);

            // Create a GraphicsPath with curved top corners
            GraphicsPath path = new GraphicsPath();
            path.AddLine(bounds.Left + radius, bounds.Top, bounds.Right - diameter - 1, bounds.Top);
            path.AddArc(bounds.Right - diameter - 1, bounds.Top, diameter, diameter, 270, 90);
            path.AddLine(bounds.Right, bounds.Top + radius, bounds.Right, bounds.Bottom);
            path.AddLine(bounds.Right, bounds.Bottom, bounds.Left - 1, bounds.Bottom);
            path.AddArc(bounds.Left, bounds.Top, diameter, diameter, 180, 90);

            // Create a colour gradient
            // <feature>Draws the title gradient grayscale when disabled.
            // <version>1.4</version>
            // <date>25-Nov-2002</date>
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (true == this.Enabled)
            {
                LinearGradientBrush brush = new LinearGradientBrush(
                    bounds, this.startColour, this.endColour, LinearGradientMode.Horizontal);

                // Paint the colour gradient into the title label.
                e.Graphics.FillPath(brush, path);
            }
            else
            {
                Colour grayStart = new Colour();
                grayStart.CurrentColour = this.startColour;
                grayStart.Saturation = 0f;
                Colour grayEnd = new Colour();
                grayEnd.CurrentColour = this.endColour;
                grayEnd.Saturation = 0f;
                LinearGradientBrush brush = new LinearGradientBrush(
                    bounds, grayStart.CurrentColour, grayEnd.CurrentColour,
                    LinearGradientMode.Horizontal);

                // Paint the grayscale gradient into the title label.
                e.Graphics.FillPath(brush, path);
            }
            // </feature>

            // Draw the header icon, if there is one
            System.Drawing.GraphicsUnit graphicsUnit = System.Drawing.GraphicsUnit.Display;
            int offsetX = CollapsiblePanel.iconBorder;
            if (null != this.image)
            {
                offsetX += this.image.Width + CollapsiblePanel.iconBorder;
                // <feature>Draws the title icon grayscale when the panel is disabled.
                // <version>1.4</version>
                // <date>25-Nov-2002</date>
                RectangleF srcRectF = this.image.GetBounds(ref graphicsUnit);
                Rectangle destRect = new Rectangle(CollapsiblePanel.iconBorder,
                    CollapsiblePanel.iconBorder, this.image.Width, this.image.Height);
                if (true == this.Enabled)
                {
                    e.Graphics.DrawImage(this.image, destRect, (int)srcRectF.Left, (int)srcRectF.Top,
                        (int)srcRectF.Width, (int)srcRectF.Height, graphicsUnit);
                }
                else
                {
                    e.Graphics.DrawImage(this.image, destRect, (int)srcRectF.Left, (int)srcRectF.Top,
                        (int)srcRectF.Width, (int)srcRectF.Height, graphicsUnit, this.grayAttributes);
                }
                // </feature>
            }

            // Draw the title text.
            SolidBrush textBrush = new SolidBrush(this.TitleFontColour);
            // <feature>Title text truncated with an ellipsis where necessary.
            // <version>1.2</version>
            // <date>18-Oct-2002</date>
            // <source>Nnamdi Onyeyiri (mailto:theeclypse@hotmail.com)</source>
            float left = (float)offsetX;
            float top = (float)offsetY + (float)CollapsiblePanel.expandBorder;
            float width = (float)this.labelTitle.Width - left - this.imageList.ImageSize.Width -
                CollapsiblePanel.expandBorder;
            float height = (float)CollapsiblePanel.minTitleHeight - (2f * (float)CollapsiblePanel.expandBorder);
            RectangleF textRectF = new RectangleF(left, top, width, height);
            StringFormat format = new StringFormat();
            format.Trimming = StringTrimming.EllipsisWord;
            // <feature>Draw title text disabled where appropriate.
            // <version>1.4</version>
            // <date>25-Nov-2002</date>
            if (true == this.Enabled)
            {
                e.Graphics.DrawString(labelTitle.Text, labelTitle.Font, textBrush,
                    textRectF, format);
            }
            else
            {
                Color disabled = SystemColors.GrayText;
                ControlPaint.DrawStringDisabled(e.Graphics, labelTitle.Text, labelTitle.Font,
                    disabled, textRectF, format);
            }
            // </feature>
            // </feature>

            // Draw a white line at the bottom:
            const int lineWidth = 1;
            SolidBrush lineBrush = new SolidBrush(Color.White);
            Pen linePen = new Pen(lineBrush, lineWidth);
            path.Reset();
            path.AddLine(bounds.Left, bounds.Bottom - lineWidth, bounds.Right,
                bounds.Bottom - lineWidth);
            e.Graphics.DrawPath(linePen, path);

            // Draw the expand/collapse image
            // <feature>Expand/Collapse image drawn grayscale when panel is disabled.
            // <version>1.4</version>
            // <date>25-Nov-2002</date>
            int xPos = bounds.Right - this.imageList.ImageSize.Width - CollapsiblePanel.expandBorder;
            int yPos = bounds.Top + CollapsiblePanel.expandBorder;
            //RectangleF srcIconRectF = this.ImageList.Images[2].GetBounds(ref graphicsUnit);
            RectangleF srcIconRectF = this.ImageList.Images[(int)this.state].GetBounds(ref graphicsUnit);
            Rectangle destIconRect = new Rectangle(xPos, yPos,
                this.imageList.ImageSize.Width, this.imageList.ImageSize.Height);
            if (true == this.Enabled)
            {
                e.Graphics.DrawImage(this.ImageList.Images[(int)this.state], destIconRect,
                    (int)srcIconRectF.Left, (int)srcIconRectF.Top, (int)srcIconRectF.Width,
                    (int)srcIconRectF.Height, graphicsUnit);
            }
            else
            {
                e.Graphics.DrawImage(this.ImageList.Images[(int)this.state], destIconRect,
                    (int)srcIconRectF.Left, (int)srcIconRectF.Top, (int)srcIconRectF.Width,
                    (int)srcIconRectF.Height, graphicsUnit, this.grayAttributes);
            }
            // </feature>
        }

        private void labelTitle_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (true == IsOverTitle(e.X, e.Y)))
            {
                if ((null != this.imageList) && (this.imageList.Images.Count >= 2))
                {
                    if (0 == this.imageIndex)
                    {
                        // Currently expanded, so store the current height.
                        this.state = PanelState.Collapsed;
                    }
                    else
                    {
                        // Currently collapsed, so expand the panel.
                        this.state = PanelState.Expanded;
                    }
                    UpdateDisplayedState();
                }
            }
        }

        private void labelTitle_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.None) && (true == IsOverTitle(e.X, e.Y)))
            {
                this.labelTitle.Cursor = Cursors.Hand;
            }
            else
            {
                this.labelTitle.Cursor = Cursors.Default;
            }
        }
        #endregion
    }
    #endregion

    #region Enumerations
    /// <summary>
    /// Defines the state of a <see cref="CollapsiblePanel">CollapsiblePanel</see>.
    /// </summary>
    public enum PanelState
    {
        /// <summary>
        /// The <see cref="CollapsiblePanel">CollapsiblePanel</see> is expanded.
        /// </summary>
        Expanded,
        /// <summary>
        /// The <see cref="CollapsiblePanel">CollapsiblePanel</see> is collapsed.
        /// </summary>
        Collapsed
    }
    #endregion

    #region Delegates
    /// <summary>
    /// A delegate type for hooking up panel state change notifications.
    /// </summary>
    public delegate void PanelStateChangedEventHandler(object sender, PanelEventArgs e);
    #endregion

    #region PanelEventArgs class
    /// <summary>
    /// Provides data for the <see cref="CollapsiblePanel.PanelStateChanged">PanelStateChanged</see> event.
    /// </summary>
    public class PanelEventArgs : System.EventArgs
    {
        #region Private Class data
        private CollapsiblePanel panel;
        #endregion

        #region Public Constructors
        /// <summary>
        /// Initialises a new <see cref="PanelEventArgs">PanelEventArgs</see>.
        /// </summary>
        /// <param name="sender">The originating <see cref="CollapsiblePanel">CollapsiblePanel</see>.</param>
        public PanelEventArgs(CollapsiblePanel sender)
        {
            this.panel = sender;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the <see cref="CollapsiblePanel">CollapsiblePanel</see> that triggered the event.
        /// </summary>
        public CollapsiblePanel CollapsiblePanel
        {
            get
            {
                return this.panel;
            }
        }

        /// <summary>
        /// Gets the <see cref="PanelState">PanelState</see> of the <see cref="CollapsiblePanel">CollapsiblePanel</see> that triggered the event.
        /// </summary>
        public PanelState PanelState
        {
            get
            {
                return this.panel.PanelState;
            }
        }
        #endregion
    }
    #endregion

    #region Colour Class
    /// <summary>
    /// Stores a colour and provides conversion between the RGB and HLS colour models
    /// </summary>
    public class Colour
    {
        // Constants
        public const int HUEMAX = 360;
        public const float SATMAX = 1.0f;
        public const float BRIGHTMAX = 1.0f;
        public const int RGBMAX = 255;

        // Member variables
        private Color m_clrCurrent = Color.Red;

        /// <summary>
        /// Default constructor
        /// </summary>
        public Colour()
        {
        }

        /// <summary>
        /// The current colour (RGB model)
        /// </summary>
        public Color CurrentColour
        {
            get
            {
                return m_clrCurrent;
            }
            set
            {
                m_clrCurrent = value;
            }
        }


        /// <summary>
        /// The Red component of the current colour
        /// </summary>
        public byte Red
        {
            get
            {
                return m_clrCurrent.R;
            }
            set
            {
                m_clrCurrent = Color.FromArgb(value, Green, Blue);
            }
        }

        /// <summary>
        /// The Green component of the current colour
        /// </summary>
        public byte Green
        {
            get
            {
                return m_clrCurrent.G;
            }
            set
            {
                m_clrCurrent = Color.FromArgb(Red, value, Blue);
            }
        }

        /// <summary>
        /// The Blue component of the current colour
        /// </summary>
        public byte Blue
        {
            get
            {
                return m_clrCurrent.B;
            }
            set
            {
                m_clrCurrent = Color.FromArgb(Red, Green, value);
            }
        }

        /// <summary>
        /// The Hue component of the current colour
        /// </summary>
        public int Hue
        {
            get
            {
                return (int)m_clrCurrent.GetHue();
            }
            set
            {
                m_clrCurrent = HSBToRGB(value,
                    m_clrCurrent.GetSaturation(),
                    m_clrCurrent.GetBrightness());
            }
        }

        // DEBUG: Mono Functions
        public float GetHue()
        {
            float top = ((float)(2 * Red - Green - Blue)) / (2 * 255);
            float bottom = (float)Math.Sqrt(((Red - Green) * (Red - Green) + (Red - Blue) * (Green - Blue)) / 255);
            return (float)Math.Acos(top / bottom);
        }

        public float GetSaturation()
        {
            return (255 -
                (((float)(Red + Green + Blue)) / 3) * Math.Min(Red, Math.Min(Green, Blue))) / 255;
        }

        public float GetBrightness()
        {
            return ((float)(Red + Green + Blue)) / (255.0f * 3.0f);
        }
        // END DEBUG

        /// <summary>
        /// The Saturation component of the current colour
        /// </summary>
        public float Saturation
        {
            get
            {
                if (0.0f == Brightness)
                {
                    return 0.0f;
                }
                else
                {
                    float fMax = (float)Math.Max(Red, Math.Max(Green, Blue));
                    float fMin = (float)Math.Min(Red, Math.Min(Green, Blue));
                    return (fMax - fMin) / fMax;
                }
            }
            set
            {
                m_clrCurrent = HSBToRGB((int)m_clrCurrent.GetHue(),
                    value, m_clrCurrent.GetBrightness());
            }
        }

        /// <summary>
        /// The Brightness component of the current colour
        /// </summary>
        public float Brightness
        {
            get
            {
                //return m_clrCurrent.GetBrightness();
                return (float)Math.Max(Red, Math.Max(Green, Blue)) / (255.0f);
            }
            set
            {
                m_clrCurrent = Colour.HSBToRGB((int)m_clrCurrent.GetHue(),
                    m_clrCurrent.GetSaturation(),
                    value);
            }
        }

        /// <summary>
        /// Converts HSB colour components to an RGB System.Drawing.Color
        /// </summary>
        /// <param name="Hue">Hue component</param>
        /// <param name="Saturation">Saturation component</param>
        /// <param name="Brightness">Brightness component</param>
        /// <returns>Returns the RGB value as a System.Drawing.Color</returns>
        public static Color HSBToRGB(int Hue, float Saturation, float Brightness)
        {
            // TODO: CheckHSBValues(Hue, Saturation, Brightness);
            int red = 0; int green = 0; int blue = 0;
            if (Saturation == 0.0f)
            {
                // Achromatic colour (black and white centre line)
                // Hue should be 0 (undefined), but we'll ignore it.
                // Set shade of grey
                red = green = blue = (int)(Brightness * 255);
            }
            else
            {
                // Chromatic colour
                // Map hue from [0-255] to [0-360] to hexagonal-space [0-6]
                // (360 / 256) * hue[0-255] / 60
                float fHexHue = (6.0f / 360.0f) * Hue;
                // Determine sector in hexagonal-space (RGB cube projection) {0,1,2,3,4,5}
                float fHexSector = (float)Math.Floor((double)fHexHue);
                // Determine exact position in particular sector [0-1]
                float fHexSectorPos = fHexHue - fHexSector;

                // Convert parameters to in-formula ranges
                float fBrightness = Brightness * 255.0f;
                float fSaturation = Saturation/*(float)Saturation * (1.0f / 360.0f)*/;

                // Magic formulas (from Foley & Van Dam). Adding 0.5 performs rounding instead of truncation
                byte bWashOut = (byte)(0.5f + fBrightness * (1.0f - fSaturation));
                byte bHueModifierOddSector = (byte)(0.5f + fBrightness * (1.0f - fSaturation * fHexSectorPos));
                byte bHueModifierEvenSector = (byte)(0.5f + fBrightness * (1.0f - fSaturation * (1.0f - fHexSectorPos)));

                // Assign values to RGB components (sector dependent)
                switch ((int)fHexSector)
                {
                    case 0:
                        // Hue is between red & yellow
                        red = (int)(Brightness * 255); green = bHueModifierEvenSector; blue = bWashOut;
                        break;
                    case 1:
                        // Hue is between yellow & green
                        red = bHueModifierOddSector; green = (int)(Brightness * 255); blue = bWashOut;
                        break;
                    case 2:
                        // Hue is between green & cyan
                        red = bWashOut; green = (int)(Brightness * 255); blue = bHueModifierEvenSector;
                        break;
                    case 3:
                        // Hue is between cyan & blue
                        red = bWashOut; green = bHueModifierOddSector; blue = (int)(Brightness * 255);
                        break;
                    case 4:
                        // Hue is between blue & magenta
                        red = bHueModifierEvenSector; green = bWashOut; blue = (int)(Brightness * 255);
                        break;
                    case 5:
                        // Hue is between magenta & red
                        red = (int)(Brightness * 255); green = bWashOut; blue = bHueModifierOddSector;
                        break;
                    default:
                        red = 0; green = 0; blue = 0;
                        break;
                }
            }

            return Color.FromArgb(red, green, blue);
        }
    }
    #endregion
}
