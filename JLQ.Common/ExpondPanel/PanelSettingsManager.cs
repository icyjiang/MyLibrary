using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace JLQ.Common
{
    public static class PanelSettingsManager
    {
        #region MethodsPublic
        public static void SetPanelProperties(Control.ControlCollection controls, PanelColors panelColors)
        {
            if (panelColors == null)
            {
                throw new ArgumentNullException("panelColors",
                    string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    StaticResource.IDS_ArgumentException,
                    "panelColors"));
            }

            PanelStyle panelStyle = panelColors.PanelStyle;
            SetPanelProperties(controls, panelStyle, panelColors);
        }

        public static void SetPanelProperties(Control.ControlCollection controls, PanelStyle panelStyle, PanelColors panelColors)
        {
            if (panelColors == null)
            {
                throw new ArgumentNullException("panelColors",
                    string.Format(System.Globalization.CultureInfo.InvariantCulture,
                    StaticResource.IDS_ArgumentException,
                    "panelColors"));
            }

			ArrayList panels = FindPanels(true, controls);
            foreach (BasePanel panel in panels)
            {
                panel.PanelStyle = panelStyle;
                panelColors.Panel = panel;
                panel.SetPanelProperties(panelColors);
            }
            ArrayList expondPanelLists = FindPanelLists(true, controls);
            foreach (ExpondPanelList expondPanelList in expondPanelLists)
            {
                expondPanelList.PanelStyle = panelStyle;
                expondPanelList.PanelColors = panelColors;
            }
        }

        public static void SetPanelProperties(Control.ControlCollection controls, PanelStyle panelStyle)
        {
            ArrayList panels = FindPanels(true, controls);
            if (panels != null)
            {
                foreach (BasePanel panel in panels)
                {
                    panel.PanelStyle = panelStyle;
                }
            }
        }

        public static ArrayList FindPanels(bool searchAllChildren, Control.ControlCollection controlsToLookIn)
        {
            return FindControls(typeof(BasePanel), searchAllChildren, controlsToLookIn, new ArrayList());
        }

        public static ArrayList FindPanelLists(bool searchAllChildren, Control.ControlCollection controlsToLookIn)
        {
            return FindControls(typeof(ExpondPanelList), searchAllChildren, controlsToLookIn, new ArrayList());
        }
        #endregion

        #region MethodsPrivate
        private static ArrayList FindControls(Type baseType, bool searchAllChildren, Control.ControlCollection controlsToLookIn, ArrayList foundControls)
        {
            if ((controlsToLookIn == null) || (foundControls == null))
            {
                return null;
            }
            try
            {
                for (int i = 0; i < controlsToLookIn.Count; i++)
                {
                    if ((controlsToLookIn[i] != null) && baseType.IsAssignableFrom(controlsToLookIn[i].GetType()))
                    {
                        foundControls.Add(controlsToLookIn[i]);
                    }
                }
                if (searchAllChildren == false)
                {
                    return foundControls;
                }
                for (int j = 0; j < controlsToLookIn.Count; j++)
                {
                    if (((controlsToLookIn[j] != null) && !(controlsToLookIn[j] is Form)) && ((controlsToLookIn[j].Controls != null) && (controlsToLookIn[j].Controls.Count > 0)))
                    {
                        foundControls = FindControls(baseType, searchAllChildren, controlsToLookIn[j].Controls, foundControls);
                    }
                }
            }
            catch (Exception exception)
            {
                if (IsCriticalException(exception))
                {
                    throw;
                }
            }
            return foundControls;
        }

        private static bool IsCriticalException(Exception exception)
        {
            return (((((exception is NullReferenceException) ||
                (exception is StackOverflowException)) ||
                ((exception is OutOfMemoryException) ||
                (exception is System.Threading.ThreadAbortException))) ||
                ((exception is ExecutionEngineException) ||
                (exception is IndexOutOfRangeException))) ||
                (exception is AccessViolationException));
        }
        #endregion
    }
}
