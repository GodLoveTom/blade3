using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

//bug . 学习技能没有双倍
//暂停界面上 缺少技能描述 弹出框
// ShowPcUIHp() 缺少轮子的处理部分
// PcAddBuff 缺轮子部分 


public static class gDefine
{
    public static bool gIsAuto = false;

    public static Transform gPcTrans;

    public static SpriteAct gPc;

    public static System.Random gRand = new System.Random();

    public static Logic gLogic;

    public static MainInput gGameMainUI;

    public static ui_MainUI gMainUI;
    public static ui_MainGainTip gMainGainTip;

    public static npcManager gNpc;

    public static DamageShowManager gDamageShow;

    public static ui_fail gUIFail;

    public static camfollow gFollowCam;

    public static Edit_Test gUIEditer;

    public static ui_FightMain gUITopInFight;

    public static bool gPause = false; // 当前是否暂停

    public static PlayerData gPlayerData = new PlayerData(); //玩家数据

    public static bool gNpcIsWood = false;

    public static data gData;

    public static CSkillInFight gSkill = new CSkillInFight();

    public static int gLVLNumInChapter = 50;

    public static int gChapterId = 1; // 当前的章节
    public static int gForbidLvL = 0; //该波次没有弹出31界面等，用于断后重玩
    public static int gChapterDifficult = 0; //当前章节的难度0-5

    public static Char_ShortGun gPcShortGun; //短枪娘

    public static Char_LongGun gPcLongGun; //长枪娘

    public static char_lun gPcLun;  //轮子娘

    public static char_BigSword gPcBigSword;  //巨剑娘

    public static char_DSword gPcDSword;  //双剑娘

    public static npcdata gNpcData;

    public static sce gMySce;

    public static CSound gSound;

    public static CABLoad gABLoad = new CABLoad();

    public static float gAirY = 3.13f;
    public static float gAirY2 = 6.5f;
    public static float gGrounY = -4.0f;

    public static Shop gShop = new Shop();

    public static BoxData gBoxData = new BoxData();
    public static CMagic gMagic = new CMagic();

    public static CMyStr gMyStr = new CMyStr();

    public static bool gIsInEndKill = false;

    public static CNpcGlobleTeam gAtkNpcGlobleTeam = new CNpcGlobleTeam();
    public static CNpcAirGlobleTeam gAirNpcGlobleTeam = new CNpcAirGlobleTeam();

    public static List<int> mSoundIdArr = new List<int>();

    public static float gCaoSleepT = 0;

    public static float gNpcHalfUPBronT = 0;

    public static MyAd gAd;

    public static bool gBtnRightDown = false;
    public static bool gBtnLeftDown = false;

    public static Vector3 gUICoinPos;
    public static Vector3 gUICrystalPos;

    //界面金币钻石效果
    static int gUIShowCoin = 0;
    static int gUIShowCrystal = 0;
    static GameObject gUICoinSERoot;
    public static ui_ClickAnim gBtnAnim = new ui_ClickAnim();
    public static bool gIsFirstInGame = false;
    public static  TDGAAccount account;

    public static CDropSystem gDropSystem = new CDropSystem();

    public static string gWaveStr="";
    public static bool gAppraiseShow = false;

    public static CGainInFight gGainInFight = new CGainInFight();

    public static Dictionary<int,Font> mOriFontBold = new Dictionary<int, Font>();

    public static Mecha gMecha ;


    //战斗女孩种类
    public enum eCharType
    {
        eLun = 0,
        eBigSword,
        eDSword,
    }

    public static eCharType mCurUseGirlType = eCharType.eDSword;

    //战斗武器种类
    public enum eGunType
    {
        eLongGun = 0,
        eShortGun,
    }
    public static eGunType mCurUseGunType = eGunType.eShortGun;

    public enum eEuqipPos
    {
        MainWeapon = 0,
        GunWeapon,
        Clothe,
        Cloak,
        Ring,
        Null,
        Count,
    }

    public enum eWeaponClass
    {
        Null = 0,
        DSword,
        BigSword,
        Lun,
        longGun,
        ShortGun,
    }

