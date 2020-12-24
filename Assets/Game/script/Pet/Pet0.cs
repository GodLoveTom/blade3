using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet0 : MonoBehaviour
{
    [Header("眼睛参考点")]
    public GameObject mRefEyePoint;
    [Header("开火的火花")]
    public GameObject mRefFireSEObj;
    [Header("跟随距离最小")]
    public float mFollowLMinParam = 2;
    [Header("跟随距离最大")]
    public float mFollowLMaxParam = 3;
    [Header("跟随速度")]
    public float mFollowVParam = 8;
    [Header("攻击间隔时间s")]
    public float mAtkTParam = 6; // 攻击间隔时间
    float mAtkT = 0; // 攻击间隔时间
    [Header("子弹发射间隔时间s")]
    public float mBulletFireTParam = 0.3f;
    float mBulletFireT = 0; // 发射子弹间隔时间
    int mFireNum = 0; //攻击的子弹数量
    [Header("技能间隔时间s")]
    public float mSkillTParam = 5; // 攻击间隔时间 
    float mSkillT = 0;

    [Header("动画")]
    public Animator mAnim;
    Vector3 mFllowPos; // 这只是一个偏移，动态计算在每一个帧里

    GameObject mBulletPreb;
    GameObject mSkillBulletPreb;
    bool mIsDamageX2;
    public bool mIsSkillOn = false;

    enum eState
    {
        Idle,
        Follow,
    }
    eState mState = eState.Idle;
    float mStateT = 0;
    float mAtkCoolDown = 0; //攻击冷却，技能和普攻如果冲突，1s之内不连发


    // Update is called once per frame
    void Update()
    {
        mBulletFireTParam = 0.08f;
        if (mState == eState.Idle)
        {
            if (Time.time > mStateT || Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) > mFollowLMaxParam)
            {
                float x = Random.Range(mFollowLMinParam, (mFollowLMinParam + mFollowLMaxParam) / 2);
                mFllowPos = new Vector3(Random.Range(0, 100) < 50 ? x : -x, 0, 0);
                mState = eState.Follow;
            }
        }
        else if (mState == eState.Follow)
        {
            //if (!mIsSkillOn)
            {
                Vector3 pos = mFllowPos + gDefine.GetPCTrans().position;
                Vector3 npos = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * mFollowVParam);
                transform.position = npos;
                if (Vector3.Distance(pos, npos) < 0.01f)
                {
                    mState = eState.Idle;
                    mStateT = Random.Range(12, 16) + Time.time;
                }

            }

        }
        UpdateAtk();
        UpdateSkill();

    }
    void UpdateSkill()
    {
        if (Time.time > mSkillT + mSkillTParam)
        {
            mAnim.Play("skill", 0, 0);
            //Event_SkillAtk();
            mSkillT = Time.time;
        }


    }

    void UpdateAtk()
    {
        CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 7, CNpcInst.eNpcClass.All);
        if (mFireNum > 0)
        {
            if (Time.time > mBulletFireT + mBulletFireTParam)
            {
                mBulletFireT = Time.time + mBulletFireTParam;
                //fire
                mAnim.Play("fire", 0, 0);
                //
                if (mBulletPreb == null)
                    mBulletPreb = gDefine.gABLoad.GetPreb("obj.bytes", "Pet0Bullet");
                GameObject b = GameObject.Instantiate(mBulletPreb);
                Pet0Bullet s = b.GetComponent<Pet0Bullet>();

                Vector3 endPos;
                if (npc != null)
                {
                    endPos = mRefEyePoint.transform.position + (npc.GetHitSEPos() - mRefEyePoint.transform.position).normalized * 30;
                }
                else
                {
                    endPos = mRefEyePoint.transform.position + Vector3.right * 20 * ((gDefine.GetPCTrans().position.x > transform.position.x) ? -1 : 1);
                }

                Vector3 bPos = mRefEyePoint.transform.position + (endPos - mRefEyePoint.transform.position).normalized * 1;

                s.Init(bPos, endPos, mIsDamageX2);

                mRefFireSEObj.transform.right = (endPos - bPos).normalized;
                mFireNum--;
            }
        }
        else if (Time.time > mAtkTParam + mAtkT && npc != null)
        {

            mFireNum = 5;

            mAtkT = Time.time;

            gDefine.PlaySound(6);
        }
        else
        {
            mFireNum = 0;
        }

    }

    public void Init(Vector3 BPos, bool IsDamageX2)
    {
        BPos.x += Random.Range(0, 100) < 50 ? (mFollowLMinParam + mFollowLMaxParam) * 0.5f : -(mFollowLMinParam + mFollowLMaxParam) * 0.5f;
        transform.position = BPos;
        mIsDamageX2 = IsDamageX2;
        mSkillT = Time.time;
    }

    public void Event_SkillAtk()
    {

        if (mSkillBulletPreb == null)
            mSkillBulletPreb = gDefine.gABLoad.GetPreb("obj.bytes", "Pet0SkillBullet");

        Vector3 bPos = mRefEyePoint.transform.position;
        for (int i = 0; i < 7; i++)
        {
            float r = -Mathf.PI * 0.25f + Mathf.PI * 0.25f * i;

            float ey = Mathf.Sin(r) * 20;
            float ex = Mathf.Cos(r) * 20;

            Vector3 epos = bPos;
            epos.x += ex;
            epos.y += ey;

            GameObject bullet = GameObject.Instantiate(mSkillBulletPreb);
            Pet0SkillBullet s = bullet.GetComponent<Pet0SkillBullet>();
            s.Init(bPos, epos, mIsDamageX2);
        }
    }

    
}

