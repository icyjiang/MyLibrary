using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;

namespace JLQ.Common
{
    //这里存放其他的静态资源：字符串、图像等
    public class StaticResource
    {
        #region 字符串
        internal static string IDS_ArgumentException
        { get { return @"Parameter {0} can't be null"; } }

        internal static string IDS_InvalidBoundArgument
        {
            get
            {
                return @"Value of '{0}' is not valid for '{1}'. 'Value' should be between '{2}' and '{3}'. 
Parameter name: {1}";
            }
        }

        internal static string IDS_InvalidLowBoundArgument
        {
            get
            {
                return @"Value of '{0}' is not valid for '{1}'. 'Maximum' must be greater than or equal to 0.
Parameter name: {1}";
            }
        }

        internal static string IDS_InvalidOperationExceptionInteger
        {
            get
            {
                return @"Value of '{0}' is not valid for '{1}'. '{1}' must be greater than or equal to {2}.
Parameter name: {1}";
            }
        }
        #endregion

        #region Bitmap
        //static Bitmap _downPic;
        internal static Bitmap ChevronDown
        {
            get
            {
                return Properties.Resources.chevrondown;
                //if (_downPic == null)
                //{
                //    string downPicStr = "Qk3GAQAAAAAAADYAAAAoAAAACgAAAAoAAAABACAAAAAAAAAAAAATCwAAEwsAAAAAAAAAAAAA/wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP//pjwA//8A////AP///wD///8A////AP///wD///8A////AP//pjwA/6Y8AP+mPAD//wD///8A////AP///wD///8A////AP//pjwA/6Y8AP//AP//pjwA/6Y8AP//AP///wD///8A////AP//pjwA/6Y8AP//AP///wD///8A//+mPAD/pjwA//8A////AP///wD///8A////AP///wD//6Y8AP//AP///wD///8A////AP///wD///8A////AP///wD//6Y8AP+mPAD/pjwA//8A////AP///wD///8A////AP///wD//6Y8AP+mPAD//wD//6Y8AP+mPAD//wD///8A////AP///wD//6Y8AP+mPAD//wD///8A////AP//pjwA/6Y8AP//AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD//w==";
                //    _downPic = StringToBitmap(downPicStr);
                //}
                //return _downPic;
            }
        }

        //static Bitmap _leftPic;
        internal static Bitmap ChevronLeft
        {
            get
            {
                return Properties.Resources.chevronleft;
                //if (_leftPic == null)
                //{
                //    string leftPicStr = "Qk12AQAAAAAAADYAAAAoAAAACgAAAAoAAAABABgAAAAAAAAAAAATCwAAEwsAAAAAAAAAAAAA/wD//wD//wD//wD//wD//wD//wD//wD//wD//wD/////AP//AP//AP//AP+mPAD/AP//AP//AP+mPAD/AP////8A//8A//8A/6Y8AKY8AP8A//8A/6Y8AKY8AP8A/////wD//wD/pjwApjwA/wD//wD/pjwApjwA/wD//wD/////AP+mPACmPAD/AP//AP+mPACmPAD/AP//AP//AP////8A//8A/6Y8AKY8AP8A//8A/6Y8AKY8AP8A//8A/////wD//wD//wD/pjwApjwA/wD//wD/pjwApjwA/wD/////AP//AP//AP//AP+mPAD/AP//AP//AP+mPAD/AP////8A//8A//8A//8A//8A//8A//8A//8A//8A//8A/////wD//wD//wD//wD//wD//wD//wD//wD//wD//wD///8=";
                //    _leftPic = StringToBitmap(leftPicStr);
                //}
                //return _leftPic;
            }
        }

