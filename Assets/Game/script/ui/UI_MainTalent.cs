using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MainTalent : MonoBehaviour
{
    public GameObject mNodePreb;
    public RectTransform mNodeRootTrans;
    List<ui_MainTalentNode> mNodeArr = new List<ui_MainTalentNode>();
    public Text[] mTipText;

    public RectTransform mListCtl;
    public RectTransform mParamTipCtl;

    void ClearTip()
    {
        for(int i=0; i<mTipText.Length; i++)
            mTipText[i].text = "";
    }

    public void Refresh()
    {
       //ClearTip();
        ui_MainTalentNode node;
        int Index = 0;
        for (int i = 0; i < (int)CTalent.eTalentType.Count; i++)
        {
            if (i < mNodeArr.Count)
                node = mNodeArr[i];
            else
            {
                node = GameObject.Instantiate(mNodePreb).GetComponent<ui_MainTalentNode>();
                node.gameObject.SetActive(true);
                node.gameObject.transform.SetParent(mNodeRootTrans);
                mNodeArr.Add(node);
            }

            CTalent talent = gDefine.gPlayerData.mTalent.FindPlayerTalent((CTalent.eTalentType)i);

            node.Init(talent);

            if ( /* talent.mLvL > 0 &&*/ Index < mTipText.Length)
                mTipText[Index++].text = gDefine.gPlayerData.mTalent.GetValueDes( talent.mType);
        }

        ReCalcSize();

        Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void ReCalcSize()
    {
        //计算菜单的长度
        float nodel = Screen.width / 3.8f;
        float l = nodel * (int)CTalent.eTalentType.Count + 8 * 6;
        mNodeRootTrans.sizeDelta = new Vector2(mNodeRootTrans.sizeDelta.x, l);

        //计算整个控件的大小，位置
        float uiscale = gDefine.RecalcUIScale();


        float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080*uiscale, Screen.height - TopH - DownH);
        rt.position = new Vector3(Screen.width/2, Screen.height-TopH,0);

        //计算下方提示框的大小
        //mParamTipCtl.sizeDelta = new Vector2(0, 217*percent);

        //计算天赋区域大小
        //mListCtl.sizeDelta = new Vector2(0,  rt.sizeDelta.y - mParamTipCtl.sizeDelta.y);
    }
}
