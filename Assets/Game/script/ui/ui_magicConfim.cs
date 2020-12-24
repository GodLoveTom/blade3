using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_magicConfim : MonoBehaviour
{
    public GameObject mBtnGet;
    public GameObject mBtnGet2;
    public Text mNameText;
    public Text mDesText;
    public Image mIconImage;
    CMagicData mMagicData;
    ui_Magic mRefRoot;

    public Text mBtnText;
    public Text mBtn2Text;

    public void Init(CMagicData MagicData, ui_Magic UIMagic)
    {
        mMagicData = MagicData;
        mNameText.text = mMagicData.GetName();
        gDefine.SetTextBold();
        mDesText.text = mMagicData.GetDes();
        gDefine.SetTextBold();
        mIconImage.sprite = mMagicData.GetIcon();
        mRefRoot = UIMagic;

        mBtnText.text = gDefine.GetStr("获    得");
        gDefine.SetTextBold();
        mBtn2Text.text = gDefine.GetStr(360);
        gDefine.SetTextBold();
        if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.English)
            mBtn2Text.fontSize = 38;
        else if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old ||
        gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple)
            mBtn2Text.fontSize = 50;

    }

    public void Btn_GetCallBack()
    {
        gDefine.gMagic.GetMagic(mMagicData.mType);
        mRefRoot.Close();
        gDefine.gLogic.GoNextLvLChange();
    }

    public void Btn_Get()
    {
        gDefine.gBtnAnim.Init(mBtnGet, 1, Btn_GetCallBack);
    }

    public void Btn_GetDoubleCallBack()
    {
        gDefine.gAd.PlayADVideo(ADGetDloubleCallBack);
         Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "附魔 双倍或者取消"); 
             dic.Add("名称", mMagicData.GetName()); 

            TalkingDataGA.OnEvent("激励视频广告", dic);
    }


    public void Btn_GetDouble()
    {
       gDefine.gBtnAnim.Init(mBtnGet2, 1, Btn_GetDoubleCallBack);
    }

    public void ADGetDloubleCallBack(bool Finished)
    {
        if (Finished)
        {
            gDefine.gMagic.GetMagicDouble(mMagicData.mType);
            mRefRoot.Close();
            gDefine.gGameMainUI.mRefLVLTipUI.ContinueTask();
        }
    }
}
