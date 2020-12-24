using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CABLoad 
{
    bool mIsInit = true;
    public const string mtileABName="tilepreb.bytes";
    public const string propName="prop.bytes";
    public const string soundName="sound.bytes";

    Dictionary<string , AssetBundle> abDict = new Dictionary<string, AssetBundle>();


    public bool LoadABFromStreamAssets(string perbName)
    {
        string path = Application.streamingAssetsPath +"/" + perbName;
        AssetBundle ab = AssetBundle.LoadFromFile(path);
        if(ab!=null)
        {
            abDict.Add(perbName,ab);
            return true;
        }
        return false;
    
    }

    
    public  TextAsset GetTextAsset(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        return ab.LoadAsset<TextAsset>(prebName);
    }


    public string GetTextAssetBytes(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);

        TextAsset asset = ab.LoadAsset<TextAsset>(prebName);

        if (asset != null)
            return asset.text;
        else

            return null;
    }

    public  Image GetImage(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        return ab.LoadAsset<Image>(prebName);
    }

    public Texture2D GetTex2D(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        return ab.LoadAsset<Texture2D>(prebName);
    }

    public Sprite GetSprite(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        return ab.LoadAsset<Sprite>(prebName);
    }

    public GameObject GetPreb(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        return ab.LoadAsset<GameObject>(prebName);
    }

    public  AudioClip GetSoundClipAsset(string packageName, string clipName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        return ab.LoadAsset<AudioClip>(clipName);
    }


    public bool IsReady()
    {
        return mIsInit;
    }

    public GameObject GetTilePreb(string prebName)
    {
            AssetBundle ab = GetAssetBundle(mtileABName);
            GameObject obj = ab.LoadAsset<GameObject>(prebName);
            return GameObject.Instantiate(obj);
    }


    public GameObject CreateObj(string packageName, string prebName)
    {
        AssetBundle ab = GetAssetBundle(packageName);
        GameObject obj = ab.LoadAsset<GameObject>(prebName);
        return GameObject.Instantiate(obj);
    }

    public GameObject LoadProp(string prebName)
    {
            AssetBundle ab = GetAssetBundle(propName);
            GameObject obj = ab.LoadAsset<GameObject>(prebName);
            return GameObject.Instantiate(obj);
    }


    public GameObject GetTilePreb(long index)
    {
        string prebname = "preb"+index.ToString();

            AssetBundle ab = GetAssetBundle(mtileABName);
            GameObject obj = ab.LoadAsset<GameObject>(prebname);
            return GameObject.Instantiate(obj);
    }


    public AssetBundle GetAssetBundle( string abstr)
    {
        AssetBundle ab ;
        abDict.TryGetValue(abstr, out ab);
        return ab;
    }

    public string GetABPath(string filePathName)
    {
        
        string path =
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.streamingAssetsPath + "/ab/ab1.bytes";
#elif UNITY_IPHONE && !UNITY_EDITOR
        "file:///" + Application.streamingAssetsPath + "/ab/ab1.bytes";
#elif UNITY_STANDLONE_WIN||UNITY_EDITOR
        "file:///" + Application.dataPath + "/AssetBundles/" + filePathName;
#else
        string.Empty;
#endif
return path;
        //StartCoroutine(ReadData(path));
    }

    //public IEnumerator ReadData(string path, string name)
    //{

    //    UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(path, 0);
    //    yield return request.SendWebRequest();
    //    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
    //    abDict.Add(name,bundle);
    //    mIsInit = true;
    //}




    
}
