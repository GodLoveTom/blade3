using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_BoxPrizeVip : MonoBehaviour
{
    public Text mTitleText;
    public Image[] mIcon;
    public Text[] mNumText;
    public Text[] mBtnAllTextArr; //两个长get 3倍， 中文，英文两个版本
    public Text mBtnGetText;  //普通获得

    public GameObject[] mBtnAllArr; //// 3倍按钮。 中文，英文两个版本

    public GameObject mRoot; //需要进行缩放的节点

    public ui_Box mRefRoot;

    public void Init()
    {
        mTitleText.text = gDefine.GetStr(422); //vip特权
        gDefine.SetTextBold();

        mBtnAllTextArr[0].text = gDefine.GetStr(381);//三倍
        gDefine.SetTextBold();
        mBtnAllTextArr[1].text = gDefine.GetStr(381);//三倍
        gDefine.SetTextBold();

        mBtnGetText.text = gDefine.GetStr(382);//领取奖励
        gDefine.SetTextBold();

        if (gDefine.gPlayerData.mLanguageType == CMyStr.eType.English)
        {
            mBtnAllArr[0].SetActive(false);
            mBtnAllArr[1].SetActive(true);
        }

        else
        {
            mBtnAllArr[0].SetActive(true);
            mBtnAllArr[1].SetActive(false);
        }

        mNumText[0].text = "300";
        mNumText[1].text = "300";
        mIcon[0].sprite = gDefine.gABLoad.GetSprite("icon.bytes", "金币1");
        mIcon[1].sprite = gDefine.gABLoad.GetSprite("icon.bytes", "钻石1");

        CBoxDataParam box = gDefine.gBoxData.GetBox(BoxData.eBoxType.Vip);
        for (int i = 0; i < 6; i++)
        {
            CItem it = gDefine.gData.GetItemData(box.mItemArr[i]);
            mIcon[i + 2].sprite = it.GetIconSprite();
            mNumText[i + 2].text = "1";
        }

        gameObject.SetActive(true);

        gDefine.RecalcAutoSize(mRoot);

    }

    public void Btn_GetPrize()
    {
        string str = gDefine.gBoxData.GetBoxPrize((int)BoxData.eBoxType.Vip, 1);

        CBoxDataParam box = gDefine.gBoxData.GetBox(BoxData.eBoxType.Vip);

        ui_BoxTip script ;
        script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
        script.Show(box.mItemArr[4], 1, box.mItemArr[5], 1, true, 0.75f);

        script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
        script.Show(box.mItemArr[2], 1, box.mItemArr[3], 1, true, 0.5f);

        script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
        script.Show(box.mItemArr[0], 1, box.mItemArr[1], 1, true, 0.25f);

        script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
        script.Show(201, 300, 202, 300, true, 0.01f);
     

        mRefRoot.Refresh();
        gameObject.SetActive(false);
    }

    public void Btn_GetPrizeDouble()
    {
        gDefine.gAd.PlayADVideo(ADGetDoubleCallBack);
        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("来源", "宝箱");
        dic.Add("名称", "vip宝箱");

        TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void ADGetDoubleCallBack(bool Finished)
    {
        if (Finished)
        {
            string str = gDefine.gBoxData.GetBoxPrize((int)BoxData.eBoxType.Vip, 3);

            CBoxDataParam box = gDefine.gBoxData.GetBox(BoxData.eBoxType.Vip);

            ui_BoxTip script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
            script.Show(201, 900, 202, 900, true);

            script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
            script.Show(box.mItemArr[0], 3, box.mItemArr[1], 3, true, 1);

            script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
            script.Show(box.mItemArr[2], 3, box.mItemArr[3], 3, true, 2);

            script = gDefine.gMainUI.mRefMainBox.CreateNewTip();
            script.Show(box.mItemArr[4], 3, box.mItemArr[5], 3, true, 3);

            mRefRoot.Refresh();
            gameObject.SetActive(false);
        }

    }
}

