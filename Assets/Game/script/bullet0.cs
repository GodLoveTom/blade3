using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet0 : MonoBehaviour
{
    bool mIsFaceRight = true;
    float mV;
    int mDamage;
    float mT = 0;
    float mBeginx = 0;
    float mLineBx = 0;
    float mLineBy = 0;

    public GameObject mRefHitSEPreb;

    List<CNpcInst> mDamgeArr = new List<CNpcInst>();
    LineRenderer mLine;

    bool mIsAtkAir = false;
    Vector3 mBpos;
    Vector3 mEpos;
    CNpcInst mAimNpc; //指定的npc

    // Update is called once per frame
    void Update()
    {
        {
            if (mAimNpc != null && mAimNpc.IsLive())
            {
                mEpos = mAimNpc.GetHitSEPos();

            }
            Vector3 oldpos = transform.position;
            Vector3 pos = Vector3.MoveTowards(transform.position, mEpos, mV * Time.deltaTime);
            transform.position = pos;
            Vector3 dir = mEpos - mBpos;
            dir.z = 0;
            transform.right = dir;
            //CalcAirDamge();
            if (Vector3.Distance(mEpos, transform.position) < 0.01f)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);

                if (mAimNpc != null && mAimNpc.IsLive())
                {
                    Vector3 npcPos = mAimNpc.GetHitSEPos();
                    if (npcPos.y < mBpos.y)
                        npcPos.y = mBpos.y;

                    if (Vector3.Distance(npcPos, mEpos) < 1)
                    {
                        if (!mAimNpc.mNpc.mIsBoss && Random.Range(0, 100) < gDefine.gPlayerData.mHeadShootPerc)
                        {
                            //爆头
                            ShowHitSE(mAimNpc);
                            mAimNpc.BeDamage(99999, false, false, false);
                        }
                        else
                        {
                            ShowHitSE(mAimNpc);
                            mAimNpc.BeDamage(mDamage, false, false, false);


                            CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.GunWeapon];
                            if (mAimNpc.IsLive())
                            {
                                // if(Random.Range(0,100)<20)

                                if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 1
                                && Random.Range(0, 100) < 20)
                                {
                                    if (mAimNpc.mNpc.mIsInAir && mAimNpc.mNpcType != npcdata.eNpcType.AirBomb
                                    )
                                        mAimNpc.ChangetoAirFallDown();
                                    else
                                    {
                                        mAimNpc.AddBuff(CBuff.eBuff.Paralysis, 4);
                                    }
                                }

                            }

                            if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 2)
                            {
                                if (Random.Range(0, 100) <= 7)
                                {
                                    if (gDefine.gPlayerData.mHp < gDefine.gPlayerData.mHpMax)
                                    {
                                        int hpadd = (int)(gDefine.gPlayerData.mHpMax * 0.01f);
                                        if (hpadd < 1) hpadd = 1;
                                        gDefine.gPlayerData.mHp += hpadd;
                                        if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                                            gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                                        gDefine.ShowPcUIHp();
                                    }
                                }
                            }
                        }



                    }


                }

                return;

            }
            else if (mAimNpc == null)
            {
                CNpcInst[] nArr = gDefine.gNpc.FindByLine(oldpos.x, pos.x, CNpcInst.eNpcClass.All);
                if (nArr != null && nArr.Length > 0)
                {
                    CNpcInst n = nArr[0];
                    if (!n.mNpc.mIsBoss && Random.Range(0, 100) < gDefine.gPlayerData.mHeadShootPerc)
                    {
                        //爆头
                        ShowHitSE(n);
                        n.BeDamage(99999, false, false, false);
                    }
                    else
                    {
                        ShowHitSE(mAimNpc);
                        n.BeDamage(mDamage, false, false, false);


                        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.GunWeapon];
                        if (n.IsLive())
                        {

                            if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 1
                            && Random.Range(0, 100) < 20)
                            {
                                if (n.mNpc.mIsInAir && n.mNpcType != npcdata.eNpcType.AirBomb
                                )
                                    n.ChangetoAirFallDown();
                                else
                                {
                                    n.AddBuff(CBuff.eBuff.Paralysis, 4);
                                }
                            }

                        }

                        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 2)
                        {
                            if (Random.Range(0, 100) <= 7)
                            {
                                if (gDefine.gPlayerData.mHp < gDefine.gPlayerData.mHpMax)
                                {
                                    int hpadd = (int)(gDefine.gPlayerData.mHpMax * 0.01f);
                                    if (hpadd < 1) hpadd = 1;
                                    gDefine.gPlayerData.mHp += hpadd;
                                    if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                                        gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

                                    gDefine.ShowPcUIHp();
                                }
                            }
                        }
                    }
                }
            }
        }
        // if( !mIsAtkAir)
        // {
        //     mT += Time.deltaTime;
        //     if( mT > 1.0f || Mathf.Abs(mBeginx - transform.position.x) > 7.8f )
        //     {
        //         gameObject.SetActive(false);
        //         GameObject.Destroy(gameObject);
        //     }
        //     else
        //     {
        //         float x = gameObject.transform.position.x + mV * Time.deltaTime;
        //         transform.position = new Vector3(x, transform.position.y,
        //             transform.position.z);

        //         //calcDamage
        //         CalcDamge(x);
        //      }
        // }
        // else
        // {
        //     //air....
        //     Vector3 pos = Vector3.MoveTowards( transform.position, mEpos, mV*Time.deltaTime);
        //     transform.position = pos;
        //     Vector3 dir = mEpos- mBpos;
        //     dir.z = 0;
        //     gameObject.transform.right = dir;
        //     CalcAirDamge();
        //     if(gameObject.activeSelf && Vector3.Distance(mEpos, transform.position) < 0.01f )
        //     {
        //         gameObject.SetActive(false);
        //         GameObject.Destroy(gameObject);
        //     }

        // }

        ReSetLine();
    }

    // void CalcAirDamge()
    // {
    //     CNpcInst inst = gDefine.gNpc.FindAirNpcByR(transform.position, 1);
    //     if (inst != null)
    //     {
    //         inst.BeDamage(mDamage, false, false,false);
    //         ShowHitSE(inst);

    //         gameObject.SetActive(false);
    //         GameObject.Destroy(gameObject);

    //         if (inst.IsLive())
    //         {
    //             // if(Random.Range(0,100)<20)
    //             inst.AddBuff(CBuff.eBuff.Paralysis, 4);
    //         }
    //     }
    // }

    // void CalcDamge(float X)
    // {
    //     CNpcInst inst = gDefine.gNpc.DoSingleDamage(mBeginx, X, mDamage,false);
    //     if (inst != null)
    //     {
    //         ShowHitSE(inst);
    //         gameObject.SetActive(false);
    //         GameObject.Destroy(gameObject);

    //         if (inst.IsLive())
    //         {
    //             //if(Random.Range(0,100)<70)
    //             CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.GunWeapon];
    //             if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 1 )
    //             {
    //                 inst.AddBuff(CBuff.eBuff.Paralysis, 4);
    //             }
    //             else if(gird.mRefItem!=null && gird.mRefItem.mSpecialIndex == 2 )
    //             {
    //                 if( Random.Range(0,100)<=7)
    //                 {
    //                    if( gDefine.gPlayerData.mHp < gDefine.gPlayerData.mHpMax)
    //                    {
    //                         int hpadd = (int)(gDefine.gPlayerData.mHpMax * 0.01f);
    //                         if(hpadd<1) hpadd = 1;
    //                              gDefine.gPlayerData.mHp += hpadd;
    //                              if(gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
    //                                 gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

    //                         gDefine.ShowPcUIHp();
    //                    }
    //                 }
    //             }

    //             //GameObject o = GameObject.Instantiate(mRefXianJingHitSEPreb);
    //             //se_Skill_XianJingHit script = o.GetComponent<se_Skill_XianJingHit>();
    //             //script.Init(7, inst, 0 , 7 );
    //         }
    //     }
    // }

    void ShowHitSE(CNpcInst inst)
    {
        GameObject o = GameObject.Instantiate(mRefHitSEPreb);
        o.transform.position = inst.GetHitSEPos();
    }

    // public void Init(Vector3 BPos, bool FaceRight, float V, int Damage)
    // {
    //     mIsFaceRight = FaceRight;

    //     mV = FaceRight ? V : -V;
    //     mDamage = Damage / 2;
    //     mT = 0;
    //     mBeginx = gDefine.GetPCTrans().position.x;
    //     BPos.y += Random.Range(-0.2f, 0.2f);
    //     gameObject.transform.position = BPos;

    //     mLineBx = BPos.x;
    //     mLineBy = BPos.y;

    //     mLine = GetComponent<LineRenderer>();

    //     ReSetLine();

    // }

    // public void Init(Vector3 BPos, Vector3 EPos, bool FaceRight, float V, int Damage)
    // {
    //     mIsFaceRight = FaceRight;

    //     mV = V;
    //     mDamage = Damage / 2;
    //     mT = 0;
    //     mBeginx = gDefine.GetPCTrans().position.x;

    //     gameObject.transform.position = BPos;

    //     mLineBx = BPos.x;
    //     mLineBy = BPos.y;

    //     mBpos = BPos;
    //     mEpos = EPos;

    //     mLine = GetComponent<LineRenderer>();

    //     ReSetLine();

    //     mIsAtkAir = true;
    // }

    public void Init(Vector3 BPos, CNpcInst NpcInst, float V, int Damage, Vector3 EPos)
    {
        mEpos = BPos;

        Vector3 hitPos = (NpcInst != null) ? NpcInst.GetHitSEPos() : EPos;

        mIsFaceRight = hitPos.x > BPos.x ? true : false;

        mV = V;
        mDamage = Damage;
        mT = 0;
        mBeginx = gDefine.GetPCTrans().position.x;

        gameObject.transform.position = BPos;

        mLineBx = BPos.x;
        mLineBy = BPos.y;

        mBpos = BPos;
        mAimNpc = NpcInst;

        if (hitPos.y < mBpos.y)
            hitPos.y = mBpos.y;

        mEpos = hitPos;

        mLine = GetComponent<LineRenderer>();

        ReSetLine();

        mIsAtkAir = true;

        Animator anim = GetComponent<Animator>();
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.FirePowerUp) != null)
            anim.Play("bullet0");
        else
        {
            anim.Play("bullet00");
        }
    }

    void ReSetLine()
    {
        if (mLine == null)
            mLine = GetComponent<LineRenderer>();

        mLine.positionCount = 2;

        Vector3[] arr = new Vector3[2];
        arr[0].x = mLineBx;
        arr[0].y = mLineBy;
        arr[0].z = transform.position.z;

        arr[1].x = transform.position.x;
        arr[1].y = transform.position.y;
        arr[1].z = transform.position.z;

        mLine.SetPositions(arr);

    }
}
