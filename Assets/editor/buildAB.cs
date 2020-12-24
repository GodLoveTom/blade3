using UnityEditor;
using System.IO;
using UnityEngine;

public class buildAB
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = Application.streamingAssetsPath;
        if(!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.CompleteAssets, BuildTarget.iOS);
    }
}