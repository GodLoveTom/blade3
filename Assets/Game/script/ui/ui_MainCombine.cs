using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainCombine : MonoBehaviour
{
    public Image mItemIcon;
    public Image mPieceIcon;
    public Text mItemNameText;
    public Text mPieceNameText;
    public Text mQualityText;
    public Text[] mParamArr;
    public Text mDividingLineText; //分割线 -- 特殊能力 --
    public Text mUnlockLineText;  //分割线-- 合成材料 --
    public Text mPieceNumText;

    public GameObject mBtnCoinObj; //按钮合成金币图标
    public Text mBtnCoinText;   //按钮合成100金币
    public Text mBtnCombineText;  //按钮合成
    public Text mBtnHowToGetText;  //按钮获得途径

    public GameObject mGetCtl; //获取途径按钮
    public Text mGetTip0; //-商店购买
    public Text mGetTip1; //-第几章 掉落
    CGird mGird;

    public GameObject mRoot;

    public void Show(CGird Gird)
    {
        gameObject.SetActive(true);
        mGird = Gird;
        Refresh();

    }

    void Refresh()
    {
        mItemIcon.sprite = mPieceIcon.sprite = mGird.mRefItem.GetIconSprite();
        mItemNameText.text = mGird.mRefItem.GetNameLocal();
        
        mPieceNameText.text = mGird.mRefItem.GetNameLocal() + "(" + gDefine.GetStr(273) + ")";
       
        mQualityText.text = mGird.mRefItem.GetPinZhiStr();
       
        mQualityText.color = mGird.mRefItem.GetPinZhiColor();

        mDividingLineText.text = gDefine.GetStr(451); //---特殊---
       
        mUnlockLineText.text = gDefine.GetStr(432);  //----合成材料----
       

        CItem[] it = gDefine.gData.GetItemsByCombinePieceId(mGird.mRefItem.Id);

        mParamArr[0].text = it[0].GetValueStr(0);
       

        mParamArr[1].text = mParamArr[2].text = "";
       

        if (it[0].mMainType == CItem.eMainType.Ring)
            mParamArr[1].text = it[0].GetValueStr(1);
         

        if (mGird.CanCombin())
        {
            mBtnCoinObj.SetActive(true);
            mBtnCoinText.gameObject.SetActive(true);

            int money = CalcNeedMoney( mGird.mRefItem.mQuality);
            mBtnCoinText.text = money.ToString();

            if (gDefine.gPlayerData.Coin >= 100)
                mBtnCoinText.color = new Color(0, 0.7372f, 0.7372f);
            else
                mBtnCoinText.color = Color.red;

            mBtnCombineText.text = gDefine.GetStr(433); // 合成
          
            mBtnCombineText.gameObject.SetActive(true);
            mBtnHowToGetText.gameObject.SetActive(false);

            mPieceNumText.text = mGird.mNum.ToString() + "/" + mGird.GetCombinPieceNum().ToString();
        }
        else
        {
            mBtnCoinObj.SetActive(false);
            mBtnCoinText.gameObject.SetActive(false);

            mBtnCombineText.gameObject.SetActive(false);
            mBtnHowToGetText.gameObject.SetActive(true);
            mBtnHowToGetText.text = gDefine.GetStr(434); // 获得途径
            

            mPieceNumText.text = "<color=red>" + mGird.mNum.ToString() + "</color>/" + mGird.GetCombinPieceNum().ToString();

        }

        mGetTip0.text = gDefine.GetStr(435); // 商店购买
       
        string str = gDefine.GetStr(436);
        
        CChapterDropDataParam c = gDefine.gDropSystem.GetChapterDataByCombinePieceId(mGird.mRefItem.Id);
        str = str.Replace("<<s0>>", c.mChapterId.ToString());
        str = str.Replace("<<s1>>", c.GetChapterLocalName());
        mGetTip1.text = str; // 第几章掉落
        

        mGetCtl.SetActive(false);

        gDefine.RecalcAutoSize(mRoot);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }

    public void Btn_Combine()
    {
        int needMoney = CalcNeedMoney(mGird.mRefItem.mQuality);
        if (!mGird.CanCombin())
        {
            mGetCtl.SetActive(true);
            gDefine.PlaySound(71);
        }
        else if (gDefine.gPlayerData.Coin < needMoney)
        {
            gDefine.ShowTip(gDefine.GetStr(160));
            gDefine.PlaySound(71);
        }
        else
        {
            int num = mGird.GetCombinPieceNum();
            mGird.mNum -= num;

            gDefine.gPlayerData.Coin -= 100;

            CItem[] arr = gDefine.gData.GetItemsByCombinePieceId(mGird.mRefItem.Id);

            int index = Random.Range(0, arr.Length);

            gDefine.gPlayerData.AddItemToBag(arr[index].Id, 1);

            if (mGird.mNum <= 0)
                mGird.mRefItem = null;

            //Refresh();
            gDefine.gMainUI.mRefMainEquip.Refresh();
            //.tip.
            string str = gDefine.GetStr(437) + arr[index].GetNameLocal();//获得了....
            gDefine.ShowTip(str);

            if (mGird.mNum > 0)
                Refresh();
            else
                gameObject.SetActive(false);
            gDefine.PlaySound(78);
             
             gDefine.gAd.PlayInterAD1(null);
            Dictionary<string, object> dic = new Dictionary<string, object>();      
        dic.Clear();
        dic.Add("来源", "碎片合成");
        TalkingDataGA.OnEvent("插屏广告", dic);
        }

    }

    int CalcNeedMoney(CItem.eQuality Quality)
    {
        switch (Quality)
        {
            case CItem.eQuality.Common:
                return 200;
            case CItem.eQuality.Excellent:
                return 300;
            case CItem.eQuality.Rare:
                return 400;
            case CItem.eQuality.Epic:
                return 500;
            case CItem.eQuality.Legend:
                return 600;
        }
        return 9999999;
    }

    public void Btn_Quit()
    {
        gameObject.SetActive(false);
    }

    public void Btn_CloseGetCtl()
    {
        mGetCtl.SetActive(false);
    }

}
