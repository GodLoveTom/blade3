using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 玩家等级数据等
/// </summary>
public class CPCLvLParam
{
    public int mLVL;
    public int mEXP;
    //public int mDamage;
    //public int mHp;
}

public class CWaveNpc
{
    public int mSubWaveIndex;
    public npcdata.eNpcType mNpcType;
    public int mNum;
    public int mDir;//0 left, 1 right, 2 random
    public float mDelay;
}

public class CWave
{
    public int mIndex;
    // public bool mIsFromOneDir;
    public List<CWaveNpc> mNpcArr = new List<CWaveNpc>();

    // public int MinNpcHp()
    // {
    //     int hp = 100000;
    //     for(int i=0; i<mNpcArr.Count; i++)
    //     {
    //        int npchp =  gDefine.gNpcData.GetNpcMaxHp(  mNpcArr[i].mNpcType, mIndex, gDefine.gChapterId) ;
    //        if( hp > npchp )
    //             hp = npchp;
    //     }
    //     return hp;
    // }
}

public class CChapterData
{
    public int mChapterId;
    List<CWave> mDict = new List<CWave>();

    public void ReadData(string[] StrArr)
    {
        int index = int.Parse(StrArr[1]);
        CWave wave = GetWave(index);
        if (wave == null)
        {
            wave = new CWave();
            wave.mIndex = index;
            mDict.Add(wave);
        }
        int subIndex = int.Parse(StrArr[2]);

        for (int j = 3; j < 100; j += 4)
        {
            if (StrArr.Length > j)
            {
                CWaveNpc npc = new CWaveNpc();
                npc.mSubWaveIndex = subIndex;
                //int npcId = int.Parse(StrArr[j]);
                //int npctype = -1;
                // if(! int.TryParse(StrArr[j],out npctype))
                // {
                //     int kkk = 1;
                // }



                npc.mNpcType = (npcdata.eNpcType)int.Parse(StrArr[j]);
                npc.mNum = int.Parse(StrArr[j + 1]);
                if (j + 2 >= StrArr.Length)
                {
                    int kkk = 1;
                }

                if (StrArr[j + 2] == "左")
                    npc.mDir = 0;
                else if (StrArr[j + 2] == "右")
                    npc.mDir = 1;
                else
                    npc.mDir = 2;
                if (j + 3 >= StrArr.Length)
                    npc.mDelay = 0;
                else
                    npc.mDelay = float.Parse(StrArr[j + 3]);
                wave.mNpcArr.Add(npc);
            }
            else
                break;
        }

    }

    public CWave GetWave(int WaveIndex)
    {
        for (int i = 0; i < mDict.Count; i++)
            if (mDict[i].mIndex == WaveIndex)
                return mDict[i];
        return null;
    }

    public int GetWaveNum()
    {
        return mDict.Count;
    }
}

public class CSoundData
{
    public int mId;
    public string mName;
    AudioClip mClip;

    public AudioClip GetClip()
    {
        if (mClip != null)
            return mClip;
        else
        {
            mClip = gDefine.gABLoad.GetSoundClipAsset("sound.bytes", mName);
            return mClip;
        }
    }
}

public class data : MonoBehaviour
{
    [Header("机甲")]
    public GameObject mMechaPreb;
    [Header("掉落的金币")]
    public GameObject mDropItemPreb;
    [Header("掉落的金币")]
    public GameObject mCoinPreb;
    [Header("掉落的钻石")]
    public GameObject mCrystalPreb;

    [Header("双刀的命中特效")]
    public GameObject mDSwordHitSEPreb;

    [Header("特效 无敌")]
    public GameObject mQuantumMaskSEPreb;

    [Header("特效 冰霜冲击")]
    public GameObject mIceSEPreb;

    [Header("特效 生命之泉")]
    public GameObject mLifeSpringSEPreb;
    [Header("特效 眩晕金星")]
    public GameObject mFaintStarSEPreb;

    [Header("特效 治愈之光")]
    public GameObject mHealLightSEPreb;

    [Header("特效 闪电球")]
    public GameObject mLightBallSEPreb;

    [Header("特效 闪电链")]
    public GameObject mLightChainSEPreb;

    [Header("特效 落雷")]
    public GameObject mThunderSEPreb;

    [Header("特效 地弩")]
    public GameObject mGroundArrowSEPreb;

