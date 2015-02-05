using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using JLQ.Common;

namespace JLQ.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //AutoClosedMsgBox.Show("嘿嘿", "aaa", 2000);
            //AutoClosedMsgBox.Show("aaaa", "xxx", 2000, MsgBoxStyle.BlueInfo_OK);
            //int input = int.Parse(txtNum.Text.Trim());
            //string s = "";
            //foreach (var item in MyFSharp.MathPrime.GetPrime(input))
            //    s += " * " + item;
            //if (!string.IsNullOrWhiteSpace(s)) s = s.Remove(0, 3);

            //string s1 = "";
            //foreach (var item in MyFSharp.MathPrime.GetDivisors(input))
            //    s1 += ", " + item;
            //if (!string.IsNullOrWhiteSpace(s1)) s1 = s1.Remove(0, 2);
            //txtRes.Text = string.Format("{0} = {1}\r\n{0}的因数有：{2}", input, s, s1);
        }
    }
}
