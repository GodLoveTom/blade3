using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_appraise : MonoBehaviour
{
    public Text mTip;
    public Text mOK;
    public Text mCancel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Check()
    {
        if(gameObject.activeSelf)
            return;

        int firstlvl = PlayerPrefs.GetInt("firstlvl",0);
        int appraise =  PlayerPrefs.GetInt("appraise",0);
        if( (firstlvl >= 1 && appraise == 0  && !gDefine.gAppraiseShow))
        {
            gDefine.gAppraiseShow = true;
            gameObject.SetActive(true);
            Show();
        }
            
        else
           gameObject.SetActive(false);    
    }

    public void Show()
    {
        mTip.text = gDefine.GetStr(405);
       
        mOK.text = gDefine.GetStr(403);
       
        mCancel.text = gDefine.GetStr(404);
        

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void Btn_OK()
    {
         const string APP_ID = "1524622001";
        var url = string.Format(
            "itms-apps://itunes.apple.com/cn/app/id{0}?mt=8&action=write-review",
            APP_ID);
        Application.OpenURL(url);

        gDefine.gPlayerData.Crystal += 200;

        PlayerPrefs.SetInt("appraise",1);
        PlayerPrefs.Save();

        gameObject.SetActive(false);
    }

    public void Btn_Cancel()
    {
        gameObject.SetActive(false);

    }
}
