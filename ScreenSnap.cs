using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

// captures the screen on mobile

public class ScreenSnap : MonoBehaviour
{
    Texture2D _textureHolder;

    public void Snap()
    {
		// capture the screen and save in _textureHolder
        StartCoroutine(SnapCo());
        FirebaseManager.instance.FirebaseTookPhotoEvent(MapManager.instance.currentMapId);
    }

    IEnumerator SnapCo()
    {
        yield return StartCoroutine(CaptureScreen());
        SaveTextureToGallery();
    }

    List<GameObject> tmpUI = new List<GameObject>();
    IEnumerator CaptureScreen()
    {
        yield return new WaitForEndOfFrame();
        _textureHolder = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        _textureHolder.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        _textureHolder.Apply();
    }

    void SaveTextureToGallery()
    {
        string filename = "pet_" + DateTime.Now.ToString("dd-MM-yyyy_hh-mm-ss") + ".png";
        string albumName = "Wildlife Rescue";
        string permission = NativeGallery.SaveImageToGallery(_textureHolder, albumName, filename).ToString();
    }


    public IEnumerator AskCamPermission()
    {
#if PLATFORM_ANDROID
        Permission.RequestUserPermission(Permission.Camera);
        yield return null;
#elif PLATFORM_IOS
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
#else
        yield return null;
#endif
    }


    public bool CameraAllowed()
    {
#if PLATFORM_ANDROID
        return Permission.HasUserAuthorizedPermission(Permission.Camera);
#elif PLATFORM_IOS
        return Application.HasUserAuthorization(UserAuthorization.WebCam);
#else
        return true;
#endif
    }
}
