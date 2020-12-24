using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class CItem
{
    public enum eMainType
    {
        Null,
        MainWeapon,
        GunWeapon,
        Clothe,
        Cloak,
        Ring,
        Scroll,
        Gem,
        Box,
        Piece,
        ComPiece,
    }

    public enum eSubType
    {
        Null,
        DSword,
        Shield,
        ShortGun,
        LongGun,
        RedGem,
        GreenGem,
        BlueGem,
        PurpleGem,
        YellowGem,
        OrangeGem,
    }

    public enum eQuality
    {
        Null,
        Common,
        Excellent,
        Rare,
        Epic,
        Legend,
        Cound,
    }

    public enum eValueType
    {
        Null,
        Damage,
        GunDamage,
        Hp,
        CriticalAtk,
        Dodge,
        PetDamage,
    }


    public eMainType mMainType; //主类型
    public eSubType mSubType; //子类型
    public eQuality mQuality;//品质
    public eValueType mValueType; //属性类型
    public float mValue;//属性值
    public eValueType mValueType1; //属性类型1
    public float mValue1;//属性值1
    public float mLvLUpValue; //升级属性改变值
    public int mMaxLvL; //最高等级
    public int mSpecialIndex; //特殊索引
    public int Id;
    public int mPieceItId; //升级材料
    public int mComPieceId; //合成材料
    public string mName;
    public string mIconName;
    public string mDes;
    public Sprite mIcon;
    public int mPrice; //金币

    //----载入后计算-----
    public gDefine.eEuqipPos mEquipPos;
    public gDefine.eWeaponClass mWeaponClass;
    public int mDamage;
    public int mHp;
    public float mGunDamage;
    public int mPetDamage;
    public float mDodge;
    public float mCriticalAtk;

    public int GetMaxLVL()
    {
        switch (mMainType)
        {
            case eMainType.MainWeapon:
            case eMainType.GunWeapon:
            case eMainType.Clothe:
            case eMainType.Cloak:
            case eMainType.Ring:
                return (int)mQuality * 10;
        }
        return 1;
    }

    public string GetNameLocal()
    {
        if (mMainType == eMainType.Scroll)
            return gDefine.GetStr(200);
        else
            return gDefine.GetStr(mName);
    }

    public string GetDesLocal()
    {
        if (mMainType == eMainType.Scroll)
            return gDefine.GetStr(201);
        else
            return gDefine.GetStr(mDes);


    }

    public Sprite GetIconSprite()
    {
        if (mIcon == null)
            mIcon = gDefine.gABLoad.GetSprite("icon.bytes", mIconName);
        return mIcon;
    }
    /// <summary>
    /// 通用的升级碎片数量
    /// </summary>
    /// <param name="LvL"></param>
    /// <returns></returns>
    public int LvLUpNeedPieceNum(int LvL)
    {
        return LvL / 10 + 1;
    }

    public int LvLUpNeedMoney(int LvL)
    {
        return (LvL - 1) * 10 + 100;
    }

    public Color GetPinZhiColor()
    {
        switch (mQuality)
        {
            case eQuality.Common:
                return Color.white;
            case eQuality.Excellent:
                return Color.green;
            case eQuality.Rare:
                return new Color(0.45f, 0.98f, 0.99f);
            case eQuality.Epic:
                return new Color(1, 0.5f, 0);
            case eQuality.Legend:
                return new Color(0.827f, 0.0561f, 0.9150f);
        }
        return Color.white;
    }

    /// <summary>
    ///  获得本地化的宝石名称
    /// </summary>
    /// <returns></returns>
    public string GetGemNameLocal()
    {
        string str = gDefine.GetStr(mName);
        return str + "Lv." + mMaxLvL.ToString();
    }

    public string GetValueStr(int index)
    {
        eValueType vt = (index == 0) ? mValueType : mValueType1;
        switch (vt)
        {
            case eValueType.Damage:
                {
                    string str = gDefine.GetStr("攻击力");
                    return str + ": +" + mDamage.ToString();
                }

            case eValueType.GunDamage:
                {
                    string str = gDefine.GetStr("火力");
                    return str + ": +" + mGunDamage.ToString("f1");
                }

            case eValueType.Hp:
                {
                    string str = gDefine.GetStr("生命值");
                    return str + ": +" + mHp.ToString();
                }
            case eValueType.CriticalAtk:
                {
                    string str = gDefine.GetStr("暴击");
                    return str + ": +" + mCriticalAtk.ToString() + "%";
                }

            case eValueType.Dodge:
                {
                    string str = gDefine.GetStr("闪避");
                    return str + ": +" + mDodge.ToString() + "%";
                }

            case eValueType.PetDamage:
                {
                    string str = gDefine.GetStr("宠物");
                    return str + ": +" + mPetDamage.ToString();
                }
        }
        return null;
    }

    public string GetValueStrInShop(int index)
    {
        eValueType vt = (index == 0) ? mValueType : mValueType1;
        switch (vt)
        {
            case eValueType.Damage:
                {
                    string str = gDefine.GetStr("攻击力");
                    return str + ": <color=#00eaff>+" + mDamage.ToString() + "</color>";
                }

            case eValueType.GunDamage:
                {
                    string str = gDefine.GetStr("火力");
                    return str + ": <color=#00eaff>+" + mGunDamage.ToString("f1") + "</color>";
                }

            case eValueType.Hp:
                {
                    string str = gDefine.GetStr("生命值");
                    return str + ": <color=#00eaff>+" + mHp.ToString() + "</color>";
                }
            case eValueType.CriticalAtk:
                {
                    string str = gDefine.GetStr("暴击");
                    return str + ": <color=#00eaff>+" + mCriticalAtk.ToString() + "%</color>";
                }

            case eValueType.Dodge:
                {
                    string str = gDefine.GetStr("闪避");
                    return str + ": <color=#00eaff>+" + mDodge.ToString() + "%</color>";
                }

            case eValueType.PetDamage:
                {
                    string str = gDefine.GetStr("宠物");
                    return str + ": <color=#00eaff>+" + mPetDamage.ToString() + "</color>";
                }
        }
        return null;
    }


    public string GetValueName_Local(int index)
    {
        eValueType vt = (index == 0) ? mValueType : mValueType1;
        switch (vt)
        {
            case eValueType.Damage:
                {
                    return gDefine.GetStr("攻击力");

                }

            case eValueType.GunDamage:
                {
                    return gDefine.GetStr("火力");

                }

            case eValueType.Hp:
                {
                    return gDefine.GetStr("生命值");

                }
            case eValueType.CriticalAtk:
                {
                    return gDefine.GetStr("暴击");

                }

            case eValueType.Dodge:
                {
                    return gDefine.GetStr("闪避");

                }

            case eValueType.PetDamage:
                {
                    return gDefine.GetStr("宠物");

                }
        }
        return "";
    }

    public string GetValueStr_Local(int index)
    {
        eValueType vt = (index == 0) ? mValueType : mValueType1;
        switch (vt)
        {
            case eValueType.Damage:
                {

                    return mDamage.ToString();
                }

            case eValueType.GunDamage:
                {

                    return mGunDamage.ToString("f1");
                }

            case eValueType.Hp:
                {

                    return mHp.ToString();
                }
            case eValueType.CriticalAtk:
                {

                    return mCriticalAtk.ToString() + "%";
                }

            case eValueType.Dodge:
                {

                    return mDodge.ToString() + "%";
                }

            case eValueType.PetDamage:
                {

                    return mPetDamage.ToString();
                }
        }
        return null;
    }



    public void ReadData(string[] Str)
    {
        Id = int.Parse(Str[0]);
        mName = Str[1];
        string[] sepStr = new string[] { "." };
        string[] valueArr = Str[2].Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        mIconName = valueArr[0];
        mDes = Str[3];
        mMainType = ReadMainType(Str[4]);
        mSubType = ReadSubType(Str[5]);
        mQuality = ReadQuality(Str[6]);
        mMaxLvL = int.Parse(Str[7]);
        mValueType = ReadValueType(Str[8]);
        mValue = float.Parse(Str[9]);
        mValueType1 = ReadValueType(Str[10]);
        mValue1 = float.Parse(Str[11]);
        mLvLUpValue = float.Parse(Str[12]);
        mSpecialIndex = int.Parse(Str[13]);
        mPieceItId = int.Parse(Str[14]);
        mComPieceId = int.Parse(Str[15]);
        mPrice = int.Parse(Str[16]);

        Recalc();
    }
    void Recalc()
    {
        mDamage = mHp = mPetDamage = 0;
        mGunDamage = 0;
        mDodge = mCriticalAtk = 0;
        switch (mValueType)
        {
            case eValueType.Damage:
                mDamage += (int)mValue;
                break;
            case eValueType.GunDamage:
                mGunDamage += mValue;
                break;
            case eValueType.Hp:
                mHp += (int)mValue;
                break;
            case eValueType.CriticalAtk:
                mCriticalAtk += mValue;
                break;
            case eValueType.Dodge:
                mDodge += mValue;
                break;
            case eValueType.PetDamage:
                mPetDamage += (int)mValue;
                break;
        }

        switch (mValueType1)
        {
            case eValueType.Damage:
                mDamage += (int)mValue1;
                break;
            case eValueType.GunDamage:
                mGunDamage += mValue1;
                break;
            case eValueType.Hp:
                mHp += (int)mValue1;
                break;
            case eValueType.CriticalAtk:
                mCriticalAtk += mValue1;
                break;
            case eValueType.Dodge:
                mDodge += mValue1;
                break;
            case eValueType.PetDamage:
                mPetDamage += (int)mValue1;
                break;
        }

        switch (mMainType)
        {
            case eMainType.MainWeapon:
                mEquipPos = gDefine.eEuqipPos.MainWeapon;
                break;
            case eMainType.GunWeapon:
                mEquipPos = gDefine.eEuqipPos.GunWeapon;
                break;
            case eMainType.Clothe:
                mEquipPos = gDefine.eEuqipPos.Clothe;
                break;
            case eMainType.Cloak:
                mEquipPos = gDefine.eEuqipPos.Cloak;
                break;
            case eMainType.Ring:
                mEquipPos = gDefine.eEuqipPos.Ring;
                break;
            default:
                mEquipPos = gDefine.eEuqipPos.Null;
                break;
        }

        switch (mSubType)
        {
            case eSubType.DSword:
                mWeaponClass = gDefine.eWeaponClass.DSword;
                break;
            case eSubType.Shield:
                mWeaponClass = gDefine.eWeaponClass.Lun;
                break;
            case eSubType.ShortGun:
                mWeaponClass = gDefine.eWeaponClass.ShortGun;
                break;
            case eSubType.LongGun:
                mWeaponClass = gDefine.eWeaponClass.longGun;
                break;
            default:
                mWeaponClass = gDefine.eWeaponClass.Null;
                break;
        }
    }

    eMainType ReadMainType(string Str)
    {
        if (Str == "主武器")
            return eMainType.MainWeapon;
        else if (Str == "衣服")
            return eMainType.Clothe;
        else if (Str == "枪")
            return eMainType.GunWeapon;
        else if (Str == "技能卷轴")
            return eMainType.Scroll;
        else if (Str == "宝石")
            return eMainType.Gem;
        else if (Str == "披风")
            return eMainType.Cloak;
        else if (Str == "饰品")
            return eMainType.Ring;
        else if (Str == "碎片")
            return eMainType.Piece;
        else if (Str == "宝箱")
            return eMainType.Box;
        else if (Str == "合成碎片")
            return eMainType.ComPiece;
        else
            return eMainType.Null;
    }

    public string GetSpeicalSkillStr()
    {
        string str = "";
        switch (mMainType)
        {
            case eMainType.MainWeapon:
                if (mSpecialIndex == 1)
                    str = "一定概率对敌人发动即死攻击";
                else if (mSpecialIndex == 2)
                    str = "每消灭5个敌人，下次攻击必暴击";
                else if (mSpecialIndex == 3)
                    str = "一定概率吸血";

                break;
            case eMainType.GunWeapon:
                if (mSubType == eSubType.ShortGun)
                {
                    if (mSpecialIndex == 1)
                        str = "一定概率麻痹目标";
                    else if (mSpecialIndex == 2)
                        str = "一定概率吸血";
                }
                else
                {
                    if (mSpecialIndex == 1)
                        return gDefine.GetStr(75);//破盾
                    else if (mSpecialIndex == 2)
                        return gDefine.GetStr(77);//击杀掉落金币
                }

                break;
            case eMainType.Clothe:
                if (mSpecialIndex == 1)
                    str = "5%伤害减免";
                else if (mSpecialIndex == 2)
                    str = "击杀一个怪增加1%血量上限，上限14%";
                else if (mSpecialIndex == 3)
                    str = "一定概率麻痹目标";
                else if (mSpecialIndex == 4)
                    str = "每次暴击增加1%伤害上限，上限5%";
                break;
            case eMainType.Cloak:
                if (mSpecialIndex == 1)
                    str = "5%伤害减免";
                else if (mSpecialIndex == 2)
                    str = "血量低于30%额外获得闪避率";
                else if (mSpecialIndex == 3)
                    str = "增加无敌类技能的时间1秒";
                else if (mSpecialIndex == 4)
                    str = "每隔30秒获得一个自身的3秒持枪幻象攻击敌人";
                break;
            case eMainType.Ring:
                if (mSpecialIndex == 1)
                    str = "免疫中毒效果";
                else if (mSpecialIndex == 2)
                    str = "免疫麻痹效果";
                else if (mSpecialIndex == 3)
                    str = "免疫诅咒效果";
                break;
        }

        return gDefine.GetStr(str);
    }



    eSubType ReadSubType(string Str)
    {
        if (Str == "双刀")
            return eSubType.DSword;
        else if (Str == "盾刃")
            return eSubType.Shield;
        else if (Str == "双枪")
            return eSubType.ShortGun;
        else if (Str == "长枪")
            return eSubType.LongGun;
        else if (Str == "红宝石")
            return eSubType.RedGem;
        else if (Str == "绿宝石")
            return eSubType.GreenGem;
        else if (Str == "蓝宝石")
            return eSubType.BlueGem;
        else if (Str == "紫宝石")
            return eSubType.PurpleGem;
        else if (Str == "黄宝石")
            return eSubType.YellowGem;
        else if (Str == "橙宝石")
            return eSubType.OrangeGem;

        else
            return eSubType.Null;
    }

    eValueType ReadValueType(string Str)
    {
        if (Str == "伤害")
            return eValueType.Damage;
        else if (Str == "血量")
            return eValueType.Hp;
        else if (Str == "火力")
            return eValueType.GunDamage;
        else if (Str == "暴击")
            return eValueType.CriticalAtk;
        else if (Str == "闪避")
            return eValueType.Dodge;
        else if (Str == "宠物伤害")
            return eValueType.PetDamage;
        else
            return eValueType.Null;
    }

    eQuality ReadQuality(string Str)
    {
        if (Str == "普通")
            return eQuality.Common;
        else if (Str == "精良")
            return eQuality.Excellent;
        else if (Str == "稀有")
            return eQuality.Rare;
        else if (Str == "史诗")
            return eQuality.Epic;
        else if (Str == "传说")
            return eQuality.Legend;
        else
            return eQuality.Null;
    }


    public string GetPinZhiStr()
    {
        string str = "";
        switch (mQuality)
        {
            case eQuality.Common:
                str = "普通";
                break;
            case eQuality.Excellent:
                str = "精良";
                break;
            case eQuality.Rare:
                str = "稀有";
                break;
            case eQuality.Epic:
                str = "史诗";
                break;
            case eQuality.Legend:
                str = "传说";
                break;
        }
        return gDefine.GetStr(str);
    }
}

