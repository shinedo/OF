using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shortcut : MonoBehaviour
{
    private Texture m_ShortShoot;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.A))
        {
            ScreenCapture.CaptureScreenshot($"{Application.dataPath}/Resources/Screenshoot.jpg");
            m_ShortShoot = Resources.Load<Texture>("ScreenShoot");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
        {
            m_ShortShoot = ScreenCapture.CaptureScreenshotAsTexture(0);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
        {
            RenderTexture renderTexture = new RenderTexture(720, 1280, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(renderTexture);
            m_ShortShoot = renderTexture;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(TScreenCapture());
        }
    }

    private IEnumerator TScreenCapture()
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture2D = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture2D.ReadPixels(new Rect(0, 0,Screen.width,Screen.height), 0,0);
        texture2D.Apply();
        byte[] bytes = texture2D.EncodeToPNG();
        string fileName = DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + ".jpg";
        System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/ScreenShot/" + fileName, bytes);
  
        AssetDatabase.Refresh();
    }

    private void OnGUI()
    {
    }
}