        //static Bitmap _rightPic;
        internal static Bitmap ChevronRight
        {
            get
            {
                return Properties.Resources.chevronright;
                //if (_rightPic == null)
                //{
                //    string rightPicStr = "Qk3GAQAAAAAAADYAAAAoAAAACgAAAAoAAAABACAAAAAAAAAAAAATCwAAEwsAAAAAAAAAAAAA/wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A//+mPAD//wD///8A////AP//pjwA//8A////AP///wD///8A////AP//pjwA/6Y8AP//AP///wD//6Y8AP+mPAD//wD///8A////AP///wD///8A//+mPAD/pjwA//8A////AP//pjwA/6Y8AP//AP///wD///8A////AP///wD//6Y8AP+mPAD//wD///8A//+mPAD/pjwA//8A////AP///wD//6Y8AP+mPAD//wD///8A//+mPAD/pjwA//8A////AP///wD//6Y8AP+mPAD//wD///8A//+mPAD/pjwA//8A////AP///wD///8A//+mPAD//wD///8A////AP//pjwA//8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD//w==";
                //    _rightPic = StringToBitmap(rightPicStr);
                //}
                //return _rightPic;
            }
        }

        //static Bitmap _upPic;
        internal static Bitmap ChevronUp
        {
            get
            {
                return Properties.Resources.chevronup;
                //if (_upPic == null)
                //{
                //    string upPicStr = "Qk3GAQAAAAAAADYAAAAoAAAACgAAAAoAAAABACAAAAAAAAAAAAATCwAAEwsAAAAAAAAAAAAA/wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP//pjwA/6Y8AP//AP///wD///8A//+mPAD/pjwA//8A////AP///wD///8A//+mPAD/pjwA//8A//+mPAD/pjwA//8A////AP///wD///8A////AP///wD//6Y8AP+mPAD/pjwA//8A////AP///wD///8A////AP///wD///8A////AP//pjwA//8A////AP///wD///8A////AP///wD//6Y8AP+mPAD//wD///8A////AP//pjwA/6Y8AP//AP///wD///8A////AP//pjwA/6Y8AP//AP//pjwA/6Y8AP//AP///wD///8A////AP///wD///8A//+mPAD/pjwA/6Y8AP//AP///wD///8A////AP///wD///8A////AP///wD//6Y8AP//AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD//w==";
                //    _upPic = StringToBitmap(upPicStr);
                //}
                //return _upPic;
            }
        }

        //static Bitmap _closePic;
        internal static Bitmap ClosePanel
        {
            get
            {
                return Properties.Resources.closepanel;
                //if (_closePic == null)
                //{
                //    string closePicStr = "Qk3GAQAAAAAAADYAAAAoAAAACgAAAAoAAAABACAAAAAAAAAAAAATCwAAEwsAAAAAAAAAAAAA/wD///8A////AP///wD///8A////AP///wD///8A////AP///wD///8A//+mPAD/pjwA//8A////AP///wD///8A//+mPAD/pjwA//8A////AP///wD//6Y8AP+mPAD//wD///8A//+mPAD/pjwA//8A////AP///wD///8A////AP//pjwA/6Y8AP+mPAD/pjwA//8A////AP///wD///8A////AP///wD///8A//+mPAD/pjwA//8A////AP///wD///8A////AP///wD///8A////AP//pjwA/6Y8AP//AP///wD///8A////AP///wD///8A////AP//pjwA/6Y8AP+mPAD/pjwA//8A////AP///wD///8A////AP//pjwA/6Y8AP//AP///wD//6Y8AP+mPAD//wD///8A////AP//pjwA/6Y8AP//AP///wD///8A////AP//pjwA/6Y8AP//AP///wD///8A////AP///wD///8A////AP///wD///8A////AP///wD//w==";
                //    _closePic = StringToBitmap(closePicStr);
                //}
                //return _closePic;
            }
        }
        #endregion

        //标头的最小高度
        internal const int CaptionMinHeight = 18;

        //边框宽度
        internal const int BorderThickness = 1;

        //static Bitmap StringToBitmap(string str)
        //{
        //    var byteArrInput = Convert.FromBase64String(str);
        //    if (byteArrInput == null) return null;
        //    using (MemoryStream ms = new MemoryStream(byteArrInput))
        //    {
        //        try { return new Bitmap(ms); }
        //        catch { return new Bitmap(0, 0); }
        //    }
        //}
    }
}
