using System;
using System.Drawing;
using System.Net.Http;
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Net;
using System.Threading;

class MainForm : Form
{
    string title = string.Empty;

    public MainForm(string token)
    {
        Text = "TextPad";
        Font = SystemFonts.MessageBoxFont;
        CenterToScreen();

        TableLayoutPanel tableLayoutPanel = new()
        {
            Dock = DockStyle.Fill,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
        };
        Controls.Add(tableLayoutPanel);

        MenuStrip menuStrip = new();
        tableLayoutPanel.Controls.Add(menuStrip, 0, 0);

        ToolStripButton toolStripButton1 = new() { Text = "New" },
                        toolStripButton2 = new() { Text = "Save" },
                        toolStripButton3 = new() { Text = "Open" };

        menuStrip.Items.Add(toolStripButton1);
        menuStrip.Items.Add(toolStripButton2);
        menuStrip.Items.Add(toolStripButton3);

        TextBox textBox = new()
        {
            Multiline = true,
            Dock = DockStyle.Fill,
            ScrollBars = ScrollBars.Both
        };
        tableLayoutPanel.Controls.Add(textBox, 0, 1);

        toolStripButton1.Click += (sender, e) =>
        {
            title = string.Empty;
            textBox.Text = string.Empty;
        };

        toolStripButton2.Click += (sender, e) =>
        {
            title = new SaveForm(title).Get().Trim();
            if (!string.IsNullOrEmpty(title))
                new Thread(() =>
                {
                    textBox.Enabled = menuStrip.Enabled = false;
                    using HttpResponseMessage httpResponseMessage = Server.Post(new()
                    {
                        ["action"] = "save",
                        ["token"] = token,
                        ["title"] = title,
                        ["content"] = textBox.Text
                    });
                    if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                        MessageBox.Show("Save failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        MessageBox.Show("Save succeed.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox.Enabled = menuStrip.Enabled = true;
                }).Start();
        };

        toolStripButton3.Click += (sender, e) =>
        {
            new Thread(() =>
            {
                textBox.Enabled = menuStrip.Enabled = false;
                HttpResponseMessage httpResponseMessage = Server.Post(new()
                {
                    ["action"] = "titles",
                    ["token"] = token
                });
                title = new OpenForm(new JavaScriptSerializer().Deserialize<string[]>(httpResponseMessage.Content.ReadAsStringAsync().Result)).Get();
                if (!string.IsNullOrEmpty(title))
                {
                    httpResponseMessage.Dispose();
                    httpResponseMessage = Server.Post(new()
                    {
                        ["action"] = "content",
                        ["token"] = token,
                        ["title"] = title
                    });
                    textBox.Text = httpResponseMessage.Content.ReadAsStringAsync().Result;
                }
                textBox.Enabled = menuStrip.Enabled = true;
            }).Start();
            // new OpenForm(default).ShowDialog();
        };
    }
}