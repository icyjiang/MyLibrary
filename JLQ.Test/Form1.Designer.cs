namespace JLQ.Test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNum = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtRes = new System.Windows.Forms.TextBox();
            this.multiLineComboBox1 = new JLQ.Common.MultiLineComboBox();
            this.SuspendLayout();
            // 
            // txtNum
            // 
            this.txtNum.Location = new System.Drawing.Point(29, 28);
            this.txtNum.Name = "txtNum";
            this.txtNum.Size = new System.Drawing.Size(100, 21);
            this.txtNum.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(135, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtRes
            // 
            this.txtRes.Location = new System.Drawing.Point(29, 64);
            this.txtRes.Multiline = true;
            this.txtRes.Name = "txtRes";
            this.txtRes.Size = new System.Drawing.Size(269, 164);
            this.txtRes.TabIndex = 2;
            // 
            // multiLineComboBox1
            // 
            this.multiLineComboBox1.AcceptsReturn = true;
            this.multiLineComboBox1.AutoScroll = true;
            this.multiLineComboBox1.DrawMode = System.Windows.Forms.DrawMode.Normal;
            this.multiLineComboBox1.DropDownStyle = JLQ.Common.MultiLineComboBoxStyle.DropDown;
            this.multiLineComboBox1.FadeEffect = JLQ.Common.MultiLineComboBoxEffect.Standard;
            this.multiLineComboBox1.Location = new System.Drawing.Point(575, 145);
            this.multiLineComboBox1.Name = "multiLineComboBox1";
            this.multiLineComboBox1.Size = new System.Drawing.Size(245, 34);
            this.multiLineComboBox1.SlidingDropDown = JLQ.Common.MultiLineComboBoxEffect.Standard;
            this.multiLineComboBox1.Style = JLQ.Common.MultiLineComboBoxAppearance.Standard;
            this.multiLineComboBox1.TabIndex = 3;
            this.multiLineComboBox1.Text = "multiLineComboBox1";
            this.multiLineComboBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.multiLineComboBox1.TextVerticalAlign = System.Windows.Forms.VisualStyles.VerticalAlignment.Top;
            // 
            // Form1
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 413);
            this.Controls.Add(this.multiLineComboBox1);
            this.Controls.Add(this.txtNum);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtRes);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtNum;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtRes;
        private Common.MultiLineComboBox multiLineComboBox1;

    }
}

