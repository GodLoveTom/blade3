using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 匝地怪
/// </summary>
public class CNpcZaDiInst : CNpcInst
{
    enum eZaDiState
    {
        Idle,
        MoveToTarget,
        Lock,
        Drop,
        Stay, //地面停留
        Up,
        Died,
        WaitInAirTeamRandomMove, //队列等待
        died1, // 坠落死亡
    }

    eZaDiState mZaDiState = eZaDiState.MoveToTarget;

    float mLockBeginT = 0;
    float mStayEndT = 0;

    float mLastAtkT = 0;
    float mDie1T = 0;

    GameObject mSELockObj;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActEventCallBack(NpcMono.eActEvent E)
    {
        if (E == NpcMono.eActEvent.Destory)
        {
            mZaDiState = eZaDiState.Died;
            mNpc.Destory();
        }
    }

    public override bool IsDiedState()
    {
        return mZaDiState == eZaDiState.Died;
    }

    public override void Event_AirNpcAtk()
    {
        mLastAtkT = Time.time + 10;
    }

    protected override bool IsOnGround()
    {
        return Mathf.Abs(GetPos().y - gDefine.gGrounY) < 3;
    }

    public override void Init(GameObject Obj, npcdata.eNpcType NpcType, Vector3 Pos, int WaveLvL)
    {
        base.Init(Obj, NpcType, Pos, WaveLvL);
        mZaDiState = eZaDiState.MoveToTarget;
        Pos.y = gDefine.gAirY;
        mNpc.SetPos(Pos);
        mNpc.mEventCallBackFunc = ActEventCallBack;

        if (mNpc.mIsAirTeam)
        {
            gDefine.gAirNpcGlobleTeam.Register(this);
            ChangeToWaitInAirTeamRandomMove();
        }
    }


    void UpdateDied1()
    {
        if (Time.time < mDie1T)
            return;

        Vector3 pos = GetPos();
        pos.y -= Time.deltaTime * 10;

        if (pos.y < gDefine.gGrounY)
        {
            pos.y = gDefine.gGrounY;
            mCurState = eState.Died;

            GameObject o = GameObject.Instantiate(gDefine.gData.mNpcRagePursuerBombFirePreb);
            o.transform.position = pos;

            DestorySelfInst();
            DestorySELockObj();
        }
        else
            mNpc.SetPos(pos);
    }

    public override void Update(float T)
    {
        if (mCurState == eState.AirFallDown)
        {
            base.Update(T);
            return;
        }

        mDamageShow.Update();

        mBuff.Update(this);

        if (mNpc == null)
            return;

        if (mHp <= 0)
        {
            if (mZaDiState == eZaDiState.died1)
                UpdateDied1();
            return;
        }


        if (Time.time < mFronzeT)
        {
            mNpc.Pause();
            return;
        }

        mNpc.Continue();



        //if (gDefine.gNpcIsWood)
        //  mCurState = eState.Idle;

        //走向玩家，等待攻击，如果可以攻击则攻击（被打状态除外，需要被打状态解除，一旦解除，则霸体）
        switch (mZaDiState)
        {
            case eZaDiState.Idle:
                //UpdateIdle(T);
                if (mNpc.mIsAirTeam)
                    ChangeToWaitInAirTeamRandomMove();
                else
                    mZaDiState = eZaDiState.MoveToTarget;
                break;
            case eZaDiState.MoveToTarget:
                UpdateMoveToTarget(T);
                break;
            case eZaDiState.Lock:
                UpdateLock();
                break;
            case eZaDiState.Drop:
                UpdateDrop();
                break;
            case eZaDiState.Stay:
                UpdateStay();
                break;
            case eZaDiState.Up:
                UpdateUp();
                break;
            //return; //死亡直接返回

            // case eState.BeAtk:
            //     UpdateBeAtk(T);
            //     break;
            // case eState.Dying2:
            //     UpdateDying2(T);
            //     break;
            // case eState.DyingFlash:
            //    // UpdateDyingFlash(T);
            //     break;
            // case eState.BeKilled:
            //     break;
            case eZaDiState.WaitInAirTeamRandomMove:
                UpdateWaitInAirTeamRandomMove();
                break;
            case eZaDiState.died1:
                UpdateDied1();
                break;
        }



        mNpc.FaceRight(GetPos().x < gDefine.GetPCTrans().position.x ? true : false);
    }

    public void ChangeToWaitInAirTeamRandomMove()
    {
        if (IsLive())
        {
            mZaDiState = eZaDiState.WaitInAirTeamRandomMove;
            mRandomMoveTarget.x = gDefine.gAirNpcGlobleTeam.GetWaitX(GetPos().x > gDefine.GetPCTrans().position.x);
            mRandomMoveTarget.y = gDefine.gAirY;
            mRandomMoveTarget.z = GetPos().z;
            mNpc.Play("move", false);
        }
    }

