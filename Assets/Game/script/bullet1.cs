using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet1 : MonoBehaviour
{
    float mV;
    int mDamage;
    float mT = 0;

    public GameObject mRefHitSEPreb;

    List<CNpcInst> mDamgeArr = new List<CNpcInst>();
    Vector3 mEpos;


    // Update is called once per frame
    void Update()
    {
        Vector3 bpos = transform.position;
        Vector3 pos = Vector3.MoveTowards(transform.position, mEpos, mV * Time.deltaTime);
        transform.position = pos;

        Vector3 cpos = bpos;

        for (int i = 0; i <= 5; i++)
        {
            cpos = bpos + (pos - bpos) * 0.2f * i;
            DoDamage(cpos);
        }

        Vector3 dir = mEpos - bpos;
        dir.z = 0;
        gameObject.transform.right = dir;

        if (gameObject.activeSelf && Vector3.Distance(mEpos, transform.position) < 0.01f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

    }

    // void CalcAirDamge()
    // {
    //     CNpcInst inst = gDefine.gNpc.FindAirNpcByR(transform.position, 1);
    //     if (inst != null)
    //     {
    //         inst.BeDamage(mDamage, false, false, false);
    //         ShowHitSE(inst);

    //         if (inst.mComAtkIgnorPerc > 0)
    //         {
    //             inst.mComAtkIgnorPerc = 0;
    //             Vector3 pos0 = inst.mNpc.GetDamageShowPos();
    //             pos0.z -= 0.5f;
    //             inst.mDamageShow.Add("破甲", pos0, Color.gray);
    //         }

    //         gameObject.SetActive(false);
    //         GameObject.Destroy(gameObject);


    //     }
    // }

    // void CalcDamge(float X)
    // {
    //     CNpcInst[] Arr = gDefine.gNpc.DoDamageShoot(mBeginx, X, mDamage, mDamgeArr, CNpcInst.eNpcClass.All,
    //         false);
    //     for(int i=0; i<Arr.Length; i++)
    //     {
    //         mDamgeArr.Add(Arr[i]);
    //         ShowHitSE(Arr[i]);

    //        if( Arr[i].mComAtkIgnorPerc > 0 )
    //         {
    //             Arr[i].mComAtkIgnorPerc = 0;
    //              Vector3 pos0 = Arr[i].mNpc.GetDamageShowPos();
    //                 pos0.z -= 0.5f;
    //             Arr[i].mDamageShow.Add("破甲",pos0, Color.gray);
    //         }
    //     }
    // }

    void ShowHitSE(CNpcInst inst)
    {
        GameObject o = GameObject.Instantiate(mRefHitSEPreb);
        o.transform.position = inst.GetHitSEPos();
    }

    // public void Init( Vector3 BPos, bool FaceRight, float V,  int Damage)
    // {
    //     mIsFaceRight = FaceRight;

    //     mV = FaceRight ? V:-V;
    //     mDamage = Damage;
    //     mT = 0;
    //     mBeginx = gDefine.GetPCTrans().position.x;
    //     BPos.y += Random.Range(-0.3f, 0.3f);
    //     gameObject.transform.position = BPos;

    //     if(!FaceRight)
    //         GetComponent<SpriteRenderer>().flipX = true;
    // }

    public void Init(Vector3 BPos, Vector3 EPos, float V, int Damage)
    {

        mV = V;
        mDamage = Damage;
        mT = 0;

        gameObject.transform.position = BPos;

        mEpos = EPos;

        Vector3 pos = (gDefine.GetPcRefMid().transform.position + BPos);

        DoDamage(pos);

        Animator anim = GetComponent<Animator>();
        if (gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.FirePowerUp) != null)
            anim.Play("bullet1");
        else
        {
            anim.Play("bullet11");
        }
    }

    void DoDamage(Vector3 Pos)
    {
        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.GunWeapon];



        Vector3 pos = Pos;
        CNpcInst[] Arr = gDefine.gNpc.FindAllByR(pos, 0.8f);
        for (int i = 0; i < Arr.Length; i++)
        {
            if (!Exist(Arr[i]))
            {

                if (!Arr[i].mNpc.mIsBoss && Random.Range(0, 100) < gDefine.gPlayerData.mHeadShootPerc)
                {
                    //爆头
                    ShowHitSE(Arr[i]);
                    Arr[i].BeDamage(99999, false, false, false, true);
                    if (!Arr[i].IsLive() && gird != null && gird.mRefItem.mSpecialIndex == 2)
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            gDefine.gPlayerData.Coin += 100;
                            gDefine.CreateSomeCoinInGame(10, Arr[i].GetPos());
                            gDefine.gGainInFight.AddCoins(100, "装备");
                        }
                    }
                }
                else
                {
                    ShowHitSE(Arr[i]);
                    Arr[i].BeDamage(mDamage, false, true, false, true);
                    mDamgeArr.Add(Arr[i]);


                    if (gird != null && gird.mRefItem.mSpecialIndex == 1)
                    {
                        if (Arr[i].mComAtkIgnorPerc > 0 && Arr[i].IsLive())
                        {
                            Arr[i].mComAtkIgnorPerc = 0;
                            Vector3 pos0 = Arr[i].mNpc.GetDamageShowPos();
                            pos0.z -= 0.5f;
                            Arr[i].mDamageShow.Add("破甲", pos0, Color.gray);
                        }
                    }


                    if (!Arr[i].IsLive() && gird != null && gird.mRefItem.mSpecialIndex == 2)
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            gDefine.gPlayerData.Coin += 100;
                            gDefine.CreateSomeCoinInGame(10, Arr[i].GetPos());
                            gDefine.gGainInFight.AddCoins(100, "装备");
                        }
                    }
                }



            }
        }
    }

    bool Exist(CNpcInst Npc)
    {
        foreach (CNpcInst N in mDamgeArr)
            if (N == Npc)
                return true;
        return false;
    }



}
