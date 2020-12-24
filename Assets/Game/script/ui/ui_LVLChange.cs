using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_LVLChange : MonoBehaviour
{
    public Text mLeft;
    public Text mMid;
    public Text mRight;
    public Text mRightRight;

    public void PlayFirstLvL()
    {
        gameObject.SetActive(true);
        mMid .text= "1";
        mRight .text= "2";
        gameObject.GetComponent<Animator>().Play("ComLvL",0,0);
        gameObject.GetComponent<Animator>().Play("FirstLvL",0,0);
        PlayerPrefs.SetInt("continueWaveLvL", 1);
        PlayerPrefs.Save();
        
    }

    public void PlayEndLvL(int CurLvL)
    {
        gameObject.SetActive(true);
        mMid .text= (CurLvL-1).ToString();
        mRight .text= (CurLvL).ToString();
        gameObject.GetComponent<Animator>().Play("LastLvL",0,0);
        gDefine.gPlayerData.mChapterEx.AddLvLFinish(gDefine.gChapterId,  gDefine.gChapterDifficult, CurLvL-1);
        PlayerPrefs.SetInt("continueWaveLvL", CurLvL-1);
        PlayerPrefs.Save();
    }

    public void PlayCommonLvL(int CurLvL)
    {
        gameObject.SetActive(true);
        mLeft.text = (CurLvL-2).ToString();
        gDefine.SetTextBold();
        mMid .text= (CurLvL-1).ToString();
        mRight .text= (CurLvL).ToString();
        mRightRight .text= (CurLvL+1).ToString();
        if(CurLvL==2)
        gameObject.GetComponent<Animator>().Play("SecondLvL",0,0);
        else
        gameObject.GetComponent<Animator>().Play("ComLvL",0,0);
        gDefine.gPlayerData.mChapterEx.AddLvLFinish(gDefine.gChapterId,  gDefine.gChapterDifficult, CurLvL-1);
         PlayerPrefs.SetInt("continueWaveLvL", CurLvL-1);
        PlayerPrefs.Save();
    }

    public void Event_End()
    {
        gameObject.SetActive(false);
        gDefine.gLogic.GoNextLvLChange();
    }
}
