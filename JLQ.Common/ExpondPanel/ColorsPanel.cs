
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace JLQ.Common
{
    #region class PanelColors
    public class PanelColors
    {
        #region Enums
        public enum KnownColors
        {
            BorderColor,
            PanelCaptionCloseIcon,
            PanelCaptionExpandIcon,
            PanelCaptionGradientBegin,
            PanelCaptionGradientEnd,
            PanelCaptionGradientMiddle,
            PanelCaptionSelectedGradientBegin,
            PanelCaptionSelectedGradientEnd,
            PanelContentGradientBegin,
            PanelContentGradientEnd,
            PanelCaptionText,
            PanelCollapsedCaptionText,
            InnerBorderColor,
            ExpondPanelBackColor,
            ExpondPanelCaptionCloseIcon,
            ExpondPanelCaptionExpandIcon,
            ExpondPanelCaptionText,
            ExpondPanelCaptionGradientBegin,
            ExpondPanelCaptionGradientEnd,
            ExpondPanelCaptionGradientMiddle,
            ExpondPanelFlatCaptionGradientBegin,
            ExpondPanelFlatCaptionGradientEnd,
            ExpondPanelPressedCaptionBegin,
            ExpondPanelPressedCaptionEnd,
            ExpondPanelPressedCaptionMiddle,
            ExpondPanelCheckedCaptionBegin,
            ExpondPanelCheckedCaptionEnd,
            ExpondPanelCheckedCaptionMiddle,
            ExpondPanelSelectedCaptionBegin,
            ExpondPanelSelectedCaptionEnd,
            ExpondPanelSelectedCaptionMiddle,
            ExpondPanelSelectedCaptionText
        }
        #endregion

        #region 字段private
        private BasePanel _basePanel;
        private ProfessionalColorTable _professionalColorTable;
        private Dictionary<KnownColors, Color> _dictionaryRGBTable;
        private bool _useSystemColors;
        #endregion

        #region 属性virtual
        public virtual Color BorderColor
        {
            get { return this.FromKnownColor(KnownColors.BorderColor); }
        }
        public virtual Color PanelCaptionCloseIcon
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionCloseIcon); }
        }
        public virtual Color PanelCaptionExpandIcon
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionExpandIcon); }
        }
        public virtual Color PanelCaptionGradientBegin
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionGradientBegin); }
        }
        public virtual Color PanelCaptionGradientEnd
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionGradientEnd); }
        }
        public virtual Color PanelCaptionGradientMiddle
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionGradientMiddle); }
        }
        public virtual Color PanelCaptionSelectedGradientBegin
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionSelectedGradientBegin); }
        }
        public virtual Color PanelCaptionSelectedGradientEnd
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionSelectedGradientEnd); }
        }
        public virtual Color PanelCaptionText
        {
            get { return this.FromKnownColor(KnownColors.PanelCaptionText); }
        }
        public virtual Color PanelCollapsedCaptionText
        {
            get { return this.FromKnownColor(KnownColors.PanelCollapsedCaptionText); }
        }
        public virtual Color PanelContentGradientBegin
        {
            get { return this.FromKnownColor(KnownColors.PanelContentGradientBegin); }
        }
        public virtual Color PanelContentGradientEnd
        {
            get { return this.FromKnownColor(KnownColors.PanelContentGradientEnd); }
        }
        public virtual Color InnerBorderColor
        {
            get { return this.FromKnownColor(KnownColors.InnerBorderColor); }
        }
        public virtual Color ExpondPanelBackColor
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelBackColor); }
        }
        public virtual Color ExpondPanelCaptionCloseIcon
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCaptionCloseIcon); }
        }
        public virtual Color ExpondPanelCaptionExpandIcon
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCaptionExpandIcon); }
        }
        public virtual Color ExpondPanelCaptionGradientBegin
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCaptionGradientBegin); }
        }
        public virtual Color ExpondPanelCaptionGradientEnd
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCaptionGradientEnd); }
        }
        public virtual Color ExpondPanelCaptionGradientMiddle
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCaptionGradientMiddle); }
        }
        public virtual Color ExpondPanelCaptionText
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCaptionText); }
        }
        public virtual Color ExpondPanelFlatCaptionGradientBegin
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelFlatCaptionGradientBegin); }
        }
        public virtual Color ExpondPanelFlatCaptionGradientEnd
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelFlatCaptionGradientEnd); }
        }
        public virtual Color ExpondPanelPressedCaptionBegin
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelPressedCaptionBegin); }
        }
        public virtual Color ExpondPanelPressedCaptionEnd
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelPressedCaptionEnd); }
        }
        public virtual Color ExpondPanelPressedCaptionMiddle
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelPressedCaptionMiddle); }
        }
        public virtual Color ExpondPanelCheckedCaptionBegin
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCheckedCaptionBegin); }
        }
        public virtual Color ExpondPanelCheckedCaptionEnd
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCheckedCaptionEnd); }
        }
        public virtual Color ExpondPanelCheckedCaptionMiddle
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelCheckedCaptionMiddle); }
        }
        public virtual Color ExpondPanelSelectedCaptionBegin
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelSelectedCaptionBegin); }
        }
        public virtual Color ExpondPanelSelectedCaptionEnd
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelSelectedCaptionEnd); }
        }
        public virtual Color ExpondPanelSelectedCaptionMiddle
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelSelectedCaptionMiddle); }
        }
        public virtual Color ExpondPanelSelectedCaptionText
        {
            get { return this.FromKnownColor(KnownColors.ExpondPanelSelectedCaptionText); }
        }
        public virtual PanelStyle PanelStyle
        {
            get { return PanelStyle.Default; }
        }
        public bool UseSystemColors
        {
            get { return this._useSystemColors; }
            set
            {
                if (value.Equals(this._useSystemColors) == false)
                {
                    this._useSystemColors = value;
                    this._professionalColorTable.UseSystemColors = this._useSystemColors;
                    Clear();
                }
            }
        }
        public BasePanel Panel
        {
            get { return this._basePanel; }
            set { this._basePanel = value; }
        }
        internal Color FromKnownColor(KnownColors color)
        {
            return (Color)this.ColorTable[color];
        }
        private Dictionary<KnownColors, Color> ColorTable
        {
            get
            {
                if (this._dictionaryRGBTable == null)
                {
                    this._dictionaryRGBTable = new Dictionary<KnownColors, Color>(0xd4);
                    if ((this._basePanel != null) && (this._basePanel.ColorScheme == ColorScheme.Professional))
                    {
                        if ((this._useSystemColors == true) || (ToolStripManager.VisualStylesEnabled == false))
                        {
                            InitBaseColors(this._dictionaryRGBTable);
                        }
                        else
                        {
                            InitColors(this._dictionaryRGBTable);
                        }
                    }
                    else
                    {
                        InitCustomColors(this._dictionaryRGBTable);
                    }
                }
                return this._dictionaryRGBTable;
            }
        }
        #endregion

        #region 构造函数
        public PanelColors()
        {
            this._professionalColorTable = new System.Windows.Forms.ProfessionalColorTable();
        }
        public PanelColors(BasePanel basePanel) : this() { this._basePanel = basePanel; }
        public void Clear()
        {
            ResetRGBTable();
        }
        #endregion

        #region 函数protected virtual
        protected virtual void InitColors(Dictionary<KnownColors, Color> rgbTable)
        {
            InitBaseColors(rgbTable);
        }
        #endregion

        #region 函数private

        private void InitBaseColors(Dictionary<KnownColors, Color> rgbTable)
        {
            rgbTable[KnownColors.BorderColor] = this._professionalColorTable.GripDark;
            rgbTable[KnownColors.InnerBorderColor] = this._professionalColorTable.GripLight;
            rgbTable[KnownColors.PanelCaptionCloseIcon] = SystemColors.ControlText;
            rgbTable[KnownColors.PanelCaptionExpandIcon] = SystemColors.ControlText;
            rgbTable[KnownColors.PanelCaptionGradientBegin] = this._professionalColorTable.ToolStripGradientBegin;
            rgbTable[KnownColors.PanelCaptionGradientEnd] = this._professionalColorTable.ToolStripGradientEnd;
            rgbTable[KnownColors.PanelCaptionGradientMiddle] = this._professionalColorTable.ToolStripGradientMiddle;
            rgbTable[KnownColors.PanelCaptionSelectedGradientBegin] = this._professionalColorTable.ButtonSelectedGradientBegin;
            rgbTable[KnownColors.PanelCaptionSelectedGradientEnd] = this._professionalColorTable.ButtonSelectedGradientEnd;
            rgbTable[KnownColors.PanelContentGradientBegin] = this._professionalColorTable.ToolStripContentPanelGradientBegin;
            rgbTable[KnownColors.PanelContentGradientEnd] = this._professionalColorTable.ToolStripContentPanelGradientEnd;
            rgbTable[KnownColors.PanelCaptionText] = SystemColors.ControlText;
            rgbTable[KnownColors.PanelCollapsedCaptionText] = SystemColors.ControlText;
            rgbTable[KnownColors.ExpondPanelBackColor] = this._professionalColorTable.ToolStripContentPanelGradientBegin;
            rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = SystemColors.ControlText;
            rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = SystemColors.ControlText;
            rgbTable[KnownColors.ExpondPanelCaptionText] = SystemColors.ControlText;
            rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = this._professionalColorTable.ToolStripGradientBegin;
            rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = this._professionalColorTable.ToolStripGradientEnd;
            rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = this._professionalColorTable.ToolStripGradientMiddle;
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = this._professionalColorTable.ToolStripGradientMiddle;
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = this._professionalColorTable.ToolStripGradientBegin;
            rgbTable[KnownColors.ExpondPanelPressedCaptionBegin] = this._professionalColorTable.ButtonPressedGradientBegin;
            rgbTable[KnownColors.ExpondPanelPressedCaptionEnd] = this._professionalColorTable.ButtonPressedGradientEnd;
            rgbTable[KnownColors.ExpondPanelPressedCaptionMiddle] = this._professionalColorTable.ButtonPressedGradientMiddle;
            rgbTable[KnownColors.ExpondPanelCheckedCaptionBegin] = this._professionalColorTable.ButtonCheckedGradientBegin;
            rgbTable[KnownColors.ExpondPanelCheckedCaptionEnd] = this._professionalColorTable.ButtonCheckedGradientEnd;
            rgbTable[KnownColors.ExpondPanelCheckedCaptionMiddle] = this._professionalColorTable.ButtonCheckedGradientMiddle;
            rgbTable[KnownColors.ExpondPanelSelectedCaptionBegin] = this._professionalColorTable.ButtonSelectedGradientBegin;
            rgbTable[KnownColors.ExpondPanelSelectedCaptionEnd] = this._professionalColorTable.ButtonSelectedGradientEnd;
            rgbTable[KnownColors.ExpondPanelSelectedCaptionMiddle] = this._professionalColorTable.ButtonSelectedGradientMiddle;
            rgbTable[KnownColors.ExpondPanelSelectedCaptionText] = SystemColors.ControlText;
        }

        private void InitCustomColors(Dictionary<KnownColors, Color> rgbTable)
        {
            PanelEx panel = this._basePanel as PanelEx;
            if (panel != null)
            {
                rgbTable[KnownColors.BorderColor] = panel.CustomColors.BorderColor;
                rgbTable[KnownColors.InnerBorderColor] = panel.CustomColors.InnerBorderColor;
                rgbTable[KnownColors.PanelCaptionCloseIcon] = panel.CustomColors.CaptionCloseIcon;
                rgbTable[KnownColors.PanelCaptionExpandIcon] = panel.CustomColors.CaptionExpandIcon;
                rgbTable[KnownColors.PanelCaptionGradientBegin] = panel.CustomColors.CaptionGradientBegin;
                rgbTable[KnownColors.PanelCaptionGradientEnd] = panel.CustomColors.CaptionGradientEnd;
                rgbTable[KnownColors.PanelCaptionGradientMiddle] = panel.CustomColors.CaptionGradientMiddle;
                rgbTable[KnownColors.PanelCaptionSelectedGradientBegin] = panel.CustomColors.CaptionSelectedGradientBegin;
                rgbTable[KnownColors.PanelCaptionSelectedGradientEnd] = panel.CustomColors.CaptionSelectedGradientEnd;
                rgbTable[KnownColors.PanelContentGradientBegin] = panel.CustomColors.ContentGradientBegin;
                rgbTable[KnownColors.PanelContentGradientEnd] = panel.CustomColors.ContentGradientEnd;
                rgbTable[KnownColors.PanelCaptionText] = panel.CustomColors.CaptionText;
                rgbTable[KnownColors.PanelCollapsedCaptionText] = panel.CustomColors.CollapsedCaptionText;
            }

            ExpondPanel expondPanel = this._basePanel as ExpondPanel;
            if (expondPanel != null)
            {
                rgbTable[KnownColors.BorderColor] = expondPanel.CustomColors.BorderColor;
                rgbTable[KnownColors.InnerBorderColor] = expondPanel.CustomColors.InnerBorderColor;
                rgbTable[KnownColors.ExpondPanelBackColor] = expondPanel.CustomColors.BackColor;
                rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = expondPanel.CustomColors.CaptionCloseIcon;
                rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = expondPanel.CustomColors.CaptionExpandIcon;
                rgbTable[KnownColors.ExpondPanelCaptionText] = expondPanel.CustomColors.CaptionText;
                rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = expondPanel.CustomColors.CaptionGradientBegin;
                rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = expondPanel.CustomColors.CaptionGradientEnd;
                rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = expondPanel.CustomColors.CaptionGradientMiddle;
                rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = expondPanel.CustomColors.FlatCaptionGradientBegin;
                rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = expondPanel.CustomColors.FlatCaptionGradientEnd;
                rgbTable[KnownColors.ExpondPanelPressedCaptionBegin] = expondPanel.CustomColors.CaptionPressedGradientBegin;
                rgbTable[KnownColors.ExpondPanelPressedCaptionEnd] = expondPanel.CustomColors.CaptionPressedGradientEnd;
                rgbTable[KnownColors.ExpondPanelPressedCaptionMiddle] = expondPanel.CustomColors.CaptionPressedGradientMiddle;
                rgbTable[KnownColors.ExpondPanelCheckedCaptionBegin] = expondPanel.CustomColors.CaptionCheckedGradientBegin;
                rgbTable[KnownColors.ExpondPanelCheckedCaptionEnd] = expondPanel.CustomColors.CaptionCheckedGradientEnd;
                rgbTable[KnownColors.ExpondPanelCheckedCaptionMiddle] = expondPanel.CustomColors.CaptionCheckedGradientMiddle;
                rgbTable[KnownColors.ExpondPanelSelectedCaptionBegin] = expondPanel.CustomColors.CaptionSelectedGradientBegin;
                rgbTable[KnownColors.ExpondPanelSelectedCaptionEnd] = expondPanel.CustomColors.CaptionSelectedGradientEnd;
                rgbTable[KnownColors.ExpondPanelSelectedCaptionMiddle] = expondPanel.CustomColors.CaptionSelectedGradientMiddle;
                rgbTable[KnownColors.ExpondPanelSelectedCaptionText] = expondPanel.CustomColors.CaptionSelectedText;
            }
        }

        private void ResetRGBTable()
        {
            if (this._dictionaryRGBTable != null)
            {
                this._dictionaryRGBTable.Clear();
            }
            this._dictionaryRGBTable = null;
        }

        #endregion
    }
    #endregion

    #region class PanelColorsBse : PanelColors
    public class PanelColorsBse : PanelColors
    {
        #region 属性重写
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Office2007; }
        }
        #endregion

        #region 构造函数
        public PanelColorsBse() : base() { }
        public PanelColorsBse(BasePanel basePanel) : base(basePanel) { }
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<KnownColors, System.Drawing.Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.PanelCaptionSelectedGradientBegin] = Color.FromArgb(156, 163, 254);
            rgbTable[KnownColors.PanelCaptionSelectedGradientEnd] = Color.FromArgb(90, 98, 254);
            rgbTable[KnownColors.ExpondPanelCheckedCaptionBegin] = Color.FromArgb(136, 144, 254);
            rgbTable[KnownColors.ExpondPanelCheckedCaptionEnd] = Color.FromArgb(111, 145, 255);
            rgbTable[KnownColors.ExpondPanelCheckedCaptionMiddle] = Color.FromArgb(42, 52, 254);
            rgbTable[KnownColors.ExpondPanelPressedCaptionBegin] = Color.FromArgb(106, 109, 228);
            rgbTable[KnownColors.ExpondPanelPressedCaptionEnd] = Color.FromArgb(88, 111, 226);
            rgbTable[KnownColors.ExpondPanelPressedCaptionMiddle] = Color.FromArgb(39, 39, 217);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionBegin] = Color.FromArgb(156, 163, 254);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionEnd] = Color.FromArgb(139, 164, 255);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionMiddle] = Color.FromArgb(90, 98, 254);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionText] = Color.White;
        }
        #endregion
    }
    #endregion

    #region class PanelColorsOffice : PanelColors
    public class PanelColorsOffice : PanelColors
    {
        #region 属性重写
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Office2007; }
        }
        #endregion

        #region 构造函数
        public PanelColorsOffice() : base() { }
        public PanelColorsOffice(BasePanel basePanel) : base(basePanel) { }
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<KnownColors, System.Drawing.Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.PanelCaptionSelectedGradientBegin] = Color.FromArgb(255, 255, 220);
            rgbTable[KnownColors.PanelCaptionSelectedGradientEnd] = Color.FromArgb(247, 193, 94);
            rgbTable[KnownColors.ExpondPanelCheckedCaptionBegin] = Color.FromArgb(255, 217, 170);
            rgbTable[KnownColors.ExpondPanelCheckedCaptionEnd] = Color.FromArgb(254, 225, 122);
            rgbTable[KnownColors.ExpondPanelCheckedCaptionMiddle] = Color.FromArgb(255, 171, 63);
            rgbTable[KnownColors.ExpondPanelPressedCaptionBegin] = Color.FromArgb(255, 189, 105);
            rgbTable[KnownColors.ExpondPanelPressedCaptionEnd] = Color.FromArgb(254, 211, 100);
            rgbTable[KnownColors.ExpondPanelPressedCaptionMiddle] = Color.FromArgb(251, 140, 60);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionBegin] = Color.FromArgb(255, 252, 222);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionEnd] = Color.FromArgb(255, 230, 158);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionMiddle] = Color.FromArgb(255, 215, 103);
            rgbTable[KnownColors.ExpondPanelSelectedCaptionText] = Color.Black;
        }
        #endregion
    }
    #endregion

    #region class PanelColorsBlack : PanelColorsBse
    public class PanelColorsBlack : PanelColorsBse
    {
        #region 属性重写 补充
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Black; }
        }
        #endregion

        #region 构造函数
        public PanelColorsBlack() : base() { }
        public PanelColorsBlack(BasePanel basePanel) : base(basePanel) { } 
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<PanelColors.KnownColors, Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.BorderColor] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.PanelCaptionCloseIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.PanelCaptionExpandIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.PanelCaptionGradientBegin] = Color.FromArgb(122, 122, 122);
            rgbTable[KnownColors.PanelCaptionGradientEnd] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.PanelCaptionGradientMiddle] = Color.FromArgb(80, 80, 80);
            rgbTable[KnownColors.PanelContentGradientBegin] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelContentGradientEnd] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelCaptionText] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.PanelCollapsedCaptionText] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.InnerBorderColor] = Color.FromArgb(185, 185, 185);
            rgbTable[KnownColors.ExpondPanelBackColor] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionText] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = Color.FromArgb(155, 155, 155);
            rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = Color.FromArgb(47, 47, 47);
            rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = Color.FromArgb(90, 90, 90);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = Color.FromArgb(155, 155, 155);
        } 
        #endregion
    }
    #endregion

    #region class PanelColorsBlue : PanelColorsBse
    public class PanelColorsBlue : PanelColorsBse
    {
        #region 属性重写 补充
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Blue; }
        }
        #endregion

        #region 构造函数
        public PanelColorsBlue() : base() { }
        public PanelColorsBlue(BasePanel basePanel) : base(basePanel) { } 
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<PanelColors.KnownColors, Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.BorderColor] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.PanelCaptionCloseIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.PanelCaptionExpandIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.PanelCaptionGradientBegin] = Color.FromArgb(128, 128, 255);
            rgbTable[KnownColors.PanelCaptionGradientEnd] = Color.FromArgb(0, 0, 128);
            rgbTable[KnownColors.PanelCaptionGradientMiddle] = Color.FromArgb(0, 0, 139);
            rgbTable[KnownColors.PanelContentGradientBegin] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelContentGradientEnd] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelCaptionText] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.PanelCollapsedCaptionText] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.InnerBorderColor] = Color.FromArgb(185, 185, 185);
            rgbTable[KnownColors.ExpondPanelBackColor] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionText] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = Color.FromArgb(128, 128, 255);
            rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = Color.FromArgb(98, 98, 205);
            rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = Color.FromArgb(0, 0, 139);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = Color.FromArgb(111, 145, 255);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = Color.FromArgb(188, 205, 254);
        } 
        #endregion
    }
    #endregion

    #region class PanelColorsOffice2007Black : PanelColorsOffice
    public class PanelColorsOffice2007Black : PanelColorsOffice
    {
        #region 属性重写 补充
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Office2007Black; }
        }
        #endregion

        #region 构造函数
        public PanelColorsOffice2007Black() : base() { }
        public PanelColorsOffice2007Black(BasePanel basePanel) : base(basePanel) { }
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<PanelColors.KnownColors, Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.BorderColor] = Color.FromArgb(76, 83, 92);
            rgbTable[KnownColors.InnerBorderColor] = Color.White;
            rgbTable[KnownColors.PanelCaptionCloseIcon] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.PanelCaptionExpandIcon] = Color.FromArgb(101, 104, 112);
            rgbTable[KnownColors.PanelCaptionGradientBegin] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelCaptionGradientEnd] = Color.FromArgb(189, 193, 200);
            rgbTable[KnownColors.PanelCaptionGradientMiddle] = Color.FromArgb(216, 219, 223);
            rgbTable[KnownColors.PanelContentGradientBegin] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelContentGradientEnd] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelCaptionText] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.PanelCollapsedCaptionText] = Color.FromArgb(0, 0, 0);
            rgbTable[KnownColors.ExpondPanelBackColor] = Color.Transparent;
            rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = Color.FromArgb(255, 255, 255);
            rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = Color.FromArgb(101, 104, 112);
            rgbTable[KnownColors.ExpondPanelCaptionText] = Color.FromArgb(55, 60, 67);
            rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = Color.FromArgb(248, 248, 249);
            rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = Color.FromArgb(219, 222, 226);
            rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = Color.FromArgb(200, 204, 209);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = Color.FromArgb(212, 215, 219);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = Color.FromArgb(253, 253, 254);
        } 
        #endregion
    }
    #endregion

    #region class PanelColorsOffice2007Blue : PanelColorsOffice
    public class PanelColorsOffice2007Blue : PanelColorsOffice
    {
        #region 属性重写 补充
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Office2007; }
        }
        #endregion

        #region 构造函数
        public PanelColorsOffice2007Blue() : base() { }
        public PanelColorsOffice2007Blue(BasePanel basePanel) : base(basePanel) { } 
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<PanelColors.KnownColors, Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.BorderColor] = Color.FromArgb(101, 147, 207);
            rgbTable[KnownColors.InnerBorderColor] = Color.White;
            rgbTable[KnownColors.PanelCaptionCloseIcon] = Color.Black;
            rgbTable[KnownColors.PanelCaptionExpandIcon] = Color.FromArgb(21, 66, 139);
            rgbTable[KnownColors.PanelCaptionGradientBegin] = Color.FromArgb(227, 239, 255);
            rgbTable[KnownColors.PanelCaptionGradientEnd] = Color.FromArgb(173, 209, 255);
            rgbTable[KnownColors.PanelCaptionGradientMiddle] = Color.FromArgb(199, 224, 255);
            rgbTable[KnownColors.PanelContentGradientBegin] = Color.FromArgb(227, 239, 255);
            rgbTable[KnownColors.PanelContentGradientEnd] = Color.FromArgb(227, 239, 255);
            rgbTable[KnownColors.PanelCaptionText] = Color.FromArgb(22, 65, 139);
            rgbTable[KnownColors.PanelCollapsedCaptionText] = Color.FromArgb(21, 66, 139);
            rgbTable[KnownColors.ExpondPanelBackColor] = Color.Transparent;
            rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = Color.Black;
            rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = Color.FromArgb(21, 66, 139);
            rgbTable[KnownColors.ExpondPanelCaptionText] = Color.FromArgb(21, 66, 139);
            rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = Color.FromArgb(227, 239, 255);
            rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = Color.FromArgb(199, 224, 255);
            rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = Color.FromArgb(173, 209, 255);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = Color.FromArgb(214, 232, 255);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = Color.FromArgb(253, 253, 254);
        } 
        #endregion
    }
    #endregion

    #region class PanelColorsOffice2007Silver : PanelColorsOffice
    public class PanelColorsOffice2007Silver : PanelColorsOffice
    {
        #region 属性重写 补充
        public override PanelStyle PanelStyle
        {
            get { return PanelStyle.Office2007Silver; }
        }
        #endregion

        #region 构造函数
        public PanelColorsOffice2007Silver() : base() { }
        public PanelColorsOffice2007Silver(BasePanel basePanel) : base(basePanel) { } 
        #endregion

        #region 函数重写
        protected override void InitColors(Dictionary<PanelColors.KnownColors, Color> rgbTable)
        {
            base.InitColors(rgbTable);
            rgbTable[KnownColors.BorderColor] = Color.FromArgb(111, 112, 116);
            rgbTable[KnownColors.InnerBorderColor] = Color.White;
            rgbTable[KnownColors.PanelCaptionCloseIcon] = Color.FromArgb(75, 79, 85);
            rgbTable[KnownColors.PanelCaptionExpandIcon] = Color.FromArgb(101, 104, 112);
            rgbTable[KnownColors.PanelCaptionGradientBegin] = Color.FromArgb(248, 248, 248);
            rgbTable[KnownColors.PanelCaptionGradientEnd] = Color.FromArgb(199, 203, 209);
            rgbTable[KnownColors.PanelCaptionGradientMiddle] = Color.FromArgb(218, 219, 231);
            rgbTable[KnownColors.PanelContentGradientBegin] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelContentGradientEnd] = Color.FromArgb(240, 241, 242);
            rgbTable[KnownColors.PanelCaptionText] = Color.FromArgb(21, 66, 139);
            rgbTable[KnownColors.PanelCollapsedCaptionText] = Color.FromArgb(21, 66, 139);
            rgbTable[KnownColors.ExpondPanelBackColor] = Color.Transparent;
            rgbTable[KnownColors.ExpondPanelCaptionCloseIcon] = Color.FromArgb(75, 79, 85);
            rgbTable[KnownColors.ExpondPanelCaptionExpandIcon] = Color.FromArgb(101, 104, 112);
            rgbTable[KnownColors.ExpondPanelCaptionText] = Color.FromArgb(76, 83, 92);
            rgbTable[KnownColors.ExpondPanelCaptionGradientBegin] = Color.FromArgb(235, 238, 250);
            rgbTable[KnownColors.ExpondPanelCaptionGradientEnd] = Color.FromArgb(212, 216, 226);
            rgbTable[KnownColors.ExpondPanelCaptionGradientMiddle] = Color.FromArgb(197, 199, 209);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientBegin] = Color.FromArgb(213, 219, 231);
            rgbTable[KnownColors.ExpondPanelFlatCaptionGradientEnd] = Color.FromArgb(253, 253, 254);
        } 
        #endregion
    }
    #endregion
}
