using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

namespace WinformDojo.Utils;

public class AssemblyVerifier
{
    /// <summary>
    /// 驗證指定 DLL 檔案的數位簽章與完整性
    /// </summary>
    public static bool VerifyAssemblySignature(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
            return false;

        using (var wtData = new WinTrustData(filePath))
        {
            // 呼叫 Windows 核心 WinVerifyTrust 進行驗證
            int result = WinVerifyTrust(IntPtr.Zero, WINTRUST_ACTION_GENERIC_VERIFY_V2, wtData);
            Debug.WriteLine($"[result] {GetVerificationErrorMessage(result)}");

            // 回傳 0 代表驗證完全通過（簽章合法、未被竄改、憑證未過期且受信任）
            return result == 0;
        }
    }
    
    public static string GetVerificationErrorMessage(int resultCode)
    {
        return resultCode switch
        {
            0 => "驗證成功",
            unchecked((int)0x800B0100) => "該檔案未包含數位簽章。",
            unchecked((int)0x800B0004) => "發行者不受信任（可能為未授權的自簽憑證）。",
            unchecked((int)0x800B0101) => "數位憑證已經過期。",
            unchecked((int)0x800B010E) => "數位憑證已被憑證授權單位撤銷。",
            unchecked((int)0x80096011) => "此簽章已被系統原則明確封鎖。",
            _ => $"驗證失敗，錯誤代碼：0x{resultCode:X8}"
        };
    }

    // P/Invoke 宣告：使用 Windows WinVerifyTrust 來驗證檔案簽章
    [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern int WinVerifyTrust(
        IntPtr hwnd,
        [MarshalAs(UnmanagedType.LPStruct)] Guid pgActionID,
        [In] WinTrustData pWTD
    );

    private static readonly Guid WINTRUST_ACTION_GENERIC_VERIFY_V2 = new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}");

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private class WinTrustData : IDisposable
    {
        public uint cbStruct = (uint)Marshal.SizeOf(typeof(WinTrustData));
        public IntPtr pFile = IntPtr.Zero;
        public IntPtr pCatalog = IntPtr.Zero;
        public IntPtr pBlob = IntPtr.Zero;
        public IntPtr pSgn = IntPtr.Zero;
        public uint dwUIChoice = 2; // WTD_UI_NONE (不跳出視窗)
        public uint fdwRevocationChecks = 1; // WTD_REVOKE_WHOLECHAIN
        public uint dwUnionChoice = 1; // WTD_CHOICE_FILE
        public IntPtr pInfoStruct = IntPtr.Zero;
        public uint dwStateAction = 0;
        public IntPtr hWVTStateData = IntPtr.Zero;
        public string pwszURLReference = null;
        public uint dwProvFlags = 0x00000010; // WTD_SAFER_FLAG
        public uint dwUIContext = 0;
        public IntPtr pSignatureSettings = IntPtr.Zero;

        public WinTrustData(string filePath)
        {
            pFile = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinTrustFileInfo)));
            var fileInfo = new WinTrustFileInfo(filePath);
            Marshal.StructureToPtr(fileInfo, pFile, false);
        }

        public void Dispose()
        {
            if (pFile != IntPtr.Zero)
            {
                Marshal.DestroyStructure(pFile, typeof(WinTrustFileInfo));
                Marshal.FreeHGlobal(pFile);
                pFile = IntPtr.Zero;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WinTrustFileInfo
    {
        public uint cbStruct;
        public string pcwszFilePath;
        public IntPtr hFile;
        public IntPtr pgKnownSubject;

        public WinTrustFileInfo(string filePath)
        {
            cbStruct = (uint)Marshal.SizeOf(typeof(WinTrustFileInfo));
            pcwszFilePath = filePath;
            hFile = IntPtr.Zero;
            pgKnownSubject = IntPtr.Zero;
        }
    }
}