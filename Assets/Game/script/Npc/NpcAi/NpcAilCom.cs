//npc技能统一接口
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAICom
{
    // Update is called once per frame
    public virtual void Update(float T, CNpcInst Npc) { }

    public virtual void Init(CNpcInst Npc) { }

    public virtual void DoSkillEvent(int CustomEventValue, CNpcInst Npc) { }

    protected static float mT0 = 0;

}


/// <summary>
/// 盾牌特殊技能
/// </summary>
public class CShieldNpcIdleAI : NpcAICom
{
    long mSkillT = 0;
    public override void Update(float T, CNpcInst Npc)
    {
        if (mSkillT <= 0)
            mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
        if (System.DateTime.Now.Ticks >= mSkillT)
        {
            mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
            if (gDefine.IsPcCanBeAtk())
            {
                if (Random.Range(0, 100) < 50)
                {
                    //----切换突击---
                    Npc.mCurState = CNpcInst.eState.Skill;
                    Npc.mSkillAI.Init(Npc); //处事化参数
                    Npc.mNpc.Play("rush");
                    Vector3 pos0 = Npc.mNpc.GetDamageShowPos();
                    pos0.z -= 0.5f;
                    Npc.mDamageShow.Add("冲撞", pos0, Color.gray);
                    Npc.BeginBABA();
                }

            }
        }
        else
            Npc.UpdateIdle(T);
    }

}


/// <summary>
/// 盾牌特殊技能
/// </summary>
public class CShieldNpcSkillAI : NpcAICom
{
    Vector3 mSkillBeginPos;
    Vector3 mSkillEndPos;
    bool mBeginMove = false;
    float mT = 0;
    float mDamageT = 0;

    public override void Update(float T, CNpcInst Npc)
    {
        if (mBeginMove)
        {
            mT -= Time.deltaTime;
            Vector3 npcPos = Npc.GetPos();
            Vector3 pcPos = gDefine.GetPCTrans().position;

            Vector3 pos = Vector3.MoveTowards(npcPos, mSkillEndPos, Time.deltaTime * GetSkillV());

            if (Time.time > mDamageT && (pcPos.x >= pos.x && pcPos.x <= npcPos.x)
            || (pcPos.x >= npcPos.x && pcPos.x <= pos.x))
            {
                gDefine.PcBeAtk(Npc.mDamage);
                gDefine.PushBackPc(npcPos, Npc);
                mDamageT = Time.time + 1;
            }
            Npc.mNpc.SetPos(pos);

            if (Mathf.Abs(pos.x - mSkillEndPos.x) < 0.01f || mT < 0)
            {
                Npc.mCurState = CNpcInst.eState.Idle;
                Npc.mNpc.Play("idle");
                mT = 1.0f;
                Npc.mFaceIsControlByAI = false;
                // if (Mathf.Abs(pcPos.x - pos.x) <= Npc.mNpc.mAtkL)
                // {
                //     gDefine.PcBeAtk(Npc.mDamage);
                // }
            }

            // Vector3 dest = pcPos - npcPos;
            // float skillL = GetSkillL();
            // float destAbsL = Mathf.Abs(dest.x);
            // if (destAbsL > GetSkillL())
            // {
            //     dest.y = dest.z = 0;
            //     dest = mSkillBeginPos + dest.normalized * skillL;
            // }
            // else if (destAbsL > 1.5f)
            // {
            //     dest.y = dest.z = 0;
            //     dest = pcPos - dest.normalized * 1.5f;
            // }
            // else
            // {
            //     dest = Npc.GetPos();
            // }

            // dest.y = gDefine.gGrounY;
            // dest.z = npcPos.z;

            // Vector3 pos = Vector3.MoveTowards(npcPos, dest, Time.deltaTime * GetSkillV());
            // Npc.mNpc.SetPos(pos);
            // Npc.mNpc.FaceRight(pcPos.x > npcPos.x);

            // if (Mathf.Abs(pos.x - dest.x) < 0.01f && mT < 0)
            // {
            //     Npc.mCurState = CNpcInst.eState.Idle;
            //     Npc.mNpc.Play("idle");

            //     if (Mathf.Abs(pcPos.x - pos.x) <= Npc.mNpc.mAtkL)
            //     {
            //         gDefine.PcBeAtk(Npc.mDamage);
            //     }
            // }
        }
    }

    public override void Init(CNpcInst Npc)
    {
        //等待动作发出移动指令
        mBeginMove = false;
    }

    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (CustomEventValue == 0)
        {
            mBeginMove = true;
            mSkillBeginPos = Npc.GetPos();
            mSkillEndPos = mSkillBeginPos + ((mSkillBeginPos.x > gDefine.GetPCTrans().position.x) ?
                            Vector3.left * 10 : Vector3.right * 10);
            mT = 0.8f;
            mDamageT = 0;
            Npc.mFaceIsControlByAI = true;
        }

    }

    int GetSkillL()
    {
        return 3;
    }

    float GetSkillV()
    {
        return 15;
    }
}

public class CShieldNpcAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (CustomEventValue == 1)
        {
            Vector3 pcPos = gDefine.GetPCTrans().position;
            Vector3 pos = Npc.GetPos();
            if (Mathf.Abs(pos.x - pcPos.x) < Npc.mNpc.mAtkL && Mathf.Abs(pcPos.y - gDefine.gGrounY) < 3)
                gDefine.PcAddBuff(CBuff.eBuff.Paralysis, 2, 0);
        }
    }

}

///回旋镖npc
public class CRoundSwordNpcAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (CustomEventValue == 0)
        {
            //投掷
            GameObject o = GameObject.Instantiate(gDefine.gData.mRoundSwordThrowSEPreb);
            Vector3 throwPos = Npc.mNpc.mThrowPoint.transform.position;
            Vector3 dest = throwPos;
            dest.x += gDefine.GetPCTrans().position.x > Npc.GetPos().x ? 9.5f : -9.5f;

            se_roundSword script = o.GetComponent<se_roundSword>();
            script.Init(throwPos, Npc, dest, Npc.mDamage);
        }
    }

}

public class CNpcHideAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (CustomEventValue == 0)
        {
            Vector3 throwPos = Npc.mNpc.mThrowPoint.transform.position;
            for (int i = 0; i < 3; i++)
            {
                GameObject o = GameObject.Instantiate(Npc.mNpc.mThrowItemPreb);
                se_NpcSkill_Throw s = o.GetComponent<se_NpcSkill_Throw>();
                Vector3 dest;
                if (gDefine.GetPCTrans().position.x > Npc.GetPos().x)
                    dest = Npc.GetPos() + Vector3.right * (5 + i * 2);
                else
                    dest = Npc.GetPos() + Vector3.left * (5 + i * 2);

                dest.y = gDefine.gGrounY;

                s.Init(throwPos, dest, Npc.GetDamage());
            }
        }
    }

}

//毒匕首Npc

public class CNpcPosionDaggerAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {

