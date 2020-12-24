using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet0SkillBullet : MonoBehaviour
{
    public float mV = 15f;
    int mDamageIs2 = 1;
    Vector3 mEPos;
    int mState = 0; // 0 move 1 bomb 2 died
    CNpcInst mNpc;
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if (mState == 0)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, mEPos, Time.deltaTime * mV);
            transform.position = pos;

            CNpcInst[] npc = gDefine.gNpc.FindAllByR(transform.position, 0.8f);
            if (npc.Length > 0)
            {
                mNpc = npc[0];
                //transform.SetParent(npc[0].GetRefMid().transform);   //= npc[0].GetHitSEPos();
                transform.position = npc[0].GetRefMid().transform.position;
                Animator anim = gameObject.GetComponent<Animator>();
                anim.Play("hit");
                mState = 1;
                //transform.localScale = Vector3.one * 2;

                npc[0].BeDamage((int)(gDefine.gPlayerData.mDamage * 1.2f * mDamageIs2), false, false, false, false, CSkill.eSkill.Null);
            }
            else if( Vector3.Distance( transform.position , mEPos) < 0.01f )
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
            }
        }
        else if(mState ==1)
        {
            if(mNpc.IsLive())
            {
                transform.position = mNpc.GetRefMid().transform.position;
            }
        }
    }

    public void Init(Vector3 BPos, Vector3 EPos, bool IsDamage2)
    {
        transform.position = BPos;
        mEPos = EPos;
        mDamageIs2 = IsDamage2 ? 2 : 1;

        Vector3 dir = EPos - BPos;
        transform.up = dir;

        mState = 0;

    }
}

