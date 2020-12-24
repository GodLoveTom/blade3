//关卡系统
//有空了，把关卡相关都整理在这里为好
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region  CChapterEXDifficult
public class CChapterEXDifficult
{
    public int mChapterId;
    public int mDifficult;
    public int mLvLFinish = 0; //完成的关卡数
    public int mBoxIndex = 0; //当前为第几个宝箱
    public bool mBoxIsGet = false; // 当前的宝箱是否被领取

    public bool IsFinished()
    {
        if (mChapterId == 1)
            return mLvLFinish == 30;
        else
            return mLvLFinish == 50;
    }

    public bool IsBoxExist()
    {
        if (mChapterId == 1 && mBoxIndex > 2)
            return false;
        else if (mBoxIndex > 4)
            return false;
        else
            return true;
    }

    public bool IsBoxCanGet()
    {
        if (mLvLFinish >= mBoxIndex * 10 + 10 && !mBoxIsGet)
            return true;
        else
            return false;
    }

    public void Load()
    {
        mBoxIndex = PlayerPrefs.GetInt("Chapter_" + mChapterId.ToString() + "_diff_" + mDifficult.ToString() + "_BoxIndex", 0);
        mBoxIsGet = PlayerPrefs.GetInt("Chapter_" + mChapterId.ToString() + "_diff_" + mDifficult.ToString() + "_BoxGet", 0) == 0 ? false : true;

        mLvLFinish = PlayerPrefs.GetInt("Chapter_" + mChapterId.ToString() + "_diff_" + mDifficult.ToString() + "_mLvLFinish", 0);

    }

    public void Save()
    {
        PlayerPrefs.SetInt("Chapter_" + mChapterId.ToString() + "_diff_" + mDifficult.ToString() + "_BoxIndex", mBoxIndex);
        PlayerPrefs.SetInt("Chapter_" + mChapterId.ToString() + "_diff_" + mDifficult.ToString() + "_BoxGet", mBoxIsGet ? 1 : 0);
        PlayerPrefs.SetInt("Chapter_" + mChapterId.ToString() + "_diff_" + mDifficult.ToString() + "_mLvLFinish", mLvLFinish);

    }
}

#endregion

#region  CChapterEX
public class CChapterEX
{
    public int mChapterId;
    public CChapterEXDifficult[] mDiffArr;

    public void FinishBox(int Difficult)
    {
        mDiffArr[Difficult].mBoxIndex++;
        mDiffArr[Difficult].Save();
        PlayerPrefs.Save();
    }

    public bool IsChapterOpen(int Difficult)
    {
        if (Difficult == 0)
        {
            if (mChapterId == 1)
                return true;
            else
                return gDefine.gPlayerData.mChapterEx.IsChapterFinish(mChapterId - 1, Difficult);
        }
        else
        {
            return mDiffArr[Difficult - 1].IsFinished();
        }
    }

    public bool IsFinished(int Difficult)
    {
        return mDiffArr[Difficult].IsFinished();
    }

    public void Load()
    {
        mDiffArr = new CChapterEXDifficult[4];

        for (int i = 0; i < mDiffArr.Length; i++)
        {
            mDiffArr[i] = new CChapterEXDifficult();
            mDiffArr[i].mChapterId = mChapterId;
            mDiffArr[i].mDifficult = i;
            mDiffArr[i].Load();
        }
    }

    public void Clear()
    {
        mDiffArr = new CChapterEXDifficult[4];

        for (int i = 0; i < mDiffArr.Length; i++)
        {
            mDiffArr[i] = new CChapterEXDifficult();
            mDiffArr[i].mChapterId = mChapterId;
            mDiffArr[i].mDifficult = i;
        }
    }

    public void Save()
    {
        for (int i = 0; i < mDiffArr.Length; i++)
        {
            mDiffArr[i].Save();
        }
    }
}

#endregion

#region  CChapterSystem
public class CChapterSystem
{
    CChapterEX[] mChapterArr;

    bool mInit = false;


    public void FinishBox(int ChapterId, int Difficult)
    {
        mChapterArr[ChapterId - 1].FinishBox(Difficult);
    }

    public CChapterEX FindChapter(int ChapterId)
    {
        return mChapterArr[ChapterId - 1];
    }

    public int GetChapterFinishLvL(int ChapterId, int Difficult)
    {
        return mChapterArr[ChapterId - 1].mDiffArr[Difficult].mLvLFinish;
    }

    public bool IsChapterFinish(int ChapterId, int Difficult)
    {
        return mChapterArr[ChapterId - 1].IsFinished(Difficult);
    }

    public int GetOpenMaxChapterID(int Difficult)
    {
        Init();

        int ChapterId = 0;
        for (int i = 0; i < mChapterArr.Length; i++)
        {
            if (!mChapterArr[i].IsChapterOpen(Difficult))
                break;
            else
                ChapterId = i + 1;
        }
        return ChapterId;
    }

    public void AddLvLFinish(int ChapterId, int Difficult, int LvL)
    {
        if (mChapterArr[ChapterId - 1].mDiffArr[Difficult].mLvLFinish < LvL)
        {
            mChapterArr[ChapterId - 1].mDiffArr[Difficult].mLvLFinish = LvL;
            mChapterArr[ChapterId - 1].mDiffArr[Difficult].Save();
            PlayerPrefs.Save();
        }
        Debug.Log("finishLvL:" + LvL.ToString());
    }

    public void Init()
    {
        if (mInit == false)
        {
            mChapterArr = new CChapterEX[10];
            for (int i = 0; i < mChapterArr.Length; i++)
            {
                mChapterArr[i] = new CChapterEX();
                mChapterArr[i].mChapterId = i + 1;
                mChapterArr[i].Load();
            }
            mInit = true;
        }
    }

    public void Save()
    {
        for (int i = 0; i < mChapterArr.Length; i++)
        {
            mChapterArr[i].Save();
        }
    }

    public void Clear()
    {
        mChapterArr = new CChapterEX[10];
        for (int i = 0; i < mChapterArr.Length; i++)
        {
            mChapterArr[i] = new CChapterEX();
            mChapterArr[i].mChapterId = i + 1;
            mChapterArr[i].Clear();
        }

        mInit = true;
    }

    public CChapterEXDifficult GetBoxParam(int ChapterId, int Difficult)
    {
        return mChapterArr[ChapterId - 1].mDiffArr[Difficult];
    }

    public bool IsBoxCanGet(int ChapterId, int Difficult)
    {

        return mChapterArr[ChapterId - 1].mDiffArr[Difficult].IsBoxCanGet();
    }

    public int GetCurBoxIndex(int ChapterId, int Difficult)
    {
        return mChapterArr[ChapterId - 1].mDiffArr[Difficult].mBoxIndex;
    }
}

#endregion
