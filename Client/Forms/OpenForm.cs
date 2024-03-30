using System.Drawing;
using System.Windows.Forms;
using System.Net.Http;

class OpenForm : Form
{
    string title = null;
    public OpenForm(string[] titles)
    {
        Text = "Open";
        Font = SystemFonts.MessageBoxFont;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MinimizeBox = MaximizeBox = false;
        ClientSize = new(952 / 2, 513 / 2);

        TableLayoutPanel tableLayoutPanel = new() { Dock = DockStyle.Fill };
        Controls.Add(tableLayoutPanel);

        ListBox listBox = new()
        {
            Dock = DockStyle.Fill,
            Margin = new(0)
        };
        foreach (string title in titles)
            listBox.Items.Add(title);

        tableLayoutPanel.RowStyles.Add(new(SizeType.Percent, 89));
        tableLayoutPanel.Controls.Add(listBox);

        Button button = new()
        {
            Text = "Open",
            Anchor = AnchorStyles.Right,
            Enabled = titles.Length != 0
        };
        button.Click += (sender, e) =>
        {
            title = listBox.SelectedItem as string;
            DialogResult = DialogResult.OK;
            Close();
        };
        tableLayoutPanel.RowStyles.Add(new(SizeType.Percent, 11));
        tableLayoutPanel.Controls.Add(button);

        CenterToParent();
    }

    public string Get()
    {
        if (ShowDialog() != DialogResult.OK)
            return string.Empty;
        return title;
    }
}