
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpcInst
{
    public enum eState
    {
        Idle = 0,
        MoveToTarget,
        Atk,
        Dying,//---kick off
        Died,
        BeAtk,
        Dying2,
        DyingFlash,
        Dying2KickFly, //有限的带减速的击飞
        BeKilled,
        PreAtk, //攻击前奏，2s钟后将开始攻击
        RandomMove, //游荡
        Skill, //技能状态
        KickDown, //击倒
        Up,//站起来
        WaitInTeamRandomMove, //战斗队列中的等待
        AirFallDown, //空中坠落模式
    }
    public NpcMono mNpc;


    public enum eNpcClass
    {
        Ground = 0, //地面怪
        Air,    //空中怪
        All,    //全部怪
        OnGround, //落地怪（空中怪降落地面的时候算落地怪）
        Air0,  //空中，并保持在空0高度
    }

    public eState mCurState = eState.Idle; //当前状态

    bool mImmunity = false; //只有盾牌会开启

    float mIdleT = 0; //发呆时间
    public bool mIsBaBa = false; //当前是否为霸体状态

    public float mComAtkIgnorPerc = 0; // 伤害的减免

    //--击飞--
    Vector3 mKickBackPos;
    Vector3 mDying2V = new Vector3(1, 3, 0); //   抛起速度
    float mDying2Acc = -2; // 引力
    //static float mDying2Y = 0;  //地平线高度
    float mDying2T = 0; // 当前的死亡闪烁时间
    const float mDying2FlashT = 0.4f; //死亡闪烁间隔时间

    //--dying2 kick fly-----
    float mDying2KickFlyV = 1;
    float mDying2KickFlyAcc = 1;

    //--npc属性---
    public int mHp = 4000;
    public int mMaxHp = 4000;
    public int mDamage = 3000;

    //--dying flash--
    int mDyingFlashNum = 0;
    float mDyingFlashT = 0;

    //--原地死亡--
    public float mDyingT = 0;

    public bool mIsPosion = false;

    float mLastAtkT = 0; //上次攻击时间

    public DamageShowCtrlInCreature mDamageShow = new DamageShowCtrlInCreature();

    protected float mFronzeT = 0;

    public bool mFaceIsControlByAI = false;

    public npcdata.eNpcType mNpcType;

    //攻击提示，特效锁定相关
    CNpcAtkTip mAtkTip = new CNpcAtkTip();

    protected CNpcBuff mBuff = new CNpcBuff();

    bool mIsBeKickBack = false;

    public bool mCanBeSelect = true;

    //游荡目标点
    protected Vector3 mRandomMoveTarget;
    float mRandomMoveT;

    //AI
    public NpcAICom mIdleAI;
    public NpcAICom mSkillAI;
    public NpcAICom mAtkAI;
    public NpcAICom mBeAtkAI;
    public NpcAICom mDyingAI;
    public NpcAICom mDyingFlashAI;
    public NpcAICom mMoveToTargetAI;

    public bool mCanBeCount = true;

    //半身特殊，死亡延迟
    float mHalfDelayDieT = 0;

    //被擒拿
    bool mBeTaken = false;

    float mAirFallDownT = 0;
    public bool IsBeTaken()
    {
        return mBeTaken;
    }

    public virtual void ChangetoAirFallDown()
    {
        //追加特效，
        mCurState = eState.AirFallDown;
        mNpc.Play("AirFallDown");
        mAirFallDownT = Random.Range(0.1f, 0.8f) + Time.time;
        DestorySELockObj();
    }

    public bool KickDown(bool IsRight, float L)
    {
        if (IsLive() && mCurState != eState.KickDown)
        {
            mCurState = eState.KickDown;
            //mKickBackPos = mNpc.gameObject.transform.position + ((IsRight) ? Vector3.right * L : Vector3.left * L);

            Transform tPc = gDefine.GetPCTrans();

            if (GetPos().x < tPc.position.x)
            {
                if (Random.Range(0, 100) < 50)
                    mDying2V = new Vector3(-7, Random.Range(20.0f, 30.0f), 0); //   抛起速度
                else
                    mDying2V = new Vector3(-12, Random.Range(25.0f, 35.0f), 0); //   抛起速度
            }
            else
            {
                if (Random.Range(0, 100) < 50)
                    mDying2V = new Vector3(7, 20, 0); //   抛起速度
                else
                    mDying2V = new Vector3(12, 25, 0); //   抛起速度
            }

            mDying2Acc = -60;


            mNpc.Play("kickfly");

            return true;
        }
        else
            return false;
    }

    public bool CanBeCount()
    {
        return mCanBeCount;
    }

    public bool CanBeEndKill()
    {
        if (mNpcType == npcdata.eNpcType.BareHand)
            return true;
        else
        {
            return false;
        }
    }

    public void BeEndKill0()
    {
        mHp = 0;
        mCurState = eState.Dying;
        mNpc.Play("die1");
    }

    public void BeTaken()
    {
        if (!mBeTaken)
        {
            mBeTaken = true;
            mNpc.Play("beTake");
        }
    }

    public void BeEndKill1()
    {
        mHp = 0;
        mCurState = eState.Dying;
        mNpc.Play("dieByEndKill");
    }

    public bool IsNpcClass(eNpcClass NpcClass)
    {
        if ((mNpcType == npcdata.eNpcType.HalfUp || mNpcType == npcdata.eNpcType.HalfDown)
        && gDefine.mCurUseGirlType == gDefine.eCharType.eDSword)
            return false;

        switch (NpcClass)
        {
            case eNpcClass.Air:
                return mNpc.mIsInAir;
            case eNpcClass.All:
                return true;
            case eNpcClass.Ground:
                return !mNpc.mIsInAir;
            case eNpcClass.OnGround:
                return IsOnGround();
            case eNpcClass.Air0:
                return IsOnAir0();
        }
        return false;
    }

    bool IsOnAir0()
    {
        if (Mathf.Abs(GetPos().y - gDefine.gAirY) < 1.5f)
            return true;
        else
            return false;
    }

    protected virtual bool IsOnGround()
    {
        if (Mathf.Abs(GetPos().y - gDefine.gGrounY) < 1.8f)
            return true;
        else
            return false;
    }

    public bool CanBeSelect()
    {
        return mCanBeSelect&&mNpc!=null;
    }

    public void DoSkillEvent(int CustomEventValue)
    {
        switch (mCurState)
        {
            case eState.Idle:
                mIdleAI?.DoSkillEvent(CustomEventValue, this);
                break;
            case eState.Atk:
                mAtkAI?.DoSkillEvent(CustomEventValue, this);
                break;
            case eState.Skill:
                mSkillAI?.DoSkillEvent(CustomEventValue, this);
                break;
            case eState.BeAtk:
                mBeAtkAI?.DoSkillEvent(CustomEventValue, this);
                break;
            case eState.Dying:
                mDyingAI?.DoSkillEvent(CustomEventValue, this);
                break;
        }
        //if (mCurState == eState.Skill && mSkillAI != null)
        //  mSkillAI.DoSkillEvent(CustomEventValue, this);
    }


    public virtual void AddBuff(CBuff.eBuff BuffType, float LastT)
    {
        mBuff.AddBuff(this, BuffType, LastT);
    }

    /// <summary>
    /// 当前是否朝右侧方向
    /// </summary>
    /// <returns></returns>
    bool IsFaceRight()
    {
        return GetPos().x < gDefine.GetPCTrans().position.x ? true : false;
    }
    /// <summary>
    /// 开启霸体状态
    /// </summary>
    public void BeginBABA()
    {
        mIsBaBa = true;
    }

    public Vector3 GetThrowPos()
    {
        return mAtkTip.GetPos();
    }

    public void CloseAtkTip()
    {
        mAtkTip.EndLockTip();
    }

    public void CloseBaBa()
    {
        mIsBaBa = false;
    }

    public float GetMaxHp()
    {
        return mMaxHp;
    }

    public int GetDamage()
    {
        return mDamage;
    }
    public GameObject GetRefMid()
    {
        return mNpc.mRefMidPoint;
    }

    public virtual void Fronze(float FronzeT)
    {
        mFronzeT = Time.time + FronzeT;
    }

    public virtual bool CanBeInAirTeam()
    {
        return true;
    }

    public virtual void Event_AirNpcAtk()
    {

    }

    public void UnFronze()
    {
        mFronzeT = 0;
    }

    public Vector3 GetHitSEPos()
    {
        return mNpc.mRefMidPoint.transform.position;
    }

    public Vector3 GetPos()
    {
        if (mNpc != null)
        {
            return mNpc.gameObject.transform.position;
        }

        else
            return Vector3.zero;
    }

    public virtual bool IsDiedState()
    {
        return mCurState == eState.Died;
    }

    public void CalcDrop()
    {
        CDropReturnData[] data = gDefine.gDropSystem.CalcDrop(gDefine.gChapterId, gDefine.gNpcData.GetNpcClass(mNpcType));
        //  Debug.Log("---Drop:" + data.Length.ToString());
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].mItemId == 201)
            {
                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.CoinMore);
                if (d != null && d.mLearnNum > 0)
                {
                    if (Random.Range(0, 100) < d.mLearnNum * 10)
                        data[i].mItemNum *= 2;
                }
                gDefine.gGainInFight.AddCoins(data[i].mItemNum, this.mNpcType.ToString() );
            }
            else if( data[i].mItemId == 202)
            {
                gDefine.gGainInFight.AddCrystals(data[i].mItemNum, this.mNpcType.ToString() );
            }

            for (int j = 0; j < data[i].mItemNum; j++)
            {
                //Debug.Log("---Drop:Create" );
                gDefine.CreateDrop(data[i].mItemId, this);

                if (data[i].mItemId == 201)
                    gDefine.gPlayerData.AddCoin(1);
                else if (data[i].mItemId == 202)
                    gDefine.gPlayerData.AddCrystal(1);
                else
                    gDefine.gPlayerData.AddItemToBagQuick(data[i].mItemId, 1);
            }
        }

        if (mNpc.mIsBoss)
            gDefine.PlaySound(86);


    }

    public bool CanKilledPlay()
    {
        int r = Random.Range(0, 100);
        return r < 30 ? true : false;
    }

    public bool IsLive()
    {
        return mHp > 0;
    }

    public void ChangeToStateIdle(float IdleT = 1)
    {
        if (IsLive())
        {
            mCurState = eState.Idle;
           if(gDefine.gLogic.mTeach.mIsInTeach &&  mNpc.gameObject.name  == "caidao")
                mIdleT = 2;
            //mIdleT = IdleT;
        }
    }

    public void DestorySelfInst()
    {
        if (mNpc != null)
        {
            mNpc.Destory();
            mNpc = null;
        }

        mAtkTip.EndLockTip();

    }

    public virtual void Init(GameObject Obj, npcdata.eNpcType NpcType, Vector3 Pos, int WaveLvL)
    {
        mNpc = Obj.GetComponent<NpcMono>();

        mNpc.SetNpcInst(this);

        if (NpcType == npcdata.eNpcType.HalfDown || NpcType == npcdata.eNpcType.HalfUp)
        {
            mMaxHp = mHp = 1;
            mDamage = 1;

            //if (NpcType == npcdata.eNpcType.HalfUp)
            {
                int r = Random.Range(0, 100);

                //if (gDefine.gNpc.GetLiveNpcNumByType(npcdata.eNpcType.BareHand) > 3)
                {
                    // if (r < 60)
                    // {
                    //     mNpc.Play("die2");
                    //     mCurState = eState.Dying;
                    //     mHp = 0;
                    // }
                    // else 
                    if (NpcType == npcdata.eNpcType.HalfDown)
                    {
                        mHalfDelayDieT = 5.0f;
                    }
                }
            }
        }
        else
        {
            mMaxHp = mHp = gDefine.gNpcData.GetNpcMaxHp(NpcType, WaveLvL, gDefine.gChapterId);
            mDamage = gDefine.gNpcData.GetNpcDamage(NpcType, WaveLvL, gDefine.gChapterId);

            if(gDefine.gLogic.mTeach.mIsInTeach && NpcType == npcdata.eNpcType.CaiDao)
            {
                 mMaxHp = mHp = (int)(mHp * 2f);
            }
            else
            if (gDefine.gChapterDifficult == 1)
            {
                mMaxHp = mHp = (int)(mHp * 2.5f);
                mDamage = (int)(mDamage * 1.4f);
            }
            else if (gDefine.gChapterDifficult == 2)
            {
                mMaxHp = mHp = (int)(mHp * 4);
                mDamage = (int)(mDamage * 2f);
            }
            else if (gDefine.gChapterDifficult == 3)
            {
                mHp = gDefine.gNpcData.GetNpcMaxHp(NpcType, WaveLvL, 10);
                mDamage = gDefine.gNpcData.GetNpcDamage(NpcType, WaveLvL, 10);
                mMaxHp = mHp = (int)(mHp * 2);
                mDamage = (int)(mDamage * 1.5f);
            }

        }

        //if (NpcType == npcdata.eNpcType.RagePursuer)
        // mMaxHp = mHp = 3000;
        if (gDefine.gChapterId == 1 && mNpc.mIsBoss)
            mMaxHp = mHp = (int)(mHp * 2.3f);

        mNpcType = NpcType;

        mNpc.Refresh(mHp, mMaxHp);


        mNpc.SetPos(Pos);

        if (mNpc.mIsGroundTeam)
        {
            gDefine.gAtkNpcGlobleTeam.Register(this);
            ChangeToWaitInTeamRandomMove();
        }
        else if (mNpc.mIsAirTeam)
        {
            // gDefine.gAirNpcGlobleTeam.Register(this);
        }
        else
        {
            mCurState = eState.MoveToTarget;
            mNpc.Play("move", true);
        }

        if (NpcType == npcdata.eNpcType.Shield)
            mImmunity = true;


    }

    /// <summary>
    /// 检查并看是否能够进入被打状态
    /// 目前被击飞0.4
    /// </summary>
    public void CheckToBeAtkState(bool KickOff)
    {
        //如果没有别的情况，其实就是霸体，那么就进入被打状态
        if (!mIsBaBa)
        {
            mCurState = eState.BeAtk;

            mNpc.Play("beAtk");
        }

        //被向后打飞一点点距离
        Vector3 deltPos = mNpc.gameObject.transform.position - gDefine.GetPCTrans().position;
        deltPos.y = deltPos.z = 0;
        if (KickOff && !mNpc.mIsBoss && mNpc.mCanKickBackAndKickFly)
        {
            //deltPos.x = deltPos.x > 0 ? 0.4f : -0.4f;
            deltPos.x = deltPos.x > 0 ? 1f : -1f;
            mKickBackPos = mNpc.gameObject.transform.position + deltPos;
            mIsBeKickBack = true;
            // float l = Random.Range(2.0f, 4.0f);
            // deltPos.x = deltPos.x > 0 ? l : -l;
        }
        else
        {
            mKickBackPos = mNpc.gameObject.transform.position;
        }



    }

    // public void CheckToDyingState()
    // {
    //     //直接被打飞
    //     {
    //         mCurState = eState.Dying;
    //         mCurAct = CNpcDefine.eNpcActType.Die;
    //         mCurActT = 0;
    //         mFrameIndex = 0;
    //         mIdleT = 0;
    //         mNextActUpdateT = 0;

    //         //被向后打飞一点点距离
    //         Vector3 deltPos = mNpc.gameObject.transform.position - gDefine.GetPCTrans().position;
    //         deltPos.y = deltPos.z = 0;
    //         deltPos.x = deltPos.x > 0 ? 50 : -50;
    //         mKickBackPos = mNpc.gameObject.transform.position + deltPos;
    //     }

    // }

    public void CheckToDying2State()
    {
        // Vibration.VibratePop ();

        mBuff.Close();

        mFronzeT = 0;

        mNpc.Continue();

        //直接被打飞
        mCurState = eState.Dying2;
        Transform tPc = gDefine.GetPCTrans();

        if (GetPos().x < tPc.position.x)
        {
            if (Random.Range(0, 100) < 50)
                mDying2V = new Vector3(-7, 20, 0); //   抛起速度
            else
                mDying2V = new Vector3(-12, 25, 0); //   抛起速度
        }
        else
        {
            if (Random.Range(0, 100) < 50)
                mDying2V = new Vector3(7, 20, 0); //   抛起速度
            else
                mDying2V = new Vector3(12, 25, 0); //   抛起速度
        }

        mDying2Acc = -60;

        mNpc.Play("kickfly");
    }

    public void CheckToKilledPlay()
    {
        // mHp = 0;
        // mCurAct = CNpcDefine.eNpcActType.BeKilled;
        // mCurActT = 0;
        // mNextActUpdateT = 0;
        // mCurState = eState.BeKilled;

        // mNpc.Close();
    }

    // protected void GainMoney()
    // {
    //     int moneyAdd = 1;
    //     npcdata.eNpcClass nclass = gDefine.gNpcData.GetNpcClass(mNpcType);
    //     if (mNpcType == npcdata.eNpcType.GoldCao || nclass == npcdata.eNpcClass.BigBoss ||
    //      nclass == npcdata.eNpcClass.Boss)
    //         moneyAdd = Random.Range(20, 51);
    //     else if (nclass == npcdata.eNpcClass.Elite)
    //         moneyAdd = 2;

    //     CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.CoinMore);
    //     if (d != null && d.mLearnNum > 0)
    //     {
    //         if (Random.Range(0, 100) < d.mLearnNum * 10)
    //             moneyAdd *= 2;
    //     }

    //     gDefine.gPlayerData.Coin += moneyAdd;
    // }

    public virtual void BeDamage(int Damage, bool IsHeavy, bool IsKickOff, bool isSkill, bool IsBigGun = false
    , CSkill.eSkill SkillType = CSkill.eSkill.Null)
    {
        if (!IsLive())
            return;

        //  if(gDefine.gChapterId==1 && mNpcType == npcdata.eNpcType.RagePursuer)
        //  {
        //      int kkk = 1;
        //  }

        if (mImmunity)
        {
            if (IsBigGun)
                mImmunity = false;
            else
            {
                //毒性攻击
                if (!mIsPosion && Random.Range(0, 100) < 30)
                {
                    CSkill s = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.Poison);
                    if (s != null)
                    {
                        GameObject o = GameObject.Instantiate(gDefine.gData.mPosionSEPreb);
                        se_Skill_Posion script = o.GetComponent<se_Skill_Posion>();
                        script.Init(GetRefMid().transform, this);
                        mIsPosion = true;
                    }
                }

                if (SkillType != CSkill.eSkill.LightBall && SkillType != CSkill.eSkill.LightChain
                && SkillType != CSkill.eSkill.Thunder && SkillType != CSkill.eSkill.DSWordGirl_FlushSecond)
                {
                    Vector3 deltPos = mNpc.gameObject.transform.position - gDefine.GetPCTrans().position;
                    deltPos.y = deltPos.z = 0;
                    if (!mNpc.mIsBoss)
                    {
                        deltPos.x = deltPos.x > 0 ? 1f : -1f;
                        mKickBackPos = mNpc.gameObject.transform.position + deltPos;
                        mIsBeKickBack = true;
                    }
                    return;
                }

            }
        }

        //if (gird != null && gird.mRefItem.mSpecialIndex == 1)
        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon];
        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 1
        && gird.mRefItem.mSubType == CItem.eSubType.Shield)
        {
            if (mComAtkIgnorPerc > 0 && Random.Range(0, 100) < 50)
            {
                mComAtkIgnorPerc = 0;
                // Vector3 pos0 = Arr[i].mNpc.GetDamageShowPos();
                // pos0.z -= 0.5f;
                // mDamageShow.Add("破甲", pos0, Color.gray);
            }
        }

        if (mBeTaken)
            return;

        if ((mNpcType == npcdata.eNpcType.HalfDown || mNpcType == npcdata.eNpcType.HalfUp) && gDefine.PcIsSword()
        && !isSkill)
            return;
        // if (mNpcType == npcdata.eNpcType.HalfDown)
        // {
        //     int kk = 1;
        // }

        if (IsBigGun)
            mNpc.BeAtk();

        if (gDefine.gPlayerData.mCurMagicData != null &&
      gDefine.gPlayerData.mCurMagicData.mType == CMagic.eMagic.Luck &&
      gDefine.gPlayerData.mCurMagicData.mNum > 0 && Time.time > gDefine.gPlayerData.mCurMagicData.mCoolDownT)
        {
            gDefine.gPlayerData.mCurMagicData.mCoolDownT = 0.5f + Time.time;

            if (Random.Range(0, 100) < 50)
            {
                gDefine.gPlayerData.Coin += 30;
                gDefine.gGainInFight.AddCoins(30, "Magic");
                gDefine.CreateSomeCoinInGame(10);
            }
            else
            {
                gDefine.gPlayerData.Crystal += 30;
                gDefine.CreateSomeCrystalInGame(10);
                gDefine.gGainInFight.AddCrystals(30, "Magic");
            }
            gDefine.gPlayerData.mCurMagicData.mNum--;
        }

        gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon];
        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 1
        && gird.mRefItem.mSubType == CItem.eSubType.DSword && !mNpc.mIsBoss)
        {
            if (Random.Range(0, 100) < 5)
                Damage = 9999;
        }

        // if (mCurState == eState.AirFallDown && Mathf.Abs(GetPos().y - gDefine.gGrounY) < 0.01f
        //     && !mNpc.mIsBoss )
        // {
        //     Damage = 9999;
        // }

        CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DeathFinger);
        if (!mNpc.mIsBoss && (skill != null && Time.time > skill.mParamFloat + 1))
        {
            float prec = 10;
            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.DeathFinger);
            if (d != null)
                prec += 10 * d.mLearnNum;
            if (Random.Range(0, 100) < prec)
            {
                if (mHp > 0)
                {
                    CalcDrop();

                    mHp = 0;

                    skill.mParamFloat = Time.time;
                    if (!mNpc.mIsInAir)
                    {
                        Vector3 _pos = GetPos();
                        _pos.y = gDefine.gGrounY;
                        mNpc.SetPos(_pos);
                    }


                    //立刻死亡
                    if ((Random.Range(0, 100) < 34 && !mNpc.mIsBoss && !mNpc.mIsInAir && mNpc.mCanKickBackAndKickFly)
                        || (mCurState == eState.KickDown))
                    {
                        CheckToDying2State();
                    }
                    else
                    {
                        CheckToDieState(IsKickOff, isSkill);
                    }

                    mNpc.CloseUI();



                    Transform t = gDefine.GetPCTrans();

                    //显示技能名称
                    gDefine.gDamageShow.CreateDamageShow(skill.mName, t.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

                    mFronzeT = 0;

                    mNpc.Continue();

                    //衣服提供的生命上限
                    gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Clothe];
                    if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 2 &&
                    gDefine.gPlayerData.mClotheMaxHpCount < 14)
                    {
                        gDefine.gPlayerData.mClotheMaxHpCount++;
                        int addhpmax = (int)(0.01f * gDefine.gPlayerData.mHpMax);
                        if (addhpmax < 1) addhpmax = 1;
                        gDefine.gPlayerData.mHpMax += addhpmax;
                    }

                }

                mAtkTip.EndLockTip();

                DestorySELockObj();

                mBuff.Close();

                return;
            }
        }

        // if (!IsHeavy && gDefine.gPlayerData.mDieNpcCount == 5)
        // {
        //     gDefine.gPlayerData.mDieNpcCount = 0;
        //     IsHeavy = true;
        // }


        if (gDefine.gPlayerData.mCurMagicData != null &&
        gDefine.gPlayerData.mCurMagicData.mType == CMagic.eMagic.Critic &&
        gDefine.gPlayerData.mCurMagicData.mNum > 0)
        {
            if (!IsHeavy)
            {
                Damage *= 2;
                IsHeavy = true;
            }
            gDefine.gPlayerData.mCurMagicData.mNum--;
        }
        else
        if (!IsHeavy)
        {
            if (Random.Range(0, 100) < gDefine.gPlayerData.mDoubleDamagePerc)
            {
                Damage *= 2;
                IsHeavy = true;
            }
        }

        if (IsHeavy && gDefine.gPlayerData.mHeavyAtkUpCount < 5)
        {
            int damageAdd = (int)(gDefine.gPlayerData.mDamage * 0.01f);
            if (damageAdd < 1)
                damageAdd = 1;
            gDefine.gPlayerData.mDamage += damageAdd;
            gDefine.gPlayerData.mHeavyAtkUpCount++;
        }

        if (!mIsPosion && Random.Range(0, 100) < 30)
        {
            CSkill s = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.Poison);
            if (s != null)
            {
                GameObject o = GameObject.Instantiate(gDefine.gData.mPosionSEPreb);
                se_Skill_Posion script = o.GetComponent<se_Skill_Posion>();
                script.Init(GetRefMid().transform, this);
                mIsPosion = true;
            }
        }

        if (!isSkill)
        {
            Damage = (int)(Damage * (1.0f - mComAtkIgnorPerc));
            if (Damage <= 0) Damage = 1;
            Vector3 pos0 = mNpc.GetDamageShowPos();
            pos0.z -= 0.5f;
            //if()
            //mDamageShow.Add("物理攻击抵抗",pos0, Color.gray);
        }

        //mHp =  mHp-Damage < 0 ?  (mIsBaBa?1:0):mHp-Damage;


        mHp = mHp - Damage < 0 ? 0 : mHp - Damage;
        mNpc.Refresh(mHp, mMaxHp);

        if (mNpc.mRefFlash != null)
            mNpc.mRefFlash.BeginFlash();

        Vector3 pos = mNpc.GetDamageShowPos();
        pos.z -= 0.5f;

        //gDefine.gDamageShow.CreateDamageShow(Damage, pos, Color.white, IsHeavy);
        mDamageShow.Add(Damage, pos, Color.white, IsHeavy);

        if (mHp > 0)
        {
            //被打状态
            if (mCurState != eState.KickDown)
                CheckToBeAtkState(IsKickOff);
        }
        else
        {
            CalcDrop();
            //gDefine.gPlayerData.GainMoney();
            //GainMoney();

            if (mNpc.mDiedPartArr != null && mNpc.mDiedPartArr.Length > 0 && Random.Range(0, 60) < 50)
            {
                mCurState = eState.Died;
                mNpc.Continue();
                mNpc.CreateDiedPart();
                DestorySelfInst();
                mNpc = null;
            }
            else if (mNpc.mAirDropDie != null && !IsOnGround() && Random.Range(0, 100) < 50)
            {
                mCurState = eState.Died;
                mNpc.Continue();
                mNpc.mAirDropDie.GoDie1();
                mNpc = null;
            }
            else
            {
                //死亡状态
                if ((Random.Range(0, 100) < 40 && !mNpc.mIsBoss && !mNpc.mIsInAir && mNpc.mCanKickBackAndKickFly)
                    || (mCurState == eState.KickDown))
                {
                    CheckToDying2State();
                }
                else
                {
                    CheckToDieState(IsKickOff, isSkill);
                }
                mNpc.Continue();

            }

            // if( mNpc.mHasHalfDie)
            // {
            //      CheckToDieState();
            // }
            // else



            //mNpc.CloseUI();

            gDefine.gFollowCam.PlayVibrate();

            mFronzeT = 0;

            mAtkTip.EndLockTip();

            mBuff.Close();

            DestorySELockObj();

            //衣服提供的生命上限
            gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Clothe];
            if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 2 &&
            gDefine.gPlayerData.mClotheMaxHpCount < 14)
            {
                gDefine.gPlayerData.mClotheMaxHpCount++;
                int addhpmax = (int)(0.01f * gDefine.gPlayerData.mHpMax);
                if (addhpmax < 1) addhpmax = 1;
                gDefine.gPlayerData.mHpMax += addhpmax;
            }

            //Handheld.Vibrate();
        }
    }
    /// <summary>
    /// 被直接秒杀
    /// </summary>
    public virtual void BeKilled(string Tip = null)
    {
        if (mBeTaken)
            return;

        if (mHp > 0 && mNpcType != npcdata.eNpcType.HalfUp &&
        mNpcType != npcdata.eNpcType.HalfDown)
        {
            //GainMoney();
            CalcDrop();

            Vector3 pos = mNpc.GetDamageShowPos();

            mDamageShow.Add(mHp, pos, Color.white, false);

            mHp = 0;

            mFronzeT = 0;

            mNpc.Continue();

            CheckToDieState(false, true);

            mNpc.CloseUI();

            gDefine.PlayVibrate();

            mAtkTip.EndLockTip();

            DestorySELockObj();

            Vector3 damagePos = mNpc.GetDamageShowPos();
            //pos.z -= 0.5f;

            //gDefine.gDamageShow.CreateDamageShow(Damage, pos, Color.white, IsHeavy);
            if (!string.IsNullOrEmpty(Tip))
                mDamageShow.Add(Tip, damagePos, Color.white);


            //衣服提供的生命上限
            CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Clothe];
            if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 2 &&
            gDefine.gPlayerData.mClotheMaxHpCount < 14)
            {
                gDefine.gPlayerData.mClotheMaxHpCount++;
                int addhpmax = (int)(0.01f * gDefine.gPlayerData.mHpMax);
                if (addhpmax < 1) addhpmax = 1;
                gDefine.gPlayerData.mHpMax += addhpmax;
            }

        }

        //gDefine.gPlayerData.GainMoney();
        //死亡状态



    }

    public virtual void DestorySELockObj()
    {

    }

    void CheckToDieState(bool KickBack, bool IsSkill)
    {
        // Vibration.VibratePop();

        mFronzeT = 0;
        mNpc.Continue();
        mBuff.Close();

        mCurState = eState.Dying;
        mDyingT = 0;
        //int dieIndex = Random.Range(0, mNpc.mDieNum);
        //if (dieIndex == 0)
        if (mNpc.mHasHalfDie && Random.Range(0, 100) < 50 && !gDefine.gLogic.mTeach.mIsInTeach)
            mNpc.Play("diehalf");
        else
        {
            if (mNpc.mDieNum >= 2 && Random.Range(0, 100) < 60)
                mNpc.Play("die1");
            else
                mNpc.Play("die");
        }
        //mNpc.Play("die");

        if (KickBack && !IsSkill && !mNpc.mIsBoss)
        {
            //被向后打飞一点点距离
            Vector3 deltPos = mNpc.gameObject.transform.position - gDefine.GetPCTrans().position;
            deltPos.y = deltPos.z = 0;
            //if (KickOff && !mNpc.mIsBoss)
            {
                //deltPos.x = deltPos.x > 0 ? 0.4f : -0.4f;
                deltPos.x = deltPos.x > 0 ? 1f : -1f;
                mKickBackPos = mNpc.gameObject.transform.position + deltPos;
                mIsBeKickBack = true;
                // float l = Random.Range(2.0f, 4.0f);
                // deltPos.x = deltPos.x > 0 ? l : -l;
            }


        }


        //else
        //   mNpc.Play("die" + dieIndex.ToString());

    }

    public void CheckPos()
    {
        if (mNpcType == npcdata.eNpcType.AirFlane)
            return;

        Vector3 pos = GetPos();
        Vector3 pcPos = gDefine.GetPCTrans().position;
        if (pos.x < pcPos.x - 20)
        {
            pos.x = Random.Range(pcPos.x - 13.0f, pcPos.x - 11.0f);
            mNpc.SetPos(pos);
        }
        else if (pos.x > pcPos.x + 20)
        {
            pos.x = Random.Range(pcPos.x + 12.0f, pcPos.x + 15.0f);
            mNpc.SetPos(pos);
        }
    }

    public virtual void Update(float T)
    {

        CheckPos();


        // if (mHalfDelayDieT > 0)
        // {
        //     mHalfDelayDieT -= Time.deltaTime;
        //     if (mHalfDelayDieT <= 0)
        //     {
        //         mCurState = eState.Dying;
        //         //mNpc.Play("die");
        //         mHp = 0;
        //         mDyingT = 0;
        //         return;
        //     }
        // }

        if (mBeTaken)
        {
            if (!gDefine.IsPcInTakenState())
            {
                mBeTaken = false;
                mCurState = eState.Idle;
                mNpc.Play("idle");
            }

            if (mCurState == eState.Dying)
                UpdateDying(Time.deltaTime);

            if (mBeTaken)
                return;
        }



        mDamageShow.Update();

        mBuff.Update(this);

        if (mNpc == null)
            return;

        if (mIsBeKickBack)
            UpdateKickBack(T);

        if (mCurState == eState.KickDown)
        {
            UpdateKickDown(T);
        }

        if (Time.time < mFronzeT)
        {
            if (!mIsBaBa)
            {
                mNpc.Pause();
                return;
            }
            else
                mNpc.Continue();

        }

        mAtkTip.Update();

        if (!gDefine.gLogic.mTeach.mIsInTeach)
            mNpc.Continue();

        if (gDefine.gNpcIsWood)
            mCurState = eState.Idle;

        //走向玩家，等待攻击，如果可以攻击则攻击（被打状态除外，需要被打状态解除，一旦解除，则霸体）
        switch (mCurState)
        {
            case eState.Idle:
                if (mIdleAI != null)
                    mIdleAI.Update(T, this);
                else
                    UpdateIdle(T);
                break;
            case eState.MoveToTarget:
                if (mMoveToTargetAI != null)
                    mMoveToTargetAI.Update(T, this);
                else
                    UpdateMoveToTarget(T);
                break;
            case eState.Atk:
                break;
            case eState.Dying:
                if (mDyingAI != null)
                    mDyingAI.Update(T, this);
                else
                    UpdateDying(T);
                break;
            case eState.Died:
                return; //死亡直接返回

            case eState.BeAtk:
                if (mBeAtkAI != null)
                    mBeAtkAI.Update(T, this);
                //UpdateBeAtk(T);
                break;
            case eState.Dying2:
                UpdateDying2(T);
                break;
            case eState.DyingFlash:
                if (mDyingFlashAI != null)
                    mDyingFlashAI.Update(T, this);
                else
                    UpdateDyingFlash(T);
                break;
            case eState.BeKilled:
                break;
            case eState.PreAtk:
                UpdatePreAtk();
                break;
            case eState.RandomMove:
                UpdateRandomMove();
                break;
            case eState.Skill:
                mSkillAI?.Update(T, this);
                break;

            case eState.WaitInTeamRandomMove:
                UpdateWaitInTeamRandomMove();
                break;
            case eState.AirFallDown:
                UpdateAirFallDown();
                break;
        }

        if (IsLive() && !mFaceIsControlByAI)
            mNpc.FaceRight(GetPos().x < gDefine.GetPCTrans().position.x ? true : false);

    }

    public void ChangeToWaitInTeamRandomMove()
    {
        if (IsLive())
        {
            mCurState = eState.WaitInTeamRandomMove;
            mRandomMoveTarget.x = gDefine.gAtkNpcGlobleTeam.GetWaitX(GetPos().x > gDefine.GetPCTrans().position.x);
            mRandomMoveTarget.y = gDefine.gGrounY;
            mRandomMoveTarget.z = GetPos().z;
            mNpc.Play("move", false);
        }
    }

    private void UpdateAirFallDown()
    {
        if (Time.time > mAirFallDownT)
        {
            Vector3 pos = GetPos();

            pos.y -= 10 * Time.deltaTime;
            if (pos.y <= gDefine.gGrounY)
                pos.y = gDefine.gGrounY;

            mNpc.SetPos(pos);
        }

    }

    public void UpdateWaitInTeamRandomMove()
    {
        if (gDefine.gAtkNpcGlobleTeam.IsSelfInAtkTeam(this, GetPos().x > gDefine.GetPCTrans().position.x))
        {
            mCurState = eState.MoveToTarget;
        }
        else
        {
            float l = Mathf.Abs(mRandomMoveTarget.x - GetPos().x);
            if (Mathf.Abs(l) <= 0.001f)
            {
                ChangeToWaitInTeamRandomMove();
            }

            else
            {
                Vector3 endPos = Vector3.MoveTowards(GetPos(), mRandomMoveTarget, Time.deltaTime * mNpc.mV * 0.4f);

                mNpc.SetPos(endPos);
            }
        }
    }

    public void ChangeToRandomMoveState(float T = -1)
    {
        if (IsLive())
            mCurState = eState.RandomMove;
        Vector3 endpos = Random.Range(-2, 2) * Vector3.right + GetPos();
        Vector3 curpos = GetPos();
        Vector3 pcPos = gDefine.GetPCTrans().position;
        if (curpos.x > pcPos.x)
        {
            if (endpos.x < pcPos.x + mNpc.mMinAtkL)
                endpos.x = pcPos.x + mNpc.mMinAtkL;

        }
        else if (curpos.x < pcPos.x)
        {
            if (endpos.x > pcPos.x - mNpc.mMinAtkL)
                endpos.x = pcPos.x - mNpc.mMinAtkL;
        }

        mRandomMoveTarget = endpos;
        mRandomMoveT = (T < 0 ? Random.Range(1.5f, 3.5f) : T) + Time.time;
        mNpc.Play("move");

    }

    protected void UpdateRandomMove()
    {
        float l = Mathf.Abs(mRandomMoveTarget.x - GetPos().x);
        float lTpPc = Mathf.Abs(mRandomMoveTarget.x - gDefine.GetPCTrans().position.x);
        if (Mathf.Abs(l) <= 0.001f || lTpPc > 18.9f)
        {
            if (Time.time >= mRandomMoveT)
            {
                //转向追踪，攻击
                mCurState = eState.MoveToTarget;
                mNpc.Play("move");
                return;
            }
            else
            {
                ChangeToRandomMoveState(mRandomMoveT - Time.time);
            }
        }
        else
        {
            if (Mathf.Abs(mRandomMoveTarget.x - gDefine.GetPCTrans().position.x) < mNpc.mMinAtkL)
            {
                ChangeToRandomMoveState(mRandomMoveT - Time.time);
            }
            else
            {
                Vector3 endPos = Vector3.MoveTowards(GetPos(), mRandomMoveTarget, Time.deltaTime * mNpc.mV * 0.4f);

                mNpc.SetPos(endPos);
            }
        }
    }



    void UpdatePreAtk()
    {
        if (mAtkTip.IsReady())
        {
            mCurState = eState.Atk;
            mNpc.Play("atk");
            mAtkTip.ChangeToAtkLockState();
        }
    }

    public void UpdateDyingFlash(float t)
    {
        mDyingFlashT += t;
        if (mDyingFlashT > 0.8f)
        {
            mCurState = eState.Died;
            mNpc.Destory();
        }

    }

    public void UpdateIdle(float T)
    {
        if (gDefine.gNpcIsWood)
            return;

        if (mNpc.mIsGroundTeam && !gDefine.gAtkNpcGlobleTeam.IsSelfInAtkTeam(this, GetPos().x > gDefine.GetPCTrans().position.x))
        {
            ChangeToWaitInTeamRandomMove();
            return;
        }

        //计算时间流逝，时间到了，追踪或攻击玩家
        mIdleT -= T;
        if (mIdleT < 0)
        {
            Transform t = gDefine.GetPCTrans();
            float l = Mathf.Abs(t.position.x - GetPos().x);
            if (Mathf.Abs(l) <= mNpc.mAtkL && !gDefine.gIsInEndKill)
            {
                float atkV = mNpc.mAtkV;
                if( gDefine.gLogic.mTeach.mIsInTeach && this.mNpcType == npcdata.eNpcType.CaiDao)
                    atkV *= 4;
                if (Time.time > mLastAtkT + atkV)
                {
                    if (mNpc.mHasAtkTip)
                    {
                        mCurState = eState.PreAtk;
                        mAtkTip.BeginLockTip(this, mNpc.mShowAtkTipT, mNpc.mShowAtkTipSE);
                    }
                    else
                    {
                        mIsBaBa = true;
                        mCurState = eState.Atk;
                        mNpc.Play("atk");
                        mLastAtkT = Time.time;
                    }
                }
            }
            else
            {
                //转换到追踪状态
                mCurState = eState.MoveToTarget;
                mNpc.Play("move");
            }
        }
    }


    public void AtkPc()
    {
        if (IsLive())
        {
            Transform pc = gDefine.GetPCTrans();
            if (Mathf.Abs(pc.position.x - GetPos().x) < mNpc.mAtkL)
            {
                gDefine.PcBeAtk(mDamage, this);
            }
        }
    }



    public virtual void UpdateMoveToTarget(float T)
    {
        Transform t = gDefine.GetPCTrans();
        if (mNpc.mIsGroundTeam && !gDefine.gAtkNpcGlobleTeam.IsSelfInAtkTeam(this, GetPos().x > t.position.x))
        {
            ChangeToWaitInTeamRandomMove();
            return;
        }


        float l = Mathf.Abs(t.position.x - GetPos().x);
        if (Mathf.Abs(l) < mNpc.mBestAtkL)
        {

            if (gDefine.gLogic.mTeach.mIsInTeach)
            {
                mCurState = eState.Atk;
                mNpc.Play("atk");
                mAtkTip.BeginLockTip(this, mNpc.mShowAtkTipT, mNpc.mShowAtkTipSE);
            }
            else
            {
                //直接转向站立状态
                mCurState = eState.Idle;
                mNpc.Play("idle");
            }

        }
        else
        {

            Vector3 endPos = t.position;
            if (endPos.x > GetPos().x)
                endPos.x -= mNpc.mBestAtkL - 0.001f;
            else
                endPos.x += mNpc.mBestAtkL - 0.001f;

            endPos.y = gDefine.gGrounY;

            endPos = Vector3.MoveTowards(GetPos(), endPos, T * mNpc.mV);

            mNpc.SetPos(endPos);

        }
    }

    /// <summary>
    /// 被打状态，通常此时被击飞一段距离。目前默认是20倍的速度被击飞
    /// </summary>
    /// <param name="T"></param>
    void UpdateKickBack(float T)
    {
        if (mIsBeKickBack)
        {
            Vector3 pos =
                Vector3.MoveTowards(GetPos(), mKickBackPos, T * mNpc.mV * 20);

            mNpc.SetPos(pos);

            if (Mathf.Abs(pos.x - mKickBackPos.x) < 0.01f)
                mIsBeKickBack = false;
        }
    }

    void UpdateKickDown(float T)
    {
        if (mCurState == eState.KickDown)
        {
            // Vector3 pos =
            //     Vector3.MoveTowards(GetPos(), mKickBackPos, T * mNpc.mV * 7);

            // pos.y = gDefine.gGrounY;

            // mNpc.SetPos(pos);

            // if (Mathf.Abs(pos.x - mKickBackPos.x) < 0.01f)
            // {
            //     mNpc.Play("up");
            //     mCurState = eState.Up;
            // }

            Vector3 pos = GetPos();
            mDying2V.y += T * mDying2Acc;
            pos += mDying2V * T;
            if (pos.y <= gDefine.gGrounY)
            {
                //落地
                pos.y = gDefine.gGrounY;

                //kick off..

                mNpc.Play("up");
                mCurState = eState.Up;
            }

            mNpc.SetPos(pos);
        }
    }

    /// <summary>
    /// 通常是击飞状态
    /// </summary>
    /// <param name="T"></param>
    public void UpdateDying(float T)
    {
        mDyingT += Time.deltaTime;
        if (mDyingT >= mNpc.mDyingDelayT)
        {
            mCurState = eState.Died;
            mNpc.Destory();
        }
        // Vector3 pos =
        //   Vector3.MoveTowards(GetPos(), mKickBackPos, T * mNpc.mV * 3);

        // mNpc.SetPos(pos) ;

        // if( pos == mKickBackPos )
        // {
        //     //切换到彻底死亡状态
        //     mCurState = eState.Died;
        //     mNpc.Destory();
        // }
    }

    /// <summary>
    /// 通常是抛起状态
    /// </summary>
    /// <param name="T"></param>
    void UpdateDying2(float T)
    {
        Vector3 pos = GetPos();
        mDying2V.y += T * mDying2Acc;
        pos += mDying2V * T;
        if (pos.y <= gDefine.gGrounY)
        {
            //落地
            pos.y = gDefine.gGrounY;
            mNpc.SetPos(pos);
            //kick off..
            //mCurState = eState.Dying2KickFly;
            //先转向闪烁结束

            mCurState = eState.DyingFlash;
            mDyingFlashNum = 0;
            mDyingFlashT = 0;
            mNpc.Play("dyingFlash");

            //mCurState = eState.Died;
            // mNpc.Destory();
            return;
        }
        mNpc.SetPos(pos);
    }

    // void UpdateDyingFlash(float T)
    // {
    //     mDyingFlashT += T;
    //     if( mDyingFlashT > 1.0f)
    //     {
    //         mDyingFlashNum++;
    //         mDyingFlashT = 0;
    //         if (mDyingFlashNum == 7 )
    //         {
    //             mCurState = eState.Died;
    //             mNpc.gameObject.SetActive(false);
    //             return;
    //         }
    //     }

    //     if( mDyingFlashNum == 2 || mDyingFlashNum == 4|| mDyingFlashNum == 6)
    //     {
    //         mNpc.mSprite.color = new Color(1, 1, 1, 1);
    //     }
    //     else
    //         mNpc.mSprite.color = new Color(1, 1, 1, 0);

    // }


}



