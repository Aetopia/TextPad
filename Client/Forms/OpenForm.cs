using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;
using System.Threading;
using System;

class OpenForm : Form
{
    string title = null;
    public OpenForm(string token, string[] titles, Form form)
    {
        Owner = form;
        Text = "Open";
        Font = SystemFonts.MessageBoxFont;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MinimizeBox = MaximizeBox = false;
        ClientSize = new(952 / 2, 513 / 2);

        // TableLayoutPanel tableLayoutPanel = new() { Dock = DockStyle.Fill };
        // Controls.Add(tableLayoutPanel);

        ListBox listBox = new()
        {
            Dock = DockStyle.Fill,
            Margin = new(0)
        };
        foreach (string title in titles)
            listBox.Items.Add(title);

        Controls.Add(listBox);

        StatusBar statusBar = new();
        Controls.Add(statusBar);

        Button button1 = new()
        {
            Text = "Open",
            Dock = DockStyle.Right,
            Enabled = titles.Length != 0
        };
        statusBar.Controls.Add(button1);

        Button button2 = new()
        {
            Text = "Delete",
            Dock = DockStyle.Left,
            Enabled = titles.Length != 0
        };
        statusBar.Controls.Add(button2);

        button1.Click += (sender, e) =>
        {
            title = listBox.SelectedItem as string;
            DialogResult = DialogResult.OK;
            Close();
        };
        button2.Click += (sender, e) => new Thread(() =>
        {
            HttpResponseMessage httpResponseMessage = Server.Post(new()
            {
                ["action"] = "delete",
                ["token"] = token,
                ["title"] = listBox.SelectedItem as string
            });
            listBox.Items.Remove(listBox.SelectedItem);
            button1.Enabled = button2.Enabled = false;
            Console.WriteLine(httpResponseMessage.Content.ReadAsStringAsync().Result);
            httpResponseMessage.Dispose();
            button1.Enabled = button2.Enabled = listBox.Items.Count != 0;
        }).Start();
        CenterToParent();
    }

    public string Get()
    {
        if (ShowDialog() != DialogResult.OK)
            return string.Empty;
        return title;
    }
}