    public void UpdateWaitInAirTeamRandomMove()
    {
        if ( !gDefine.gLogic.mTeach.mIsInTeach && gDefine.gAirNpcGlobleTeam.IsSelfInAtkTeam(this, GetPos().x > gDefine.GetPCTrans().position.x))
        {
            mZaDiState = eZaDiState.MoveToTarget;
        }
        else
        {
            float l = Mathf.Abs(mRandomMoveTarget.x - GetPos().x);
            if (Mathf.Abs(l) <= 0.001f)
            {
                ChangeToWaitInAirTeamRandomMove();
            }

            else
            {
                Vector3 endPos = Vector3.MoveTowards(GetPos(), mRandomMoveTarget, Time.deltaTime * mNpc.mV);

                mNpc.SetPos(endPos);
            }
        }
    }

    public override void AddBuff(CBuff.eBuff BuffType, float LastT)
    {
        //if (mZaDiState >= eZaDiState.Drop)
        //   return;
        if (mHp <= 0)
            return;
        mBuff.AddBuff(this, BuffType, LastT);
    }

    public override void DestorySELockObj()
    {
        if (mSELockObj != null)
        {
            mSELockObj.SetActive(false);
            GameObject.Destroy(mSELockObj);
        }
    }

    public override void UpdateMoveToTarget(float T)
    {
        Transform t = gDefine.GetPCTrans();
        if (!gDefine.gAirNpcGlobleTeam.IsSelfInAtkTeam(this, GetPos().x > t.position.x) )
        {
            ChangeToWaitInAirTeamRandomMove();
            return;
        }

        float l = Mathf.Abs(t.position.x - GetPos().x);
        float dl = 0.001f;
        if( gDefine.gMecha!=null)
            dl = 0.5f;
        if (Mathf.Abs(l) <= dl )
        {
            if (mNpc.IsTimeReadyToAtk() && !gDefine.gLogic.mTeach.mIsInTeach)
            {
                //直接转向站立状态
                mZaDiState = eZaDiState.Lock;
                mNpc.Play("lock");
                mLockBeginT = Time.time;

                mSELockObj = GameObject.Instantiate(gDefine.gData.mLockSEPreb);
                Vector3 lockPos = GetPos();
                lockPos.y = gDefine.gGrounY;
                mSELockObj.transform.position = lockPos;

                mNpc.mQuitDestoryObj = mSELockObj;
            }
            else
            {
                
            }
            

        }
        else
        {

            Vector3 endPos = t.position;

            endPos.y = gDefine.gAirY;

            endPos = Vector3.MoveTowards(GetPos(), endPos, T * mNpc.mV);

            mNpc.SetPos(endPos);

        }
    }

