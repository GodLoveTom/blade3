using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSkillPosionTrap : MonoBehaviour
{
    float mDamage;
    float mDamageT;
    float mLiveT;
    bool mIsClosed = false;


    // Update is called once per frame
    void Update()
    {
        if (Time.time > mLiveT && !mIsClosed)
        {
            Animator a = GetComponent<Animator>();
            a.Play("end");
            mIsClosed = true;
        }
        
        //else if (Time.time >= mDamageT)
        {
            Vector3 pcPos = gDefine.GetPCTrans().transform.position;
            if (Mathf.Abs(pcPos.x - transform.position.x) < 1.1f && Mathf.Abs(pcPos.y - gDefine.gGrounY) < 3)
            {
                gDefine.PcAddBuff(CBuff.eBuff.Posion, 10, (int)mDamage);
            }
            mDamageT = Time.time+0.25f;
        }

    }

    public void Init(Vector3 Pos, float Damage )
    {
        Pos.y = gDefine.gGrounY;
        transform.position = Pos;
        mLiveT = Time.time + 2;
        mDamageT = Time.time + 0.25f;
        mDamage = Damage;
    }

    void Event_Destory()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

}
