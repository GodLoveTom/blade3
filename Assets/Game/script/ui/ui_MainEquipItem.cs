using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipItem : MonoBehaviour
{
    public Image mIcon;
    public Image[] mGemIcon3 = new Image[3];
    public Text [] mGemLvL3 = new Text[3];
    public Text mName;
    public Text mPinZhi;

    public Image mPieceImage;

    public Text mLvL;
    public Text mFenJieBtnText;
    public Text mSpecialTipText;
    public Text mGemTipTex;
    public Text[] mMainParam = new Text[2];

    //-----------------//
    public Text[] mGem = new Text[2];

    //-----------------//
    public Text[] mSpcial = new Text[3];

    //----------------//
    public Text mPiece;

    public Text mEquipBtnText;
    public Text mUpgradeBtnText;
    public Text mUpgradeBtnTMoneyText;
    public GameObject mUpgradeBtnCrystal;
    public Text mUpgradeBtnMaxLvLText;

    public Text mChaiFenBtnText;
    public GameObject mChaiFenBtn;

    public ui_MainEquipItemFenJieFrame mFenJieScript;

    ui_mainEquip mRefRoot;
    CGird mGird;
    bool mIsEquiped;

    int mLvLUpCount = 0;

    public void Init(CGird Gird, bool IsEquiped, ui_mainEquip MainEquip, bool ShowChaiFen = true)
    {
        gameObject.SetActive(true);

        mFenJieScript.gameObject.SetActive(false);
        if (ShowChaiFen)
            mChaiFenBtn.gameObject.SetActive(true);
        else
            mChaiFenBtn.gameObject.SetActive(false);

        mGird = Gird;
        mRefRoot = MainEquip;
        mIsEquiped = IsEquiped;

        mName.text = gDefine.GetStr(mGird.mRefItem.mName);
        gDefine.SetTextBold();
        mPinZhi.text = mGird.mRefItem.GetPinZhiStr();
        gDefine.SetTextBold();
        mPinZhi.color = mGird.mRefItem.GetPinZhiColor();

        string lvlstr = gDefine.GetStr("等级") + ": " + mGird.mLVL.ToString();
        int maxLvL = mGird.mRefItem.GetMaxLVL();
        if (maxLvL > 1)
            lvlstr = lvlstr + "/" + maxLvL.ToString();
        mLvL.text = lvlstr;

        mFenJieBtnText.text = gDefine.GetStr(364);//分 解
        gDefine.SetTextBold();
        mSpecialTipText.text = gDefine.GetStr(365);//----特殊能力---
        gDefine.SetTextBold();
        mGemTipTex.text = gDefine.GetStr(366); //---宝石加成----
        gDefine.SetTextBold();

        mIcon.sprite = mGird.mRefItem.GetIconSprite();


        //属性0
        string value0str = mGird.GetValueStr(0);
        if (mGird.mLVL < mGird.mRefItem.mMaxLvL)
            value0str += "(<color=#00ff00ff>↑+" + mGird.mRefItem.mLvLUpValue.ToString() + "</color>)";

        // if (!IsEquiped)
        // {
        //     CGird gird = gDefine.gPlayerData.mEquipGird[(int)mGird.mRefItem.mEquipPos];
        //     if (gird != null && gird.mRefItem != null)
        //     {
        //         float delt = 0;
        //         if (mGird.mRefItem.mMainType == CItem.eMainType.MainWeapon)
        //             delt = mGird.mDamage - gird.mDamage;
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.GunWeapon)
        //             delt = mGird.mGunDamage - gird.mGunDamage;
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.Clothe)
        //             delt = mGird.mHp - gird.mHp;
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.Cloak)
        //             delt = mGird.mHp - gird.mHp;
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.Ring)
        //             delt = mGird.mCrit - gird.mCrit;

        //         if (delt > 0)
        //         {
        //             if (mGird.mRefItem.mMainType == CItem.eMainType.Ring)
        //                 value0str += "(<color=#00ff00ff>" + "↑+" + delt.ToString("f1") + "%</color>)";
        //             else
        //                 value0str += "(<color=#00ff00ff>" + "↑+" + delt.ToString() + "</color>)";
        //         }
        //         else if (delt < 0)
        //         {
        //             if (mGird.mRefItem.mMainType == CItem.eMainType.Ring)
        //                 value0str += "(<color=#ff0000ff>" + "↓" + delt.ToString("f1") + "%</color>)";
        //             else
        //                 value0str += "(<color=#ff0000ff>" + "↓" + delt.ToString() + "</color>)";
        //         }

        //     }
        //     else if (gird == null || gird.mRefItem == null)
        //     {
        //         if (mGird.mRefItem.mMainType == CItem.eMainType.MainWeapon)
        //             value0str += "(<color=#00ff00ff>" + "↑+" + mGird.mDamage.ToString() + "</color>)";
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.GunWeapon)
        //             value0str += "(<color=#00ff00ff>" + "↑+" + mGird.mGunDamage.ToString() + "</color>)";
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.Clothe)
        //             value0str += "(<color=#00ff00ff>" + "↑+" + mGird.mHp.ToString() + "</color>)";
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.Cloak)
        //             value0str += "(<color=#00ff00ff>" + "↑+" + mGird.mHp.ToString() + "</color>)";
        //         else if (mGird.mRefItem.mMainType == CItem.eMainType.Ring)
        //         {
        //             double v = mGird.mCrit;
        //             value0str += "(<color=#00ff00ff>" + "↑+" + v.ToString() + "%</color>)";
        //         }

        //     }
        // }

        mMainParam[0].text = value0str;
        gDefine.SetTextBold();

        //属性1
        mMainParam[1].text = "";
        if (mGird.mRefItem.mMainType == CItem.eMainType.Ring)
        {
            string value1str = mGird.GetValueStr(1);

            // if (!IsEquiped)
            // {
            //     CGird gird = gDefine.gPlayerData.mEquipGird[(int)mGird.mRefItem.mEquipPos];
            //     if (gird != null && gird.mRefItem != null)
            //     {
            //         float delt = mGird.mDodge - gird.mDodge;

            //         if (delt > 0)
            //         {
            //             value1str += "(<color=#00ff00ff>" + "↑" + delt.ToString("f1") + "%</color>)";
            //         }
            //         else if (delt < 0)
            //         {

            //             value1str += "(<color=#ff0000ff>" + "↓" + delt.ToString("f1") + "%</color>)";

            //         }

            //     }
            //     else if (gird == null || gird.mRefItem == null)
            //     {

            //         value1str += "(<color=#00ff00ff>" + "↑" + mGird.mDodge.ToString("f1") + "%</color>)";
            //     }
            // }
            if (mGird.mLVL < mGird.mRefItem.mMaxLvL)
                value1str += "(<color=#00ff00ff>↑+" + mGird.mRefItem.mLvLUpValue.ToString() + "</color>)";

            mMainParam[1].text = value1str;
        }

        //gem.
        mGem[0].text = mGem[1].text = mGem[2].text = "";
        mGemIcon3[0].sprite = mGemIcon3[1].sprite = mGemIcon3[2].sprite =
           gDefine.gABLoad.GetSprite("icon.bytes", "空宝石槽");
        mGemLvL3[0].text =mGemLvL3[1].text =mGemLvL3[2].text = "";

        CItem gem0 = null;
        if (mGird.mGem[0] > 0)
        {
            gem0 = gDefine.gData.GetItemData(mGird.mGem[0]);
            mGemIcon3[0].sprite = gem0.GetIconSprite();
            mGem[0].text = gem0.GetValueName_Local(0)
            + "+" + gem0.GetValueStr_Local(0);
            mGemLvL3[0].text = gem0.mMaxLvL.ToString();
        }
        CItem gem1 = null;
        if (mGird.mGem[1] > 0)
        {
            gem1 = gDefine.gData.GetItemData(mGird.mGem[1]);
            mGemIcon3[1].sprite = gem1.GetIconSprite();
            mGem[1].text = gem1.GetValueName_Local(0)
            + "+" + gem1.GetValueStr_Local(0);
            mGemLvL3[1].text = gem1.mMaxLvL.ToString();
        }

        CItem gem2 = null;
        if (mGird.mGem[2] > 0)
        {
            gem2 = gDefine.gData.GetItemData(mGird.mGem[2]);
            mGemIcon3[2].sprite = gem2.GetIconSprite();
            mGem[2].text = gem2.GetValueName_Local(0)
            + "+" + gem2.GetValueStr_Local(0);
            mGemLvL3[2].text = gem2.mMaxLvL.ToString();
        }

        //伤害
        // string str = "";
        // if (mGird.mRefItem.mDamage > 0)
        //     str += (mGird.mRefItem.mDamage  + mGird.mRefItem.mLvLUpValue* (mGird.mLVL-1)  ) .ToString();
        // int mDamageAdd = 0;

        // if (gem0?.mDamage > 0)
        //     mDamageAdd += gem0.mDamage;
        // if (gem1?.mDamage > 0)
        //     mDamageAdd += gem1.mDamage;

        // if (mDamageAdd > 0)
        //     str += " +" + mDamageAdd.ToString();

        // if (!string.IsNullOrEmpty(str) && )
        //     mMainParam[0].text = gDefine.GetStr("攻击力") + ":" + str;

        // //枪械
        // str = "";
        // if (mGird.mRefItem.mGunDamage > 0)
        //     str += (mGird.mRefItem.mGunDamage  + mGird.mRefItem.mLvLUpValue* (mGird.mLVL-1)  ) .ToString();
        // int mGunDamageAdd = 0;

        // if (gem0 != null && gem0.mGunDamage > 0)
        //     mGunDamageAdd += gem0.mGunDamage;
        // if (gem1 != null && gem1.mGunDamage > 0)
        //     mGunDamageAdd += gem1.mGunDamage;

        // if (mGunDamageAdd > 0)
        //     str += " +" + mGunDamageAdd.ToString();

        // if (!string.IsNullOrEmpty(str))
        //     mMainParam[0].text = gDefine.GetStr("枪械攻击力")+":" + str;
        // //hp
        // str = "";
        // if (mGird.mRefItem.mHp > 0)
        //     str += (mGird.mRefItem.mHp  + mGird.mRefItem.mLvLUpValue* (mGird.mLVL-1)  ) .ToString();
        // int mHpAdd = 0;

        // if (gem0 != null && gem0.mHp > 0)
        //     mHpAdd += gem0.mHp;
        // if (gem1 != null && gem1.mHp > 0)
        //     mHpAdd += gem1.mHp;

        // if (mHpAdd > 0)
        //     str += " +" + mHpAdd.ToString();

        // if (!string.IsNullOrEmpty(str))
        //     mMainParam[0].text = gDefine.GetStr("生命值")+":" + str;

        // //暴击
        // str = "";
        // if (mGird.mRefItem.mCriticalAtk > 0)
        //     str += (mGird.mRefItem.mCriticalAtk  + mGird.mRefItem.mLvLUpValue* (mGird.mLVL-1)  ) .ToString();

        // float criticalAtkAdd = 0;

        // if (gem0?.mCriticalAtk > 0)
        //     criticalAtkAdd += gem0.mHp;
        // if (gem1?.mCriticalAtk > 0)
        //     criticalAtkAdd += gem1.mHp;

        // if (criticalAtkAdd > 0)
        //     str += " +" + ((int)criticalAtkAdd).ToString() + "%";

        // if (!string.IsNullOrEmpty(str))
        //     mMainParam[0].text = gDefine.GetStr("暴击")+":" + str;

        // //闪避
        // str = "";
        // if (mGird.mRefItem.mDodge > 0)
        //     str += (mGird.mRefItem.mDodge  + mGird.mRefItem.mLvLUpValue* (mGird.mLVL-1)  ) .ToString();
        // float dodgeAdd = 0;

        // if (gem0?.mDodge > 0)
        //     dodgeAdd += gem0.mDodge;
        // if (gem1?.mDodge > 0)
        //     dodgeAdd += gem1.mDodge;

        // if (dodgeAdd > 0)
        //     str += " +" + (dodgeAdd).ToString() + "%";

        // if (!string.IsNullOrEmpty(str))
        //     mMainParam[0].text = gDefine.GetStr("闪避")+":" + str;


        //特殊
        mSpcial[0].text = mSpcial[1].text = mSpcial[2].text = "";
        if (mGird.mRefItem.mEquipPos != gDefine.eEuqipPos.Null)
            mSpcial[0].text = mGird.mRefItem.GetSpeicalSkillStr();


        if (mGird.mRefItem.mPieceItId > 0)
        {
            CGird pieceGird = gDefine.gPlayerData.FindGridByItemId(mGird.mRefItem.mPieceItId);
            CItem pieceIt =
                (pieceGird == null) ?
                    gDefine.gData.GetItemData(mGird.mRefItem.mPieceItId) : pieceGird.mRefItem;
            if (mPieceImage != null)
                mPieceImage.sprite = pieceIt.GetIconSprite();
            mPiece.text = //gDefine.GetStr("碎片") + ": " + 
            (pieceGird != null ? pieceGird.mNum.ToString() : "0")
             + "/" + mGird.mRefItem.LvLUpNeedPieceNum(mGird.mLVL).ToString();
            //mPieceObj.gameObject.SetActive(true);
        }
        else
        {
            mPiece.text = "";
            //mPieceObj.gameObject.SetActive(false);
        }

        if (IsEquiped)
        {
            mEquipBtnText.text = gDefine.GetStr(363);//卸下
        }

        else if (mGird.mRefItem.mEquipPos != gDefine.eEuqipPos.Null)
        {
            if (gDefine.gPlayerData.mEquipGird[(int)mGird.mRefItem.mEquipPos].mRefItem != null)
                mEquipBtnText.text = gDefine.GetStr("替    换");
            else
                mEquipBtnText.text = gDefine.GetStr("装    备");
        }


        if (mGird.mLVL < mGird.mRefItem.mMaxLvL)
        {
            mUpgradeBtnTMoneyText.text = mGird.mRefItem.LvLUpNeedMoney(mGird.mLVL).ToString();
            mUpgradeBtnText.text = gDefine.GetStr("升    级");
            mUpgradeBtnCrystal.SetActive(true);

            mUpgradeBtnMaxLvLText.text = "";

        }
        // mLVLupBtnText.text =
        // "升级_n_" + mGird.mRefItem.LvLUpNeedMoney(mGird.mLVL).ToString();
        else
        {
            mUpgradeBtnTMoneyText.text = "";
            mUpgradeBtnText.text = "";
            mUpgradeBtnCrystal.SetActive(false);

            //mUpgradeBtnMaxLvLText.gameObject.SetActive(true);
            mUpgradeBtnMaxLvLText.text = gDefine.GetStr(241); //满级
        }
    }

    public void Btn_ChaiFen()
    {
        mChaiFenBtn.gameObject.SetActive(false);
        mFenJieScript.Init(mGird, this);
    }

    public void ChaiFen()
    {
        if (mGird.mGem[0] > 0)
            gDefine.gPlayerData.AddItemToBag(mGird.mGem[0], 1);

        if (mGird.mGem[1] > 0)
            gDefine.gPlayerData.AddItemToBag(mGird.mGem[1], 1);

        if (mGird.mGem[2] > 0)
            gDefine.gPlayerData.AddItemToBag(mGird.mGem[2], 1);

        int money = mGird.CalcChaiFenMoney();
        int pieceNum = mGird.CalcChaiPieceNum();
        int pieceId = mGird.mRefItem.mPieceItId;
        mGird.mRefItem = null;
        gDefine.gPlayerData.Coin += money;
        gDefine.gPlayerData.AddItemToBag(pieceId, pieceNum);

        CItem it = gDefine.gData.GetItemData(pieceId);

        //string str = "获得:" + " 金币" + money.ToString() + " " + it.mName + " " + pieceNum.ToString();
        //mRefRoot.Tip(str);

        gameObject.SetActive(false);

        gDefine.gPlayerData.SaveGird();
        PlayerPrefs.Save();

        mRefRoot.Refresh();

        gDefine.PlaySound(78);


    }

    public void ADCallBack()
    {
        Init(mGird, mIsEquiped, mRefRoot);
    }

    public void Btn_LvLUp()
    {
        if (mGird.mLVL < mGird.mRefItem.GetMaxLVL())
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("名称", mGird.mRefItem.mName);
            TalkingDataGA.OnEvent("装备升级点击", dic);

            int money = mGird.mRefItem.LvLUpNeedMoney(mGird.mLVL);
            int pieceNum = mGird.mRefItem.LvLUpNeedPieceNum(mGird.mLVL);
            if (gDefine.gPlayerData.Coin < money)
            {
                gDefine.ShowLackItTip(201, 200, ADCallBack);
                // gDefine.ShowTip(gDefine.GetStr(160));
                gDefine.PlaySound(71);
                dic.Clear();
                dic.Add("名称", mGird.mRefItem.mName);
                dic.Add("当前等级", mGird.mLVL.ToString());
                dic.Add("原因", "缺钱");
                TalkingDataGA.OnEvent("装备升级失败", dic);
                return;
            }
            CGird pieceGrid = gDefine.gPlayerData.FindGridByItemId(mGird.mRefItem.mPieceItId);
            if (pieceGrid == null || pieceGrid.mNum < pieceNum)
            {
                //gDefine.ShowTip( gDefine.GetStr(275));
                gDefine.ShowLackItTip(mGird.mRefItem.mPieceItId, 1, ADCallBack);
                gDefine.PlaySound(71);
                dic.Clear();
                dic.Add("名称", mGird.mRefItem.mName);
                dic.Add("当前等级", mGird.mLVL.ToString());
                dic.Add("原因", "缺碎片");
                TalkingDataGA.OnEvent("装备升级失败", dic);
                return;
            }

            gDefine.gPlayerData.Coin -= money;
            pieceGrid.mNum -= pieceNum;
            mGird.mLVL++;
            if (pieceGrid.mNum <= 0)
                pieceGrid.mRefItem = null;

            mGird.ReCalcValue();
            gDefine.gPlayerData.SaveGird();
            PlayerPrefs.Save();
            Init(mGird, mIsEquiped, mRefRoot);

            mRefRoot.Refresh();
            gDefine.PlaySound(78);

            dic.Clear();
            dic.Add("名称", mGird.mRefItem.mName);
            dic.Add("当前等级", mGird.mLVL.ToString());
            TalkingDataGA.OnEvent("装备升级成功", dic);

            mLvLUpCount++;

            if( mLvLUpCount>=2)
            {
                mLvLUpCount = 0;
                    
                gDefine.gAd.PlayInterAD(ADCallBackFunc);

                //Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Clear();
                dic.Add("来源", "装备升级双次"); 
                TalkingDataGA.OnEvent("插屏广告", dic);
            }

         
        }
        else
        {
            gDefine.PlaySound(71);
        }

    }

    public void ADCallBackFunc(bool Finished)
    {}

    public void CloseFenJieFrame()
    {
        mFenJieScript.gameObject.SetActive(false);
        mChaiFenBtn.gameObject.SetActive(true);
    }

    public void Btn_Equip()
    {
        if (mIsEquiped)
        {
            //gameObject.SetActive(false);
            gDefine.gPlayerData.ChangeEquipGird(mGird);
        }
        else
            gDefine.gPlayerData.ChangeBagGird(mGird);

        gDefine.gPlayerData.SaveGird();
        PlayerPrefs.Save();

        gameObject.SetActive(false);

        mRefRoot.mRefParamScript.Refresh(mRefRoot);

        gDefine.PlaySound(58);
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false);
        gDefine.PlaySound(70);
    }
}
