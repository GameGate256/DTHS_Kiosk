using System.Collections;
using System.Net;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using TMPro;

public class QRScanner : MonoBehaviour
{
    
    public TMP_Text outputText;
    WebCamTexture webcamTexture;
    string QrCode = string.Empty;
    bool isScanning = false;

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
