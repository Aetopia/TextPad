using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading;
using System.Net;

class LoginForm : Form
{
    string token = null;

    public LoginForm()
    {
        Text = "Login";
        Font = SystemFonts.MessageBoxFont;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MinimizeBox = MaximizeBox = false;
        ClientSize = new(800 / 4, (int)(600 / 4.6));
        CenterToScreen();

        TableLayoutPanel tableLayoutPanel = new() { Dock = DockStyle.Fill };
        Controls.Add(tableLayoutPanel);

        tableLayoutPanel.Controls.Add(new Label
        {
            Text = "Username",
            AutoSize = true,
            TextAlign = ContentAlignment.BottomLeft
        });

        TextBox textBox1 = new() { Dock = DockStyle.Fill };
        tableLayoutPanel.Controls.Add(textBox1);

        tableLayoutPanel.Controls.Add(new Label
        {
            Text = "Password",
            AutoSize = true,
            TextAlign = ContentAlignment.BottomLeft
        });

        TextBox textBox2 = new()
        {
            UseSystemPasswordChar = true,
            Dock = DockStyle.Fill
        };
        tableLayoutPanel.Controls.Add(textBox2);

        StatusBar statusBar = new() { };
        Controls.Add(statusBar);

        Button button1 = new()
        {
            Text = "Login",
            Dock = DockStyle.Right
        };
        statusBar.Controls.Add(button1);
        Button button2 = new()
        {
            Text = "Register",
            Dock = DockStyle.Left
        };
        statusBar.Controls.Add(button2);

        button1.Click += (sender, e) =>
        {
            button1.Enabled = button2.Enabled = false;
            HttpResponseMessage httpResponseMessage = Server.Post(new()
            {
                ["action"] = "login",
                ["username"] = textBox1.Text.Trim(),
                ["password"] = textBox2.Text
            });

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                MessageBox.Show("Login failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
            {
                token = httpResponseMessage.Content.ReadAsStringAsync().Result;
                Close();
            }
            button1.Enabled = button2.Enabled = true;
        };

        button2.Click += (sender, e) =>
        {
            new Thread(() =>
            {
                button1.Enabled = button2.Enabled = false;
                HttpResponseMessage httpResponseMessage = Server.Post(new()
                {
                    ["action"] = "register",
                    ["username"] = textBox1.Text.Trim(),
                    ["password"] = textBox2.Text
                });

                switch (httpResponseMessage.StatusCode)
                {
                    case HttpStatusCode.Forbidden:
                        MessageBox.Show(@"Account already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case HttpStatusCode.BadRequest:
                        MessageBox.Show(@"Account couldn't be created.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case HttpStatusCode.Created:
                        MessageBox.Show(@"Account created.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }

                button1.Enabled = button2.Enabled = true;
            }).Start();
        };
    }

    public string GetToken()
    {
        ShowDialog();
        return token;
    }
}