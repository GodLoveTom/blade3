using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainUp : MonoBehaviour
{

    public Text mLvLText;
    public Text mEXPText;
    public Text mTiLiText;
    public Text mCoinText;
    public Text mCrystalText;
    public Image mEXPImage;
    public GameObject mEXPMask;
    
  


    public bool mNeedRefresh = true;

    // Start is called before the first frame update
    void Start()
    {
        mNeedRefresh = true;
        gameObject.transform.position -= Vector3.up * Screen.safeArea.y;
    }

    public float CalcH()
    {
        return Screen.width * 181.0f / 1080.0f +  Screen.safeArea.y;
    }

    public void NeedRefresh()
    {
        mNeedRefresh = true;
    }

    // Update is called once per frame
    void Update()
    {
        if( mNeedRefresh )
        {
            mNeedRefresh = false;
            Refresh();
        }

        UpdateFlash();
    }

    void UpdateFlash()
    {

    }


    public void Refresh()
    {
        mLvLText.text = gDefine.gPlayerData.LVL.ToString();

        CPCLvLParam pclvldata = gDefine.gData.GetPCData( gDefine.gPlayerData.LVL);

        double perc = (double)gDefine.gPlayerData.EXP / (double)pclvldata.mEXP;

        if (perc < 0) perc = 0;
            
        mEXPText.text = ((int)(perc*100)).ToString()+ "%";

        if (perc <= 0)
            mEXPImage.gameObject.SetActive(false);
        else
        {
            mEXPMask.transform.localScale = new Vector3((float)perc, 1, 1);
            mEXPImage.transform.localScale = new Vector3(1.0f / (float)perc, 1, 1);
            mEXPImage.gameObject.SetActive(true);
        }

        mTiLiText.text = gDefine.gPlayerData.TiLI.ToString();
        mCoinText.text = gDefine.gPlayerData.Coin.ToString();
        mCrystalText.text = gDefine.gPlayerData.Crystal.ToString();
        //  Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        // foreach(Text _t in textArr)
        //     gDefine.ResetFontBold(_t);
    }

    public void Btn_ShowUI_BuyTiLi()
    {

    }

    public void Btn_ShowUI_BuyCoin()
    {

    }

    public void Btn_ShowUI_BuyCrystal()
    {

    }

    public void Btn_ShowUI_Setting()
    {
        //clear all save.
        //gDefine.gPlayerData.ClearAllData();
        gDefine.gMainUI.ChangeToOption();
        gDefine.PlayUIClickSound();

    }
}
