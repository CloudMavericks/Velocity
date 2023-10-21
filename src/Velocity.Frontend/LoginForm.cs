using System.Net.Http.Json;
using Velocity.Frontend.Extensions;
using Velocity.Frontend.Http;
using Velocity.Shared.Requests;
using Velocity.Shared.Responses;

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

    private async void button1_Click(object sender, EventArgs e)
    {
        if (_validated)
        {
            try
            {
                var request = new LoginRequest()
                {
                    UserName = textBox1.Text,
                    Password = textBox2.Text
                };
                var client = HttpApi.GetClient();
                var response = await client.PostAsJsonAsync("api/login", request);
                response.EnsureSuccessStatusCode();
                if(response.IsSuccessStatusCode)
                {
                    _validated = true;
                    var result = await response.ToResult<LoginResponse>();
                    if(result.Succeeded)
                    {
                        this.Hide();
                        new MainForm().Show();
                    }
                    else
                    {
                        throw new Exception(result.Messages.FirstOrDefault());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        else
        {
            MessageBox.Show("Enter validities");
        }
    }
}
