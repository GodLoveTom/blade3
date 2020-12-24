using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoxDataParam
{
    public long mT = 0; // 计时时间
    public BoxData.eBoxType mIndex; //自己的箱子索引
    public int[] mItemArr = null;  //物品
    public int[] mItemNumArr = null; //数量
    public int mFightCount; // 战斗的计数,或者被用于计算当前的vip是否可用
    List<int> mTmpArr = new List<int>(); //计算用
    public void Save()
    {
        PlayerPrefs.SetString("Box_" + mIndex.ToString(), mT.ToString());
        PlayerPrefs.SetInt("Box_" + mIndex.ToString() + "fightCount", mFightCount);

        if ((mIndex == BoxData.eBoxType.Piece && mFightCount >= 2) ||
        (mIndex == BoxData.eBoxType.Gem && mFightCount >= 3) ||
        (mIndex == BoxData.eBoxType.Scroll && mFightCount >= 4) ||
        (mIndex == BoxData.eBoxType.Vip && mFightCount > 0) ||
         (mIndex == BoxData.eBoxType.CombinPiece && mFightCount >= 4)
        )

            if (mItemArr != null)
            {
                for (int i = 0; i < mItemArr.Length; i++)
                {
                    PlayerPrefs.SetInt("Box_" + mIndex.ToString() + "_item_" + i.ToString(), mItemArr[i]);
                    if (mIndex != BoxData.eBoxType.Vip)
                        PlayerPrefs.SetInt("Box_" + mIndex.ToString() + "_itemNum_" + i.ToString(), mItemNumArr[i]);
                }
            }


    }

    public void Load()
    {
        string tStr = PlayerPrefs.GetString("Box_" + mIndex.ToString(), "0");
        mT = long.Parse(tStr);
        mFightCount = PlayerPrefs.GetInt("Box_" + mIndex.ToString() + "fightCount", 0);

        if (mIndex != BoxData.eBoxType.Login)
        {
            //if (mT == 0)
                //mT = System.DateTime.Now.Ticks;

            if ((mIndex == BoxData.eBoxType.Piece && mFightCount >= 2) ||
               (mIndex == BoxData.eBoxType.Gem && mFightCount >= 3) ||
               (mIndex == BoxData.eBoxType.Scroll && mFightCount >= 4) ||
               (mIndex == BoxData.eBoxType.Vip && mFightCount > 0) ||
                (mIndex == BoxData.eBoxType.CombinPiece && mFightCount >= 5))
            {
                int num = (mIndex == BoxData.eBoxType.Vip) ? 6 : 2;
                mItemArr = new int[num];
                mItemNumArr = new int[num];
                for (int i = 0; i < num; i++)
                {
                    mItemArr[i] = PlayerPrefs.GetInt("Box_" + mIndex.ToString() + "_item_" + i.ToString());
                    if (mIndex != BoxData.eBoxType.Vip)
                        mItemNumArr[i] = PlayerPrefs.GetInt("Box_" + mIndex.ToString() + "_itemNum_" + i.ToString());
                }
            }
        }
        Save();
    }

    public void Cancel()
    {
        mFightCount = 0;
        Save();
        PlayerPrefs.Save();
    }

    public void Active()
    {
        if (mIndex == BoxData.eBoxType.Piece && mFightCount < 2
            && System.DateTime.Now.Ticks >= mT + (long)8 * 3600 * 10000000)
        {
            mFightCount++;
            if (mFightCount >= 2)
            {
                //mT = System.DateTime.Now.Ticks;

                mItemArr = new int[2];
                mItemNumArr = new int[2];

                //计算两个碎片核心
                mTmpArr.Clear();
                for (int i = 194; i <= 198; i++)
                    mTmpArr.Add(i);

                mItemArr[0] = mTmpArr[Random.Range(0, mTmpArr.Count)];
                mTmpArr.Remove(mItemArr[0]);
                mItemArr[1] = mTmpArr[Random.Range(0, mTmpArr.Count)];

                mItemNumArr[0] = Random.Range(2, 4);
                mItemNumArr[1] = Random.Range(2, 4);
            }
            Save();
            PlayerPrefs.Save();
        }
        else if (mIndex == BoxData.eBoxType.Gem && mFightCount < 3 &&
       System.DateTime.Now.Ticks >= mT + (long)14 * 3600 * 10000000)
        {

            mFightCount++;
            if (mFightCount >= 3)
            {
                //mT = System.DateTime.Now.Ticks;
                mItemArr = new int[2];
                mItemNumArr = new int[2];

                //计算两个宝石
                mTmpArr.Clear();

                mTmpArr.Add(99);
                mTmpArr.Add(109);
                mTmpArr.Add(119);
                mTmpArr.Add(129);
                mTmpArr.Add(139);

                mItemArr[0] = mTmpArr[Random.Range(0, mTmpArr.Count)];
                mTmpArr.Remove(mItemArr[0]);
                mItemArr[1] = mTmpArr[Random.Range(0, mTmpArr.Count)];

                mItemNumArr[0] = Random.Range(2, 4);
                mItemNumArr[1] = Random.Range(2, 4);
            }
            Save();
            PlayerPrefs.Save();
        }
        else if (mIndex == BoxData.eBoxType.Scroll && mFightCount < 4
        && System.DateTime.Now.Ticks >= mT + (long)14 * 3600 * 10000000)
        {
            mFightCount++;
            if (mFightCount >= 4)
            {
                //mT = System.DateTime.Now.Ticks;
                mItemArr = new int[2];
                mItemNumArr = new int[2];

                //计算两个卷轴
                mTmpArr.Clear();

                for (int i = 73; i <= 91; i++)
                    mTmpArr.Add(i);
                for (int i = 93; i <= 95; i++)
                    mTmpArr.Add(i);
                for (int i = 97; i <= 99; i++)
                    mTmpArr.Add(i);

                mTmpArr.Add(204);
                mTmpArr.Add(205);
                mTmpArr.Add(203);

                mItemArr[0] = mTmpArr[Random.Range(0, mTmpArr.Count)];
                mTmpArr.Remove(mItemArr[0]);
                mItemArr[1] = mTmpArr[Random.Range(0, mTmpArr.Count)];

                mItemNumArr[0] = 1;
                mItemNumArr[1] = 1;
            }
            Save();
            PlayerPrefs.Save();
        }
        else if (mIndex == BoxData.eBoxType.Vip && mFightCount == 0)
        {
            mFightCount++;
            {
                mItemArr = new int[6];
                //计算两个碎片核心
                mTmpArr.Clear();
                for (int i = 194; i <= 198; i++)
                    mTmpArr.Add(i);

                mItemArr[0] = mTmpArr[Random.Range(0, mTmpArr.Count)];
                mTmpArr.Remove(mItemArr[0]);
                mItemArr[1] = mTmpArr[Random.Range(0, mTmpArr.Count)];

                //计算两个钻石
                mTmpArr.Clear();

                mTmpArr.Add(99);
                mTmpArr.Add(109);
                mTmpArr.Add(119);
                mTmpArr.Add(129);
                mTmpArr.Add(139);

                mItemArr[2] = mTmpArr[Random.Range(0, mTmpArr.Count)];
                mTmpArr.Remove(mItemArr[0]);
                mItemArr[3] = mTmpArr[Random.Range(0, mTmpArr.Count)];


                //计算两个卷轴
                mTmpArr.Clear();

                for (int i = 73; i <= 91; i++)
                    mTmpArr.Add(i);
                for (int i = 93; i <= 95; i++)
                    mTmpArr.Add(i);
                for (int i = 97; i <= 99; i++)
                    mTmpArr.Add(i);

                mTmpArr.Add(204);
                mTmpArr.Add(205);
                mTmpArr.Add(203);

                mItemArr[4] = mTmpArr[Random.Range(0, mTmpArr.Count)];
                mTmpArr.Remove(mItemArr[0]);
                mItemArr[5] = mTmpArr[Random.Range(0, mTmpArr.Count)];

            }
            Save();
            PlayerPrefs.Save();
        }
        else if (mIndex == BoxData.eBoxType.CombinPiece && mFightCount < 5
        && System.DateTime.Now.Ticks >= mT + (long)14 * 3600 * 10000000)
        {
            mFightCount++;
            if (mFightCount >= 5)
            {
                //mT = System.DateTime.Now.Ticks;
                mItemArr = new int[2];
                mItemNumArr = new int[2];

                //计算两个当前关卡掉落
                int chapterId = gDefine.gPlayerData.mChapterEx.GetOpenMaxChapterID(0);
                CChapterDropDataParam c = gDefine.gDropSystem.FindCharpter(chapterId);
                int index = Random.Range(0, c.mPiece.Count);
                mItemArr[0] = c.mPiece[index].Id;
                mItemArr[1] = c.mPiece[(index + 1) % c.mPiece.Count].Id;
                mItemNumArr[0] = mItemNumArr[1] = 1;
            }
            Save();
            PlayerPrefs.Save();
        }
    }

    public string GetBoxT()
    {
        if (mIndex == BoxData.eBoxType.Login)
        {
            if (System.DateTime.Now.Ticks >= mT + (long)8 * 3600 * 10000000)
                return "00:00:00";
            else
            {
                long s = 8 * 3600 - (System.DateTime.Now.Ticks - mT) / (long)10000000;
                long h = s / 3600;
                long m = s % 3600 / 60;
                s = s % 60;
                return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            }
        }
        else if (mIndex == BoxData.eBoxType.Piece)
        {
            if (System.DateTime.Now.Ticks >= mT + (long)8 * 3600 * 10000000)
            {
                if (mFightCount < 2)
                {
                    string str = gDefine.GetStr(431);
                    str = str.Replace("<<s>>", mFightCount.ToString() + "/2");
                    return str;
                }
                else
                {
                    return "00:00:00";
                }
            }
            else
            {
                long s = 8 * 3600 - (System.DateTime.Now.Ticks - mT) / (long)10000000;
                long h = s / 3600;
                long m = s % 3600 / 60;
                s = s % 60;
                return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            }
        }

        else if (mIndex == BoxData.eBoxType.Gem || mIndex == BoxData.eBoxType.Scroll)
        {
            if (System.DateTime.Now.Ticks >= mT + (long)14 * 3600 * 10000000)
            {
                int fightNum = (mIndex == BoxData.eBoxType.Gem) ? 3 : 4;
                if (mFightCount < fightNum)
                {
                    string str = gDefine.GetStr(431);
                    str = str.Replace("<<s>>", mFightCount.ToString() + "/" + fightNum.ToString());
                    return str;
                }
                else
                    return "00:00:00";
            }

            else
            {
                long s = 14 * 3600 - (System.DateTime.Now.Ticks - mT) / (long)10000000;
                long h = s / 3600;
                long m = s % 3600 / 60;
                s = s % 60;
                return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            }
        }
        else if (mIndex == BoxData.eBoxType.Vip)
        {
            if (mFightCount == 0)
                return "";
            else if (mT == 0)
                return "00:00:00";
            else
            {
                System.DateTime dt = new System.DateTime(mT);
                if (System.DateTime.Now.Day != dt.Day)
                    return "00:00:00";
                else
                {
                    long s = 60 - System.DateTime.Now.Second;
                    long h = 24 - System.DateTime.Now.Hour;
                    long m = 60 - System.DateTime.Now.Minute;
                    return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
                }
            }

        }
        else if (mIndex == BoxData.eBoxType.CombinPiece)
        {
            if (System.DateTime.Now.Ticks >= mT + (long)14 * 3600 * 10000000)
            {
                int fightNum = 5;
                if (mFightCount < fightNum)
                {
                    string str = gDefine.GetStr(431);
                    str = str.Replace("<<s>>", mFightCount.ToString() + "/" + fightNum.ToString());
                    return str;
                }
                else
                    return "00:00:00";
            }

            else
            {
                long s = 14 * 3600 - (System.DateTime.Now.Ticks - mT) / (long)10000000;
                long h = s / 3600;
                long m = s % 3600 / 60;
                s = s % 60;
                return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
            }
        }

        return "";
    }

    public bool CanGetPrize()
    {
        return GetBoxT() == "00:00:00";
    }

    public string GetBoxPrize(int Mult)
    {

        switch (mIndex)
        {
            case BoxData.eBoxType.Login:
                {
                    gDefine.gPlayerData.Coin += 200 * Mult;
                    gDefine.gPlayerData.Crystal += 200 * Mult;
                    mT = System.DateTime.Now.Ticks;

                    Save();
                    PlayerPrefs.Save();

                    int num = 200 * Mult;
                    string str = gDefine.GetStr("获得") + " " + gDefine.GetStr("金币") + num.ToString()
                    + " " + gDefine.GetStr("钻石") + num.ToString();
                    ui_BoxTip script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
                    script.Show(201, 200 * Mult, 202, 200 * Mult, true);

                    //gDefine.gMainUI.mRefMainBox.mBoxTip.Show(201, 200 * Mult, 202, 200 * Mult,true);

                    return str;
                }
            case BoxData.eBoxType.Piece:
            case BoxData.eBoxType.Scroll:
            case BoxData.eBoxType.Gem:
            case BoxData.eBoxType.CombinPiece:
                {
                    mT = System.DateTime.Now.Ticks;

                    for (int i = 0; i < 2; i++)
                    {
                        gDefine.gPlayerData.AddItemToBag(mItemArr[i], mItemNumArr[i] * Mult);
                    }

                    mFightCount = 0;
                    Save();
                    PlayerPrefs.Save();

                    //gDefine.gPlayerData.Save();
                    CItem it0 = gDefine.gData.GetItemData(mItemArr[0]);
                    CItem it1 = gDefine.gData.GetItemData(mItemArr[1]);
                    int num0 = mItemNumArr[0] * Mult;
                    int num1 = mItemNumArr[1] * Mult;

                    string str = gDefine.GetStr("获得") + " " + it0.GetNameLocal() + num0.ToString()
                    + " " + it1.GetNameLocal() + num1.ToString();

                    if (mIndex == BoxData.eBoxType.Gem)
                        str = gDefine.GetStr("获得") + " " + it0.GetGemNameLocal() + num0.ToString()
                    + " " + it1.GetGemNameLocal() + num1.ToString();
                    ui_BoxTip script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
                    script.Show(mItemArr[0], num0, mItemArr[1], num1, true);
                    return str;

                }


            case BoxData.eBoxType.Vip:
                {
                    gDefine.gPlayerData.Coin += 300 * Mult;
                    gDefine.gPlayerData.Crystal += 300 * Mult;

                    for (int i = 0; i < 6; i++)
                    {
                        gDefine.gPlayerData.AddItemToBag(mItemArr[i], Mult);
                    }

                    mT = System.DateTime.Now.Ticks;
                    mFightCount = 0;
                    Active();

                    Save();
                    PlayerPrefs.Save();

                    return "a lot of things.";
                }

        }

        return "";
    }

}

