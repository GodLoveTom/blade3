using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_AirZuZhouDieBomb : MonoBehaviour
{
    public se_NpcFlowerBombDrop mRefDrop;
    Animator mAnimator;
    [Header("飞行速度")]
    public float mV = 8;
    [Header("投掷偏移高度")]
    public float mH = 2.5f;
    Vector3 mEndPos;
    Vector3 mOriPos;
    float mT;
    int mDamage;
    float mDamageR;

    enum eState
    {
        eFly,
        eAlarm,
    }
    eState mState = eState.eFly;
    public GameObject mAlarmObj;
    // Update is called once per frame
    void Update()
    {
        switch (mState)
        {
            case eState.eFly:
                if (mRefDrop.IsDropEnd())
                {
                    mState = eState.eAlarm;
                    mAnimator.Play("alarm");
                    mT = 0;
                    mAlarmObj.SetActive(true);
                }
                //  mT += Time.deltaTime;
                // float perc = mT * mV / Mathf.Abs(mEndPos.x - mOriPos.x) ;
                // if( perc >= 1.0f)
                // {
                //    mState = eState.eAlarm;
                //    mAnimator.Play("alarm");
                //    mT = 0;
                //    mAlarmObj.SetActive(true);
                //    transform.position = mEndPos;
                // }
                // else
                // {
                //     float x = mOriPos.x + (mEndPos.x - mOriPos.x)*perc;
                //     float y = mOriPos.y + (mEndPos.y - mOriPos.y)*perc +  ((perc<0.5f) ?  mH * perc : mH *(1-perc));

                //     transform.position = new Vector3( x, y, transform.position.z);
                // }

                break;
            case eState.eAlarm:
                mT += Time.deltaTime;
                if (mT > 2.0f)
                {
                    GameObject o = GameObject.Instantiate(gDefine.gData.mNpcRagePursuerBombFirePreb);
                    o.transform.position = transform.position;

                    gameObject.SetActive(false);
                    GameObject.Destroy(gameObject);

                    Vector3 pcPos = gDefine.GetPCTrans().position;
                    if (Mathf.Abs(pcPos.x - mEndPos.x) < mDamageR && pcPos.y - gDefine.gGrounY < 3.0f)
                    {
                        gDefine.PcBeAtk(mDamage);
                        gDefine.PcAddBuff(CBuff.eBuff.Curse, 10, 0);
                    }

                }
                break;
        }
    }

    public void Init(Vector3 OriPos, Vector3 EndPos, float DamageR, int Damage)
    {
        mOriPos = OriPos;
        mEndPos = EndPos;
        transform.position = mOriPos;
        mDamageR = DamageR;
        mDamage = Damage;

        mT = 0;
        mAnimator = GetComponent<Animator>();
        mAnimator.Play("fly");

        mState = eState.eFly;

        mRefDrop.Init();
    }
}
