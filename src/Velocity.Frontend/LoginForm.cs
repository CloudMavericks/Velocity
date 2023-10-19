using System;
using System.Windows.Forms;

namespace Velocity.Frontend;

public partial class LoginForm : Form
{
    public LoginForm()
    {
        InitializeComponent();
    }

    private bool _validated;

    private void LoginForm_Load(object sender, EventArgs e)
    {

    }

    private void label1_Click(object sender, EventArgs e)
    {

    }

    private void button2_Click(object sender, EventArgs e)
    {
        Application.Exit();
    }

    private void textBox1_TextChanged(object sender, EventArgs e)
    {

    }

    private void textBox1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (String.IsNullOrWhiteSpace(textBox1.Text))
        {

            e.Cancel = true;
            textBox1.Focus();
            errorProvider1.SetError(textBox1, "Name should not be left blank...");
            _validated = false;
        }

        else
        {
            e.Cancel = false;
            errorProvider1.SetError(textBox1, "");
            _validated = true;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (_validated)
        {
            this.Hide();
            new MainForm().Show();

        }
        else
        {
            MessageBox.Show("Enter validities");
        }
    }
}
