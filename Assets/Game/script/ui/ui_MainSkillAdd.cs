using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainSkillAdd : MonoBehaviour
{
    public GameObject mNodePreb;
    public RectTransform mNodeRootTrans;
    List<ui_MainSkillAddNode> mNodeArr = new List<ui_MainSkillAddNode>();

    public ui_MainSkillSkillTip mRefTip;


    public void Refresh()
    {
        ui_MainSkillAddNode node;
        int index = 0;
        for (int i = 0; i < (int)CSkillAdd.eSkillAdd.Count; i++)
        {
            if(i==(int)CSkillAdd.eSkillAdd.BigLun_Lighting ||i==(int)CSkillAdd.eSkillAdd.Kill
            ||i==(int)CSkillAdd.eSkillAdd.StarBless || i==(int)CSkillAdd.eSkillAdd.BulletBack  )
                continue;

            if (index < mNodeArr.Count)
             {
                  node = mNodeArr[index];
             }  
            else
            {
                node = GameObject.Instantiate(mNodePreb).GetComponent<ui_MainSkillAddNode>();
                node.gameObject.SetActive(true);
                node.gameObject.transform.SetParent(mNodeRootTrans);
                mNodeArr .Add(node);
            }

            index++;

            CSkillAddData data = gDefine.gPlayerData.mSkillAdd.Find((CSkillAdd.eSkillAdd)i);

            node.Init(data, this);

        }

        ReCalcSize();
         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void ShowSkillTip(Transform T, CSkill SkillData)
    {
        mRefTip.Show(T, SkillData);
    }

    public void ReCalcSize()
    {
         //计算整个控件的大小，位置
        float uiscale = gDefine.RecalcUIScale();

        //计算菜单的长度
        float nodel = 270.0f * uiscale;
        float l = nodel * ((int)CSkillAdd.eSkillAdd.Count -4+1);
        mNodeRootTrans.sizeDelta = new Vector2(mNodeRootTrans.sizeDelta.x, l);

       


        float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080*uiscale, Screen.height - TopH - DownH);
        rt.position = new Vector3(Screen.width/2, Screen.height-TopH,0);



        // //计算菜单的长度
        // float nodel = Screen.width / 3.8f;
        // float l = (nodel + 8) * (int)CSkillAdd.eSkillAdd.Count ;
        // mNodeRootTrans.sizeDelta = new Vector2(mNodeRootTrans.sizeDelta.x, l);

        // //计算整个控件的大小，位置
        // float percent = Screen.width / 1080.0f;


        // float TopH = gDefine.gMainUI.mRefMainUp.CalcH();
        // float DownH = gDefine.gMainUI.mRefMainDown.CalcH();

        // RectTransform rt = GetComponent<RectTransform>();
        // rt.sizeDelta = new Vector2(Screen.width, Screen.height - TopH - DownH);
        // rt.position = new Vector3(Screen.width/2, Screen.height-TopH,0);

        //计算下方提示框的大小
       // mParamTipCtl.sizeDelta = new Vector2(0, 217*percent);

        //计算天赋区域大小
       // mListCtl.sizeDelta = new Vector2(0,  rt.sizeDelta.y - mParamTipCtl.sizeDelta.y);
    }
}
