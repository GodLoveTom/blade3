using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Magic : MonoBehaviour
{
    public GameObject mBtnChange;
    public GameObject mBtnQuit;

    const int mCtlL = 5;
    public GameObject[] mChoose5Btn = new GameObject[5];
    CMagicData[] mMagicArr;
    public ui_magicConfim mRefUIConfim;
    public GameObject ui_choose;
    public int mChooseIndex = -1;
    public Text mNameText;
    public Text mDesText;
    public Text []mNameArrText;
    public Text []mDesArrText;
    public GameObject[] mChooseFrame = new GameObject[5];
    public Image[] mBtnImage = new Image[5];
    public Text mBtnText;
    public Text mBtnQuitText;

    public void Btn_ChangeCallBack()
    {
        gDefine.gAd.PlayADVideo(ADChangeCallBack);
         Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "附魔换一波"); 
          

            TalkingDataGA.OnEvent("激励视频广告", dic);
    }
    public void Btn_Change()
    {
        gDefine.gBtnAnim.Init(mBtnChange,1,Btn_ChangeCallBack )   ;
    }

    public void ADChangeCallBack(bool Finished)
    {
        if (Finished)
        {
            gDefine.gMagic.ReSetMagicData();
            mMagicArr = gDefine.gMagic.GetDataArr();
            Refresh();
        }
    }

    public void Btn_Choose(int Index)
    {
        if (mMagicArr.Length == 1)
            Index = 0;
        // if (mChooseIndex < 0 || mChooseIndex != Index)
        // {
        //     mChooseIndex = Index;
        //     Refresh();
        // }
        // else
        {
            mRefUIConfim.Init(mMagicArr[Index], this);
            ui_choose.SetActive(false);
            mRefUIConfim.gameObject.SetActive(true);
        }

    }

    public bool Show(int LvL)
    {
        if (LvL % 10 == 0)
        {
            if (LvL == 10)
                gDefine.gMagic.ReSetMagicData();

            mChooseIndex = -1;
            mDesText.text = "";


            mMagicArr = gDefine.gMagic.GetDataArr();
            if (mMagicArr.Length > 0)
            {
                gameObject.SetActive(true);
                ui_choose.SetActive(true);
                Refresh();
                mRefUIConfim.gameObject.SetActive(false);
                gDefine.gPause = true;

                return true;
            }

        }
        return false;
    }

    void Refresh()
    {
        gDefine.RecalcAutoSize(mRefUIConfim.gameObject);
        gDefine.RecalcAutoSize(ui_choose);

        int beginIndex = 0;
        if (mMagicArr.Length == 2)
            beginIndex = 3;
        else if (mMagicArr.Length == 1)
        {
            beginIndex = 1;
            mChooseIndex = 0;
        }



        for (int i = 0; i < mCtlL; i++)
        {
            mChoose5Btn[i].gameObject.SetActive(false);
            mChooseFrame[i].SetActive(false);
            mNameArrText[i].text ="";
            mDesArrText[i].text ="";
        }


        for (int i = 0; i < mMagicArr.Length; i++)
        {
            mChoose5Btn[beginIndex + i].gameObject.SetActive(true);
            mBtnImage[beginIndex + i].sprite = mMagicArr[i].GetIcon();
            mNameArrText[beginIndex+i].text = mMagicArr[i].GetName();
            gDefine.SetTextBold();
            mDesArrText[beginIndex+i].text = mMagicArr[i].GetDes();
            gDefine.SetTextBold();
        }

        if (mChooseIndex >= 0)
        {
            mChooseFrame[mChooseIndex + beginIndex].SetActive(true);
            mDesText.text = mMagicArr[mChooseIndex].GetDes();
            gDefine.SetTextBold();
            // mChooseImage.sprite = mMagicArr[mChooseIndex].GetIcon();
        }

        mBtnText.text = gDefine.GetStr("换一批");
        
        mNameText.text = gDefine.GetStr("附    魔");
        
        mBtnQuitText.text = gDefine.GetStr(306);
        

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }

    public void Close()
    {
        gameObject.SetActive(false);
        gDefine.gPause = false;
    }

    public void Btn_Close()
    {
        gDefine.gBtnAnim.Init(mBtnQuit, 1, Close);
          gDefine.gPause = false;
    }


}
