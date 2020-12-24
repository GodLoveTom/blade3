using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_se_BlackMask : MonoBehaviour
{
    public delegate void CallBack();
    CallBack mCallBackFunc;
    float mT;
    public float mLastT = 2f;
    public float mBlackDelayT = 1;
    public Image mImage;

    enum eType
    {
        Null,
        BlackToClear,
        ToBalckToClear,
    }

    eType mType = eType.Null;

    public void ShowBlackToClear(CallBack CallBackFunc)
    {
        mType = eType.BlackToClear;
        mCallBackFunc = CallBackFunc;
        mT = Time.time;
        gameObject.SetActive(true);
        mImage.color = new Color(0, 0, 0, 1);

    }

    public void PlayToBlackToDisapper(CallBack CallBackFunc)
    {
        mType = eType.ToBalckToClear;
        mCallBackFunc = CallBackFunc;
        mImage.color = Color.clear;
        gameObject.SetActive(true);
        mT = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (mType == eType.BlackToClear)
        {
            if (Time.time >= mT + mLastT +mBlackDelayT)
            {
                mCallBackFunc();
                gameObject.SetActive(false);
            }
            else if( Time.time < mT + mBlackDelayT)
            {
                 mImage.color = Color.black;
            }
            else
            {
                float a = 1.0f - (Time.time - mT-mBlackDelayT) / mLastT;
                mImage.color = new Color(0, 0, 0, a);
            }

        }
        else if (mType == eType.ToBalckToClear)
        {
            if (Time.time >= mT + mLastT * 2 + mBlackDelayT)
            {
                mCallBackFunc();
                gameObject.SetActive(false);
            }
            else
            {
                if (Time.time < mT + mLastT)
                {
                    float a = (Time.time - mT) / mLastT;
                    mImage.color = new Color(0, 0, 0, a);
                }
                else if (Time.time < mT + mLastT + mBlackDelayT)
                {
                    mImage.color = new Color(0, 0, 0, 1);
                }
                else
                {
                    float a = 1.0f - (Time.time - mT - mLastT - mBlackDelayT) / mLastT;
                    mImage.color = new Color(0, 0, 0, a);
                }
            }
        }
    }
}
