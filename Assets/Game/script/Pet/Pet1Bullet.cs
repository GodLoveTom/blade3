using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet1Bullet : MonoBehaviour
{
    public float mV = 15f;
    int mDamageIs2 = 1;
    Vector3 mEPos;
     bool mIsMove = true;
     
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
       if (Vector3.Distance(mEPos, transform.position) < 0.01f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
            return;
        }
        else if (mIsMove)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, mEPos, Time.deltaTime * mV);
            transform.position = pos;

            CNpcInst[] npc = gDefine.gNpc.FindAllByR(transform.position, 0.8f);
            if (npc.Length > 0)
            {
                transform.SetParent(npc[0].GetRefMid().transform);//= npc[0].GetHitSEPos();
                transform.localPosition = Vector3.zero;
                Animator anim = gameObject.GetComponent<Animator>();
                anim.Play("hit");
                mIsMove = false;
                //transform.localScale = Vector3.one * 2;

                npc[0].BeDamage((int)(gDefine.gPlayerData.mDamage * 0.4f * mDamageIs2), false, false, false, false, CSkill.eSkill.Null);
            }
        }
    }

    public void Init( Vector3 BPos, Vector3 EPos, bool IsDamage2)
    {
        transform.position = BPos;
        mEPos = EPos;
        mDamageIs2 = IsDamage2?2:1;

        Vector3 dir = EPos - BPos;
        transform.right = dir;
    }
}
