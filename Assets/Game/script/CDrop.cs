using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//关卡掉落数据
public class CChapterDropDataParam
{
    public int mChapterId; //掉落关卡id
    public int mNameStrId;//
    public List<CItem> mSkill = new List<CItem>(); // 技能书列表
    public List<CItem> mPiece = new List<CItem>();// 装备碎片列表

    public string GetChapterLocalName()
    {
        return gDefine.GetStr(mNameStrId);
    }

    public bool ContainPiece(int PieceId)
    {
        foreach (var v in mPiece)
        {
            if (v.Id == PieceId)
                return true;
        }
        return false;
    }

    public CDropReturnData[] CalcDrop(npcdata.eNpcClass NpcClass)
    {
        switch (NpcClass)
        {
            case npcdata.eNpcClass.Common:
               //return CalcBossDrop();
                return CalcCommonDrop();
            case npcdata.eNpcClass.Elite:
                return CalcEliteDrop();
            case npcdata.eNpcClass.Boss:
            case npcdata.eNpcClass.BigBoss:
                return CalcBossDrop();

                //return CalcBigBossDrop();
        }
        return new CDropReturnData[0];
    }

    public CDropReturnData[] CalcCommonDrop()
    {
        int r = UnityEngine.Random.Range(0, 100);
        
        if (r < 5)
        {
            CDropReturnData[] arr = new CDropReturnData[1];
            arr[0] = new CDropReturnData();
            arr[0].mItemId = 202;
            arr[0].mItemNum = 1;
            return arr;
        }
        else if (r < 15)
        {
            CDropReturnData[] arr = new CDropReturnData[1];
            arr[0] = new CDropReturnData();
            arr[0].mItemId = 201;
            arr[0].mItemNum = 2;
            return arr;
        }
        else
            return new CDropReturnData[0];
    }

    public CDropReturnData[] CalcEliteDrop()
    {
        CDropReturnData[] arr = new CDropReturnData[2];
        arr[0] = new CDropReturnData();
        arr[0].mItemId = 202;
        arr[0].mItemNum = UnityEngine.Random.Range(1, 3);
        arr[1] = new CDropReturnData();
        arr[1].mItemId = 201;
        arr[1].mItemNum = UnityEngine.Random.Range(2, 5);

        return arr;
    }

    public CDropReturnData[] CalcBossDrop()
    {
        bool skillIsdroop = UnityEngine.Random.Range(0, 100) < 20 ? true : false;
        CDropReturnData[] arr = new CDropReturnData[skillIsdroop ? 5 : 4];
        arr[0] = new CDropReturnData();
        arr[0].mItemId = 202;
        arr[0].mItemNum = UnityEngine.Random.Range(3, 5);
        arr[1] = new CDropReturnData();
        arr[1].mItemId = 201;
        arr[1].mItemNum = UnityEngine.Random.Range(10, 15);
        arr[2] = new CDropReturnData();
        arr[2].mItemId = (UnityEngine.Random.Range(0, 100) < 50) ? DropGem() : DropCores();
        arr[2].mItemNum = 1;
        arr[3] = new CDropReturnData();
        arr[3].mItemId = DropCombinePiece();
        arr[3].mItemNum = 1;
        if (skillIsdroop)
        {
            arr[4] = new CDropReturnData();
            arr[4].mItemId = DropSkill();
            arr[4].mItemNum = 1;

        }
        return arr;
    }

    public int DropGem()
    {
        List<int> tmpArr = new List<int>();

        tmpArr.Add(99);
        tmpArr.Add(109);
        tmpArr.Add(119);
        tmpArr.Add(129);
        tmpArr.Add(139);

        return tmpArr[UnityEngine.Random.Range(0, tmpArr.Count)];
    }

    public int DropCores()
    {
        List<int> tmpArr = new List<int>();

        for (int i = 194; i <= 198; i++)
            tmpArr.Add(i);

        return tmpArr[UnityEngine.Random.Range(0, tmpArr.Count)];
    }
    public int DropCombinePiece()
    {
        int index = UnityEngine.Random.Range(0, mPiece.Count);
        return mPiece[index].Id;
    }

    public int DropSkill()
    {
       int index = UnityEngine.Random.Range(0, mSkill.Count);
        return mSkill[index].Id;
    }

}

//掉落返回数据
public class CDropReturnData
{
    public int mItemId;  //掉落道具id
    public int mItemNum; //掉落道具数量
}

//掉落系统
public class CDropSystem
{
    CChapterDropDataParam[] mCharpterDropArr = new CChapterDropDataParam[10];
    /// <summary>
    /// 根据npc类型，计算npc的掉落
    /// </summary>
    /// <param name="ChapterId">关卡id，【1-10】</param>
    /// <param name="NpcClass">npc类型</param>
    /// <returns></returns>
    public CDropReturnData[] CalcDrop(int ChapterId, npcdata.eNpcClass NpcClass)
    {
        CChapterDropDataParam chapter = FindCharpter(ChapterId);
        return chapter.CalcDrop(NpcClass);
    }

    public CChapterDropDataParam FindCharpter(int ChapterId)
    {
        return mCharpterDropArr[ChapterId - 1];
    }

    public void Init(TextAsset Data)
    {

        string str = Data.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] ChapterArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < ChapterArr.Length; i++)
        {
            string[] valueArr = ChapterArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);

            CChapterDropDataParam data = new CChapterDropDataParam();
            data.mChapterId = int.Parse(valueArr[0]);
            data.mNameStrId = int.Parse(valueArr[1]);

            mCharpterDropArr[data.mChapterId - 1] = data;

            for (int j = 2; j < valueArr.Length; j++)
            {
                CItem it = gDefine.gData.GetItemData(int.Parse(valueArr[j]));
                if (it.mMainType == CItem.eMainType.ComPiece)
                    data.mPiece.Add(it);
                else
                    data.mSkill.Add(it);
            }
        }
    }

    public CChapterDropDataParam GetChapterDataByCombinePieceId(int PieceId)
    {
        for (int i = 0; i < mCharpterDropArr.Length; i++)
        {
            if (mCharpterDropArr[i].ContainPiece(PieceId))
                return mCharpterDropArr[i];
        }
        return null;
    }
}
