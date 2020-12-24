//附魔

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMagicData
{
    public CMagic.eMagic mType;
    public int mNum;
    public float mCoolDownT = 0;

    public void ResetNum(int Times=1)
    {
        switch (mType)
        {
            case CMagic.eMagic.BloodSuck:
                mNum = 7*Times;
                break;
            case CMagic.eMagic.Critic:
                mNum = 7*Times;
                break;
            case CMagic.eMagic.Posion:
                mNum = 5*Times;
                break;
            case CMagic.eMagic.Def:
                mNum = 4*Times;
                break;
            case CMagic.eMagic.Luck:
                mNum = 7*Times;
                break;
        }
        
    }

    public string GetName()
    {
        switch (mType)
        {
            case CMagic.eMagic.BloodSuck:
                return gDefine.GetStr(93);//吸血
            case CMagic.eMagic.Critic:
                return gDefine.GetStr(60);//暴击
            case CMagic.eMagic.Posion:
                return gDefine.GetStr(390);// "荼毒";
            case CMagic.eMagic.Def:
                return gDefine.GetStr(388);//"抵挡";
            case CMagic.eMagic.Luck:
                return gDefine.GetStr(389); //"幸运";
        }
        return "";
    }

    public string GetDes()
    {
        switch (mType)
        {
            case CMagic.eMagic.BloodSuck:
                return gDefine.GetStr(291);//("接下来的7次攻击附加吸血") ;
            case CMagic.eMagic.Critic:
                return gDefine.GetStr(292);//"接下来的7次攻击为暴击");
            case CMagic.eMagic.Posion:
                return gDefine.GetStr(293);//"接下来的5波敌人初始即中毒");
            case CMagic.eMagic.Def:
                return gDefine.GetStr(294);//"抵挡接下来的4次攻击");
            case CMagic.eMagic.Luck:
                return gDefine.GetStr(295);//"接下来的7次攻击获得金币或钻石");
        }
        return "";
    }

    public Sprite GetIcon()
    {
        switch (mType)
        {
            case CMagic.eMagic.BloodSuck:
                return  gDefine.gABLoad.GetSprite("icon.bytes", "吸血") ;
            case CMagic.eMagic.Critic:
                return gDefine.gABLoad.GetSprite("icon.bytes", "附魔暴击") ;
            case CMagic.eMagic.Posion:
                return gDefine.gABLoad.GetSprite("icon.bytes", "荼毒") ;
            case CMagic.eMagic.Def:
                return gDefine.gABLoad.GetSprite("icon.bytes", "抵挡") ;
            case CMagic.eMagic.Luck:
                return gDefine.gABLoad.GetSprite("icon.bytes", "幸运") ;
        }
        return null;
    }
}

public class CMagic 
{
    public enum eMagic
    {
        BloodSuck,
        Critic,
        Posion,
        Def,
        Luck,
    }
    List<CMagicData> mData = new List<CMagicData>();
    public CMagicData[] ReSetMagicData()
    {
        mData.Clear();
        for (eMagic c = eMagic.BloodSuck; c <= eMagic.Luck; c++)
        {
            CMagicData data = new CMagicData();
            data.mType = c;
            mData.Add(data);
        }


        mData.RemoveAt(Random.Range(0, mData.Count));
        mData.RemoveAt(Random.Range(0, mData.Count));

        return mData.ToArray();
    }

    public CMagicData[] GetDataArr()
    {
        return mData.ToArray();
    }


    public void GetMagic( CMagic.eMagic MagicType)
    {
         for (int i = 0; i < mData.Count; i++)
            if (mData[i].mType == MagicType)
            {
                mData[i].ResetNum();
                gDefine.gPlayerData.mCurMagicData = mData[i];
                mData.RemoveAt(i);
            }
    }

    public void GetMagicDouble( CMagic.eMagic MagicType)
    {
         for (int i = 0; i < mData.Count; i++)
            if (mData[i].mType == MagicType)
            {
                mData[i].ResetNum(2);
                gDefine.gPlayerData.mCurMagicData = mData[i];
                mData.RemoveAt(i);
            }
    }


}
