using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace WinformDojo.Utils;

public class TrustedPublisherVerifier
{
    // 【關鍵】這是在主程式中唯一需要寫死的「固定資訊」：你公司的憑證指紋 (Thumbprint)
    // 只要用這張憑證簽署的 DLL，未來怎麼更新都能直接通過驗證！
    private const string TrustedThumbprint = "A1B2C3D4E5F67890123456789ABCDEF012345678";

    /// <summary>
    /// 驗證指定的 plugin DLL 是否由受信任的公鑰（憑證）所簽署
    /// </summary>
    public static bool VerifyPluginByPublisher(string dllPath)
    {
        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"[安全警告] 找不到指定的外掛檔案: {dllPath}");
            return false;
        }

        try
        {
            // 1. 從 DLL 檔案中讀取內嵌的簽署憑證（其中包含你的公鑰）
            // 注意：若該 DLL 未經數位簽署，此行會拋出 CryptographicException
            using var cert = new X509Certificate2(dllPath);

            // 2. 嚴格比對憑證指紋（憑證公鑰的唯一雜湊識別）
            if (!string.Equals(cert.Thumbprint, TrustedThumbprint, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"[安全攔截] 憑證指紋不符！此外掛並非由受信任的發行者簽署。");
                Console.WriteLine($"實際指紋: {cert.Thumbprint}");
                return false;
            }

            // 3. 檢查憑證是否在有效期內
            if (DateTime.Now < cert.NotBefore || DateTime.Now > cert.NotAfter)
            {
                Console.WriteLine($"[安全攔截] 簽署該外掛的憑證已經過期。");
                return false;
            }

            // 通過所有安全檢查！
            return true;
        }
        catch (System.Security.Cryptography.CryptographicException)
        {
            // 攔截未簽章的惡意或第三方未授權 DLL
            Console.WriteLine($"[安全攔截] 該 DLL 檔案未包含任何數位簽章: {Path.GetFileName(dllPath)}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[安全錯誤] 驗證過程發生例外狀況: {ex.Message}");
            return false;
        }
    }
}