        Vector3 throwPos = Npc.mNpc.mThrowPoint.transform.position;

        GameObject o = GameObject.Instantiate(Npc.mNpc.mThrowItemPreb);
        se_PosionDagger s = o.GetComponent<se_PosionDagger>();
        Vector3 dest;
        if (gDefine.GetPCTrans().position.x > Npc.GetPos().x)
            dest = throwPos + Vector3.right * 6.0f;
        else
            dest = throwPos + Vector3.left * 6.0f;



        s.Init(throwPos, dest, Npc.GetDamage());

    }

}

public class CNpcPosionDaggerBeAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        Npc.mCurState = CNpcInst.eState.Skill;
        Npc.mNpc.Play("jumpBack");
        Npc.mSkillAI.DoSkillEvent(0, Npc);
        Npc.BeginBABA();
    }

}

public class CNpcPosionDaggerSkillAI : NpcAICom
{
    int state = 0; //0 开始后跳 1 后跳中  若后跳结束，并做一次攻击
    float mJumpBackV = 10;
    Vector3 mDestPos;
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (CustomEventValue == 0)
        {   //jumpBack
            state = 1;
        }
        else if (CustomEventValue == 1)
        {
            Npc.mCurState = CNpcInst.eState.Atk;
            Npc.mNpc.Play("atk");
        }
    }

    public override void Update(float T, CNpcInst Npc)
    {
        if (state == 1)
        {
            Vector3 npcPos = Npc.GetPos();
            if (gDefine.GetPCTrans().position.x > npcPos.x)
                npcPos.x -= Time.deltaTime * mJumpBackV;
            else
                npcPos.x += Time.deltaTime * mJumpBackV;

            Npc.mNpc.SetPos(npcPos);
        }


    }

}


//炸弹Npc

public class CNpcBombAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {

        Vector3 throwPos = Npc.mNpc.mThrowPoint.transform.position;

        GameObject o = GameObject.Instantiate(Npc.mNpc.mThrowItemPreb);
        se_Npc_Bomb s = o.GetComponent<se_Npc_Bomb>();
        Vector3 dest;
        if (gDefine.GetPCTrans().position.x > Npc.GetPos().x)
            dest = throwPos + Vector3.right * 8.0f;
        else
            dest = throwPos + Vector3.left * 8.0f;

        throwPos.y = dest.y = gDefine.gGrounY + 0.45f;


        s.Init(throwPos, dest, Npc.GetDamage());

    }

}

public class CNpcBombDyingFlashAI : NpcAICom
{
    float mT = 0;
    public override void Update(float T, CNpcInst Npc)
    {
        Vector3 pos = Npc.GetPos();
        Npc.UpdateDyingFlash(T);
        if (Npc.mCurState == CNpcInst.eState.Died)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mNpcBombBombPreb);

            pos.y = gDefine.gGrounY;
            o.transform.position = pos;

            if (Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
                    Mathf.Abs(gDefine.GetPCTrans().position.y - pos.y) < 3)
            {
                gDefine.PcBeAtk(Npc.GetDamage());
            }
        }
    }
}


public class CNpcBombDyingAI : NpcAICom
{

    float mT = 0;
    public override void Update(float T, CNpcInst Npc)
    {
        Vector3 pos = Npc.GetPos();
        Npc.UpdateDying(T);
        if (Npc.mCurState == CNpcInst.eState.Died)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mNpcBombBombPreb);

            pos.y = gDefine.gGrounY;
            o.transform.position = pos;

            if (Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
                    Mathf.Abs(gDefine.GetPCTrans().position.y - pos.y) < 3)
            {
                gDefine.PcBeAtk(Npc.GetDamage());
            }
        }
    }

}

///
public class CPosionPursuerNpcIdleAI : NpcAICom
{
    public long mSkillT = 0;

    public override void Init(CNpcInst Npc)
    {

        if (mT0 <= 0)
            mT0 = Time.time + Random.Range(7f, 8f);
        else
        {
            mT0 = Time.time + Random.Range(10f, 12f);
        }
    }
    public override void Update(float T, CNpcInst Npc)
    {
        if (mT0 <= 0)
            mT0 = Time.time + Random.Range(7f, 8f);

        if (Time.time >= mT0)
        {
            mT0 = Time.time + Random.Range(10f, 12f);
            if (gDefine.IsPcCanBeAtk())
            //&& Mathf.Abs(gDefine.GetPCTrans().position.x - Npc.GetPos().x) < Npc.mNpc.mAtkL)
            {
                //----切换突击---
                Npc.mCurState = CNpcInst.eState.Skill;
                Npc.mNpc.Play("skill");
                Npc.BeginBABA();
            }
        }
        else
            Npc.UpdateIdle(T);
    }

}

public class CPosionPursuerNpcMoveToTargetAI : NpcAICom
{

    public override void Init(CNpcInst Npc)
    {
        if (mT0 <= 0)
            mT0 = Time.time + Random.Range(7f, 8f);
        else
        {
            mT0 = Time.time + Random.Range(10f, 12f);
        }
    }


    public override void Update(float T, CNpcInst Npc)
    {
        if (mT0 <= 0)
            mT0 = Time.time + Random.Range(7f, 8f);

        if (Time.time >= mT0)
        {
            mT0 = Time.time + Random.Range(10f, 12f);
            if (gDefine.IsPcCanBeAtk())
            // && Mathf.Abs(gDefine.GetPCTrans().position.x - Npc.GetPos().x) < Npc.mNpc.mAtkL)
            {

                //----切换突击---
                Npc.mCurState = CNpcInst.eState.Skill;
                Npc.mNpc.Play("skill");
                Npc.BeginBABA();
            }
        }
        else
            Npc.UpdateMoveToTarget(T);
    }

}

public class CPosionPursuerNpcSkillAI : NpcAICom
{
    float mSpareT = 0;
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (Time.time > mSpareT)
        {
            mSpareT = Time.time + 2;
            GameObject o = GameObject.Instantiate(gDefine.gData.mNpcPosionBombPreb);
            se_Npc毒飞弹新 s = o.GetComponent<se_Npc毒飞弹新>();

            s.Init(Npc.GetDamage(), 2, 2);

            Npc.mCurState = CNpcInst.eState.Idle;
            Npc.mIdleAI.Init(Npc);
            Npc.mMoveToTargetAI.Init(Npc);
        }


    }

}



/// <summary>
/// 标枪IDle
/// </summary>
public class CBiaoQiangNpcIdleAI : NpcAICom
{

    long mSkillT = 0;
    public override void Update(float T, CNpcInst Npc)
    {
         Npc.mCurState = CNpcInst.eState.MoveToTarget;
         Npc.mNpc.Play("move");

        // if (mSkillT <= 0)
        //     mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
        // if (System.DateTime.Now.Ticks >= mSkillT)
        // {
        //     mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
        //     if (gDefine.IsPcCanBeAtk())
        //     {
        //         if (Random.Range(0, 100) < 50)
        //         {
        //             //----切换突击---
        //             Npc.mCurState = CNpcInst.eState.Skill;
        //             Npc.mSkillAI.Init(Npc); //处事化参数
        //             Npc.mNpc.Play("skill");
        //             Npc.BeginBABA();
        //         }
        //         else
        //             Npc.UpdateIdle(T);
        //     }
        // }
        // else
        // {
        //     Npc.ChangeToRandomMoveState();
        // }
    }

}