    public static void SetTextBold()
    {
        // int hashcode = T.GetHashCode();
        // bool saved = false;
        // FontStyle fs ;
        // if( mOriFontBold.TryGetValue(hashcode, out fs) )
        //     saved = true;
        
        // if(gDefine.gPlayerData.mLanguageType == CMyStr.eType.English)
        // {
        //     if( !saved )
        //         mOriFontBold.Add( hashcode, T.fontStyle);
            
        //     T.fontStyle = FontStyle.Bold;
        // }
        // else
        // {
        //      if( saved )
        //           T.fontStyle = fs;
        // }
    }

    public static void CreateMecha(float LiveT)
    {
        if(mCurUseGirlType == eCharType.eDSword)
            gPcDSword.gameObject.SetActive(false);
        else 
            gPcLun.gameObject.SetActive(false);

        GameObject mecha = GameObject.Instantiate(gData.mMechaPreb);
        gMecha = mecha.GetComponent<Mecha>();
        Vector3 pos = GetPCTrans().transform.position;
        gMecha.Init(pos, LiveT);
    }


    public static void ResetFontBold(Text T)
    {
        int hashcode = T.GetHashCode();
        bool saved = false;
        Font f ;
        if( mOriFontBold.TryGetValue(hashcode, out f) )
            saved = true;
        
        if(gDefine.gPlayerData.mLanguageType == CMyStr.eType.English)
        {
            if( !saved )
                mOriFontBold.Add( hashcode, T.font);
            
            if(T.font.name != "7PX2BUS")
                T.font = gData.mFont;
        }
        else
        {
             if( saved )
                  T.font = f;
        }
    }


    public static void PlayUIGainCoinSE(GameObject mUIRoot)
    {
        gUICoinSERoot = mUIRoot;
        gUIShowCoin = 30;
    }

    public static void PlayUIGainCrystalSE(GameObject mUIRoot)
    {
        gUICoinSERoot = mUIRoot;
        gUIShowCrystal = 30;
    }

    public static void PlayUIClickSound()
    {
        PlaySound(24);
    }

    public static bool PcThrowItem(CSkill.eSkill SkillType, int Num)
    {

        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return gPcLun.ThrowItem(SkillType, Num);
            case eCharType.eBigSword:
                return gPcBigSword.ThrowItem(SkillType, Num);
            case eCharType.eDSword:
                return gPcDSword.ThrowItem(SkillType, Num);
        }