public class BoxData
{
    public enum eBoxType
    {
        Login = 0, // 登陆宝箱
        Piece, //核心
        Gem,//宝石宝箱
        Scroll,//卷轴宝箱
        Vip,//vip宝箱
        CombinPiece,//合成碎片宝箱
        Count,
    }
    public CBoxDataParam[] mBoxArr = new CBoxDataParam[(int)eBoxType.Count];
    // public long[] mBoxT = new long[4]; // <0 , 没有被激活
    // public int [] mBoxFightCount = new int[4]; // 战斗激活计数
    // public int [] mBox2PieceId;
    // public int mBox3ScrollId;
    // public bool mBox4IsCrystal;
    // public int mBox4GemId;

    public void Save()
    {
        for (int i = 0; i < (int)eBoxType.Count; i++)
            mBoxArr[i].Save();
        // PlayerPrefs.SetString("Box_" + i.ToString(), mBoxT[i].ToString());
        // PlayerPrefs.SetInt("mBox2PieceId", mBox2PieceId);
        // PlayerPrefs.SetInt("mBox3ScrollId", mBox3ScrollId);
        // PlayerPrefs.SetInt("mBox4IsCrystal", mBox4IsCrystal ? 1 : 0);
        // PlayerPrefs.SetInt("mBox4GemId", mBox4GemId);

        //PlayerPrefs.SetInt("BoxDataExist", 1);
    }

