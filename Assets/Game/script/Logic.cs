using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logic : MonoBehaviour
{
    public GameObject mGameRootObj;
    public GameObject mSceRefPoint;

    public GameObject mPcBronPosObj;

    public GameObject[] mSceObjArr = new GameObject[10];
    public Sprite[] mSceGroundTex = new Sprite[10];
    public Image mFightUIBg;

    //public npcManager mRefNpcManager; //npc manager

    public npcdata mRefNpcData;

    public bool mIsBegin = false; //是否开始游戏

    public int mWaveLvL = -1; //关卡
    int mCurWaveFromWhichSide = 0; //0, 两个方向 1 左面 2 右边
    List<int> mWaveLvLNpc = new List<int>(); //存放每个波次npc数量，
    List<npcdata.eNpcType> mWaveNpcTypeDict = new List<npcdata.eNpcType>(); //波次中的所有npc

    public LogicWave mLogicWave = new LogicWave();

    //战斗场景改变后的3选一，奇遇，附魔
    List<ui_lvlTip.eInsertTask> mLvLChangeNextAct = new List<ui_lvlTip.eInsertTask>();

    public Teach mTeach = new Teach();
    public OpenStory mOpenStory;

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gLogic = this;
        mGameRootObj.SetActive(false);
    }

    public void ClearLvLChanage()
    {
        mLvLChangeNextAct.Clear();
    }

    public void AddLvLChange(ui_lvlTip.eInsertTask Task)
    {
        mLvLChangeNextAct.Add(Task);
    }

    public void GoNextLvLChange()
    {
        if (mLvLChangeNextAct.Count > 0)
        {
            ui_lvlTip.eInsertTask Task = mLvLChangeNextAct[0];
            mLvLChangeNextAct.RemoveAt(0);
            if (Task == ui_lvlTip.eInsertTask.Choose31)
                gDefine.gGameMainUI.Show3Choose1();
            else if (Task == ui_lvlTip.eInsertTask.Magic)
                gDefine.gGameMainUI.ShowUIMagic();
            else if (Task == ui_lvlTip.eInsertTask.Adv)
                gDefine.gGameMainUI.ShowUIAdv();
        }
    }


    public int GetCurWaveIndex()
    {
        return mWaveLvL;
    }

    public void BeginGame()
    {
        mIsBegin = true;
        gDefine.gPause = false;
    }

    public void ShowGameSce(int LVL)
    {
        //
        mGameRootObj.SetActive(true);


        if (mSceRefPoint.transform.childCount > 0)
        {
            Transform child = mSceRefPoint.transform.GetChild(0);
            child.SetParent(null);
            GameObject.Destroy(child.gameObject);
        }

        if (Screen.width / Screen.height > 2)
        {
            Camera.main.orthographicSize = 18;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
              1.7f, Camera.main.transform.position.z);
        }
        else
        {
            Camera.main.orthographicSize = 16.73f;
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,
              1.0f, Camera.main.transform.position.z);
        }



        int sceIndex = 0;
        // if( gDefine.gChapterId == 1 )
        // {
        //     sceIndex = Random.Range(0,10);
        // }
        // else
        {
            sceIndex = gDefine.gChapterId - 1;
        }

        // sceIndex = 5;

        GameObject sce = GameObject.Instantiate(mSceObjArr[sceIndex]);
        sce.transform.SetParent(mSceRefPoint.transform);
        sce.transform.localPosition = Vector3.zero;

        gDefine.gMySce = sce.GetComponent<sce>();
        gDefine.gMySce.mRefBeginPos = mPcBronPosObj;

        mFightUIBg.sprite = mSceGroundTex[sceIndex];

        //重新计算属性
        gDefine.gPlayerData.InitBeforeGame();

        //重新设置位置
        gDefine.gMySce.ReInitBeforeGame();

        gDefine.gPcLun.gameObject.SetActive(false);
        gDefine.gPcBigSword.gameObject.SetActive(false);
        gDefine.gPcDSword.gameObject.SetActive(false);
        gDefine.gPcLongGun.gameObject.SetActive(false);
        gDefine.gPcShortGun.gameObject.SetActive(false);

        gDefine.gPcLun.gameObject.transform.position = gDefine.gMySce.mRefBeginPos.transform.position;
        gDefine.gPcDSword.gameObject.transform.position = gDefine.gMySce.mRefBeginPos.transform.position;
        gDefine.gPcBigSword.gameObject.transform.position = gDefine.gMySce.mRefBeginPos.transform.position;

        gDefine.gPcLun.ClearSkillSE();
        gDefine.gPcDSword.ClearSkillSE();
        gDefine.gPcBigSword.ClearSkillSE();

        gDefine.gPcDSword.mBuff.Close();
        gDefine.gPcLun.mBuff.Close();

        gDefine.gPcLun.ClearTarget();
        gDefine.gPcDSword.ClearTarget();
        gDefine.gPcBigSword.ClearTarget();

        gDefine.gPlayerData.ClearLearnSkill();


        gDefine.gFollowCam.InitPos();


        switch (gDefine.mCurUseGirlType)
        {
            case gDefine.eCharType.eLun:
                gDefine.gPcLun.gameObject.SetActive(true);
                gDefine.gPcLun.RefreshHPUI(false);
                break;
            case gDefine.eCharType.eBigSword:
                gDefine.gPcBigSword.gameObject.SetActive(true);
                gDefine.gPcBigSword.RefreshHPUI();
                break;
            case gDefine.eCharType.eDSword:
                gDefine.gPcDSword.gameObject.SetActive(true);
                gDefine.gPcDSword.RefreshHPUI(false);
                break;
        }

        ReStartGame(LVL);

    }

    public void CloseGameSce()
    {
        mGameRootObj.SetActive(false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (
            pauseStatus && !mTeach.mIsInTeach &&
            !(gDefine.gGameMainUI.mRefFailUI != null && !gDefine.gGameMainUI.mRefFailUI .gameObject.activeSelf)
            &&!(gDefine.gGameMainUI.mRefWinUI != null && !gDefine.gGameMainUI.mRefWinUI .gameObject.activeSelf)
        )
        {
            gDefine.gPause = true;
            gDefine.gGameMainUI.mRefPauseUI.Show();
        }

    }

    // public int CalcCurWaveNpcMinHp()
    // {
    //     CChapterData c = gDefine.gData.GetChapterData(gDefine. gChapterId);
    //     CWave wave = c.GetWave(mWaveLvL);
    //     return wave.MinNpcHp();

    // }

    // Update is called once per frame
    void Update()
    {
        gDefine.UpdateInGame();

        if (mTeach.mIsInTeach)
        {
            mTeach.Update();
            return;
        }

        //bool onlyone = false;

        if (!mIsBegin || gDefine.gPause)
            return;

        if (mLvLChangeNextAct.Count > 0)
            return;

        if (!gDefine.gGameMainUI.IsShowLvLUpTipFinish())
            return;

        if (gDefine.gGameMainUI.mRef3Choose1InFight.gameObject.activeSelf
        || gDefine.gGameMainUI.mRefMagicUI.gameObject.activeSelf||
        gDefine.gGameMainUI.mRefWinUI.gameObject.activeSelf)
            return;

        mLogicWave.Update();

        GameObject[] objItArr = GameObject.FindGameObjectsWithTag("it");
        GameObject[] obj1ItArr = GameObject.FindGameObjectsWithTag("it1");
        GameObject[] ditArr = GameObject.FindGameObjectsWithTag("dit");


        if (gDefine.gNpc.IsNpcClear() && mLogicWave.mIsEnd  && ditArr.Length<=0/* && gDefine.gNpc.IsEndDelayOK() /* && objItArr.Length == 0*/)
        {
            gDefine.gNpc.ResetAllDieDelayT();

            for(int i=0; i<objItArr.Length; i++)
            {
                GameObject.Destroy(objItArr[i]);
            }

            for(int i=0; i<obj1ItArr.Length; i++)
            {
                GameObject.Destroy(obj1ItArr[i]);
            }

            //开始新的一个波次
            mWaveLvL++;

            if(mWaveLvL>100)
            {
                int kkk = 1;
            }
            if (mWaveLvL > 1)
            {
                 TDGAMission.OnCompleted(gDefine.gWaveStr);
              //   Debug.Log("mWaveLvL:" + mWaveLvL.ToString() );
            }   
            
            
            gDefine.gWaveStr = "Chap_" + gDefine.gChapterId.ToString() + "_wave_" + mWaveLvL.ToString();
              
            TDGAMission.OnBegin(gDefine.gWaveStr);

            if (mWaveLvL > 1)
            {
                CPCLvLParam p = gDefine.gData.GetPCData(gDefine.gPlayerData.LVL);
                gDefine.gPlayerData.AddEXP(gDefine.gData.mWAveExpInChapter[gDefine.gChapterId - 1]);

                //血量恢复
                float hpRecoverPerc = 0.1f;
                CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.StarBless);
                hpRecoverPerc += d.mLearnNum * 0.5f;

                gDefine.gPlayerData.mHp += (int)(gDefine.gPlayerData.mHpMax * hpRecoverPerc);
                if (gDefine.gPlayerData.mHp > gDefine.gPlayerData.mHpMax)
                    gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;
            }

            if (mWaveLvL > gDefine.gLVLNumInChapter)
            {
                if (!gDefine.gGameMainUI.mRefWinUI.gameObject.activeSelf)
                    gDefine.gGameMainUI.mRefWinUI.Show(gDefine.gChapterId, gDefine.gLVLNumInChapter,
                    gDefine.gPlayerData.mCoinGain);
                return;
            }

            CChapterData curChapter = gDefine.gData.GetChapterData(gDefine.gChapterId);

            CWave wave = curChapter.GetWave(mWaveLvL);

            mLogicWave.Init(wave, mWaveLvL);

            //初始化一个新的关卡
            //InitLVL(wave);

            return;
            //}



            // int npcNum = mWaveLvLNpc[0];
            // mWaveLvLNpc.RemoveAt(0);

            // //生成一波新的Npc
            // //先生成左边的Npc
            // //再生成右边的Npc

            // int rightoff = 7;
            // int leftoff = -7;

            // for(int i=0; i<npcNum; i++)
            // {
            //     Vector3 off = Vector3.zero;
            //     if( mCurWaveFromWhichSide == 0 )
            //     {
            //          if (Random.Range(0, 100) < 50)
            //         {
            //             off.x = rightoff + 2;
            //             rightoff += 2;
            //         }
            //         else
            //         {
            //             off.x = leftoff - 2;
            //             leftoff -= 2;
            //         }
            //     }
            //     else if( mCurWaveFromWhichSide == 1 )
            //     {
            //          off.x = leftoff - 2;
            //          leftoff -= 2;
            //     }
            //     else
            //     {
            //         off.x = rightoff + 2;
            //         rightoff += 2;
            //     }

            //     npcdata.eNpcType npcType = mWaveNpcTypeDict[0];

            //     mWaveNpcTypeDict.RemoveAt(0);
            //     // if( !onlyone)
            //     // {
            //     //     onlyone = true;
            //     //     npcType = npcdata.eNpcType.Detonator;
            //     // }

            //    // npcType = npcdata.eNpcType.Detonator;

            //     GameObject npc = Instantiate( gDefine.gNpcData.GetNpcPreb(npcType));

            //     Vector3 pos = gDefine.GetPCTrans().position + off;

            //     pos.y = gDefine.gGrounY;

            //     CNpcInst npcInst = gDefine.gNpc.CreateNpcInst(npcType);

            //     npcInst.Init( npc, npcType, pos, mWaveLvL );

            //     gDefine.gNpc.AddNpc(npcInst);

            // }

        }



        //int remainNpc = 0;
        //for (int i = 0; i < mWaveLvLNpc.Count; i++)
        //  remainNpc += mWaveLvLNpc[i];

        gDefine.gGameMainUI.RefreshTipText(mWaveLvL + 1, gDefine.gNpc.GetLiveNpc() + mLogicWave.GetRemainNpcNum());

        gDefine.gPlayerData.SkillUpdate();

    }

    void InitLVL(CWave Wave)
    {
        //    mCurWaveFromWhichSide = 0;
        //    if(Wave.mIsFromOneDir)
        //         mCurWaveFromWhichSide = Random.Range(0,100)<50 ? 1 : 2;

        //     //System.Random rand = new System.Random();
        //     //int sumNum = Wave.GetNpcNum();
        //     mWaveNpcTypeDict.Clear();
        //     List <npcdata.eNpcType> tmpList = new List<npcdata.eNpcType>();
        //     for(int i=0; i<Wave.mNpcArr.Count; i++)
        //     {
        //         for(int j=0; j<Wave.mNpcArr[i].mNum; j++)
        //             tmpList.Add( Wave.mNpcArr[i].mNpcType);
        //     }

        //     mWaveNpcTypeDict.Clear();    
        //     while( tmpList.Count> 0 )
        //     {
        //         int index = Random.Range(0, tmpList.Count);
        //         mWaveNpcTypeDict.Add(tmpList[index]);
        //         tmpList.RemoveAt(index);
        //     } 

        //     mWaveLvLNpc.Clear();

        //     if(mWaveNpcTypeDict.Count<5)
        //     {
        //         //只有一波
        //         mWaveLvLNpc.Add(mWaveNpcTypeDict.Count);
        //     }
        //     else
        //     {
        //         int subWaveNum = 0;
        //         int minWaveNum = mWaveNpcTypeDict.Count % 8 == 0?  mWaveNpcTypeDict.Count /8 : mWaveNpcTypeDict.Count /8+1;
        //         int maxWaveNum =  mWaveNpcTypeDict.Count % 5 == 0?  mWaveNpcTypeDict.Count /5 : mWaveNpcTypeDict.Count /5+1;
        //         subWaveNum = Random.Range(minWaveNum, maxWaveNum + 1);

        //         int sumNum = mWaveNpcTypeDict.Count;

        //         while(sumNum > 0)
        //         {
        //             int num = Random.Range( 5,8) ;
        //             if( sumNum < num )
        //             {
        //                 if( sumNum <= 2 )
        //                 {
        //                     mWaveLvLNpc[mWaveLvLNpc.Count-1] += sumNum;
        //                 }
        //                 else
        //                     mWaveLvLNpc.Add(sumNum);
        //                 break;
        //             }
        //             else
        //             {
        //                 mWaveLvLNpc.Add(num);
        //                 sumNum -= num;
        //             }
        //         }
        //     }

        //     //CChapterData chapterData = gDefine.gData.GetChapterData(gDefine.gChapter);
        //     //chapterData.

        //     gDefine.gGameMainUI.RefreshTipText(mWaveLvL+1, mWaveNpcTypeDict.Count);

        //     if (mWaveLvL >= 1)
        //         gDefine.gGameMainUI.ShowLvLUpTip("通过关卡");

        //     gDefine.gGameMainUI.ShowLvLUpTip("第" + (mWaveLvL+1).ToString() + "关");

        //     if( NeedShow31(mWaveLvL) )
        //     {
        //         gDefine.gGameMainUI.PlayLvLUpTip( true );
        //        // gDefine.gPause = true;
        //        // gDefine.gGameMainUI.Show3Choose1();
        //         // gDefine.gPlayerData.mWaveLvLUpInGame = false;
        //     }
        //     else
        //     {
        //          gDefine.gGameMainUI.PlayLvLUpTip( false );
        //     }

        //mWaveLvL++;
    }

    public void ReStartGame(int LvL)
    {
        //1 清空关卡
        mWaveLvLNpc.Clear();

        mWaveLvL = LvL;
    

        if (LvL > 1)
        {
            gDefine.gPlayerData.PreLoad();
        }

       // mWaveLvL = 4;

        //2 清空npc
        gDefine.gNpc.ClearAll();
        if( gDefine.gMecha != null )
        {
            GameObject.Destroy(gDefine.gMecha.gameObject);
            gDefine.gMecha = null;
        }


        //3 刷新界面
        gDefine.gGameMainUI.ShowStartUI();

        mIsBegin = false;
        mLogicWave.mIsEnd = true;
        mLvLChangeNextAct.Clear();
        gDefine.gUIFail.InitBeforeGame();

        //gDefine.gIsFirstInGame = true;
        if (gDefine.gIsFirstInGame)
        {
            mOpenStory.BeginOPStory();
           
            //mTeach.BeginTeach();
        }
    }
}
