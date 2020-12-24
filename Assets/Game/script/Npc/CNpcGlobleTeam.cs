﻿/// <summary>
/// 全局队列
/// 负责控制注册进来的npc，攻击序列排队， 左右各有两个进入攻击状态，
/// 其他队列中npc周围等待
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CNpcGlobleTeam
{
    List<CNpcInst> mWaitDict = new List<CNpcInst>();
    List<CNpcInst> mLeftTeam = new List<CNpcInst>();
    List<CNpcInst> mRightTeam = new List<CNpcInst>();

    int count = 0;
    float mWaitMinX=3.5f;
    float mWaitMaxX=6.0f;
    int mInFightNum = 1; //每侧交战人数

    public void Clear()
    {
        mWaitDict.Clear();
        mLeftTeam.Clear();
        mRightTeam.Clear();
    }


    public void Register(CNpcInst Npc)
    {
        mWaitDict.Add(Npc);
    }

    /// <summary>
    /// 设置交战等待距离
    /// </summary>
    /// <param name="WaitMinL"></param>
    /// <param name="WaitMaxL"></param>
    public void Init(float WaitMinL, float WaitMaxL)
    {
        mWaitMinX = WaitMinL;
        mWaitMaxX = WaitMaxL;
    }

    /// <summary>
    /// 获得一个随机的交战等待距离
    /// </summary>
    /// <param name="IsRight"></param>
    /// <returns></returns>
    public float GetWaitX(bool IsRight)
    {
        float l = Random.Range(mWaitMinX, mWaitMaxX);
        return gDefine.GetPCTrans().position.x + (IsRight ? l : -l);
    }

    // Update is called once per frame
    public void Update()
    {
        //每3帧计算一次
        count = count % 3;
        if (count == 0)
        {
            Recalc();
        }
    }

    public bool IsSelfInAtkTeam(CNpcInst Npc, bool IsRight)
    {
        if (IsRight)
        {
            foreach (CNpcInst N in mRightTeam)
                if (N == Npc)
                    return true;
        }

        else
        {
            foreach (CNpcInst N in mLeftTeam)
                if (N == Npc)
                    return true;
        }

        return false;
    }

    void Recalc()
    {
        mLeftTeam.Clear();
        mRightTeam.Clear();

        Vector3 pcPos = gDefine.GetPCTrans().position;

        for(int i=0 ; i<mWaitDict.Count; i++)
        {
            if( mWaitDict[i]==null || !mWaitDict[i].IsLive() )
            {
                mWaitDict.RemoveAt(i);
                i--;
                continue;
            }

            Vector3 pos = mWaitDict[i].GetPos();
            if( pos.x > pcPos.x)
            {
                
                for(int j=0; j<mRightTeam.Count;j++)
                {
                    if( pos.x < mRightTeam[j].GetPos().x )
                    {
                         mRightTeam.Insert(j, mWaitDict[i]);
                         goto Next0;
                    }
                }
                
                mRightTeam.Add(mWaitDict[i]);
              Next0:  
                if( mRightTeam.Count > mInFightNum)
                    mRightTeam.RemoveAt(mInFightNum);
            }
            else
            {
                int j=0;
                for(j=0; j<mLeftTeam.Count;j++)
                {
                    if( pos.x > mLeftTeam[j].GetPos().x )
                    {
                        mLeftTeam.Insert(j, mWaitDict[i]);
                        goto Next1;
                    }
                }
                
                    mLeftTeam.Add(mWaitDict[i]);
                Next1:
                if( mLeftTeam.Count > mInFightNum)
                    mLeftTeam.RemoveAt(mInFightNum);
            }
        }
    }
}
