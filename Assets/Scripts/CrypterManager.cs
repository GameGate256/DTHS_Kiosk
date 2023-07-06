using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class CrypterManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string x = "";
        //Debug.Log("Encode Text:" + EncryptPlainTextToCipherText(x));
    }

    //This security key should be very complex and Random for encrypting the text. This playing vital role in encrypting the text.
    private const string SecurityKey = "LO8to6z}sfyM7116'U{ze><<5Yp8eYA8wn/UYvim&uZ3b->hh&:H]@rB_wbnDCJ";

    //This method is used to convert the plain text to Encrypted/Un-Readable Text format.
    public static string EncryptPlainTextToCipherText(string PlainText)
    {
        // Getting the bytes of Input String.
        byte[] toEncryptedArray = UTF8Encoding.UTF8.GetBytes(PlainText);

        MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();
        //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
        byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
        //De-allocatinng the memory after doing the Job.
        objMD5CryptoService.Clear();

        var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
        //Assigning the Security key to the TripleDES Service Provider.
        objTripleDESCryptoService.Key = securityKeyArray;
        //Mode of the Crypto service is Electronic Code Book.
        objTripleDESCryptoService.Mode = CipherMode.ECB;
        //Padding Mode is PKCS7 if there is any extra byte is added.
        objTripleDESCryptoService.Padding = PaddingMode.PKCS7;


        var objCrytpoTransform = objTripleDESCryptoService.CreateEncryptor();
        //Transform the bytes array to resultArray
        byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptedArray, 0, toEncryptedArray.Length);
        objTripleDESCryptoService.Clear();
        return System.Convert.ToBase64String(resultArray, 0, resultArray.Length);
    }

    //This method is used to convert the Encrypted/Un-Readable Text back to readable  format.
    public static string DecryptCipherTextToPlainText(string CipherText)
    {
        try
        {
            byte[] toEncryptArray = System.Convert.FromBase64String(CipherText);
            MD5CryptoServiceProvider objMD5CryptoService = new MD5CryptoServiceProvider();

            //Gettting the bytes from the Security Key and Passing it to compute the Corresponding Hash Value.
            byte[] securityKeyArray = objMD5CryptoService.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
            objMD5CryptoService.Clear();

            var objTripleDESCryptoService = new TripleDESCryptoServiceProvider();
            //Assigning the Security key to the TripleDES Service Provider.
            objTripleDESCryptoService.Key = securityKeyArray;
            //Mode of the Crypto service is Electronic Code Book.
            objTripleDESCryptoService.Mode = CipherMode.ECB;
            //Padding Mode is PKCS7 if there is any extra byte is added.
            objTripleDESCryptoService.Padding = PaddingMode.PKCS7;

            var objCrytpoTransform = objTripleDESCryptoService.CreateDecryptor();
            //Transform the bytes array to resultArray
            byte[] resultArray = objCrytpoTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            objTripleDESCryptoService.Clear();

            //Convert and return the decrypted data/byte into string format.
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        catch (System.Exception)
        {
            return "@DECODEFAILED";
        }
    }
}
