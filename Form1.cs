using System;
using System.Windows.Forms;
using WinformDojo.Dialogs;

namespace WinformDojo;

public partial class Form1 : Form
{
    private readonly DojoConfig config;
    public Form1()
    {
        config = new DojoConfig();
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
        using (DlgMultiSvrSwitch dlg = new DlgMultiSvrSwitch(config.ServerNames))
        {
            dlg.ShowDialog();
            config.ServerNames = dlg.ServerInfos;
        }
    }
}
