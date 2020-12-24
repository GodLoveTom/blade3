using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainUpFlash : MonoBehaviour
{
    public Sprite [] mCoinSpriteArr; //金币闪烁列表
    public Sprite [] mCrystalSpriteArr; //水晶闪烁列表
    float mFlashT=-1; //闪烁时间间隔
    float mAnimT; //动画间隔
    bool mIsCoinFlash = true; //当前是否为金币在闪烁
    bool mIsFlashing = false; //当前是否在闪烁中 
    int mFrameIndex=0;
    const float mAnimSpareT = 0.1f;

    public Image mCoin;
    public Image mCrystal;

    // Update is called once per frame
    void Update()
    {
        gDefine.gUICoinPos = mCoin.transform.position;
        gDefine.gUICrystalPos = mCrystal.transform.position;

        if( mFlashT <= 0)
        {
            mFlashT = 1;//Random.Range(3.0f,5.0f);
        }

        if( mFlashT > 0 )
        {
            mFlashT -= Time.deltaTime;
            if(mFlashT<=0)
            {
                mIsFlashing = true;
                mIsCoinFlash = (Random.Range(0,100)<50)?true:false;
                mFrameIndex = 0;
                mAnimT = mAnimSpareT;
            }
        }

        if( mIsFlashing)
        {
            Image image = mCoin;
            Sprite [] arr = mCoinSpriteArr;
            if(!mIsCoinFlash)
            {
                image = mCrystal;
                arr = mCrystalSpriteArr;
            }

            mAnimT -= Time.deltaTime;
            if(mAnimT<=0)
            {
                mAnimT = mAnimSpareT;
                mFrameIndex ++;
                if( mFrameIndex >= arr.Length)
                {
                    mIsFlashing = false;
                    image.sprite = arr[0];
                    return;
                }
            }

            image.sprite = arr[mFrameIndex];
        }
    }
}
