using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_LackTip : MonoBehaviour
{
    public Image mIcon;
    public Text mText;

    public Image mIconEN;
    public Text mTextEN;

     public Image mIcon1;
      public Text mText1;

     
    int mNum;
    public delegate void CallBackFunc();
    CallBackFunc mFunc;
    CItem mIt ;
    
    public void Show(int ItemId, int Num, CallBackFunc Func)
    {
        if(ItemId == 201 || ItemId == 202)
            Num = 300;

        if(gDefine.gPlayerData.mLanguageType == CMyStr.eType.English || 
            gDefine.gPlayerData.mLanguageType == CMyStr.eType.Japanese )
            {
                mIconEN.gameObject.SetActive(true);
                mTextEN.gameObject.SetActive(true);
                 mIcon.gameObject.SetActive(false);
                mText.gameObject.SetActive(false);
            }
            else
            {
                 mIconEN.gameObject.SetActive(false);
                mTextEN.gameObject.SetActive(false);
                 mIcon.gameObject.SetActive(true);
                mText.gameObject.SetActive(true);
            }

        // {
        //     PlayerPrefs.SetInt("iap_door",1);
        //     PlayerPrefs.Save();
        // }

        CItem it = gDefine.gData.GetItemData(ItemId);
        mIcon.sprite = it.GetIconSprite();
        mIcon1 .sprite = it.GetIconSprite();
        mIconEN.sprite = it.GetIconSprite();

        string str="";
        if(ItemId == 201)
            str = gDefine.GetStr(160);
        else if(ItemId == 202)
            str = gDefine.GetStr(159);
        else if( it.mMainType == CItem.eMainType.Piece)
            str = gDefine.GetStr(275);
        else if( it.mMainType == CItem.eMainType.Scroll)
            str = gDefine.GetStr(302);

        
        mText .text = str;
        mTextEN.text = str;
        
        mText1.text = "+" + Num.ToString();
       

        mIt = it;
        mNum = Num;

        mFunc = Func;

        gameObject.SetActive(true);

        gDefine.RecalcAutoSize(gameObject);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void Btn_AD()
    {
        gDefine.gAd.PlayADVideo(ADCallBack);
         Dictionary<string, object> dic = new Dictionary<string, object>();
            
            dic.Add("来源", "物资匮乏提示界面"); 
             dic.Add("名称", mIt.mName); 

            TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void ADCallBack(bool Finish)
    {
        if(Finish)
        {
            if(mIt.Id == 201)
            {
                gDefine.gPlayerData.Coin += mNum;
            }
            else if(mIt.Id == 202)
            {
                gDefine.gPlayerData.Crystal += mNum;
            }
            else 
            {
                gDefine.gPlayerData.AddItemToBag(mIt.Id, mNum);
            }
        
            gDefine.gMainGainTip.Show(mIt.Id, mNum);

            if(mFunc!=null)
                mFunc();
            mFunc = null;
        }
        gameObject.SetActive(false);
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false); 
    }
}
