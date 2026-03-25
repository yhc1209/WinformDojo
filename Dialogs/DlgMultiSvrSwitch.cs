using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinformDojo.Dialogs;

public class DlgMultiSvrSwitch : Form
{
    private readonly Dictionary<string, string> dictServers;
    public DlgMultiSvrSwitch()
    {
        InitializeComponent();
        dictServers = new Dictionary<string, string>();
    }

    private void BtnConnectClickCallback(object sender, EventArgs e)
    {
        if (CbxServerUrl.Text.Length == 0)
        {
            MessageBox.Show("請指定要連線的server URL。", Text);
            return;
        }
        string svrurl = AutoFormat(CbxServerUrl.Text);
        CbxServerUrl.Text = svrurl;
        Debug.WriteLine($"向{svrurl}連線。");
        if (NewServerUrl(svrurl))
            Debug.WriteLine($"新增{svrurl}至server名單。");
    }

    private string AutoFormat(string rawText)
    {
        UriBuilder builder = new UriBuilder(rawText);
        return builder.Uri.ToString();
    }

    private bool NewServerUrl(string url)
    {
        if (dictServers.ContainsKey(url))
            return false;
        dictServers.Add(url, null);
        return true;
    }

    #region GUI components
    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private Label LblServerUrl = new Label();
    private ComboBox CbxServerUrl = new ComboBox();
    private Button BtnConnect = new Button();
    private ToolTip Tip = new ToolTip();

    private void InitializeComponent()
    {
        // LblServerUrl
        LblServerUrl.Name = "LblServerUrl";
        LblServerUrl.Text = "伺服器：";
        LblServerUrl.AutoSize = true;
        LblServerUrl.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        // CbxServerUrl
        CbxServerUrl.Name = "CbxServerUrl";
        CbxServerUrl.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        CbxServerUrl.TabIndex = 1;

        // BtnConnect
        BtnConnect.Name = "BtnConnect";
        BtnConnect.Text = "連線";
        BtnConnect.AutoSize = true;
        BtnConnect.TextAlign = ContentAlignment.MiddleCenter;
        BtnConnect.Click += BtnConnectClickCallback;
        BtnConnect.TabIndex = 2;

        // TlpMain
        TlpMain.Name = "TlpMain";
        TlpMain.Dock = DockStyle.Fill;
        TlpMain.Margin = new Padding(10);
        TlpMain.ColumnCount = 3;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 3;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.Controls.Add(LblServerUrl, 0, 0);
        TlpMain.Controls.Add(CbxServerUrl, 1, 0);
        TlpMain.Controls.Add(BtnConnect, 2, 2);
        TlpMain.SetColumnSpan(CbxServerUrl, 2);

        // tip
        Tip.SetToolTip(BtnConnect, "不會真的連線。");

        // DlgMultiSvrSwitch
        Name = "DlgMultiSvrSwitch";
        Text = "多伺服器切換介面";
        Padding = new Padding(10);
        Size = new Size(420, 150);
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        StartPosition = FormStartPosition.CenterScreen;
        AcceptButton = BtnConnect;
        SuspendLayout();
        Controls.Add(TlpMain);
        ResumeLayout(false);
    }
    #endregion
}