using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_fail : MonoBehaviour
{
    public GameObject mBtnAD;
    public GameObject mBtnClose;
    public GameObject mDie2Dlg;
    public Text mTextNum;
    public Text mBtnTip;
    public Text mBtnTip0;
    public GameObject mAdObj;
    public Text mTip;
    float mT;
    bool mIsFirst = true;

    public GameObject mRoot;
    bool mStop = false;
    bool mPause = false;
    bool mPlayAD = false;

    // Start is called before the first frame update
    void Start()
    {
        //gDefine.gUIFail = this;
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (mStop||mPause)
            return;

        if (mT <= 0)
        {
            CallBack0Func(false);
            //if(!mPlayAD)
            // gDefine.gAd.PlayInterAD(CallBack0Func);

            // Dictionary<string, object> dic = new Dictionary<string, object>();
            
            // dic.Add("来源", "过关失败"); 
            // TalkingDataGA.OnEvent("插屏广告", dic);
            //mPlayAD = true;

            //if(mT < -30)
               

            return;
        }
        mT -= Time.deltaTime;
        if(mT<=0)
             mTextNum.text ="0";
        else
            mTextNum.text = ((int)mT).ToString();
    }

     public void CallBack0Func(bool Finished)
    {
            PlayerPrefs.SetInt("continueFight", 0);
            //--reStart--

            GameObject[] arr = GameObject.FindGameObjectsWithTag("it");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            arr = GameObject.FindGameObjectsWithTag("it1");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            arr = GameObject.FindGameObjectsWithTag("pet");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);


            gDefine.GoToMainUI();
            gameObject.SetActive(false);
    }

    public void Btn_AD()
    {
        gDefine.gBtnAnim.Init(mBtnAD, 1, ADAnimCallBack);
    }

    public void ADAnimCallBack()
    {
        if (mIsFirst)
        {
            //.....continue....
            mStop = true;
            gDefine.gAd.PlayADVideo(ADCallBackFunc);
             Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "死亡复活"); 

            TalkingDataGA.OnEvent("激励视频广告", dic);
        }
        else
        {
            PlayerPrefs.SetInt("continueFight", 0);
            gDefine.GoToMainUI();
        }
        gameObject.SetActive(false);
    }

    public void ADCallBackFunc(bool Finished)
    {
        if (Finished)
        {
            gDefine.ReliveHero();
            mIsFirst = false;
            gDefine.PlaySound(54);
            PlayerPrefs.SetInt("Relive",1);
        }
        else
        {
            mStop = false;
        }
    }

    public void Btn_ReStart()
    {
        mT = (int)mT - 1;
        if (mT <= 0)
        {

            GameObject[] arr = GameObject.FindGameObjectsWithTag("it");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            arr = GameObject.FindGameObjectsWithTag("it1");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            arr = GameObject.FindGameObjectsWithTag("pet");
            for (int i = 0; i < arr.Length; i++)
                GameObject.Destroy(arr[i]);

            //--reStart--
            gDefine.GoToMainUI();
            gameObject.SetActive(false);

            PlayerPrefs.SetInt("continueFight", 0);

            return;
        }
        mTextNum.text = ((int)mT).ToString();
    }

    public void BtnQuitCallBack()
    {
        PlayerPrefs.SetInt("continueFight", 0);

        GameObject[] arr = GameObject.FindGameObjectsWithTag("it");
        for (int i = 0; i < arr.Length; i++)
            GameObject.Destroy(arr[i]);

        arr = GameObject.FindGameObjectsWithTag("it1");
        for (int i = 0; i < arr.Length; i++)
            GameObject.Destroy(arr[i]);

        arr = GameObject.FindGameObjectsWithTag("pet");
        for (int i = 0; i < arr.Length; i++)
            GameObject.Destroy(arr[i]);

        //--reStart--
        gDefine.GoToMainUI();
        gameObject.SetActive(false);

        PlayerPrefs.GetInt("Relive",0);

        TDGAMission.OnFailed("Chap_" + gDefine.gChapterId.ToString(), "dead");
        TDGAMission.OnFailed(gDefine.gWaveStr, "dead");


    }

    public void Btn_GoToMainUI()
    {
        gDefine.gBtnAnim.Init(mBtnClose, 1, BtnQuitCallBack);
        TDGAMission.OnFailed("Chap_" + gDefine.gChapterId.ToString(), "dead");
        TDGAMission.OnFailed(gDefine.gWaveStr, "dead");
    }

    public void InitBeforeGame()
    {
        mIsFirst = PlayerPrefs.GetInt("Relive",0)==0?true:false;
        mStop = false;
    }

    public void Show()
    {
        mT = 10 + 0.9f;
        mTextNum.text = ((int)mT).ToString();
        gameObject.SetActive(true);
        Refresh();
        mStop = false;
        mPlayAD = false;

        mPause = true;
        gDefine.gAd.PlayInterAD(CallBackFunc);
        Dictionary<string, object> dic = new Dictionary<string, object>();
        
        dic.Add("来源", "过关失败"); 
        TalkingDataGA.OnEvent("插屏广告", dic);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

       // gDefine.gAd.TestBtn();
    }

    void Refresh()
    {
        if (mIsFirst)
        {
            mRoot.SetActive(true);
            mDie2Dlg.SetActive(false);
            mTip.text = gDefine.GetStr(278);
            gDefine.SetTextBold();
            mBtnTip.text = gDefine.GetStr(279);
            gDefine.SetTextBold();

            mBtnTip.gameObject.SetActive(true);
            mAdObj.SetActive(true);
            mBtnTip0.gameObject.SetActive(false);
        }
        else
        {
            mRoot.SetActive(false);
            mDie2Dlg.SetActive(true);

            // gDefine.gAd.PlayInterAD(CallBackFunc);
            //  Dictionary<string, object> dic = new Dictionary<string, object>();
            
            // dic.Add("来源", "过关失败"); 
            // TalkingDataGA.OnEvent("插屏广告", dic);

            // mTip.text = gDefine.GetStr(280);
             mBtnTip0.text = gDefine.GetStr(272);
             gDefine.SetTextBold();

            //mBtnTip.gameObject.SetActive(false);
             mAdObj.SetActive(false);
             mBtnTip0.gameObject.SetActive(true);
        }

        gDefine.RecalcAutoSize(mRoot);
        gDefine.RecalcAutoSize(mDie2Dlg);

    }

    public void CallBackFunc(bool Finished)
    {
        mPause = false;
    }


}
