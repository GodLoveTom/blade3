using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Box : MonoBehaviour
{
    public GameObject mNodePreb;
    public RectTransform mNodeRootTrans;
    //List<ui_BoxNode> mNodeArr = new List<ui_BoxNode>();
    public RectTransform mListCtl;

    //public ui_MainEquipTip mTip;
    public ui_BoxPrize mPrize;
    public ui_BoxPrizeVip mPrizeVIP;

    public RectTransform mUpCtl;

    public GameObject mRootCtl;

    public Text mTitle;
    public ui_BoxTip mBoxTip;

    public GameObject mVipNode; 


    public ui_BoxTip CreateNewTip()
    {
        GameObject o = GameObject.Instantiate(gDefine.gMainUI.mRefMainBox.mBoxTip.gameObject);
        ui_BoxTip script = o.GetComponent<ui_BoxTip>();
        o.transform.SetParent(gDefine.gMainUI.mRefMainBox.mBoxTip.gameObject.transform.parent);
        o.transform.localPosition = gDefine.gMainUI.mRefMainBox.mBoxTip.gameObject.transform.localPosition;
        return script;
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false);
        gDefine.gMainUI.ChangeToFight();
    }

    public void Show()
    {
        mPrize.gameObject.SetActive(false);
        mPrizeVIP.gameObject.SetActive(false);
        Refresh();
        ReCalcSize();
    }


    public void Refresh()
    {
        mTitle.text = gDefine.GetStr(378);
        gDefine.SetTextBold();

        if (gDefine.gBoxData.IsVipActive())
            mVipNode.SetActive(true);
        else
            mVipNode.SetActive(false);
        // ui_BoxNode node;
        // for (int i = 0; i < 4; i++)
        // {
        //     if (i < mNodeArr.Count)
        //         node = mNodeArr[i];
        //     else
        //     {
        //         node = GameObject.Instantiate(mNodePreb).GetComponent<ui_BoxNode>();
        //         node.gameObject.SetActive(true);
        //         node.gameObject.transform.SetParent(mNodeRootTrans);
        //         mNodeArr.Add(node);
        //     }

        //     node.Init(i, this);
        // }
        //ReCalcSize();

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }


    public void ReCalcSize()
    {
        //计算整个控件的大小，位置
        float uiscale = gDefine.RecalcUIScale();


        float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        RectTransform rt = mRootCtl.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080 * uiscale, Screen.height);
        rt.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        // float percent = Screen.width / 1080.0f;

        //计算菜单的长度
        float nodel = 1023.184f * uiscale / 3.206467f;
        int nodeNum = gDefine.gBoxData.IsVipActive() ? 6 : 5;

        float l = nodel * nodeNum + 50 * uiscale * nodeNum;
        mNodeRootTrans.sizeDelta = new Vector2(mNodeRootTrans.sizeDelta.x, l);

        //计算list
        mListCtl.sizeDelta = new Vector2(0, Screen.height - mUpCtl.sizeDelta.y);
        mListCtl.position = new Vector3(Screen.width / 2, Screen.height - mUpCtl.sizeDelta.y, 0);

        // //计算整个控件的大小，位置
        // float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        // float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        // RectTransform rt = GetComponent<RectTransform>();
        // rt.sizeDelta = new Vector2(Screen.width, Screen.height - TopH - DownH);
        // rt.position = new Vector3(Screen.width/2, Screen.height-TopH,0);

        gDefine.RecalcAutoSize(mBoxTip.gameObject);
     

    }

    public void ShowPrize(int BoxIndex)
    {
        if(BoxIndex==(int)BoxData.eBoxType.Vip)
            mPrizeVIP.Init();
        else
            mPrize.Init(BoxIndex, this);
    }
}
