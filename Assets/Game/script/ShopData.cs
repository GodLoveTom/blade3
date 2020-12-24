//商店数据
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ShopData
{
    public int mId;
    public int mItemId;
    public int mItemNum;
    public int mMoney;
    public int mMoneyType; //0 coin 1 crystal
    public int mChapterLockId;
    public bool mIsSold = false;

    public void ReadData(string[] StrArr)
    {
        mId = int.Parse(StrArr[0]);
        mItemId = int.Parse(StrArr[1]);
        mItemNum = int.Parse(StrArr[2]);
        mMoneyType = StrArr[3] == "金币" ? 0 : 1;
        mMoney = int.Parse(StrArr[4]);
    }
}

public class Shop
{
    List<ShopData> mDict = new List<ShopData>();
    List<ShopData> mOutDict = new List<ShopData>();

    List<ShopData> tmpDict = new List<ShopData>();
    public long mLastAutoRefreshT; //自动2小时刷新计时
    public int mTodayRefreshCount; //今日刷新次数
    public int mTodayFreeRefreshCount;
    public long mLastDayT; //今日刷新计时
    bool mNeedAutoRefresh = false;

    public enum eRefreshType
    {
        Auto,
        Free,
        AD,
        Crystal,
    }

    public void LoadRes(string Str)
    {
        string str = Str;
        string[] sepStr = new string[] { "\r\n" };
        string[] sepStr1 = new string[] { "\t" };
        string[] waveArr = str.Split(sepStr, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < waveArr.Length; i++)
        {
            string[] valueArr = waveArr[i].Split(sepStr1, StringSplitOptions.RemoveEmptyEntries);

            ShopData data = new ShopData();
            data.ReadData(valueArr);
            mDict.Add(data);
        }
    }

    public int GetTodayRefreshCount()
    {
        System.DateTime dt = new DateTime(mLastDayT);
        if (dt.Year != System.DateTime.Now.Year || dt.Month != System.DateTime.Now.Month
        || dt.Day != System.DateTime.Now.Day)
        {
            mTodayRefreshCount = 4;
            mTodayFreeRefreshCount = 1;
            mLastDayT = System.DateTime.Now.Ticks;
        }
        return mTodayRefreshCount;
    }

    public int GetTodayFreeRefreshCount()
    {
        System.DateTime dt = new DateTime(mLastDayT);
        if (dt.Year != System.DateTime.Now.Year || dt.Month != System.DateTime.Now.Month
        || dt.Day != System.DateTime.Now.Day)
        {
            mTodayRefreshCount = 4;
            mTodayFreeRefreshCount = 1;
            mLastDayT = System.DateTime.Now.Ticks;
        }
        return mTodayFreeRefreshCount;
    }


    public string GetAutoRefreshTStr()
    {
        long t = System.DateTime.Now.Ticks - mLastAutoRefreshT;
        if (t > (long)7200 * 10000000)
        {
            Recalc();
            mLastAutoRefreshT = System.DateTime.Now.Ticks;
            Save();
            PlayerPrefs.Save();
            return "00:00:00";
        }
        else
        {
            t = 7200 - t / 10000000;
            long hour = t / 3600;
            long min = (t % 3600) / 60;
            long s = t % 60;
            return hour.ToString() + ":" + min.ToString() + ":" + s.ToString();
        }
    }

    public bool Refresh(eRefreshType RefreshType)
    {
        if (RefreshType == eRefreshType.Free)
        {
            if (mTodayFreeRefreshCount > 0)
            {
                mTodayFreeRefreshCount--;
                Recalc();
                return true;
            }
        }
        else if (RefreshType == eRefreshType.Crystal)
        {
            if (gDefine.gPlayerData.Crystal >= 100)
            {
                gDefine.gPlayerData.Crystal -= 100;
                mTodayRefreshCount--;
                Recalc();
                return true;
            }
        }
        else if (RefreshType != eRefreshType.Free)
        {
            Recalc();
            if (RefreshType == eRefreshType.Auto)
                mLastAutoRefreshT = System.DateTime.Now.Ticks;

            return true;

        }

        return false;



    }

    public void Save()
    {
        PlayerPrefs.SetString("shop_mLastAutoRefreshT", mLastAutoRefreshT.ToString());

        PlayerPrefs.SetInt("shop_mTodayRefreshCount", mTodayRefreshCount);

        PlayerPrefs.SetInt("shop_mTodayFreeRefreshCount", mTodayFreeRefreshCount);

        PlayerPrefs.SetString("shop_mLastDayT", mLastDayT.ToString());

        for (int i = 0; i < mOutDict.Count; i++)
        {
            PlayerPrefs.SetInt("shop_data_" + i.ToString(), mOutDict[i].mItemId);
            PlayerPrefs.SetInt("shop_data_sold_" + i.ToString(), mOutDict[i].mIsSold ? 1 : 0);

        }


        PlayerPrefs.SetInt("shop_data_num", mOutDict.Count);


    }