    public void Load()
    {
        for (int i = 0; i < (int)eBoxType.Count; i++)
        {
            mBoxArr[i] = new CBoxDataParam();
            mBoxArr[i].mIndex = (eBoxType)i;
            mBoxArr[i].Load();
        }

        // if (PlayerPrefs.GetInt("BoxDataExist", 0) == 1)
        // {
        //     for (int i = 0; i < (int)eBoxType.Count; i++)
        //         mBoxArr[i].Load();
        //     // for (int i = 0; i < 4; i++)
        //     // {
        //     //     string str = PlayerPrefs.GetString("Box_" + i.ToString(), "-1");
        //     //     mBoxT[i] = long.Parse(str);
        //     // }
        //     // mBox2PieceId = PlayerPrefs.GetInt("mBox2PieceId", 194);
        //     // mBox3ScrollId = PlayerPrefs.GetInt("mBox3ScrollId", 71);
        //     // mBox4IsCrystal = PlayerPrefs.GetInt("mBox4IsCrystal", 0) == 0 ? false : true;
        //     // mBox4GemId = PlayerPrefs.GetInt("mBox4GemId", 99);

        //     // if (mBox2PieceId == 0 || mBox3ScrollId == 0 || (mBox4IsCrystal == false && mBox4GemId == 0)
        //     // || mBox3ScrollId == 71 || mBox3ScrollId == 72 || mBox3ScrollId == 92 || mBox3ScrollId == 96)
        //     // {
        //     //     RefreshBox2();
        //     //     RefreshBox3();
        //     //     RefreshBox4();
        //     //     mBoxT[0] = mBoxT[1] = mBoxT[2] = mBoxT[3] = System.DateTime.Now.Ticks;
        //     //     Save();
        //     //     PlayerPrefs.Save();
        //     // }
        // }
    }