/// <summary>
/// 标枪IDle
/// </summary>
public class CBiaoQiangNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    float mSkillT = 0;
    Vector3 mDestPos;

    public override void Init(CNpcInst Npc)
    {
         CalcDestPos(Npc);
    }

    public override void Update(float T, CNpcInst Npc)
    {
        if (!mInit)
        {
            mSkillT = Time.time + Random.Range(5f, 7f);
            CalcDestPos(Npc);
            mInit = true;
        }

        if (Time.time > mSkillT)
        {
            mSkillT = Time.time + Random.Range(5f, 7f);
            Npc.mCurState = CNpcInst.eState.Skill;
            Npc.mSkillAI.Init(Npc); //处事化参数
            Npc.mNpc.Play("skill");
            Npc.BeginBABA();
            Npc.mFaceIsControlByAI = true;
        }
        else
        {
            Vector3 pcPos = gDefine.GetPCTrans().position;
            Vector3 selfPos = Npc.GetPos();
            if( Mathf.Abs(pcPos.x - mDestPos.x) < 3 ||Mathf.Abs(pcPos.x - mDestPos.x) > 19 )
            {
                CalcDestPos(Npc);
            }


            Vector3 pos = Vector3.MoveTowards( selfPos, mDestPos,Time.deltaTime* Npc.mNpc.mV);
            Npc.mNpc.SetPos(pos);
            if(Vector3.Distance(pos, mDestPos )<0.01f )
                CalcDestPos(Npc);
        }

        // if (mSkillT <= 0)
        //     mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
        // if (System.DateTime.Now.Ticks >= mSkillT)
        // {
        //     mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
        //     if (gDefine.IsPcCanBeAtk())
        //     {
        //         if (Random.Range(0, 100) < 50)
        //         {
        //             //----切换突击---
        //             Npc.mCurState = CNpcInst.eState.Skill;
        //             Npc.mSkillAI.Init(Npc); //处事化参数
        //             Npc.mNpc.Play("skill");
        //             Npc.BeginBABA();
        //         }
        //         else
        //             Npc.UpdateIdle(T);
        //     }
        // }
        // else
        // {
        //     Npc.ChangeToRandomMoveState();
        // }
    }

    void CalcDestPos(CNpcInst Npc)
    {
        Vector3 pcPos = gDefine.GetPCTrans().position;
        Vector3 selfPos = Npc.GetPos();
        float xOff = Random.Range(-3f,3f);
        float x =  (Npc.GetPos().x > pcPos.x?6.5f:-6.5f) + xOff + pcPos.x ;
        mDestPos = selfPos;
        mDestPos.x = x;
        mDestPos.y = gDefine.gGrounY;
    }

}


/// <summary>
/// 标枪特殊技能
/// </summary>
public class CBiaoQiangNpcSkillAI : NpcAICom
{
    Vector3 mSkillBeginPos;
    Vector3 mSkillEndPos;
    bool mBeginMove = false;
    float mT = 0;
    float mDamageT = 0;

    public override void Update(float T, CNpcInst Npc)
    {
        if (mBeginMove)
        {
            mT -= Time.deltaTime;
            Vector3 npcPos = Npc.GetPos();
            Vector3 pcPos = gDefine.GetPCTrans().position;

            Vector3 pos = Vector3.MoveTowards(npcPos, mSkillEndPos, Time.deltaTime * GetSkillV());

            if (Time.time > mDamageT && (pcPos.x >= pos.x && pcPos.x <= npcPos.x)
            || (pcPos.x >= npcPos.x && pcPos.x <= pos.x) && pcPos.y < gDefine.gGrounY+ 3)
            {
                gDefine.PcBeAtk(Npc.mDamage);
                mDamageT = Time.time + 1;
            }
            Npc.mNpc.SetPos(pos);

            if (Mathf.Abs(pos.x - mSkillEndPos.x) < 0.01f || mT < 0)
            {
                Npc.mCurState = CNpcInst.eState.MoveToTarget;
                Npc.mNpc.Play("move");
                mT = 1.0f;
                Npc.mFaceIsControlByAI = false;
                Npc.mMoveToTargetAI.Init(Npc);

                // if (Mathf.Abs(pcPos.x - pos.x) <= Npc.mNpc.mAtkL)
                // {
                //     gDefine.PcBeAtk(Npc.mDamage);
                // }
            }

           
        }
        // if (mBeginMove)
        // {
        //     mT -= Time.deltaTime;
        //     Vector3 npcPos = Npc.GetPos();
        //     Vector3 pcPos = gDefine.GetPCTrans().position;
        //     Vector3 dest = pcPos - npcPos;
        //     float skillL = GetSkillL();
        //     float destAbsL = Mathf.Abs(dest.x);
        //     if (destAbsL > GetSkillL())
        //     {
        //         dest.y = dest.z = 0;
        //         dest = mSkillBeginPos + dest.normalized * skillL;
        //     }
        //     else if (destAbsL > 1.5f)
        //     {
        //         dest.y = dest.z = 0;
        //         dest = pcPos - dest.normalized * 1.5f;
        //     }
        //     else
        //     {
        //         dest = Npc.GetPos();
        //     }

        //     dest.y = gDefine.gGrounY;
        //     dest.z = npcPos.z;

        //     Vector3 pos = Vector3.MoveTowards(npcPos, dest, Time.deltaTime * GetSkillV());
        //     Npc.mNpc.SetPos(pos);
        //     Npc.mNpc.FaceRight(pcPos.x > npcPos.x);

        //     if (Mathf.Abs(pos.x - dest.x) < 0.01f && mT < 0)
        //     {
        //         Npc.mCurState = CNpcInst.eState.Idle;
        //         Npc.mNpc.Play("idle");

        //         if (Mathf.Abs(pcPos.x - pos.x) <= Npc.mNpc.mAtkL)
        //         {
        //             gDefine.PcBeAtk(Npc.mDamage);
        //         }
        //     }
        // }
    }

    public override void Init(CNpcInst Npc)
    {
        mBeginMove = false;
    }

    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        if (CustomEventValue == 0)
        {
            mBeginMove = true;
            mSkillBeginPos = Npc.GetPos();
            mSkillEndPos = mSkillBeginPos + ((mSkillBeginPos.x > gDefine.GetPCTrans().position.x) ?
                            Vector3.left * 14 : Vector3.right * 14);
            mT = 0.8f;
            mDamageT = 0;
            Npc.mFaceIsControlByAI = true;
        }

    }

    int GetSkillL()
    {
        return 3;
    }

    float GetSkillV()
    {
        return 40;
    }
}



