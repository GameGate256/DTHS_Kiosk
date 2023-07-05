using System.Collections;
using System.Net;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRScanner : MonoBehaviour
{
    WebCamTexture webcamTexture;
    string QrCode = string.Empty;

    void Start()
    {

    }

    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {
            try
            {
                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result= barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result!= null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                         Debug.Log("DECODED TEXT FROM QR: " + QrCode);
                        break;
                    }
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
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
        QrCode = "@STOP";
    }
}