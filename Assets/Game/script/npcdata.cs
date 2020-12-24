using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[System.Serializable]
///关卡Npc数据
public class NpcWaveParamData
{
    public int[][] mParam;
    // public int [] mComHp;
    // public int [] mComDamage;
    // public int [] mEliteHp;
    // public int [] mEliteDamage;
    // public int [] mBossHp;
    // public int [] mBossDamage;
    // public int [] mBigBossHp;
    // public int [] mBigBossDamage;

    /// <summary>
    /// 读取每个关卡的数据
    /// </summary>
    /// <param name="StrArr">总的数据列表</param>
    /// <param name="Index">偏移索引，该偏移=关卡Id（从0开始）</param>
    public void Read(string[] StrArr, int Index)
    {
        mParam = new int[8][];
        string[] sepStr1 = new string[] { "\t" };
        for (int i = 0; i < 8; i++)
        {
            string[] waveArr = StrArr[i + Index * 8].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);
            mParam[i] = new int[waveArr.Length - 1];
            for (int j = 0; j < waveArr.Length - 1; j++)
                mParam[i][j] = int.Parse(waveArr[j + 1]);
        }
    }

    public int GetWaveNum()
    {
        return mParam[0].Length;
    }


    public int GetHP(npcdata.eNpcClass NpcClass, int WaveIndex)
    {
        switch (NpcClass)
        {
            case npcdata.eNpcClass.Common:
                return mParam[0][WaveIndex];
            case npcdata.eNpcClass.Elite:
                return mParam[2][WaveIndex];
            case npcdata.eNpcClass.Boss:
                return mParam[4][WaveIndex];
            case npcdata.eNpcClass.BigBoss:
                return mParam[6][WaveIndex];
        }
        return 0;
    }

    public int GetDamage(npcdata.eNpcClass NpcClass, int WaveIndex)
    {
        switch (NpcClass)
        {
            case npcdata.eNpcClass.Common:
                return mParam[1][WaveIndex];
            case npcdata.eNpcClass.Elite:
                return mParam[3][WaveIndex];
            case npcdata.eNpcClass.Boss:
                return mParam[5][WaveIndex];
            case npcdata.eNpcClass.BigBoss:
                return mParam[7][WaveIndex];
        }
        return 0;
    }

}

public class npcdata : MonoBehaviour
{
    public enum eNpcType
    {
        BareHand = 1, //
        Detonator = 2,
        Shield = 3,
        RoundSword = 4,
        BiaoQiang = 5,
        HideDetonator = 6,
        PosionSword = 7,
        BallBomb = 8,
        CaiDao = 9,
        RagePursuer = 10,
        PosionPursuer = 11,
        DefPursuer = 12,
        RebackPursuer = 13,
        JumpPursuer = 14,
        Cao = 15,
        AirBomb = 16,
        AirHitGround = 17,
        AirZuZhou = 18,
        AirFire = 19,
        AirFlane = 20,
        AirLight = 21,
        GoldCao = 22,
        HalfDown=23,
        HalfUp=24,
        Count,
    }

    public enum eNpcClass
    {
        Common = 0,
        Elite,
        Boss,
        BigBoss,
    }

    [Header("Npc preb 列表")]
    public GameObject[] mNpcPrebArr = new GameObject[(int)eNpcType.Count];

    //[Header("Npc 关卡数据 列表")]
    NpcWaveParamData[] mNpcLvLParam;

    [Header("Npc波次 txt")]
    public TextAsset mNpcWaveText;

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gNpcData = this;
        ReadNpcWaveParamData();
    }

    void ReadNpcWaveParamData()
    {
        mNpcLvLParam = new NpcWaveParamData[10];

        string str = mNpcWaveText.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] waveArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < 10; i++)
        {
            mNpcLvLParam[i] = new NpcWaveParamData();
            mNpcLvLParam[i].Read(waveArr, i);
        }

    }

    public GameObject GetNpcPreb(eNpcType NpcType)
    {
        if ((int)NpcType >= (int)mNpcPrebArr.Length)
            return mNpcPrebArr[0];
        else if (mNpcPrebArr[(int)NpcType] == null)
            return mNpcPrebArr[0];
        else
            return mNpcPrebArr[(int)NpcType];
    }

    public eNpcClass GetNpcClass(eNpcType NpcType)
    {
        switch (NpcType)
        {
            case eNpcType.Shield:
                return eNpcClass.Elite;
            case eNpcType.CaiDao:
                return eNpcClass.Elite;

            case eNpcType.RagePursuer:
                return eNpcClass.Boss;

            case eNpcType.PosionPursuer:
                return eNpcClass.Boss;
            case eNpcType.DefPursuer:
                return eNpcClass.BigBoss;
            case eNpcType.RebackPursuer:
                return eNpcClass.BigBoss;
            case eNpcType.JumpPursuer:
                return eNpcClass.BigBoss;

            case eNpcType.Cao:
            case eNpcType.GoldCao:
                return eNpcClass.Elite;
        }

        return eNpcClass.Common;
    }

    public NpcMono CreateNewNpc(eNpcType NpcType)
    {
        switch (NpcType)
        {
            case eNpcType.BareHand:
                GameObject o = GameObject.Instantiate(mNpcPrebArr[(int)NpcType]);
                return o.GetComponent<NpcMono>();

        }

        return null;
    }

    public int getChapterWaveNum(int ChapterId)
    {
        return mNpcLvLParam[ChapterId].GetWaveNum();
    }
    public int GetNpcMaxHp(eNpcType NpcType, int WaveIndex, int ChapterId)
    {
        eNpcClass npcClass = GetNpcClass(NpcType);
        return mNpcLvLParam[ChapterId - 1].GetHP(npcClass, WaveIndex - 1);

    }

    public int GetNpcDamage(eNpcType NpcType, int WaveIndex, int ChapterId)
    {
        eNpcClass npcClass = GetNpcClass(NpcType);
        return mNpcLvLParam[ChapterId - 1].GetDamage(npcClass, WaveIndex - 1);
    }

}