public class CGird
{
    public CItem mRefItem;
    public int mNum = 0;
    public int mLVL = 0;
    public int[] mGem = new int[3] { 0, 0, 0 };
    public long mUId; // 唯一id

    //--以下为后期计算属性
    public int mDamage;
    public float mGunDamage;
    public int mHp;
    public float mCrit;
    public float mDodge;
    public int mPet;

    public bool EquipCanLvLUp()
    {
        if (mRefItem != null && mRefItem.mMainType <= CItem.eMainType.Ring)
        {
            if (mLVL < mRefItem.GetMaxLVL())
            {
                int money = mRefItem.LvLUpNeedMoney(mLVL);
                if (gDefine.gPlayerData.Coin < money)
                    return false;
                int pieceNum = mRefItem.LvLUpNeedPieceNum(mLVL);

                CGird pieceGrid = gDefine.gPlayerData.FindGridByItemId(mRefItem.mPieceItId);
                if (pieceGrid != null && pieceGrid.mNum >= pieceNum)
                    return true;

            }
        }


        return false;

    }


    public int CalcChaiFenMoney()
    {
        if (mLVL == 1)
            return mRefItem.mPrice / 2;
        else
        {
            int money = 0;

            for (int i = 1; i < mLVL; i++)
            {
                money += mRefItem.LvLUpNeedMoney(i);

            }
            return money / 2;

        }
    }