    public CBoxDataParam GetBox(eBoxType BoxType)
    {
        return mBoxArr[(int)BoxType];
    }

    public bool IsVipActive()
    {
        return PlayerPrefs.GetInt("vip") == 1;
    }

    public string GetBoxName(int BoxIndex)
    {
        switch (BoxIndex)
        {
            case 0:
                return gDefine.GetStr(425);
            case 1:
                return gDefine.GetStr(450);
            case 2:
                return gDefine.GetStr(427);
            case 3:
                return gDefine.GetStr(428);
            case 4:
                return gDefine.GetStr(424);
            case 5:
                return gDefine.GetStr(426);
        }
        return "";
    }

    public void FightActive()
    {
        for (eBoxType i = eBoxType.Piece; i < eBoxType.Count; i++)
        {
            if (i == eBoxType.Vip)
                continue;
            mBoxArr[(int)i].Active();
        }
    }

    public void VipActive()
    {
        mBoxArr[(int)eBoxType.Vip].Active();
    }

    public void VipCancel()
    {
        mBoxArr[(int)eBoxType.Vip].Cancel();
    }

    // public void RefreshBox2()
    // {
    //     if (System.DateTime.Now.Ticks >= mBoxT[1] + (long)3600 * 8 * 10000000)
    //     {
    //         mBox2PieceId = Random.Range(194, 199);
    //         mBoxT[1] = System.DateTime.Now.Ticks;
    //     }
    // }