    public void Load()
    {
        string str = PlayerPrefs.GetString("shop_mLastAutoRefreshT");
        if (!string.IsNullOrEmpty(str))
            mLastAutoRefreshT = long.Parse(str);

        mTodayRefreshCount = PlayerPrefs.GetInt("shop_mTodayRefreshCount");

        mTodayFreeRefreshCount = PlayerPrefs.GetInt("shop_mTodayFreeRefreshCount");

        str = PlayerPrefs.GetString("shop_mLastDayT");
        if (!string.IsNullOrEmpty(str))
            mLastDayT = long.Parse(str);

        int num = PlayerPrefs.GetInt("shop_data_num", 0);
        for (int i = 0; i < num; i++)
        {
            int itemId = PlayerPrefs.GetInt("shop_data_" + i.ToString(), 0);
            bool isSold = PlayerPrefs.GetInt("shop_data_sold_" + i.ToString(), 0) == 1 ? true : false;
            if (itemId > 0)
            {
                ShopData d = new ShopData();
                d.mItemId = itemId;
                d.mIsSold = isSold;
                d.mItemNum = 1;
                mOutDict.Add(d);
            }
        }

    }

    ShopData GetShopData(int Id)
    {
        foreach (var item in mDict)
        {
            if (item.mId == Id)
                return item;
        }
        return null;
    }

    public ShopData[] GetData()
    {
        return mOutDict.ToArray();
    }

    void Recalc()
    {
        //一共12个
        mOutDict.Clear();

        // if (UnityEngine.Random.Range(0, 100) < 60)
        // {
        //     if (UnityEngine.Random.Range(0, 100) < 50)
        //     {
        //         ShopData boxCoin = new ShopData();
        //         boxCoin.mItemId = 199;
        //         boxCoin.mIsSold = false;
        //         boxCoin.mItemNum = 1;

        //         mOutDict.Add(boxCoin);
        //     }
        //     else
        //     {
        //         ShopData boxCrystal = new ShopData();
        //         boxCrystal.mItemId = 200;
        //         boxCrystal.mIsSold = false;
        //         boxCrystal.mItemNum = 1;

        //         mOutDict.Add(boxCrystal);
        //     }



        // }
        // else
        {
            ShopData boxCoin = new ShopData();
            boxCoin.mItemId = 199;
            boxCoin.mIsSold = false;
            boxCoin.mItemNum = 1;

            mOutDict.Add(boxCoin);

            ShopData boxCrystal = new ShopData();
            boxCrystal.mItemId = 200;
            boxCrystal.mIsSold = false;
            boxCrystal.mItemNum = 1;

            mOutDict.Add(boxCrystal);
        }


        //根据关卡解锁获取装备，分为武器和装备两类
        //分别选择出一个武器，和两个其他防具饰品
        mOutDict.Add(CalcWeapon());
        ShopData[] arr = CalcEquip();
        for (int i = 0; i < arr.Length; i++)
            mOutDict.Add(arr[i]);

        //一级宝石，3-6颗
        List<int> tmpGem = new List<int>(new int[] { 99, 109, 119, 129, 139 });
        int num = UnityEngine.Random.Range(3, 6);
        for (int i = 0; i < num; i++)
        {
            if (tmpGem.Count == 0)
                tmpGem = new List<int>(new int[] { 99, 109, 119, 129, 139 });

            int index = UnityEngine.Random.Range(0, tmpGem.Count);
            int gemId = tmpGem[index];
            tmpGem.RemoveAt(index);

            ShopData data = new ShopData();
            data.mItemId = gemId;
            data.mItemNum = 1;
            data.mIsSold = false;

            mOutDict.Add(data);
        }


        //准备技能卷轴和装备碎片
        //剩余格子都从卷轴，碎片中出
        List<int> tmp = new List<int>(new int[] { 194, 195, 196, 197, 198 });
        num = UnityEngine.Random.Range(0,100);
        if( num < 10)
            num = 3;
        else if(num <35)
            num = 2;
        else
            num = 1;

        for(int i=0; i<num; i++)
        {
            int index = UnityEngine.Random.Range(0, tmp.Count);
            int itId = tmp[index];
            tmp.RemoveAt(index);

            ShopData data = new ShopData();
            data.mItemId = itId;
            data.mItemNum = 1;
            data.mIsSold = false;

            mOutDict.Add(data);
        }

        tmp.Clear();
        //
    
        for (int i = 73; i <= 91; i++)
            tmp.Add(i);
        for (int i = 93; i <= 95; i++)
            tmp.Add(i);
        for (int i = 97; i <= 99; i++)
            tmp.Add(i);

        tmp.Add(204);
        tmp.Add(205);
        tmp.Add(203);

        num = 12 - mOutDict.Count;
        for (int i = 0; i < num; i++)
        {
            int index = UnityEngine.Random.Range(0, tmp.Count);
            int itId = tmp[index];
            tmp.RemoveAt(index);

            ShopData data = new ShopData();
            data.mItemId = itId;
            data.mItemNum = 1;
            data.mIsSold = false;

            mOutDict.Add(data);
        }


        // foreach (var item in mDict)
        // {
        //     if (item.mChapterLockId <= gDefine.gPlayerData.MaxChapterId)
        //         tmpDict.Add(item);
        // }

        // int count = tmpDict.Count;
        // if (count > 8 && count < 16)
        //     count = 8;
        // else if (count >= 16 && count <= 32)
        //     count = 12;
        // else if (count > 32)
        //     count = 16;

        // mOutDict.Clear();
        // for (int i = 0; i < count; i++)
        // {
        //     int index = UnityEngine.Random.Range(0, tmpDict.Count);
        //     mOutDict.Add(tmpDict[index]);
        //     tmpDict.RemoveAt(index);
        // }
        // mLastAutoRefreshT = System.DateTime.Now.Ticks;
        Save();
    }

