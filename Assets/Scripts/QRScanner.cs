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
    
    //public TMP_Text outputText;
    WebCamTexture webcamTexture;
    public string QrCode = string.Empty;
    bool isScanning = false;
    public bool isScanSuccess = false, isScanEnded = false;
    

    string QRoutput;

    const string URL = "https://script.google.com/macros/s/AKfycbw2RH7uetonE1gGpvq1vyc7N-pPGkzRnzCqtyJi0NIWJEyITYPxZXtdtGErfZFCVa9pgw/exec";
    void Start()
    {

    }

    IEnumerator GetQRCode()
    {
        isScanSuccess = false;
        isScanEnded = false;
        isScanning = true;
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {
            try
            {
                if(isScanning == false)
                {
                    isScanEnded = true;
                    break;
                }

                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result= barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result!= null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        try
                        {
                            //Debug.Log("Plain Text: " + QrCode);
                            //Debug.Log("Decrypted Text: " + CrypterManager.DecryptAES(QrCode));
                            QRoutput = CrypterManager.DecryptAES(QrCode);
                            isScanSuccess = true;
                            isScanEnded = true;
                            break;
                        }
                        catch (Exception ex)
                        {
                            Debug.LogWarning(ex.Message);
                            isScanEnded = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex) 
            { 
                Debug.LogWarning(ex.Message); 
                isScanEnded = true;
                break;
            }
            yield return null;
        }
        webcamTexture.Stop();
        if(isScanSuccess)
        {
            WWWForm form = new WWWForm();
            form.AddField("value", QRoutput);
            using(UnityWebRequest www = UnityWebRequest.Post(URL, form))
            {
                yield return www.SendWebRequest();

                string data = www.downloadHandler.text;
                //print(data);
            }
        }
    }

    public void StartDetection()
    {
        QrCode = string.Empty;
        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(512, 512);
        this.GetComponent<Renderer>().material.mainTexture = webcamTexture;

        //print(webcamTexture.deviceName);
        StartCoroutine(GetQRCode());
    }

    public void StopDetection()
    {
        isScanning = false;
    }
}