    // public void RefreshBox3()
    // {
    //     if (System.DateTime.Now.Ticks >= mBoxT[2] + (long)3600 * 8 * 10000000)
    //     {
    //         List<int> tmp = new List<int>();
    //         for (int i = 73; i <= 91; i++)
    //             tmp.Add(i);
    //         tmp.Add(93); tmp.Add(94); tmp.Add(95);
    //         tmp.Add(97); tmp.Add(98);


    //         mBox3ScrollId = tmp[Random.Range(0, tmp.Count)];
    //         mBoxT[2] = System.DateTime.Now.Ticks;
    //     }
    // }

    // void RefreshBox4()
    // {
    //     if (System.DateTime.Now.Ticks >= mBoxT[3] + (long)3600 * 8 * 10000000)
    //     {
    //         int[] gemId = new int[] { 99, 109, 119, 129, 139};
    //         mBox4GemId = gemId[Random.Range(0, gemId.Length)];
    //         mBox4IsCrystal = Random.Range(0, 100) < 50 ? true : false;
    //         mBoxT[3] = System.DateTime.Now.Ticks;
    //     }
    // }

    public string GetBoxT(int BoxIndex)
    {
        return mBoxArr[BoxIndex].GetBoxT();
        // if (System.DateTime.Now.Ticks >= mBoxT[BoxIndex] + (long)8 * 3600 * 10000000)
        //     return "00:00:00";
        // else
        // {
        //     long s = 8 * 3600 - (System.DateTime.Now.Ticks - mBoxT[BoxIndex]) / (long)10000000;
        //     long h = s / 3600;
        //     long m = s % 3600 / 60;
        //     s = s % 60;

        //     return h.ToString("D2") + ":" + m.ToString("D2") + ":" + s.ToString("D2");
        // }
    }

    public bool IsCanGetBoxPrize(int BoxIndex)
    {
        return mBoxArr[BoxIndex].CanGetPrize();
        // if (System.DateTime.Now.Ticks < mBoxT[BoxIndex] + (long)8 * 3600 * 10000000)
        //     return false;
        // else
        //     return true;
    }

