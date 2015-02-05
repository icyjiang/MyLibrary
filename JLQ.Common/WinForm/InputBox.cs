using System;
using System.Windows.Forms;

namespace JLQ.Common
{
    /// <summary>
    /// clsInputBox 的摘要说明。
    /// </summary>
    public class InputBox : System.Windows.Forms.Form
    {
        private System.Windows.Forms.TextBox txtData;
        private Button btnOK;
        private Button btnCancel;
        private System.ComponentModel.Container components = null;

        private InputBox()
        {
            InitializeComponent();
            btnOK.Click += (a, b) => this.Close();
            btnCancel.Click += (a, b) =>
            {
                txtData.Text = string.Empty;
                this.Close();
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            #region InitializeComponent
            this.txtData = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtData
            // 
            this.txtData.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtData.Location = new System.Drawing.Point(21, 15);
            this.txtData.Multiline = true;
            this.txtData.Name = "txtData";
            this.txtData.Size = new System.Drawing.Size(204, 52);
            this.txtData.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(241, 15);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(62, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(241, 44);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(62, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // InputBox
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(325, 82);
            this.ControlBox = false;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "InputBox";
            this.Text = "InputBox";
            this.ResumeLayout(false);
            this.PerformLayout();
            #endregion
        }

        //显示InputBox
        public static string ShowInputBox(string Title, out DialogResult res)
        {
            InputBox inputbox = new InputBox();
            inputbox.Text = Title;
            inputbox.ShowDialog();
            res = inputbox.DialogResult;
            return inputbox.txtData.Text;
        }
    }
}

/*调用实例：
 * DialogResult dr;
 * string inMsg = InputBox.ShowInputBox("输入信息", out dr);
   if (dr != DialogResult.Cancel)
   {
       //对用户的输入信息进行检查
       if (inMsg.Trim() != string.Empty)
           MessageBox.Show(inMsg);
       else
           MessageBox.Show("输入为string.Empty");
   }
*/