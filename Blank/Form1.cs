namespace Blank
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text;
            textBox3.Text = textBox1.Text;
            textBox4.Text = textBox1.Text;
            textBox5.Text = textBox1.Text;
            textBox6.Text = textBox1.Text;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            textBox9.Text = textBox8.Text;
            textBox10.Text = textBox8.Text;
            textBox11.Text = textBox8.Text;
            textBox12.Text = textBox8.Text;
            textBox13.Text = textBox8.Text;
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox5.Text = guna2TextBox1.Text;
            guna2TextBox6.Text = guna2TextBox1.Text;
            guna2TextBox7.Text = guna2TextBox1.Text;
            guna2TextBox8.Text = guna2TextBox1.Text;
            guna2TextBox4.Text = guna2TextBox1.Text;
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {
            guna2TextBox3.Text = guna2TextBox2.Text;
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox4.Text = comboBox1.Text;
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            comboBox3.Text = comboBox2.Text;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            textBox19.Text = textBox14.Text;
        }
    }
}
