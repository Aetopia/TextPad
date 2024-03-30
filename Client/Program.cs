using System;
using System.Windows.Forms;
using System.Net.Http;

static class Program
{
  static void Main()
  {
    Application.EnableVisualStyles();
    string token = new LoginForm().GetToken();
    if (token != null)
      new MainForm(token).ShowDialog();
  }
}