    [Header("特效 陷阱")]
    public GameObject mXianJingSEPreb;

    [Header("PC投掷特效 ")]
    public GameObject mThrowItemSEPreb;

    [Header("特效 元气护体")]
    public GameObject mForceAwakeSEPreb;

    [Header("特效 环绕电磁")]
    public GameObject mElectBallAround;
    [Header("特效 毒性攻击")]
    public GameObject mPosionSEPreb;

    [Header("特效 锁定提示")]
    public GameObject mLockSEPreb;



    [Header("特效 后闪刀光")]
    public GameObject mSkillBackFlshSEPreb;

    [Header("特效 buff 麻痹")]
    public GameObject mBuffParalysisPreb;

    [Header("特效 buff 中毒")]
    public GameObject mBuffPosionPreb;
    [Header("特效 buff 诅咒")]
    public GameObject mBuffCursePreb;



    [Header("特效 闪电链 空")]
    public GameObject mLightChainNullPreb;

    [Header("特效 剑雨")]
    public GameObject mSwordRainPreb;

    [Header("特效 双刀顺斩斩杀刀光")]
    public GameObject mDSwordQuickKillFlashPreb;

    [Header("特效 双刀顺斩落地飞出刀光")]
    public GameObject mDSwordQuickKillDownFlyFlashPreb;

    [Header("特效 奇遇界面加血特效")]
    public GameObject mUIHealSEPreb;
    [Header("特效 奇遇技能等界面加攻击特效")]
    public GameObject mUIAtkAddSEPreb;

    [Header("Npc狂暴追击者炸弹特效")]
    public GameObject mNpcRagePursuerBombPreb;
    [Header("Npc狂暴追击者炸弹爆炸特效")]
    public GameObject mNpcRagePursuerBombFirePreb;

    [Header("Npc回旋镖特效")]
    public GameObject mRoundSwordThrowSEPreb;

    [Header("Npc炸弹爆炸特效")]
    public GameObject mNpcBombBombPreb;

    [Header("Npc空中自爆怪爆炸特效")]
    public GameObject mNpc空中自爆怪爆炸特效Preb;

    [Header("Npc空中怪死亡落地爆炸特效")]
    public GameObject m空中怪死亡落地爆照特效Preb;

    [Header("Npc毒飞弹---毒飞弹追踪者的技能")]
    public GameObject mPosionPurSuerPosionBallPreb;


    [Header("Npc毒陷阱")]
    public GameObject mNpcPosionTrapPreb;

    [Header("Npc毒地波")]
    public GameObject mNpcPosionWavePreb;

    [Header("空中怪死亡爆炸特效")]
    public GameObject mAirNpcDieSE;
    [Header("激光怪死亡爆炸特效")]
    public GameObject mLaserNpcDieSE;

    [Header("巢穴撞地爆炸特效")]
    public GameObject mCaoHitGroundSEPreb;

    [Header("巢穴锁定提示特效")]
    public GameObject mCaoLockSEPreb;

    [Header("黄金巢穴金币掉落特效")]
    public GameObject mGlodCaoCoinDropSEPreb;

    [Header("反击喷毒Npc毒陷阱")]
    public GameObject mNpcPosionTrapSEPreb;

    [Header("Npc毒飞弹新")]
    public GameObject mNpcPosionBombPreb;

    [Header("Npc匝地追踪者额外特效")]
    public GameObject mNpcHitGroundEXSEPreb;


    [Header("界面金币特效")]
    public GameObject mUICoinsSEPreb;
    [Header("界面钻石特效")]
    public GameObject mUICrystalSEPreb;


    [Header("攻击提示")]
    public GameObject mAtkTipPreb;


    [Header("物品数据")]
    public CItem[] mItemArr;

    [Header("技能升级")]
    public TextAsset mSkillAdd;

    [Header("技能名称描述 xml")]
    public TextAsset mSkillData;

    [Header("Item 数据")]
    public TextAsset mItemData;

    [Header("关卡波次 txt")]
    public TextAsset mWaveData;

    [Header("pc 升级数据 txt")]
    public TextAsset mPCLVLText;

    [Header("天赋 txt")]
    public TextAsset mTalentText;

    [Header("音效表 txt")]
    public TextAsset mSoundText;
    [Header("商店 txt")]
    public TextAsset mShopText;
    [Header("字符串 txt")]
    public TextAsset mStrText;
    [Header("道具默认图标")]
    public Sprite mItemIconSprite;
    [Header("字体，微软，粗")]
    public Font mFont;

