using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace JLQ.Common
{
    #region class CustomColors
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Description("The colors used in a panel")]
    public class CustomColors
    {
        #region Events
        [Description("Occurs when the value of the CustomColors property changes.")]
        public event EventHandler<EventArgs> CustomColorsChanged;
        #endregion

        #region FieldsPrivate
        private Color _borderColor = ProfessionalColors.GripDark;
        private Color _captionCloseIcon = SystemColors.ControlText;
        private Color _captionExpandIcon = SystemColors.ControlText;
        private Color _captionGradientBegin = ProfessionalColors.ToolStripGradientBegin;
        private Color _captionGradientEnd = ProfessionalColors.ToolStripGradientEnd;
        private Color _captionGradientMiddle = ProfessionalColors.ToolStripGradientMiddle;
        private Color _captionText = SystemColors.ControlText;
        private Color _innerBorderColor = ProfessionalColors.GripLight;
        #endregion

        #region Properties
        [Description("The border color of a Panel or ExpondPanel.")]
        public virtual Color BorderColor
        {
            get { return this._borderColor; }
            set
            {
                if (value.Equals(this._borderColor) == false)
                {
                    this._borderColor = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The forecolor of a close icon in a Panel or ExpondPanel.")]
        public virtual Color CaptionCloseIcon
        {
            get { return this._captionCloseIcon; }
            set
            {
                if (value.Equals(this._captionCloseIcon) == false)
                {
                    this._captionCloseIcon = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The forecolor of an expand icon in a Panel or ExpondPanel.")]
        public virtual Color CaptionExpandIcon
        {
            get { return this._captionExpandIcon; }
            set
            {
                if (value.Equals(this._captionExpandIcon) == false)
                {
                    this._captionExpandIcon = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The starting color of the gradient at the caption on a Panel or ExpondPanel.")]
        public virtual Color CaptionGradientBegin
        {
            get { return this._captionGradientBegin; }
            set
            {
                if (value.Equals(this._captionGradientBegin) == false)
                {
                    this._captionGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the gradient at the caption on a Panel or ExpondPanel")]
        public virtual Color CaptionGradientEnd
        {
            get { return this._captionGradientEnd; }
            set
            {
                if (value.Equals(this._captionGradientEnd) == false)
                {
                    this._captionGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The middle color of the gradient at the caption on a Panel or ExpondPanel.")]
        public virtual Color CaptionGradientMiddle
        {
            get { return this._captionGradientMiddle; }
            set
            {
                if (value.Equals(this._captionGradientMiddle) == false)
                {
                    this._captionGradientMiddle = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The text color at the caption on a Panel or ExpondPanel.")]
        public virtual Color CaptionText
        {
            get { return this._captionText; }
            set
            {
                if (value.Equals(this._captionText) == false)
                {
                    this._captionText = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The inner border color of a Panel.")]
        public virtual Color InnerBorderColor
        {
            get { return this._innerBorderColor; }
            set
            {
                if (value.Equals(this._innerBorderColor) == false)
                {
                    this._innerBorderColor = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion

        #region MethodsProtected
        protected virtual void OnCustomColorsChanged(object sender, EventArgs e)
        {
            if (this.CustomColorsChanged != null)
            {
                this.CustomColorsChanged(sender, e);
            }
        }
        #endregion
    } 
    #endregion

    #region class CustomPanelColors : CustomColors
    public class CustomPanelColors : CustomColors
    {
        #region FieldsPrivate
        private Color m_captionSelectedGradientBegin = System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientBegin;
        private Color m_captionSelectedGradientEnd = System.Windows.Forms.ProfessionalColors.ButtonSelectedGradientEnd;
        private Color m_collapsedCaptionText = SystemColors.ControlText;
        private Color m_contentGradientBegin = System.Windows.Forms.ProfessionalColors.ToolStripContentPanelGradientBegin;
        private Color m_contentGradientEnd = System.Windows.Forms.ProfessionalColors.ToolStripContentPanelGradientEnd;
        #endregion

        #region Properties

        [Description("The starting color of the hover icon in the captionbar on the Panel.")]
        public virtual Color CaptionSelectedGradientBegin
        {
            get { return this.m_captionSelectedGradientBegin; }
            set
            {
                if (value.Equals(this.m_captionSelectedGradientBegin) == false)
                {
                    this.m_captionSelectedGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the hover icon in the captionbar on the Panel.")]
        public virtual Color CaptionSelectedGradientEnd
        {
            get { return this.m_captionSelectedGradientEnd; }
            set
            {
                if (value.Equals(this.m_captionSelectedGradientEnd) == false)
                {
                    this.m_captionSelectedGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The text color of a Panel when it's collapsed.")]
        public virtual Color CollapsedCaptionText
        {
            get { return this.m_collapsedCaptionText; }
            set
            {
                if (value.Equals(this.m_collapsedCaptionText) == false)
                {
                    this.m_collapsedCaptionText = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The starting color of the gradient used in the Panel.")]
        public virtual Color ContentGradientBegin
        {
            get { return this.m_contentGradientBegin; }
            set
            {
                if (value.Equals(this.m_contentGradientBegin) == false)
                {
                    this.m_contentGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the gradient used in the Panel.")]
        public virtual Color ContentGradientEnd
        {
            get { return this.m_contentGradientEnd; }
            set
            {
                if (value.Equals(this.m_contentGradientEnd) == false)
                {
                    this.m_contentGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion
    } 
    #endregion

    #region class CustomExpondPanelColors : CustomColors
    public class CustomExpondPanelColors : CustomColors
    {
        #region FieldsPrivate
        private Color _backColor = SystemColors.Control;
        private Color _flatCaptionGradientBegin = ProfessionalColors.ToolStripGradientMiddle;
        private Color _flatCaptionGradientEnd = ProfessionalColors.ToolStripGradientBegin;
        private Color _captionPressedGradientBegin = ProfessionalColors.ButtonPressedGradientBegin;
        private Color _captionPressedGradientEnd = ProfessionalColors.ButtonPressedGradientEnd;
        private Color _captionPressedGradientMiddle = ProfessionalColors.ButtonPressedGradientMiddle;
        private Color _captionCheckedGradientBegin = ProfessionalColors.ButtonCheckedGradientBegin;
        private Color _captionCheckedGradientEnd = ProfessionalColors.ButtonCheckedGradientEnd;
        private Color _captionCheckedGradientMiddle = ProfessionalColors.ButtonCheckedGradientMiddle;
        private Color _captionSelectedGradientBegin = ProfessionalColors.ButtonSelectedGradientBegin;
        private Color _captionSelectedGradientEnd = ProfessionalColors.ButtonSelectedGradientEnd;
        private Color _captionSelectedGradientMiddle = ProfessionalColors.ButtonSelectedGradientMiddle;
        private Color _captionSelectedText = SystemColors.ControlText;
        #endregion

        #region Properties

        [Description("The backcolor of a ExpondPanel.")]
        public virtual Color BackColor
        {
            get { return this._backColor; }
            set
            {
                if (value.Equals(this._backColor) == false)
                {
                    this._backColor = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The starting color of the gradient on a flat ExpondPanel captionbar.")]
        public virtual Color FlatCaptionGradientBegin
        {
            get { return this._flatCaptionGradientBegin; }
            set
            {
                if (value.Equals(this._flatCaptionGradientBegin) == false)
                {
                    this._flatCaptionGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the gradient on a flat ExpondPanel captionbar.")]
        public virtual Color FlatCaptionGradientEnd
        {
            get { return this._flatCaptionGradientEnd; }
            set
            {
                if (value.Equals(this._flatCaptionGradientEnd) == false)
                {
                    this._flatCaptionGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The starting color of the gradient used when the ExpondPanel is pressed down.")]
        public virtual Color CaptionPressedGradientBegin
        {
            get { return this._captionPressedGradientBegin; }
            set
            {
                if (value.Equals(this._captionPressedGradientBegin) == false)
                {
                    this._captionPressedGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the gradient used when the ExpondPanel is pressed down.")]
        public virtual Color CaptionPressedGradientEnd
        {
            get { return this._captionPressedGradientEnd; }
            set
            {
                if (value.Equals(this._captionPressedGradientEnd) == false)
                {
                    this._captionPressedGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The middle color of the gradient used when the ExpondPanel is pressed down.")]
        public virtual Color CaptionPressedGradientMiddle
        {
            get { return this._captionPressedGradientMiddle; }
            set
            {
                if (value.Equals(this._captionPressedGradientMiddle) == false)
                {
                    this._captionPressedGradientMiddle = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The starting color of the gradient used when the ExpondPanel is checked.")]
        public virtual Color CaptionCheckedGradientBegin
        {
            get { return this._captionCheckedGradientBegin; }
            set
            {
                if (value.Equals(this._captionCheckedGradientBegin) == false)
                {
                    this._captionCheckedGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the gradient used when the ExpondPanel is checked.")]
        public virtual Color CaptionCheckedGradientEnd
        {
            get { return this._captionCheckedGradientEnd; }
            set
            {
                if (value.Equals(this._captionCheckedGradientEnd) == false)
                {
                    this._captionCheckedGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The middle color of the gradient used when the ExpondPanel is checked.")]
        public virtual Color CaptionCheckedGradientMiddle
        {
            get { return this._captionCheckedGradientMiddle; }
            set
            {
                if (value.Equals(this._captionCheckedGradientMiddle) == false)
                {
                    this._captionCheckedGradientMiddle = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The starting color of the gradient used when the ExpondPanel is selected.")]
        public virtual Color CaptionSelectedGradientBegin
        {
            get { return this._captionSelectedGradientBegin; }
            set
            {
                if (value.Equals(this._captionSelectedGradientBegin) == false)
                {
                    this._captionSelectedGradientBegin = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The end color of the gradient used when the ExpondPanel is selected.")]
        public virtual Color CaptionSelectedGradientEnd
        {
            get { return this._captionSelectedGradientEnd; }
            set
            {
                if (value.Equals(this._captionSelectedGradientEnd) == false)
                {
                    this._captionSelectedGradientEnd = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The middle color of the gradient used when the ExpondPanel is selected.")]
        public virtual Color CaptionSelectedGradientMiddle
        {
            get { return this._captionSelectedGradientMiddle; }
            set
            {
                if (value.Equals(this._captionSelectedGradientMiddle) == false)
                {
                    this._captionSelectedGradientMiddle = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }

        [Description("The text color used when the ExpondPanel is selected.")]
        public virtual Color CaptionSelectedText
        {
            get { return this._captionSelectedText; }
            set
            {
                if (value.Equals(this._captionSelectedText) == false)
                {
                    this._captionSelectedText = value;
                    OnCustomColorsChanged(this, EventArgs.Empty);
                }
            }
        }
        #endregion
    } 
    #endregion
}
