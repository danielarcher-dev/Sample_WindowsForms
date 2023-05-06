using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simple_Chess_Timer
{
    public partial class Form1 : Form
    {
        decimal counter1 = 0;
        decimal counter2 = 0;

        public Form1()
        {
            InitializeComponent();
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox2.TextAlign = HorizontalAlignment.Center;

            timer1.Interval = 10;
            timer2.Interval = 10;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.BackColor = Color.Green;
            button2.BackColor = Color.Red;
            timer1.Start();
            timer2.Stop();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.BackColor = Color.Green;
            button1.BackColor = Color.Red;
            timer2.Start();
            timer1.Stop();

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            update_textbox1();

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            update_textbox2();

        }

        private void update_textbox1()
        {
            textBox1.Text = counter1.ToString();
            counter1 = counter1 + (decimal)0.01;
        }

        private void update_textbox2()
        {
            counter2 = counter2 + (decimal)0.01;
            textBox2.Text = counter2.ToString();
        }
    }
}
