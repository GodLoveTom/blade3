using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_PcHp : MonoBehaviour
{
    float mPerc;
    float mT;
    float mV = 1.0f;
    float mDestPerc;
    float mRealPerc = -1;

    public GameObject mHpImage;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > mT)
        {
            gameObject.SetActive(false);
            if( mRealPerc>0)
            {
                mPerc = mRealPerc;
                mRealPerc = -1;
                mHpImage.transform.localScale = new Vector3(mPerc, 1, 1);
            }
                
            return;
        }

        if (mDestPerc > mPerc)
        {
            mPerc = mPerc + Time.deltaTime * mV;
            if (mPerc > mDestPerc)
                mPerc = mDestPerc;
        }
        else
        {
            mPerc = mPerc - Time.deltaTime * mV;
            if (mPerc < mDestPerc)
                mPerc = mDestPerc;
        }

        mHpImage.transform.localScale = new Vector3(mPerc, 1, 1);
    }

    public void Refresh(float Perc, bool Show)
    {
        if (Mathf.Abs(Perc - mPerc) < 0.01f)
            return;
        if (Show)
        {
            mT = Time.time + 2;
            mDestPerc = Perc;
            gameObject.SetActive(true);
        }
        else
        {
            mPerc = Perc;
        }
        mV = 1;

    }

    public void JustPlay(float BPerc, float EPerc, float RealPerc, float V=1f)
    {
        mRealPerc = RealPerc;
        mPerc = BPerc;
        mDestPerc = EPerc;
        gameObject.SetActive(true);
        mT = Time.time + 2;
        mV = V;
    }
}
