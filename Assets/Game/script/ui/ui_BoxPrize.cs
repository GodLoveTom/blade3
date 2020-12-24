using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_BoxPrize : MonoBehaviour
{
    public Text[] mNameText;
    public Image[] mIcon;
    public Text[] mNumText;
    public Text[] mConstTipText = new Text[2];

    public GameObject mRoot;


    int mBoxIndex = -1;
    string mTipStr;
    ui_Box mRefRoot;

    bool mIsThree = false;

    public void Init(int BoxIndex, ui_Box UIBox)
    {
        //if (Random.Range(0, 100) < 20)
        mIsThree = true;
        //else
        // mIsThree = false;

        mConstTipText[0].text = gDefine.GetStr(382);
        gDefine.SetTextBold();
        // mBtnQuitText.text = gDefine.GetStr();

        if (mIsThree)
            mConstTipText[1].text = gDefine.GetStr(381);//三倍
        else
        {
            mConstTipText[1].text = gDefine.GetStr(380);//双倍
            gDefine.SetTextBold();

        }

        mRefRoot = UIBox;
        mBoxIndex = BoxIndex;
        switch (mBoxIndex)
        {
            case 0:
                mNameText[0].text = "金币";
                gDefine.SetTextBold();
                mNameText[1].text = "钻石";
                gDefine.SetTextBold();
                mNumText[0].text = "200";
                mNumText[1].text = "200";
                mIcon[0].sprite = gDefine.gABLoad.GetSprite("icon.bytes", "金币1");
                mIcon[1].sprite = gDefine.gABLoad.GetSprite("icon.bytes", "钻石1");

                break;
            case 1:
                {
                    CBoxDataParam box = gDefine.gBoxData.GetBox(BoxData.eBoxType.Piece);

                    CItem it0 = gDefine.gData.GetItemData(box.mItemArr[0]);
                    CItem it1 = gDefine.gData.GetItemData(box.mItemArr[1]);
                    mNameText[0].text = it0.GetNameLocal();
                    gDefine.SetTextBold();
                    mNameText[1].text = it1.GetNameLocal();
                    gDefine.SetTextBold();
                    mNumText[0].text = box.mItemNumArr[0].ToString();
                    mNumText[1].text = box.mItemNumArr[1].ToString();

                    mIcon[0].sprite = it0.GetIconSprite();
                    mIcon[1].sprite = it1.GetIconSprite();

                    //mTipStr = "获得 " + it.mName + " 钻石200";
                }

                break;
            case 2:
                {
                    CBoxDataParam box = gDefine.gBoxData.GetBox(BoxData.eBoxType.Gem);

                    CItem it0 = gDefine.gData.GetItemData(box.mItemArr[0]);
                    CItem it1 = gDefine.gData.GetItemData(box.mItemArr[1]);
                    mNameText[0].text = it0.GetGemNameLocal();
                    gDefine.SetTextBold();
                    mNameText[1].text = it1.GetGemNameLocal();
                    gDefine.SetTextBold();
                    mNumText[0].text = box.mItemNumArr[0].ToString();
                    mNumText[1].text = box.mItemNumArr[1].ToString();

                    mIcon[0].sprite = it0.GetIconSprite();
                    mIcon[1].sprite = it1.GetIconSprite();
                }

                break;

            case 3:
                {
                    CBoxDataParam box = gDefine.gBoxData.GetBox(BoxData.eBoxType.Scroll);

                    CItem it0 = gDefine.gData.GetItemData(box.mItemArr[0]);
                    CItem it1 = gDefine.gData.GetItemData(box.mItemArr[1]);
                    mNameText[0].text = it0.GetNameLocal();
                    gDefine.SetTextBold();
                    mNameText[1].text = it1.GetNameLocal();
                    gDefine.SetTextBold();
                    mNumText[0].text = box.mItemNumArr[0].ToString();
                    mNumText[1].text = box.mItemNumArr[1].ToString();

                    mIcon[0].sprite = it0.GetIconSprite();
                    mIcon[1].sprite = it1.GetIconSprite();

                }

                break;
        }

        gameObject.SetActive(true);

        gDefine.RecalcAutoSize(mRoot);

    }

    // Update is called once per frame

    public void ADGetCallBAck(bool Finished)
    {
        //if(Finished)
        {
            string str = gDefine.gBoxData.GetBoxPrize(mBoxIndex, 1);
            if (!string.IsNullOrEmpty(str))
            {
                // gDefine.ShowTip(str);
                mRefRoot.Refresh();
            }
            gameObject.SetActive(false);
        }
    }

    public void Btn_GetPrize()
    {
       gDefine.gAd.PlayInterAD(ADGetCallBAck);
       Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("来源", "宝箱");
        dic.Add("名称", mBoxIndex.ToString());

        TalkingDataGA.OnEvent("插屏广告", dic);
    }

    public void Btn_GetPrizeDouble()
    {
        gDefine.gAd.PlayADVideo(ADGetDoubleCallBack);
        Dictionary<string, object> dic = new Dictionary<string, object>();

        dic.Add("来源", "宝箱");
        dic.Add("名称", mBoxIndex.ToString());

        TalkingDataGA.OnEvent("激励视频广告", dic);
    }

    public void ADGetDoubleCallBack(bool Finished)
    {
        if (Finished)
        {
            string str = gDefine.gBoxData.GetBoxPrize(mBoxIndex, mIsThree ? 3 : 2);
            if (!string.IsNullOrEmpty(str))
            {
                //gDefine.ShowTip(str);
                mRefRoot.Refresh();
            }
            gameObject.SetActive(false);
        }

    }
}
