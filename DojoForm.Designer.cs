using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace WinformDojo;

partial class DojoForm
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private IContainer components = null;

    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private Button BtnAbout = new Button();
    private Button BtnMultiSvrSelection = new Button();
    private Button BtnGifSplitter = new Button();
    private Button BtnTrayicon = new Button();

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.SuspendLayout();

        // TlpMain
        TlpMain.Name = "TlpMain";
        TlpMain.Margin = new Padding(10);
        TlpMain.Dock = DockStyle.Fill;
        // TlpMain.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        TlpMain.SuspendLayout();
        TlpMain.ColumnCount = 1;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 5;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.Controls.Add(BtnAbout, 0, 0);
        TlpMain.Controls.Add(BtnMultiSvrSelection, 0, 1);
        TlpMain.Controls.Add(BtnGifSplitter, 0, 2);
        TlpMain.Controls.Add(BtnTrayicon, 0, 3);
        TlpMain.ResumeLayout(false);

        // BtnAbout
        BtnAbout.Name = "BtnAbout";
        BtnAbout.Text = "About";
        BtnAbout.AutoSize = true;
        BtnAbout.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnAbout.Click += BtnAboutClickCallback;

        // BtnMultiSvrSelection
        BtnMultiSvrSelection.Name = "BtnMultiSvrSelection";
        BtnMultiSvrSelection.Text = "Multi-Server Selection";
        BtnMultiSvrSelection.AutoSize = true;
        BtnMultiSvrSelection.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnMultiSvrSelection.Click += BtnMultiSvrSelectionCallback;

        // BtnGifSplitter
        BtnGifSplitter.Name = "BtnGifSplitter";
        BtnGifSplitter.Text = "GIF Splitter";
        BtnGifSplitter.AutoSize = true;
        BtnGifSplitter.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnGifSplitter.Click += BtnGifSplitterCallback;

        // BtnTrayicon
        BtnTrayicon.Name = "BtnTrayicon";
        BtnTrayicon.Text = "Trayicon";
        BtnTrayicon.AutoSize = true;
        BtnTrayicon.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnTrayicon.Click += BtnTrayiconCallback;

        // main form
        this.components = new Container();
        this.AutoScaleMode = AutoScaleMode.Font;
        this.AutoSize = true;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Padding = new Padding(10);
        this.Text = "Dojo";
        this.Controls.Add(TlpMain);
        this.ResumeLayout(false);
    }

    #endregion
}
