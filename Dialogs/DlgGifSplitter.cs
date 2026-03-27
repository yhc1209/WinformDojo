using System.Drawing;
using System.Windows.Forms;

namespace WinformDojo.Dialogs;

public class DlgGifSplitter : Form
{
    public DlgGifSplitter()
    {
        InitializeComponent();
    }

    #region GUI components
    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private GroupBox GbxSource = new GroupBox();
    private TableLayoutPanel TlpSource = new TableLayoutPanel();
    private TextBox TbxSource = new TextBox();
    private Button BtnSource = new Button();
    private Button BtnSplit = new Button();

    private void InitializeComponent()
    {
        SuspendLayout();
        // TbxSource
        TbxSource.Name = "TbxSource";
        TbxSource.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        TbxSource.ReadOnly = true;
        TbxSource.PlaceholderText = "請選擇一個GIF圖...";
        TbxSource.TabStop = false;

        // BtnSource
        BtnSource.Name = "BtnSource";
        BtnSource.Text = "選擇";
        BtnSource.AutoSize = true;
        BtnSource.TabIndex = 1;

        // BtnSplit
        BtnSplit.Name = "BtnSplit";
        BtnSplit.Text = "分離！";
        BtnSplit.AutoSize = true;
        BtnSplit.TabIndex = 2;
        BtnSplit.Dock = DockStyle.Fill;

        // TlpSource
        TlpSource.SuspendLayout();
        TlpSource.Name = "TlpSource";
        TlpSource.Dock = DockStyle.Fill;
        TlpSource.ColumnCount = 2;
        TlpSource.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpSource.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpSource.RowCount = 2;
        TlpSource.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpSource.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpSource.Controls.Add(TbxSource, 0, 0);
        TlpSource.Controls.Add(BtnSource, 1, 0);
        TlpSource.ResumeLayout(false);

        // GbxSource
        GbxSource.SuspendLayout();
        GbxSource.Name = "GbxSource";
        GbxSource.Text = "來源GIF圖";
        GbxSource.AutoSize = true;
        GbxSource.Dock = DockStyle.Fill;
        GbxSource.Controls.Add(TlpSource);
        GbxSource.ResumeLayout(false);

        // TlpMain
        TlpMain.SuspendLayout();
        TlpMain.Name = "TlpMain";
        TlpMain.Dock = DockStyle.Fill;
        TlpMain.ColumnCount = 2;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 1;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.Controls.Add(GbxSource, 0, 0);
        TlpMain.Controls.Add(BtnSplit, 1, 0);
        TlpMain.ResumeLayout(false);

        // DlgGifSplitter
        Name = "DlgGifSplitter";
        Text = "GIF分離器";
        // AutoSize = true;
        ClientSize = new Size(420, 75);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Controls.Add(TlpMain);
        ResumeLayout(false);
    }
    #endregion
}