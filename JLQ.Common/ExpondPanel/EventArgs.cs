using System;
using System.Collections.Generic;
using System.Text;

namespace JLQ.Common
{
    #region class ColorSchemeChangeEventArgs
    public class ColorSchemeChangeEventArgs : EventArgs
    {
        private ColorScheme _colorSchema;
        public ColorScheme ColorSchema
        {
            get { return this._colorSchema; }
        }
        public ColorSchemeChangeEventArgs(ColorScheme colorSchema)
        {
            this._colorSchema = colorSchema;
        }
    } 
    #endregion

    #region class HoverStateChangeEventArgs
    public class HoverStateChangeEventArgs : EventArgs
    {
        private HoverState _hoverState;
        public HoverState HoverState
        {
            get { return this._hoverState; }
        }
        public HoverStateChangeEventArgs(HoverState hoverState)
        {
            this._hoverState = hoverState;
        }
    } 
    #endregion

    #region class PanelStyleChangeEventArgs
    public class PanelStyleChangeEventArgs : EventArgs
    {
        private PanelStyle _panelStyle;
        public PanelStyle PanelStyle
        {
            get { return this._panelStyle; }
        }
        public PanelStyleChangeEventArgs(PanelStyle panelStyle)
        {
            this._panelStyle = panelStyle;
        }
    } 
    #endregion

    #region class ExpondStateChangeEventArgs
    public class ExpondStateChangeEventArgs : EventArgs
    {
        private bool _expand;
        public bool Expand
        {
            get { return _expand; }
        }
        public ExpondStateChangeEventArgs(bool expand)
        {
            this._expand = expand;
        }
    } 
    #endregion
}
