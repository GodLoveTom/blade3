using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_ScaleBig : MonoBehaviour
{
    float mLastT = 0.2f;
    float mBT = 0f;
    public bool mIsGo = false;
    float mCurScale = 1;

    // Update is called once per frame
    void Update()
    {
        if (mIsGo)
        {
            float scale = 0.2f + (Time.time - mBT) / (mLastT * 0.85f);
            if (Time.time - mBT > mLastT)
            {
                 scale = 1f;
                 mIsGo = false;
            } 
            else if (Time.time - mBT > mLastT * 0.85f)
            {
                scale = 1f + (1.0f - (Time.time - mBT - mLastT * 0.85f) / (mLastT * 0.15f)) * 0.2f;
            }

            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    public void Play()
    {
        mBT = Time.time;
        transform.localScale = new Vector3(0.2f,0.2f,0.2f);
        mIsGo = true;
    }


}
