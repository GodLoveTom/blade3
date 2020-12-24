using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainIapMainPage : MonoBehaviour
{
    public Text mTitleText;
    public RectTransform mRefUp;
    public RectTransform mRefMid;
    public RectTransform mRefContainCtl;
    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
        RecalcSize();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


    void Refresh()
    {
       mTitleText .text = gDefine.GetStr(423);
    }

    void RecalcSize()
    {
        float percent = gDefine.RecalcUIScale();
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(1080 * percent , Screen.height);

        float upH = mRefUp.sizeDelta.y;

        mRefMid.sizeDelta = new Vector2( 0, Screen.height - upH);
        mRefMid.localPosition = new Vector3(0,Screen.height*0.5f-upH,0);

         //计算菜单的长度
        float nodel = 1023.184f * percent / 2.81249f;
        float l = nodel ;
        nodel = 1023.184f * percent / 3.978163f;
        l +=  nodel* (int)(6 + 1);
        mRefContainCtl.sizeDelta = new Vector2(mRefContainCtl.sizeDelta.x, l);


    }
   
}