        return false;
    }

    public static void ShowLackItTip(int ItemId, int Num, ui_LackTip.CallBackFunc Func)
    {
        gMainUI.mRefMainLackTip.Show(ItemId, Num, Func);
    }

    public static bool PcIsSword()
    {
        return mCurUseGirlType == eCharType.eDSword;
    }

    public static bool IsPcCanBeAtk()
    {
        return true;
    }

    public static void PlayVibrate()
    {
        // Vibration.VibratePop ();
        Camera.main.GetComponent<cam_shake>().shake();
    }

    public static float CalcCaoSleepT()
    {
        float t = gCaoSleepT;
        gCaoSleepT = Time.time + 2.0f;
        return t;
    }


    public static string GetStr(string Str)
    {
        return gMyStr.Get(Str, gPlayerData.mLanguageType);
    }

    public static string GetStr(int StrId)
    {
        return gMyStr.Get(StrId, gPlayerData.mLanguageType);
    }

    public static void CreateDrop( int ItemId, CNpcInst Npc)
    {
            Vector3 pos = Npc.mNpc.transform.position;
            pos.x += Random.Range(-1.5f, 1.5f);
            GameObject coin;
            if(ItemId == 201)
                coin = GameObject.Instantiate(gDefine.gData.mCoinPreb);
            else if( ItemId == 202)
                coin = GameObject.Instantiate(gDefine.gData.mCrystalPreb);
            else
            {
                coin = GameObject.Instantiate(gDefine.gData.mDropItemPreb);
                CItem it = gDefine.gData.GetItemData(ItemId);
                //coin.transform.localScale = coin.transform.localScale*0.75f;
                coin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = it.GetIconSprite() ;
            }

            
            coin.transform.position = pos;
            se_itemDrop script = coin.GetComponent<se_itemDrop>();
            script.mEndFlyToPc = true;
            script.Init();
            script.mVy *= 1.5f;
             if( ItemId != 201 && ItemId != 202)
            // {
            //    script.mTimeScale = 0.8f;
                script.mLiveT *= 2f;
            //    script.gameObject.transform.localPosition = new Vector3(0,1,0);
            // }     
            //Debug.Log("create ok");
    }

    public static void CreateSomeCoinInGame(int Num)
    {
        Vector3 pcPos = gDefine.GetPCTrans().position;
        pcPos.y = gDefine.gGrounY + 1;
        for (int i = 0; i < Num; i++)
        {
            Vector3 pos = pcPos;
            pos.x += Random.Range(-2f, 2f);
            GameObject coin = GameObject.Instantiate(gDefine.gData.mCoinPreb);
            coin.transform.position = pos;
            se_itemDrop script = coin.GetComponent<se_itemDrop>();
            script.mEndFlyToPc = true;
            script.Init();
        }
    }

    public static void CreateSomeCoinInGame(int Num, Vector3 Pos)
    {
        //Pos.y = gDefine.gGrounY + 1;
        for (int i = 0; i < Num; i++)
        {
            Vector3 pos = Pos;
            pos.x += Random.Range(-2f, 2f);
            GameObject coin = GameObject.Instantiate(gDefine.gData.mCoinPreb);
            coin.transform.position = pos;
            se_itemDrop script = coin.GetComponent<se_itemDrop>();
            script.mEndFlyToPc = true;
            script.Init();
        }
    }

    public static void CreateSomeCrystalInGame(int Num)
    {
        Vector3 pcPos = gDefine.GetPCTrans().position;
        pcPos.y = gDefine.gGrounY + 1;
        for (int i = 0; i < Num; i++)
        {
            Vector3 pos = pcPos;
            pos.x += Random.Range(-2f, 2f);
            GameObject coin = GameObject.Instantiate(gDefine.gData.mCrystalPreb);
            coin.transform.position = pos;
            se_itemDrop script = coin.GetComponent<se_itemDrop>();
            script.mEndFlyToPc = true;
            script.Init();
        }
    }

    public static void PlaySound(int SoundId, float delay = 0)
    {
        AudioClip clip = gData.GetSoundClip(SoundId);
        if (clip != null)
            gSound.Play(clip, delay, 1, 0);
        // mSoundIdArr.Add(SoundId);
    }

    public static void UseShootGirlNow(Vector3 Pos, bool FaceR, float ExistT)
    {
        if (mCurUseGunType == eGunType.eLongGun)
            gPcLongGun.Begin(Pos, FaceR, ExistT);
        else
            gPcShortGun.Begin(Pos, FaceR, 1.0f);
    }

    public static void UseCloneShootGirlNow(Vector3 Pos, bool FaceR, float ExistT)
    {
        ExistT = 4f;

        GameObject o = GameObject.Instantiate(gPcLongGun.gameObject);
        o.tag = "it1";
        Char_LongGun s = o.GetComponent<Char_LongGun>();
        Pos.y = gDefine.gGrounY;
        s.Begin(Pos, FaceR, ExistT, true);
        o.GetComponent<Renderer>().material.SetColor("_Color0", new Color(0.866f, 0, 0, 1f));
    }

    public static void UseGunGirlNow()
    {
         switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.UseGunGirl();
                break;
            case eCharType.eBigSword:
                break;
            case eCharType.eDSword:
                gPcDSword.UseGunGirl();
                break;
        }
    }

    public static bool IsPCCanUseGun()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return gPcLun.CanUseGun();
            case eCharType.eBigSword:
                return false;
            case eCharType.eDSword:
                return gPcDSword.CanUseGun();
        }
        return false;
    }



    public static void UseShortGunGirlNow(Vector3 Pos, bool FaceR, float ExistT)
    {
        gPcShortGun.Begin(Pos, FaceR, ExistT);
    }

    public static void UseLongGunGirlNow(Vector3 Pos, bool FaceR, float ExistT)
    {
        gPcLongGun.Begin(Pos, FaceR, ExistT);
    }

    public static void EndUseGunGirl()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.Show();
                break;
            case eCharType.eBigSword:
                gPcBigSword.Show();
                break;
            case eCharType.eDSword:
                gPcDSword.Show();
                break;
        }
        gDefine.gGameMainUI.mRefGunUI.ReSetGunCoolDown();
    }

     public static void EndUseMacheGirl()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.Show();
                break;
            case eCharType.eBigSword:
                gPcBigSword.Show();
                break;
            case eCharType.eDSword:
                gPcDSword.Show();
                break;
        }
        gMecha = null;
        GameObject o = GameObject.Instantiate(gData.mQuantumMaskSEPreb);
        o.GetComponent<se_skill_QuantumMask>().Init(GetPcRefMid());
    }

    public static bool IsPcInTakenState()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return false;
            case eCharType.eBigSword:
                return false;
            case eCharType.eDSword:
                gPcDSword.IsInEndKillState();
                return true;
        }
        return false;
    }

    public static void PushBackPc(Vector3 Pos, CNpcInst OriNpc)
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                //return gPcLun.PushBack(Pos);
                break;
            case eCharType.eBigSword:
                //gPcBigSword.Show();
                break;
            case eCharType.eDSword:
                gPcDSword.PushBack(Pos, OriNpc);
                break;
        }
    }

    public static bool IsUseGunGirl()
    {
        if (gPcLongGun != null && gPcLongGun.gameObject.activeSelf)
            return true;
        else if (gPcShortGun != null && gPcShortGun.gameObject.activeSelf)
            return true;
        else
            return false;
    }

    public static void PcAddBuff(CBuff.eBuff BuffType, float ExistT, int Damage)
    {
        if (gPlayerData.mHp > 0)
        {
            switch (mCurUseGirlType)
            {
                case eCharType.eLun:
                   gPcLun .AddBuff(BuffType, ExistT, Damage, GetPcRefMid().transform, gPcLun.BuffBegin, gPcLun.BuffEnd);

                    break;
                case eCharType.eBigSword:
                    break;
                case eCharType.eDSword:

                    gPcDSword.AddBuff(BuffType, ExistT, Damage, GetPcRefMid().transform, gPcDSword.BuffBegin, gPcDSword.BuffEnd);

                    break;
            }

        }

    }

    public static void ShowTipEX(int ItemId, int Num,  int ItemId1, int Num1, float WaitT)
    {
        ui_BoxTip sript = gMainUI.CreateTip();
        sript.Show(ItemId,Num,ItemId1,Num1,true,WaitT);
    }

    public static Transform GetPCTrans()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return gPcLun.transform;
            case eCharType.eBigSword:
                return gPcBigSword.transform;
            case eCharType.eDSword:
                return gPcDSword.transform;
        }
        return gPcTrans;
    }

    public static Vector3 GetPCCamFollowPos()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return gPcLun.GetPcCamFollowPos();
            case eCharType.eBigSword:
                return gPcBigSword.transform.position;
            case eCharType.eDSword:
                return gPcDSword.transform.position;
        }
        return Vector3.zero;
    }


    public static void ShowPcUIHp()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                //gPcLun.RefreshHPUI(true);
                break;
            case eCharType.eBigSword:
                break;
            //return gPcBigSword.transform.position;
            case eCharType.eDSword:
                gPcDSword.RefreshHPUI(true);
                break;
        }

    }

    public static bool IsPcFaceRight()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return gPcLun.mCurFaceRight;
            case eCharType.eBigSword:
                return gPcBigSword.mCurFaceRight;
            case eCharType.eDSword:
                return gPcDSword.mCurFaceRight;
        }
        return true;
    }

    public static void BeginGame()
    {
        gLogic.BeginGame();
        gGameMainUI.ShowInGameUI();
        gSkill.Clear();
        gDefine.gNpc.ClearAll();
        gDefine.gAtkNpcGlobleTeam.Clear();
        gDefine.gAirNpcGlobleTeam.Clear();
    }

    public static void ReliveHero()
    {
        gPause = false;

        gDefine.gPlayerData.mHp = gDefine.gPlayerData.mHpMax;

        switch (mCurUseGirlType)
        {
            case eCharType.eDSword:
                gPcDSword.Relive();
                break;
            case eCharType.eBigSword:
                gPcBigSword.Relive();
                break;
            case eCharType.eLun:
                gPcLun.Relive();
                break;

        }

        GameObject o = GameObject.Instantiate(gData.mQuantumMaskSEPreb);
        o.GetComponent<se_skill_QuantumMask>().Init(GetPcRefMid());
    }

    public static void RecalcAutoSize(GameObject O)
    {
        float perc0 = Screen.width / 1080.0f;
        float perc1 = Screen.height / 1920.0f;
        float perc = Mathf.Min(perc0, perc1);
        O.transform.localScale = new Vector3(perc, perc, perc);
    }

     public static void RecalcAutoSizeNew(GameObject O)
    {
        float perc0 = Screen.width / 1080.0f;
        float perc1 = Screen.height / 1920.0f;
        float perc2 = (Screen.height - Screen.safeArea.y)/1920.0f;
        float perc = Mathf.Min(perc0, perc1);
        perc = Mathf.Min(perc, perc2);
        O.transform.localScale = new Vector3(perc, perc, perc);
    }

    public static float RecalcUIScale()
    {
        float perc0 = Screen.width / 1080.0f;
        float perc1 = Screen.height / 1920.0f;
        return Mathf.Min(perc0, perc1);
    }



    // public static void ReStartGame()
    // {
    //     gPause = false;
    //     gLogic.ReStartGame();
    // }

    public static void ShowGameSce(int LvL)
    {
        gLogic.ShowGameSce(LvL);
    }

    public static void ShowMainUI()
    {
        gMainUI.gameObject.SetActive(true);
        gMainUI.ChangeToFight();

    }

    public static float GetEdit_Dying2_Up()
    {
        return gUIEditer.GetEdit_Dying2AUpV();
    }

    public static float GetEdit_Dying2_DownAcc()
    {

        return gUIEditer.GetEdit_Dying2DownAcc();
    }

    public static float GetEdit_Dying2_XV()
    {

        return gUIEditer.GetEdit_Dying2_XV();
    }

    public static void KilledPlayEnd()
    {
        gDefine.gPc.EndKilledPlay();
    }

    public static void PcSkillActEndCallBack()
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.SkillActEndCallBack();
                break;
        }
    }

    public static void Init()
    {
        gMyStr.Read(gData.mStrText);

        gDefine.gSkill.LoadFromXml();
        gDefine.gPlayerData.mSkillAdd.ReadData(gDefine.gData.mSkillAdd.text);

        gABLoad.LoadABFromStreamAssets("sound.bytes");
        gABLoad.LoadABFromStreamAssets("icon.bytes");
        gABLoad.LoadABFromStreamAssets("obj.bytes");


        gShop.LoadRes(gDefine.gData.mShopText.text);

        gDefine.gPlayerData.Load();

    }

    /// <summary>
    /// 退出战斗
    /// </summary>
    public static void GoToMainUI()
    {
        gDefine.gLogic.CloseGameSce();
        gDefine.gMainUI.ChangeToFight();
    }

    public static void Btn_Down(bool IsRight)
    {
        if (IsRight)
            gBtnRightDown = true;
        else
            gBtnLeftDown = true;

        if(gDefine.gMecha!=null)
        {
            if(IsRight)
            {
                gDefine.gMecha.mRightIsDown = true;
                gDefine.gMecha.mLeftIsDown = false;
            }
            else
            {
                gDefine.gMecha.mLeftIsDown = true;
                gDefine.gMecha.mRightIsDown = false;
            }
                
            return;
        }

        if (gDefine.gPlayerData.mHp <= 0)
            return;

        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.Btn_Down(IsRight);
                break;
            case eCharType.eBigSword:
                gPcBigSword.Btn_Down(IsRight);
                break;
            case eCharType.eDSword:
                gPcDSword.Btn_Down(IsRight);
                break;
        }
    }

    public static void Btn_Up(bool IsRight)
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return;

         if(gDefine.gMecha!=null)
        {
            // if(IsRight)
            //     gDefine.gMecha.mRightIsDown = false;
            // else
            //     gDefine.gMecha.mLeftIsDown = false;
            return;
        }

        if (IsUseGunGirl())
        {
            if (mCurUseGunType == eGunType.eShortGun)
                gPcShortGun.Mouse_UpOrClick(IsRight);
            else
            {
                gPcLongGun.Mouse_UpOrClick(IsRight);
            }
        }
        else
        {
            switch (mCurUseGirlType)
            {
                case eCharType.eLun:
                    gPcLun.Btn_Up(IsRight);
                    break;
                case eCharType.eBigSword:
                    gPcBigSword.Btn_Up(IsRight);
                    break;
                case eCharType.eDSword:
                    gPcDSword.Btn_Up(IsRight);
                    break;
            }
        }
    }

    public static void Btn_Click(bool IsRight)
    {
        if (gDefine.gPlayerData.mHp <= 0)
            return;

        if(gMecha!=null)
        {
            if(IsRight)
            {
                gDefine.gMecha.mRightIsDown = true;
                gDefine.gMecha.mLeftIsDown = false;
            }
            else
            {
                gDefine.gMecha.mLeftIsDown = true;
                gDefine.gMecha.mRightIsDown = false;
            }
            return;
        }


        if (IsUseGunGirl())
        {
            if (mCurUseGunType == eGunType.eShortGun)
                gPcShortGun.Mouse_UpOrClick(IsRight);
            else
            {
                gPcLongGun.Mouse_UpOrClick(IsRight);
            }
        }
        else
        {
            switch (mCurUseGirlType)
            {
                case eCharType.eLun:
                    gPcLun.Btn_Click(IsRight);
                    break;
                case eCharType.eBigSword:
                    gPcBigSword.Btn_Click(IsRight);
                    break;
                case eCharType.eDSword:
                    gPcDSword.Btn_Click(IsRight);
                    break;
            }
        }


    }

    public static void DoThrowDamage(CNpcInst[] Arr, int Damage)
    {
        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.DoThrowDamage(Arr, Damage);
                break;
            case eCharType.eBigSword:
                gPcBigSword.DoThrowDamage(Arr, Damage);
                break;
        }
    }

    public static void CalcSceMove(Vector3 Delt)
    {
        gMySce.Move(Delt);
    }

    public static void RefreshHPUI(float BPerc, float EPerc, float RPerc, float V = 1f)
    {
        if (mCurUseGunType == eGunType.eLongGun)
            gPcLongGun.RefreshHPUI();
        else
            gPcShortGun.RefreshHPUI();


        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.mUIHp.JustPlay(BPerc, EPerc, RPerc, V);
                break;
            case eCharType.eBigSword:
                gPcBigSword.RefreshHPUI();
                break;
            case eCharType.eDSword:
                gPcDSword.mUIHp.JustPlay(BPerc, EPerc, RPerc, V);
                break;
        }

    }

    public static Transform GetPcRefMid()
    {

        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                return gPcLun.mRefPointMid.transform;

            case eCharType.eBigSword:
                return gPcBigSword.mRefPointMid.transform;

            case eCharType.eDSword:
                return gPcDSword.mRefPointMid.transform;
        }

        return null;

    }

    public static void ShowTip(string Str)
    {
        gMainUI.ShowTip(Str);
    }

    public static void PcBeAtk(int Damage, CNpcInst Npc = null)
    {
        if(gMecha!=null)
        {
            gMecha.BeAtk();
            return;
        }

        switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.BeAtk(Damage, Npc);
                break;

            case eCharType.eBigSword:
                gPcBigSword.BeAtk(Damage);
                break;

            case eCharType.eDSword:
                gPcDSword.BeAtk(Damage, Npc);
                break;
        }
    }

    public static void PcSetPos(Vector3 Pos)
    {
         switch (mCurUseGirlType)
        {
            case eCharType.eLun:
                gPcLun.gameObject.transform.position = Pos;
                break;

            case eCharType.eBigSword:
                gPcBigSword.gameObject.transform.position = Pos;
                break;

            case eCharType.eDSword:
                gPcDSword.gameObject.transform.position = Pos;
                break;
        }
    }

    public static void UpdateInGame()
    {
        gAtkNpcGlobleTeam.Update();
        gAirNpcGlobleTeam.Update();

        gBtnAnim.Update();

        gPlayerData.mGunCoolDown -= Time.deltaTime;

        if(gDefine.gPlayerData.mNeedToSaveBag)
        {
            gDefine.gPlayerData.mNeedToSaveBag = false;
              gDefine.gPlayerData.SaveBag();
        }
          

        // foreach( int id in mSoundIdArr)
        // {
        //      AudioClip clip = gData.GetSoundClip(id);
        //     if (clip != null)
        //         gSound.Play(clip);
        // }
        // mSoundIdArr.Clear();

        if(!gDefine.gPause)
            UpdateDarkShadow();
    }

    public static void Update()
    {
        //---
        if (gUIShowCoin > 0)
        {
            int num = Random.Range(2, 3);
            gUIShowCoin -= num;
            for (int i = 0; i < num; i++)
            {
                GameObject o = GameObject.Instantiate(gDefine.gData.mUICoinsSEPreb);
                o.transform.SetParent(gUICoinSERoot.transform);
                o.transform.localPosition = new Vector3(Random.Range(-250, 250), 0, 0);
                o.SetActive(true);

                ui_ItemDrop s = o.GetComponent<ui_ItemDrop>();
                s.Init();

            }
        }

        if (gUIShowCrystal > 0)
        {
            int num = Random.Range(2, 3);
            gUIShowCrystal -= num;

            for (int i = 0; i < num; i++)
            {
                GameObject o = GameObject.Instantiate(gDefine.gData.mUICrystalSEPreb);
                o.transform.SetParent(gUICoinSERoot.transform);
                o.transform.localPosition = new Vector3(Random.Range(-250, 250), 0, 0);
                o.SetActive(true);

                ui_ItemDrop s = o.GetComponent<ui_ItemDrop>();
                s.Init();

            }

        }

        if (gPlayerData.mTiLiT > 0 && System.DateTime.Now.Ticks >= gPlayerData.mTiLiT
            + ((long)20 * 60 * 10000000))
        {
            long tili = (System.DateTime.Now.Ticks - gPlayerData.mTiLiT) / ((long)20 * 60 * 10000000);
            gPlayerData.TiLI += (int)tili;
            if (gPlayerData.TiLI > 20)
                gPlayerData.TiLI = 20;

            gPlayerData.mTiLiT = System.DateTime.Now.Ticks;
        }

         if(gDefine.gPlayerData.mNeedToSaveBag)
        {
            gDefine.gPlayerData.mNeedToSaveBag = false;
              gDefine.gPlayerData.SaveBag();
        }


    }

    public static void UpdateDarkShadow()
    {
        //dark shadow..
        CGird gird = gPlayerData.mEquipGird[(int)eEuqipPos.Cloak];
        if (gird.mRefItem != null && gird.mRefItem.mSpecialIndex == 4 &&
           gDefine.gPlayerData.IsDarkShdowTReady())
        {
            Vector3 pos = gDefine.GetPCTrans().position;
            CNpcInst npc = gDefine.gNpc.FindByR(pos.x, 7, CNpcInst.eNpcClass.All);
            if (npc != null)
            {
                UseCloneShootGirlNow(pos, npc.GetPos().x > pos.x ? true : false, 5f);
                gDefine.gPlayerData.ResetDarkShadowT();
            }
        }
    }

    public static void CreateHalfDown(NpcMono Inst)
    {
        GameObject npc = GameObject.Instantiate(gNpcData.mNpcPrebArr[(int)npcdata.eNpcType.HalfDown]);
        Vector3 pos = Inst.mRefDHDPoint.transform.position;

        pos.y = gDefine.gGrounY;

        CNpcInst npcInst = gDefine.gNpc.CreateNpcInst(npcdata.eNpcType.HalfDown);

        npcInst.Init(npc, npcdata.eNpcType.HalfDown, pos, 0);
        npcInst.mCanBeCount = false;

        gDefine.gNpc.AddNpc(npcInst);



    }

    public static void CreateHalfUp(NpcMono Inst)
    {
        GameObject npc = GameObject.Instantiate(gNpcData.mNpcPrebArr[(int)npcdata.eNpcType.HalfUp]);
        Vector3 pos = Inst.mRefDHUPoint.transform.position;

        pos.y = gDefine.gGrounY;

        CNpcInst npcInst = gDefine.gNpc.CreateNpcInst(npcdata.eNpcType.HalfUp);

        npcInst.Init(npc, npcdata.eNpcType.HalfUp, pos, 0);
        npcInst.mCanBeCount = true;

        gDefine.gNpc.AddNpc(npcInst);
    }









}