    public TextAsset mChapterDropText;

    Dictionary<int, CChapterData> mChapterDict = new Dictionary<int, CChapterData>();
    /// <summary>
    /// 玩家等级数据数组
    /// </summary>
    CPCLvLParam[] mPCLVLDataArr = null;

    public int[] mWAveExpInChapter = new int[] { 62, 72, 79, 90, 107, 141, 197, 278, 341, 657 };

    CSoundData[] mSoundArr;


    // Start is called before the first frame update
    void Start()
    {
        gDefine.gData = this;

        ParaseWaveData();
        ParasePCData();
        ParaseSoundData();
        ParaseItemData();
        ParaseChapterDropData();

        gDefine.Init();
    }

    public AudioClip GetSoundClip(int Id)
    {
        if (mSoundArr.Length > Id)
            return mSoundArr[Id].GetClip();
        else
        {
            return null;
        }
    }

    public CPCLvLParam GetPCData(int LVL)
    {
        if (LVL >= 0 && LVL < mPCLVLDataArr.Length)
            return mPCLVLDataArr[LVL];
        else
            return null;
    }

    public void ParaseWaveData()
    {
        string str = mWaveData.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] waveArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < waveArr.Length; i++)
        {
            string[] valueArr = waveArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);
            int chapterId = int.Parse(valueArr[0]);

            CChapterData chapter = CreateChapterData(chapterId);
            chapter.ReadData(valueArr);
        }
    }

    public void ParaseItemData()
    {
        mItemArr = new CItem[260];

        string str = mItemData.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] itArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < itArr.Length; i++)
        {

            string[] valueArr = itArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);

            int id = int.Parse(valueArr[0]);

            if (mItemArr[id] == null)
                mItemArr[id] = new CItem();

            mItemArr[id].ReadData(valueArr);
        }
    }

    public void ParaseChapterDropData()
    {
        gDefine.gDropSystem.Init(mChapterDropText);
    }

    public void ParaseSoundData()
    {
        string str = mSoundText.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] waveArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        mSoundArr = new CSoundData[waveArr.Length + 1];
        for (int i = 0; i < waveArr.Length; i++)
        {
            string[] valueArr = waveArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);
            CSoundData data = new CSoundData();
            data.mId = int.Parse(valueArr[0]);
            data.mName = valueArr[1];

            mSoundArr[data.mId] = data;
        }
    }

    public void ParasePCData()
    {
        string str = mPCLVLText.text;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] lvlArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);

        mPCLVLDataArr = new CPCLvLParam[lvlArr.Length];

        for (int i = 0; i < lvlArr.Length; i++)
        {
            string[] valueArr = lvlArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);

            CPCLvLParam param = new CPCLvLParam();

            param.mLVL = (int)(float.Parse(valueArr[0]));
            param.mEXP = (int)(float.Parse(valueArr[1]));
            //param.mHp = (int)(float.Parse(valueArr[2]));
            //param.mDamage = (int)(float.Parse(valueArr[3]));

            mPCLVLDataArr[param.mLVL - 1] = param;
        }
    }

    CChapterData CreateChapterData(int ChaperId)
    {
        CChapterData data = GetChapterData(ChaperId);
        if (data == null)
        {
            data = new CChapterData();
            data.mChapterId = ChaperId;
            mChapterDict.Add(ChaperId, data);
        }
        return data;
    }



    public CChapterData GetChapterData(int ChapterId)
    {
        CChapterData data = null;
        mChapterDict.TryGetValue(ChapterId, out data);
        return data;
    }

    public CItem GetItemData(int Id)
    {
        return mItemArr[Id];
        // for(int i=0; i<mItemArr.Length; i++)
        //     if( mItemArr[i]?.Id == Id )
        //         return mItemArr[i];

        // return null;
    }

    public CItem[] GetItemsByCombinePieceId(int PieceId)
    {
        List<CItem> tmp = new List<CItem>();
        for (int i = 0; i < mItemArr.Length; i++)
        {
            if (mItemArr[i] != null && mItemArr[i].mComPieceId == PieceId)
                tmp.Add(mItemArr[i]);
        }
        return tmp.ToArray();
    }



}
