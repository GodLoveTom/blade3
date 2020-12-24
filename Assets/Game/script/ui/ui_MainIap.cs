using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainIap : MonoBehaviour
{
    public ui_MainIapMainPage mIapObj;
    public ui_MainIapVipPage mVipObj;
    public Text mText; //debug_msg

    public Text[] mRemoveADSTextArr;
    public Text[] mBuyText = new Text[6];//0-4; 5vip
    public iap miap;
    public RectTransform mFrame;


    // Update is called once per frame
    void Update()
    {
        mText.text = miap.result;
    }

    public void Show()
    {
        mBuyText[0].text = gDefine.GetStr(453);
        mBuyText[1].text = gDefine.GetStr(454);
        mBuyText[2].text = gDefine.GetStr(455);
        mBuyText[3].text = gDefine.GetStr(456);
        mBuyText[4].text = gDefine.GetStr(457);
        mBuyText[5].text = gDefine.GetStr(458);
        mBuyText[6].text = gDefine.GetStr(461);

        gameObject.SetActive(true);
        mVipObj.Close();
        mIapObj.Show();

        if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.English)
        {
            mRemoveADSTextArr[0].gameObject.SetActive(false);
            mRemoveADSTextArr[1].gameObject.SetActive(true);
            mRemoveADSTextArr[0].text = gDefine.GetStr(460);
        }
        else
        {
            mRemoveADSTextArr[0].gameObject.SetActive(true);
            mRemoveADSTextArr[1].gameObject.SetActive(false);
            mRemoveADSTextArr[0].text = gDefine.GetStr(460);
        }

        float perc = gDefine.RecalcUIScale();
        float nodel = 363.8f * perc ;
        nodel += 257.2f * perc  * 8;

        mFrame.sizeDelta = new Vector2( mFrame.sizeDelta.x, nodel);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }

    public void Btn_ChangeToVip()
    {
        mVipObj.Show();
        mIapObj.Close();
    }

    public void Btn_ChangeToMainPage()
    {
        mVipObj.Close();
        mIapObj.Show();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        gDefine.gMainUI.ChangeToFight();
    }

    public void Btn_BuyCoins()
    {
        if (gDefine.gPlayerData.Crystal >= 1000)
        {
            gDefine.gPlayerData.Coin += 2000;
            gDefine.gPlayerData.Crystal -= 1000;
            gDefine.gMainGainTip.Show(201, 2000);
            gDefine.PlaySound(57);
        }
        else
        {
            gDefine.PlaySound(24); //标准ui点击
        }
    }
}