/// <summary>
/// 格挡毒液反击
/// </summary>
public class CDefNpcBeAtkAI : NpcAICom
{
    int mBeAtkCount = 0;
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        mBeAtkCount++;
        if (mBeAtkCount >= 2)
        {
            mBeAtkCount = 0;
            if (Random.Range(0, 100) < 80)
            {
                Npc.mSkillAI.Init(Npc);
                Npc.BeginBABA();
                Npc.mNpc.Play("def");
                Npc.mCurState = CNpcInst.eState.Skill;
            }
        }
    }

}

public class CDefNpcSkillAI : NpcAICom
{
    int mState = 0; // 格挡免疫所有伤害， 1 吐毒
    float mT = 0;
    int mBeAtkCount = 0;
    float mPcBeAtkT = 0;
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        //do ..damage..
        if (
          Mathf.Abs(gDefine.GetPCTrans().position.x - Npc.GetPos().x) < 7f &&
          Mathf.Abs(gDefine.GetPCTrans().position.y - Npc.GetPos().y) < 3
          && Time.time > mPcBeAtkT)
        {
            mPcBeAtkT = Time.time + 0.5f;
            gDefine.PcBeAtk((int)(Npc.GetDamage()));
            int damage = (int)(gDefine.gPlayerData.mHpMax * 0.1f);
            gDefine.PcAddBuff(CBuff.eBuff.Posion, 5, damage);
        }
    }

    public override void Update(float T, CNpcInst Npc)
    {
        if (gDefine.gPlayerData.mHp <= 0)
        {
            mState = 0;
            Npc.mCurState = CNpcInst.eState.Idle;
            Npc.mNpc.Play("Idle");
            return;
        }


        mT += Time.deltaTime;
        if (mState == 0)
        {
            if (mT >= 1.8f)
            {
                mState = 1;
                Npc.mNpc.Play("posion");
            }

        }
        else if (mT >= 3.0f)
        {
            mState = 0;
            Npc.mCurState = CNpcInst.eState.Idle;
            Npc.mNpc.Play("Idle");

            //create posion trap
            GameObject trap = GameObject.Instantiate(gDefine.gData.mNpcPosionTrapSEPreb);
            Vector3 pos = Npc.mNpc.mRefAtkPoint.transform.position;
            pos.y = gDefine.gGrounY;

            trap.transform.position = pos;
            NpcSkillPosionTrap s = trap.GetComponent<NpcSkillPosionTrap>();
            float hp = gDefine.gPlayerData.mHpMax * 0.02f;
            if (hp < 1)
                hp = 1;
            s.Init(pos, hp);
        }
    }

    public override void Init(CNpcInst Npc)
    {
        mState = 0;
        mT = 0;
    }

}


/// <summary>
/// 跳跃者idle
/// </summary>
public class CJumpNpcIdleAI : NpcAICom
{
    long mSkillT = 0;
    public override void Update(float T, CNpcInst Npc)
    {
        Npc.mNpc.mSELockObj.SetActive(false);
        if (mSkillT <= 0)
            mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
        if (System.DateTime.Now.Ticks >= mSkillT)
        {
            mSkillT = System.DateTime.Now.Ticks + Random.Range(5, 7) * (long)10000000;
            if (gDefine.IsPcCanBeAtk())
            {
                //----切换跳跃---
                Npc.mCurState = CNpcInst.eState.Skill;
                Npc.mSkillAI.Init(Npc); //处事化参数

                Npc.mNpc.Play("jumpUp");
                Npc.BeginBABA();

                Npc.mCanBeSelect = false;
            }
        }
        else
            Npc.UpdateIdle(T);
    }

    public override void Init(CNpcInst Npc)
    {
        mSkillT = 0;
    }

}


public class CJumpNpcSkillAI : NpcAICom
{
    int mState = 0; // 0 跳起， 1 提示坠落  2 坠落
    float mV = 65;
    float mT = 0;
    // GameObject mTipObj;


    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        //毒地波
        //向左波
        GameObject wave = GameObject.Instantiate(gDefine.gData.mNpcPosionWavePreb);
        se_Npc_PosionWave script = wave.GetComponent<se_Npc_PosionWave>();

        Vector3 bpos = Npc.GetPos();
        bpos.y = gDefine.gGrounY;

        script.Init(bpos, bpos + Vector3.left * 10, (int)(Npc.GetDamage() * 1.5f));
        script.mV = 7f;

        //向右波
        wave = GameObject.Instantiate(gDefine.gData.mNpcPosionWavePreb);
        script = wave.GetComponent<se_Npc_PosionWave>();

        script.Init(bpos, bpos + Vector3.right * 10, (int)(Npc.GetDamage() * 1.5f));
        script.mV = 7f;

        Npc.mCanBeSelect = true;

        Npc.mIdleAI.Init(Npc);
    }

    public override void Update(float T, CNpcInst Npc)
    {
        if (gDefine.gPlayerData.mHp <= 0)
        {
            mState = 0;
            Npc.mCurState = CNpcInst.eState.Idle;
            Npc.mNpc.Play("Idle");
            Vector3 pos = Npc.GetPos();
            pos.y = gDefine.gGrounY;
            Npc.mNpc.SetPos(pos);
            Npc.mNpc.mSELockObj.SetActive(false);
            Npc.mCanBeSelect = true;
            return;
        }

        mT += Time.deltaTime;
        if (mState == 0)
        {
            Npc.mNpc.transform.Translate(0, Time.deltaTime * mV, 0);
            if (Npc.GetPos().y > gDefine.gGrounY + 25)
            {
                mState = 1;
                mT = 0;


                //if (mTipObj == null)
                //  mTipObj = GameObject.Instantiate(gDefine.gData.mAtkTipPreb);

            }
        }
        else if (mState == 1)
        {
            if (mT > 2.0f)
            {
                Vector3 pos = gDefine.GetPCTrans().position;
                pos.y = gDefine.gGrounY;
                Npc.mNpc.mSELockObj.SetActive(true);
                Npc.mNpc.mSELockObj.transform.position = pos;
                mState = 2;
                mT = 0;
            }
        }
        else if (mState == 2)
        {
            if (mT > 3.0f)
            {
                Npc.mNpc.mSELockObj.SetActive(false);
                mState = 3;
                Npc.mNpc.Play("drop");

                Vector3 pos = Npc.mNpc.mSELockObj.transform.position;
                pos.y = Npc.GetPos().y;
                Npc.mNpc.SetPos(pos);

                GameObject ex = GameObject.Instantiate(gDefine.gData.mNpcHitGroundEXSEPreb);
                pos.y = gDefine.gGrounY;
                ex.transform.position = pos;
            }
            else
            {
                // Vector3 pos = gDefine.GetPCTrans().position;
                // pos.y = gDefine.gGrounY;
                // if (mTipObj == null)
                //     mTipObj = GameObject.Instantiate(gDefine.gData.mAtkTipPreb);
                // mTipObj.transform.position = pos;
            }
        }
        else if (mState == 3)
        {
            Npc.mNpc.transform.Translate(0, -Time.deltaTime * mV, 0);
            if (Npc.GetPos().y <= gDefine.gGrounY)
            {
                mState = 4;
                Vector3 pos = Npc.GetPos();
                pos.y = gDefine.gGrounY;
                Npc.mNpc.SetPos(pos);
                Npc.mNpc.Play("hitGround");
                gDefine.PlayVibrate();

                //伤害
                if (
                        Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
                        Mathf.Abs(gDefine.GetPCTrans().position.y - pos.y) < 3)
                {
                    gDefine.PcBeAtk((int)(Npc.GetDamage() * 1.5f));
                }

            }
        }
    }

    public override void Init(CNpcInst Npc)
    {
        mState = 0;
        mT = 0;
    }

}

