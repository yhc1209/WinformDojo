using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Windows.Forms;

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

    private void LoadDll(string pluginPath)
    {
        if (pluginPath is null)
            throw new ArgumentNullException("No plugin path was specified.");
        if (!File.Exists(pluginPath))
            throw new IOException("The specified plugin path does not exist.");

        loadContext = new SpeakerLoadContext(pluginPath);
        // Assembly assembly = loadContext.LoadFromAssemblyPath(pluginPath);
    }

    private void Output(string message)
    {
        TbxOutput.AppendText($"{message}{Environment.NewLine}");
    }
    
    #region GUI components
    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private ComboBox CbxSpeaker = new ComboBox();
    private Button BtnSpeak = new Button();
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
        TlpMain.ColumnCount = 2;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 2;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.Controls.Add(CbxSpeaker, 0, 0);
        TlpMain.Controls.Add(BtnSpeak, 1, 0);
        TlpMain.Controls.Add(TbxOutput, 0, 1);
        TlpMain.SetColumnSpan(TbxOutput, 2);
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