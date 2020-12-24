using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCom : MonoBehaviour
{
    // enum eState
    // {
    //     Idle = 0,
    //     MoveToTarget,
    //     Atk,
    //     Dying,//---kick off
    //     Died,
    //     BeAtk,
    //     Dying2,
    //     DyingFlash,
    //     Dying2KickFly, //有限的带减速的击飞
    //     BeKilled,
    // }
    // public NpcResData mRefRes; //资源索引
    // //public SpriteRenderer mRefSprite;  //实例索引
    // //public SpriteRenderer mRefSESprite; //特效那个
    // public npcUI mRefNpcUI;

    // eState mCurState = eState.Idle; //当前状态
    // float mIdleT = 0; //发呆时间
   
    // float mMoveV = 4.0f; //移动速度
    // bool mIsBaBa = false; //当前是否为霸体状态

    // //--动画--
    // float mCurActT = 0;  //当前动作流逝时间
    // float mNextActUpdateT = 1.6f; //下次期待的更新时间
    // int mFrameIndex = 0;  //当前动画帧数

    // //--击飞--
    // Vector3 mKickBackPos;
    // Vector3 mDying2V = new Vector3(1,3,0); //   抛起速度
    // float mDying2Acc = -2; // 引力
    // static float mDying2Y = 0;  //地平线高度
    // float mDying2T = 0; // 当前的死亡闪烁时间
    // const float mDying2FlashT = 0.4f; //死亡闪烁间隔时间

    // CNpcDefine.eNpcActType mCurAct = CNpcDefine.eNpcActType.Idle;

    // //--dying2 kick fly-----
    // float mDying2KickFlyV = 1;
    // float mDying2KickFlyAcc = 1;

    // //--npc属性---
    // int mHp = 4000;
    // int mMaxHp = 4000;
    // int mDamage = 3000;

    // //--dying flash--
    // int mDyingFlashNum = 0;
    // float mDyingFlashT = 0;

    // public bool mIsPosion = false;

    // DamageShowCtrlInCreature mDamageShow = new DamageShowCtrlInCreature();
    // List<CDelayDamage> mDelayDamage = new List<CDelayDamage>();

    // public GameObject mRefMidPoint;

    // float mFronzeT = 0;

    // public float GetMaxHp()
    // {
    //     return mMaxHp;
    // }
    // public GameObject GetRefMid()
    // {
    // return mRefNpcUI.mRefMidPoint;
    // }

    // public void Fronze(float FronzeT)
    // {
    // mFronzeT = Time.time + FronzeT;
    // }

    // public Vector3 GetHitSEPos()
    // {
    //     return mRefNpcUI.gameObject.transform.position + Vector3.up * 1.5f;
    // }

    // public Vector3 GetPos()
    // {
    //     return mRefNpcUI.gameObject.transform.position;
    // }

    // public bool IsDiedState()
    // {
    //     return mCurState == eState.Died;
    // }

    // public bool CanKilledPlay()
    // {
    //     int r = Random.Range(0, 100);
    //     return r < 30 ? true : false;
    // }

    // public bool IsLive()
    // {
    //     return mHp > 0;
    // }

    // public void DestorySelfInst()
    // {
    //     GameObject.Destroy(mRefNpcUI.gameObject);
    // }

    // public void Init( GameObject Obj , NpcResData Res)
    // {
    //     mRefRes = Res;
    //     mRefNpcUI = Obj.GetComponent<npcUI>();
    //     //mRefSprite = Obj.GetComponent<SpriteRenderer>();
    //     //mRefSESprite = Obj.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
    //     mRefNpcUI.mSESprite.gameObject.SetActive(false);
    //     mCurState = eState.MoveToTarget;
    //     mCurAct = CNpcDefine.eNpcActType.Move;
    //     mFrameIndex = 0;
    //     mNextActUpdateT = 0;
    //     mCurActT = Random.Range(0, mRefRes.mAct[(int)mCurAct].GetSumT());
    //     mMaxHp = mHp = 10000;
    //     if (gDefine.gNpcIsWood)
    //     {
    //         mHp = 4000000;
    //         mMaxHp = mHp;
    //     }

    //     mDamage = 3000;

    //     mRefNpcUI.Refresh(mHp, mMaxHp);
             

    //     RefreshSprite();
    // }

    // void RefreshSprite()
    // {
    //     SpriteActNodeInst node = mRefRes.mAct[(int)mCurAct].GetFrame(mFrameIndex);

    //     if( mCurState == eState.BeKilled )
    //     {
    //         mRefNpcUI.mBeKilledSprite.sprite = node.mS;

    //         Transform trans = gDefine.GetPCTrans();
    //         mRefNpcUI.mBeKilledSprite.flipX = trans.position.x > mRefNpcUI.gameObject.transform.position.x ? true : false;

    //         mRefNpcUI.mSESprite.gameObject.SetActive(false);
    //         mRefNpcUI.mBeKilledSprite.gameObject.SetActive(true);
    //         mRefNpcUI.mSprite.color = new Color(1, 1, 1, 0);

    //     }
    //     else
    //     {
    //         mRefNpcUI.mSprite.sprite = node.mS;

    //         Transform trans = gDefine.GetPCTrans();

    //         mRefNpcUI.mSprite.flipX = trans.position.x > mRefNpcUI.gameObject.transform.position.x ? true : false;


    //         if (node.mSE == null)
    //             mRefNpcUI.mSESprite.gameObject.SetActive(false);
    //         else
    //         {
    //             mRefNpcUI.mSESprite.sprite = node.mSE;
    //             mRefNpcUI.mSESprite.gameObject.SetActive(true);
    //             mRefNpcUI.mSESprite.flipX = mRefNpcUI.mSprite.flipX;
    //         }

    //         mRefNpcUI.mBeKilledSprite.gameObject.SetActive(false);
    //     }
    // }

    // void CalcCurActDelayDamage()
    // {
    //     List<CSpriteActionEvent> e = mRefRes. mAct[(int)mCurAct].GetDelayDamageEventArr();
    //     for (int i = 0; i < e.Count; i++)
    //     {
    //         CDelayDamage d = new CDelayDamage();

    //         d.mDelayT = (long)(e[i].mT * 10000000) + System.DateTime.Now.Ticks;

    //         if (e[i].mEvent == SpriteActNode.eEvent.HAtk)
    //         {
    //             d.mDamage = (int)(gDefine.gPlayerData.mDamage * 2);
    //             d.mIsHeavy = true;
    //         }
    //         else
    //             d.mDamage = (int)gDefine.gPlayerData.mDamage;

    //         mDelayDamage.Add(d);

    //         if (e[i].mEvent == SpriteActNode.eEvent.DAtk)
    //         {
    //             CDelayDamage d1 = new CDelayDamage();
    //             d1.mDelayT = (long)(e[i].mT  * 10000000) + System.DateTime.Now.Ticks;
    //             if (e[i].mEvent == SpriteActNode.eEvent.HAtk)
    //             {
    //                 d1.mDamage = (int)(gDefine.gPlayerData.mDamage * 2);
    //                 d1.mIsHeavy = true;
    //             }
    //             else
    //                 d1.mDamage = (int)gDefine.gPlayerData.mDamage;
    //             mDelayDamage.Add(d1);
    //         }
    //     }
    // }


    // void UpdateDelayDamage()
    // {
    //     //如果位置到位，则计算伤害的触发。
    //     //如果没到位，等待。
    //     Transform t = gDefine.GetPCTrans();

    //     if (Mathf.Abs(mRefNpcUI.gameObject.transform.position.x - t.position.x) < 2 )
    //     {
    //         for (int i = 0; i < mDelayDamage.Count; i++)
    //         {
    //             if (System.DateTime.Now.Ticks >= mDelayDamage[i].mDelayT)
    //             {
    //                 //do damage..
    //                 Transform pc = gDefine.GetPCTrans();
    //                 if (Mathf.Abs(pc.position.x - mRefNpcUI.gameObject.transform.position.x) < 2)
    //                 {
    //                     gDefine.PcBeAtk(mDamage);
    //                 }
    //                 mDelayDamage.RemoveAt(i);
    //                 i--;
    //             }
    //         }
    //     }
    // }


    // /// <summary>
    // /// 检查并看是否能够进入被打状态
    // /// 目前被击飞0.4
    // /// </summary>
    // public void CheckToBeAtkState()
    // {
        
    //     //如果没有别的情况，其实就是霸体，那么就进入被打状态
    //     if (!mIsBaBa)
    //     {
    //         mCurState = eState.BeAtk;
    //         mCurAct = CNpcDefine.eNpcActType.BeAtk;
    //         mCurActT = 0;
    //         mFrameIndex = 0;
    //         mIdleT = 0;
    //         mNextActUpdateT = 0;

    //         //被向后打飞一点点距离
    //         Vector3 deltPos = mRefNpcUI.gameObject.transform.position - gDefine.GetPCTrans().position;
    //         deltPos.y = deltPos.z = 0;
    //         deltPos.x = deltPos.x > 0 ? 0.4f: - 0.4f ;
    //         mKickBackPos = mRefNpcUI.gameObject.transform.position + deltPos;
    //     }

    // }

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
    //         Vector3 deltPos = mRefNpcUI.gameObject.transform.position - gDefine.GetPCTrans().position;
    //         deltPos.y = deltPos.z = 0;
    //         deltPos.x = deltPos.x > 0 ? 50 : -50;
    //         mKickBackPos = mRefNpcUI.gameObject.transform.position + deltPos;
    //     }

    // }

    // public void CheckToDying2State()
    // {
    //     //直接被打飞
    //     {
    //         mCurState = eState.Dying2;
    //         mCurAct = CNpcDefine.eNpcActType.Die;
    //         mCurActT = 0;
    //         mFrameIndex = 0;
    //         mIdleT = 0;
    //         mNextActUpdateT = 0;
    //         Transform tPc = gDefine.GetPCTrans();
    //         if (mRefNpcUI.gameObject.transform.position.x < tPc.position.x)
    //         { 
    //             mDying2V = new Vector3(-7, 20, 0); //   抛起速度

    //             mDying2V = new Vector3(-12, 25, 0); //   抛起速度
    //         }
    //         else
    //         {            
    //             mDying2V = new Vector3(12,  25, 0); //   抛起速度
    //         }


    //         //mDying2Acc = gDefine.GetEdit_Dying2_DownAcc(); 
    //         mDying2Acc = -60;
    //         mDying2Y = mRefNpcUI.gameObject.transform.position.y;

    //         //20,-60,7
    //         //    mDying2Acc = -60;
    //     }

    // }

    // public void CheckToKilledPlay()
    // {
    //     mHp = 0;
    //     mCurAct = CNpcDefine.eNpcActType.BeKilled;
    //     mCurActT = 0;
    //     mNextActUpdateT = 0;
    //     mCurState = eState.BeKilled;

    //     mRefNpcUI.Close();
    // }

    

    // public void BeDamage(int Damage, bool IsHeavy = false)
    // {
    //     CSkill skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.DeathFinger);
    //     if( skill !=null && Time.time > skill.mParamFloat + 1 )
    //     {
    //         if( Random.Range(0,100)<10)
    //         {
    //             mHp = 0;

    //             skill.mParamFloat = Time.time;
    //             //立刻死亡
    //             CheckToDying2State();

    //             mRefNpcUI.Close();

    //             gDefine.gFollowCam.PlayVibrate();

    //             Transform t = gDefine.GetPCTrans();

    //             //显示技能名称
    //             gDefine.gDamageShow.CreateDamageShow(skill.mName, t.position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
                
    //             return;
    //         }
    //     }

    //     //skill = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.CriticalAtk);
    //     if( !IsHeavy )
    //     {
    //         if( Random.Range(0,100)< gDefine.gPlayerData.mDoubleDamagePerc)
    //         {
    //             Damage *= 2;
    //             IsHeavy = true;
    //         }
    //     }

    //     if(!mIsPosion)
    //     {
    //         CSkill s = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.Poison);
    //         if(s!=null)
    //         {
    //             GameObject o = GameObject.Instantiate(gDefine.gData.mPosionSEPreb);
    //             se_Skill_Posion script = o.GetComponent<se_Skill_Posion>();
    //             script.Init( GetRefMid().transform, this);
    //             mIsPosion = true;
    //         }
    //     }

    //     //mHp =  mHp-Damage < 0 ?  (mIsBaBa?1:0):mHp-Damage;
    //     mHp = mHp - Damage < 0 ? 0 : mHp - Damage;
    //     mRefNpcUI.Refresh(mHp, mMaxHp);

    //     Vector3 pos = mRefNpcUI.GetDamageShowPos();
    //     pos.z -= 0.5f;

    //     //gDefine.gDamageShow.CreateDamageShow(Damage, pos, Color.white, IsHeavy);
    //     mDamageShow.Add(Damage, pos, Color.white, IsHeavy);

    //     if ( mHp >0 )
    //     {
    //         //被打状态
    //         CheckToBeAtkState();
    //     }
    //     else
    //     {
    //         //死亡状态
    //         CheckToDying2State();

    //         mRefNpcUI.Close();

    //         gDefine.gFollowCam.PlayVibrate();

    //         //Handheld.Vibrate();

    //         //gDefine.gPlayerData.AddEXP(75);
    //     }
    // }

    // public void Update(float T)
    // {
    //     if( Time.time < mFronzeT )
    //         return;
    
    //     if (gDefine.gNpcIsWood)
    //         mCurState = eState.Idle;

    //     //走向玩家，等待攻击，如果可以攻击则攻击（被打状态除外，需要被打状态解除，一旦解除，则霸体）
    //     switch( mCurState)
    //     {
    //         case eState.Idle:
    //             UpdateIdle(T);
    //             break;
    //         case eState.MoveToTarget:
    //             UpdateMoveToTarget(T);
    //             break;
    //         case eState.Atk:
    //             break;
    //         case eState.Dying:
    //             UpdateDying(T);
    //             break;
    //         case eState.Died:
    //             return; //死亡直接返回
               
    //         case eState.BeAtk:
    //             UpdateBeAtk(T);
    //             break;
    //         case eState.Dying2:
    //             UpdateDying2(T);
    //             break;
    //         case eState.DyingFlash:
    //             UpdateDyingFlash(T);
    //             break;
    //         case eState.BeKilled:
    //             break;
    //     }

    //     UpdateSprite(T);
    //     RefreshSprite();
    //     mDamageShow.Update();
    //     UpdateDelayDamage();
    // }

    // void UpdateIdle(float T)
    // {
    //     if (gDefine.gNpcIsWood)
    //         return;
    //     //计算时间流逝，时间到了，追踪或攻击玩家
    //     mIdleT -= T;
    //     if(mIdleT<0)
    //     {
    //         Transform t = gDefine.GetPCTrans();
    //         float l = Mathf.Abs( t.position.x - mRefNpcUI.gameObject.transform.position.x ) ;
    //         if (Mathf.Abs(l - 1.8f) < 0.001f)
    //         {
    //             //在攻击范围内，是否进行攻击？
    //             if (gDefine.gRand.Next(0, 100) < 40)
    //             {
    //                 mIsBaBa = true;
    //                 mCurState = eState.Atk;
    //                 mCurAct = CNpcDefine.eNpcActType.ComAtk;
    //                 mCurActT = 0;
    //                 mFrameIndex = 0;
    //                 mNextActUpdateT = 0;

    //                 //do damage.
    //                 //gDefine.gPc.BeAtk(1500,0.45f);
    //                 CalcCurActDelayDamage();
    //             }
    //             else
    //                 mIdleT = Random.Range(1, 2.5f);
    //         }
    //         else
    //         {
    //             //转换到追踪状态
    //             mCurState = eState.MoveToTarget;
    //             mCurAct = CNpcDefine.eNpcActType.Move;
    //             mCurActT = 0;
    //             mNextActUpdateT = 0;
    //             mFrameIndex = 0;
    //         }
    //     }
    // }

    

    // void UpdateMoveToTarget(float T)
    // {
    //     Transform t = gDefine.GetPCTrans();
    //     float l = Mathf.Abs(t.position.x - mRefNpcUI.gameObject.transform.position.x);
    //     if(Mathf.Abs(l-1.8f)<0.001f)
    //     {
    //         //直接转向站立状态
    //         mCurState = eState.Idle;
    //         mCurAct = CNpcDefine.eNpcActType.Idle;
    //         mCurActT = 0;
    //         mFrameIndex = 0;
    //         mIdleT = 0;
    //         mNextActUpdateT = 0;
    //     }
    //     else
    //     {
            
    //         Vector3 endPos = t.position;
    //         if (endPos.x > mRefNpcUI.gameObject.transform.position.x)
    //             endPos.x -= 1.8f;
    //         else
    //             endPos.x += 1.8f;

    //         endPos = Vector3.MoveTowards(mRefNpcUI.gameObject.transform.position, endPos, T * mMoveV);

    //         mRefNpcUI.gameObject.transform.position = endPos;

    //     }
    // }

    // /// <summary>
    // /// 被打状态，通常此时被击飞一段距离。目前默认是20倍的速度被击飞
    // /// </summary>
    // /// <param name="T"></param>
    // void UpdateBeAtk(float T)
    // {    
    //     Vector3 pos =
    //        Vector3.MoveTowards(mRefNpcUI.gameObject.transform.position, mKickBackPos, T * mMoveV * 20);

    //     mRefNpcUI.gameObject.transform.position = pos;
    // }

    // /// <summary>
    // /// 通常是击飞状态
    // /// </summary>
    // /// <param name="T"></param>
    // void UpdateDying(float T)
    // {
    //     Vector3 pos =
    //       Vector3.MoveTowards(mRefNpcUI.gameObject.transform.position, mKickBackPos, T * mMoveV * 3);

    //     mRefNpcUI.gameObject.transform.position = pos;

    //     if( pos == mKickBackPos )
    //     {
    //         //切换到彻底死亡状态
    //         mCurState = eState.Died;
    //         mRefNpcUI.gameObject.SetActive(false);
    //     }
    // }

    // /// <summary>
    // /// 通常是抛起状态
    // /// </summary>
    // /// <param name="T"></param>
    // void UpdateDying2(float T)
    // {
    //     Vector3 pos = mRefNpcUI.gameObject.transform.position;
    //     mDying2V.y += T * mDying2Acc;
    //     pos += mDying2V * T;
    //     if( pos.y <= mDying2Y )
    //     {
    //         //落地
    //         pos.y = mDying2Y;
    //         //kick off..
    //         //mCurState = eState.Dying2KickFly;
    //         //先转向闪烁结束

    //         //mCurState = eState.DyingFlash;
    //         //mDyingFlashNum = 0;
    //         // mDyingFlashT = 0;

    //         mCurState = eState.Died;
    //         mRefNpcUI.gameObject.SetActive(false);
    //         return;
    //     }
    //     mRefNpcUI.gameObject.transform.position = pos;
    // }

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
    //             mRefNpcUI.gameObject.SetActive(false);
    //             return;
    //         }
    //     }

    //     if( mDyingFlashNum == 2 || mDyingFlashNum == 4|| mDyingFlashNum == 6)
    //     {
    //         mRefNpcUI.mSprite.color = new Color(1, 1, 1, 1);
    //     }
    //     else
    //         mRefNpcUI.mSprite.color = new Color(1, 1, 1, 0);

    // }


    // void UpdateSprite(float deltT)
    // {

    //     if (mCurActT + deltT < mNextActUpdateT)
    //     {
    //         mCurActT += deltT;
    //         return;
    //     }
    //     else
    //         mCurActT += deltT;

    //     //先算第几个循环
        
    //     if (!mRefRes. mAct[(int)mCurAct].UpdateT(ref mCurActT, ref mFrameIndex, ref mNextActUpdateT))
    //     {
    //         if (mCurState == eState.Dying || mCurState == eState.Dying2
    //             || mCurState == eState.DyingFlash)
    //             return;

    //         if(mCurState == eState.BeKilled)
    //         {
    //             //演出结束
    //             mRefNpcUI.gameObject.SetActive(false);
    //             mCurState = eState.Died;
    //             mNextActUpdateT = 100000;
    //             gDefine.KilledPlayEnd();
    //             return;
    //         }


    //         //如果是攻击动作，呼叫伤害等的回调

    //         if( mCurAct == CNpcDefine.eNpcActType.ComAtk )
    //         {
    //             //攻击结束，霸体取消
    //             mIsBaBa = false;

    //            //Transform t = gDefine.GetPCTrans();
    //            //if( Mathf.Abs(t.position.x - mRefNpcUI.gameObject.transform.position.x) < 2 )
    //            //{
    //             //    gDefine.gPc.BeAtk(1500);
    //            //}
               
    //         }

    //         if (mHp > 0)
    //         {
    //             //切换向发呆
    //             mCurState = eState.Idle;
    //             mCurAct = CNpcDefine.eNpcActType.Idle;
    //             mCurActT = 0;
    //             mFrameIndex = 0;
    //             mIdleT = 0;
    //             mNextActUpdateT = 0;
    //         }
    //     }
    // }
}