    public override void BeDamage(int Damage, bool IsHeavy, bool IsKickOff, bool IsSkill, bool IsLongGun = false,
     CSkill.eSkill SkillType = CSkill.eSkill.Null)
    {
        if (!IsLive())
            return;

        if (mCurState == eState.AirFallDown)
        {
            base.BeDamage(Damage, IsHeavy, IsKickOff, IsSkill, IsLongGun, SkillType);
        }

        CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DeathFinger);
        if (skill != null && Time.time > skill.mParamFloat + 1 && !mNpc.mIsBoss)
        {
            if (Random.Range(0, 100) < 10)
            {
                mHp = 0;

                skill.mParamFloat = Time.time;
                //立刻死亡
                //CheckToDying2State();
                mNpc.Play("die");

                mNpc.CloseUI();

                gDefine.gFollowCam.PlayVibrate();

                Transform t = gDefine.GetPCTrans();

                //显示技能名称
                gDefine.gDamageShow.CreateDamageShow(skill.mName, t.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));

                mBuff.Close();

                if (mSELockObj != null)
                {
                    mSELockObj.SetActive(false);
                    GameObject.Destroy(mSELockObj);
                }



                return;
            }

        }

        if (!IsHeavy)
        {
            if (Random.Range(0, 100) < gDefine.gPlayerData.mDoubleDamagePerc)
            {
                Damage *= 2;
                IsHeavy = true;
            }
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

        //mHp =  mHp-Damage < 0 ?  (mIsBaBa?1:0):mHp-Damage;
        mHp = mHp - Damage < 0 ? 0 : mHp - Damage;
        mNpc.Refresh(mHp, mMaxHp);

        Vector3 pos = mNpc.GetDamageShowPos();
        pos.z -= 0.5f;

        //gDefine.gDamageShow.CreateDamageShow(Damage, pos, Color.white, IsHeavy);
        mDamageShow.Add(Damage, pos, Color.white, IsHeavy);

        // if ( mHp >0 )
        // {
        //     //被打状态
        //     CheckToBeAtkState(IsKickOff);
        // }
        // else
        // {
        //     gDefine.gPlayerData.GainMoney();

        //       //死亡状态
        //     if( Random.Range(0,100)<55)
        //     {
        //         CheckToDying2State();
        //     }
        //     else
        //     {

        //     }

        if (mHp <= 0)
        {
            CalcDrop();
            if (mNpc.mAirDropDie != null && !IsOnGround() && Random.Range(0, 100) < 50)
            {
                mCurState = eState.Died;
                mZaDiState = eZaDiState.Died;
                mNpc.Continue();
                mNpc.mAirDropDie.GoDie1();
                mNpc.Play("down");
                mNpc = null;
                if (mSELockObj != null)
                {
                    mSELockObj.SetActive(false);
                    GameObject.Destroy(mSELockObj);
                }
                return;

            }
            // else if(Random.Range(0,100)<10 && !IsOnGround())
            // {
            //     mZaDiState = eZaDiState.died1;
            //     mNpc.Play("die1");
            //     mDie1T = 0.8f+ Time.time;
            // }
            else
            {
                mNpc.Play("die");
            }


            mNpc.CloseUI();

            gDefine.gFollowCam.PlayVibrate();

            mFronzeT = 0;

            mNpc.Continue();

            if (mSELockObj != null)
            {
                mSELockObj.SetActive(false);
                GameObject.Destroy(mSELockObj);
            }

            mBuff.Close();
        }

        //Handheld.Vibrate();
        //}
    }

    public override void Fronze(float FronzeT)
    {
        mFronzeT = Time.time + FronzeT;

        if (mZaDiState == eZaDiState.Lock)
        {
            mZaDiState = eZaDiState.Idle;
            if (mSELockObj != null)
            {
                mSELockObj.SetActive(false);
                GameObject.Destroy(mSELockObj);
            }
            mNpc.Event_RefreshAtkT();
            mNpc.Play("idle");
        }
    }

    // public override bool DoDamage(Vector3 Pos, float L, bool IsFaceRight, int Damage,bool IsHeavy,bool KickOff)
    // {

    //     return base.DoDamage(Pos,L,IsFaceRight,Damage,IsHeavy,KickOff);
    //     //return false;
    // }

    public void UpdateLock()
    {
        if (Time.time >= mLockBeginT + 1.5f)
        {
            mZaDiState = eZaDiState.Drop;
            mNpc.Play("drop");
            mSELockObj.SetActive(false);
            GameObject.Destroy(mSELockObj);

            Event_AirNpcAtk();
        }
    }

    public void UpdateStay()
    {
        if (Time.time >= mStayEndT)
        {
            mZaDiState = eZaDiState.Up;
            mNpc.Play("up");
        }
    }

    public override bool CanBeInAirTeam()
    {
        return Time.time > mLastAtkT && mNpc.IsTimeReadyToAtk();
    }

    public void UpdateDrop()
    {
        Vector3 ePos = gDefine.GetPCTrans().position;
        ePos.x = GetPos().x;
        Vector3 pos = Vector3.MoveTowards(GetPos(), ePos, Time.deltaTime * 40);
        if (Mathf.Abs(pos.y - ePos.y) <= 0.01f)
        {
            mZaDiState = eZaDiState.Stay;
            mNpc.Play("stay");
            mStayEndT = Time.time + 1;

            if (Mathf.Abs(gDefine.GetPCTrans().position.x - GetPos().x) <= mNpc.mAtkL)
                gDefine.PcBeAtk(mDamage);

            AudioClip clip = gDefine.gData.GetSoundClip(10);
            if (clip != null)
                gDefine.gSound.Play(clip);

            gDefine.gFollowCam.PlayVibrate();

        }
        mNpc.SetPos(pos);
    }

    public void UpdateUp()
    {
        Vector3 ePos = GetPos();
        ePos.y = gDefine.gAirY;
        Vector3 pos = Vector3.MoveTowards(GetPos(), ePos, Time.deltaTime * 10);
        mNpc.SetPos(pos);
        if (Mathf.Abs(pos.y - ePos.y) <= 0.01f)
        {
            mZaDiState = eZaDiState.MoveToTarget;
            if ((ePos - pos).sqrMagnitude < 0.1f)
                mNpc.Play("move");
        }

    }
    public override void BeKilled(string Tip)
    {
        if (mHp > 0)
        {
            //gDefine.gPlayerData.GainMoney();
            CalcDrop();

            //死亡状态
            Vector3 pos = mNpc.GetDamageShowPos();

            mDamageShow.Add(mHp, pos, Color.white, false);

            mHp = 0;

            mNpc.Play("die");

            mNpc.CloseUI();

            gDefine.gFollowCam.PlayVibrate();

            mFronzeT = 0;

            mNpc.Continue();

            if (mSELockObj != null)
            {
                mSELockObj.SetActive(false);
                GameObject.Destroy(mSELockObj);
            }

            Vector3 damagePos = mNpc.GetDamageShowPos();

            if (!string.IsNullOrEmpty(Tip))
                mDamageShow.Add(Tip, damagePos, Color.white);

           // base.GainMoney();

        }

    }


}
