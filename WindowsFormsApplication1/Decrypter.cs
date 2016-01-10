﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    public partial class Decrypter : Form
    {
        dog x;
        public Decrypter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fbd = new OpenFileDialog();
            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                textBox1.Text = fbd.FileName;
            }      
        }

        private void button2_Click(object sender, EventArgs e)
        {
            x = new dog(textBox1.Text);
            x.startDecryptAction();
            label2.Visible = true;
        }
    }
}
