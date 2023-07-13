using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using UnityEngine; 

public class CrypterManager : MonoBehaviour
{
    static string aes_key = "03nxUXTG4l1ZMH54oqR2NbaAbknyt3qhBN9jaUL3FtM="; //44 characters
    static string aes_iv = "2878dc064fdaf29f"; //16 characters

    public string x;

    public void PrintEncodeText()
    {
        print(EncryptAES(x));
    }

    public static string EncryptAES(string plainText)
    {
        byte[] encrypted;

        using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = System.Convert.FromBase64String(aes_key);
            aes.IV = Encoding.UTF8.GetBytes(aes_iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform enc = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, enc, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }

                    encrypted = ms.ToArray();
                }
            }
        }

        return System.Convert.ToBase64String(encrypted);
    }

    public static string DecryptAES(string encryptedText)
    {
        string decrypted = null;
        byte[] cipher = System.Convert.FromBase64String(encryptedText);

        using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
        {
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = System.Convert.FromBase64String(aes_key);
            aes.IV = Encoding.UTF8.GetBytes(aes_iv);
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            ICryptoTransform dec = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new MemoryStream(cipher))
            {
                using (CryptoStream cs = new CryptoStream(ms, dec, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader(cs))
                    {
                        decrypted = sr.ReadToEnd();
                    }
                }
            }
        }

        return decrypted;
    }
}
