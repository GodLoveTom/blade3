using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainTalentNode : MonoBehaviour
{
    public Image mIcon;
    public Text mNameText;
    public Text mDesText;
    public Text mPointText;
    public Text mNeedMoney;
    public GameObject mMoneyIcon;
    public GameObject mMoneyXIcon;
    public Image mBtnImage;
    public Text mBtnTip;


    CTalent mTalent;

    static int mLvLUpCount = 0;

    public void Init(CTalent Talent)
    {
        mTalent = Talent;
        //mTalent.mPoint = 1;
        Refresh();

    }

    void Refresh()
    {
        mIcon.sprite = gDefine.gABLoad.GetSprite("icon.bytes", mTalent.mName);

        //name
        string str = gDefine.gMyStr.Get(mTalent.mName, gDefine.gPlayerData.mLanguageType);
        mNameText.text = str;

        //des
        str = gDefine.GetStr(mTalent.mDes) + ": ";

        gDefine.gPlayerData.InitBeforeGame();


        switch (mTalent.mType)
        {
            case CTalent.eTalentType.UnForce:
                str += gDefine.gPlayerData.mDamage.ToString();
                break;
            case CTalent.eTalentType.StarHeart:
                str += gDefine.gPlayerData.mHp.ToString();
                break;
            case CTalent.eTalentType.QuantumArmor:
                str += (gDefine.gPlayerData.mDamageReduce * 100).ToString("f1") + "%";
                break;
            case CTalent.eTalentType.JediSprite:
                str += (gDefine.gPlayerData.mTalent.mKillPrec + 10).ToString("f1") + "%";
                break;
            case CTalent.eTalentType.LuckAtk:
                str += (gDefine.gPlayerData.mDoubleDamagePerc).ToString("f1") + "%";
                break;
            case CTalent.eTalentType.GhostDef:
                str += (gDefine.gPlayerData.mDodgePerc).ToString("f1") + "%";
                break;
        }

        str += "<color=#00FF00FF> (" + mTalent.GetValueStr() + ")</color>";

        mDesText.text = str;

        mPointText.text = gDefine.gMyStr.Get("点数", gDefine.gPlayerData.mLanguageType) + " " + mTalent.mPoint.ToString();

        if (mTalent.mPoint > 0)
        {
            mBtnImage.color = Color.white;
            mBtnTip.text = gDefine.gMyStr.Get("升    级", gDefine.gPlayerData.mLanguageType);
            mNeedMoney.text = LVLUpNeedMoney(mTalent.mLvL).ToString();

            mMoneyIcon.SetActive(true);
            mMoneyXIcon.SetActive(false);
        }
        else
        {
            mBtnImage.color = Color.white;
            mBtnTip.text = gDefine.gMyStr.Get("升    级", gDefine.gPlayerData.mLanguageType);
            mNeedMoney.text = LVLUpNeedMoney(mTalent.mLvL).ToString();
            mMoneyIcon.SetActive(true);
            mMoneyXIcon.SetActive(false);
        }
    }
    int LVLUpNeedMoney(int LvL)
    {
        return LvL* 10 + 100;
    }

    public void Btn_UpLvL()
    {
        if (mTalent.mPoint <= 0)
        {
            gDefine.ShowTip(gDefine.GetStr(358));
            gDefine.PlaySound(71);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("失败", "缺点数"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            dic.Add("名字", gDefine.gMyStr.Get(mTalent.mName, gDefine.gPlayerData.mLanguageType));
            TalkingDataGA.OnEvent("点击天赋升级", dic);
            return;
        }
        int needMoney = LVLUpNeedMoney(mTalent.mLvL);
        if (gDefine.gPlayerData.Coin < needMoney)
        {
            gDefine.ShowTip(gDefine.GetStr(160));
            gDefine.PlaySound(71);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("失败", "缺钱"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            dic.Add("名字", gDefine.gMyStr.Get(mTalent.mName, gDefine.gPlayerData.mLanguageType)); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            TalkingDataGA.OnEvent("点击天赋升级", dic);
            return;
        }


        //if( mTalent.mPoint>0 && gDefine.gPlayerData.Coin >= 100)
        {
            mTalent.mPoint -= 1;
            gDefine.gPlayerData.Coin -= needMoney;
            mTalent.mLvL++;
            gDefine.gPlayerData.mTalent.ReCalcPlayerTalentValue();
            Refresh();
            gDefine.gPlayerData.Save();
            gDefine.PlaySound(78);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("成功", "good"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            dic.Add("名字", gDefine.gMyStr.Get(mTalent.mName, gDefine.gPlayerData.mLanguageType));
            TalkingDataGA.OnEvent("点击天赋升级", dic);

            mLvLUpCount++;
            if (mLvLUpCount >= 2)
            {
                mLvLUpCount = 0;

                gDefine.gAd.PlayInterAD(ADCallBackFunc);

                //Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Clear();
                dic.Add("来源", "天赋升级双次");
                TalkingDataGA.OnEvent("插屏广告", dic);

            }
        }
    }

    public void ADCallBackFunc(bool Finished)
    { }
}
