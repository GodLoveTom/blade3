using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcStoryBoss : MonoBehaviour
{
    public GameObject mRefLasserBeginPos;
    public LineRenderer mLine;
    public GameObject mLineHitSE;
    public delegate void CallBackFunc();
    public GameObject mLasserObj;
    public float mKillLastT = 1.5f;
    CallBackFunc mCallBackFunc;
    float mKillT;
    public bool mIsInKill = false;
    public void PlayKill(CallBackFunc Func)
    {
        gameObject.SetActive(true);
        mCallBackFunc = Func;
        mLasserObj.SetActive(true);
        mKillT = Time.time + mKillLastT;
        mIsInKill = true;

        Vector3 [] posarr = new Vector3[2];
        posarr[0] = mRefLasserBeginPos.transform.position;
        posarr[1] = gDefine.GetPcRefMid().transform.position;

        mLine.SetPositions(posarr);

        mLineHitSE.transform.position = posarr[1];
        mLineHitSE.SetActive(true);

    }

    public void PlayGo(CallBackFunc Func)
    {
        mCallBackFunc = Func;
        Animator at = gameObject.GetComponent<Animator>();
        at.Play("go", 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (mIsInKill)
        {
            if (Time.time > mKillT )
            {
                mCallBackFunc();
                mIsInKill = false;
                mLasserObj.SetActive(false);
                mLineHitSE.SetActive(false);
            }
        }
    }

    public void Event_Go()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
        mCallBackFunc();

    }
}
