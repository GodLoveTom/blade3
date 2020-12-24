using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpcAtkTip 
{
    enum eState{
        Close =0,
        PreAtkTip,
        AtkTip,
    }
    eState mState = eState.Close;
    GameObject mSELockObj;  //锁定特效
    CNpcInst mNpcInst; 
    float mPreAtkTipT = 0; //攻击提示计时=
    bool mShowTipSE = true;
    Vector3 mLockPos ; //当前锁定的位置
    public Vector3 GetPos()
    {
        return mLockPos;
    }

     /// <summary>
    /// 开始： 攻击前提示
    /// </summary>
    public void BeginLockTip( CNpcInst NpcInst,float TipT=2.0f, bool ShowTipSE=true)
    {
        mState = eState.PreAtkTip;
        mNpcInst = NpcInst;
        
        mPreAtkTipT = TipT;
        mNpcInst.BeginBABA();

        mShowTipSE = ShowTipSE;

        if( mNpcInst.mNpc.mIsAtkTipFollowPc)
        {
            mLockPos = gDefine.GetPCTrans().position;
            mLockPos.y = gDefine.gGrounY;    
           
        }
        else
        {
            mLockPos = mNpcInst.mNpc.mRefAtkPoint.transform.position;
            mLockPos.y = gDefine.gGrounY;
        }

        if( ShowTipSE)
        {
            mSELockObj = GameObject.Instantiate(gDefine.gData.mLockSEPreb);
            mSELockObj.transform.position = mLockPos;
            
        } 
    }

    public bool IsReady()
    {
        return mState == eState.PreAtkTip && mPreAtkTipT<0;
    }

    /// <summary>
    /// 更新攻击提示位置
    /// </summary>
    public void Update()
    {
        if( mState == eState.PreAtkTip  )
        {
            mPreAtkTipT -= Time.deltaTime;

            if( mNpcInst.mNpc.mIsAtkTipFollowPc)
            {
                Vector3 playerPos = gDefine.GetPCTrans().position;
                playerPos.y = gDefine.gGrounY;
                mLockPos = Vector3.MoveTowards( mLockPos,  playerPos, Time.deltaTime);
                mLockPos.y = gDefine.gGrounY;
            }
            else
            {
                mLockPos = mNpcInst.mNpc.mRefAtkPoint.transform.position;
                mLockPos.y = gDefine.gGrounY;
            }
            
            
            if(mShowTipSE)
            {
                mSELockObj.transform.position = mLockPos;
            }
        }
    }
    /// <summary>
    /// 将状态设置为：攻击中提示
    /// </summary>
    public void ChangeToAtkLockState()
    {
        　mState = eState.AtkTip;
    }

    public void EndLockTip()
    {
        mState = eState.Close;
        if(mSELockObj!=null)
        {
            mSELockObj.SetActive(false);
            GameObject.Destroy(mSELockObj);
        }
    }

  
}
