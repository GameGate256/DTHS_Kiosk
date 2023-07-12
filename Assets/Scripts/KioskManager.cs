using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KioskManager : MonoBehaviour
{
    bool needStop = false, res, needRescan = false;
    public QRScanner qrscanner;

    public GameObject MainScreen, DetectScreen, ResultScreen, ErrorScreen;

    public TMP_Text statusText;

    public void StartDetection()
    {
        if(needRescan)
        {
            statusText.text = "QR코드를 제대로 인식하지 못했습니다. 다시 시도해주세요.";
        }
        else
        {
            statusText.text = "QR코드를 인식시켜주세요.";
        }

        qrscanner.StartDetection();
        StartCoroutine(DetectQRCode());
    }

    IEnumerator DetectQRCode()
    {
        while (!qrscanner.isScanEnded)
        {
            if (needStop)
            {
                qrscanner.StopDetection();
                needStop = false;
                yield break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        //if res is true, go to result screen
        if (qrscanner.isScanSuccess)
        {
            DetectScreen.SetActive(false);
            ResultScreen.SetActive(true);
            needRescan = false;
        }

        else
        {
            needRescan = true;
            StartDetection();
        }
    }

    public void StopDetection() => needStop = true;

    public void Init_needRescan() => needRescan = false;
}
