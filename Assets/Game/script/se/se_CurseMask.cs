using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_CurseMask : MonoBehaviour
{
    public GameObject mSEObj;
    public float mAtkL = 3.5f;
    bool IsOpen = false;
    float mDamageT = 0;
    public NpcMono mRefNpc;
    float mPGWT = 0;
    float mT = 0; //8,5,10
    int mState = 0; //0 close  , 1 open
    public Animator mAnimator;

    public GameObject mPosionWavePreb;

    // Update is called once per frame
    void Update()
    {
        if (mRefNpc.mNpcInst != null && mRefNpc.mNpcInst.IsLive())
        {
            if (mT <= 0)
                mT = Time.time + 5;
            if (mPGWT <= 0)
                mPGWT = Time.time + 5.6f;

            if (!IsOpen)
            {
                if (Time.time > mT)
                {
                    mSEObj.SetActive(true);
                    mAnimator.Play("open", 0);
                    IsOpen = true;
                    mPGWT = 5.5f;
                    mT = Time.time + 8;
                }
            }
            else
            {
                if (Time.time >= mT)
                {
                    //mSEObj.SetActive(false);
                    mAnimator.Play("close", 0);
                    IsOpen = false;
                    mT = Time.time + 5;
                    return;
                }
                else
                {
                    mDamageT -= Time.deltaTime;
                    if (Time.time > mDamageT)
                    {
                        mDamageT = Time.time + 1;

                        Vector3 pcPos = gDefine.GetPCTrans().position;
                        Vector3 npcPos = mRefNpc.gameObject.transform.position;

                        if (Mathf.Abs(pcPos.y - npcPos.y) < 3 && Mathf.Abs(pcPos.x - npcPos.x) < mAtkL)
                        {
                            int Damage = (int)(gDefine.gPlayerData.mHpMax * 0.1f);
                            gDefine.PcBeAtk(Damage);
                        }
                    }

                    //wave
                    //mPGWT -= Time.deltaTime;
                    if (Time.time > mPGWT)
                    {
                        mPGWT = 5.5f + Time.time;

                        //向左波
                        GameObject wave = GameObject.Instantiate(mPosionWavePreb);
                        se_Npc_PosionWave script = wave.GetComponent<se_Npc_PosionWave>();

                        Vector3 bpos = transform.position;
                        bpos.y = gDefine.gGrounY;

                        script.Init(bpos, bpos + Vector3.left * 6, (int)(mRefNpc.mNpcInst.GetDamage() * 1.5f));

                        //向右波
                        wave = GameObject.Instantiate(mPosionWavePreb);
                        script = wave.GetComponent<se_Npc_PosionWave>();

                        script.Init(bpos, bpos + Vector3.right * 6, (int)(mRefNpc.mNpcInst.GetDamage() * 1.5f));


                    }
                }
            }
        }
    }

    void Event_Close()
    {
        mSEObj.SetActive(false);
    }

    public void Cancel()
    {
        if( IsOpen)
        {
            mAnimator.Play("close", 0);
            IsOpen = false;
            mT = Time.time + 10;
        }
    }
}
