using System;
using System.Diagnostics;
using System.Windows.Forms;
using WinformDojo.Dialogs;

namespace WinformDojo;

public partial class DojoForm : Form
{
    private readonly DojoConfig config;
    public DojoForm()
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

    private void BtnGifSplitterCallback(object sender, EventArgs e)
    {
        using (DlgGifSplitter dlg = new DlgGifSplitter())
            dlg.ShowDialog();
    }

    private void BtnTrayiconCallback(object sender, EventArgs e)
    {
        using (DlgTrayicon dlg = new DlgTrayicon())
            dlg.ShowDialog();
    }

    private void BtnPluginCallback(object sender, EventArgs e)
    {
        using (DlgSpeakers dlg = new DlgSpeakers())
            dlg.ShowDialog();
    }

    private void BtnAssemblyVerifierCallback(object sender, EventArgs e)
    {
        string assemblyPath = null;
        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Title = "Select a assembly file to verify...";
            ofd.Filter = "dynamic link library|*.dll|executable file|*.exe|ALL|*.*";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            assemblyPath = ofd.FileName;
        }
        Debug.WriteLine($"[dbg] choosed '{assemblyPath}'.");

        // TODO: 檢驗
    }
}
