using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformDojo.Dialogs;

public class DlgTrayicon : Form
{
    const string SCRIPT_NAME_TURNON = "sn_turn_on";
    const string SCRIPT_NAME_TURNOFF = "sn_turn_off";
    const string SCRIPT_NAME_CYCLE = "sn_cycle";
    private CancellationTokenSource ctsTrayicon = null;

    public DlgTrayicon()
    {
        InitializeComponent();
    }

    private void DlgClosedCallback(object sender, FormClosedEventArgs e)
    {
        Trayicon.Visible = false;
        ctsTrayicon?.Cancel();
        Trayicon.Dispose();
    }

    private void StatusSwitch(object sender, EventArgs e)
    {
        if (sender is RadioButton RbnChecked)
        {
            if (RbnChecked.Checked)
                SetTrayIconScript(RbnChecked.Name);
        }
    }

    private void SetTrayIconScript(string scriptName)
    {
        ctsTrayicon?.Cancel();
        switch (scriptName)
        {
            case SCRIPT_NAME_TURNON:
                Trayicon.Icon = ICO_SWITCHON;
                break;
            case SCRIPT_NAME_TURNOFF:
                Trayicon.Icon = ICO_SWITCHOFF;
                break;
            case SCRIPT_NAME_CYCLE:
                ctsTrayicon = new CancellationTokenSource();
                Icon[] frames = [
                    ICO_CYCLE_00,
                    ICO_CYCLE_01,
                    ICO_CYCLE_02,
                    ICO_CYCLE_03,
                ];
                _ = TrayiconAnimateAsync(frames, ctsTrayicon.Token);
                break;
            default:
                Debug.WriteLine("do nothing...");
                break;
        }
    }

    private async Task TrayiconAnimateAsync(Icon[] frames, CancellationToken token)
    {
        int i = 0;
        while (!token.IsCancellationRequested)
        {
            Trayicon.Icon = frames[i];
            await Task.Delay(TimeSpan.FromMilliseconds(100));
            i = (i + 1) % frames.Length;
        }
    }

    #region GUI components
    private TableLayoutPanel TlpMain = new TableLayoutPanel();
    private Label LblStatus = new Label();
    private RadioButton RbnOn = new RadioButton();
    private RadioButton RbnOff = new RadioButton();
    private RadioButton RbnCycle = new RadioButton();
    private NotifyIcon Trayicon = new NotifyIcon();

    private void InitializeComponent()
    {
        SuspendLayout();
        // Trayicon
        Trayicon.Visible = true;

        // LblStatus
        LblStatus.Name = "LblStatus";
        LblStatus.Text = "狀態";
        LblStatus.AutoSize = true;
        LblStatus.Anchor = AnchorStyles.Left;

        // RbnOn
        RbnOn.Name = SCRIPT_NAME_TURNON;
        RbnOn.Text = "啟動";
        RbnOn.AutoSize = true;
        RbnOn.Anchor = AnchorStyles.Left;
        RbnOn.Margin = new Padding(10, 0, 0, 0);
        RbnOn.TabIndex = 1;
        RbnOn.Checked = true;
        RbnOn.CheckedChanged += StatusSwitch;

        // RbnOff
        RbnOff.Name = SCRIPT_NAME_TURNOFF;
        RbnOff.Text = "停止";
        RbnOff.AutoSize = true;
        RbnOff.Anchor = AnchorStyles.Left;
        RbnOff.Margin = new Padding(10, 0, 0, 0);
        RbnOff.TabIndex = 2;
        RbnOff.CheckedChanged += StatusSwitch;

        // RbnCycle
        RbnCycle.Name = SCRIPT_NAME_CYCLE;
        RbnCycle.Text = "迴轉";
        RbnCycle.AutoSize = true;
        RbnCycle.Anchor = AnchorStyles.Left;
        RbnCycle.Margin = new Padding(10, 0, 0, 0);
        RbnCycle.TabIndex = 3;
        RbnCycle.CheckedChanged += StatusSwitch;

        // TlpMain
        TlpMain.Name = "TlpMain";
        TlpMain.Dock = DockStyle.Fill;
        TlpMain.ColumnCount = 1;
        TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        TlpMain.RowCount = 5;
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TlpMain.Controls.Add(LblStatus, 0, 0);
        TlpMain.Controls.Add(RbnOn, 0, 1);
        TlpMain.Controls.Add(RbnOff, 0, 2);
        TlpMain.Controls.Add(RbnCycle, 0, 3);

        // DlgTrayicon
        Name = "DlgTrayicon";
        Text = "Trayicon控制器";
        ClientSize = new Size(200, 200);
        Padding = new Padding(10);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedToolWindow;
        Controls.Add(TlpMain);
        FormClosed += DlgClosedCallback;
        ResumeLayout(false);
    }
    #endregion

    #region icon resources
    public static Icon ICO_SWITCHON { get { return icoSwitchOn.Value; } }
    public static Icon ICO_SWITCHOFF { get { return icoSwitchOff.Value; } }
    public static Icon ICO_CYCLE_00 { get { return icoCycle0.Value; } }
    public static Icon ICO_CYCLE_01 { get { return icoCycle1.Value; } }
    public static Icon ICO_CYCLE_02 { get { return icoCycle2.Value; } }
    public static Icon ICO_CYCLE_03 { get { return icoCycle3.Value; } }

    private static Lazy<Icon> icoSwitchOn = new Lazy<Icon>(GetIcon("switch_on.ico"));
    private static Lazy<Icon> icoSwitchOff = new Lazy<Icon>(GetIcon("switch_off.ico"));
    private static Lazy<Icon> icoCycle0 = new Lazy<Icon>(GetIcon("cycle_0.ico"));
    private static Lazy<Icon> icoCycle1 = new Lazy<Icon>(GetIcon("cycle_1.ico"));
    private static Lazy<Icon> icoCycle2 = new Lazy<Icon>(GetIcon("cycle_2.ico"));
    private static Lazy<Icon> icoCycle3 = new Lazy<Icon>(GetIcon("cycle_3.ico"));
    private static Icon GetIcon(string name)
    {
        return new Icon(typeof(Program), $"imgs.{name}");
    }
    #endregion
}