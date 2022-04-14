using System.Security.Cryptography;
using System.Text;
using RichbetsResurrected.Utilities.Constants;

namespace RichbetsResurrected.Utilities.Helpers;

public static class CrashHelper
{
    private static readonly string _crashHash = "";
    private static readonly string _salt = "2d1f5bd223dd4c09f1c9de69e57c14042e85cdf15623bfa138e7e8272676bc90";


    public static decimal RandomMultiplier()
    {
        var newHash = GenerateHash(Encoding.ASCII.GetBytes(_crashHash));

        return (decimal) Math.Round(CrashPointFromHash(newHash), 2);
    }


    private static double CrashPointFromHash(byte[] serverSeed)
    {
        var hash = new HMACSHA256(serverSeed).ComputeHash(Encoding.ASCII.GetBytes(_salt));
        var sHash = hash.ConvertByteToString();

        var hs = Convert.ToInt32(100 / CrashConfigs.PercentValueToCrashInstantly);

        if (CheckIfShouldCrashInstantly(sHash, hs))
        {
            //Debug.WriteLine("Rolled 1");
            return 1;
        }

        var h = Convert.ToInt64(sHash.Substring(0, 52 / 4), 16);
        var e = Math.Pow(2, 52);

        return Math.Floor((100 * e - h) / (e - h)) / 100.0;
    }

    private static bool CheckIfShouldCrashInstantly(string hash, int mod)
    {
        var val = 0;

        var o = hash.Length % 4;
        for (var i = o > 0 ? o - 4 : 0; i < hash.Length; i += 4)
            val = ((val << 16) + Convert.ToInt32(hash.Substring(i, 4), 16)) % mod;

        return val == 0;
    }

    private static byte[] GenerateHash(byte[] seed)
    {
        var x = new HMACSHA256().ComputeHash(seed);
        return x;
    }
    
    
    private static string ConvertByteToString(this byte[] bytes)
    {
        var sb = new StringBuilder();

        foreach (var b in bytes)
            sb.Append(b.ToString("x2"));

        return sb.ToString();
    }
}