//空中轰炸机
public class CAirPlaneNpcMoveToTargetAI : NpcAICom
{
    const float mState0L = 10; //距离10的时候开始初始化
    bool mInit = false;
    Vector3 mDestPos;

    float mV = 5.5f;

    float mDeltBombL = 4.1f;
    float mFirstBombL = 8.2f;
    //8.72..4.86  //10

    int mState = 0; // 0 加速接近玩家， //1 开始轰炸  // 2 快速飞离， 并自毁
    float mDirIsR = 1;

    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        //do ..damage..
        if (
          Mathf.Abs(gDefine.GetPCTrans().position.x - Npc.GetPos().x) < 6.7f &&
          Mathf.Abs(gDefine.GetPCTrans().position.y - Npc.GetPos().y) < 3)
        {
            gDefine.PcBeAtk((int)(Npc.GetDamage() * 1.5f));
            int damage = (int)(gDefine.gPlayerData.mHpMax * 0.1f);
            gDefine.PcAddBuff(CBuff.eBuff.Posion, 5, damage);
        }
    }

    public override void Update(float T, CNpcInst Npc)
    {
        Npc.mCanBeSelect = false;
        Vector3 pos = Npc.GetPos();
        Vector3 pcPos = gDefine.GetPCTrans().position;
        Npc.mFaceIsControlByAI = true;

        if (!mInit)
        {
            mDirIsR = pcPos.x > pos.x ? 1 : -1;
            mInit = true;
        }

        if (mState == 0)
        {
            //加速接近玩家
            float x = pos.x + mDirIsR * Time.deltaTime * mV * 4;
            pos.x = x;
            Npc.mNpc.SetPos(pos);
            if (Mathf.Abs(x - pcPos.x) < 12)
            {
                mState = 1;
                //初始化轰炸，以及轰炸位置//
                InitBombLockPos(Npc);
            }
        }
        else
        if (mState == 1)
        {
            pos = Vector3.MoveTowards(pos, mDestPos, Time.deltaTime * mV);
            Npc.mNpc.SetPos(pos);
            if (Vector3.Distance(pos, mDestPos) < 0.01f)
            {
                mState = 2;
            }
        }
        else if (mState == 2)
        {
            float x = pos.x + mDirIsR * Time.deltaTime * mV * 4;
            pos.x = x;
            Npc.mNpc.SetPos(pos);
            if (Mathf.Abs(x - pcPos.x) > 12)
            {
                Npc.mCurState = CNpcInst.eState.Died;
                Npc.mHp = 0;
                GameObject.Destroy(Npc.mNpc.gameObject);
            }
        }
    }

    void InitBombLockPos(CNpcInst Npc)
    {
        int damage = Npc.GetDamage();

        NpcAirBombContainer c = Npc.mNpc.gameObject.GetComponent<NpcAirBombContainer>();
        Vector3 cenPos = gDefine.GetPCTrans().position;
        cenPos.y = gDefine.gGrounY;
        if (mDirIsR > 0)
        {
            //左向右
            float bx = cenPos.x - mFirstBombL;
            for (int i = 0; i < 5; i++)
            {
                cenPos.x = bx + i * mDeltBombL;
                c.mBombArr[i].ShowTargetLock(cenPos, damage, i);
            }

            mDestPos = Npc.GetPos();
            mDestPos.x = bx + 20;
        }
        else
        {
            //右向左
            float bx = cenPos.x + mFirstBombL;
            for (int i = 0; i < 5; i++)
            {
                cenPos.x = bx - i * mDeltBombL;
                c.mBombArr[i].ShowTargetLock(cenPos, damage, i);
            }

            mDestPos = Npc.GetPos();
            mDestPos.x = bx - 20;

        }
    }

}

//
//空中散弹
//
public class CAirFireNpcMoveToTargetAI : NpcAICom
{
    const float mFireMinT = 5.0f;
    const float mFireMaxT = 7.0f;

    float mAtkT = -1;
    Vector3 mDestPos;
    float mV = 3;

    bool mInit = false;

    public override void Update(float T, CNpcInst Npc)
    {
        Vector3 pos = Npc.GetPos();
        if (mAtkT < 0)
            mAtkT = Random.Range(mFireMinT, mFireMaxT);

        mAtkT -= Time.deltaTime;
        if (mAtkT < 0)
        {
            Fire(Npc);
        }

        if (!mInit)
        {
            CalcDestPos(Npc);
            mInit = true;
        }


        pos = Vector3.MoveTowards(pos, mDestPos, Time.deltaTime * mV);
        pos.y = gDefine.gAirY;
        Npc.mNpc.SetPos(pos);

        if (Vector3.Distance(pos, mDestPos) < 0.01f)
        {
            CalcDestPos(Npc);
        }
    }

    void CalcDestPos(CNpcInst Npc)
    {
        mDestPos.x = Camera.main.transform.position.x + Random.Range(-5.0f, 5.0f);
        mDestPos.y = gDefine.gAirY;
        mDestPos.z = Npc.GetPos().z;
    }

    void Fire(CNpcInst Npc)
    {
        for (int i = 0; i < 8; i++)
        {
            GameObject o = GameObject.Instantiate(Npc.mNpc.mThrowItemPreb);
            Npc_AirFireBullet script = o.GetComponent<Npc_AirFireBullet>();
            Vector3 deltPos = CalcBulletDestPos(i);
            script.Init(Npc.mNpc.mThrowPoint.transform.position, Npc.mNpc.mThrowPoint.transform.position + deltPos, Npc.GetDamage());
        }
    }

    Vector3 CalcBulletDestPos(int Index)
    {
        Vector3 v = Vector3.right;
        switch (Index)
        {
            case 1:
                v.x = 1; v.y = 1;
                break;
            case 2:
                v.x = 0; v.y = 1;
                break;
            case 3:
                v.x = -1; v.y = 1;
                break;
            case 4:
                v.x = -1; v.y = 0;
                break;
            case 5:
                v.x = -1; v.y = -1;
                break;
            case 6:
                v.x = 0; v.y = -1;
                break;
            case 7:
                v.x = 1; v.y = -1;
                break;
        }
        v = v.normalized * 30;
        return v;
    }

}

public class CAirFireNpcBeAtkAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        Npc.mCurState = CNpcInst.eState.MoveToTarget;
    }
}

public class CAirFireNpcDyingAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mAirNpcDieSE);
        o.transform.position = Npc.GetRefMid().transform.position;
    }

    public override void Update(float T, CNpcInst Npc)
    {
        Npc.UpdateDying(T);
    }
}

//黄金巢穴
public class CGoldCaoNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    GameObject mLockObj;
    float mDelayT;
    float mV = 30;
    float mSleepT = 0;


    public override void Update(float T, CNpcInst Npc)
    {
        if (!mInit)
        {
            //sleep.
            if (mSleepT == 0)
            {
                mSleepT = gDefine.CalcCaoSleepT();
            }

            if (Time.time < mSleepT)
            {
                Vector3 initpos = gDefine.GetPCTrans().position + Vector3.up * 100;
                Npc.mNpc.SetPos(initpos);
                return;
            }

            mInit = true;
            mDelayT = 1.5f;
            mLockObj = GameObject.Instantiate(gDefine.gData.mCaoLockSEPreb);
            Vector3 pos = gDefine.GetPCTrans().position;
            pos.y = gDefine.gGrounY;
            mLockObj.transform.position = pos;
            pos.y += 30;
            Npc.mNpc.SetPos(pos);
            Npc.mNpc.Play("down");
        }

        Npc.mCanBeSelect = false;

        mDelayT -= Time.deltaTime;
        if (mDelayT < 0)
        {
            Vector3 pos = Npc.GetPos();
            pos.y -= Time.deltaTime * 30;
            if (pos.y <= gDefine.gGrounY)
            {
                pos.y = gDefine.gGrounY;

                Npc.mCurState = CNpcInst.eState.Idle;
                //
                GameObject o = GameObject.Instantiate(gDefine.gData.mCaoHitGroundSEPreb);
                o.transform.position = pos;
                //
                if (
                   Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
                   Mathf.Abs(gDefine.GetPCTrans().position.y - Npc.GetPos().y) < 3)
                {
                    gDefine.PcBeAtk((int)(Npc.GetDamage()));
                }

                Npc.mCurState = CNpcInst.eState.Idle;
                Npc.mCanBeSelect = true;

                GameObject.Destroy(mLockObj);
                Npc.mNpc.Play("idle");
            }
            Npc.mNpc.SetPos(pos);
        }
    }
}

public class CGoldCaoNpcIdleAI : NpcAICom
{

}

//空中自爆怪
public class CAirBombNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    float mDelayT;
    float mUpV = 10;
    float mComMoveV = 5;
    float mFollowV = 7.5f;
    float mCloseL = 1.8f; //追逐开始距离
    int mState = 0; //0 出生，飞往正常高度 //1飞向女主方向 //2 加速追踪女主  //3 自曝提示  //4 自爆
    float mLiveT = 0;
    float mRandSinRaind = 0;


    public override void Update(float T, CNpcInst Npc)
    {
        if (!mInit)
        {
            mInit = true;
            mState = 0;
            if (Npc.GetPos().y >= gDefine.gAirY)
                mState = 1;
            mRandSinRaind = Random.Range(0, Mathf.PI * 2);
        }

        mLiveT += Time.deltaTime;

        Npc.BeginBABA();
        Vector3 pos = Npc.GetPos();
        if (mState == 0)
        {
            pos.y += Time.deltaTime * mUpV;
            if (pos.y >= gDefine.gAirY)
            {
                pos.y = gDefine.gAirY;
                mState = 1;
            }
            Npc.mNpc.SetPos(pos);

        }
        else if (mState == 1)
        {
            if (gDefine.GetPCTrans().position.x > pos.x)
                pos.x += mComMoveV * Time.deltaTime;
            else
                pos.x -= mComMoveV * Time.deltaTime;

            pos.y = gDefine.gAirY + Mathf.Sin(mLiveT * Mathf.PI * 3 + mRandSinRaind) * 0.5f;
            if (Mathf.Abs(pos.x - gDefine.GetPCTrans().position.x) < 4)
            {
                mState = 2;
            }
            Npc.mNpc.SetPos(pos);

        }
        else if (mState == 2)
        {
            Vector3 pcPos = gDefine.GetPcRefMid().position;
            Vector3 npcPos = Npc.GetPos();
            pcPos.z = npcPos.z;
            Vector3 npos = Vector3.MoveTowards(npcPos, pcPos, Time.deltaTime * mFollowV);
            Npc.mNpc.SetPos(npos);

            if (Vector3.Distance(pcPos, npos) < 0.2f)
            {
                mState = 3;
                Npc.mNpc.Play("alarm");
                mDelayT = 1.0f;
            }

        }
        else if (mState == 3)
        {
            mDelayT -= Time.deltaTime;
            if (mDelayT < 0)
            {
                //..damage..
                if (
                  Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
                  Mathf.Abs(gDefine.GetPCTrans().position.y - Npc.GetPos().y) < 3)
                {
                    gDefine.PcBeAtk((int)(Npc.GetDamage()));
                }

                //..se bomb..
                //GameObject o = GameObject.Instantiate(gDefine.gData.mAirNpcDieSE);
                //o.transform.position = Npc.mNpc.mRefMidPoint.transform.position;

                //
                Npc.mCurState = CNpcInst.eState.Dying;
                Npc.mNpc.Play("die");

            }

        }
    }
}


public class CAirBombDyingAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mNpc空中自爆怪爆炸特效Preb);
        o.transform.position = Npc.GetRefMid().transform.position;
        Npc.mCanBeSelect = false;
    }

    public override void Update(float T, CNpcInst Npc)
    {
        Npc.UpdateDying(T);
    }
}



//空中散花怪
// public class CAirZuZhouNpcMoveToTargetAI : NpcAICom
// {
//     bool mInit = false;
//     float mDelayT;
//     float mUpV = 10;
//     float mComMoveV = 4;
//     float mFollowV = 8;
//     float mCloseL = 1.8f; //追逐开始距离
//     int mState = 0; //0 出生，飞往正常高度 //1飞向女主方向 //2 加速追踪女主  //3 自曝提示  //4 自爆


//     public override void Update(float T, CNpcInst Npc)
//     {
//         if (!mInit)
//         {
//             mInit = true;
//             mState = 0;
//             if (Npc.GetPos().y >= gDefine.gGrounY)
//                 mState = 1;
//         }

//         Npc.BeginBABA();
//         Vector3 pos = Npc.GetPos();
//         if (mState == 0)
//         {
//             pos.y += Time.deltaTime * mUpV;
//             if (pos.y >= gDefine.gAirY)
//             {
//                 pos.y = gDefine.gAirY;
//                 mState = 1;
//             }
//             Npc.mNpc.SetPos(pos);

