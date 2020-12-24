/// <summary>
/// Buff
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBuff
{
    public enum eBuff
    {
        Paralysis = 0,//麻痹
        Posion, //毒
        Curse, //诅咒

    }

    public eBuff mBuffType = eBuff.Paralysis;
    public float mLiveT = 0;
    GameObject mSEObj;

    public delegate void BuffBegin_CallBack(eBuff BuffType);
    public delegate void BuffEnd_CallBack(eBuff BuffType);

    BuffBegin_CallBack mBuffBegin_CallBackFunc;
    BuffEnd_CallBack mBuffEnd_CallBackFunc;

    Transform mTrans;
    int mDamage;
    float mBombT; // 爆发时间，每次间隔1s

    public void Close()
    {
        if (mSEObj != null)
        {
            mSEObj.SetActive(false);
            GameObject.Destroy(mSEObj);
        }
        mLiveT = -1;
    }

    public void Update()
    {
        mLiveT -= Time.deltaTime;
        if (mLiveT <= 0)
        {
            if (mSEObj != null)
            {
                mSEObj.transform.SetParent(null);
                mSEObj.SetActive(false);
                GameObject.Destroy(mSEObj);
                mSEObj = null;
                if (mBuffEnd_CallBackFunc != null)
                    mBuffEnd_CallBackFunc(mBuffType);
            }
            mLiveT = 0;
        }
    }

    /// <summary>
    /// 玩家的buff更新
    /// </summary>
    public void UpdatePlayer()
    {
        Update();
        if (mLiveT > 0 && mDamage > 0)
        {
            mBombT -= Time.deltaTime;
            if (mBombT <= 0)
            {
                mBombT = 1.0f;
                gDefine.PcBeAtk(mDamage);
            }
        }
    }

    /// <summary>
    /// 创建一个buff
    /// </summary>
    /// <param name="BuffType">类型</param>
    /// <param name="ExistT">时间</param>
    /// <param name="Trans">挂载特效的节点</param>
    /// <param name="BFunc">buff开始回调函数</param>
    /// <param name="EFunc">buff结束回调函数</param>
    public void Refresh(eBuff BuffType, float ExistT, int Damage, Transform Trans, BuffBegin_CallBack BFunc, BuffEnd_CallBack EFunc)
    {
        mBuffType = BuffType;
        mLiveT += ExistT;
        mBuffBegin_CallBackFunc = BFunc;
        mBuffEnd_CallBackFunc = EFunc;
        mTrans = Trans;
        mDamage = Damage;
        mBombT = 0.0f;

        if (mSEObj == null)
        {
            switch (mBuffType)
            {
                case eBuff.Paralysis:
                    mSEObj = GameObject.Instantiate(gDefine.gData.mBuffParalysisPreb);
                    break;
                case eBuff.Posion:
                    mSEObj = GameObject.Instantiate(gDefine.gData.mBuffPosionPreb);
                    break;
                case eBuff.Curse:
                    mSEObj = GameObject.Instantiate(gDefine.gData.mBuffCursePreb);
                    break;
            }

            mSEObj.transform.SetParent(Trans);
            mSEObj.transform.localPosition = Vector3.zero;

            if (BFunc != null)
                BFunc(mBuffType);
        }
    }

    // Update is called once per frame
    public void Update(CNpcInst NpcInst)
    {
        if (Time.time > mLiveT)
        {
            if (mSEObj != null)
            {
                mSEObj.transform.SetParent(null);
                mSEObj.SetActive(false);
                GameObject.Destroy(mSEObj);

                if (mBuffType == eBuff.Paralysis)
                    NpcInst.UnFronze();
            }

            return;
        }

    }

    public void Refresh(CNpcInst NpcInst, float LastT)
    {
        if (mSEObj == null)
        {
            mSEObj = GameObject.Instantiate(gDefine.gData.mBuffParalysisPreb);
            mSEObj.transform.SetParent(NpcInst.GetRefMid().transform);
            mSEObj.transform.localPosition = Vector3.zero;

            if (mBuffType == eBuff.Paralysis)
                NpcInst.Fronze(LastT);

            if(NpcInst.mNpc.mIsBoss)
                mSEObj.transform.localScale = Vector3.one * 1.5f;
                
        }

        mLiveT = Time.time + LastT;
    }


}

public class CNpcBuff
{
    List<CBuff> mArr = new List<CBuff>();

    public void Update(CNpcInst NpcInst)
    {
        foreach (CBuff buff in mArr)
            buff.Update(NpcInst);
    }

    CBuff FindBuff(CBuff.eBuff BuffType)
    {
        foreach (CBuff f in mArr)
            if (f.mBuffType == BuffType)
                return f;
        return null;
    }

    CBuff CreateBuff(CNpcInst NpcInst, CBuff.eBuff BuffType, float LastT)
    {
        CBuff buff = new CBuff();
        buff.mBuffType = BuffType;
        buff.Refresh(NpcInst, LastT);
        mArr.Add(buff);
        return buff;
    }

    public void AddBuff(CNpcInst NpcInst, CBuff.eBuff BuffType, float LastT)
    {
        CBuff buff = FindBuff(BuffType);
        if (buff == null)
            CreateBuff(NpcInst, BuffType, LastT);
        else
            buff.Refresh(NpcInst, LastT);
    }

    public void Close()
    {
        foreach (CBuff buff in mArr)
        {
            buff.Close();
        }
        mArr.Clear();
    }
}



public class CPlayerBuff
{
    List<CBuff> mArr = new List<CBuff>();

    public void Update()
    {
        foreach (CBuff buff in mArr)
            buff.UpdatePlayer();
    }

    public bool HasBuff(CBuff.eBuff BuffType)
    {
        foreach (CBuff f in mArr)
            if (f.mBuffType == BuffType && f.mLiveT > 0)
                return true;
        return false;
    }

    CBuff FindBuff(CBuff.eBuff BuffType)
    {
        foreach (CBuff f in mArr)
            if (f.mBuffType == BuffType)
                return f;
        return null;
    }

    public void AddBuff(CBuff.eBuff BuffType, float LastT, int Damage, Transform Trans, CBuff.BuffBegin_CallBack BFunc, CBuff.BuffEnd_CallBack EFunc)
    {
        CBuff buff = FindBuff(BuffType);
        if (buff == null)
        {
            buff = new CBuff();
            mArr.Add(buff);
        }

        buff.Refresh(BuffType, LastT, Damage, Trans, BFunc, EFunc);
    }

    public void Close()
    {
        foreach (CBuff buff in mArr)
        {
            buff.Close();
        }
        mArr.Clear();
    }
}