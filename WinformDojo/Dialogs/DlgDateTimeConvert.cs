using System;
using System.Drawing;
using System.Text.Json;
using System.Windows.Forms;

namespace WinformDojo.Dialogs;

public class DlgDateTimeConvert : Form
{
    public DlgDateTimeConvert()
    {
        InitializeComponent();
    }

    private void TbxInputTextChangedCallback(object sender, EventArgs e)
    {
        BtnConvert.Enabled = TbxInput.TextLength > 0;
    }

    private void BtnConvertClickCallback(object sender, EventArgs e)
    {
        if (TbxInput.TextLength <= 0)
            return;

        try
        {
            DateTime dt = DateTime.Parse(TbxInput.Text, null);
            string msg = GetDateTimeDescription(dt);
            MessageBox.Show(msg, "Convert");
        }
        catch (Exception excp)
        {
            string msg = string.Format(
                "轉換時間時發生例外狀況。（{0} - {1}）",
                excp.GetType(), excp.Message
            );
            MessageBox.Show(msg, "Failed to convert");
            TbxInput.SelectAll();
        }
    }

    private string GetDateTimeDescription(DateTime dt)
    {
        string seriStr = JsonSerializer.Serialize(dt);
        string description = string.Format(
            "{0,10}: {1}\n{2,10}: {3}",
            "tostring", dt.ToString(),
            "serialize", seriStr
        );

        return description;
    }

    #region GUI components
    private readonly TableLayoutPanel TlpMain = new TableLayoutPanel();
    private readonly TextBox TbxInput = new TextBox();
    private readonly Button BtnConvert = new Button();

    private void InitializeComponent()
    {
        SuspendLayout();

        // TbxInput
        TbxInput.Name = "TbxInput";
        TbxInput.TabIndex = 0;
        TbxInput.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        TbxInput.TextChanged += TbxInputTextChangedCallback;

        // BtnConvert
        BtnConvert.Name = "BtnConvert";
        BtnConvert.TabIndex = 1;
        BtnConvert.AutoSize = true;
        BtnConvert.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        BtnConvert.Text = "Convert";
        BtnConvert.Enabled = false;
        BtnConvert.Click += BtnConvertClickCallback;

        // TlpMain
        TlpMain.Name = "TlpMain";
        TlpMain.Dock = DockStyle.Fill;
        TlpMain.ColumnCount = 2;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 1;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.Controls.Add(TbxInput, 0, 0);
        TlpMain.Controls.Add(BtnConvert, 1, 0);

        // DlgDateTimeConvert
        Name = "DlgDateTimeConvert";
        Text = "DateTime Convert";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(420, 100);
        MaximizeBox = false;
        MinimizeBox = false;
        Controls.Add(TlpMain);
        AcceptButton = BtnConvert;
        
        ResumeLayout(false);
    }
    #endregion
}