using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinformDojo.Dialogs;

public class DlgDictionaryEditor : Form
{
    private readonly string KeyDescription;
    private readonly string ValueDescription;
    public Dictionary<string, string> Dictionary { get; private set; }

    public DlgDictionaryEditor(string keyDesc, string valueDesc, Dictionary<string, string> dictionary)
    {
        KeyDescription = keyDesc;
        ValueDescription = valueDesc;
        Dictionary = dictionary;
        InitializeComponent();
    }

    private void LoadCallback(object sender, EventArgs e)
    {
        if (Dictionary is null)
            throw new Exception("You must specify a dictionary!");
        if (string.IsNullOrEmpty(KeyDescription))
            throw new Exception("You must specify a description for key!");
        if (string.IsNullOrEmpty(ValueDescription))
            throw new Exception("You must specify a description for value!");
        UpdateListView();
    }

    private void DictionaryItemEdit(object sender, MouseEventArgs e)
    {
        ListViewItem item = LsvDictionary.HitTest(e.Location).Item;
        if (item is null)
        {
            Debug.WriteLine("點了個空氣。");
            return;
        }

        Debug.WriteLine($"編輯{item.Text}的說明。");
        TbxEditor.Bounds = item.SubItems[1].Bounds;
        TbxEditor.Text = item.SubItems[1].Text ?? string.Empty;
        TbxEditor.Visible = true;
        TbxEditor.Focus();
        TbxEditor.SelectAll();
    }

    private void DlgClosedCallback(object sender, FormClosedEventArgs e)
    {
        if (DialogResult == DialogResult.OK)
        {
            Dictionary.Clear();
            foreach (ListViewItem item in LsvDictionary.Items)
            {
                if (item.Text?.Length > 0)
                    Dictionary.Add(item.Text, item.SubItems[1].Text);
            }
        }
    }

    private void AssignValue(object sender, EventArgs e)
    {
        LsvDictionary.SelectedItems[0].SubItems[1].Text = TbxEditor.Text;
        TbxEditor.Visible = false;
    }

    private void UpdateListView()
    {
        LsvDictionary.BeginUpdate();
        LsvDictionary.Columns.Add(KeyDescription);
        LsvDictionary.Columns.Add(ValueDescription);
        foreach (var pair in Dictionary)
        {
            string[] contents = new string[] { pair.Key, pair.Value };
            ListViewItem item = new ListViewItem(contents);
            LsvDictionary.Items.Add(item);
        }
        foreach (ColumnHeader columnHeader in LsvDictionary.Columns)
            columnHeader.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
        LsvDictionary.EndUpdate();
    }

    #region GUI components
    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private ListView LsvDictionary = new ListView();
    private Button BtnOk = new Button();
    private Button BtnCancel = new Button();
    private TextBox TbxEditor = new TextBox();

    private void InitializeComponent()
    {
        this.SuspendLayout();
        // TbxEditor
        TbxEditor.Name = "TbxEditor";
        TbxEditor.Text = string.Empty;
        TbxEditor.TabStop = false;
        TbxEditor.Visible = false;
        TbxEditor.Multiline = false;
        TbxEditor.Leave += AssignValue;

        // LsvDictionary
        LsvDictionary.Name = "LsvDictionary";
        LsvDictionary.Dock = DockStyle.Fill;
        LsvDictionary.View = View.Details;
        LsvDictionary.FullRowSelect = true;
        LsvDictionary.MultiSelect = false;
        LsvDictionary.Controls.Add(TbxEditor);
        LsvDictionary.MouseDoubleClick += DictionaryItemEdit;

        // BtnOk
        BtnOk.Name = "BtnOk";
        BtnOk.Text = "確認";
        BtnOk.AutoSize = true;
        BtnOk.TabIndex = 1;
        BtnOk.DialogResult = DialogResult.OK;

        // BtnCancel
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Text = "取消";
        BtnCancel.AutoSize = true;
        BtnCancel.TabIndex = 2;
        BtnCancel.DialogResult = DialogResult.Cancel;

        // TlpMain
        TlpMain.Name = "TlpMain";
        TlpMain.Dock = DockStyle.Fill;
        TlpMain.SuspendLayout();
        TlpMain.ColumnCount = 3;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 2;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.Controls.Add(LsvDictionary, 0, 0);
        TlpMain.Controls.Add(BtnOk, 1, 1);
        TlpMain.Controls.Add(BtnCancel, 2, 1);
        TlpMain.SetColumnSpan(LsvDictionary, 3);
        TlpMain.ResumeLayout(false);

        // DlgDictionaryEditor
        Name = "DlgDictionaryEditor";
        Size = new Size(420, 270);
        // AcceptButton = BtnOk;
        // CancelButton = BtnCancel;
        StartPosition = FormStartPosition.CenterParent;
        Controls.Add(TlpMain);
        Load += LoadCallback;
        FormClosed += DlgClosedCallback;
        ResumeLayout(false);
    }
    #endregion
}