    public ShopData CalcWeapon()
    {
        List<int> tmp = new List<int>(new int[] { 1, 6, 11, 16, 21, 26, 51, 56, 61, 66 });

        int maxChapterId = gDefine.gPlayerData.mChapterEx.GetOpenMaxChapterID(0);

        if (maxChapterId >= 3)
        {
            int[] arr = new int[] { 2, 7, 12, 17, 22, 27, 52, 57, 62, 67 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        if (maxChapterId >= 5)
        {
            int[] arr = new int[] { 3, 8, 13, 18, 23, 28, 53, 58, 63, 68 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        if (maxChapterId >= 7)
        {
            int[] arr = new int[] { 4, 9, 14, 19, 24, 29, 54, 59, 64, 69 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        if (maxChapterId >= 9)
        {
            int[] arr = new int[] { 5, 10, 15, 20, 25, 30, 55, 60, 65, 70 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        int Index = UnityEngine.Random.Range(0, tmp.Count);
        int itId = tmp[Index];

        ShopData data = new ShopData();
        data.mItemId = itId;
        data.mIsSold = false;
        data.mItemNum = 1;
        return data;

    }

    public ShopData[] CalcEquip()
    {
        List<int> tmp = new List<int>(new int[] { 31, 36, 41, 159, 164, 169, 174, 179, 184, 189 });
        int maxChapterId = gDefine.gPlayerData.mChapterEx.GetOpenMaxChapterID(0);
        if (maxChapterId >= 3)
        {
            int[] arr = new int[] { 32, 37, 42, 47, 160, 165, 170, 175, 180, 185, 190 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        if (maxChapterId >= 5)
        {
            int[] arr = new int[] { 33, 38, 43, 48, 161, 166, 171, 176, 181, 186, 191 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        if (maxChapterId >= 7)
        {
            int[] arr = new int[] { 34, 39, 44, 49, 162, 167, 172, 177, 182, 187, 192 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        if (maxChapterId >= 9)
        {
            int[] arr = new int[] { 35, 40, 45, 50, 163, 168, 173, 178, 183, 188, 193 };
            for (int i = 0; i < arr.Length; i++)
                tmp.Add(arr[i]);
        }

        int Index = UnityEngine.Random.Range(0, tmp.Count);
        int itId = tmp[Index];
        tmp.RemoveAt(Index);

        ShopData[] two = new ShopData[2];

        ShopData data = new ShopData();
        data.mItemId = itId;
        data.mIsSold = false;
        data.mItemNum = 1;
        two[0] = data;

        Index = UnityEngine.Random.Range(0, tmp.Count);
        itId = tmp[Index];

        data = new ShopData();
        data.mItemId = itId;
        data.mIsSold = false;
        data.mItemNum = 1;
        two[1] = data;

        return two;

    }






}
