using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class CSkillAddData
{
    public CSkillAdd.eSkillAdd mType; //类型
    public string mName;
    public int mNameId;
    public string mIconName;
    public string mDes;
    public int mDesId;
    public int mPieceItemId;
    public int mLearnNum; //学过次数
    public int mShowNum; //在3选1中出现次数
    public bool mChoosed; //true 在3选1中被选中过


    public string GetNameLocal()
    {
        if(mNameId>0)
            return gDefine.GetStr(mNameId);
        else
            return gDefine.GetStr(mName);
    }

    public int GetSkillAddUIValue()
    {
        switch( mType)
        {
            case CSkillAdd.eSkillAdd.DSWordGirl_Flus://一闪
                return mLearnNum+1;
             case CSkillAdd.eSkillAdd.BackFlash://后闪
                return mLearnNum*10+20;
            case CSkillAdd.eSkillAdd.DSWordKill://剑气斩
                return mLearnNum*5+40;
                case CSkillAdd.eSkillAdd.DSWordAirFlush: //空中一闪
                return mLearnNum*10 + 20;

                 case CSkillAdd.eSkillAdd.QuantumMask://量子罩
                return mLearnNum+3;
                 case CSkillAdd.eSkillAdd.GunMaster://枪械专家
                return mLearnNum*5+10;
                 case CSkillAdd.eSkillAdd.FirePowerUp://火力强化
                return mLearnNum*10+30;
                 case CSkillAdd.eSkillAdd.BloodSuck://吸血
                return mLearnNum*10+10;
                 case CSkillAdd.eSkillAdd.Berserk://狂战士
                return mLearnNum*10+100;
                 case CSkillAdd.eSkillAdd.LifeSpring://生命之泉
                return mLearnNum+2;
                    case CSkillAdd.eSkillAdd.HealLight://治愈之光
                return mLearnNum*10+10;
                    case CSkillAdd.eSkillAdd.LightChain://闪电链
                return  8 - mLearnNum*2;
                    case CSkillAdd.eSkillAdd.LightBall://闪电球
                return 12 + mLearnNum*3;
                    case CSkillAdd.eSkillAdd.Thunder://落雷
                return mLearnNum*10;
                    case CSkillAdd.eSkillAdd.CoinMore://金币收藏
                return mLearnNum*10;
                    case CSkillAdd.eSkillAdd.Poison://毒性攻击
                return 5+1*mLearnNum;

                 case CSkillAdd.eSkillAdd.XianJing://磁暴陷阱
                return 3+ mLearnNum;
                 case CSkillAdd.eSkillAdd.DamageMore://额外伤害
                return 10 + mLearnNum*2;
                 case CSkillAdd.eSkillAdd.DeathFinger://死亡宣告
                return 10+ mLearnNum*10;
                 case CSkillAdd.eSkillAdd.ElectricityArround://环绕电磁
                return mLearnNum*10;
                case CSkillAdd.eSkillAdd.CriticalAtk://暴击
                return 20+ mLearnNum*10;
                 case CSkillAdd.eSkillAdd.GroundArrow://地弩
                return 10+mLearnNum*3;
                 case CSkillAdd.eSkillAdd.ForceAwaken://元气护体
                return 50+10*mLearnNum;
                 case CSkillAdd.eSkillAdd.BigLun_HalfKill://截斩
                return 150 + mLearnNum*10;

                   case CSkillAdd.eSkillAdd.BigLun_Saw://电锯
                return 8+3*mLearnNum;
                   case CSkillAdd.eSkillAdd.BigLun_DoubleWave://双向波
                return mLearnNum;
                   case CSkillAdd.eSkillAdd.BigLun_Cut://切割
                return 30 + mLearnNum*10;
                   
        }
        return 0;
    }


    public string GetDesStrLocal()
    {
        if(mDesId>0)
            return gDefine.GetStr(mDesId);
        else
        {
            return gDefine.GetStr(mDes);
        }
    }


    public int LVLUPNeedCrystal()
    {
        switch(mLearnNum)
        {
            case 0:
                return 300;
            case 1:
                return 600;
            case 2:
                return 1000;
        }
        return 1000;
    }

    public int LVLUPNeedPiece()
    {
           switch(mLearnNum)
        {
            case 0:
                return 5;
            case 1:
                return 10;
            case 2:
                return 15;
        }
        return 100;
    }

    public bool CanLvLUp(out int error)
    {
        error = 0;
        if(mLearnNum>=3)
        {
            error = 3;
            return false;
        }
            
        
        int pieceNum = LVLUPNeedPiece();
        int crystalNum = LVLUPNeedCrystal();
        if(gDefine.gPlayerData.Crystal >= crystalNum )
        {
            CGird gird =  gDefine.gPlayerData.FindGridByItemId( mPieceItemId );
            if( gird?.mNum >= pieceNum)
                return true;
            else
                error = 2;    
        }
        else
        {
            error = 1;
        }
        return false;
    }

    public bool LVLUp()
    {
        if(mLearnNum>=3)
            return false;
        
        int pieceNum = LVLUPNeedPiece();
        int crystalNum = LVLUPNeedCrystal();
        if(gDefine.gPlayerData.Crystal >= crystalNum )
        {
            CGird gird =  gDefine.gPlayerData.FindGridByItemId( mPieceItemId );
            if( gird?.mNum >= pieceNum)
            {
                gird.mNum -= pieceNum;
                gDefine.gPlayerData.Crystal -= crystalNum;
                mLearnNum++;
                return true;
            }
        }
        return false;
    }

    public void ReadData( CSkillAdd.eSkillAdd TypeClass,string [] StrArr)
    {
        mType = TypeClass;
        mName = StrArr[2];
        mDes = StrArr[3];
        mPieceItemId = int.Parse(StrArr[4]);
        mIconName = StrArr[5];
        mDesId = int.Parse(StrArr[6]);
        mNameId = int.Parse(StrArr[7]);

    }

}

