
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ui_3Choose1InFight : MonoBehaviour
{
    public Text mName;
    public Text mDes;
    public Text mBtnText;
    public Text mBtnAllText;

    public Image[] mBtnImage;
    public Text[] mSkillNameText;
    public Text[] mSkillDesText;
    public GameObject[] mSelectFrame;
    public GameObject mBtnAll;
    public GameObject mBtnChange;

    public ui_3Choose1Confirm mConfirmScript;
    public GameObject mPage0;
    public GameObject mRoot;
    int mIndex = -1;
    List<CSkill> mChoose;

    float mDelayT = 0;

    public GameObject mEnObj;// 英文装饰
    public GameObject mCHObj; //汉语装饰

    public GameObject mMaskObj;
    float mMaskT = 0;

    public void ADCallBack(bool Finished)
    {

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(Time.time > mMaskT)
        {
            mMaskObj.SetActive(false);
        }
    }

    public void Show()
    {
        Refresh();
        gameObject.SetActive(true);
        mPage0.SetActive(true);
        mConfirmScript.gameObject.SetActive(false);
        mDelayT = Time.time + 0.6f;
        gDefine.gPause = true;
        mMaskObj.SetActive(false);

        if(gDefine.gLogic.mWaveLvL == 4)
        {
             gDefine.gAd.PlayInterAD1(ADCallBack);
             mMaskObj.SetActive(true);
             mMaskT = Time.time +3;
        }
        else if( gDefine.gChapterId == 12 ||  gDefine.gChapterId == 21 ||
         gDefine.gChapterId == 33 ||  gDefine.gChapterId == 47)
         {
             gDefine.gAd.PlayInterAD(ADCallBack);
             mMaskObj.SetActive(true);
             mMaskT = Time.time +3;
         }

          Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
           
    }

    public void Refresh()
    {
        gDefine.RecalcAutoSize(mRoot);
        mChoose = gDefine.gSkill.CalcChoose3();
        bool isEnglish = gDefine.gPlayerData.mLanguageType == CMyStr.eType.English;
        for (int i = 0; i < mChoose.Count; i++)
        {
            mBtnImage[i].sprite = gDefine.gABLoad.GetSprite("icon.bytes", mChoose[i].mName);
            mSelectFrame[i].SetActive(false);
            mSkillNameText[i].text = gDefine.GetStr(mChoose[i].mName);
            gDefine.SetTextBold();
            mSkillDesText[i].text = mChoose[i].GetDes();
            gDefine.SetTextBold();
            mSkillNameText[i].fontSize = isEnglish?17:20;
            mSkillNameText[i].fontStyle = isEnglish?FontStyle.Italic:FontStyle.Normal;

        }
        //mSkillNameText[3].text = "";

        mName.text = gDefine.GetStr("请选择一个技能");
        gDefine.SetTextBold();
        mBtnText.text = gDefine.GetStr("换一批");
        gDefine.SetTextBold();
        mBtnAllText.text = gDefine.GetStr("全部都要");
        gDefine.SetTextBold();
        //mDes.text = "";
        mIndex = -1;

        if (Random.Range(0, 100) < 20)
            mBtnAll.SetActive(true);
        else
        {
            mBtnAll.SetActive(false);
        }

        if(isEnglish)
        {
            mCHObj.SetActive(false);
            mEnObj.SetActive(true);
        }
        else 
        {
            mCHObj.SetActive(true);
            mEnObj.SetActive(false);
        }
    }

    public void Btn_ChangeADCAllBack(bool Finished)
    {
        if (Finished)
        {
            Refresh();
        }
    }

    public void Btn_ChangeCallBack()
    {
        gDefine.gAd.PlayADVideo(Btn_ChangeADCAllBack);
        Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "1/3换一波"); 
            TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void Btn_Change()
    {
        gDefine.gBtnAnim.Init(mBtnChange, 1, Btn_ChangeCallBack);
    }


    public void Btn_Choose(int Index)
    {
        // if (mIndex < 0 || mIndex != Index)
        // {
        //     if (mIndex >= 0) mSelectFrame[mIndex].SetActive(false);
        //     mIndex = Index;
        //     mSelectFrame[Index].SetActive(true);
        //     mDes.text = gDefine.GetStr(mChoose[Index].mDes);
        //     mSkillNameText[3].text = gDefine.GetStr(mChoose[Index].mName);

        // }
        // else
        {
            mConfirmScript.Init(mChoose[Index], this);
            mPage0.SetActive(false);
        }
    }

    public void Btn_AllADCAllBack(bool Finished)
    {
        if (Finished)
        {
            for (int i = 0; i < 3; i++)
            {
                gDefine.gPlayerData.AddLearnSkillInFigth(mChoose[i]);
                gDefine.gSkill.Choose(mChoose[i]);
            }

            gameObject.SetActive(false);
            gDefine.gPause = false;
            gDefine.gLogic.GoNextLvLChange();
        }
    }

    public void Btn_AllCallBack()
    {
        gDefine.gAd.PlayADVideo(Btn_AllADCAllBack);
        Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "1/3 全部获得"); 
            TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void Btn_All()
    {
        if (Time.time <= mDelayT)
            return;

        gDefine.gBtnAnim.Init( mBtnAll,1, Btn_AllCallBack  );
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}
