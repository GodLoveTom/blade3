using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Option : MonoBehaviour
{
    public int mFromWhere;
    public GameObject mPlane0;
    public GameObject mPlane1;
    public GameObject mUpCtl;
    public InputField mInput;
    public InputField mNum;

    public Sprite[] mMusicSprite;
    public Sprite[] mSoundSprite;

    public Image mMusicImage;
    public Image mSoundImage;

    public GameObject[] mLanguageObj;

    public Text mMakerTip;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Show(int Where)
    {
        gameObject.SetActive(true);
        mFromWhere = Where;
        Refresh();
    }

    public void Refresh()
    {
        mPlane0.SetActive(true);
        mPlane1.SetActive(false);

        mSoundImage.sprite = (gDefine.gPlayerData.mSoundIsOpen) ? mSoundSprite[0] : mSoundSprite[1];
        mMusicImage.sprite = (gDefine.gPlayerData.mMusicIsOpen) ? mMusicSprite[0] : mMusicSprite[1];

        mMakerTip.text = gDefine.GetStr(383);

        RefreshLanguage();
        gDefine.RecalcAutoSize(mPlane0);
        gDefine.RecalcAutoSize(mPlane1);
        gDefine.RecalcAutoSize(mUpCtl);

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);

    }

    public void Btn_MoreGame()
    {
        Application.OpenURL("https://apps.apple.com/us/app/id1412622515");
         Dictionary<string, object> dic = new Dictionary<string, object>();
        TalkingDataGA.OnEvent("设置界面推荐", dic);
    }

    public void Btn_GiveMeStar()
    {
        const string APP_ID = "1524622001";
        var url = string.Format(
            "itms-apps://itunes.apple.com/cn/app/id{0}?mt=8&action=write-review",
            APP_ID);
        Application.OpenURL(url);

         Dictionary<string, object> dic = new Dictionary<string, object>();

        TalkingDataGA.OnEvent("设置界面评价", dic);
    }

    public void Btn_CloseMaker()
    {
        mPlane0.SetActive(true);
        mPlane1.SetActive(false);
    }

    public void Btn_Maker()
    {
        mPlane0.SetActive(false);
        mPlane1.SetActive(true);

         gDefine.gAd.PlayInterAD(null);
         
        Dictionary<string, object> dic = new Dictionary<string, object>();      
        dic.Clear();
        dic.Add("来源", "设置--制作者名单");
        TalkingDataGA.OnEvent("插屏广告", dic);
    }

    void RefreshLanguage()
    {
        //string [] strArr = new string[]{"简体中文","繁體中文","English"} ;

        for (int i = 0; i < 10; i++)
        {
            Image image = mLanguageObj[i].GetComponent<Image>();
            Text text = mLanguageObj[i].transform.GetChild(0).GetComponent<Text>();
            text.text = gDefine.GetStr(179 + i);
            //strArr[i];//gDefine.gMyStr.Get(strArr[i], gDefine.gPlayerData.mLanguageType);
            image.color = ((int)gDefine.gPlayerData.mLanguageType == i) ?
              Color.white : Color.clear;
            if (i >= 4)
                text.color = Color.gray;

        }
    }

    public void Btn_Sound()
    {
        gDefine.gPlayerData.mSoundIsOpen = !gDefine.gPlayerData.mSoundIsOpen;
        gDefine.gSound.EnableSound(gDefine.gPlayerData.mSoundIsOpen);
        //  mSoundImage.sprite = (gDefine.gPlayerData.mSoundIsOpen) ? mSoundSprite[0] : mSoundSprite[1];
        mMusicImage.sprite = (gDefine.gPlayerData.mSoundIsOpen) ? mMusicSprite[0] : mMusicSprite[1];
        gDefine.gPlayerData.Save();

         Dictionary<string, object> dic = new Dictionary<string, object>();


        TalkingDataGA.OnEvent("设置界面音效", dic);
    }

    public void Btn_Music()
    {
        gDefine.gPlayerData.mMusicIsOpen = !gDefine.gPlayerData.mMusicIsOpen;
        gDefine.gSound.EnableMusic(gDefine.gPlayerData.mMusicIsOpen);

        mSoundImage.sprite = (gDefine.gPlayerData.mMusicIsOpen) ? mSoundSprite[0] : mSoundSprite[1];
        gDefine.gPlayerData.Save();

         Dictionary<string, object> dic = new Dictionary<string, object>();

       
        TalkingDataGA.OnEvent("设置界面音乐", dic);
    }

    public void Btn_Language(int Index)
    {
        if(Index>=4)
            return;
        gDefine.gPlayerData.mLanguageType = (CMyStr.eType)Index;
        RefreshLanguage();
        gDefine.gPlayerData.Save();

        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("选项", Index.ToString());
        TalkingDataGA.OnEvent("设置界面语言", dic);

        gDefine.gMainUI.mRefMainDown.Refresh();

        gDefine.gAd.PlayInterAD(null);
         
        // Dictionary<string, object> dic = new Dictionary<string, object>();      
         dic.Clear();
        dic.Add("来源", "设置--语言界面");
        TalkingDataGA.OnEvent("插屏广告", dic);
    }


    public void Btn_GiveMoney()
    {
        gDefine.gPlayerData.Coin += 100;
    }

    public void Btn_GiveCrystal()
    {
        gDefine.gPlayerData.Crystal += 100;
    }

    public void Btn_ClearAll()
    {
        gDefine.gPlayerData.ClearAllData();
    }

    public void Btn_lvlup()
    {
        if (gDefine.gPlayerData.LVL < 59)
            gDefine.gPlayerData.LVL++;
    }


    public void Btn_GiveItem()
    {
        int itemId = -1;
        int num = 0;
        if (int.TryParse(mInput.text, out itemId))
        {
            if (int.TryParse(mNum.text, out num))
                gDefine.gPlayerData.AddItemToBag(itemId, num);
        }
    }

    public void Btn_Close()
    {
        gameObject.SetActive(false);
        switch (mFromWhere)
        {
            case 0:
                gDefine.gMainUI.ChangeToShop();
                break;
            case 1:
                gDefine.gMainUI.ChangeToEquipent();
                break;
            case 2:
                gDefine.gMainUI.ChangeToFight();
                break;
            case 3:
                gDefine.gMainUI.ChangeToTalent();
                break;
            case 4:
                gDefine.gMainUI.ChangeToKillAdd();
                break;
        }

    }
}
