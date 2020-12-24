using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_NpcSkill_Throw : MonoBehaviour
{
    [Header("Y轴偏离修正")]
    public float mMaxOffH;
    [Header("x方向扔出速度")]
    public float mVx;
    [Header("落地后特效")]
    public GameObject mHitSEPreb;
    [Header("伤害半径")]
    public float mAtkAreaR = 1.5f;

    Animator mAnimator;
    enum eState
    {
        Fly = 0,
        Alarm,
    }
    eState mState = eState.Fly;
    int mDamage;
    Vector3 mBPos;
    Vector3 mEPos;
    float mT = 0;
    CSkill.eSkill mSkillType;

    // Update is called once per frame
    void Update()
    {
        if (mState == eState.Fly)
        {
            mT += Time.deltaTime;
            float perc = mT * mVx / Mathf.Abs(mEPos.x - mBPos.x);
            if (perc >= 1.0f)
            {
                // if (mHitSEPreb != null)
                // {
                //     GameObject o = GameObject.Instantiate(mHitSEPreb);
                //     o.transform.position = mEPos;
                //     o.transform.localScale = Vector3.one * 2;
                // }

                // if (Mathf.Abs(transform.position.x - gDefine.GetPCTrans().position.x) < mAtkAreaR)
                // {
                //     gDefine.PcBeAtk(mDamage);
                // }

                // gameObject.SetActive(false);
                // GameObject.Destroy(gameObject);
                
                mT = Time.time + 1;
                mAnimator.Play("alarm");
                mState = eState.Alarm;

                transform.position = mEPos;

            }
            else
            {
                float x = mBPos.x + (mEPos.x - mBPos.x) * perc;
                float y = mBPos.y + (mEPos.y - mBPos.y) * perc + ((perc < 0.5f) ? mMaxOffH * perc : mMaxOffH * (1 - perc));

                transform.position = new Vector3(x, y, mBPos.z);
            }
        }
        else
        {
            if(Time.time > mT)
            {
                 if (mHitSEPreb != null)
                {
                    GameObject o = GameObject.Instantiate(mHitSEPreb);
                    o.transform.position = mEPos;
                    o.transform.localScale = Vector3.one * 2;
                }

                if (Mathf.Abs(transform.position.x - gDefine.GetPCTrans().position.x) < mAtkAreaR)
                {
                    gDefine.PcBeAtk(mDamage);
                    gDefine.PcAddBuff(CBuff.eBuff.Paralysis, 1,0);
                }

                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);

                gDefine.PlaySound(12);

            }
        }
    }

    public void Init(Vector3 BPos, Vector3 EPos, int Damage)
    {
        mT = 0;
        mDamage = Damage;
        transform.position = BPos;
        mBPos = BPos;
        mEPos = EPos;
        mEPos.y = gDefine.gGrounY;

        mState = eState.Fly;
        mAnimator = GetComponent<Animator>();
    }
}
