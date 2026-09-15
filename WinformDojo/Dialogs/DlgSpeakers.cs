using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Collections.Generic;

using SharedContract.Speaker;
using WinformDojo.Plugin;

namespace WinformDojo.Dialogs;

public class DlgSpeakers : Form
{
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
    { }

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
        string msgTitle = "Import Plugin";
        try
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Title = msgTitle;
                ofd.Multiselect = false;
                ofd.CheckPathExists = true;
                ofd.Filter = "dll files|*.dll|All files|*.*";
                if (ofd.ShowDialog() != DialogResult.OK)
                    return;
                LoadDll(ofd.FileName);
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(string.Format(
                "載入plugin時出例外：{0} - {1}\n{2}",
                excp.GetType(), excp.Message, excp.StackTrace
            ), msgTitle);
        }
    }

    private void LoadDll(string pluginPath)
    {
        if (pluginPath is null)
            throw new ArgumentNullException("No plugin path was specified.");
        if (!File.Exists(pluginPath))
            throw new IOException("The specified plugin path does not exist.");

        // 1. 初始化隔離的載入上下文
        var loadContext = new SpeakerLoadContext(pluginPath);
        Assembly pluginAssembly = loadContext.LoadFromAssemblyPath(pluginPath);

        // 2. 透過反射尋找實作 IPlugin 的類別
        Type[] types = pluginAssembly.GetTypes();
        Type pluginType = types.FirstOrDefault(t => typeof(ISpeakerPlugin).IsAssignableFrom(t) && !t.IsInterface);
        if (pluginType is null)
            throw new Exception("The plugin resource was not found.");

        // 3. 實例化外掛主體
        ISpeakerPlugin plugin = (ISpeakerPlugin)Activator.CreateInstance(pluginType);
        Debug.WriteLine($"成功載入外掛：{plugin.PluginName}");

        // 4. 取得並操作外掛的擴充物件
        IReadOnlyList<ISpeaker> extendedObjects = plugin.GetExtendedObjects();
        CbxSpeaker.Items.AddRange(extendedObjects.ToArray());

        // 5. 卸載外掛與記憶體回收 (當 isCollectible = true 時)
        loadContext.Unload();
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