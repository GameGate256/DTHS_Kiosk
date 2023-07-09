using System.Collections;
using System.Net;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;
using UnityEngine.Networking;

public class QRScanner : MonoBehaviour
{
    
    public TMP_Text outputText;
    WebCamTexture webcamTexture;
    public string QrCode = string.Empty;
    bool isScanning = false;

    const string URL = "https://script.google.com/macros/s/AKfycbyK3YFjdsv2KV15rAes-IYjHh50RiolD72oj2yJzYFdxndzvEG_3UtUhPDRIAEviawu/exec";

    void Start()
    {

    }

    IEnumerator GetQRCode()
    {
        isScanning = true;
        outputText.text = "Scanning...";
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {
            try
            {
                if(isScanning == false)
                {
                    outputText.text = "Stopped.";
                    break;
                }

                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result= barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result!= null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {

                        Debug.Log("Plain Text: " + QrCode);
                        Debug.Log("Decrypted Text: " + CrypterManager.DecryptCipherTextToPlainText(QrCode));
                        outputText.text = CrypterManager.DecryptCipherTextToPlainText(QrCode);
                        break;
                    }
                }
            }
            catch (Exception ex) 
            { 
                Debug.LogWarning(ex.Message); 
                outputText.text = "Exception: " + ex.Message;
            }
            yield return null;
        }
        webcamTexture.Stop();

        WWWForm form = new WWWForm();
        form.AddField("value", CrypterManager.DecryptCipherTextToPlainText(QrCode));
        if (CrypterManager.DecryptCipherTextToPlainText(QrCode) != "@DECODEFAILED")
            
            using(UnityWebRequest www = UnityWebRequest.Post(URL, form))
            {
                yield return www.SendWebRequest();

                string data = www.downloadHandler.text;
                print(data);
            }
    }

    public void StartDetection()
    {
        QrCode = string.Empty;
        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(512, 512);
        this.GetComponent<Renderer>().material.mainTexture = webcamTexture;
        StartCoroutine(GetQRCode());
    }

    public void StopDetection()
    {
        isScanning = false;
    }
}
