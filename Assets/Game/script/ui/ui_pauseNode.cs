using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ui_pauseNode : MonoBehaviour
{
    public GameObject [] mBtnArr =new GameObject[4];
    public GameObject [] mTipRefPoint =new GameObject[4];
    CSkill [] mSkillArr = new CSkill[4];
    ui_pause mUIRoot;

    public void Refresh(CSkill [] SkillArr, ui_pause UIRoot)
    {
        mUIRoot = UIRoot;

        for(int i=0; i<mBtnArr.Length; i++)
        {
            mSkillArr[i] = SkillArr[i];
            if(SkillArr[i]!=null)
            {
                mBtnArr[i].gameObject.SetActive(true);
                mBtnArr[i].gameObject.GetComponent<Image>().sprite = 
                    gDefine.gABLoad.GetSprite("icon.bytes", SkillArr[i].mName) ;
            }
            else
            {
                mBtnArr[i].gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(true);
    }

    public void Btn_ShowTip(int Index)
    {
        if(mSkillArr[Index]!=null)
        {
            mUIRoot.mRefTip.Show(mTipRefPoint[Index].transform, mSkillArr[Index]);
        }
    }
}
