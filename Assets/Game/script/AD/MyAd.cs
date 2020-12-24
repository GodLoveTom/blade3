using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyAd : MonoBehaviour
{
    public Adtest mAdmob;
    public delegate void BackFunc(bool Finished);
    BackFunc mFunc;
    BackFunc mInterFunc;
    BackFunc mInterFunc1;
    public Example mByteDanceAD;

    public Text mText; 

    bool mIsUseAdmob = true;

    

    public void AdMobInst1CallBack()
    {
        if (mInterFunc1 != null)
            mInterFunc1(true);
        mInterFunc1 = null;

        gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
    }



    public void ADMobCallBack(bool Finished)
    {
        if (mFunc != null)
            mFunc(Finished);
        mFunc = null;
         
        gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
    }

    public void AdMobInstCallBack()
    {
        if (mInterFunc != null)
            mInterFunc(true);
        mInterFunc = null;

        gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
    }

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gAd = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(mIsUseAdmob)
            return;

        //vide0
        // int s = mByteDanceAD.GetState();

        // mText.text = s.ToString() +":"+ mByteDanceAD.information;

        // if (mFunc != null && mByteDanceAD.GetState() == 2)
        // {
        //     mByteDanceAD.ShowExpressRewardAd();
        // }
        // else
        // {
        //     int r = mByteDanceAD.GetState();
        //     if (r == 5 || r == 6)
        //     {
        //         if (mFunc != null)
        //             mFunc(r == 5 ? false : true);
        //         mFunc = null;

        //         gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        //       gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
        //     }


        //     if (mByteDanceAD.GetState() == 0 || mByteDanceAD.GetState() == 5 || mByteDanceAD.GetState() == 6)
        //     {
        //         mByteDanceAD.LoadExpressRewardAd();
        //     }
        // }

        //inter
        // s = mByteDanceAD.GetInterADState();
        //  mText.text = s.ToString() +":"+ mByteDanceAD.information;
        //  if (mInterFunc != null && mByteDanceAD.GetInterADState() == 2)
        // {
        //     mByteDanceAD.ShowExpressInterstitialAd();
        // }
        // else
        // {
            
        //     if (s == 5 || s  == 4)
        //     {
        //         if (mInterFunc != null)
        //             mInterFunc((s == 5 )? false : true);
        //         mInterFunc = null;

        //         gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        //         gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
        //     }


        //     if (s == 0 || s == 4 || s==5)
        //     {
        //         mByteDanceAD.LoadExpressInterstitialAd();
        //     }
        // }
    }

    public void PlayADVideo(BackFunc Func)
    {
        if(Func!=null)
            Func(true);
        return;

        // if(!Application.isMobilePlatform)
        // {
        //     Func(true);

        //      gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        //       gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);
        // }
        // else
        // {
        //     if( mAdmob.rewardIsLoad)
        //     {
        //         mFunc = Func;
        //         gDefine.gSound.EnableSound(false);
        //         gDefine.gSound.EnableMusic(false);

        //         mAdmob.Btn_ShowReward();
        //         mIsUseAdmob = true;
        //     }
        //     #if IAPWORLD
        //     #else
        //     else if (mByteDanceAD.GetState() == 2)
        //     {
        //         mFunc = Func;
        //         gDefine.gSound.EnableSound(false);
        //         gDefine.gSound.EnableMusic(false);

        //         mIsUseAdmob = false;
        //         mByteDanceAD.ShowExpressRewardAd();
        //     }
        //     #endif
        //     else
        //     {
        //         Func(false);
        //     }
        // }
    }

    public void PlayInterAD(BackFunc Func)
    {
         if(Func!=null)
                Func(true);
        return;

        // if(!Application.isMobilePlatform || PlayerPrefs.GetInt("removeInterAD", 0) == 1
        // || PlayerPrefs.GetInt("vip", 0) == 1)
        // {
        //     if( Func!=null)
        //         Func(true);
        //     return ;
        // }

        // if( mAdmob.instIsLoad)
        // {
        //     mInterFunc = Func;
        //     gDefine.gSound.EnableSound(false);
        //     gDefine.gSound.EnableMusic(false);
        //     mAdmob.Btn_ShowInstAD();
        //     mIsUseAdmob = true;
        // }
        // #if IAPWORLD
        // #else
        // else if( mByteDanceAD.GetInterADState() == 2)
        // {
        //     gDefine.gSound.EnableSound(false);
        //     gDefine.gSound.EnableMusic(false);
        //     mInterFunc = Func;
        //     mByteDanceAD.ShowExpressInterstitialAd();
        //     mIsUseAdmob = false;
        // }
        // #endif
        // else
        // {
        //     if(Func!=null)
        //         Func(false);
        // }
        
        // mInterFunc = Func;
        // //if( mByteDanceAD.GetInterADState() == 2)
        // mByteDanceAD.ShowExpressInterstitialAd();
        //  gDefine.gSound.EnableSound(false);
        //   gDefine.gSound.EnableMusic(false);

        
    }

    //图片插屏
    public void PlayInterAD1(BackFunc Func)
    {
        if(Func!=null)
                Func(true);
        return;
        
        // if( !Application.isMobilePlatform || PlayerPrefs.GetInt("removeInterAD", 0) == 1
        // || PlayerPrefs.GetInt("vip", 0) == 1)
        // {
        //     if(Func!=null)
        //      Func(true);
        //     return ;
        // }

        // if( mAdmob.instIsLoad1)
        // {
        //     mInterFunc1 = Func;
        //     //gDefine.gSound.EnableSound(false);
        //     //gDefine.gSound.EnableMusic(false);
        //     mAdmob.Btn_ShowInstAD1();
        //     mIsUseAdmob = true;
        // }
        // #if IAPWORLD
        // #else
        // else if( mByteDanceAD.GetInterADState() == 2)
        // {
        //     //gDefine.gSound.EnableSound(false);
        //     //gDefine.gSound.EnableMusic(false);
        //     mInterFunc = Func;
        //     mByteDanceAD.ShowExpressInterstitialAd();
        //     mIsUseAdmob = false;
        // }
        // #endif
        // else
        // {
        //     if(Func!=null)
        //         Func(false);
        // }
        
        // mInterFunc = Func;
        // //if( mByteDanceAD.GetInterADState() == 2)
        // mByteDanceAD.ShowExpressInterstitialAd();
        //  gDefine.gSound.EnableSound(false);
        //   gDefine.gSound.EnableMusic(false);

        
    }

    public void TestBtn()
    {
        if(mAdmob.instIsLoad)
        {
            mIsUseAdmob = true;
             mAdmob.Btn_ShowInstAD();
        }
        else
        {
             mByteDanceAD.ShowExpressInterstitialAd();
             mIsUseAdmob = false;
        }
           
         
    }





}
