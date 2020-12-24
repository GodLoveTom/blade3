using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Gun : MonoBehaviour
{
    public Text mTText;
    float mT = 0;
    float mCoolDownT = 7;
    bool mIsInGun = false;
    public Image mCircleCtl;
    int mCurStep = 0;
    Animator mAnimator;


    // Update is called once per frame
    void Update()
    {
        if( Time.time >= mCoolDownT + mT )
        {
            if(mCurStep==0)
            {
                mCurStep = 1;
                 mAnimator.Play("step1",0);
            }
        }
        else
        {
            int t =(int)(mCoolDownT - ( Time.time - mT ));
            mTText .text = t.ToString();
            gDefine.SetTextBold();

            float perc = (mCoolDownT - ( Time.time - mT ))/ mCoolDownT;
            mCircleCtl.fillAmount = perc;
        }
    }

    public void Btn_UseGun()
    {
        if( Time.time >= mCoolDownT + mT && gDefine.gMecha != null)
        {
            gDefine.gMecha.UseMissile();

             mIsInGun = true;
            mAnimator.Play("step3",0);
            mT = Time.time;
             mCoolDownT=12;
        }
        else
        if( Time.time >= mCoolDownT + mT && gDefine.IsPCCanUseGun() && 
            gDefine.gPlayerData.mHp > 0 )
        {
            gDefine.UseGunGirlNow();
            mIsInGun = true;
            mAnimator.Play("step3",0);
            mT = Time.time;
            mCoolDownT=1;
        }
    }

    public void Show( bool Visible)
    {
        if(Visible)
        {
            mAnimator = GetComponent<Animator>();
            ReSetGunCoolDown();
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
        
    }

    public void ReSetGunCoolDown()
    {
        mIsInGun = false;
        mT = Time.time;
        CGird gird = gDefine.gPlayerData. mEquipGird[(int)gDefine.eEuqipPos.GunWeapon];
        if(gDefine.gMecha != null)
            mCoolDownT = 12;
        else if(gird.mRefItem != null && gird.mRefItem.mSubType == CItem.eSubType.LongGun)
            mCoolDownT = 12f;
        else
            mCoolDownT = 5f;

        if(gDefine.gLogic.mTeach.mIsInTeach)
            mCoolDownT = 0.01f;
        
        mCurStep = 0;
        mAnimator.Play("step0",0);
    }

    public void TeachReady()
    {
        gameObject.SetActive(true);
          mAnimator = GetComponent<Animator>(); 

        mIsInGun = false;
        mT = Time.time;
        mCoolDownT = 0.1f;
        mCurStep = 0;
        mAnimator.Play("step0",0);
    }
}
