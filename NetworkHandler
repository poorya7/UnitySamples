using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

//fetches images from cms using api calls

public class NetworkHandler : MonoBehaviour
{

    string _token;
    public Texture2D _textureHolder;

    public static NetworkHandler Instance;

    void Awake()
    {
        Instance = this;
    }


    public IEnumerator FetchImages()
    {
        _token = null;
        yield return StartCoroutine(GetToken());
        if (_token != null)
        {
            string url = ConfigLoader.Instance.GetURLList();
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.SetRequestHeader("Authorization", _token);
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success)
                ExtractData(www.downloadHandler.text);
            else
                Debug.Log(www.error);
        }
    }


    void ExtractData(string json)
    {
        JSONArray images = (JSONArray)JSON.Parse(json)["data"];
        foreach(JSONNode image in images)
        {
            ImageInfo imageInfo = new ImageInfo();
            imageInfo._code = image["code"];
            imageInfo._date = DateTime.Parse(image["date"]);
            imageInfo._url= image["image"];
            ImageTracker.Instance.AddImage(imageInfo);
        }
        ImageTracker.Instance.SortByDate();
    }


    public IEnumerator GetImageFromURL(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
            _textureHolder = ((DownloadHandlerTexture)www.downloadHandler).texture;
        else
            Debug.Log("*** Error downloading image from url: " + url);
        www.Dispose();
    }


    public Texture2D GetTexture()
    {
        return _textureHolder;
    }


    string ToJsonString(string name, string email, string code)
    {
        string output = "{ ";
        output += "\"name\":" + "\""+ name + "\",";
        output += "\"email\":" + "\""+email + "\",";
        output += "\"code\":" + "\"" + code + "\"";
        output += " }";
        return output;
    }


    IEnumerator GetToken()
    {
        string username = ConfigLoader.Instance.GetUsername();
        string password = ConfigLoader.Instance.GetPassword();
        string url = ConfigLoader.Instance.GetURLAuth();
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
        if (www.result == UnityWebRequest.Result.Success)
            _token = "JWT " + JSON.Parse(www.downloadHandler.text)["data"]["token"];
        else
            Debug.Log(www.error);
    }

}
