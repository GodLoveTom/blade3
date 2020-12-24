/// <summary>
/// 战斗的教学
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teach
{
    public enum eTeachStep
    {
        Null,
        TipBtnClick,  //攻击点击提示
        CreateNpc0,  //刷兵
        CreateNpc1, //刷兵
        CheckAllDie, //检查是否攻击教学完成
        CreateBoss, //创建boss
        WaitToTipDodgeBtnClick, //等待提示躲避按钮
        TipDodgeBtnClick,//提示躲避按钮
        WaitToFreeBoss, // 0.5后释放boss
        CheckBossDied,  //检查boss是佛被击败
        CreateAirNpc, //创建空中
        WaitToTipFire, //等待空中
        TipFire,  //提示开火
        CheckAirAllDied, // 检查空中是否都已经死亡
    }
    public bool mIsInTeach = false;
    eTeachStep mTeachStep = eTeachStep.Null;
    float mT;

    CNpcInst mBoss;
    public bool mBossShouldPause = false;

    // Update is called once per frame
    public void Update()
    {
        switch (mTeachStep)
        {
            case eTeachStep.CreateNpc0:
                {
                    if (Time.time >= mT)
                    {
                        mTeachStep = eTeachStep.CreateNpc1;
                        CreateNpcLeft();
                        mTeachStep = eTeachStep.CheckAllDie;
                    }
                    break;
                }

            case eTeachStep.CheckAllDie:
                {
                    if (gDefine.gNpc.IsEmpty())
                    {
                        mTeachStep = eTeachStep.CreateBoss;
                        CreateBoss();
                    }
                    break;
                }
            case eTeachStep.WaitToFreeBoss:
                {
                    if (Time.time > mT)
                    {
                        mBoss.mNpc.Continue();
                        mTeachStep = eTeachStep.CheckBossDied;
                    }
                    break;
                }

            case eTeachStep.CheckBossDied:
                {
                    if (gDefine.gNpc.IsEmpty())
                    {
                        CreateAir();
                    }
                    break;
                }
            case eTeachStep.WaitToTipFire:
            {
                if(Time.time > mT)
                {
                    mTeachStep = eTeachStep.TipFire;
                    gDefine.gGameMainUI.ShowTipTeachFireClick();
                }
                break;
            }

            case eTeachStep.CheckAirAllDied:
                {
                    if (gDefine.gNpc.IsEmpty())
                    {
                        TeachIsOver();
                    }
                    break;
                }

        }

        // for(int i=0; i<mNpc.Count; i++)
        // {
        //     if(mNpc[i].IsLive())
        //         mNpc[i].Update(Time.deltaTime);
        //     else
        //     {
        //         mNpc[i].DestorySelfInst();
        //         mNpc.RemoveAt(i);
        //         i--;
        //     }
        // }


    }

    public void BeginTeach()
    {
        mIsInTeach = true;
        mTeachStep = eTeachStep.TipBtnClick;
        gDefine.gGameMainUI.ShowTipTeachBtnClick();
        TDGAMission.OnBegin("Teach0_0");
        
    }

    public void PlayerClick_AndNext()
    {
        if (mTeachStep == eTeachStep.TipBtnClick)
        {
            mTeachStep = 0;
            mT = Time.time + 1;
            CreateNpcRight();
        }
        else if (mTeachStep == eTeachStep.WaitToTipDodgeBtnClick)
        {
            gDefine.gPcDSword.mCurFaceRight = true;
            gDefine.gPcDSword.ResetDir();
            gDefine.gGameMainUI.ShowTipTeachDodgeClick();
            mTeachStep = eTeachStep.TipDodgeBtnClick;

            
        }
        else if (mTeachStep == eTeachStep.TipDodgeBtnClick)
        {
            PlayerChangeToDodge();
            mTeachStep = eTeachStep.WaitToFreeBoss;
            mT = Time.time + 0.5f;
            mBossShouldPause = false;
        }
        else if (mTeachStep == eTeachStep.TipFire)
        {
            PlayerFire();
        }
    }

    void PlayerFire()
    {
        gDefine.gGameMainUI.mRefGunUI.Btn_UseGun();
        mTeachStep = eTeachStep.CheckAirAllDied;
    }

    void PlayerChangeToDodge()
    {
        gDefine.gPcDSword.mTargetObj = mBoss;
        gDefine.gPcDSword.Btn_Click(false);

    }

    public void TeachIsOver()
    {
        mIsInTeach = false;
        gDefine.gIsFirstInGame = false;
        gDefine.gGameMainUI.EndTeach();
        TDGAMission.OnCompleted("Teach0_0");
    }

    public void BossAtkStop_Next()
    {
        mTeachStep = eTeachStep.TipDodgeBtnClick;
        gDefine.gGameMainUI.ShowTipTeachDodgeClick();
    }


    void CreateNpcLeft()
    {

        for (int i = 0; i < 3; i++)
        {
            CNpcInst inst = gDefine.gNpc.CreateNpcInst(npcdata.eNpcType.BareHand);
            GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcdata.eNpcType.BareHand));

            Vector3 pos = gDefine.GetPCTrans().position;
            pos.y = gDefine.gGrounY;
            pos.x -= 9 + i;
            inst.Init(npc, npcdata.eNpcType.BareHand, pos, 1);

            gDefine.gNpc.AddNpc(inst);
        }
    }

    void CreateNpcRight()
    {
        CNpcInst inst = gDefine.gNpc.CreateNpcInst(npcdata.eNpcType.BareHand);
        GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcdata.eNpcType.BareHand));

        Vector3 pos = gDefine.GetPCTrans().position;
        pos.y = gDefine.gGrounY;
        pos.x += 9;
        inst.Init(npc, npcdata.eNpcType.BareHand, pos, 1);

        //mNpc.Add(inst);
        gDefine.gNpc.AddNpc(inst);

        mTeachStep = eTeachStep.CreateNpc0;
        mT = Time.time + 1.5f;
    }

    void CreateBoss()
    {
        CNpcInst inst = gDefine.gNpc.CreateNpcInst(npcdata.eNpcType.CaiDao);
        GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcdata.eNpcType.CaiDao));
        npc.name = "caidao";
        Vector3 pos = gDefine.GetPCTrans().position;
        pos.y = gDefine.gGrounY;
        pos.x += 11;
        inst.Init(npc, npcdata.eNpcType.BareHand, pos, 1);

        inst.mHp *= 3;
        inst.mMaxHp *= 3;
        inst.mNpc.mBestAtkL = 3f;

        gDefine.gNpc.AddNpc(inst);

        gDefine.gPcDSword.mCurFaceRight = true;
        gDefine.gPcDSword.ResetDir();

        gDefine.gGameMainUI.ShowTeachBtnNull();

        mTeachStep = eTeachStep.WaitToTipDodgeBtnClick;

        mBossShouldPause = true;
        mBoss = inst;
    }

    void CreateAir()
    {
        CNpcInst inst = gDefine.gNpc.CreateNpcInst(npcdata.eNpcType.AirHitGround);
        GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcdata.eNpcType.AirHitGround));

        Vector3 pos = gDefine.GetPCTrans().position;
        pos.y = gDefine.gGrounY;
        pos.x += 10;
        inst.Init(npc, npcdata.eNpcType.AirHitGround, pos, 1);

        inst.mHp =  inst.mHp/2;
        inst.mMaxHp = inst.mMaxHp/2;
        inst.mDamage = 1;

        gDefine.gNpc.AddNpc(inst);

        gDefine.gGameMainUI.ShowTeachBtnNull();
        gDefine.gGameMainUI.mRefGunUI.TeachReady();

        mTeachStep = eTeachStep.WaitToTipFire;

        mT = Time.time+1.25f;
    } 

}


