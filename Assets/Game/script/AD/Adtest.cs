using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GoogleMobileAds.Placement;
using GoogleMobileAds.Api;

using UnityEngine.UI;


public class Adtest : MonoBehaviour
{
    public Text mText;
    public MyAd mMyAd;
    // public Text mText;
    // public GameObject preb;
    // public GameObject adObj; 
    public RewardedAdGameObject rewardedAdGameObject;
    public InterstitialAdGameObject interstitialAd;
    public InterstitialAdGameObject interstitialAd1; //纯图片

    public bool rewardIsLoad = false;
    public bool userEarnReward = false;
    public bool instIsLoad = false;
    public bool instIsLoad1 = false;

    public bool rewardIsFailed = false;
    public float rewardT = 0;

     public bool instIsFailed = false;
    public float instT = 0;

      public bool instIsFailed1 = false;
    public float instT1 = 0;



    public string str = "";
    bool mInit = false;



    // Start is called before the first frame update
    void Start()
    {
        str ="";
        MobileAds.Initialize((initStatus) =>
        {
            Debug.Log("Initialized MobileAds");
        });

    }

    // Update is called once per frame
    void Update()
    {
        // if(rewardIsFailed&& Time.time > rewardT)
        // {
        //     //str += "begin load reward----/n" + rewardIsFailed.ToString();
        //     rewardT = Time.time + 30;
        //     rewardIsFailed = false;
        //     Btn_LoadReward();
        // }

        // if(instIsFailed&& Time.time > instT)
        // {
        //     //str += "begin load isnt";
        //     instT = Time.time+ 30;
        //     instIsFailed=false;
        //     Btn_LoadInstAD();
        // }

        // if(instIsFailed1&& Time.time > instT1)
        // {
        //     // str += "begin load isnt1";
        //     instT1 =Time.time +  30;
        //     instIsFailed1 = false;
        //     Btn_LoadInstAD1();
        // }

        /* 
        //    mText .text = str;
        //     if(mInit==false)
        //     {
        //         mInit = true;
        //         OnClickLoad();
        //         Btn_LoadInstAD();
        //     }
        */
    }

    public void Btn_ShowReward()
    {
        rewardIsLoad = false;
        userEarnReward = false;
        rewardedAdGameObject.ShowIfLoaded();
    }

    public void Btn_LoadReward()
    {
        rewardedAdGameObject.LoadAd();
    }

    public void OnRewardLoad()
    {

        rewardIsLoad = true;
        // str += "Load  reward ok---XXXX/n";
        //  GameObject o = GameObject.Find("TextBtn");
        // o.GetComponent<Text>().text = "Load ok";
    }

     public void OnRewardFailedLoad(string Str)
    {
        rewardIsFailed = true;
        rewardT = Time.time + 30;
       // str += "Load  reward failed---" + str +"/n";
        //  GameObject o = GameObject.Find("TextBtn");
        // o.GetComponent<Text>().text = "Load ok";
    }

     public void OnInstFailedLoad(string Str)
    {
        instIsFailed = true;
        instT = Time.time + 30;
       //  str += "Load inst failed---" + str +"/n";
        //  GameObject o = GameObject.Find("TextBtn");
        // o.GetComponent<Text>().text = "Load ok";
    }

     public void OnInst1FailedLoad(string Str)
    {
        instIsFailed1 = true;
        instT1 = Time.time + 30;
       // str += "Load inst 1 failed---" + str +"/n";
        //  GameObject o = GameObject.Find("TextBtn");
        // o.GetComponent<Text>().text = "Load ok";
    }

    public void onRewardClose()
    {
        // str += "onADClose ---/n";
        // GameObject o = GameObject.Find("textbtn");
        // o.GetComponent<Text>().text = "onADClose";
        mMyAd.ADMobCallBack(userEarnReward);

        Btn_LoadReward();
    }

    public void OnUserEarnedReward(Reward reward)
    {
        // Debug.Log("OnUserEarnedReward: reward=" +
        //     reward.Type + ", amount=" + reward.Amount);
         //str += "OnUserEarnedReward----/n";
        userEarnReward = true;
    }

    public void onADShow()
    {
        // str += "OnAdShow----/n";
        // Debug.Log("OnAdShow----/n");
    }

    public void Btn_LoadInstAD()
    {
        interstitialAd.LoadAd();
    }

    public void Btn_ShowInstAD()
    {
        instIsLoad = false;
        interstitialAd.ShowIfLoaded();
    }

    public void OnInstLoadOK()
    {
        instIsLoad = true;
       // str += "OnInstLoadOK----/n";
        // Debug.Log("OnInstLoadOK----/n");
    }

    public void OnInstClosed()
    {
         //str += "OnInstClosed----/n";
        // Debug.Log("OnInstClosed----/n");
        Btn_LoadInstAD();
        gDefine.gAd.AdMobInstCallBack();
    }

    public void Btn_LoadInstAD1()
    {

        interstitialAd1.LoadAd();
    }

    public void Btn_ShowInstAD1()
    {
        instIsLoad1 = false;
        interstitialAd1.ShowIfLoaded();
    }

    public void OnInstLoadOK1()
    {
        instIsLoad1 = true;
        //str += "OnInst1LoadOK----/n";
        // Debug.Log("OnInstLoadOK----/n");
    }

    public void OnInstClosed1()
    {
         //str += "OnInstClosed----/n";
        // Debug.Log("OnInstClosed----/n");
        Btn_LoadInstAD1();
        gDefine.gAd.AdMobInst1CallBack();
    }


}
