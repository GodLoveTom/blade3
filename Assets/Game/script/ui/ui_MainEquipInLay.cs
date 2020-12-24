using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipInLay : MonoBehaviour
{
    public GameObject mEquipNodePreb;
    public GameObject mGemNodePreb;
    public RectTransform mEquipNodeRootTrans;
    public RectTransform mGemNodeRootTrans;
    List<ui_MainEquipInLayEquipNode> mEquipNodeArr = new List<ui_MainEquipInLayEquipNode>();
    List<ui_MainEquipInLayGemNode> mGemNodeArr = new List<ui_MainEquipInLayGemNode>();

    public RectTransform mUpCtl;
    public RectTransform mListEquipCtl;
    public RectTransform mListGemCtl;
    public RectTransform mEquipFrameCtl;
    public RectTransform mGemFrameCtl;
    public RectTransform mDownCtl;
    public RectTransform mMidCtl;
    public RectTransform mPlane;

    public CGird mEquipGird;
    public bool mEquipGirdIsEquiped = false;

    //---InLay Ctl ---//
    public Text[] mGemParam = new Text[2];
    public Text[] mGemName = new Text[2];
    public Text[] mGemLvL = new Text[2];
    public Image[] mGemIcon = new Image[2];
    public Image[] mGemSelectFrame = new Image[2];

    public Image [] mGemPutFrame = new Image[3];

    //---3个固定字符串----
    public Text[] mConstTipStr = new Text[3];

    int mCurSelectGemIndex = 0;

    int mEqipNodeNum = 0;
    int mGemNodeNum = 0;

    //---InLay Item---//
    public Text mEquipName;
    public Image mEquipIcon;
    public Text mEquipLvL;
    ui_mainEquip mRefMainEquip;

    //---拖拽----//
    public ui_MainInlayDrag mDrag;

    public ui_MainEquipItem mRefItemTip;
    public ui_MainEquipInLayGemTip mRefGemTip;

    public GameObject mTeachWeaponRef;
    public GameObject mTeachGemRef;
    public GameObject mTeachGemTipRef;

    public ui_InlayTeach mRefTeach;
    public CGird mTeachWeaponGird;
    public CGird mTeachGemGird;
    

    public void Show(ui_mainEquip MainEquip)
    {
        mRefMainEquip = MainEquip;

        gameObject.SetActive(true);

        mEquipGird = null;

        Refresh();

        //PlayerPrefs.SetInt("firstInlay",0);
        if(PlayerPrefs.GetInt("firstInlay",0) == 0)
        {
            PlayerPrefs.SetInt("firstInlay",1);
            PlayerPrefs.Save();

            mRefTeach.mWeaponRef = mTeachWeaponRef;
            mRefTeach.mGemRef = mTeachGemRef;
            mRefTeach.mGemTipRef = mTeachGemTipRef;

            mRefTeach.Show();
        }
        else
        {
            mRefTeach.gameObject.SetActive(false);
        }

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void TeachSetWeapon()
    {
        SetInLayItem( mTeachWeaponGird, true );
    }

    public void TeachBeginDrag()
    {
        ShowDrag( mTeachGemGird ,  false);
    }

    public void Refresh()
    {
        mConstTipStr[0].text = gDefine.GetStr(374);
        gDefine.SetTextBold();
        mConstTipStr[1].text = gDefine.GetStr(376);
        gDefine.SetTextBold();
        mConstTipStr[2].text = gDefine.GetStr(169);
        gDefine.SetTextBold();

        RefreshEquipGird();

        RefreshGemGird();

        RefreshMid();

        ReCalcSize();

        
    }

    public void ShowDrag(CGird Gird, bool IsFromEquip)
    {
        mDrag.Show(Gird, this);
    }

    public void Btn_BeginDrag(int Index)
    {
        mDrag.Show(mEquipGird, Index, this);
    }

    public void ShowGemTip(Transform T, CGird Gird)
    {
        mRefGemTip.Show(T, Gird);
    }

    void RefreshMid()
    {

        // mEquipName.text = "";
        mEquipIcon.gameObject.SetActive(false);

        // mGemParam[0].text = mGemParam[1].text = "";
        // mGemName[0].text = mGemName[1].text = "";
        // //mGemLvL[0].text = mGemLvL[1].text = "";
        mGemIcon[0].gameObject.SetActive(false);
        mGemIcon[1].gameObject.SetActive(false);
        mGemIcon[2].gameObject.SetActive(false);
        mGemLvL[0].text ="";
        mGemLvL[1].text ="";
        mGemLvL[2].text ="";

        mEquipLvL.text="";
        // mGemSelectFrame[0].gameObject.SetActive(false);
        // mGemSelectFrame[1].gameObject.SetActive(false);

        if (mEquipGird != null)
        {
            //mEquipName.text = mEquipGird.mRefItem.mName;
            mEquipIcon.gameObject.SetActive(true);
            mEquipIcon.sprite = mEquipGird.mRefItem.GetIconSprite();
            mEquipLvL.text = mEquipGird.mLVL.ToString();

            //int paramIndex = 0;
            if (mEquipGird.mGem[0] > 0)
            {
                CItem gem = gDefine.gData.GetItemData(mEquipGird.mGem[0]);
                //mGemName[0].text = gem.mName;
                mGemIcon[0].gameObject.SetActive(true);
                mGemIcon[0].sprite = gem.GetIconSprite();
                mGemLvL[0].text = gem.mMaxLvL.ToString();
                
                //mGemParam[paramIndex++].text = gem.GetValueStr(0);  // + ":" + gem.mValue.ToString();
            }

            if (mEquipGird.mGem[1] > 0)
            {
                CItem gem = gDefine.gData.GetItemData(mEquipGird.mGem[1]);
                //mGemName[1].text = gem.mName;
                mGemIcon[1].gameObject.SetActive(true);
                mGemIcon[1].sprite = gem.GetIconSprite();
                 mGemLvL[1].text = gem.mMaxLvL.ToString();
                //mGemParam[paramIndex++].text = gem.GetValueStr(0);   // + ":" + gem.mValue.ToString();
            }

            if (mEquipGird.mGem[2] > 0)
            {
                CItem gem = gDefine.gData.GetItemData(mEquipGird.mGem[2]);
                //mGemName[1].text = gem.mName;
                mGemIcon[2].gameObject.SetActive(true);
                mGemIcon[2].sprite = gem.GetIconSprite();
                mGemLvL[2].text = gem.mMaxLvL.ToString();
                //mGemParam[paramIndex++].text = gem.GetValueStr(0);   // + ":" + gem.mValue.ToString();
            }

            // if (mCurSelectGemIndex == 1)
            //     mGemSelectFrame[1].gameObject.SetActive(true);
            // else
            //     mGemSelectFrame[0].gameObject.SetActive(true);

        }
    }

    public void Btn_ChangeSelect(int Index)
    {
        mCurSelectGemIndex = Index;
        if (mCurSelectGemIndex == 0)
        {
            mGemSelectFrame[0].gameObject.SetActive(true);
            mGemSelectFrame[1].gameObject.SetActive(false);
        }
        else
        {
            mGemSelectFrame[0].gameObject.SetActive(false);
            mGemSelectFrame[1].gameObject.SetActive(true);
        }
    }

    public void SetInLayItem(CGird Gird, bool IsEquiped)
    {
        if( mEquipGird == Gird)
        {
            mRefItemTip.Init(Gird,IsEquiped,null,false);
        }
        else
        {
            mEquipGird = Gird;
            mEquipGirdIsEquiped = IsEquiped;
            Refresh();
            gDefine.PlaySound(58);
        }
    }

    public void Btn_ShowEquipGirdTip()
    {
        if( mEquipGird != null)
        {
            mRefItemTip.Init(mEquipGird,mEquipGirdIsEquiped,null,false);
        }
    }


    public bool IsInGemGird(int GemGirdIndex, Vector3 Pos)
    {
        RectTransform rt = mGemPutFrame[GemGirdIndex].GetComponent<RectTransform>();
        Rect rect = rt.rect;

        Vector3 pos = rt.position;
        pos.y = Screen.height - pos.y;
        rect.position = pos;

        if (rect.Contains(Pos))
            return true;
        else
            return false;

    }

    bool GemIsToEquip(CGird Gird)
    {
        if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.MainWeapon
            && Gird.mRefItem.mSubType == CItem.eSubType.RedGem)
            return true;
        else if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.GunWeapon
            && Gird.mRefItem.mSubType == CItem.eSubType.BlueGem)
            return true;
        else if ((mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.Cloak
        || mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.Clothe)
          && Gird.mRefItem.mSubType == CItem.eSubType.GreenGem)
            return true;
        else if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.Ring
          && (Gird.mRefItem.mSubType == CItem.eSubType.YellowGem ||
           Gird.mRefItem.mSubType == CItem.eSubType.PurpleGem))
            return true;
        else
        {
            return false;
        }

    }

    public void SetGem(CGird Gird, int GemIndex)
    {
        if (mEquipGird != null)
        {
            if (GemIsToEquip(Gird))
            {
                mCurSelectGemIndex = GemIndex;
                if (mEquipGird.mGem[mCurSelectGemIndex] > 0)
                {
                    //拆下
                    gDefine.gPlayerData.AddItemToBag(mEquipGird.mGem[mCurSelectGemIndex], 1);
                }

                //镶嵌
                mEquipGird.mGem[mCurSelectGemIndex] = Gird.mRefItem.Id;

                //善后
                Gird.mNum--;
                if (Gird.mNum <= 0)
                    Gird.mRefItem = null;

                Refresh();

                mEquipGird.ReCalcValue();

                gDefine.gPlayerData.SaveGird();
                PlayerPrefs.Save();

                mRefMainEquip.mRefParamScript.Refresh(mRefMainEquip);

                gDefine.PlaySound(61);

                if(mRefTeach.mIsOn)
                {
                    mRefTeach.DragEnd();
                }

                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("镶嵌装备", mEquipGird.mRefItem.mName); 
                dic.Add("宝石", Gird.mRefItem.mName );
                dic.Add("宝石等级", Gird.mLVL.ToString() );
                TalkingDataGA.OnEvent("宝石镶嵌", dic);
        

            }
            else
                ShowEquipGemTip();
        }
    }

    public void ShowEquipGemTip()
    {
        if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.MainWeapon
         )
            gDefine.ShowTip(gDefine.GetStr(330));
        else if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.GunWeapon
            )
            gDefine.ShowTip(gDefine.GetStr(332));
        else if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.Cloak
        || mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.Clothe
       )
            gDefine.ShowTip(gDefine.GetStr(331));
        else if (mEquipGird.mRefItem.mEquipPos == gDefine.eEuqipPos.Ring
         )
            gDefine.ShowTip(gDefine.GetStr(333));
    }

    void ClearEquipNode()
    {
        foreach (var item in mEquipNodeArr)
        {
            item.gameObject.SetActive(false);
        }
    }
    void ClearGemNode()
    {
        foreach (var item in mGemNodeArr)
        {
            item.gameObject.SetActive(false);
        }
    }

    void RefreshGemGird()
    {
        mGemNodeNum = 0;
        CGird[] girdArr = new CGird[5];

        ClearGemNode();

        int girdIndex = 0;
        int nodeIndex = 0;
        ui_MainEquipInLayGemNode node;

        for (int i = 0; i < gDefine.gPlayerData.mBagGird.Length; i++)
        {
            if (gDefine.gPlayerData.mBagGird[i].mRefItem != null &&
                gDefine.gPlayerData.mBagGird[i].mRefItem.mMainType == CItem.eMainType.Gem)
            {
                girdArr[girdIndex++] = gDefine.gPlayerData.mBagGird[i];
                if (girdIndex == 5)
                {
                    mGemNodeNum++;
                    if (nodeIndex < mGemNodeArr.Count)
                        node = mGemNodeArr[nodeIndex++];
                    else
                    {
                        node = GameObject.Instantiate(mGemNodePreb).GetComponent<ui_MainEquipInLayGemNode>();

                        node.gameObject.transform.SetParent(mGemNodeRootTrans);

                        mGemNodeArr.Add(node);

                        nodeIndex++;
                    }
                    node.gameObject.SetActive(true);

                    node.Init(girdArr, this);

                    girdIndex = 0;
                    girdArr[0] = girdArr[1] = girdArr[2] = girdArr[3] = girdArr[4] = null;
                }
            }
        }

        if (girdIndex > 0)
        {
            mGemNodeNum++;
            if (nodeIndex < mGemNodeArr.Count)
                node = mGemNodeArr[nodeIndex++];
            else
            {
                node = GameObject.Instantiate(mGemNodePreb).GetComponent<ui_MainEquipInLayGemNode>();

                node.gameObject.transform.SetParent(mGemNodeRootTrans);

                mGemNodeArr.Add(node);

                nodeIndex++;
            }
            node.gameObject.SetActive(true);

            node.Init(girdArr, this);

        }


    }

    void RefreshEquipGird()
    {
        mEqipNodeNum = 0;
        CGird[] girdArr = new CGird[5];
        bool[] isEquipedArr = new bool[5];

        ClearEquipNode();

        int girdIndex = 0;
        int nodeIndex = 0;
        ui_MainEquipInLayEquipNode node;

        for (int i = 0; i < gDefine.gPlayerData.mEquipGird.Length; i++)
        {
            if (gDefine.gPlayerData.mEquipGird[i].mRefItem != null)
            {
                girdArr[girdIndex] = gDefine.gPlayerData.mEquipGird[i];
                isEquipedArr[girdIndex++] = true;
                if (girdIndex == 5)
                {
                    mEqipNodeNum++;

                    if (nodeIndex < mEquipNodeArr.Count)
                        node = mEquipNodeArr[nodeIndex++];
                    else
                    {
                        node = GameObject.Instantiate(mEquipNodePreb).GetComponent<ui_MainEquipInLayEquipNode>();

                        node.gameObject.transform.SetParent(mEquipNodeRootTrans);

                        mEquipNodeArr.Add(node);

                        nodeIndex++;
                    }
                    node.gameObject.SetActive(true);

                    node.Init(girdArr, isEquipedArr, this);

                    girdIndex = 0;
                    girdArr[0] = girdArr[1] = girdArr[2] = girdArr[3] = girdArr[4] = null;
                    isEquipedArr[0] = isEquipedArr[1] = isEquipedArr[2] = isEquipedArr[3] = isEquipedArr[4] = false;
                }
            }
        }

        for (int i = 0; i < gDefine.gPlayerData.mBagGird.Length; i++)
        {
            if (gDefine.gPlayerData.mBagGird[i].mRefItem != null &&
                gDefine.gPlayerData.mBagGird[i].mRefItem.mEquipPos != gDefine.eEuqipPos.Null)
            {
                girdArr[girdIndex++] = gDefine.gPlayerData.mBagGird[i];
                if (girdIndex == 5)
                {
                    mEqipNodeNum++;

                    if (nodeIndex < mEquipNodeArr.Count)
                        node = mEquipNodeArr[nodeIndex++];
                    else
                    {
                        node = GameObject.Instantiate(mEquipNodePreb).GetComponent<ui_MainEquipInLayEquipNode>();

                        node.gameObject.transform.SetParent(mEquipNodeRootTrans);

                        mEquipNodeArr.Add(node);

                        nodeIndex++;
                    }

                    node.gameObject.SetActive(true);

                    node.Init(girdArr, isEquipedArr, this);

                    girdIndex = 0;
                    girdArr[0] = girdArr[1] = girdArr[2] = girdArr[3] = girdArr[4] = null;
                    isEquipedArr[0] = isEquipedArr[1] = isEquipedArr[2] = isEquipedArr[3] = isEquipedArr[4] = false;
                }
            }
        }

        if (girdIndex > 0)
        {
            mEqipNodeNum++;

            if (nodeIndex < mEquipNodeArr.Count)
                node = mEquipNodeArr[nodeIndex++];
            else
            {
                node = GameObject.Instantiate(mEquipNodePreb).GetComponent<ui_MainEquipInLayEquipNode>();

                node.gameObject.transform.SetParent(mEquipNodeRootTrans);

                mEquipNodeArr.Add(node);
            }
            node.gameObject.SetActive(true);

            node.Init(girdArr, isEquipedArr, this);

        }
    }



    public void ReCalcSize()
    {

        float percent = gDefine.RecalcUIScale();
        //计算菜单的长度
        //装备菜单长度
        float nodel = 930.3601f * percent / 5;
        float l = (nodel + 10) * mEqipNodeNum;
        mEquipNodeRootTrans.sizeDelta = new Vector2(mEquipNodeRootTrans.sizeDelta.x, l);

        l = (nodel + 1.5f) * mGemNodeNum;
        mGemNodeRootTrans.sizeDelta = new Vector2(mGemNodeRootTrans.sizeDelta.x, l);

        //计算整体
        //float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        //float DownH = gDefine.gMainUI.mRefMainDown.CalcH();


        //整体
        RectTransform rt = mPlane.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080 * percent, Screen.height);
        //rt.position = new Vector3(Screen.width / 2, Screen.height - TopH, 0);

        //计算mid
        //已经自动计算了

        //计算up,down的大小位置
        mUpCtl.sizeDelta = new Vector2(0, (Screen.height - mMidCtl.GetComponent<RectTransform>().sizeDelta.y) / 2 + 40 * percent);
        mDownCtl.sizeDelta = mUpCtl.sizeDelta;

        //计算装备栏大小
        l = (mUpCtl.sizeDelta.y - mEquipFrameCtl.sizeDelta.y);
        mListEquipCtl.sizeDelta = new Vector2(0, l);
        //mListEquipCtl.localPosition = new Vector3(0, 0, 0);


        //计算提示栏大小位置
        // mEquipFrameCtl.sizeDelta = new Vector2(0, 62 * percent);
        // mEquipFrameCtl.localPosition = new Vector3(0, -mUpCtl.sizeDelta.y - 4, 0);



        //  //计算提示栏大小位置
        // mGemFrameCtl.sizeDelta = new Vector2(0, 62 * percent);
        // mGemFrameCtl.localPosition = new Vector3(0, mListEquipCtl.localPosition.y - mListEquipCtl.sizeDelta.y - 4, 0);

        // //计算第二个可变区域大小位置
        //l = (Screen.height - TopH - DownH - 410 * percent - 62*percent*2 -4*4)/2;
        mListGemCtl.sizeDelta = new Vector2(0, l);
        mListGemCtl.localPosition = new Vector3(0, 0, 0);

        // float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        // float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        // RectTransform rt = GetComponent<RectTransform>();
        // rt.sizeDelta = new Vector2(Screen.width, Screen.height - TopH - DownH);
        // rt.position = new Vector3(Screen.width / 2, Screen.height - TopH, 0);

        // //计算下方提示框的大小
        // mParamTipCtl.sizeDelta = new Vector2(0, 217 * percent);

        // //计算天赋区域大小
        // mListCtl.sizeDelta = new Vector2(0, rt.sizeDelta.y - mParamTipCtl.sizeDelta.y);


        //mDownCtl.localPosition = Vector3.zero;




    }



    public void Tip(string Str)
    {
        mRefMainEquip.Tip(Str);
    }

    public void Btn_GemLvLUp()
    {
        Dictionary<string, object> dic = new Dictionary<string, object>();
        dic.Add("step", "1"); // 在注册环节的每一步完成时，以步骤名作为value传送数据
        TalkingDataGA.OnEvent("宝石合成点击", dic);
        bool ok = false;
        for (int i = 107; i <= 157; i += 10)
        {
            for (int j = i; j > i - 9; j--)
            {
                CGird gird = gDefine.gPlayerData.FindGridByItemId(j);
                if (gird != null && gird.mNum >= 2)
                {
                    int upGemNum = gird.mNum / 2;
                    gird.mNum = gird.mNum % 2;
                    if (gird.mNum == 0)
                        gird.mRefItem = null;

                    gDefine.gPlayerData.AddItemToBag(j + 1, upGemNum);
                    ok = true;
                }
            }
        }
        Refresh();
    
        gDefine.gPlayerData.SaveGird();
        PlayerPrefs.Save();
    
        gDefine.PlaySound(ok?78:71);
        if(ok&&!mRefTeach.mIsOn)
            gDefine.gAd.PlayInterAD1(null);
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false);
        gDefine.gMainUI.mRefMainEquip.Refresh();
    }

    public void Btn_RemoveGem(int Index)
    {
        if (mEquipGird != null && mEquipGird.mGem[Index] > 0)
        {
            gDefine.gPlayerData.AddItemToBag(mEquipGird.mGem[Index], 1);
            mEquipGird.mGem[Index] = 0;
            mEquipGird.ReCalcValue();
            Refresh();
            gDefine.gPlayerData.SaveGird();
            PlayerPrefs.Save();
        }
    }
}