public class npcManager : MonoBehaviour
{
    List<CNpcInst> mDict = new List<CNpcInst>();

    public GameObject mBloodSEPreb;

    float mAllDiedDelayT = -1;

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gNpc = this;
    }

    public void ResetAllDieDelayT()
    {
        mAllDiedDelayT = -1;
    }

    public void ClearAll()
    {
        foreach (CNpcInst inst in mDict)
            inst.DestorySelfInst();
        mDict.Clear();

        mAllDiedDelayT = 0;
    }

    public CNpcInst CreateNpcInst(npcdata.eNpcType NpcType)
    {
        if (NpcType == npcdata.eNpcType.AirHitGround)
            return new CNpcZaDiInst();
        else if (NpcType == npcdata.eNpcType.Shield)
        {
            CNpcInst inst = new CNpcInst();
            inst.mAtkAI = new CShieldNpcAtkAI();
            inst.mIdleAI = new CShieldNpcIdleAI();
            inst.mSkillAI = new CShieldNpcSkillAI();
            inst.mComAtkIgnorPerc = 0.9f;
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.RoundSword)
        {
            CNpcInst inst = new CNpcInst();
            inst.mAtkAI = new CRoundSwordNpcAtkAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.BiaoQiang)
        {
            CNpcInst inst = new CNpcInst();
            inst.mIdleAI = new CBiaoQiangNpcIdleAI();
            inst.mSkillAI = new CBiaoQiangNpcSkillAI();
            inst.mMoveToTargetAI = new CBiaoQiangNpcMoveToTargetAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.HideDetonator)
        {
            CNpcInst inst = new CNpcInst();
            inst.mAtkAI = new CNpcHideAtkAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.PosionSword)
        {
            CNpcInst inst = new CNpcInst();
            inst.mAtkAI = new CNpcPosionDaggerAtkAI();
            inst.mSkillAI = new CNpcPosionDaggerSkillAI();
            inst.mBeAtkAI = new CNpcPosionDaggerBeAtkAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.BallBomb)
        {
            CNpcInst inst = new CNpcInst();
            inst.mAtkAI = new CNpcBombAtkAI();
            inst.mDyingAI = new CNpcBombDyingAI();
            inst.mDyingFlashAI = new CNpcBombDyingFlashAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.DefPursuer)
        {
            CNpcInst inst = new CNpcInst();
            inst.mBeAtkAI = new CDefNpcBeAtkAI();
            inst.mSkillAI = new CDefNpcSkillAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.PosionPursuer)
        {
            CNpcInst inst = new CNpcInst();
            inst.mIdleAI = new CPosionPursuerNpcIdleAI();
            inst.mSkillAI = new CPosionPursuerNpcSkillAI();
            inst.mMoveToTargetAI = new CPosionPursuerNpcMoveToTargetAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.JumpPursuer)
        {
            CNpcInst inst = new CNpcInst();
            inst.mIdleAI = new CJumpNpcIdleAI();
            inst.mSkillAI = new CJumpNpcSkillAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.AirFlane)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CAirPlaneNpcMoveToTargetAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.AirFire)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CAirFireNpcMoveToTargetAI();
            inst.mBeAtkAI = new CAirFireNpcBeAtkAI();
            inst.mDyingAI = new CAirFireNpcDyingAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.Cao)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CCaoNpcMoveToTargetAI();
            inst.mIdleAI = new CCaoNpcIdleAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.GoldCao)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CGoldCaoNpcMoveToTargetAI();
            inst.mIdleAI = new CGoldCaoNpcIdleAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.AirBomb)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CAirBombNpcMoveToTargetAI();
            inst.mDyingAI = new CAirBombDyingAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.AirZuZhou)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CAirFlowerNpcMoveToTargetAI();
            inst.mDyingAI = new CAirZuZhouNpcDyingAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.AirLight)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CAirLightNpcMoveToTargetAI();
            inst.mDyingAI = new CAirLightDyingAI();
            return inst;
        }
        else if (NpcType == npcdata.eNpcType.HalfDown)
        {
            CNpcInst inst = new CNpcInst();
            inst.mMoveToTargetAI = new CHalfDownNpcMoveToTargetAI();
            inst.mIdleAI = new CHalfDownIdleAI();
            return inst;
        }
        else
        {
            return new CNpcInst();
        }
    }

    public virtual bool DoDamage(Vector3 Pos, float L, bool IsFaceRight, int Damage, bool IsHeavy, bool KickOff,
        CNpcInst.eNpcClass NpcClass, bool IsSkill)
    {
        bool r = false;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass))
            {
                float l = inst.mNpc.gameObject.transform.position.x - Pos.x;
                if ((l > 0 && IsFaceRight) || (l < 0 && !IsFaceRight))
                {
                    if (Mathf.Abs(l) <= L)
                    {
                        //计算伤害
                        inst.BeDamage(Damage, IsHeavy, KickOff, IsSkill);
                        r = true;

                        //
                        GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                        bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                            new Vector3(0, 1.5f, -0.4f);

                    }
                }
            }
        }
        return r;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="L"></param>
    /// <param name="IsFaceRight"></param>
    /// <param name="Damage"></param>
    /// <param name="IsHeavy"></param>
    /// <param name="KickOff"></param>
    /// <param name="NpcClass"></param>
    /// <param name="IsSkill"></param>
    /// <returns></returns>
    public virtual List<CNpcInst> DoDamage(Vector3 Pos, float L, bool IsFaceRight, int Damage, bool IsHeavy, bool KickOff,
       CNpcInst.eNpcClass NpcClass, bool IsSkill, List<CNpcInst> ExceptNpcArr)
    {
        List<CNpcInst> arr = new List<CNpcInst>();
        bool r = false;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) && !ExceptNpcArr.Contains(inst))
            {
                float l = inst.mNpc.gameObject.transform.position.x - Pos.x;
                if ((l > 0 && IsFaceRight) || (l < 0 && !IsFaceRight))
                {
                    if (Mathf.Abs(l) <= L)
                    {
                        //计算伤害
                        inst.BeDamage(Damage, IsHeavy, KickOff, IsSkill);
                        r = true;

                        //
                        GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                        bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                            new Vector3(0, 1.5f, -0.4f);

                    }
                }
            }
        }
        return arr;
    }






    /// <summary>
    /// 致死率攻击
    /// </summary>
    /// <param name="Bx"></param>
    /// <param name="Ex"></param>
    /// <param name="Damage"></param>
    /// <param name="DeadPerc"></param>
    public CNpcInst[] DoDamageLineWithDeadPrec(float BX, float EX, int Damage, int DeadPerc, CNpcInst.eNpcClass NpcClass,
    bool IsSkill, CSkill.eSkill SkillType = CSkill.eSkill.Null)
    {
        List<CNpcInst> tmpArr = new List<CNpcInst>();

        foreach (CNpcInst inst in mDict)
        {
            float npcX = inst.mNpc.gameObject.transform.position.x;
            if (
                inst.IsLive() && ((npcX >= BX && npcX <= EX) || (npcX <= BX && npcX >= EX)) &&
                inst.IsNpcClass(NpcClass)
               )
            {
                //计算伤害
                inst.BeDamage(Damage, false, false, IsSkill, false, SkillType);
                tmpArr.Add(inst);

                //
                GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                    new Vector3(0, 1.5f, -0.4f);
            }
        }
        return tmpArr.ToArray();
    }



    public CNpcInst DoSingleDamage(float BX, float EX, int Damage, bool IsSkill)
    {
        CNpcInst tmp = null;
        float tmpL = 100000;

        foreach (CNpcInst inst in mDict)
        {
            float npcX = inst.GetPos().x;
            if (
                inst.IsLive() && ((npcX >= BX && npcX <= EX) || (npcX <= BX && npcX >= EX))
               )
            {
                float l = Mathf.Abs(npcX - BX);
                if (l < tmpL)
                {
                    tmp = inst;
                    tmpL = l;
                }

            }
        }

        if (tmp != null)
        {
            tmp.BeDamage(Damage, false, false, IsSkill);

            ////计算伤害
            GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
            bloodSe.transform.position = tmp.GetPos() +
                new Vector3(0, 1.5f, -0.4f);
        }

        return tmp;
    }

    public void DoAllDamge(int Damage, bool IsSkill)
    {
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive())
            {
                //计算伤害
                inst.BeDamage(Damage, false, false, IsSkill);

                //
                GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                    new Vector3(0, 1.5f, -0.4f);
            }
        }
    }


    public CNpcInst[] DoDamageLR(Vector3 Pos, float L, int Damage, bool IsHeavy, CNpcInst.eNpcClass NpcClass,
    bool IsSkill)
    {
        List<CNpcInst> tmpArr = new List<CNpcInst>();

        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) &&
                Mathf.Abs(inst.mNpc.gameObject.transform.position.x - Pos.x) <= L)
            {
                //计算伤害
                inst.BeDamage(Damage, IsHeavy, false, IsSkill);
                tmpArr.Add(inst);

                //
                GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                    new Vector3(0, 1.5f, -0.4f);
            }
        }
        return tmpArr.ToArray();
    }

    public CNpcInst[] DoDamageLRAndPushOff(Vector3 Pos, float L, int Damage, CNpcInst.eNpcClass NpcClass,
    bool IsSkill)
    {
        List<CNpcInst> tmpArr = new List<CNpcInst>();

        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) &&
                Mathf.Abs(inst.mNpc.gameObject.transform.position.x - Pos.x) <= L)
            {
                //计算伤害
                inst.BeDamage(Damage, false, true, IsSkill);
                tmpArr.Add(inst);

                //
                GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                    new Vector3(0, 1.5f, -0.4f);
            }
        }
        return tmpArr.ToArray();
    }

    public CNpcInst[] DoDamageLR(Vector3 Pos, float L, int Damage, bool IsHeavy, CNpcInst.eNpcClass NpcClass,
    bool IsSkill, List<CNpcInst> Arr)
    {
        List<CNpcInst> tmpArr = new List<CNpcInst>();

        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) &&
                Mathf.Abs(inst.mNpc.gameObject.transform.position.x - Pos.x) <= L && !Arr.Contains(inst))
            {
                //计算伤害
                inst.BeDamage(Damage, IsHeavy, false, IsSkill);
                tmpArr.Add(inst);

                //
                GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                    new Vector3(0, 1.5f, -0.4f);
            }
        }
        return tmpArr.ToArray();
    }

    public CNpcInst[] DoDamageShoot(float BX, float EX, int Damage, List<CNpcInst> Arr, CNpcInst.eNpcClass NpcClass,
    bool IsSkill)
    {
        List<CNpcInst> tmpArr = new List<CNpcInst>();

        foreach (CNpcInst inst in mDict)
        {
            float npcX = inst.mNpc.gameObject.transform.position.x;
            if (inst.IsLive() && ((npcX >= BX && npcX <= EX) || (npcX <= BX && npcX >= EX))
                && !Arr.Contains(inst) && inst.IsNpcClass(NpcClass) && inst.CanBeSelect())
            {
                //计算伤害
                inst.BeDamage(Damage, false, false, IsSkill);
                tmpArr.Add(inst);

                //
                GameObject bloodSe = GameObject.Instantiate(mBloodSEPreb);
                bloodSe.transform.position = inst.mNpc.gameObject.transform.position +
                    new Vector3(0, 1.5f, -0.4f);
            }
        }
        return tmpArr.ToArray();
    }

    public void PushBackWithInR(Vector3 CPos, float L)
    {
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.CanBeSelect() && !inst.mNpc.mIsInAir
                && inst.mNpcType != npcdata.eNpcType.Cao && inst.mNpcType != npcdata.eNpcType.GoldCao)
            {
                if (inst.GetPos().x > CPos.x && inst.GetPos().x - CPos.x < L)
                {
                    Vector3 pos = CPos + Vector3.right * L;
                    inst.mNpc.SetPos(pos);
                }
                else if (inst.GetPos().x < CPos.x && CPos.x - inst.GetPos().x < L)
                {
                    Vector3 pos = CPos - Vector3.right * L;
                    inst.mNpc.SetPos(pos);
                }
            }
        }
    }

    public bool HasSomeoneBeTake()
    {
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsBeTaken())
                return true;
        }
        return false;

    }


    public bool BeginKilledPlay(Vector3 PcPos, float L, bool IsFaceRight, ref Vector3 Pos)
    {
        CNpcInst inst = FindByL(PcPos.x, IsFaceRight, L);
        if (inst != null && inst.CanKilledPlay())
        {
            inst.CheckToKilledPlay();
            Pos = IsFaceRight ? inst.mNpc.mLeftPos.transform.position : inst.mNpc.mRightPos.transform.position;
            return true;
        }

        return false;
    }

    public CNpcInst FindByLine(float BX, float EX)
    {
        float tmpL = 7;
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive())
            {
                float npcx = inst.GetPos().x;
                if ((npcx > BX && npcx < EX) || (npcx < BX && npcx > EX))
                {
                    float l = Mathf.Abs(npcx - BX);
                    if (l < tmpL)
                    {
                        n = inst;
                        tmpL = Mathf.Abs(l);
                    }

                }
            }
        }

        return n;
    }

    public CNpcInst[] FindByLine(float BX, float EX, CNpcInst.eNpcClass NpcClass)
    {
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        foreach (CNpcInst inst in mDict)
        {
            float npcx = inst.GetPos().x;
            if (inst.IsLive() && ((npcx > BX && npcx < EX) || (npcx < BX && npcx > EX))
            && inst.IsNpcClass(NpcClass) && inst.CanBeSelect())
            {
                tmpDict.Add(inst);
            }
        }
        return tmpDict.ToArray();
    }

    public int KickDownAll(Vector3 PcPos, bool IsRight, CNpcInst ExceptNpc)
    {
        int r = 0;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && !inst.mNpc.mIsBoss && Mathf.Abs(inst.GetPos().x - PcPos.x) < 1.0f
            && inst != ExceptNpc && inst.mCanBeSelect && inst.mNpc.mCanKickBackAndKickFly)
            {
                float damage = gDefine.gPlayerData.mDamage;
                if (inst.mHp > damage)
                    inst.mHp -= (int)damage;
                else
                    inst.mHp = (int)(Random.Range(8, 20) * 0.01f * inst.mMaxHp);

                if (inst.KickDown(IsRight, Random.Range(7.0f, 8.0f)))
                {
                    r++;
                }
            }
        }
        return r;
    }

    public CNpcInst FindAirNpcByR(Vector3 Pos, float R)
    {
        float tmpL = 7;
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.mNpc.mIsInAir)
            {
                float l = Vector3.Distance(Pos, inst.GetHitSEPos());
                if (l < R)
                {
                    n = inst;
                    tmpL = l;
                }
            }
        }
        return n;
    }

    public CNpcInst FindByR(float X, float R, CNpcInst.eNpcClass NpcClass)
    {
        float tmpL = 100;
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.CanBeSelect() && inst.IsNpcClass(NpcClass))
            {
                float l = Mathf.Abs(X - inst.GetPos().x);
                if (l < R)
                {
                    n = inst;
                    tmpL = l;
                }
            }
        }
        return n;
    }

    /// <summary>
    /// bullet1 使用这个查找
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="R"></param>
    /// <returns></returns>
    public CNpcInst[] FindAllByR(Vector3 Pos, float R)
    {
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        foreach (CNpcInst inst in mDict)
        {
            float npcx = inst.GetPos().x;
            if (inst.IsLive() && inst.CanBeSelect() && Vector2.Distance(Pos, inst.GetRefMid().transform.position) < R)
            {
                tmpDict.Add(inst);
            }
        }
        return tmpDict.ToArray();
    }

    public int GetLiveNpcNumByType(npcdata.eNpcType NpcType)
    {
        int count = 0;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.mNpcType == NpcType)
            {
                count++;
            }
        }
        return count;
    }

    public bool ThereIsOnlyAirNpc()
    {
        bool haveAirNpc = false;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive())
            {
                if (inst.mNpc.mIsInAir)
                    haveAirNpc = true;
                else
                    return false;
            }
        }
        return haveAirNpc;
    }


    public CNpcInst FindByL(float X, bool IsFaceRight, float L, CNpcInst ExceptNpc = null)
    {
        float tmpL = L;
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && !inst.mNpc.mIsInAir && inst.CanBeSelect() && inst != ExceptNpc)
            {
                float l = inst.mNpc.gameObject.transform.position.x - X;
                if ((l > 0 && IsFaceRight) || (l < 0 && !IsFaceRight))
                {
                    if (Mathf.Abs(l) <= tmpL)
                    {
                        n = inst;
                        tmpL = Mathf.Abs(l);
                    }
                }
            }
        }

        return n;
    }

    /// <summary>
    /// 查找方向最近的空中怪
    /// </summary>
    /// <param name="X"></param>
    /// <param name="IsFaceRight"></param>
    /// <param name="L"></param>
    /// <returns></returns>
    public CNpcInst FindAirByL(float X, bool IsFaceRight, float L)
    {
        float tmpL = L;
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.mNpc.mIsInAir && inst.CanBeSelect())
            {
                float l = inst.mNpc.gameObject.transform.position.x - X;
                if ((l > 0 && IsFaceRight) || (l < 0 && !IsFaceRight))
                {
                    if (Mathf.Abs(l) <= tmpL)
                    {
                        n = inst;
                        tmpL = Mathf.Abs(l);
                    }
                }
            }
        }

        return n;
    }




    /// <summary>
    /// 以R为半径查找npc(实际上是用x来查找)，
    /// </summary>
    /// <param name="X">pc的坐标</param>
    /// <param name="R">查找范围，半径</param>
    /// <returns>Npc实例</returns>
    public CNpcInst FindWithR(float X, float R, CNpcInst.eNpcClass NpcClass)
    {
        float tmpL = Mathf.Abs(R);
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) && inst.CanBeSelect())
            {
                float l = Mathf.Abs(inst.mNpc.gameObject.transform.position.x - X);
                if (l < tmpL)
                {
                    n = inst;
                    tmpL = l;
                }
            }
        }

        return n;
    }

    /// <summary>
    /// 以R为半径查找区域内所有的npc(实际上是用x来查找)，
    /// </summary>
    /// <param name="X"></param>
    /// <param name="R"></param>
    /// <param name="NpcClass"></param>
    /// <returns></returns>
    public CNpcInst [] FindAllWithR(float X, float R, CNpcInst.eNpcClass NpcClass)
    {
        List<CNpcInst> arr = new List<CNpcInst>();
        float tmpL = Mathf.Abs(R);
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) && inst.CanBeSelect() )
            {
                float l = Mathf.Abs(inst.mNpc.gameObject.transform.position.x - X);
                if (l < tmpL)
                {
                    arr.Add(inst);
                }
            }
        }

        return arr.ToArray();
    }

    public CNpcInst [] FindHalfUpWithR(float X, float R, CNpcInst.eNpcClass NpcClass)
    {
        List<CNpcInst> arr = new List<CNpcInst>();
        float tmpL = Mathf.Abs(R);
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.mNpcType == npcdata.eNpcType.HalfUp )
            {
                float l = Mathf.Abs(inst.mNpc.gameObject.transform.position.x - X);
                if (l < tmpL)
                {
                    arr.Add(inst);
                }
            }
        }

        return arr.ToArray();
    }


    /// <summary>
    /// 真实的R半径寻找
    /// </summary>
    /// <param name="X"></param>
    /// <param name="R"></param>
    /// <param name="NpcClass"></param>
    /// <returns></returns>
    public CNpcInst FindWithRealRAndNpcHitPos(Vector3 Pos, float R, CNpcInst.eNpcClass NpcClass)
    {
        float tmpR = Mathf.Abs(R);
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) && inst.CanBeSelect())
            {
                Vector3 npcPos = inst.GetHitSEPos();
                npcPos.z = Pos.z = 0;

                float r = Vector3.Distance(Pos, npcPos);
                if (r <= R)
                {
                    n = inst;
                    tmpR = r;
                }
            }
        }

        return n;
    }

    /// <summary>
    /// 按照分类来查找npc
    /// </summary>
    /// <param name="X"></param>
    /// <param name="L"></param>
    /// <param name="NpcClass"></param>
    /// <returns></returns>
    public CNpcInst FindByL(float X, float L, CNpcInst.eNpcClass NpcClass)
    {
        float tmpL = L + 0.001f;
        CNpcInst n = null;
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass) && inst.CanBeSelect())
            {
                float l = Mathf.Abs(inst.GetPos().x - X);
                if (l < tmpL)
                {
                    n = inst;
                    tmpL = l;
                }
            }
        }

        return n;
    }

    public CNpcInst FindRandomNpc()
    {
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive())
                tmpDict.Add(inst);
        }

        if (tmpDict.Count > 0)
            return tmpDict[Random.Range(0, tmpDict.Count)];
        else
            return null;
    }
    /// <summary>
    /// 在某个范围内获得随机Npc
    /// </summary>
    /// <param name="X">中心 位置</param>
    /// <param name="R">查询半径</param>
    /// <returns>NPC</returns>
    public CNpcInst FindRandomNpc(float X, float R)
    {
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive())
            {
                float l = Mathf.Abs(X - inst.GetPos().x);
                if (l <= R)
                    tmpDict.Add(inst);
            }
        }

        if (tmpDict.Count > 0)
            return tmpDict[Random.Range(0, tmpDict.Count)];
        else
            return null;
    }

    public CNpcInst[] FindByL(float X, float L, CNpcInst ExceptNpc)
    {
        float tmpL = L + 0.001f;
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        int hashcode = ExceptNpc.GetHashCode();
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.GetHashCode() != hashcode)
            {
                float l = Mathf.Abs(inst.GetPos().x - X);
                if (l < tmpL)
                {
                    tmpDict.Add(inst);
                }
            }
        }

        return tmpDict.ToArray();
    }

    public CNpcInst[] OrderNpcByL(float X, float L, CNpcInst.eNpcClass NpcClass)
    {
        float tmpL = L + 0.001f;
        List<CNpcInst> tmpDict = new List<CNpcInst>();
        List<float> tmpLDict = new List<float>();
        foreach (CNpcInst inst in mDict)
        {
            if (inst.IsLive() && inst.IsNpcClass(NpcClass))
            {
                float l = Mathf.Abs(inst.GetPos().x - X);
                if (l < tmpL)
                {
                    for (int j = 0; j < tmpDict.Count; j++)
                    {
                        if (tmpLDict[j] >= l)
                        {
                            tmpDict.Insert(j, inst);
                            tmpLDict.Insert(j, l);
                            goto Here;
                        }
                    }

                    tmpDict.Add(inst);
                    tmpLDict.Add(l);
                }
            }
        Here:;
        }
        return tmpDict.ToArray();
    }

    public int GetLiveNpc()
    {
        int count = 0;
        for (int i = 0; i < mDict.Count; i++)
        {
            if (mDict[i].IsLive() && mDict[i].mNpcType != npcdata.eNpcType.HalfDown)
            {
                count++;
            }
        }
        return count;
    }

    public bool IsEndDelayOK()
    {
        if (Time.time > mAllDiedDelayT)
            return true;
        else
            return false;
    }

    public bool IsNpcClear()
    {
        for (int i = 0; i < mDict.Count; i++)
        {
            if (mDict[i].IsLive())
                return false;

            // if( mDict[i].mCurState != CNpcInst.eState.Died)
            //     return false;
        }
        if (mAllDiedDelayT <= 0)
            mAllDiedDelayT = Time.time + 0.5f;
        return true;
        //return mDict.Count == 0;
    }

    public bool IsEmpty()
    {
        return mDict.Count == 0;
    }

    /// <summary>
    /// 用来统计剩余npc的。 排除掉不需要统计的部分
    /// </summary>
    /// <returns></returns>
    public int GetLiveNpcForCount()
    {
        int count = 0;
        for (int i = 0; i < mDict.Count; i++)
        {
            if (mDict[i].IsLive() && mDict[i].CanBeCount())
            {
                count++;
            }
        }
        return count;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gDefine.gPause || gDefine.gLogic.mTeach.mIsInTeach)
        {
            for (int i = 0; i < mDict.Count; i++)
            {
                if (mDict[i].IsDiedState() || mDict[i].mNpc == null)
                {
                    CNpcInst npc = mDict[i];
                    npc.DestorySelfInst();
                    mDict.RemoveAt(i);
                    i--;
                    continue;
                }
                mDict[i].Update(Time.deltaTime);
            }
        }
    }


    public void AddNpc(CNpcInst Inst)
    {
        mDict.Add(Inst);
    }



}
