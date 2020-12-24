using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Npc毒飞弹新 : MonoBehaviour
{
    int mNextNum = 0;
    float mT;
    public GameObject mRefLock;
    Vector3 mDest;
    public float mV = 40;
    float mDamage;
    int mState = 0; // 0 wait // 1   drop // 2 bomb

    // Update is called once per frame
    void Update()
    {
        if (mState == 0)
        {
            if (Time.time >= mT)
            {
                mState = 1;
                mT = Time.time + 2.0f;
               
                Vector3 pos = gDefine.GetPCTrans().position;
                pos.y = gDefine.gGrounY;
                mRefLock = GameObject.Instantiate(gDefine.gData.mLockSEPreb);
                mRefLock.transform.localScale = new Vector3(2.0f,1,1);
                mRefLock.transform.position = pos;
                 mRefLock.SetActive(true);
                mDest = pos;
                pos.y += 50;
                transform.position = pos;
              
            }
        }
        else if (mState == 1)
        {
            mV += 50*Time.deltaTime;
            Vector3 pos = Vector3.MoveTowards(transform.position, mDest, mV * Time.deltaTime);
            transform.position = pos;
            mRefLock.transform.position = mDest;
            if (Vector3.Distance(pos, mDest) < 0.01f)
            {
                mState = 2;
                pos.y = gDefine.gGrounY;
                transform.position = pos;
                mRefLock.SetActive(false);
                GameObject.Destroy(mRefLock);

                Animator a = GetComponent<Animator>();
                a.Play("die");

                //伤害
                Vector3 pcPos = gDefine.GetPCTrans().position;
                if (Mathf.Abs(pcPos.x - pos.x) < 2f && Mathf.Abs(pcPos.y - gDefine.gGrounY) < 3.0f)
                {
                    gDefine.PcBeAtk((int)mDamage);
                }

                //
               
            }
        }
    }

    public void Init(float Damage, int Num, float DelayT)
    {
        mDamage = Damage;
        mNextNum = Num;
        mT = DelayT + Time.time;
        mState = 0;
        Vector3 pos = gDefine.GetPCTrans().position;
        pos.y += 50;

        transform.position = pos;
       
        Debug.Log("毒飞弹"+ Num.ToString());

    }

    void Event_End()
    {
        if (mNextNum > 0)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mNpcPosionBombPreb);
            se_Npc毒飞弹新 script = o.GetComponent<se_Npc毒飞弹新>();
            script.Init(mDamage, mNextNum - 1, 0.1f);
            mNextNum = 0;

        }

        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }
}