//         }
//         else if (mState == 1)
//         {
//             if (gDefine.GetPCTrans().position.x > pos.x)
//                 pos.x += mComMoveV * Time.deltaTime;
//             else
//                 pos.x -= mComMoveV * Time.deltaTime;
//             if (Mathf.Abs(pos.x - gDefine.GetPCTrans().position.x) < 4)
//             {
//                 mState = 2;
//             }
//             Npc.mNpc.SetPos(pos);

//         }
//         else if (mState == 2)
//         {
//             Vector3 pcPos = gDefine.GetPcRefMid().position;
//             Vector3 npcPos = Npc.GetPos();
//             pcPos.z = npcPos.z;
//             Vector3 npos = Vector3.MoveTowards(npcPos, pcPos, Time.deltaTime * mFollowV);
//             Npc.mNpc.SetPos(npos);

//             if (Vector3.Distance(pcPos, npos) < mCloseL)
//             {
//                 mState = 3;
//                 Npc.mNpc.Play("alarm");
//                 mDelayT = 1.0f;
//             }

//         }
//         else if (mState == 3)
//         {
//             mDelayT -= Time.deltaTime;
//             if (mDelayT < 0)
//             {
//                 //..damage..
//                 if (
//                   Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
//                   Mathf.Abs(gDefine.GetPCTrans().position.y - Npc.GetPos().y) < 3)
//                 {
//                     gDefine.PcBeAtk((int)(Npc.GetDamage()));
//                 }

//                 //..se bomb..
//                 //GameObject o = GameObject.Instantiate(gDefine.gData.mAirNpcDieSE);
//                 //o.transform.position = Npc.mNpc.mRefMidPoint.transform.position;

//                 //
//                 Npc.mCurState = CNpcInst.eState.Dying;
//                 Npc.mNpc.Play("die");

//             }

//         }
//     }
// }

//空中散花怪
public class CAirFlowerNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    Vector3 mDestPos;
    float mComMoveV = 4;
    float mFollowV = 8;
    float mSkillT = 0;

    public override void Update(float T, CNpcInst Npc)
    {
        if (!mInit)
        {
            mInit = true;
            CalcNewDestPos();
            mSkillT = Random.Range(3.0f, 6.0f) + Time.time;
        }

        Npc.BeginBABA();
        Vector3 pos = Npc.GetPos();
        if (Mathf.Abs(gDefine.GetPCTrans().position.x - mDestPos.x) > 19)
            CalcNewDestPos();

        Vector3 npos = Vector3.MoveTowards(Npc.GetPos(), mDestPos, Time.deltaTime * mComMoveV);
        Npc.mNpc.SetPos(npos);
        if (Vector3.Distance(npos, mDestPos) < 0.01f)
        {
            CalcNewDestPos();
        }

        if (Time.time > mSkillT)
        {
            int num = Random.Range(2, 4);
            for (int i = 0; i < num; i++)
            {
                GameObject o = GameObject.Instantiate(Npc.mNpc.mThrowItemPreb);
                Npc_AirZuZhouDieBomb s = o.GetComponent<Npc_AirZuZhouDieBomb>();
                Vector3 bPos = Npc.mNpc.mRefMidPoint.transform.position;
                Vector3 ePos = bPos + Random.Range(-5.0f, 5.0f) * Vector3.right;
                ePos.y = gDefine.gGrounY;
                s.Init(bPos, ePos, 1.5f, Npc.GetDamage());
                mSkillT = Random.Range(3.0f, 6.0f) + Time.time;
            }
        }
    }

    void CalcNewDestPos()
    {
        Vector3 pos = gDefine.GetPCTrans().position;
        pos.x += Random.Range(-6f, 6f);
        pos.y = gDefine.gAirY;
        mDestPos = pos;
    }

}


public class CAirZuZhouNpcDyingAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mAirNpcDieSE);
        o.transform.position = Npc.GetRefMid().transform.position;
        Npc.mCanBeSelect = false;

        //散花
        int num = Random.Range(3, 5);
        for (int i = 0; i < num; i++)
        {
            o = GameObject.Instantiate(Npc.mNpc.mThrowItemPreb);
            Npc_AirZuZhouDieBomb s = o.GetComponent<Npc_AirZuZhouDieBomb>();
            Vector3 bPos = Npc.mNpc.mRefMidPoint.transform.position;
            Vector3 ePos = bPos + Random.Range(-5.0f, 5.0f) * Vector3.right;
            ePos.y = gDefine.gGrounY;
            s.Init(bPos, ePos, 1.5f, Npc.GetDamage());
        }
    }

    public override void Update(float T, CNpcInst Npc)
    {
        Npc.UpdateDying(T);
    }
}


//巢穴
public class CCaoNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    GameObject mLockObj;
    float mDelayT;
    float mV = 30;


    public override void Update(float T, CNpcInst Npc)
    {
        if (!mInit)
        {
            mInit = true;
            mDelayT = 1.5f;
            mLockObj = GameObject.Instantiate(gDefine.gData.mCaoLockSEPreb);
            Npc.mNpc.mQuitDestoryObj = mLockObj;
            Vector3 pos = gDefine.GetPCTrans().position;
            pos.y = gDefine.gGrounY;
            mLockObj.transform.position = pos;
            pos.y += 30;
            Npc.mNpc.SetPos(pos);
            Npc.mNpc.Play("down");

            Npc.BeginBABA();
        }
        Npc.mCanBeSelect = false;

        mDelayT -= Time.deltaTime;
        if (mDelayT < 0)
        {
            Vector3 pos = Npc.GetPos();
            pos.y -= Time.deltaTime * 30;
            if (pos.y <= gDefine.gGrounY)
            {
                pos.y = gDefine.gGrounY;

                Npc.mCurState = CNpcInst.eState.Idle;
                //
                GameObject o = GameObject.Instantiate(gDefine.gData.mCaoHitGroundSEPreb);
                o.transform.position = pos;
                //
                if (
                   Mathf.Abs(gDefine.GetPCTrans().position.x - pos.x) < 2 &&
                   Mathf.Abs(gDefine.GetPCTrans().position.y - Npc.GetPos().y) < 3)
                {
                    gDefine.PcBeAtk((int)(Npc.GetDamage()));
                }

                Npc.mCurState = CNpcInst.eState.Idle;
                Npc.mCanBeSelect = true;

                GameObject.Destroy(mLockObj);

                gDefine.PlayVibrate();

                Npc.mNpc.Play("idle");
            }
            Npc.mNpc.SetPos(pos);
        }
    }
}

public class CCaoNpcIdleAI : NpcAICom
{
    float mSkillT = 0;

    public override void Init(CNpcInst Npc)
    {
        mSkillT = Random.Range(4.0f, 6.0f);
    }

    public override void Update(float T, CNpcInst Npc)
    {
        mSkillT -= Time.deltaTime;
        if (mSkillT <= 0)
        {
            Npc.mNpc.Play("skill");
            mSkillT = Random.Range(4.0f, 6.0f);
        }
    }

    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        Vector3 pos = Npc.mNpc.mThrowPoint.transform.position;

