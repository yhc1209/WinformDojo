using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace WinformDojo.Dialogs;

public class DlgGifSplitter : Form
{
    public DlgGifSplitter()
    {
        InitializeComponent();
    }

    private void SelectSource(object sender, EventArgs e)
    {
        try
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "GIF|*.gif";
                ofd.CheckPathExists = true;
                ofd.CheckFileExists = true;
                ofd.ShowPreview = true;
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.Cancel)
                    return;

                TbxSource.Text = ofd.FileName;
            }
        }
        catch (Exception excp)
        {
            MessageBox.Show(string.Format(
                "選擇原始圖檔時發生例外。 ({0})\n{1}",
                excp.Message, excp.StackTrace
            ), BtnSource.Text);
            TbxSource.Clear();
        }
    }

    private void Export(object sender, EventArgs e)
    {
        try
        {
            if (!SelectExportFolder(out string outputFolder))
                return;

            SplitGif(TbxSource.Text, outputFolder);
            MessageBox.Show(string.Format("拆解圖檔存放在{0}", outputFolder), BtnSplit.Text);
        }
        catch (Exception excp)
        {
            MessageBox.Show(string.Format(
                "匯出分離圖檔時發生例外。 ({0})\n{1}",
                excp.Message, excp.StackTrace
            ), BtnSplit.Text);
        }
    }

    private void TbxSourceTextChangedCallback(object sender, EventArgs e)
    {
        BtnSplit.Enabled = TbxSource.TextLength > 0;
    }

    private bool SelectExportFolder(out string outputFolder)
    {
        using (FolderBrowserDialog fbd = new FolderBrowserDialog())
        {
            if (fbd.ShowDialog() == DialogResult.OK)
                outputFolder = fbd.SelectedPath;
            else
                outputFolder = null;
            return !string.IsNullOrEmpty(outputFolder);
        }
    }

    private void SplitGif(string gifPath, string outputFolder)
    {
        using (Image gifImage = Image.FromFile(gifPath))
        {
            outputFolder = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(gifPath));
            Directory.CreateDirectory(outputFolder);

            FrameDimension dimension = new FrameDimension(gifImage.FrameDimensionsList[0]);
            int frameCount = gifImage.GetFrameCount(dimension);
            for (int i = 0; i < frameCount; i++)
            {
                gifImage.SelectActiveFrame(dimension, i);
                string framePath = Path.Combine(outputFolder, $"frame_{i:D3}.png");
                gifImage.Save(framePath, ImageFormat.Png);
            }
        }
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
        TbxSource.TextChanged += TbxSourceTextChangedCallback;

        // BtnSource
        BtnSource.Name = "BtnSource";
        BtnSource.Text = "選擇";
        BtnSource.AutoSize = true;
        BtnSource.TabIndex = 1;
        BtnSource.Click += SelectSource;

        // BtnSplit
        BtnSplit.Name = "BtnSplit";
        BtnSplit.Text = "分離！";
        BtnSplit.AutoSize = true;
        BtnSplit.TabIndex = 2;
        BtnSplit.Dock = DockStyle.Fill;
        BtnSplit.Enabled = false;
        BtnSplit.Click += Export;

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