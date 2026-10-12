using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography.Pkcs;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace WinformDojo.Utils;

public class DigiSignVerifier
{
    public static async Task<bool> VerifyAsync(string assemblyPath, string thrumbPrint, CancellationToken token)
    {
        try
        {
            byte[] fileContent = await File.ReadAllBytesAsync(assemblyPath, token).ConfigureAwait(false);

            SignedCms signedCmd = new SignedCms();
            signedCmd.Decode(fileContent);
            signedCmd.CheckSignature(true);

            foreach (SignerInfo signer in signedCmd.SignerInfos)
            {
                if (signer.Certificate.Thumbprint.Equals(thrumbPrint, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            Debug.WriteLine("[dbg] 簽章憑證不如預期。");
            return false;
        }
        catch (Exception excp)
        {
            Debug.WriteLine($"[dbg] 驗簽章時出例外。（{excp.GetType().FullName} - {excp.Message}）");
            Debug.WriteLine(excp.StackTrace);
            return false;
        }
    }

    public static bool VerifyMySelfSignedDll(string dllPath, string myExpectedThumbprint)
    {
        try
        {
            // A. 以唯讀模式開啟檔案，取得核心要求的 SafeFileHandle
            using (FileStream fs = new FileStream(dllPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                SafeFileHandle handle = fs.SafeFileHandle;
                int bufferLength = 0;

                // B. 第一階段呼叫：故意傳入 IntPtr.Zero 與長度 0，讓 API 回傳正確的「所需緩衝區大小」
                ImageGetCertificateData(handle, 0, IntPtr.Zero, ref bufferLength);
                
                if (bufferLength <= 0)
                {
                    Console.WriteLine("此檔案並未包含數位簽章區塊。");
                    return false;
                }

                // C. 配置非託管記憶體緩衝區
                IntPtr buffer = Marshal.AllocHGlobal(bufferLength);
                try
                {
                    // D. 第二階段呼叫：傳入配置好的記憶體，真正將簽章填入緩衝區
                    if (!ImageGetCertificateData(handle, 0, buffer, ref bufferLength))
                    {
                        int err = Marshal.GetLastWin32Error();
                        Console.WriteLine($"提取簽章失敗，Win32 錯誤碼: {err}");
                        return false;
                    }

                    // E. 解析 WIN_CERTIFICATE 結構標頭
                    WIN_CERTIFICATE winCert = Marshal.PtrToStructure<WIN_CERTIFICATE>(buffer);

                    if (winCert.wCertificateType != WIN_CERT_TYPE_PKCS_SIGNED_DATA)
                    {
                        Console.WriteLine("不支援的簽章結構類型。");
                        return false;
                    }

                    // F. 關鍵：剔除結構頭部（8個位元組），只取出後方純粹的 PKCS#7 / CMS 簽章位元組
                    int headerSize = Marshal.SizeOf<WIN_CERTIFICATE>() - ANYSIZE_ARRAY_PADDING_ADJUSTMENT();
                    // 註：WIN_CERTIFICATE 結構長度為 8 位元組（4+2+2）
                    int pkcs7Length = winCert.dwLength - 8; 
                    
                    byte[] cmsBytes = new byte[pkcs7Length];
                    // 從緩衝區位移 8 位元組的位置開始複製
                    Marshal.Copy(IntPtr.Add(buffer, 8), cmsBytes, 0, pkcs7Length);

                    // G. 丟給 SignedCms 解碼（現在不會再報 ASN1 損壞錯誤了！）
                    var signedCms = new SignedCms();
                    signedCms.Decode(cmsBytes);

                    // H. 略過系統信任鏈與過期檢查，只進行密碼學防竄改驗證
                    signedCms.CheckSignature(true);

                    // I. 驗證通過，檢查是否為指定的憑證指紋
                    foreach (var signer in signedCms.SignerInfos)
                    {
                        if (signer.Certificate.Thumbprint.Equals(myExpectedThumbprint, StringComparison.OrdinalIgnoreCase))
                        {
                            return true; // 完全符合安全規範！
                        }
                    }
                }
                finally
                {
                    // 務必釋放非託管記憶體，避免記憶體洩漏
                    Marshal.FreeHGlobal(buffer);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"驗證過程中發生例外: {ex.Message}");
        }
        return false;
    }

    private static int ANYSIZE_ARRAY_PADDING_ADJUSTMENT() => 0; // 用於語法輔助，C# 欄位定義恰好為 8


    #region WINAPI
    [DllImport("Imagehlp.dll", SetLastError = true, EntryPoint = "ImageGetCertificateData")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool ImageGetCertificateData(
        SafeFileHandle fileHandle,
        int certificateIndex,
        IntPtr certificateBuffer,
        ref int requiredLength);

    /// <summary>
    /// 參照 Windows 的 WIN_CERTIFICATE 結構
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    private struct WIN_CERTIFICATE
    {
        /// <summary>
        /// 整個簽章資料的長度
        /// </summary>
        public int dwLength;
        /// <summary>
        /// 憑證版本 (Authenticode 通常是 0x0200)
        /// </summary>
        public short wRevision;
        /// <summary>
        /// 憑證類型 (PKCS SignedData 是 0x0002)
        /// </summary>
        public short wCertificateType;

        // /// <summary>
        // /// An array of certificates.
        // /// </summary>
        // /// <remarks>The format of this member depends on the value of wCertificateType.</remarks>
        // public byte[] bCertificate;
    }

    /// <summary>
    /// The bCertificate member contains a PKCS SignedData structure.
    /// </summary>
    private const int WIN_CERT_TYPE_PKCS_SIGNED_DATA = 0x0002;
    #endregion
}