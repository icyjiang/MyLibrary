using System;
using System.Collections.Generic;
using System.Text;

namespace JLQ.Common
{
    #region enum CaptionStyle
    public enum CaptionStyle
    {
        /// <summary>
        ///  The normal style of a caption.
        /// </summary>
        Normal,
        /// <summary>
        /// The flat style of a caption.
        /// </summary>
        Flat
    }
    #endregion

    #region enum ColorScheme
    public enum ColorScheme
    {
        /// <summary>
        /// Draws the panels caption with <see cref="System.Windows.Forms.ProfessionalColors">ProfessionalColors</see>
        /// </summary>
        Professional,
        /// <summary>
        /// Draws the panels caption with custom colors.
        /// </summary>
        Custom
    }
    #endregion

    #region enum HoverState
    public enum HoverState
    {
        /// <summary>
        /// The hoverstate in its normal state (none of the other states apply).
        /// </summary>
        None,
        /// <summary>
        /// The hoverstate over which a mouse pointer is resting.
        /// </summary>
        Hover
    }
    #endregion

    #region enum PanelStyle
    public enum PanelStyle
    {
        /// <summary>
        /// Draws the panels caption in the default office 2003 style.
        /// </summary>
        Default,
        /// <summary>
        /// Draws the panels caption in the office 2007 style.
        /// </summary>
        Office2007,

        Black,

        Blue,

        Office2007Black,

        Office2007Silver,
    }


    //#region enum PanelStyle
    //public enum PanelStyle
    //{
    //    /// <summary>
    //    /// Draws the panels caption in the default office 2003 style.
    //    /// </summary>
    //    Default,
    //    /// <summary>
    //    /// Draws the panels caption in the office 2007 style.
    //    /// </summary>
    //    Office2007,
    //} 
    #endregion
}
