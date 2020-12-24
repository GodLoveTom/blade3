using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_mainEquip : MonoBehaviour
{
    public Text[] mConstTipText = new Text[2]; //
    public GameObject mNodePreb;
    public RectTransform mNodeRootTrans;

    public GameObject mPieceNodePreb;

    //equip
    List<ui_MainEquipNode> mNodeArr = new List<ui_MainEquipNode>(); 
    int mNodeCount = 0;

    //piece
    List<ui_MainEquipNode> mPieceNodeArr = new List<ui_MainEquipNode>();
    int mPieceNodeCount = 0;

    //背包分割线
    public GameObject mDividingLineObj;


    public RectTransform mListCtl;
    public RectTransform mParamTipCtl;
    public RectTransform mUpCtl;
    public RectTransform mDownCtl;

    public RectTransform mRefPoint;

    public ui_MainEquipParam mRefParamScript; //
    public ui_MainEquipItem mRefItemScript;
    public ui_MainEquipTip mRefTipScript;
    public ui_MainEquipInLay mRefInLayScript;
    public ui_EquipTeach mRefEquipTeach;

    public ui_MainCombine mRefCombine;


    public void Refresh()
    {
        mRefParamScript.Refresh(this);

        RefreshBagGird();

        ReCalcSize();

        mConstTipText[0].text = gDefine.GetStr(375);
     
        mConstTipText[1].text = gDefine.GetStr(165);
         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }

    void CloseAllNode()
    {
        foreach (var item in mNodeArr)
        {
            item.gameObject.transform.SetParent(null);
            item.gameObject.SetActive(false);
        }

        foreach (var item in mPieceNodeArr)
        {
            item.gameObject.transform.SetParent(null);
            item.gameObject.SetActive(false);
        }

        mDividingLineObj.transform.SetParent(null);
        mDividingLineObj.SetActive(false);

    }



    void RefreshBagGird()
    {
        mNodeCount = 0;

        CGird[] girdArr = new CGird[5];
        //int girdIndex = 0;
        int nodeIndex = 0;
        ui_MainEquipNode node;

        CloseAllNode();
#region 装备
        //排序
        List<CGird> tmpArr = new List<CGird>();
        for (int i = 0; i < gDefine.gPlayerData.mBagGird.Length; i++)
        {
            if (gDefine.gPlayerData.mBagGird[i].mRefItem != null && gDefine.gPlayerData.mBagGird[i].mNum > 0
            && gDefine.gPlayerData.mBagGird[i].mRefItem.mEquipPos != gDefine.eEuqipPos.Null)
            {
                for (int j = 0; j < tmpArr.Count; j++)
                {
                    if (gDefine.gPlayerData.mBagGird[i].mRefItem.mQuality > tmpArr[j].mRefItem.mQuality
                    || (gDefine.gPlayerData.mBagGird[i].mRefItem.mQuality == tmpArr[j].mRefItem.mQuality
                        && gDefine.gPlayerData.mBagGird[i].mLVL > tmpArr[j].mLVL))
                    {
                        tmpArr.Insert(j, gDefine.gPlayerData.mBagGird[i]);
                        goto Next;
                    }
                }
                tmpArr.Add(gDefine.gPlayerData.mBagGird[i]);
            Next:
                continue;
            }
        }

        if (tmpArr.Count > 0)
        {
            int nodeNum = (tmpArr.Count + 4) / 5;
            for (int i = 0; i < nodeNum; i++)
            {
                girdArr[0] = ((i * 5) >= tmpArr.Count) ? null : tmpArr[i * 5];
                girdArr[1] = ((i * 5 + 1) >= tmpArr.Count) ? null : tmpArr[i * 5 + 1];
                girdArr[2] = ((i * 5 + 2) >= tmpArr.Count) ? null : tmpArr[i * 5 + 2];
                girdArr[3] = ((i * 5 + 3) >= tmpArr.Count) ? null : tmpArr[i * 5 + 3];
                girdArr[4] = ((i * 5 + 4) >= tmpArr.Count) ? null : tmpArr[i * 5 + 4];

                if (nodeIndex < mNodeArr.Count)
                    node = mNodeArr[nodeIndex++];
                else
                {
                    nodeIndex++;
                    node = GameObject.Instantiate(mNodePreb).GetComponent<ui_MainEquipNode>();
                     //node.gameObject.transform.SetParent(null);
                    
                    mNodeArr.Add(node);
                }

                node.Init(girdArr, this);
                node.gameObject.SetActive(true);
                node.gameObject.transform.SetParent(mNodeRootTrans);
                     node.gameObject.transform.SetAsLastSibling();
            }
        }

        mNodeCount = (tmpArr.Count + 4) / 5;
#endregion
        mDividingLineObj.transform.SetParent(null);
        mDividingLineObj.SetActive(true);
        mDividingLineObj.transform.SetParent(mNodeRootTrans);
         mDividingLineObj.transform.SetAsLastSibling();

#region 碎片  
        nodeIndex = 0;
        tmpArr.Clear();
        for (int i = 0; i < gDefine.gPlayerData.mBagGird.Length; i++)
        {
            if (gDefine.gPlayerData.mBagGird[i].mRefItem != null && gDefine.gPlayerData.mBagGird[i].mNum > 0
            && gDefine.gPlayerData.mBagGird[i].mRefItem.mMainType == CItem.eMainType.ComPiece)
            {
                for (int j = 0; j < tmpArr.Count; j++)
                {
                    if (gDefine.gPlayerData.mBagGird[i].mRefItem.mQuality >= tmpArr[j].mRefItem.mQuality)
                    //|| (gDefine.gPlayerData.mBagGird[i].mRefItem.mQuality == tmpArr[j].mRefItem.mQuality)
                        // && gDefine.gPlayerData.mBagGird[i].mLVL > tmpArr[j].mLVL))
                    {
                        tmpArr.Insert(j, gDefine.gPlayerData.mBagGird[i]);
                        goto Next;
                    }
                }
                tmpArr.Add(gDefine.gPlayerData.mBagGird[i]);
            Next:
                continue;
            }
        }

        if (tmpArr.Count > 0)
        {
            int nodeNum = (tmpArr.Count + 4) / 5;
            for (int i = 0; i < nodeNum; i++)
            {
                girdArr[0] = ((i * 5) >= tmpArr.Count) ? null : tmpArr[i * 5];
                girdArr[1] = ((i * 5 + 1) >= tmpArr.Count) ? null : tmpArr[i * 5 + 1];
                girdArr[2] = ((i * 5 + 2) >= tmpArr.Count) ? null : tmpArr[i * 5 + 2];
                girdArr[3] = ((i * 5 + 3) >= tmpArr.Count) ? null : tmpArr[i * 5 + 3];
                girdArr[4] = ((i * 5 + 4) >= tmpArr.Count) ? null : tmpArr[i * 5 + 4];

                if (nodeIndex < mPieceNodeArr.Count)
                    node = mPieceNodeArr[nodeIndex++];
                else
                {
                    nodeIndex++;
                    node = GameObject.Instantiate(mPieceNodePreb).GetComponent<ui_MainEquipNode>();

                   
                    mPieceNodeArr.Add(node);
                }

                node.Init(girdArr, this);
                node.gameObject.SetActive(true);
                 node.gameObject.transform.SetParent(mNodeRootTrans);
                    node.gameObject.transform.SetAsLastSibling();
            }
        }

        mPieceNodeCount = (tmpArr.Count + 4) / 5;

#endregion





        /*   for (int i = 0; i < gDefine.gPlayerData.mBagGird.Length; i++)
           {
               if (gDefine.gPlayerData.mBagGird[i].mRefItem != null && gDefine.gPlayerData.mBagGird[i].mNum > 0
                   && gDefine.gPlayerData.mBagGird[i].mRefItem.mEquipPos != gDefine.eEuqipPos.Null)
               {
                   girdArr[girdIndex++] = gDefine.gPlayerData.mBagGird[i];
                   if (girdIndex == 5)
                   {
                       mNodeCount++;
                       if (nodeIndex < mNodeArr.Count)
                           node = mNodeArr[nodeIndex++];
                       else
                       {
                           nodeIndex++;
                           node = GameObject.Instantiate(mNodePreb).GetComponent<ui_MainEquipNode>();

                           node.gameObject.transform.SetParent(mNodeRootTrans);
                           mNodeArr.Add(node);
                       }

                       node.Init(girdArr, this);
                       node.gameObject.SetActive(true);

                       girdIndex = 0;
                       girdArr[0] = girdArr[1] = girdArr[2] = girdArr[3] = girdArr[4] =null;
                   }
               }
           }

           if (girdIndex > 0)
           {
               mNodeCount++;
               if (nodeIndex < mNodeArr.Count)
                   node = mNodeArr[nodeIndex++];
               else
               {
                   node = GameObject.Instantiate(mNodePreb).GetComponent<ui_MainEquipNode>();
                   node.gameObject.SetActive(true);
                   node.gameObject.transform.SetParent(mNodeRootTrans);
                   mNodeArr.Add(node);
               }

               node.Init(girdArr, this);
               node.gameObject.SetActive(true);
           }*/
    }

    public void ReCalcSize()
    {
        float percent = gDefine.RecalcUIScale();

        //计算菜单的长度
        float nodel = 947.3f * percent / 5.4f;
        float piecenodel = 947.3f * percent / 4.079673f;
        float framenodel = 947.3f * percent / 31.68227f;
        float l = (nodel + 2) * (mNodeCount + 1) + (piecenodel+2)*mPieceNodeCount + framenodel;
        mNodeRootTrans.sizeDelta = new Vector2(mNodeRootTrans.sizeDelta.x, l);

        //计算整个控件的大小，位置

        float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080 * percent, Screen.height - TopH - DownH);
        rt.position = new Vector3(Screen.width / 2, Screen.height - TopH, 0);

        //计算up界面大小位置
        mUpCtl.sizeDelta = new Vector2(0, 719.6f * percent);

        //计算下方提示框的大小
        mParamTipCtl.sizeDelta = new Vector2(0, 281.7f * percent);
        mParamTipCtl.localPosition = new Vector3(0, -mUpCtl.sizeDelta.y, 0);

        //计算下方装饰框大小
        mDownCtl.sizeDelta = new Vector2(0, 50 * percent);

        //计算区域大小
        mListCtl.sizeDelta = new Vector2(947.3f * percent, rt.sizeDelta.y - mParamTipCtl.sizeDelta.y * 0.7f
        - mUpCtl.sizeDelta.y - mDownCtl.sizeDelta.y);
        mListCtl.position = mRefPoint.position;  //new Vector3(0, mRefPoint.transform.position.y, 0);

        //计算下方装饰框位置
        //mDownCtl.sizeDelta = new Vector2(0, 50 * percent);
        mDownCtl.localPosition = new Vector3(0, -rt.sizeDelta.y, 0);

    }

    public void ShowPiecePage(CGird Gird)
    {
        mRefCombine.Show(Gird);
    }

    public void ShowItemDes(CGird Gird, bool IsEquiped)
    {
        mRefItemScript.Init(Gird, IsEquiped, this);
    }

    public void Tip(string Str)
    {
        mRefTipScript.Tip(Str);
    }

    public void Btn_OPenXiangQian()
    {
        mRefInLayScript.Show(this);
          Dictionary<string, object> dic = new Dictionary<string, object>();
        
            dic.Add("无", 0);
            TalkingDataGA.OnEvent("镶嵌界面开启点击", dic);
    }


    // public GameObject [] mEquipPos;
    // public GameObject [] mBag;

    // int mLastClickIndex=-1;
    // float mLastClickT=-1;

    // public void Event_Click(int Index)
    // {
    //     if( mLastClickIndex != Index )
    //     {
    //         mLastClickIndex = Index;
    //         mLastClickT = Time.time;
    //     }       
    //     else
    //     {
    //         if( Time.time < mLastClickT + 0.35f)
    //         {
    //             CheckGird(Index);
    //             mLastClickT = 0;
    //         }
    //         else
    //         {
    //              mLastClickT = Time.time;
    //         }
    //     }
    // }

    // void CheckGird(int Index)
    // {   
    //     if( gDefine.gPlayerData.ChangeGird(Index) )
    //     {
    //         Refresh();
    //     }
    // }

    // // Start is called before the first frame update
    // void Start()
    // {
    //      //Refresh();
    // }

    // void Wake()
    // {
    //     Refresh();
    // }

    // public void Refresh()
    // {
    //      for(int i=0; i<gDefine.gPlayerData.mEquipGird.Length; i++)
    //     {
    //         Image iconImage = mEquipPos[i].transform.Find("Image").gameObject.GetComponent<Image>();
    //         Text nameText = mEquipPos[i].transform.Find("Text").gameObject.GetComponent<Text>();

    //         CItem it = gDefine.gPlayerData.mEquipGird[i]?.mRefItem;
    //         if(it!=null )
    //         {
    //             iconImage.sprite = it.GetIconSprite();
    //             iconImage.gameObject.SetActive(true);
    //             nameText.gameObject.SetActive(false);
    //         }
    //         else
    //         {
    //             iconImage.gameObject.SetActive(false);
    //              nameText.gameObject.SetActive(true);
    //         }
    //     }

    //     int len = mBag.Length < gDefine.gPlayerData.mBagGird.Length?mBag.Length:gDefine.gPlayerData.mBagGird.Length;

    //     for(int i=0; i<len; i++)
    //     {
    //         Image iconImage = mBag[i].transform.Find("Image").gameObject.GetComponent<Image>();
    //         Text nameText = mBag[i].transform.Find("Text").gameObject.GetComponent<Text>();

    //         CItem it = gDefine.gPlayerData.mBagGird[i]?.mRefItem;
    //         if(it!=null )
    //         {
    //             iconImage.sprite = it.GetIconSprite();
    //             iconImage.gameObject.SetActive(true);
    //             nameText .text = it.mName;
    //         }
    //         else
    //         {
    //             iconImage.gameObject.SetActive(false);
    //             nameText .text = "";
    //         }
    //     } 
    // }

    public void Open()
    {
        gameObject.SetActive(true);

        Refresh();
        mRefParamScript.SetGirlDefaultAct();

        mRefInLayScript.gameObject.SetActive(false);
        mRefItemScript.gameObject.SetActive(false);
        //  mRefTipScript.gameObject.SetActive(false);
        //PlayerPrefs.SetInt("FirstInEquip",0); 
        if( PlayerPrefs.GetInt("FirstInEquip",0) == 0)
        {
            mRefEquipTeach.Show();
            PlayerPrefs.SetInt("FirstInEquip",1); 
        }
    }
}
