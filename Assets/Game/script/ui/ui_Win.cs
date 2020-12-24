using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Win : MonoBehaviour
{
    public GameObject mRoot;
    public GameObject mCloseBtn;
    public GameObject mNextBtn;

    public MyMusic mRefMusic;

    public void Show(int Chapter, int LVL, int Coins)
    {
        gDefine.gPlayerData.ChapterDifficultFinished();

        // if(ItemId == 201 || ItemId == 202)
        {
            PlayerPrefs.SetInt("iap_door", 1);
            PlayerPrefs.Save();
        }

        GameObject[] arr = GameObject.FindGameObjectsWithTag("it");
        for (int i = 0; i < arr.Length; i++)
            GameObject.Destroy(arr[i]);

        arr = GameObject.FindGameObjectsWithTag("it1");
        for (int i = 0; i < arr.Length; i++)
            GameObject.Destroy(arr[i]);

        arr = GameObject.FindGameObjectsWithTag("pet");
        for (int i = 0; i < arr.Length; i++)
            GameObject.Destroy(arr[i]);

        PlayerPrefs.SetInt("continueFight", 0);
        gameObject.SetActive(true);
        gDefine.RecalcAutoSize(mRoot);
        //gDefine.gPlayerData.MaxChapterId = gDefine.gChapterId + 1;
        gDefine.gPlayerData.mChapterEx.AddLvLFinish(gDefine.gChapterId, gDefine.gChapterDifficult, gDefine.gLVLNumInChapter);

        PlayerPrefs.SetInt("firstlvl", 1);

        TDGAMission.OnCompleted("Chap_" + gDefine.gChapterId.ToString());
        TDGAMission.OnCompleted(gDefine.gWaveStr);

        Animator a = gameObject.GetComponent<Animator>();
        a.Play("win",0,0);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void CallBackFunc(bool Finished)
    {
    }

    public void Btn_Close()
    {
        gDefine.gBtnAnim.Init(mCloseBtn, 1, CloseCallBack);
    }

    public void Event_PlayAD()
    {
        gDefine.gAd.PlayInterAD(CallBackFunc);

        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("来源", "过关胜利");
        TalkingDataGA.OnEvent("插屏广告", dic);
    }

    public void CloseCallBack()
    {
        gameObject.SetActive(false);
        gDefine.GoToMainUI();
        mRefMusic.BeginPlayUIMusic();
    }

    public void ContineCallBack()
    {
        //开始下一次战斗
        if (gDefine.gChapterId < 10)
        {
            //if (gDefine.gPlayerData.TiLI >= 5)
            //{
            //gDefine.gPlayerData.TiLI-=5;
            gDefine.gForbidLvL = 0;
            gDefine.gChapterId++;
            CChapterData chapter = gDefine.gData.GetChapterData(gDefine.gChapterId);
            gDefine.gLVLNumInChapter = chapter.GetWaveNum();
            gDefine.ShowGameSce(0);
            gDefine.gMainUI.Close();
            //}
            // else
            // {


            //     gDefine.GoToMainUI();
            // }
        }
        else
        {
            gDefine.GoToMainUI();

            gDefine.gAd.PlayInterAD(CallBackFunc);

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("来源", "过关胜利");
            TalkingDataGA.OnEvent("插屏广告", dic);
        }

        gameObject.SetActive(false);
    }


    public void Btn_Continue()
    {
        gDefine.gBtnAnim.Init(mNextBtn, 1, ContineCallBack);
    }
}
