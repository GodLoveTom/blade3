using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainFightDropData
{
    public int mItemId;
    public int mNum;
}

public class ui_MainFight : MonoBehaviour
{
    public ui_MainFightDrop mRefUIDrop;
    //int mCurCharpterId = 0;
    public Texture2D[] mTexArr;
    public RawImage mBackGround;
    public Text mNameText;
    public Text mLvLText;
    public string[] mNameArr;
    public Text mBeginText;
    public GameObject mRootCtl; //屏幕适应的根结点
    public GameObject mLockCtl;

    Vector2 mDragBPos;
    bool mIsDrag = false;
    bool mIsFirstShow = true;

    public GameObject mIapBtnObj;

    public Text mDifficultText;
    public Text mBoxText;
    public Image mDifficltTex;
    public GameObject mDiffictltLockObj; //难度的锁
    public ui_fightBox mRefBox;
    public GameObject mBoxlockObj;
    public Text mBoxTipText;
    float mBoxTipT = 0;

    public Material mGrayMat;



    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void Btn_ShowDropCtl()
    {
        mRefUIDrop.Show();
        mRefUIDrop.gameObject.GetComponent<ui_ScaleBig>().Play();
        gDefine.PlaySound(74);
    }

    public void ShowBoxTip(string Str)
    {
        mBoxTipText.text = Str;
        gDefine.SetTextBold();
        mBoxTipT = Time.time + 1f;
        mBoxTipText.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Time.time > mBoxTipT)
            mBoxTipText.gameObject.SetActive(false);
    }

    public void Btn_ChangeDiffcult()
    {
        gDefine.gChapterDifficult = (gDefine.gChapterDifficult + 1) % 4;
        gDefine.PlaySound(74);
        // if (gDefine.gChapterDifficult > gDefine.gPlayerData.GetChapterDifficult(gDefine.gChapterId))
        // {
        //     if(gDefine.gChapterDifficult == 1)
        //         gDefine.ShowTip(gDefine.GetStr(442));
        //     else if(gDefine.gChapterDifficult == 2)
        //         gDefine.ShowTip(gDefine.GetStr(443));
        //     else if(gDefine.gChapterDifficult == 3)
        //         gDefine.ShowTip(gDefine.GetStr(444));
        // }

        Refresh();
    }

    public void Btn_BoxClick()
    {
        if (gDefine.gPlayerData.mChapterEx.IsBoxCanGet(gDefine.gChapterId, gDefine.gChapterDifficult))
        {
            //get prize.
            GetPrize();
            Refresh();
        }
        else
        {
            gDefine.PlaySound(74);
            string str = gDefine.GetStr(446);
            int boxIndex = gDefine.gPlayerData.mChapterEx.GetCurBoxIndex(gDefine.gChapterId, gDefine.gChapterDifficult);
            if (boxIndex == 0)
            {
                str = str.Replace("<<s0>>", "4");
                str = str.Replace("<<s1>>", "2");
                str = str.Replace("<<s2>>", "3");
                str = str.Replace("<<s3>>", "100");
            }
            else if (boxIndex == 1)
            {
                str = str.Replace("<<s0>>", "5");
                str = str.Replace("<<s1>>", "3");
                str = str.Replace("<<s2>>", "4");
                str = str.Replace("<<s3>>", "150");
            }
            else if (boxIndex == 2)
            {
                str = str.Replace("<<s0>>", "6");
                str = str.Replace("<<s1>>", "4");
                str = str.Replace("<<s2>>", "5");
                str = str.Replace("<<s3>>", "100");
            }
            else if (boxIndex == 3)
            {
                str = str.Replace("<<s0>>", "7");
                str = str.Replace("<<s1>>", "5");
                str = str.Replace("<<s2>>", "6");
                str = str.Replace("<<s3>>", "250");
            }
            else if (boxIndex == 4)
            {
                str = gDefine.GetStr(447);
                str = str.Replace("<<s0>>", "10");
                str = str.Replace("<<s1>>", "10");
                str = str.Replace("<<s2>>", "10");
                str = str.Replace("<<s3>>", "200");
            }

            ShowBoxTip(str);
        }
    }

    void GetPrize()
    {
        int boxIndex = gDefine.gPlayerData.mChapterEx.GetCurBoxIndex(gDefine.gChapterId, gDefine.gChapterDifficult);
        List<ui_MainFightDropData> arr = new List<ui_MainFightDropData>();
        if (boxIndex == 0)
        {
            GetPieces(4, arr);
            GetGems(2, arr);
            GetCores(3, arr);
            GetCoins(100, arr);

        }
        else if (boxIndex == 1)
        {
            GetPieces(5, arr);
            GetGems(3, arr);
            GetCores(4, arr);
            GetCoins(150, arr);
        }
        else if (boxIndex == 2)
        {
            GetPieces(6, arr);
            GetGems(4, arr);
            GetCores(5, arr);
            GetCoins(100, arr);
        }
        else if (boxIndex == 3)
        {
            GetPieces(7, arr);
            GetGems(5, arr);
            GetCores(6, arr);
            GetCoins(250, arr);
        }
        else if (boxIndex == 4)
        {
            GetPieces(10, arr);
            GetGems(10, arr);
            GetCores(10, arr);
            GetCrystal(200, arr);
        }


        for (int i = 0; i < arr.Count; i++)
        {
            if (arr[i].mItemId == 201)
                gDefine.gPlayerData.Coin += arr[i].mNum;
            else if (arr[i].mItemId == 202)
                gDefine.gPlayerData.Crystal += arr[i].mNum;
            else
            {
                gDefine.gPlayerData.AddItemToBag(arr[i].mItemId, arr[i].mNum);
            }
        }

        int num = (arr.Count + 1) / 2;

        for (int i = 0; i < num; i++)
        {
            if (i * 2 + 1 >= arr.Count)
                gDefine.ShowTipEX((int)arr[i * 2].mItemId, (int)arr[i * 2].mNum, -1, 0, (num - i - 1) * 0.4f);
            else
                gDefine.ShowTipEX((int)arr[i * 2].mItemId, (int)arr[i * 2].mNum, (int)arr[i * 2 + 1].mItemId, (int)arr[i * 2 + 1].mNum, (num - i - 1) * 0.4f);

        }

        gDefine.gPlayerData.mChapterEx.FinishBox(gDefine.gChapterId, gDefine.gChapterDifficult);
    }

    void GetPieces(int Num, List<ui_MainFightDropData> Arr)
    {
        CChapterDropDataParam c = gDefine.gDropSystem.FindCharpter(gDefine.gChapterId);
        for (int i = 0; i < Num; i++)
        {
            int index = Random.Range(0, c.mPiece.Count);
            int pieceId = c.mPiece[index].Id;
            for (int j = 0; j < Arr.Count; j++)
            {
                if ((int)Arr[j].mItemId == pieceId)
                {
                    Arr[j].mNum += 1;
                    goto Next;
                }
            }
            ui_MainFightDropData data = new ui_MainFightDropData();
            data.mItemId = pieceId;
            data.mNum = 1;

            Arr.Add(data);

        Next:;

        }
    }

    void GetGems(int Num, List<ui_MainFightDropData> Arr)
    {
        int[] gems = new int[] { 99, 109, 119, 129, 139 };
        for (int i = 0; i < Num; i++)
        {
            int gemId = gems[Random.Range(0, gems.Length)];
            for (int j = 0; j < Arr.Count; j++)
            {
                if (Arr[j].mItemId == gemId)
                {
                    Arr[j].mNum += 1;
                    goto Next;
                }
            }
            ui_MainFightDropData data = new ui_MainFightDropData();
            data.mItemId = gemId;
            data.mNum = 1;

            Arr.Add(data);

        Next:;

        }
    }

    void GetCores(int Num, List<ui_MainFightDropData> Arr)
    {
        int[] cores = new int[] { 194, 195, 196, 197, 198 };
        for (int i = 0; i < Num; i++)
        {
            int id = cores[Random.Range(0, cores.Length)];
            for (int j = 0; j < Arr.Count; j++)
            {
                if (Arr[j].mItemId == id)
                {
                    Arr[j].mNum += 1;
                    goto Next;
                }
            }
            ui_MainFightDropData data = new ui_MainFightDropData();
            data.mItemId = id;
            data.mNum = 1;

            Arr.Add(data);

        Next:;

        }
    }

    void GetCoins(int Num, List<ui_MainFightDropData> Arr)
    {
        for (int j = 0; j < Arr.Count; j++)
        {
            if (Arr[j].mItemId == 201)
            {
                Arr[j].mNum += Num;
                return;
            }
        }
        ui_MainFightDropData data = new ui_MainFightDropData();
        data.mItemId = 201;
        data.mNum = Num;
        Arr.Add(data);

    }

    void GetCrystal(int Num, List<ui_MainFightDropData> Arr)
    {
        for (int j = 0; j < Arr.Count; j++)
        {
            if (Arr[j].mItemId == 202)
            {
                Arr[j].mNum += Num;
                return;
            }
        }
        ui_MainFightDropData data = new ui_MainFightDropData();
        data.mItemId = 202;
        data.mNum = Num;
        Arr.Add(data);
    }

    public void Btn_BeginGame()
    {
        if (gDefine.gChapterDifficult > gDefine.gPlayerData.GetChapterDifficult(gDefine.gChapterId))
        {
            gDefine.ShowTip(CalcDifficultTipText());
            return;
        }

        if (gDefine.gPlayerData.TiLI < 5)
        {
            gDefine.gMainUI.ChangeToBuyTiLi();
            return;
        }

        if (gDefine.gChapterId <= gDefine.gPlayerData.mChapterEx.GetOpenMaxChapterID(gDefine.gChapterDifficult) &&
        gDefine.gPlayerData.TiLI >= 5)
        {
            gDefine.gForbidLvL = 0;
            gDefine.gPlayerData.TiLI -= 5;
            //gDefine.gChapterId = mCurCharpterId + 1;
            PlayerPrefs.SetInt("continueChapterId", gDefine.gChapterId);
            CChapterData chapter = gDefine.gData.GetChapterData(gDefine.gChapterId);
            gDefine.gLVLNumInChapter = chapter.GetWaveNum();
            gDefine.ShowGameSce(0);
            gDefine.gMainUI.Close();

        }
        // else
        // {
        //     gDefine.ShowTip(gDefine.GetStr(361));
        // }
    }

    public void ContinueGame()
    {
        int continueChapterId = 1;
        int continueWaveLvL = 0;
        if (!gDefine.gIsFirstInGame)
        {
            continueChapterId = PlayerPrefs.GetInt("continueChapterId", 1);
            continueWaveLvL = PlayerPrefs.GetInt("continueWaveLvL", 0);
        }

        gDefine.gForbidLvL = continueWaveLvL + 1;
        gDefine.gChapterId = continueChapterId;

        CChapterData chapter = gDefine.gData.GetChapterData(gDefine.gChapterId);
        gDefine.gLVLNumInChapter = chapter.GetWaveNum();
        gDefine.ShowGameSce(continueWaveLvL);
        gDefine.gMainUI.Close();
    }

    public void Btn_Left()
    {
        if (gDefine.gChapterId > 1)
        {
            gDefine.gChapterId--;
            Refresh();
            gDefine.PlayUIClickSound();
        }
    }

    public void Btn_Right()
    {
        if (gDefine.gChapterId < 10)
        {
            gDefine.gChapterId++;
            Refresh();
            gDefine.PlayUIClickSound();
        }
    }

    public void Refresh()
    {
        if (mIsFirstShow)
        {
            //gDefine.gChapterId = PlayerPrefs.GetInt("MaxChapterId", 1) ;
            gDefine.gChapterId = gDefine.gPlayerData.mChapterEx.GetOpenMaxChapterID(gDefine.gChapterDifficult);

            if (gDefine.gChapterId < 1)
                gDefine.gChapterId = 1;

            mIsFirstShow = false;
        }

        // if (PlayerPrefs.GetInt("iap_door") == 1)
        //     mIapBtnObj.SetActive(true);
        // else
#if IAPWORLD
        mIapBtnObj.SetActive(true);
#else
        mIapBtnObj.SetActive(false);
#endif


        mBackGround.texture = mTexArr[gDefine.gChapterId - 1];
        string str = gDefine.gMyStr.Get(mNameArr[gDefine.gChapterId - 1], gDefine.gPlayerData.mLanguageType);
        mNameText.text = (gDefine.gChapterId).ToString() + ": " + str;
        gDefine.SetTextBold();
        str = gDefine.gMyStr.Get("最高纪录", gDefine.gPlayerData.mLanguageType);

        mLvLText.text = str + " : " + gDefine.gPlayerData.mChapterEx.GetChapterFinishLvL(gDefine.gChapterId, gDefine.gChapterDifficult)//+ gDefine.gNpcData.getChapterWaveNum(mCurCharpterId).ToString()
        + "/" + ((gDefine.gChapterId == 1) ? "30" : "50");
        gDefine.SetTextBold();
        str = gDefine.gMyStr.Get("开    始", gDefine.gPlayerData.mLanguageType);

        mBeginText.text = str;
        gDefine.SetTextBold();
        gDefine.RecalcAutoSizeNew(mRootCtl);

        if (gDefine.gChapterId > gDefine.gPlayerData.mChapterEx.GetOpenMaxChapterID(gDefine.gChapterDifficult))
            mLockCtl.SetActive(true);
        else
            mLockCtl.SetActive(false);

        //难度
        mDifficltTex.sprite = GetChapterDifficultSprite();
        mDifficultText.text = CalcChapterDifficultText();
        gDefine.SetTextBold();
        if (gDefine.gChapterDifficult > gDefine.gPlayerData.GetChapterDifficult(gDefine.gChapterId))
            mDifficltTex.material = mGrayMat;
        else
            mDifficltTex.material = null;

        //宝箱
        CChapterEXDifficult boxdata = gDefine.gPlayerData.mChapterEx.GetBoxParam(gDefine.gChapterId, gDefine.gChapterDifficult);
        if (boxdata.IsBoxExist())
        {
            if (boxdata.IsBoxCanGet())
            {
                mRefBox.gameObject.GetComponent<Image>().material = null;
                mRefBox.gameObject.GetComponent<ui_Rock>().PlayRock();
            }

            else
            {
                mRefBox.gameObject.GetComponent<Image>().material = mGrayMat;
                mRefBox.gameObject.GetComponent<ui_Rock>().Stop();
            }


            mRefBox.SetImage(boxdata.mBoxIndex);
            mBoxText.text = CalcBoxText(boxdata.mBoxIndex);
            gDefine.SetTextBold();

            mRefBox.gameObject.SetActive(true);
            mBoxText.gameObject.SetActive(true);
        }

        else
        {
            mRefBox.gameObject.SetActive(false);
            mBoxText.gameObject.SetActive(false);
        }

        mRefUIDrop.gameObject.SetActive(false);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    string CalcBoxText(int Index)
    {
        return gDefine.GetStr(445) + gDefine.gChapterId.ToString() + "-" + (Index * 10 + 10).ToString();
    }

    Sprite GetChapterDifficultSprite()
    {
        switch (gDefine.gChapterDifficult)
        {
            case 0:
                return gDefine.gABLoad.GetSprite("icon.bytes", "普通");
            case 1:
                return gDefine.gABLoad.GetSprite("icon.bytes", "英雄");
            case 2:
                return gDefine.gABLoad.GetSprite("icon.bytes", "深渊");
            case 3:
                return gDefine.gABLoad.GetSprite("icon.bytes", "星魂");
        }
        return null;
    }

    public string CalcChapterDifficultText()
    {
        switch (gDefine.gChapterDifficult)
        {
            case 0:
                return gDefine.GetStr(438);
            case 1:
                return gDefine.GetStr(439);
            case 2:
                return gDefine.GetStr(440);
            case 3:
                return gDefine.GetStr(441);
        }
        return "";
    }

    public string CalcDifficultTipText()
    {
        switch (gDefine.gChapterDifficult)
        {
            case 1:
                return gDefine.GetStr(442);
            case 2:
                return gDefine.GetStr(443);
            case 3:
                return gDefine.GetStr(444);
        }
        return "";
    }

    public void Event_BeginDrag()
    {
        mDragBPos = Input.mousePosition;
        mIsDrag = true;
    }

    public void Event_EndDrag()
    {
        mIsDrag = false;
        Vector3 pos = Input.mousePosition;
        if (pos.x > mDragBPos.x)
        {
            if (gDefine.gChapterId > 1)
            {
                gDefine.gChapterId--;
                Refresh();
                gDefine.PlayUIClickSound();
            }

        }
        else if (pos.x < mDragBPos.x)
        {
            if (gDefine.gChapterId < 10)
            {
                gDefine.gChapterId++;
                Refresh();
                gDefine.PlayUIClickSound();
            }

        }

    }


}
