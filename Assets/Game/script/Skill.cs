using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CSkill
{
    public enum eSkill
    {
        Null,
        QuantumMask, //量子罩
        GunMaster,//枪械专家
        FirePowerUp,
        PetDamageUp,
        PetAtkUp,
        HpUpMin,
        BloodSuck,
        DamageUpSmall,
        DamageUpBig,
        Berserk = 10,
        BackFlash,
        Ice,
        LifeSpring,
        HealLight,
        LightChain,
        LightBall,
        Thunder,
        CoinMore,
        Poison,
        XianJing,
        DamageMore,
        DeathFinger,
        ElectricityArround,
        CriticalAtk,
        BulletBack,
        GroundArrow,
        ForceAwaken,
        //pc skill
        DSWordGirl_Flush,
        DSWordGirl_FlushSecond,
        BigSword_SwordRain,
        BigSword_KillAll,
        BigSword_BigLun,
        BigSword_SwordFlush,
        BigLun_HalfKill,
        BigLun_Lighting,
        BigLun_Saw,
        BigLun_DoubleWave,
        BigLun_Cut, //切割
        DSWordGirl_FlashKill, // 顺斩
        DSWordGirl_FlashKillSecond, // 顺斩2
        DSWordGirl_DownKill, //落斩
        DSWordKill, //剑气斩
        Count,
    }

    public eSkill mType;
    public string mName;
    public string mDes;
    public int mDesId;
    public string mIconName;
    public int mReUse;

    public float mT;
    public float mSkillT = 0;
    public int mParamInt;
    public float mParamFloat;

    public string GetDes()
    {
        if(mDesId>0)
            return gDefine.GetStr(mDesId);
        else
            return gDefine.GetStr(mDes);
    }

    void UpdateQuantumMask()
    {
        if (Time.time > mSkillT)
        {
           // gDefine.gPlayerData.mIgonrDamageT = Time.time;

            GameObject o = GameObject.Instantiate(gDefine.gData.mQuantumMaskSEPreb);

            Transform t = gDefine.GetPcRefMid();

            se_skill_QuantumMask script = o.GetComponent<se_skill_QuantumMask>();

            script.Init(t);

            mSkillT = Time.time + 20;
        }
    }

    void UpdateIce()
    {
        if (Time.time > mSkillT)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mIceSEPreb);

            Transform t = gDefine.GetPcRefMid();

            se_Skill_Ice script = o.GetComponent<se_Skill_Ice>();

            CNpcInst npc = gDefine.gNpc.FindWithR(gDefine.GetPCTrans().position.x, 8, CNpcInst.eNpcClass.OnGround);
            if (npc != null)
            {
                bool isRight = npc.GetPos().x > gDefine.GetPCTrans().position.x ? true : false;
                script.Init(gDefine.GetPcRefMid().transform.position,
                       isRight, (int)gDefine.gPlayerData.mDamage);
            }
            else

                script.Init(gDefine.GetPcRefMid().transform.position,
                gDefine.IsPcFaceRight(), (int)gDefine.gPlayerData.mDamage);

            mSkillT = Time.time + 30;
        }
    }

    void UpdateLifeSpring()
    {
        if (Time.time > mSkillT)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mLifeSpringSEPreb);

            Transform t = gDefine.GetPcRefMid();

            se_Skill_LifeSpring script = o.GetComponent<se_Skill_LifeSpring>();

            script.Init(gDefine.GetPcRefMid().transform);

            mSkillT = Time.time + 30;
        }
    }

    void UpdateSwordRain()
    {
        if (Time.time > mSkillT)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mSwordRainPreb);

            o.GetComponent<se_skillRain>().Init((int)gDefine.gPlayerData.mDamage);

            mSkillT = Time.time + 20;
        }
    }



    void UpdateHealLight()
    {
        if (Time.time > mSkillT)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mHealLightSEPreb);

            Transform t = gDefine.GetPcRefMid();

            se_Skill_LifeSpring script = o.GetComponent<se_Skill_LifeSpring>();

            script.Init(gDefine.GetPcRefMid().transform);

            mSkillT = Time.time + 30;
        }
    }

    void UpdateLightBall()
    {
        if (Time.time > mSkillT)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mLightBallSEPreb);

            Transform t = gDefine.GetPcRefMid();

            se_Skill_lightball script = o.GetComponent<se_Skill_lightball>();

            script.Init(gDefine.GetPcRefMid().transform.position);

            mSkillT = Time.time + 30;
        }
    }

    void UpdateThunder()
    {
        if (Time.time > mSkillT)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mThunderSEPreb);

            se_Skill_Thunder script = o.GetComponent<se_Skill_Thunder>();

            script.Init();

            mSkillT = Time.time + 20;
        }
    }

    void UpdateXianJing()
    {
        if (Time.time > mSkillT)
        {
            if( gDefine.PcThrowItem(eSkill.XianJing, 3) )
            {
                mSkillT = Time.time + 30;
            }
            else
            {
                mSkillT = Time.time + 10;
            }
        }
    }

    void UpdateGroundArrow()
    {
        if (Time.time > mSkillT)
        {
            if( gDefine.PcThrowItem(eSkill.GroundArrow, 1) )
            {
                mSkillT = Time.time + 30;
            }
            else
            {
                mSkillT = Time.time + 10;
            }
        }
    }


    public void Update()
    {
        switch (mType)
        {
            case CSkill.eSkill.QuantumMask:
                UpdateQuantumMask();
                break;

            case CSkill.eSkill.Ice:
                UpdateIce();
                break;

            case CSkill.eSkill.LifeSpring:
                UpdateLifeSpring();
                break;

            case CSkill.eSkill.HealLight:
                UpdateHealLight();
                break;

            case CSkill.eSkill.LightBall:
                UpdateLightBall();
                break;
            case CSkill.eSkill.Thunder:
                UpdateThunder();
                break;

            case CSkill.eSkill.XianJing:
                UpdateXianJing();
                break;

            case CSkill.eSkill.GroundArrow:
                UpdateGroundArrow();
                break;

            case CSkill.eSkill.BigSword_SwordRain:
                UpdateSwordRain();
                break;

        }
    }
}

