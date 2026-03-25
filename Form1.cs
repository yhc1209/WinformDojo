using System;
using System.Windows.Forms;

namespace WinformDojo;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void BtnAboutClickCallback(object sender, EventArgs e)
    {
        MessageBox.Show(
            "This is a WinForms DOJO!\nHope you can learn some \"DO\" about WinForms here.",
            "About WinForms DOJO",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information
        );
    }

    private void BtnMultiSvrSelectionCallback(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
