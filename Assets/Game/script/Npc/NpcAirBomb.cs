using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAirBomb : MonoBehaviour
{
    Vector3 mLockPos;
    int mIndex;
    public GameObject mRefLockPreb;
    GameObject mLockObj;
    bool mIsGo = false;
    public float mV = 40;
    public GameObject mSEBombPreb;
    int mDamage;

    bool mIsInAlram = false;
    float mAlramT = 0;

    float mLToDest = 1000;

    bool mInit = false;

    bool mDirR = true;


    // Update is called once per frame
    void Update()
    {
        if (!mInit)
            return;

        if (mIsGo)
        {
            if (mIsInAlram)
            {
                if (Time.time > mAlramT)
                {
                    GameObject o = GameObject.Instantiate(mSEBombPreb);
                    o.transform.position = mLockPos;

                    //..do damage..
                    if (
                        Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 2.2f &&
                         Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
                    {
                        gDefine.PcBeAtk(mDamage);
                    }
                    GameObject.Destroy(mLockObj);
                    GameObject.Destroy(gameObject);

                    gDefine.PlayVibrate();

                    
                }
            }
            else
            {

                Vector3 pos = Vector3.MoveTowards(transform.position, mLockPos, Time.deltaTime * mV);
                transform.position = pos;
                if (Vector3.Distance(pos, mLockPos) < 0.01f)
                {
                    pos.y = gDefine.gGrounY;
                    transform.position = pos;

                    mIsInAlram = true;
                    mAlramT = Time.time + 3.0f;

                    mLockObj = GameObject.Instantiate(mRefLockPreb);
                    mLockObj.transform.position = pos;

                }

            }



            // Vector3 pos = Vector3.MoveTowards(transform.position, mLockPos, Time.deltaTime * mV);
            // transform.position = pos;
            // if (Vector3.Distance(pos, mLockPos) < 0.01f)
            // {
            //     GameObject o = GameObject.Instantiate(mSEBombPreb);
            //     o.transform.position = mLockPos;

            //     //..do damage..
            //     if (
            //         Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 2 &&
            //          Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
            //     {
            //         gDefine.PcBeAtk(mDamage);
            //     }
            //     GameObject.Destroy(mLockObj);
            //     GameObject.Destroy(gameObject);
            // }
        }
        else
        {
            
            float l = Mathf.Abs(transform.position.x - mLockPos.x);
            //if (l > mLToDest)
            if((mDirR && transform.position.x > mLockPos.x) || 
            (!mDirR && transform.position.x < mLockPos.x) )
            {
                if(mIsGo == false && mIndex == 0)
                    gDefine.PlaySound(88);
                mIsGo = true;
                transform.SetParent(null);
                Vector3 pos = transform.position;
                pos.x = mLockPos.x;

                
            }
            else
            {
                mLToDest = l;
            }
        }
    }

    public void ShowTargetLock(Vector3 Pos, int Damage, int Index)
    {
        if (!mInit)
        {
            mLockPos = Pos;
            mDamage = Damage;
            mInit = true;
            mLToDest = Mathf.Abs(transform.position.x - Pos.x);
            mIndex = Index;
            mDirR = Pos.x > transform.position.x;
        }

    }


}
