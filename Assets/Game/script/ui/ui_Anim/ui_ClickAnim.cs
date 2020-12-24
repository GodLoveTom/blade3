//点击后收缩和恢复的动画
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_ClickAnim
{
    public GameObject mRefObj;
    float mLastT = 0.12f;
    float mMinScale = 0.66f;
    float mT;
    float mOriLocalScale;
    public delegate void CallBackFunc();
    CallBackFunc mFunc;
    public void Init(GameObject Obj, float OriLocalScale, CallBackFunc Func=null)
    {
        mT = 0;
        mRefObj = Obj;
        mOriLocalScale = OriLocalScale;
        mFunc = Func;
    }

    // Update is called once per frame
    public void Update()
    {
        if (mT < mLastT && mRefObj != null)
        {
            mT += Time.deltaTime;
            if (mT >= mLastT)
            {
                mRefObj.transform.localScale = Vector3.one * mOriLocalScale;
                mRefObj = null;
                 if(mFunc!=null)
                    mFunc();
            }
            else
            {
                if (mT < mLastT * 0.5f)
                {
                    //收缩
                    float perc = (1.0f - mT / (mLastT * 0.5f)) * (1 - mMinScale) + mMinScale;
                    Vector3 s = Vector3.one * mOriLocalScale * perc;
                    mRefObj.transform.localScale = s;
                   
                }
                else
                {
                    //扩张
                    float perc = (mT - mLastT*0.5f) / (mLastT * 0.5f) * (1 - mMinScale) + mMinScale;
                     Vector3 s = Vector3.one * mOriLocalScale * perc;
                    mRefObj.transform.localScale = s;
                }
            }
        }
    }
}
