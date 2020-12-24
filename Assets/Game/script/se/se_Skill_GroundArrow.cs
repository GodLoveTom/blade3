using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_GroundArrow : MonoBehaviour
{
    [Header("存在时间")]
    public float mLiveT = 10;
    float mT = 0;
    [Header("激活后存在时间")]
    public float mActiveLiveT = 3;
    float mActiveT;
    bool mIsActive;
    [Header("伤害系数")]
    public float mDamageParam = 0.5f;
    float mDamage;
    public GameObject mArrowSEPreb;
    [Header("触发距离")]
    public float mFindL;
    [Header("攻击间隔")]
    public float mDamageSprareT;
    float mDamageT = 0;
    bool mFaceRight = true;
    Animator mAnimator;

    [Header("参考点")]
    public GameObject mRefPoint;

    [Header("炮")]
    public GameObject mRefGunObj;


    public void Event_PlaySound(int SoundId)
    {
        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if (clip != null)
            gDefine.gSound.Play(clip);
    }

    // Update is called once per frame
    void Update()
    {
        CNpcInst npc = gDefine.gNpc.FindByL(transform.position.x, mFindL, CNpcInst.eNpcClass.All);
        if (npc != null && !mIsActive)
        {
            mIsActive = true;
            mActiveT = Time.time + mActiveLiveT;
            mDamageT = Time.time - 0.0001f;
        }

        if (npc != null)
        {
            // if (npc.GetPos().x > transform.position.x)
            // {
            //     mFaceRight = true;
            //     transform.rotation = new Quaternion();
            // }
            // else
            // {
            //     mFaceRight = false;
            //     transform.rotation = Quaternion.Euler(0, 180, 0);
            // }

            Vector3 dir = npc.GetHitSEPos() - mRefGunObj.transform.position;
            dir.z = 0;
            dir.Normalize();

            mRefGunObj.transform.right = dir;
        }

        if (mIsActive)
        {
            if (Time.time > mActiveT)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
            }
            else
            {
                if (Time.time > mDamageT&&npc!=null)
                {
                    mDamageT = Time.time + mDamageSprareT;

                    mAnimator.Play("shoot");

                    GameObject o = GameObject.Instantiate(mArrowSEPreb);

                    se_Skill_GroundArrow_Arrow script = o.GetComponent<se_Skill_GroundArrow_Arrow>();

                    script.Init(mRefPoint.transform.position, npc.GetHitSEPos(), npc, (int)mDamage);
                }
            }
        }
        else
        {
            if (Time.time > mT)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
            }
        }

    }

    public void Init(Vector3 BPos, bool FaceRight)
    {
        mAnimator = GetComponent<Animator>();

        transform.position = BPos;
        mDamage = gDefine.gPlayerData.mDamage * mDamageParam;
        mT = Time.time + mLiveT;
        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.GroundArrow);
        if (d != null)
        {
            mT = mLiveT + Time.time + d.mLearnNum * 3;
            mDamage = gDefine.gPlayerData.mDamage * mDamageParam;
            mDamage += mDamage * 0.1f * d.mLearnNum;
        }


        mFaceRight = FaceRight;
        mDamageT = 0;



        //if(FaceRight)

        transform.rotation = new Quaternion();
        if (!FaceRight)
            transform.Rotate(0, 180, 0, Space.World);

        //transform.rotation = Quaternion.EulerAngles(0,Mathf.PI,0)  ;

        gameObject.SetActive(true);

        mIsActive = false;

    }
}
