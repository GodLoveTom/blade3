using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CTalent
{
    public enum eTalentType
    {
        UnForce = 0,
        StarHeart,
        QuantumArmor,
        JediSprite,
        LuckAtk,
        GhostDef, //幽灵护体
        Count,
    }
    public eTalentType mType;
    public string mName;
    public int mDes;
    public int mLvL = 0;  //当前等级
    public int mPoint = 0; //当前的点数

    public void Clear()
    {
        mLvL = 0;
        mPoint = 0;
    }

    public float GetValue()
    {
        switch (mType)
        {
            case eTalentType.UnForce:
                return UnForceValue();
            case eTalentType.StarHeart:
                return StarHeartValue();
            case eTalentType.QuantumArmor:
                return 0.05f;
            case eTalentType.JediSprite:
                return -0.3f;
            case eTalentType.LuckAtk:
                return 0.1f;
            case eTalentType.GhostDef:
                return 0.1f;
        }
        return 0.0f;
    }

    public string GetValueStr()
    {
        switch (mType)
        {
            case eTalentType.UnForce:
                return "+" + UnForceValue(mLvL+1).ToString();
            case eTalentType.StarHeart:
                return "+"+StarHeartValue(mLvL+1).ToString();
            case eTalentType.QuantumArmor:
                return "+0.1%";
            case eTalentType.JediSprite:
                return "+0.5%";
            case eTalentType.LuckAtk:
                return "+0.1%";
            case eTalentType.GhostDef:
                return "+0.1%";
        }
        return "";
    }

     public float UnForceValue()
     {
        return UnForceValue(mLvL);
     }

    public float UnForceValue(int LvL)
    {
        if(LvL==0)
            return 0;
        if (LvL <= 10)
            return 2;
        else if (LvL <= 16)
            return 3;
        else if (LvL <= 22)
            return 4;
        else if (LvL <= 28)
            return 5;
        else if (LvL <= 34)
            return 8.5f;
        else if (LvL <= 40)
            return 13;
        else if (LvL <= 46)
            return 18.5f;
        else if (LvL <= 53)
            return 25;
        else if (LvL <= 60)
            return 37;
        else
            return 0;


    }

    public float StarHeartValue()
    {
        return StarHeartValue(mLvL);
    }

    public float StarHeartValue(int LvL)
    {
        if( LvL == 0 )
            return 0;
        else if (LvL <= 4)
            return 7;
        else if (LvL <= 10)
            return 7f;
        else if (LvL <= 16)
            return 10;
        else if (LvL <= 22)
            return 13;
        else if (LvL <= 28)
            return 19.5f;
        else if (LvL <= 34)
            return 29;
        else if (LvL <= 40)
            return 43.5f;
        else if (LvL <= 46)
            return 65;
        else if (LvL <= 53)
            return 83.5f;
        else if (LvL <= 60)
            return 125.5f;
        else
            return 0;
    }

    public void Save()
    {
        PlayerPrefs.SetInt("Talent_"+mType.ToString()+"_LvL", mLvL);
        PlayerPrefs.SetInt("Talent_"+mType.ToString()+"_Point", mPoint);
        PlayerPrefs.Save();
    }

    public void Load()
    {
       mLvL = PlayerPrefs.GetInt("Talent_"+mType.ToString()+"_LvL", 0);
       mPoint = PlayerPrefs.GetInt("Talent_"+mType.ToString()+"_Point", 0);
    }

}

public class CPlayerTalent
{
    List<CTalent> mTalentArr = new List<CTalent>();
    public float mDamage = 0;//伤害
    public float mHp = 0;//生命上限
    public float mDef = 0;//伤害减免
    public float mKillPrec = 30;//残暴概率
    public float mLuck = 0;//会心一击
    public float mDodge = 0;//闪避

    public void Init()
    {
        ReInitValue();
        LoadFromXml();
        Load();
    }

    public void ClearAllData()
    {
        ReInitValue();
        foreach(CTalent t in mTalentArr)
            t.Clear();
    }

    public void AddPointToAll()
    {
        foreach( CTalent t in mTalentArr)
            t.mPoint += 2;
        Save();
    }

    /// <summary>
    /// 获取描述信息
    /// </summary>
    /// <returns></returns>
    public string GetValueDes( CTalent.eTalentType TalentType)
    {
         switch (TalentType)
        {
            case CTalent. eTalentType.UnForce:
                return mDamage > 0 ? "伤害: +" + mDamage.ToString() : "伤害: " + mDamage.ToString();
            case CTalent. eTalentType.StarHeart:
                return mHp>0? "血量上限: +"+ mHp.ToString():"血量上限: "+ mHp.ToString();
            case CTalent. eTalentType.QuantumArmor:
                return mDef > 0 ? "伤害减免: -"+ ((int)(mDef*100)).ToString() + "%" : "伤害减免: "+ ((int)(mDef*100)).ToString() + "%" ;
            case CTalent. eTalentType.JediSprite:
                return mKillPrec < 30? "终结技冷却: -"+ (30-mKillPrec).ToString()+"秒(当前:"+ mKillPrec.ToString()+")" 
                :"终结技冷却: 30秒" ;
            case CTalent. eTalentType.LuckAtk:
                return mLuck > 0 ? "暴击率: +"+ ((int)(mLuck*100)).ToString() + "%":"暴击率: 0%";
            case CTalent. eTalentType.GhostDef:
                return mDodge > 0 ? "闪避率: +" +((int)(mDodge*100)).ToString() + "%" : 
                 "闪避率: 0%" ;
        }
        return "";
    }


    public void Save()
    {
        foreach(CTalent talent in mTalentArr)
            talent.Save();
    }

    public void Load()
    {
        foreach(CTalent talent in mTalentArr)
            talent.Load();
    }

    void ReInitValue()
    {
        mDamage = 0;
        mHp = 0;
        mDef = 0;
        mKillPrec = 30;
        mLuck = 0;
        mDodge = 0;
    }

    public void LoadFromXml()
    {
        string str = gDefine.gData.mTalentText.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] talentArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < talentArr.Length; i++)
        {
            string[] valueArr = talentArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);
            CTalent data = new CTalent();
            data.mName = valueArr[0];
            data.mDes = int.Parse(valueArr[1]) ;
            data.mType = (CTalent.eTalentType)Enum.Parse(typeof(CTalent.eTalentType), valueArr[2]);

            mTalentArr.Add(data);
        }
    }

    public CTalent FindPlayerTalent(CTalent.eTalentType TalentType)
    {
        foreach (CTalent t in mTalentArr)
            if (t.mType == TalentType)
                return t;
        return null;
    }

    public void ReCalcPlayerTalentValue()
    {
        ReInitValue();

        foreach (CTalent t in mTalentArr)
        {
            switch (t.mType)
            {
                case CTalent.eTalentType.UnForce:
                    for (int i = 0; i <= t.mLvL; i++)
                        mDamage += t.UnForceValue(i);
                    break;
                case CTalent.eTalentType.StarHeart:
                    for (int i = 0; i <= t.mLvL; i++)
                        mHp += t.StarHeartValue(i);
                    break;
                case CTalent.eTalentType.QuantumArmor:
                    mDef = 0.001f * t.mLvL;
                    break;
                case CTalent.eTalentType.JediSprite:
                    mKillPrec =  0.5f * t.mLvL;
                    break;
                case CTalent.eTalentType.LuckAtk:
                    mLuck = 0.1f * t.mLvL;
                    break;
                case CTalent.eTalentType.GhostDef:
                    mDodge = 0.1f * t.mLvL;
                    break;
            }
        }
    }
    
}
