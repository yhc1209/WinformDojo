using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows.Forms;

using SharedContract.Speaker;

namespace WinformDojo.Dialogs;

public class DlgSpeakers : Form
{
    private SpeakerLoadContext loadContext = null;

    public DlgSpeakers()
    {
        InitializeComponent();
    }

    private void FormLodingCallback(object sender, EventArgs e)
    {
        CbxSpeaker.Items.Add(new Dog());
        CbxSpeaker.Items.Add(new Cat());
    }

    private void FormClosedCallback(object sender, FormClosedEventArgs e)
    {
        loadContext?.Unload();
    }

    private void BtnSpeakCallback(object sender, EventArgs e)
    {
        ISpeaker speaker = CbxSpeaker.SelectedItem as ISpeaker;
        if (speaker is null)
            Output("Please select a speaker first.");
        else
            Output(speaker.Speak());
    }

    private void BtnImportCallback(object sender, EventArgs e)
    {
        using (OpenFileDialog ofd = new OpenFileDialog())
        {
            ofd.Multiselect = false;
            ofd.CheckPathExists = true;
            ofd.Filter = "dll files|*.dll|All files|*.*";
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            LoadDll(ofd.FileName);
        }
    }

    private void LoadDll(string pluginPath)
    {
        if (pluginPath is null)
            throw new ArgumentNullException("No plugin path was specified.");
        if (!File.Exists(pluginPath))
            throw new IOException("The specified plugin path does not exist.");

        int count = 0;
        string strISpeaker = typeof(ISpeaker).ToString();
        Debug.WriteLine($"[import] 要找繼承{strISpeaker}的class。");

        Assembly assembly = Assembly.LoadFrom(pluginPath);
        foreach (Type type in assembly.GetTypes())
        {
            if (!type.IsClass || type.IsAbstract)
                continue;

            string[] interfaceStrs = type.GetInterfaces().Select(i => i.ToString()).ToArray();
            Debug.WriteLine($"發現class：{type} (interfaces: {string.Join(", ", interfaceStrs)})");
            if (!interfaceStrs.Contains(strISpeaker))
                continue;

            string typeName = type.ToString();
            Debug.WriteLine($"偵測到ISpeaker：{typeName}");
            bool fExist = false;
            foreach (var item in CbxSpeaker.Items)
            {
                if (typeName == item.ToString())
                {
                    fExist = true;
                    Debug.WriteLine($"'{typeName}'已經存在，略過。");
                    break;
                }
            }
            if (fExist)
                continue;

            if (Activator.CreateInstance(type) is ISpeaker speaker)
            {
                CbxSpeaker.Items.Add(speaker);
                count++;
            }
        }

        MessageBox.Show($"成功加入{count}個會發出聲音的東東。", "載入plug-in");
    }

    private void Output(string message)
    {
        TbxOutput.AppendText($"{message}{Environment.NewLine}");
    }

    #region GUI components
    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private ComboBox CbxSpeaker = new ComboBox();
    private Button BtnSpeak = new Button();
    private Button BtnImport = new Button();
    private TextBox TbxOutput = new TextBox();

    private void InitializeComponent()
    {
        this.SuspendLayout();
        // CbxSpeaker
        CbxSpeaker.Name = "CbxSpeaker";
        CbxSpeaker.Anchor = AnchorStyles.Left | AnchorStyles.Right;

        // BtnSpeak
        BtnSpeak.Name = "BtnSpeak";
        BtnSpeak.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnSpeak.Text = "speak";
        BtnSpeak.AutoSize = true;
        BtnSpeak.TabIndex = 1;
        BtnSpeak.Click += BtnSpeakCallback;

        // BtnImport
        BtnImport.Name = "BtnBtnImportSpeak";
        BtnImport.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnImport.Text = "import";
        BtnImport.AutoSize = true;
        BtnImport.TabIndex = 2;
        BtnImport.Click += BtnImportCallback;

        // TbxOutput
        TbxOutput.Name = "TbxOutput";
        TbxOutput.Text = string.Empty;
        TbxOutput.TabStop = false;
        TbxOutput.Multiline = true;
        TbxOutput.Dock = DockStyle.Fill;
        TbxOutput.ScrollBars = ScrollBars.Both;

        // TlpMain
        TlpMain.Name = "TlpMain";
        TlpMain.Dock = DockStyle.Fill;
        TlpMain.SuspendLayout();
        TlpMain.ColumnCount = 3;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 2;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.Controls.Add(CbxSpeaker, 0, 0);
        TlpMain.Controls.Add(BtnSpeak, 1, 0);
        TlpMain.Controls.Add(BtnImport, 2, 0);
        TlpMain.Controls.Add(TbxOutput, 0, 1);
        TlpMain.SetColumnSpan(TbxOutput, TlpMain.ColumnCount);
        TlpMain.ResumeLayout(false);

        // DlgSpeakers
        Name = "DlgSpeakers";
        Text = "話盒";
        Size = new Size(420, 270);
        MinimumSize = new Size(420, 270);
        ShowInTaskbar = false;
        MaximizeBox = false;
        StartPosition = FormStartPosition.CenterParent;
        Controls.Add(TlpMain);
        Load += FormLodingCallback;
        FormClosed += FormClosedCallback;
        ResumeLayout(false);
    }
    #endregion
}