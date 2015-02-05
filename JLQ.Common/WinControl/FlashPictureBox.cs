using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

namespace JLQ.Common
{
    public class FlashPictureBox : PictureBox
    {
        public bool IsFlash { get; set; }
        public Image OriImage { get; set; }
        public Image TransParentImage { get; set; }

        const int timeSpan = 400;//闪烁间歇，单位毫秒
        Thread flash;

        public void Flash(bool isFlash)
        {
            if (isFlash)
            {
                flash = new Thread(new ThreadStart(Method));
                flash.IsBackground = true;
                flash.Start();
            }
            else
            {
                if (flash != null) flash.Abort();
                if (this.IsHandleCreated) this.Invoke(new Action(() => this.Image = OriImage));
            }
            IsFlash = isFlash;
        }

        bool b = false;
        void Method()
        {
            Action ToDo = () =>
            {
                if (!b)
                    this.Image = OriImage;
                else
                    this.Image = TransParentImage;
                b = !b;
            };
            while (true)
            {
                if (this.IsHandleCreated)
                    this.Invoke(ToDo);
                Thread.Sleep(timeSpan);
            }
        }
    }
}