        npcdata.eNpcType npcType = npcdata.eNpcType.AirBomb;

        GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcType));

        CNpcInst npcInst = gDefine.gNpc.CreateNpcInst(npcType);

        npcInst.Init(npc, npcType, pos, gDefine.gLogic.mWaveLvL);

        gDefine.gNpc.AddNpc(npcInst);
    }

}



//空中激光怪
public class CAirLightNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    float mSkillT;
    float mCloseL = 4;
    float mV = 4;
    int mState = 0; //0 move.  //1 fire.
    Vector3 mSkillBpos, mSkillEpos;
    Vector3 mAtkPos;

    Npc_AirCom mScript;
    bool mDamageDone = false;
    float mState1L = 0;
    float mState1T = 0;

    float mState2T = 0;


    public override void Update(float T, CNpcInst Npc)
    {
        if (!mInit)
        {
            mInit = true;
            mSkillT = Random.Range(5.0f, 7.0f);
            Npc.BeginBABA();
            mState = 0;
            mScript = Npc.mNpc.gameObject.GetComponent<Npc_AirCom>();
        }
        //Npc.mCanBeSelect = false;

        mSkillT -= Time.deltaTime;
        if (mSkillT < 0)
        {
            mSkillT = Random.Range(5.0f, 7.0f);
            mState = 1;
            mDamageDone = false;
            mState1L = 3;
            mState1T = Time.time;

            mSkillBpos = Npc.GetPos();
            mSkillBpos.y = gDefine.gGrounY;
            if (gDefine.GetPCTrans().position.x > mSkillBpos.x)
            {
                mSkillBpos = mSkillBpos + Vector3.right * 0.5f;
                mSkillEpos = mSkillBpos + 7f * Vector3.right;
                mAtkPos = mSkillBpos;
            }
            else
            {
                mSkillBpos = mSkillBpos - Vector3.right * 0.5f;
                mSkillEpos = mSkillBpos - 7f * Vector3.right;
                mAtkPos = mSkillBpos;
            }

            gDefine.PlaySound(49);
        }

        if (mState == 0)
        {
            Vector3 pcPos = gDefine.GetPCTrans().position;
            Vector3 selfPos = Npc.GetPos();
            pcPos.y = selfPos.y;
            pcPos.z = selfPos.z;
            if (Vector3.Distance(pcPos, selfPos) > 2.5f)
            {
                selfPos = Vector3.MoveTowards(selfPos, pcPos, Time.deltaTime * mV);
                Npc.mNpc.SetPos(selfPos);
            }
        }
        else if (mState == 1)
        {
            if (Time.time >= mState1T + 0.2f)
            {
                mState = 2;
                mState2T = Time.time;
            }
            else
            {
                float dletT = 1.0f - (Time.time - mState1T) / 0.2f;
                float L = dletT * mState1L;
                mScript.ShowAim(Npc.GetPos().x, L);
            }
        }
        else if (mState == 2)
        {
            if (Time.time > mState2T + 0.3f)
            {
                mState = 3;
            }
            else
            {
                float perc = (Time.time - mState2T) / 0.25f;
                mScript.ShowHead(perc);
            }
        }
        else if (mState == 3)
        {
            Vector3 pos = Vector3.MoveTowards(mAtkPos, mSkillEpos, Time.deltaTime * 4);
            mScript.Show(pos);

            if (Vector3.Distance(pos, mSkillEpos) < 0.01f)
            {
                mScript.Close();
                mState = 0;
            }
            //do damage..
            Vector3 pcPos = gDefine.GetPCTrans().position;

            if (((pcPos.x >= mAtkPos.x && pcPos.x <= pos.x) || (pcPos.x <= mAtkPos.x && pcPos.x >= pos.x))
                   && Mathf.Abs(gDefine.GetPCTrans().position.y - gDefine.gGrounY) < 3)
            {

                if (!mDamageDone)
                {
                    gDefine.PcBeAtk((int)(Npc.GetDamage()));
                    mDamageDone = true;
                }
            }

            mAtkPos = pos;
        }
    }
}


public class CAirLightDyingAI : NpcAICom
{
    public override void DoSkillEvent(int CustomEventValue, CNpcInst Npc)
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mLaserNpcDieSE);
        o.transform.position = Npc.GetRefMid().transform.position;
        Npc.mCanBeSelect = false;
    }

    public override void Update(float T, CNpcInst Npc)
    {
        Npc_AirCom script = Npc.mNpc.gameObject.GetComponent<Npc_AirCom>();
        script.Close();
        Npc.UpdateDying(T);
    }
}


//下半身逻辑
//原地站立死亡，
//向前移动死亡

public class CHalfDownNpcMoveToTargetAI : NpcAICom
{
    bool mInit = false;
    Vector3 mDestPos;

    float mV = 1.0f;

    float mDeltX = 3;

    float mLiveT = 0;

    public override void Update(float T, CNpcInst Npc)
    {
        Npc.mCanBeSelect = false;
        Npc.mFaceIsControlByAI = true;
        Vector3 pos = Npc.GetPos();
        if (mInit == false)
        {
            // if (Random.Range(0, 100) < 40)
            // {
            //       Npc.mCurState = CNpcInst.eState.Idle;
            //       Npc.mNpc.Play("idle",false);
            // }
            // else
            {
                mV = 0.8f + Random.Range(-0.3f, 0.3f);

                if (gDefine.GetPCTrans().position.x > pos.x)
                    mDestPos += pos + Vector3.right * Random.Range(0.6f, 1.2f);
                else
                    mDestPos += pos + Vector3.left * Random.Range(0.6f, 1.2f);

                mDestPos.y = gDefine.gGrounY;

            }

            mLiveT = Time.time + 3.0f;
            mInit = true;

        }

        pos = Vector3.MoveTowards(pos, mDestPos, Time.deltaTime * mV);
        pos.y = gDefine.gGrounY;
        Npc.mNpc.SetPos(pos);

        Npc.mNpc.FaceRight(pos.x < mDestPos.x ? true : false);

        if (Vector3.Distance(pos, mDestPos) < 0.01f || Time.time > mLiveT)
        {
            Npc.mCurState = CNpcInst.eState.Dying;
            Npc.mNpc.Play("die", false);
            Npc.mDyingT = 0;
            //GameObject.Destroy(Npc.mNpc.gameObject);
        }
    }

}

public class CHalfDownIdleAI : NpcAICom
{
    bool mInit = false;
    float mIdleT = 0;

    public override void Update(float T, CNpcInst Npc)
    {
        if (mInit == false)
        {
            mIdleT = Random.Range(0.6f, 1.2f);
            mInit = true;
        }
        mIdleT -= Time.deltaTime;
        if (mIdleT <= 0)
        {
            Npc.mNpc.Play("die");
            Npc.mCurState = CNpcInst.eState.Dying;
        }
    }

}








