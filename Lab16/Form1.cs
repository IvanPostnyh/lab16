using System;
using System.Windows.Forms;

namespace Lab_4
{
    public partial class Form1 : Form
    {
        GameTrade Trade = new GameTrade();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            numericUpDown1.Value = (decimal)Trade.BuyPrice1;
            Trade.CalculateSellPrice();
            numericUpDown2.Value = (decimal)Trade.SellPrice1;
            numericUpDown3.Value = (decimal)Trade.QuatityRub1;
            numericUpDown4.Value = (decimal)Trade.QuatityDollar1;
            chart1.Series[0].Points.AddXY(0, Trade.BuyPrice1);
            timer1.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Trade.BuyDollar((double)numericUpDown5.Value,(double)numericUpDown1.Value))
            {
                numericUpDown3.Value = (decimal)Trade.QuatityRub1;
                numericUpDown4.Value = (decimal)Trade.QuatityDollar1;
                textBox1.Text = "Success";
            }
            else textBox1.Text = "UnSuccess";
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (Trade.SellDollar((double)numericUpDown6.Value, (double)numericUpDown2.Value))
            {
                numericUpDown3.Value = (decimal)Trade.QuatityRub1;
                numericUpDown4.Value = (decimal)Trade.QuatityDollar1;
                textBox1.Text = "Success";
            }
            else textBox1.Text = "UnSuccess";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!timer1.Enabled) timer1.Start();
            else timer1.Stop();
        }
        int i = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            Trade.CalculateNextPrice();
            numericUpDown1.Value = (decimal)Trade.BuyPrice1;
            Trade.CalculateSellPrice();
            numericUpDown2.Value = (decimal)Trade.SellPrice1;
            chart1.Series[0].Points.AddXY(i, Trade.BuyPrice1);
            i++;
        }
    }

    class GameTrade
    {
        const double Mu = 0.10d;
        const double Sigma =0.10d;
        double BuyPrice =65.00d;
        double SellPrice;
        double QuatityRub = 10000d;
        double QuatityDollar = 0d;
        double Step = 30d;
        double dt;
        Random rnd = new Random();
        public double BuyPrice1 { get => BuyPrice; set => BuyPrice = value; }
        public double SellPrice1 { get => SellPrice; set => SellPrice = value; }
        public double QuatityRub1 { get => QuatityRub; set => QuatityRub = value; }
        public double QuatityDollar1 { get => QuatityDollar; set => QuatityDollar = value; }
        public void CalculateSellPrice()
        {
            SellPrice1 = BuyPrice - BuyPrice * 2 / 100;
        }
        
        public void CalculateNextPrice()
        {
            BuyPrice += BGM();
        }

        public bool BuyDollar(double quantity, double BuyPrice)
        {
            if (quantity * BuyPrice > QuatityRub) return false;
            else
            {
                QuatityRub -= quantity * BuyPrice;
                QuatityDollar += quantity;
                return true;
            }
        }

        public bool SellDollar(double quantity, double SellPrice)
        {
            if (quantity > QuatityDollar) return false;
            else
            {
                QuatityDollar -= quantity;
                QuatityRub += quantity * SellPrice;
                return true;
            }
        }
        public double BGM()
        {
            dt = 1d / Step;
            double X;
            X = Mu * BuyPrice * dt + Sigma * BuyPrice * IniformDistribution() * Math.Sqrt(dt);
            return X;
        }
        private double IniformDistribution()
        {
            double A1 = rnd.NextDouble();
            double A2 = rnd.NextDouble();
            double Xi = Math.Sqrt(-2.0 * Math.Log(A1)) * Math.Sin(2.0 * Math.PI * A2);
            return Xi;
        }
    }
}