public class CSkillAdd 
{
    CSkillAddData [] mDict = new CSkillAddData[(int)eSkillAdd.Count] ;
    public enum eSkillAdd
    {
        Kill, //处决
        StarBless, //流星祝福
        DSWordGirl_Flus,//一闪
         BackFlash,//后闪
         DSWordKill,//剑气斩
         DSWordAirFlush,//浮空
        QuantumMask, //量子罩
        GunMaster,//枪械专家
        FirePowerUp, //火力强化
        BloodSuck, //吸血
        Berserk, //狂战士
        LifeSpring, //生命之泉
        HealLight, //治愈之光
        LightChain,//闪电链
        LightBall,//闪电球
        Thunder, //落雷
        CoinMore,//金币收藏
        Poison, //毒性攻击
        XianJing, // 磁暴陷阱
        DamageMore, //额外伤害
        DeathFinger,//死亡宣告
        ElectricityArround,//环绕电磁
        CriticalAtk,//暴击
        BulletBack, // 子弹反弹
        GroundArrow,//地弩
        ForceAwaken,//元气护体
        BigLun_HalfKill,//截斩
        BigLun_Lighting,//聚能光线
        BigLun_Saw,//电锯
        BigLun_DoubleWave,//双向波
        BigLun_Cut,//切割
        Count,
    }

    public CSkillAddData Find(eSkillAdd EType)
    {
        return mDict[(int)EType];
    }

    public void Clear()
    {
        for(int i=0; i<mDict.Length; i++)
        {
            mDict[i].mLearnNum = 0;
            mDict[i].mChoosed = false;
            mDict[i].mShowNum = 0;
        }
    }

    public void Load()
    {
        for(int i=0; i<mDict.Length; i++)
        {
            //mDict[i] = new CSkillAddData();
            mDict[i].mType = (eSkillAdd) i;

            mDict[i].mLearnNum = PlayerPrefs.GetInt( "SkillAdd_" + i.ToString() + "mLearnNum", 0);
            mDict[i].mChoosed = PlayerPrefs.GetInt( "SkillAdd_" + i.ToString() + "mChoosed", 0)==0?false:true;
            mDict[i].mShowNum = PlayerPrefs.GetInt( "SkillAdd_" + i.ToString() + "mShowNum", 0);
        }
    }

    public void Save()
    {
        for(int i=0; i<mDict.Length; i++)
        {
            PlayerPrefs.SetInt( "SkillAdd_" + i.ToString() + "mLearnNum",  mDict[i].mLearnNum);
            PlayerPrefs.SetInt( "SkillAdd_" + i.ToString() + "mChoosed",  mDict[i].mChoosed?1:0);
            PlayerPrefs.SetInt( "SkillAdd_" + i.ToString() + "mShowNum",  mDict[i].mShowNum);
        }
    }

    public void ReadData(string Str)
    {
        string str = Str;
        string [] sepStr = new string[]{"\r\n"};
        string [] sepStr1 = new string[]{"\t"};
        string [] itArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for(int i=0; i<itArr.Length; i++)
        {
            string [] valueArr = itArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);

            CSkillAdd.eSkillAdd typeClass = (CSkillAdd.eSkillAdd)Enum.Parse(typeof(CSkillAdd.eSkillAdd), valueArr[1]);
            
            if( mDict[(int)typeClass] == null )
                mDict[(int)typeClass]  = new CSkillAddData();

            mDict[(int)typeClass].ReadData(typeClass ,valueArr);
        }

    }
}