    public int GetCombinPieceNum()
    {
        switch (mRefItem.mQuality)
        {
            case CItem.eQuality.Common:
                return 10;
            case CItem.eQuality.Excellent:
                return 12;
            case CItem.eQuality.Epic:
                return 15;
            case CItem.eQuality.Rare:
                return 20;
            case CItem.eQuality.Legend:
                return 30;

        }
        return 9999;
    }

    public bool CanCombin()
    {
        if (mRefItem.mMainType == CItem.eMainType.ComPiece)
        {
            if (mNum >= GetCombinPieceNum())
                return true;
        }
        return false;
    }

    public int CalcChaiPieceNum()
    {
        if (mLVL == 1)
            return (int)mRefItem.mQuality;
        else
        {
            int pieceNum = 0;
            for (int i = 1; i < mLVL; i++)
                pieceNum += mRefItem.LvLUpNeedPieceNum(i);
            return pieceNum;
        }
    }

    public int CalcDamage()
    {
        return mRefItem.mDamage > 0 ? (int)(mRefItem.mDamage + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
    }

    public float CalcGunDamage()
    {
        return mRefItem.mGunDamage > 0 ? (int)(mRefItem.mGunDamage + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
    }

    public float CalcHp()
    {
        return mRefItem.mHp > 0 ? (int)(mRefItem.mHp + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
    }

    public float CalcCrit()
    {
        return mRefItem.mCriticalAtk > 0 ? (mRefItem.mCriticalAtk + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
    }

    public float CalcDodge()
    {
        return mRefItem.mDodge > 0 ? (mRefItem.mDodge + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
    }

    public void ReCalcValue()
    {
        if (mRefItem != null)
        {
            mDamage = mRefItem.mDamage > 0 ? (int)(mRefItem.mDamage + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
            mGunDamage = mRefItem.mGunDamage > 0 ? (int)(mRefItem.mGunDamage + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
            mHp = mRefItem.mHp > 0 ? (int)(mRefItem.mHp + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
            mPet = mRefItem.mPetDamage > 0 ? (int)(mRefItem.mPetDamage + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
            mCrit = mRefItem.mCriticalAtk > 0 ? (mRefItem.mCriticalAtk + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;
            mDodge = mRefItem.mDodge > 0 ? (mRefItem.mDodge + mRefItem.mLvLUpValue * (mLVL - 1)) : 0;

            if (mGem[0] > 0)
            {
                CItem gem = gDefine.gData.GetItemData(mGem[0]);
                mDamage += gem.mDamage;
                mGunDamage += gem.mGunDamage;
                mHp += gem.mHp;
                mPet += gem.mPetDamage;
                mCrit += gem.mCriticalAtk;
                mDodge += gem.mDodge;
            }

            if (mGem[1] > 0)
            {
                CItem gem = gDefine.gData.GetItemData(mGem[1]);
                mDamage += gem.mDamage;
                mGunDamage += gem.mGunDamage;
                mHp += gem.mHp;
                mPet += gem.mPetDamage;
                mCrit += gem.mCriticalAtk;
                mDodge += gem.mDodge;
            }

            if (mGem[2] > 0)
            {
                CItem gem = gDefine.gData.GetItemData(mGem[2]);
                mDamage += gem.mDamage;
                mGunDamage += gem.mGunDamage;
                mHp += gem.mHp;
                mPet += gem.mPetDamage;
                mCrit += gem.mCriticalAtk;
                mDodge += gem.mDodge;
            }

        }
    }

    public string GetValueStr(int ParamIndex)
    {
        CItem.eValueType vt = ParamIndex == 0 ? mRefItem.mValueType : mRefItem.mValueType1;
        switch (vt)
        {
            case CItem.eValueType.Damage:
                {
                    string str = gDefine.GetStr("攻击力");
                    return str + ": +" + mDamage.ToString();
                }

            case CItem.eValueType.GunDamage:
                {
                    string str = gDefine.GetStr("火力");
                    return str + ": +" + mGunDamage.ToString();
                }

            case CItem.eValueType.Hp:
                {
                    string str = gDefine.GetStr("生命值");
                    return str + ": +" + mHp.ToString();
                }
            case CItem.eValueType.CriticalAtk:
                {
                    string str = gDefine.GetStr("暴击");
                    return str + ": +" + mCrit.ToString() + "%";
                }

            case CItem.eValueType.Dodge:
                {
                    string str = gDefine.GetStr("闪避");
                    return str + ": +" + mDodge.ToString() + "%";
                }

            case CItem.eValueType.PetDamage:
                {
                    string str = gDefine.GetStr("宠物");
                    return str + ": +" + mPet.ToString();
                }
        }
        return "";
    }
}

public class PlayerData
{
    int mCoin = 200;
    public int Coin
    {
        get { return mCoin; }
        set { mCoin = value; if (mLoadFinish) SaveCoin(); gDefine.gMainUI?.RefreshPCParam(); }
    }
    int mCrystal = 0;
    public int Crystal
    {
        get { return mCrystal; }
        set { mCrystal = value; if (mLoadFinish) SaveCrystal(); gDefine.gMainUI?.RefreshPCParam(); }
    }

    //--战斗外----
    int mLVL = 1;
    public int LVL
    {
        get { return mLVL; }
        set { mLVL = value; if (mLoadFinish) SaveLvL(); gDefine.gMainUI?.RefreshPCParam(); }
    }
    int mEXP;
    public int EXP
    {
        get { return mEXP; }
        set { mEXP = value; if (mLoadFinish) SaveExp(); gDefine.gMainUI?.RefreshPCParam(); }
    }

    int mTiLi = 20;
    public int TiLI
    {
        get { return mTiLi; }
        set { mTiLi = value; if (mLoadFinish) SaveTiLi(); gDefine.gMainUI?.RefreshPCParam(); }
    }

    public long mTiLiT = 0;

    //int mMaxChapterId;  //从0计数，显示时请+1
    // public int MaxChapterId
    // {
    //     get { return mMaxChapterId; }
    //     set { mMaxChapterId = value; if (mLoadFinish) Save(); }
    // }

    //int mMaxChapterLVL; //从0计数，显示时请+1
    // public int MaxChapterLVL
    // {
    //     get { return mMaxChapterLVL; }
    //     set { mMaxChapterLVL = value; if (mLoadFinish) Save(); }
    // }

    public CMyStr.eType mLanguageType = CMyStr.eType.Simple; //当前语言类型


    public bool mSoundIsOpen = true;
    public bool mMusicIsOpen = true;


    //--战斗中的属性--
    public int mHp = 285;
    public int mHpMax = 285;
    public float mDamage = 41; //
    public float mDamageReduce = 0;//伤害减免
    public float mGunDamage = 0;
    public float mDoubleDamagePerc = 0; //暴击百分比
    public float mDodgePerc = 0; //闪避百分比
    public float mGunCoolDown = 0;

    public float mHeadShootPerc = 10; //爆头率，基础10%，枪械专家 +20%， 技能每个等级+5%
    //--buff In fight--
    List<CSkill> mLearnSkillInFight = new List<CSkill>();
    int mLearnSkillNum = 0;
    public int mCoinGain; //游戏中获得的金币数量
    List<CSkill> mSkillAutoArr = new List<CSkill>(); //自动释放技能
    //int mBuffDamageInFight = 0;
    //int mBuffHpInFight = 0;
    //bool mBloodSuck = false;
    float mBloodSuckNumPerc = 0; //吸血量
    float mBloodSuckPerc = 0; //吸血概率
    public float mChangeToGunPerc = 15; //换枪概率
    float mBulletBackPerc = 0; //子弹反弹概率 
    bool mMoreMoney = false;
    public float mIgonrDamageT;
    public CGird[] mEquipGird = new CGird[(int)gDefine.eEuqipPos.Count];
    public CGird[] mBagGird = new CGird[100];
    public long mUID = 1000;
    public CPlayerTalent mTalent = new CPlayerTalent();
    public bool mLoadFinish = false;
    public bool mNeedToSaveBag = false;
    public CSkillAdd mSkillAdd = new CSkillAdd();
    public CMagicData mCurMagicData = new CMagicData();
    //public int[] mLvLFinish = new int[11];
    public int[] mLvLDifficult = new int[11]; // 章节难度
    public CChapterSystem mChapterEx = new CChapterSystem(); //章节信息都应该整合到这里， 目前暂时只有关卡宝箱
    public float mEndKillCoolDownT = 0; //终结技cd
    public int mAdvCount = 0;

    public int mClotheMaxHpCount = 0;

    public int mHeavyAtkUpCount = 0;

    public float mDarkShadowT = 0;

    public GameObject mTmpHelthSE = null;

    public int GetChapterDifficult(int ChapterId)
    {
        return mLvLDifficult[ChapterId];
    }
    /// <summary>
    /// 根据当前关卡难度，提升调整关卡可挑战难度
    /// </summary>
    public void ChapterDifficultFinished()
    {
        mLvLDifficult[gDefine.gChapterId] = gDefine.gChapterDifficult + 1;
    }

    public void AddCoin(int Coin)
    {
        mCoin += Coin;
        PlayerPrefs.SetInt("coin", mCoin);
        PlayerPrefs.Save();
    }

    public void SaveCoin()
    {
        PlayerPrefs.SetInt("coin", mCoin);
        PlayerPrefs.Save();
    }

    public void AddCrystal(int Crystal)
    {
        mCrystal += Crystal;
        PlayerPrefs.SetInt("crystal", mCrystal);
    }

    public void SaveCrystal()
    {
        PlayerPrefs.SetInt("crystal", mCrystal);
        PlayerPrefs.Save();
    }

    public void SaveLvL()
    {
        PlayerPrefs.SetInt("lvl", mLVL);
        PlayerPrefs.Save();
    }

    public void SaveExp()
    {
        PlayerPrefs.SetInt("exp", mEXP);
        PlayerPrefs.Save();
    }

    public void SaveTiLi()
    {
        PlayerPrefs.SetInt("tili", mTiLi);
        PlayerPrefs.Save();
    }






    public float GetGunCoolDown()
    {
        return mGunCoolDown;
    }

    public bool IsDarkShdowTReady()
    {
        if (Time.time > mDarkShadowT)
            return true;
        else
        {
            return false;
        }
    }
    public void ResetDarkShadowT()
    {
        mDarkShadowT = Time.time + 30;
    }
    public void AddLvLFinish(int Chapter, int Difficult, int MaxLvL)
    {

    }

    public void ClearLearnSkill()
    {
        mLearnSkillInFight.Clear();
        mSkillAutoArr.Clear();
        mChangeToGunPerc = 15;
        mBloodSuckNumPerc = 0; //吸血量
        mBloodSuckPerc = 0; //吸血概率
        mDoubleDamagePerc = 0; //暴击百分比
        mBulletBackPerc = 0;
        mMoreMoney = false;
    }

    public void ClearAllData()
    {
        LVL = 1;
        EXP = 0;
        Coin = 0;
        Crystal = 0;
        mTalent.Init();
        mSkillAdd.Clear();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Application.Quit();
    }

    public long GenUID()
    {
        return mUID++;
    }



    public void Save()
    {
        PlayerPrefs.SetInt("exist", 1);
        PlayerPrefs.SetString("UID", mUID.ToString());

        PlayerPrefs.SetInt("lvl", LVL);
        PlayerPrefs.SetInt("exp", EXP);
        PlayerPrefs.SetInt("coin", Coin);
        PlayerPrefs.SetInt("crystal", Crystal);
        PlayerPrefs.SetInt("tili", TiLI);
        PlayerPrefs.SetString("tiliT", mTiLiT.ToString());

        //PlayerPrefs.SetInt("MaxChapterId", MaxChapterId);
        //PlayerPrefs.SetInt("MaxChapterLVL", MaxChapterLVL);


        PlayerPrefs.SetInt("LanguageType", (int)mLanguageType);
        PlayerPrefs.SetInt("MusicIsOpen", mMusicIsOpen ? 1 : 0);
        PlayerPrefs.SetInt("SoundIsOpen", mSoundIsOpen ? 1 : 0);

        for (int i = 1; i <= 10; i++)
        {
            PlayerPrefs.SetInt("LvLDifficult_" + i.ToString(), mLvLDifficult[i]);
        }


        SaveGird();

        mTalent.Save();

        mSkillAdd.Save();

        gDefine.gShop.Save();

        gDefine.gBoxData.Save();

        PlayerPrefs.Save();

        mChapterEx.Save();
    }

    public void Load()
    {
        mUID = long.Parse(PlayerPrefs.GetString("UID", "100"));
        mTalent.Init();

        for (int i = 0; i < mEquipGird.Length; i++)
            mEquipGird[i] = new CGird();

        for (int i = 0; i < mBagGird.Length; i++)
            mBagGird[i] = new CGird();

        int exist = PlayerPrefs.GetInt("exist", 0);

        if (exist == 1)
        {
            LVL = PlayerPrefs.GetInt("lvl", 1);
            EXP = PlayerPrefs.GetInt("exp", 0);
            Coin = PlayerPrefs.GetInt("coin", 0);
            Crystal = PlayerPrefs.GetInt("crystal", 0);
            TiLI = PlayerPrefs.GetInt("tili", 0);
            string strTiliT = PlayerPrefs.GetString("tiliT", "");
            if (string.IsNullOrEmpty(strTiliT))
                mTiLiT = System.DateTime.Now.Ticks;
            else
                mTiLiT = long.Parse(strTiliT);

            // MaxChapterId = PlayerPrefs.GetInt("MaxChapterId", 1);
            // if (mMaxChapterId == 0)
            //     mMaxChapterId = 1;

            // MaxChapterLVL = PlayerPrefs.GetInt("MaxChapterLVL", 0);

            mLanguageType = (CMyStr.eType)PlayerPrefs.GetInt("LanguageType", 0);
            mMusicIsOpen = (PlayerPrefs.GetInt("MusicIsOpen", 1) == 1) ? true : false;
            gDefine.gSound.EnableMusic(mMusicIsOpen);
            mSoundIsOpen = (PlayerPrefs.GetInt("SoundIsOpen", 1) == 1) ? true : false;
            gDefine.gSound.EnableSound(mSoundIsOpen);

            for (int i = 1; i <= 10; i++)
            {
                mLvLDifficult[i] = PlayerPrefs.GetInt("LvLDifficult_" + i.ToString(), 0);
            }

            LoadGird();

            mChapterEx.Init();
        }
        else
        {
            mEquipGird[0].mRefItem = gDefine.gData.GetItemData(11);
            mEquipGird[0].mLVL = 1;
            mEquipGird[0].mNum = 1;
            mEquipGird[0].mUId = GenUID();
            mEquipGird[0].ReCalcValue();


            mEquipGird[1].mRefItem = gDefine.gData.GetItemData(51);
            mEquipGird[1].mLVL = 1;
            mEquipGird[1].mNum = 1;
            mEquipGird[1].mUId = GenUID();
            mEquipGird[1].ReCalcValue();

            mEquipGird[2].mRefItem = gDefine.gData.GetItemData(31);
            mEquipGird[2].mLVL = 1;
            mEquipGird[2].mNum = 1;
            mEquipGird[2].mUId = GenUID();
            mEquipGird[2].ReCalcValue();

            mEquipGird[3].mRefItem = gDefine.gData.GetItemData(159);
            mEquipGird[3].mLVL = 1;
            mEquipGird[3].mNum = 1;
            mEquipGird[3].mUId = GenUID();
            mEquipGird[3].ReCalcValue();

            mEquipGird[4].mRefItem = gDefine.gData.GetItemData(179);
            mEquipGird[4].mLVL = 1;
            mEquipGird[4].mNum = 1;
            mEquipGird[4].mUId = GenUID();
            mEquipGird[4].ReCalcValue();

            AddItemToBag(99, 2);
            AddItemToBag(6, 1);


            // mBagGird[0].mRefItem = gDefine.gData.GetItemData(6);
            // mBagGird[0].mLVL = 1;

            // mBagGird[1].mRefItem = gDefine.gData.GetItemData(56);
            // mBagGird[1].mLVL = 1;
            // mBagGird[2].mRefItem = gDefine.gData.GetItemData(32);
            // mBagGird[2].mLVL = 1;

            //AddItemToBag(6, 1);
            //AddItemToBag(56, 1);
            //AddItemToBag(32, 1);


            mCrystal = 1000;
            mCoin = 1000;
            mTiLi = 20;
            mEXP = 0;
            mLVL = 1;

            mTiLiT = System.DateTime.Now.Ticks;

            mTalent.ClearAllData();
            mChapterEx.Clear();



            // mBagGird[3].mRefItem = gDefine.gData.GetItemData(71);
            //AddItemToBag(3, 71, 10000);
        }

        mSkillAdd.Load();

        gDefine.gShop.Load();

        gDefine.gBoxData.Load();

        RecalcEquipParam();
        mLoadFinish = true;


        Save();
    }

    // /// <summary>
    // /// 获得当前关卡的最大
    // /// </summary>
    // /// <param name="ChapterId"></param>
    // /// <returns></returns>
    // public int GetChapterPlayerMaxLVL(int ChapterId)
    // {
    //     if (ChapterId < mMaxChapterId)
    //         return ChapterId == 0 ? 30 : 50;
    //     else if (ChapterId == mMaxChapterId)
    //         return mMaxChapterLVL;
    //     else
    //         return 0;
    // }

    // /// <summary>
    // /// 解锁关卡
    // /// </summary>
    // /// <param name="ChapterId">新解锁的关卡ID</param>
    // public void UnLockChapter(int ChapterId)
    // {
    //     if (ChapterId > mMaxChapterId)
    //     {
    //         mMaxChapterId = ChapterId;
    //         mMaxChapterLVL = 0;
    //     }
    // }

    // public void AddItemToBag(int GirdId, int ItemId, int num)
    // {

    //     mBagGird[GirdId].mRefItem = gDefine.gData.GetItemData(ItemId);
    //     mBagGird[GirdId].mNum = num;
    //     mBagGird[GirdId].mLVL = 1;
    // }

    public void AddItemToBag(int ItemId, int num)
    {
        CItem it = gDefine.gData.GetItemData(ItemId);
        if (it.mEquipPos != gDefine.eEuqipPos.Null)
        {
            for (int i = 0; i < mBagGird.Length; i++)
            {
                if (mBagGird[i].mRefItem == null)
                {
                    mBagGird[i].mRefItem = it;
                    mBagGird[i].mNum = num;
                    mBagGird[i].mLVL = 1;
                    mBagGird[i].mUId = GenUID();
                    mBagGird[i].ReCalcValue();
                    break;
                }
            }
        }
        else
        {
            CGird sparegird = null;
            for (int i = 0; i < mBagGird.Length; i++)
            {
                if (mBagGird[i].mRefItem == null)
                {
                    if (sparegird == null)
                        sparegird = mBagGird[i];
                }
                else
                {
                    if (mBagGird[i].mRefItem.Id == ItemId)
                    {
                        mBagGird[i].mNum += num;
                        return;
                    }
                }
            }

            if (sparegird != null)
            {
                sparegird.mRefItem = it;
                sparegird.mNum = num;
                sparegird.mLVL = 1;
                sparegird.mUId = GenUID();
                sparegird.ReCalcValue();
            }
        }

        if (mLoadFinish)
            Save();

    }

    public void AddItemToBagQuick(int ItemId, int num)
    {
        if (mLoadFinish)
            mNeedToSaveBag = true;

        CItem it = gDefine.gData.GetItemData(ItemId);
        if (it.mEquipPos != gDefine.eEuqipPos.Null)
        {
            for (int i = 0; i < mBagGird.Length; i++)
            {
                if (mBagGird[i].mRefItem == null)
                {
                    mBagGird[i].mRefItem = it;
                    mBagGird[i].mNum = num;
                    mBagGird[i].mLVL = 1;
                    mBagGird[i].mUId = GenUID();
                    mBagGird[i].ReCalcValue();
                    break;
                }
            }
        }
        else
        {
            CGird sparegird = null;
            for (int i = 0; i < mBagGird.Length; i++)
            {
                if (mBagGird[i].mRefItem == null)
                {
                    if (sparegird == null)
                        sparegird = mBagGird[i];
                }
                else
                {
                    if (mBagGird[i].mRefItem.Id == ItemId)
                    {
                        mBagGird[i].mNum += num;
                        return;
                    }
                }
            }

            if (sparegird != null)
            {
                sparegird.mRefItem = it;
                sparegird.mNum = num;
                sparegird.mLVL = 1;
                sparegird.mUId = GenUID();
                sparegird.ReCalcValue();
            }
        }



    }

    void LoadGird()
    {
        int num = PlayerPrefs.GetInt("equip_item_num", 0);

        for (int i = 0; i < num; i++)
        {
            int index = PlayerPrefs.GetInt("equip_" + i.ToString() + "_Index", -1);
            mEquipGird[index].mLVL = PlayerPrefs.GetInt("equip_" + i.ToString() + "_LVL", 0);
            mEquipGird[index].mNum = PlayerPrefs.GetInt("equip_" + i.ToString() + "_Num", 0);
            mEquipGird[index].mGem[0] = PlayerPrefs.GetInt("equip_" + i.ToString() + "_Gem0", 0);
            mEquipGird[index].mGem[1] = PlayerPrefs.GetInt("equip_" + i.ToString() + "_Gem1", 0);
            mEquipGird[index].mGem[2] = PlayerPrefs.GetInt("equip_" + i.ToString() + "_Gem2", 0);

            mEquipGird[index].mUId = long.Parse(PlayerPrefs.GetString("equip_" + i.ToString() + "_UId", "0"));

            int itemId = PlayerPrefs.GetInt("equip_" + i.ToString() + "_ItemId", 0);
            mEquipGird[index].mRefItem = gDefine.gData.GetItemData(itemId);
            mEquipGird[index].ReCalcValue();

        }


        num = PlayerPrefs.GetInt("bag_item_num", 0);
        for (int i = 0; i < num; i++)
        {
            int index = PlayerPrefs.GetInt("bag_" + i.ToString() + "_Index", -1);
            mBagGird[index].mLVL = PlayerPrefs.GetInt("bag_" + i.ToString() + "_LVL", 0);
            mBagGird[index].mNum = PlayerPrefs.GetInt("bag_" + i.ToString() + "_Num", 0);
            mBagGird[index].mGem[0] = PlayerPrefs.GetInt("bag_" + i.ToString() + "_Gem0", 0);
            mBagGird[index].mGem[1] = PlayerPrefs.GetInt("bag_" + i.ToString() + "_Gem1", 0);
            mBagGird[index].mGem[2] = PlayerPrefs.GetInt("bag_" + i.ToString() + "_Gem2", 0);

            mBagGird[index].mUId = long.Parse(PlayerPrefs.GetString("bag_" + i.ToString() + "_UId", "0"));


            int itemId = PlayerPrefs.GetInt("bag_" + i.ToString() + "_ItemId", 0);
            mBagGird[index].mRefItem = gDefine.gData.GetItemData(itemId);
            mBagGird[index].ReCalcValue();
        }


        // else
        // {
        //     mEquipGird[0].mRefItem = gDefine.gData.GetItemData(1);
        //     mEquipGird[0].mLVL = 1;
        //     mEquipGird[0].mUId = GenUID();
        //     mEquipGird[1].mRefItem = gDefine.gData.GetItemData(51);
        //      mEquipGird[1].mLVL = 1;
        //      mEquipGird[1].mUId = GenUID();
        //     mEquipGird[2].mRefItem = gDefine.gData.GetItemData(31);
        //      mEquipGird[2].mLVL = 1;
        //      mEquipGird[2].mUId = GenUID();
        //     mEquipGird[3].mRefItem = gDefine.gData.GetItemData(159);
        //      mEquipGird[3].mLVL = 1;
        //      mEquipGird[3].mUId = GenUID();
        //     mEquipGird[4].mRefItem = gDefine.gData.GetItemData(179);
        //      mEquipGird[4].mLVL = 1;
        //      mEquipGird[4].mUId = GenUID();


        //     //mBagGird[0].mRefItem = gDefine.gData.GetItemData(6);
        //      //mBagGird[0].mLVL = 1;
        //     //mBagGird[1].mRefItem = gDefine.gData.GetItemData(56);
        //     // mBagGird[1].mLVL = 1;
        //    // mBagGird[2].mRefItem = gDefine.gData.GetItemData(32);
        //     // mBagGird[2].mLVL = 1;

        //     Crystal = 1000;

        //     AddItemToBag(6,1);
        //     AddItemToBag(56,1);
        //     AddItemToBag(32,1);
        //     AddItemToBag(71,100);

        //     //mBagGird[3].mRefItem = gDefine.gData.GetItemData(71);
        //     // mBagGird[3].mLVL = 1;
        // }



    }

    public void SaveGird()
    {
        int index = 0;
        for (int i = 0; i < mEquipGird.Length; i++)
        {
            if (mEquipGird[i].mRefItem != null)
            {
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_Index", i);
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_ItemId", mEquipGird[i].mRefItem.Id);
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_Num", mEquipGird[i].mNum);
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_LVL", mEquipGird[i].mLVL);
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_Gem0", mEquipGird[i].mGem[0]);
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_Gem1", mEquipGird[i].mGem[1]);
                PlayerPrefs.SetInt("equip_" + index.ToString() + "_Gem2", mEquipGird[i].mGem[2]);
                PlayerPrefs.SetString("equip_" + index.ToString() + "_UId", mEquipGird[i].mUId.ToString());
                index++;
            }
        }
        PlayerPrefs.SetInt("equip_item_num", index);

        index = 0;
        for (int i = 0; i < mBagGird.Length; i++)
        {
            if (mBagGird[i].mRefItem != null)
            {
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Index", i);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_ItemId", mBagGird[i].mRefItem.Id);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Num", mBagGird[i].mNum);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_LVL", mBagGird[i].mLVL);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Gem0", mBagGird[i].mGem[0]);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Gem1", mBagGird[i].mGem[1]);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Gem2", mBagGird[i].mGem[2]);
                PlayerPrefs.SetString("bag_" + index.ToString() + "_UId", mBagGird[i].mUId.ToString());
                index++;
            }
        }
        PlayerPrefs.SetInt("bag_item_num", index);
    }

    public void SaveBag()
    {
        int index = 0;
        for (int i = 0; i < mBagGird.Length; i++)
        {
            if (mBagGird[i].mRefItem != null)
            {
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Index", i);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_ItemId", mBagGird[i].mRefItem.Id);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Num", mBagGird[i].mNum);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_LVL", mBagGird[i].mLVL);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Gem0", mBagGird[i].mGem[0]);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Gem1", mBagGird[i].mGem[1]);
                PlayerPrefs.SetInt("bag_" + index.ToString() + "_Gem2", mBagGird[i].mGem[2]);
                PlayerPrefs.SetString("bag_" + index.ToString() + "_UId", mBagGird[i].mUId.ToString());
                index++;
            }
        }
        PlayerPrefs.SetInt("bag_item_num", index);
        PlayerPrefs.Save();
    }

    public void ChangeEquipGird(CGird Gird)
    {
        int bagIndex = FindSpareGridInBag();
        if (bagIndex >= 0)
        {
            int equipIndex = (int)Gird.mRefItem.mEquipPos;
            CGird refEquip = mEquipGird[equipIndex];
            mEquipGird[equipIndex] = mBagGird[bagIndex];
            mBagGird[bagIndex] = refEquip;
            RecalcEquipParam();
            gDefine.gMainUI.mRefMainEquip.Refresh();
            Save();
        }
    }

    public void ChangeBagGird(CGird Gird)
    {
        int EquipIndex = (int)Gird.mRefItem.mEquipPos;
        CGird refEquip = mEquipGird[EquipIndex];
        mEquipGird[EquipIndex] = Gird;

        int girdIndex = FindGirdIndexByUID(Gird.mUId);
        mBagGird[girdIndex] = refEquip;

        RecalcEquipParam();
        gDefine.gMainUI.mRefMainEquip.Refresh();

        if (Gird.mRefItem.mWeaponClass == gDefine.eWeaponClass.ShortGun ||
            Gird.mRefItem.mWeaponClass == gDefine.eWeaponClass.longGun)
        {
            AudioClip clip = gDefine.gData.GetSoundClip(31);
            if (clip != null)
                gDefine.gSound.Play(clip);
        }

        Save();

    }


    // public bool ChangeGird(int Index)
    // {
    //     if (Index < 1000)
    //     {
    //         if (mEquipGird[Index].mRefItem == null)
    //             return false;
    //         else
    //         {
    //             CGird refEquip = mEquipGird[Index];
    //             int bagIndex = FindSpareGridInBag();
    //             if (bagIndex >= 0)
    //             {
    //                 mEquipGird[Index] = mBagGird[bagIndex];
    //                 mBagGird[bagIndex] = refEquip;
    //                 RecalcEquipParam();
    //                 gDefine.gMainUI.mRefMainEquip.Refresh();

    //             }

    //         }
    //     }
    //     else
    //     {
    //         Index -= 1000;
    //         if (mBagGird[Index].mRefItem == null)
    //             return false;
    //         else
    //         {
    //             int EquipIndex = (int)mBagGird[Index].mRefItem.mEquipPos;
    //             CGird refEquip = mEquipGird[EquipIndex];
    //             mEquipGird[EquipIndex] = mBagGird[Index];
    //             mBagGird[Index] = refEquip;
    //             RecalcEquipParam();
    //             gDefine.gMainUI.mRefMainEquip.Refresh();

    //             if (mBagGird[Index].mRefItem.mWeaponClass == gDefine.eWeaponClass.ShortGun ||
    //                 mBagGird[Index].mRefItem.mWeaponClass == gDefine.eWeaponClass.longGun)
    //             {
    //                 AudioClip clip = gDefine.gData.GetSoundClip(31);
    //                 if (clip != null)
    //                     gDefine.gSound.Play(clip);
    //             }
    //         }
    //     }

    //     return true;
    // }

    void RecalcEquipParam()
    {
        if (mEquipGird[0].mRefItem == null)
            gDefine.mCurUseGirlType = gDefine.eCharType.eDSword;
        else
        {
            if (mEquipGird[0].mRefItem.mWeaponClass == gDefine.eWeaponClass.DSword)
                gDefine.mCurUseGirlType = gDefine.eCharType.eDSword;
            else if (mEquipGird[0].mRefItem.mWeaponClass == gDefine.eWeaponClass.BigSword)
                gDefine.mCurUseGirlType = gDefine.eCharType.eBigSword;
            else
                gDefine.mCurUseGirlType = gDefine.eCharType.eLun;
        }


        if (mEquipGird[1].mRefItem == null)
            gDefine.mCurUseGunType = gDefine.eGunType.eShortGun;
        else
        {
            if (mEquipGird[1].mRefItem.mWeaponClass == gDefine.eWeaponClass.ShortGun)
                gDefine.mCurUseGunType = gDefine.eGunType.eShortGun;
            else
                gDefine.mCurUseGunType = gDefine.eGunType.eLongGun;
        }


        InitBeforeGame();
    }

    int FindSpareGridInBag()
    {
        for (int i = 0; i < mBagGird.Length; i++)
        {
            if (mBagGird[i].mRefItem == null)
                return i;
        }
        return -1;
    }

    int FindGirdIndexByUID(long UID)
    {
        for (int i = 0; i < mBagGird.Length; i++)
        {
            if (mBagGird[i].mUId == UID)
                return i;
        }
        return -1;
    }

    public CGird FindGridByItemId(int ItemId)
    {
        foreach (CGird gird in mBagGird)
        {
            if (gird?.mRefItem?.Id == ItemId)
                return gird;
        }
        return null;
    }

    //当前是否免疫伤害
    public bool IsCurIgonrDamage()
    {
        return Time.time < mIgonrDamageT;
    }

    public void InitBeforeGame()
    {

        //重算所有数据
        mChangeToGunPerc = 20;

        mGunDamage = 0;

        CPCLvLParam pcdata = gDefine.gData.GetPCData(LVL);

        mHp = mHpMax = 0;

        mDamage = 0;

        mDoubleDamagePerc = 0.0f;

        mDodgePerc = 0;

        mDamageReduce = 0;

        mClotheMaxHpCount = 0;

        mHeavyAtkUpCount = 0;

        mLearnSkillNum = 0;

        mDarkShadowT = Time.time + 30;

        mTmpHelthSE = null;

        mBloodSuckPerc = 0;
        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.BloodSuck);
        if (d != null)
            mBloodSuckPerc = d.mLearnNum * 10;

        mHeadShootPerc = 10;
        d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.GunMaster);
        if (d != null)
            mHeadShootPerc += d.mLearnNum * 5;

        //装备
        for (int i = 0; i < mEquipGird.Length; i++)
        {
            if (mEquipGird[i].mRefItem != null)
            {
                mDamage += mEquipGird[i].mDamage;
                mGunDamage += mEquipGird[i].mGunDamage;
                mHp += mEquipGird[i].mHp;
                mDoubleDamagePerc += mEquipGird[i].mCrit;
                mDodgePerc += mEquipGird[i].mDodge;
            }
        }

        d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.FirePowerUp);
        if (d != null)
            mGunDamage += d.mLearnNum * 0.1f * d.mLearnNum;


        // if (mEquipGird[(int)gDefine.eEuqipPos.MainWeapon].mRefItem != null)
        //     mDamage += mEquipGird[(int)gDefine.eEuqipPos.MainWeapon].mRefItem.mDamage;
        // if (mEquipGird[(int)gDefine.eEuqipPos.Clothe].mRefItem != null)
        //     mHp += mEquipGird[(int)gDefine.eEuqipPos.Clothe].mRefItem.mHp;
        // if (mEquipGird[(int)gDefine.eEuqipPos.Cloak].mRefItem != null)
        //     mHp += mEquipGird[(int)gDefine.eEuqipPos.Cloak].mRefItem.mHp;



        // if (mEquipGird[(int)gDefine.eEuqipPos.GunWeapon].mRefItem != null)
        //     mGunDamage = mEquipGird[(int)gDefine.eEuqipPos.GunWeapon].mRefItem.mGunDamage;
        // else
        //     mGunDamage = mEquipGird[(int)gDefine.eEuqipPos.MainWeapon].mRefItem.mDamage / 3;




        mHpMax = mHp;

        //天赋
        mTalent.ReCalcPlayerTalentValue();

        mDamage += mTalent.mDamage;//伤害

        mHp = mHpMax = mHp + (int)mTalent.mHp;//生命上限

        mDoubleDamagePerc += mTalent.mLuck; //暴击加成

        d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.CriticalAtk);
        if (d != null)
            mDoubleDamagePerc += 10 * d.mLearnNum;


        mDodgePerc += mTalent.mDodge;

        mDamageReduce = mTalent.mDef;


        //披风特殊提供
        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Cloak];
        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 1)
        {
            mDamageReduce += 0.05f;
        }

        //衣服特殊提供
        gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Clothe];
        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 1)
        {
            mDamageReduce += 0.05f;
        }

        if (mHp <= 0)
            mHp = mHpMax = 1;
        if (mDamage <= 0)
            mDamage = 1;
        if (mGunDamage <= 0)
            mGunDamage = 1;

    }

    public int GetBloodSuckHp()
    {
        if (mCurMagicData != null && mCurMagicData.mType == CMagic.eMagic.BloodSuck &&
        mCurMagicData.mNum > 0)
        {
            mCurMagicData.mNum--;
            return (int)(mHpMax * (mBloodSuckNumPerc + 0.05f));
        }
        else if (mBloodSuckPerc > 0)
        {
            if (UnityEngine.Random.Range(0, 100) <= mBloodSuckPerc)
                return (int)(mHpMax * mBloodSuckNumPerc);
        }
        return 0;
    }

    public void SkillUpdate()
    {
        foreach (CSkill s in mSkillAutoArr)
        {
            s.Update();
        }
    }

    public CSkill FindSkillInLearn(CSkill.eSkill SkillType)
    {
        foreach (CSkill skill in mLearnSkillInFight)
        {
            if (skill.mType == SkillType)
                return skill;
        }
        return null;
    }

    public void PreLoad()
    {
        //skill
        int num = PlayerPrefs.GetInt("LearnSkillNum", 0);
        for (int i = 0; i < num; i++)
        {
            CSkill.eSkill e = (CSkill.eSkill)PlayerPrefs.GetInt("LearnSkill_" + i.ToString(), 0);
            CSkill skill = gDefine.gSkill.GetSkill(e);
            if (skill != null)
                AddLearnSkillInFigth(skill, true);
        }
        //adv
        gDefine.gGameMainUI.mRefAdvUI.Pre_Load();
    }

    public void AddLearnSkillInFigth(CSkill Skill, bool IsPreLoad = false)
    {
        if(IsPreLoad)
        {
            gDefine.gSkill.Choose(Skill);
        }

        PlayerPrefs.SetInt("LearnSkill_" + mLearnSkillNum.ToString(), (int)Skill.mType);
        mLearnSkillNum++;
        PlayerPrefs.SetInt("LearnSkillNum", mLearnSkillNum);

        mLearnSkillInFight.Add(Skill);
        Skill.mT = Time.time;
        Skill.mSkillT = 0;
        switch (Skill.mType)
        {
            case CSkill.eSkill.QuantumMask:
                mSkillAutoArr.Add(Skill);
                break;

            case CSkill.eSkill.GunMaster://枪械专家
                mChangeToGunPerc += 20;
                mHeadShootPerc += 20; //爆头率+20%
                break;
            case CSkill.eSkill.FirePowerUp://枪械专家
                mGunDamage += mGunDamage * 0.3f;

                break;
            case CSkill.eSkill.PetDamageUp://枪械专家
                break;
            case CSkill.eSkill.PetAtkUp://枪械专家
                break;

            case CSkill.eSkill.HpUpMin:
                // mBuffHpInFight += (int)(mHpMax * 0.1f);
                float oldPerc = (float)mHp / (float)mHpMax;
                int addhp = (int)(mHpMax * 0.1f);
                float newperc = (float)mHp / (float)(mHpMax + addhp);
                mHpMax += addhp;
                mHp += addhp;
                float realPerc = (float)mHp / (float)mHpMax;
                if (!IsPreLoad)
                    gDefine.RefreshHPUI(oldPerc, newperc, realPerc);
                break;
            case CSkill.eSkill.BloodSuck:
                mBloodSuckNumPerc += 0.1f;
                mBloodSuckPerc += 10;
                break;
            case CSkill.eSkill.DamageUpSmall:
                mDamage = (int)(mDamage * 1.05f);
                break;
            case CSkill.eSkill.DamageUpBig:
                mDamage = (int)(mDamage * 1.12f);
                break;

            case CSkill.eSkill.Berserk:
                float oldhpPerc = (float)mHp / (float)mHpMax;

                float addPrec = 0.2f;
                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.Berserk);
                if (d != null)
                    addPrec += 0.1f * d.mLearnNum;

                mHp = (int)(mHp * 0.4f);
                if(mHp<1) mHp = 1;
              

                mDamage += (int)(mDamage * addPrec);

                float newhpPerc = oldhpPerc / 3;
                float rPerc = (float)mHp / (float)mHpMax;
                if (!IsPreLoad)
                {
                    gDefine.RefreshHPUI(oldhpPerc, newhpPerc, rPerc);

                    GameObject se = GameObject.Instantiate(gDefine.gData.mUIAtkAddSEPreb);
                    se.transform.SetParent(gDefine.GetPcRefMid());
                    se.transform.localPosition = Vector3.zero;

                    gDefine.PlaySound(87);
                }
                break;
            case CSkill.eSkill.BackFlash:
                //--finish--
                break;
            case CSkill.eSkill.Ice:
                mSkillAutoArr.Add(Skill);
                break;

            case CSkill.eSkill.LifeSpring:
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.HealLight:
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.LightChain:
                //--finish--
                break;
            case CSkill.eSkill.LightBall:
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.Thunder:
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.CoinMore:
                mMoreMoney = true;
                break;
            case CSkill.eSkill.Poison:
                //--finish--
                break;
            case CSkill.eSkill.XianJing:
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.DamageMore:

                float add = 0.1f;
                CSkillAddData sd = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.DamageMore);
                if (sd != null)
                    add += sd.mLearnNum * 0.02f;

                mDamage += (int)(mDamage * add);
                if (!IsPreLoad)
                {
                    GameObject se = GameObject.Instantiate(gDefine.gData.mUIAtkAddSEPreb);
                    se.transform.SetParent(gDefine.GetPcRefMid());
                    se.transform.localPosition = Vector3.zero;
                     gDefine.PlaySound(87);
                }

                break;
            case CSkill.eSkill.DeathFinger:
                //--finish--
                break;

            case CSkill.eSkill.ElectricityArround:
                //create ball to pc
                {
                    GameObject o = GameObject.Instantiate(gDefine.gData.mElectBallAround);
                    se_Skill_ElectricityArround script = o.GetComponent<se_Skill_ElectricityArround>();
                    Transform t = gDefine.GetPcRefMid().transform;
                    script.Init(t);
                }
                break;
            case CSkill.eSkill.CriticalAtk:
                mDoubleDamagePerc += 20;
                if (!IsPreLoad)
                {
                    GameObject se = GameObject.Instantiate(gDefine.gData.mUIAtkAddSEPreb);
                    se.transform.SetParent(gDefine.GetPcRefMid());
                    se.transform.localPosition = Vector3.zero;
                     gDefine.PlaySound(87);
                }
                break;
            case CSkill.eSkill.BulletBack:
                mBulletBackPerc += 10;
                break;
            case CSkill.eSkill.GroundArrow:
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.ForceAwaken:
                //--finish--
                break;
            case CSkill.eSkill.DSWordGirl_Flush:
                //--finish--
                break;
            case CSkill.eSkill.DSWordGirl_FlushSecond:
                //--finish--
                break;
            case CSkill.eSkill.BigSword_SwordRain:
                //--finish--
                mSkillAutoArr.Add(Skill);
                break;
            case CSkill.eSkill.BigSword_KillAll:
                //--finish--
                break;
            case CSkill.eSkill.BigSword_BigLun:
                //--finish--
                break;

            case CSkill.eSkill.BigSword_SwordFlush:
                //--finish--
                break;
            case CSkill.eSkill.BigLun_HalfKill:
                //--finish--
                break;
            case CSkill.eSkill.BigLun_Lighting:
                //--finish--
                break;
            case CSkill.eSkill.BigLun_Saw:
                //--finish--
                break;
            case CSkill.eSkill.BigLun_DoubleWave:
                //--finish--
                break;


        }
    }

    public void AddEXP(int Exp)
    {
        mEXP += Exp;
        CPCLvLParam pcLvLData = gDefine.gData.GetPCData(LVL);
        if (mEXP >= pcLvLData.mEXP)
        {
            LVL++;
            mEXP -= pcLvLData.mEXP;
            mTalent.AddPointToAll();
        }
        gDefine.gMainUI.mRefMainUp.Refresh();
    }


}
