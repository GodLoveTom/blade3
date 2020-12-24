using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_3Choose1Confirm : MonoBehaviour
{
    public Text mNameText;
    public Text mDesText;
    public Image mIconImage;
    CSkill mSkillData;
    ui_3Choose1InFight mFather;
    public Text mBtnText;

    public GameObject mBtn2;
    public GameObject mBtn2Chinese;
    public Text mBtn2Text;
    public Text mBtn2ChineseText;


    public void Init(CSkill SkillData, ui_3Choose1InFight UI31)
    {
        mSkillData = SkillData;
        mNameText.text = gDefine.GetStr(SkillData.mName);
       // gDefine.SetTextBold(mNameText);
        mDesText.text = SkillData.GetDes();
       // gDefine.SetTextBold(mDesText);
        mIconImage.sprite = gDefine.gABLoad.GetSprite("icon.bytes", SkillData.mName);
        mFather = UI31;

        mBtnText.text = gDefine.GetStr("获    得");
        gDefine.SetTextBold();
        gDefine.RecalcAutoSize(gameObject);
        gameObject.SetActive(true);
        mBtn2Text.text = gDefine.GetStr(360);
        gDefine.SetTextBold();
        mBtn2ChineseText.text = gDefine.GetStr(360);
        gDefine.SetTextBold();

        mBtn2.SetActive(false);
        mBtn2Chinese.SetActive(false);
        if (SkillData.mType == CSkill.eSkill.DSWordGirl_FlashKill ||
         SkillData.mType == CSkill.eSkill.BigLun_Cut || SkillData.mType == CSkill.eSkill.DeathFinger ||
         SkillData.mType == CSkill.eSkill.GunMaster || SkillData.mType == CSkill.eSkill.Thunder || SkillData.mType == CSkill.eSkill.DamageUpSmall
         || SkillData.mType == CSkill.eSkill.DamageUpBig || SkillData.mType == CSkill.eSkill.HealLight
         || SkillData.mType == CSkill.eSkill.LifeSpring || SkillData.mType == CSkill.eSkill.FirePowerUp
         || SkillData.mType == CSkill.eSkill.DamageMore || SkillData.mType == CSkill.eSkill.FirePowerUp ||
         SkillData.mType == CSkill.eSkill.GroundArrow || SkillData.mType == CSkill.eSkill.QuantumMask ||
         SkillData.mType == CSkill.eSkill.LightBall)
        {
           // if (Random.Range(0, 100) < 30 || SkillData.mType ==CSkill.eSkill.DamageMore )
            {
                if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.Old ||
                    gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple)
                    mBtn2Chinese.SetActive(true);
                else
                    mBtn2.SetActive(true);
            }
        }
    }

    public void Btn_Get()
    {
        gDefine.gPlayerData.AddLearnSkillInFigth(mSkillData);
        gameObject.SetActive(false);
        mFather.Close();
        gDefine.gSkill.Choose(mSkillData);
        gDefine.gPause = false;
        gDefine.gLogic.GoNextLvLChange();
    }

    public void Btn_GetDoubleADCallBack(bool Finished)
    {
        if (Finished)
        {
            gDefine.gPlayerData.AddLearnSkillInFigth(mSkillData);
            gameObject.SetActive(false);
            mFather.Close();
            gDefine.gSkill.Choose(mSkillData);
            gDefine.gPause = false;
            gDefine.gLogic.GoNextLvLChange();
        }
    }

    public void Btn_GetDoubleCallBack()
    {
        gDefine.gAd.PlayADVideo(Btn_GetDoubleADCallBack);
         Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "3/1 双倍"); 
            dic.Add("名称", mSkillData.mName); 
            TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void Btn_GetDouble(int Index)
    {
        if(Index==0)
            gDefine.gBtnAnim.Init(mBtn2Chinese ,1, Btn_GetDoubleCallBack);
        else
            gDefine.gBtnAnim.Init( mBtn2, 1, Btn_GetDoubleCallBack);
    }
}
