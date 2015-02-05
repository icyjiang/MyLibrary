using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;


namespace JLQ.Common
{
    public class Singleton : Form
    {

        #region InitializeComponent
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Name = "Form3";
            this.Text = "Form3";
            this.ResumeLayout(false);
        }
        #endregion

        static Singleton _singleton = null;
        private Singleton()
        {
            InitializeComponent();
            //此句会在关闭窗口之后清空_singleton。如果希望下次打开窗口时还原之前的数据，注释此句。
            this.FormClosing += (a, b) => _singleton = null;
        }

        /*
        * 窗口单实例化
        */
        public static Singleton GetSingleton()
        {
            if (_singleton == null || _singleton.IsDisposed)
                _singleton = new Singleton();
            if (Setting.SetForegroundWindow != null)
                Setting.SetForegroundWindow(_singleton.Handle);
            if (_singleton.WindowState == FormWindowState.Minimized)
                _singleton.WindowState = FormWindowState.Normal;
            return _singleton;
        }
    }
}


//调用
/*
private void button2_Click(object sender, EventArgs e)
{
    var form = Singleton.GetSingleton();
    form.Show();
}
 */