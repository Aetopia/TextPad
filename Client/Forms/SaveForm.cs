using System.Drawing;
using System.Windows.Forms;


class SaveForm : Form
{
    readonly TextBox textBox = new() { Dock = DockStyle.Fill };
    readonly string title = null;

    public SaveForm(string title)
    {
        this.title = title;
        Text = "Save";
        Font = SystemFonts.MessageBoxFont;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MinimizeBox = MaximizeBox = false;

        Controls.Add(textBox);

        StatusBar statusBar = new();
        Controls.Add(statusBar);

        Button button = new()
        {
            Text = "Save",
            Dock = DockStyle.Right,
            Enabled = false
        };
        button.Click += (sender, e) =>
        {
            DialogResult = DialogResult.OK;
            Close();
        };
        statusBar.Controls.Add(button);

        textBox.TextChanged += (sender, e) => button.Enabled = textBox.Text.Trim().Length != 0;
        ClientSize = new(textBox.Width, textBox.Height + button.Height);
        CenterToParent();
    }

    public string Get()
    {
        textBox.Text = title;
        if (ShowDialog() != DialogResult.OK)
            return string.Empty;
        return textBox.Text;
    }
}