public class CSkillInFight
{
    CSkill[] mSkillArr = new CSkill[(int)CSkill.eSkill.Count];
    int[] mUsedArr = new int[(int)CSkill.eSkill.Count];

    public List<CSkill> mChooseThree = new List<CSkill>();


    public void Clear()
    {
        for (int i = 0; i < mUsedArr.Length; i++)
            mUsedArr[i] = 0;
    }

    public CSkill GetSkill(CSkill.eSkill SkillType)
    {
        return mSkillArr[(int)SkillType];
    }

    public CSkill GetSkillByName(string Name)
    {
        for(int i=0; i<mSkillArr.Length; i++)
        {
            if(mSkillArr[i]!=null && mSkillArr[i].mName == Name)
                return mSkillArr[i];
        }
        return null;
    }

    public List<CSkill> CalcChoose3()
    {
        List<CSkill> tmpDict = new List<CSkill>();
        // tmpDict.Add(mSkillArr[(int)CSkill.eSkill.DSWordGirl_FlashKill]);
        //tmpDict.Add(mSkillArr[(int)CSkill.eSkill.DSWordGirl_FlashKillSecond]);
        // tmpDict.Add(mSkillArr[ (int)CSkill.eSkill.DSWordGirl_DownKill ]);

        mChooseThree.Clear();

        if(PlayerPrefs.GetInt("first31",0)==0)
        {
            PlayerPrefs.SetInt("first31",1);
            mChooseThree.Add( mSkillArr[(int)CSkill.eSkill.LightChain]);
            mChooseThree.Add( mSkillArr[(int)CSkill.eSkill.LightBall]);
            mChooseThree.Add( mSkillArr[(int)CSkill.eSkill.Thunder]);
            return mChooseThree;
        }


        for (int i = 0; i < mSkillArr.Length; i++)
        {
            if (mSkillArr[i] == null)
                continue;

            if (mSkillArr[i].mReUse <= mUsedArr[i])
                continue;
            else
            {

                if (i == (int)CSkill.eSkill.BackFlash && gDefine.mCurUseGirlType != gDefine.eCharType.eDSword)
                    continue;

                if (i == (int)CSkill.eSkill.DSWordGirl_Flush && gDefine.mCurUseGirlType != gDefine.eCharType.eDSword)
                    continue;

                //if(i== (int) CSkill.eSkill.DSWordGirl_FlushSecond && gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_Flush) == null )
                //  continue;

                if (i == (int)CSkill.eSkill.DSWordGirl_FlashKillSecond && gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_FlashKill) == null)
                    continue;

                if (i == (int)CSkill.eSkill.DSWordGirl_DownKill && gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordGirl_FlashKill) == null)
                    continue;

                //if (i == (int)CSkill.eSkill.DSWordKill && gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DSWordKill) != null)
                  //  continue;

                  if (i == (int)CSkill.eSkill.DSWordKill && gDefine.mCurUseGirlType != gDefine.eCharType.eDSword)
                    continue;

                if (i >= (int)CSkill.eSkill.BigSword_KillAll && i <= (int)CSkill.eSkill.BigSword_SwordFlush && gDefine.mCurUseGirlType != gDefine.eCharType.eBigSword)
                    continue;

                if (i >= (int)CSkill.eSkill.BigLun_HalfKill && i <= (int)CSkill.eSkill.BigLun_Cut && gDefine.mCurUseGirlType != gDefine.eCharType.eLun)
                    continue;

                 if (i == (int)CSkill.eSkill.DSWordGirl_FlashKill  && gDefine.mCurUseGirlType != gDefine.eCharType.eDSword)
                    continue;

                //if( i==(int)CSkill.eSkill.HpUpMin &&(gDefine.gLogic.GetCurWaveIndex()!=5&&
                //gDefine.gLogic.GetCurWaveIndex()!=10))
                //    continue;
                if (i == (int)CSkill.eSkill.HpUpMin)
                    continue;

                if (i == (int)CSkill.eSkill.DamageUpSmall && (gDefine.gLogic.GetCurWaveIndex() != 5 &&
                gDefine.gLogic.GetCurWaveIndex() != 10))
                    continue;

                if (i == (int)CSkill.eSkill.DamageUpBig && (gDefine.gLogic.GetCurWaveIndex() != 5 &&
                gDefine.gLogic.GetCurWaveIndex() != 10))
                    continue;

                if (i == (int)CSkill.eSkill.BigLun_Lighting)
                    continue;



                tmpDict.Add(mSkillArr[i]);
            }
        }


        for (int i = 0; i < 3; i++)
        {
            if (mChooseThree.Count >= 3)
                break;

            int index = UnityEngine.Random.Range(0, tmpDict.Count);
            mChooseThree.Add(tmpDict[index]);
            tmpDict.RemoveAt(index);
            // if (mSkillArr[(int)tmpDict[index].mType].mReUse <= mUsedArr[(int)tmpDict[index].mType])
            // {
            //     tmpDict.RemoveAt(index);
            // }
        }

        return mChooseThree;
    }

    public void Choose(CSkill Skill)
    {
        mUsedArr[(int)Skill.mType] += 1;
    }

    public void LoadFromXml()
    {
        string str = gDefine.gData.mSkillData.text;
        XMLParser parser = new XMLParser();
        XMLNode root = parser.Parse(str);
        XMLNodeList nodeList = root.GetNodeList("Root>0>Node");
        foreach (XMLNode n in nodeList)
        {
            CSkill s = new CSkill();
            string Id = n.GetValue("@Id");
            s.mType = (CSkill.eSkill)Enum.Parse(typeof(CSkill.eSkill), Id);
            s.mName = n.GetValue("@name");
            s.mDes = n.GetValue("@des");
            s.mReUse = int.Parse(n.GetValue("@reuse"));
            s.mDesId = int.Parse(n.GetValue("@desId"));
            
            
            mSkillArr[(int)s.mType] = s;

        }
    }

}
