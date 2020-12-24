using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class firstSce : MonoBehaviour
{
    bool mInit = false;
    public Text mText;
    int num = 0;
    float mT = 0;
    AsyncOperation asyncLoad;
    float mLastT = 1.5f;

    void Start()
    {
        Debug.Log("Unity SDK  init begin ");
        TalkingDataGA.OnStart("ECC7C9154CD44F2BA40DF0249903D757", "TalkingData");
        Debug.Log("Unity SDK  init completed ");
        gDefine.account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());
        gDefine.account.SetAccountType(AccountType.ANONYMOUS);

        if (PlayerPrefs.GetInt("FirstLanguage", 0) == 0)
        {
            if (Application.systemLanguage == SystemLanguage.English)
                gDefine.gPlayerData.mLanguageType = CMyStr.eType.English;
            else if (Application.systemLanguage == SystemLanguage.ChineseSimplified)
                gDefine.gPlayerData.mLanguageType = CMyStr.eType.Simple;
            else if (Application.systemLanguage == SystemLanguage.ChineseTraditional)
                gDefine.gPlayerData.mLanguageType = CMyStr.eType.Old;
            else if (Application.systemLanguage == SystemLanguage.Japanese)
                gDefine.gPlayerData.mLanguageType = CMyStr.eType.Japanese;
            PlayerPrefs.SetInt("FirstLanguage", 1);
            PlayerPrefs.SetInt("LanguageType", (int)gDefine.gPlayerData.mLanguageType);
            PlayerPrefs.Save();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!mInit)
        {
            StartCoroutine(LoadYourAsyncScene());
            mInit = true;

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("step", "1"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            TalkingDataGA.OnEvent("loading", dic);

        }



        // if( Time.time > mT )
        //     num++;

        // num = num %6;
        // string str ="";
        // for(int i=0; i<num ; i++)
        // {
        //     str += ".";
        // }
        if (mT < mLastT)
        {
            mT += Time.deltaTime;
            if (mT < mLastT)
            {
                int num = (int)(100 * (mT / mLastT));
                mText.text = "..." + num.ToString() + "%...";
            }
            else
            {
                mText.text = "...100%...";
                asyncLoad.allowSceneActivation = true;
            }
        }



        //mText.text = str;

        // if(asyncLoad!=null)
        // {

        // }
    }

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        asyncLoad = SceneManager.LoadSceneAsync("sce0");
        asyncLoad.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
