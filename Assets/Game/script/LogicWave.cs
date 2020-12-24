using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicWave
{
    List<CWaveNpc> mOrderList = new List<CWaveNpc>();
    int mSubWaveIndex; //当前波次
    int mSumNpcNum;
    int mCurNpcNum;
    float mT = 0;
    float mLXOffset;
    float mRXOffset;
    public bool mIsEnd = true;
    public int mWaveIndex = 0;

    public int GetRemainNpcNum()
    {
        return mCurNpcNum;
    }

    // Update is called once per frame
    public void Update()
    {
        if (mIsEnd)
            return;

        bool isBossCome = false;

        mT += Time.deltaTime;

        bool remainNpc = false;
        bool onlyone = false;

        bool IsPosionGo = false;
        for (int i = 0; i < mOrderList.Count; i++)
        {
            if (mOrderList[i].mSubWaveIndex > mSubWaveIndex)
                break;
            if (mOrderList[i].mDelay < mT)
            {
                //createNpc.
                for (int k = 0; k < mOrderList[i].mNum; k++)
                {
                    Vector3 off = Vector3.zero;

                    if (mOrderList[i].mDir == 0)
                    {
                        off.x += mLXOffset;
                        mLXOffset -= 2;
                    }
                    else if (mOrderList[i].mDir == 1)
                    {
                        off.x += mRXOffset;
                        mRXOffset += 2;
                    }
                    else
                    {
                        if (Random.Range(0, 100) < 50)
                        {
                            off.x += mLXOffset;
                            mLXOffset -= 2;
                        }
                        else
                        {
                            off.x += mRXOffset;
                            mRXOffset += 2;
                        }
                    }

                    npcdata.eNpcType npcType = mOrderList[i].mNpcType;

                    //npcType = npcdata.eNpcType.AirLight;

                    GameObject npc = GameObject.Instantiate(gDefine.gNpcData.GetNpcPreb(npcType));

                    Vector3 pos = gDefine.GetPCTrans().position + off;

                    pos.y = gDefine.gGrounY;

                    CNpcInst npcInst = gDefine.gNpc.CreateNpcInst(npcType);

                    if (npc.GetComponent<NpcMono>().mIsBoss)
                        isBossCome = true;

                    if (npcType == npcdata.eNpcType.AirFlane)
                        pos.y = gDefine.gAirY2;
                    else if (npcType == npcdata.eNpcType.AirBomb || npcType == npcdata.eNpcType.AirZuZhou
                || npcType == npcdata.eNpcType.AirLight)
                        pos.y = gDefine.gAirY;

                    npcInst.Init(npc, npcType, pos, mWaveIndex);

                    gDefine.gNpc.AddNpc(npcInst);

                    mCurNpcNum--;

                    if (gDefine.gPlayerData.mCurMagicData != null && gDefine.gPlayerData.mCurMagicData.mType == CMagic.eMagic.Posion
                    && gDefine.gPlayerData.mCurMagicData.mNum > 0)
                    {
                        //CSkill s = gDefine.gPlayerData.FindSkillInLearn(CSkill.eSkill.Poison);
                        //if (s != null)
                        //{
                            GameObject o = GameObject.Instantiate(gDefine.gData.mPosionSEPreb);
                            se_Skill_Posion script = o.GetComponent<se_Skill_Posion>();
                            script.Init(npcInst.GetRefMid().transform, npcInst);
                            npcInst.mIsPosion = true;

                            IsPosionGo = true;


                        //}
                    }

                    if (onlyone)
                        break;
                }

                //
                mOrderList.RemoveAt(i);
                i--;
            }
            else
                remainNpc = true;
        }

        if(IsPosionGo)
        {
           gDefine.gPlayerData.mCurMagicData.mNum--;
        }

        if (mOrderList.Count == 0)
        {
            mIsEnd = true;
            mCurNpcNum = 0;
        }
        else
        if (gDefine.gNpc.GetLiveNpc() == 0 && !remainNpc)
        {
            mSubWaveIndex++;
            if (gDefine.gPlayerData.mCurMagicData != null && gDefine.gPlayerData.mCurMagicData.mType == CMagic.eMagic.Posion
               && gDefine.gPlayerData.mCurMagicData.mNum > 0)
                gDefine.gPlayerData.mCurMagicData.mNum--;
            mT = 0;
        }

        if (isBossCome)
        {
            // gDefine.PlaySound(1);
        }
    }

    public void Init(CWave Wave, int WaveIndex)
    {
        mWaveIndex = WaveIndex;
        mOrderList.Clear();
        mSubWaveIndex = 1;
        mT = 0;
        mLXOffset = -11;
        mRXOffset = 11;
        mSumNpcNum = 0;
        mIsEnd = false;
        for (int i = 0; i < Wave.mNpcArr.Count; i++)
        {
            bool isInsert = false;
            for (int j = 0; j < mOrderList.Count; j++)
            {
                if (Wave.mNpcArr[i].mDelay < mOrderList[j].mDelay && Wave.mNpcArr[i].mSubWaveIndex < mOrderList[j].mSubWaveIndex)
                {
                    mOrderList.Insert(j, Wave.mNpcArr[i]);
                    mSumNpcNum += Wave.mNpcArr[i].mNum;
                    isInsert = true;
                    break;
                }
            }

            if (!isInsert)
            {
                mOrderList.Add(Wave.mNpcArr[i]);
                mSumNpcNum += Wave.mNpcArr[i].mNum;
            }
        }

        mCurNpcNum = mSumNpcNum;

        //gDefine.gGameMainUI.RefreshTipText(WaveIndex, mSumNpcNum);

        if (WaveIndex == 1)
            gDefine.gGameMainUI.mRefLvLChangeUIFirst.PlayFirstLvL();
        else if (WaveIndex == gDefine.gLVLNumInChapter)
            gDefine.gGameMainUI.mRefLvLChangeUI.PlayEndLvL(WaveIndex);
        else
            gDefine.gGameMainUI.mRefLvLChangeUI.PlayCommonLvL(WaveIndex);

        // if (WaveIndex > 1)
        //     gDefine.gGameMainUI.ShowLvLUpTip("通过关卡");

        // gDefine.gGameMainUI.ShowLvLUpTip("第" + (WaveIndex).ToString() + "关");

        // gDefine.gGameMainUI.mRefLVLTipUI.ClearInsertTask();

        // bool otherTask = false;
        if (gDefine.gForbidLvL != WaveIndex)
        {
            if (NeedShow31(WaveIndex))
            {
                gDefine.gLogic.AddLvLChange(ui_lvlTip.eInsertTask.Choose31);
                //gDefine.gGameMainUI.mRefLVLTipUI.InsertTask(ui_lvlTip.eInsertTask.Choose31);
                //otherTask = true;
            }

            if (NeedShowMagic(WaveIndex))
            {
                gDefine.gLogic.AddLvLChange(ui_lvlTip.eInsertTask.Magic);
                //gDefine.gGameMainUI.mRefLVLTipUI.InsertTask(ui_lvlTip.eInsertTask.Magic);
                //otherTask = true;
            }

            if (NeedShowAdv(WaveIndex))
            {
                gDefine.gLogic.AddLvLChange(ui_lvlTip.eInsertTask.Adv);
                //gDefine.gGameMainUI.mRefLVLTipUI.InsertTask(ui_lvlTip.eInsertTask.Adv);
                //otherTask = true;
            }

        }



        // if (otherTask)
        //     gDefine.gGameMainUI.PlayLvLUpTip(true);
        // else
        //     gDefine.gGameMainUI.PlayLvLUpTip(false);

    }

    bool NeedShow31(int LVL)
    {
        int[] lvlArr = new int[] { 1, 2, 4, 6, 9, 12, 15, 18, 21, 25, 29, 33, 37, 42, 47 };
        for (int i = 0; i < lvlArr.Length; i++)
        {
            if (lvlArr[i] == LVL)
                return true;
        }
        return false;
    }

    bool NeedShowAdv(int LVL)
    {
        int[] lvlArr = new int[] { 5, 8, 13, 17, 23, 27, 31, 35, 44, 48 };
        for (int i = 0; i < lvlArr.Length; i++)
        {
            if (lvlArr[i] == LVL)
                return true;
        }
        return false;
    }

    bool NeedShowMagic(int LVL)
    {
        if (LVL > 0 && LVL % 10 == 0)
            return true;
        else
            return false;
    }


}
