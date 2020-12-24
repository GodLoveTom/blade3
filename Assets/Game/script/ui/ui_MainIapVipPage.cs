using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainIapVipPage : MonoBehaviour
{
    public Text [] mTextArr = new Text[6];
    public Text [] mChnTipArr = new Text[5];
    public Text [] mEngTipArr = new Text[5];
    public Text mBtnText;
    public Text mTitleText;
    public ui_MainIap mRefRoot;
    public RectTransform mRefUp;
    public RectTransform mRefMid;
    public RectTransform mRefDown;
    public GameObject mCHNNode;
    public GameObject mEngNode;

    public RectTransform mContainCtl;
    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
        RecalcSize();
    }
    

    void Refresh()
    {
        float percent = gDefine.RecalcUIScale();
       

        mTitleText.text = gDefine.GetStr(422);

        mTextArr[0].text = gDefine.GetStr(415);  
       // mTextArr[1].text = gDefine.GetStr(416);
        mTextArr[2].text = gDefine.GetStr(417);
       // mTextArr[3].text = gDefine.GetStr(418);
        mTextArr[4].text = gDefine.GetStr(419);
       // mTextArr[5].text = gDefine.GetStr(420);

        mChnTipArr[0].text = mEngTipArr[0].text =gDefine.GetStr(408);
        mChnTipArr[1].text = mEngTipArr[1].text =gDefine.GetStr(409);
        mChnTipArr[2].text = mEngTipArr[2].text =gDefine.GetStr(410);
        mChnTipArr[3].text = mEngTipArr[3].text =gDefine.GetStr(411);
        mChnTipArr[4].text = mEngTipArr[4].text =gDefine.GetStr(412);
        mChnTipArr[5].text = mEngTipArr[5].text =gDefine.GetStr(413);
        mChnTipArr[6].text = mEngTipArr[6].text =gDefine.GetStr(414);

        int fontSize = (int)(40 * percent);

        for(int i=0; i<5;i++)
             mTextArr[i].fontSize = fontSize;
        fontSize = (int)(35 * percent);

        for(int i=0; i<7;i++)
        {
            mChnTipArr[i].fontSize = mEngTipArr[i].fontSize = fontSize;
        }
            
        if( PlayerPrefs.GetInt("vip",0) == 0 )
        {
            if(gDefine.gPlayerData.mLanguageType == CMyStr.eType.Simple)
                mBtnText.text = "¥18";
            else
            {
                mBtnText.text = "$2.99";
            }
        }
        else
        {
             mBtnText.text = gDefine.GetStr(421);;
        }

        if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.English )
        {
            mEngNode.SetActive(true);
            mCHNNode.SetActive(false);
        }
        else
        {
            mEngNode.SetActive(false);
            mCHNNode.SetActive(true);
        }

        mContainCtl.localPosition = Vector3.zero;

    }

    void RecalcSize()
    {
        float percent = gDefine.RecalcUIScale();
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2( 1080*percent, Screen.height);


        float y = mRefUp.sizeDelta.y;

        float midH = Screen.height - y - mRefDown.sizeDelta.y;

        mRefMid.transform.localPosition = new Vector3(0, Screen.height *0.5f - y, 0);
        mRefMid.sizeDelta = new Vector2( mRefMid.sizeDelta.x , midH);

        mRefDown.transform.localPosition = new Vector3(0, Screen.height *0.5f -Screen.height + mRefDown.sizeDelta.y);

        //计算菜单的长度

        float nodel = 1028.747f * percent / 3.978163f;
        float l = nodel * (3 + 0.2f) ;
        if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.English )
        {
            nodel = 1023.184f * percent / 1.384213f;
            l+=nodel;
        }
        else
        {
            nodel = 1023.184f * percent / 1.70097f;
            l+=nodel;
        }
        
        mContainCtl.sizeDelta = new Vector2(mContainCtl.sizeDelta.x, l);

    }

    public void Btn_LinkA()
    {
         Application.OpenURL("https://www.facebook.com/FuncoreStudio/posts/742323816340157");
    }

    public void Btn_LinkB()
    {
        Application.OpenURL("https://www.facebook.com/FuncoreStudio/posts/741035853135620");
    }

    public void Btn_Close()
    {
        Close();
        mRefRoot.Show();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
