using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainShop : MonoBehaviour
{
    public GameObject mNodePreb;
    public RectTransform mNodeRootTrans;
    List<ui_MainShopNode> mNodeArr = new List<ui_MainShopNode>();
    public RectTransform mListCtl;
    public RectTransform mCmdCtl;
    public RectTransform mUpCtl;
    public RectTransform mDownCtl;

    public RectTransform mImageCtl;

    public Text mRefreshBtn;
    public Text mTimeTxt;

    //---刷新界面---
    public GameObject mRefRefreshCtl;
    public Text mFreeText;
    public Text mCrystalText;
    //---购买确认界面---
    public GameObject mRefConfirmCtl;
    public ShopData mRefShopData; // 当前要购买的道具
    public Text mConfirmNameText;
    public Text mConfirmMoneyText;
    public Text mConfirmDesText;
    //---提示界面---
    public GameObject mTipCtl;
    public Text mTipText;
    float mTipT;

    public Text mRefreshCtlTipText;

    public ui_MainShopItemTip mPieceTip;
    public ui_MainShopItemTip mGemTip;
    public ui_MainShopItemScrollTip mScrollTip;
    public ui_MainShopEquipTip mEquipTip;
    public ui_MainShopEquipRingTip mEquipRingTip;

    ui_ClickAnim[] mRefreshBtnAnim = new ui_ClickAnim[3];
    public GameObject[] mRefreshBtnArr = new GameObject[3];
    float mRefreshPlaneCloseDelayT = -1;


    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        mTimeTxt.text = gDefine.GetStr(359) + " " + gDefine.gShop.GetAutoRefreshTStr();
        if (mTipT > 0)
        {
            mTipT -= Time.deltaTime;
            if (mTipT < 0)
            {
                mTipCtl.SetActive(false);
            }
        }

        for (int i = 0; i < mRefreshBtnAnim.Length; i++)
            if (mRefreshBtnAnim[i] != null)
                mRefreshBtnAnim[i].Update();

        if (mRefreshPlaneCloseDelayT > 0)
        {
            mRefreshPlaneCloseDelayT -= Time.deltaTime;
            if (mRefreshPlaneCloseDelayT < 0)
                mRefRefreshCtl.SetActive(false);

        }
    }

    public void Refresh()
    {
        mTimeTxt.text = gDefine.GetStr(359) + " " + gDefine.gShop.GetAutoRefreshTStr();

        ShopData[] dataArr = gDefine.gShop.GetData();
        ui_MainShopNode node;
        int num = (dataArr.Length + 3) / 4;
        if (num > 3) num = 3;

        for (int i = 0; i < mNodeArr.Count; i++)
            mNodeArr[i].gameObject.SetActive(false);

        for (int i = 0; i < num; i++)
        {
            if (i < mNodeArr.Count)
                node = mNodeArr[i];
            else
            {
                node = GameObject.Instantiate(mNodePreb).GetComponent<ui_MainShopNode>();
                node.gameObject.SetActive(true);
                node.gameObject.transform.SetParent(mNodeRootTrans);
                mNodeArr.Add(node);
            }

            node.Init(this, dataArr, i * 4);
        }

        string str = gDefine.GetStr("刷新");
        mRefreshBtn.text = str;

        str = gDefine.GetStr("请选择一种刷新方式");
        mRefreshCtlTipText.text = str;

        ReCalcSize();

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void ReCalcSize()
    {
        float percent = gDefine.RecalcUIScale();

        //计算整个控件的大小，位置
        //float percent = Screen.width / 1080.0f;

        float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080 * percent, Screen.height - TopH - DownH);
        rt.position = new Vector3(Screen.width / 2, Screen.height - TopH, 0);

        //计算图片大小
        mImageCtl.sizeDelta = new Vector2(0, 378.3f * percent);

        //计算控制栏
        mCmdCtl.sizeDelta = new Vector2(0, 160 * percent);
        mCmdCtl.localPosition = new Vector3(0, -378.3f * percent - 5, 0);

        //上方装饰条
        mUpCtl.sizeDelta = new Vector2(0, 54 * percent);
        mUpCtl.localPosition = new Vector3(0, -378.3f * percent - 5 - 160 * percent, 0);

        //计算list区域
        mListCtl.sizeDelta = new Vector2(0, rt.sizeDelta.y - mImageCtl.sizeDelta.y - mCmdCtl.sizeDelta.y - 5
        - mUpCtl.sizeDelta.y - mDownCtl.sizeDelta.y);
        mListCtl.localPosition = new Vector3(0, -378.3f * percent - 160 * percent - 5 - 54 * percent, 0);


        //下方装饰条
        mDownCtl.sizeDelta = new Vector2(0, 54 * percent);
        mDownCtl.localPosition = new Vector3(0, -378.3f * percent - 160 * percent - 5 - 54 * percent
        - mListCtl.sizeDelta.y, 0);

        //
        //gDefine.RecalcAutoSize(gameObject);
        // return;

        //计算菜单的长度
        float nodel = 1064.32f * percent / 3.8f;
        float l = nodel * (int)(mNodeArr.Count + 1);
        mNodeRootTrans.sizeDelta = new Vector2(mNodeRootTrans.sizeDelta.x, l);

    }

    public void Btn_Refresh()
    {
        mRefRefreshCtl.SetActive(true);
        RefreshRefreshCtl();
        mGemTip.gameObject.SetActive(false);
        mEquipTip.gameObject.SetActive(false);
    }

    void RefreshRefreshCtl()
    {
        int freecount = gDefine.gShop.GetTodayFreeRefreshCount();
        mFreeText.text = freecount.ToString() + "/1";
        int count = gDefine.gShop.GetTodayRefreshCount();
        //mCrystalText.text = "200";
    }

    public void Btn_FreeRefresh()
    {
        if (gDefine.gShop.Refresh(Shop.eRefreshType.Free))
        {
            mRefreshPlaneCloseDelayT = 0.5f;
            //mRefRefreshCtl.SetActive(false);
            Refresh();

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("step", "1"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            TalkingDataGA.OnEvent("免费刷新", dic);
        }
        else
        {
            gDefine.ShowTip(gDefine.GetStr(367));
        }

        if (mRefreshBtnAnim[0] == null)
            mRefreshBtnAnim[0] = new ui_ClickAnim();
        mRefreshBtnAnim[0].Init(mRefreshBtnArr[0], 1);


    }

    public void Btn_CrystalRefresh()
    {
        if (gDefine.gPlayerData.Crystal >= 100)
        {
            //mRefRefreshCtl.SetActive(false);
            mRefreshPlaneCloseDelayT = 0.5f;
            gDefine.gShop.Refresh(Shop.eRefreshType.Crystal);
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("step", "1"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
            TalkingDataGA.OnEvent("钻石刷新", dic);
            Refresh();

            gDefine.gAd.PlayInterAD(null);
       //Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("来源", "宝石刷新");
       

        TalkingDataGA.OnEvent("插屏广告", dic);




        }
        else
        {
            string str = gDefine.GetStr(159); // 钻石不足
            gDefine.ShowTip(str);
            //PlayerPrefs.SetInt("iap_door", 1);
            PlayerPrefs.Save();
        }

        if (mRefreshBtnAnim[1] == null)
            mRefreshBtnAnim[1] = new ui_ClickAnim();
        mRefreshBtnAnim[1].Init(mRefreshBtnArr[1], 1);
    }

    

    public void Btn_ADRefresh()
    {
        //mRefRefreshCtl.SetActive(false);
        if (mRefreshBtnAnim[2] == null)
            mRefreshBtnAnim[2] = new ui_ClickAnim();
        mRefreshBtnAnim[2].Init(mRefreshBtnArr[2], 1);

        mRefreshPlaneCloseDelayT = 0.5f;

        gDefine.gAd.PlayADVideo(ADRefreshCallBack);

        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("step", "1"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
        TalkingDataGA.OnEvent("广告刷新", dic);


    }

    public void ADRefreshCallBack(bool Finished)
    {
        if (Finished)
        {
            gDefine.gShop.Refresh(Shop.eRefreshType.AD);
            Refresh();
        }

    }

    public void Btn_CloseRefreshCtl()
    {
        mRefRefreshCtl.SetActive(false);
    }

    public void OpenConfirmCtl(ShopData Data)
    {
        mRefShopData = Data;
        mRefConfirmCtl.SetActive(true);
        RefreshConfirmCtl();
    }

    void RefreshConfirmCtl()
    {
        CItem it = gDefine.gData.GetItemData(mRefShopData.mItemId);
        mConfirmNameText.text = it.mName;
        mConfirmDesText.text = it.mDes;
        //mConfirmMoneyText.text = mRefShopData.mMoneyType == 0 ? "金币:" + mRefShopData.mMoney.ToString() :
        //   "钻石:" + mRefShopData.mMoney.ToString();
        mConfirmMoneyText.text = "钻石:" + it.mPrice.ToString();

    }

    public void GainCoin(bool Finished)
    {
        if (Finished)
        {
            int coin = 300;//Random.Range(200, 301);
            gDefine.gPlayerData.Coin += coin;
            mRefShopData.mIsSold = false;
            gDefine.gMainUI.mRefMainUp.NeedRefresh();
            Refresh();
            //gDefine.ShowTip(gDefine.GetStr("金币") + " " + coin.ToString());
            gDefine.PlaySound(71);

            gDefine.gMainGainTip.Show(201, coin);
        }
        // else
        // {
        //     gDefine.ShowTip(gDefine.GetStr(388));
        // }

    }

    public void GainCrystal(bool Finished)
    {
        if (Finished)
        {
            int crystal = 300;//Random.Range(200, 301);
            gDefine.gPlayerData.Crystal += crystal;
            mRefShopData.mIsSold = false;
            gDefine.gMainUI.mRefMainUp.NeedRefresh();
            Refresh();
            //gDefine.ShowTip(gDefine.GetStr("钻石") + " " + crystal.ToString());
            gDefine.PlaySound(71);

            gDefine.gMainGainTip.Show(202, crystal);

        }
        // else
        // {
        //     gDefine.ShowTip(gDefine.GetStr(388));
        // }

    }

    public void Btn_Buy(ShopData RefShopData)
    {
        if (RefShopData.mItemId == 199)
        {
            mRefShopData = RefShopData;
            //GainCoin(true);
            gDefine.gAd.PlayADVideo(GainCoin);

            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("来源", "商店金币购买");


            TalkingDataGA.OnEvent("激励视频广告", dic);

            // int coin = Random.Range(200, 301);
            // gDefine.gPlayerData.Coin += coin;
            // RefShopData.mIsSold = true;
            // gDefine.gMainUI.mRefMainUp.NeedRefresh();
            // Refresh();
            // gDefine.ShowTip(gDefine.GetStr("金币") + " " + coin.ToString());
            // gDefine.PlaySound(71);
        }
        else if (RefShopData.mItemId == 200)
        {
            mRefShopData = RefShopData;
            // GainCrystal(true);
            gDefine.gAd.PlayADVideo(GainCrystal);
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("来源", "商店钻石购买");

            TalkingDataGA.OnEvent("激励视频广告", dic);

        }
        else
        {
            CItem it = gDefine.gData.GetItemData(RefShopData.mItemId);
            int price = it.mPrice;
            if(it.mComPieceId>0)
            {
                CItem comPiece = gDefine.gData.GetItemData( it.mComPieceId);
                price = comPiece.mPrice;
            }
            if (gDefine.gPlayerData.Crystal >= price)
            {
                gDefine.gPlayerData.Crystal -= price;
                RefShopData.mIsSold = true;

                int itId = RefShopData.mItemId;
                if(it.mMainType <= CItem.eMainType.Ring )
                {
                    itId = it.mComPieceId;
                    //RefShopData.mItemNum = 20;
                }  

                gDefine.gPlayerData.AddItemToBag(itId, RefShopData.mItemNum);
                gDefine.gMainUI.mRefMainUp.NeedRefresh();

                gDefine.gPlayerData.SaveGird();
                PlayerPrefs.Save();
                //string str = gDefine.GetStr("购买成功");

                //Tip(str);
                //mRefConfirmCtl.SetActive(false);
                Refresh();

                gDefine.PlaySound(57);
            }
            else
            {
                string str = gDefine.GetStr("钻石不足！");
                gDefine.ShowTip(str);
                gDefine.PlaySound(74);
                //PlayerPrefs.SetInt("iap_door", 1);
                PlayerPrefs.Save();
            }

        }



        // if (mRefShopData.mMoneyType == 0)
        // {
        //     if (gDefine.gPlayerData.Coin > mRefShopData.mMoney)
        //     {
        //         gDefine.gPlayerData.Coin -= mRefShopData.mMoney;
        //         mRefShopData.mIsSold = true;
        //         gDefine.gPlayerData.AddItemToBag(mRefShopData.mItemId, mRefShopData.mItemNum);
        //         Tip("购买成功");
        //         mRefConfirmCtl.SetActive(false);
        //         Refresh();
        //     }
        //     else
        //         Tip("金币不足");

        // }
        // else if (mRefShopData.mMoneyType == 1 )
        // {
        //     if (gDefine.gPlayerData.Crystal > mRefShopData.mMoney)
        //     {
        //         gDefine.gPlayerData.Crystal -= mRefShopData.mMoney;
        //         mRefShopData.mIsSold = true;
        //         gDefine.gPlayerData.AddItemToBag(mRefShopData.mItemId, mRefShopData.mItemNum);
        //         Tip("购买成功");
        //         mRefConfirmCtl.SetActive(false);
        //         Refresh();
        //     }
        //     else
        //     {
        //         Tip("钻石不足");
        //     }

        // }
    }

    public void Btn_CancelBuy()
    {
        mRefConfirmCtl.SetActive(false);
    }


    public void ShowTip(Transform T, ShopData Item)
    {
        mGemTip.gameObject.SetActive(false);
        mEquipTip.gameObject.SetActive(false);
        mScrollTip.gameObject.SetActive(false);
        mEquipRingTip.gameObject.SetActive(false);
        mPieceTip.gameObject.SetActive(false);

        CItem it = gDefine.gData.GetItemData(Item.mItemId);

        if (it.mMainType == CItem.eMainType.Piece
        ||  it.mMainType == CItem.eMainType.Box)
            mPieceTip.Show(T, Item);
        else if(it.mMainType == CItem.eMainType.Gem)
            mGemTip.Show(T,Item);
        else if (it.mMainType == CItem.eMainType.Scroll)
            mScrollTip.Show(T, Item);
        else if (it.mMainType != CItem.eMainType.Ring)
            mEquipTip.Show(T, Item);
        else if (it.mMainType == CItem.eMainType.Ring)
            mEquipRingTip.Show(T, Item);

    }



}