    public bool IsAnyBoxReady()
    {
        for (int i = 0; i < (int)eBoxType.Count; i++)
        {
            if (mBoxArr[i].CanGetPrize())
                return true;
        }
        // for (int i = 0; i < 4; i++)
        //     if (System.DateTime.Now.Ticks >= mBoxT[i] + (long)8 * 3600 * 10000000)
        //         return true;

        return false;
    }

    public string GetBoxPrize(int BoxIndex, int Multiple)
    {
        return mBoxArr[BoxIndex].GetBoxPrize(Multiple);
        // if (System.DateTime.Now.Ticks < mBoxT[BoxIndex] + (long)8 * 3600 * 10000000)
        //     return "";

        // int two = Multiple;
        // //int num=0; 
        // //string str="";
        // switch (BoxIndex)
        // {
        //     case 0:
        //         {
        //             gDefine.gPlayerData.Coin += 200 * two;
        //             gDefine.gPlayerData.Crystal += 200 * two;
        //             mBoxT[0] = System.DateTime.Now.Ticks;

        //             Save();
        //             PlayerPrefs.Save();

        //             int num = 200 * two;
        //             string str = gDefine.GetStr("获得") + " " + gDefine.GetStr("金币") + num.ToString()
        //             + " " + gDefine.GetStr("钻石") + num.ToString();

        //             gDefine.gMainUI.mRefMainBox.mBoxTip.Show(201, 200 * two, 202, 200 * two);

        //             return str;
        //         }


        //     case 1:
        //         gDefine.gPlayerData.AddItemToBag(mBox2PieceId, 1 * two);
        //         gDefine.gPlayerData.Crystal += 200 * two;

        //         //gDefine.gPlayerData.Save();
        //         CItem it = gDefine.gData.GetItemData(mBox2PieceId);

        //         int num1 = 200 * two;
        //         string str1 = gDefine.GetStr("获得") + " " + it.GetGemNameLocal() + two.ToString()
        //         + " " + gDefine.GetStr("钻石") + num1.ToString();

        //         gDefine.gMainUI.mRefMainBox.mBoxTip.Show(mBox2PieceId, two, 202, 200 * two);

        //         RefreshBox2();
        //         Save();
        //         PlayerPrefs.Save();

        //         return str1;


        //     case 2:
        //         gDefine.gPlayerData.AddItemToBag(mBox3ScrollId, 1 * two);
        //         gDefine.gPlayerData.Coin += 200 * two;

        //         CItem it0 = gDefine.gData.GetItemData(mBox3ScrollId);

        //         int num2 = 200 * two;
        //         string str2 = gDefine.GetStr("获得") + " " + it0.GetGemNameLocal() + two.ToString()
        //         + " " + gDefine.GetStr("金币") + num2.ToString();

        //         gDefine.gMainUI.mRefMainBox.mBoxTip.Show(mBox3ScrollId, two, 201, 200 * two);

        //         RefreshBox3();
        //         Save();
        //         PlayerPrefs.Save();

        //         return str2;

        //     case 3:
        //         gDefine.gPlayerData.AddItemToBag(mBox4GemId, 1 * two);
        //         CItem it1 = gDefine.gData.GetItemData(mBox4GemId);
        //         if (mBox4IsCrystal)
        //         {
        //             gDefine.gPlayerData.Crystal += 200 * two;


        //             int num3 = 200 * two;
        //             string str3 = gDefine.GetStr("获得") + " " + it1.GetGemNameLocal() + two.ToString()
        //             + " " + gDefine.GetStr("水晶") + num3.ToString();

        //             gDefine.gMainUI.mRefMainBox.mBoxTip.Show(mBox4GemId, two, 202, 200 * two);


        //             RefreshBox4();
        //             Save();
        //             PlayerPrefs.Save();


        //             return str3;
        //         }
        //         else
        //         {
        //             gDefine.gPlayerData.Coin += 200 * two;



        //             int num3 = 200 * two;
        //             string str3 = gDefine.GetStr("获得") + " " + it1.GetGemNameLocal() + two.ToString()
        //             + " " + gDefine.GetStr("金币") + num3.ToString();

        //             gDefine.gMainUI.mRefMainBox.mBoxTip.Show(mBox4GemId, two, 201, 200 * two);


        //             RefreshBox4();
        //             Save();
        //             PlayerPrefs.Save();

        //             return str3;
        //         }
        // }

        // return "";

    }

}
