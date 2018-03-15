using System.Security.Cryptography;
using System.Text;

public class TUtils{

    public static string MDEncode(string value)
    {
        string pwd = "";
        MD5 md5 = MD5.Create();
        byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(value));
        for (int i = 0; i < s.Length; i++)
        {
            pwd = pwd + s[i].ToString("x2");
        }
        return pwd;
    }
}
