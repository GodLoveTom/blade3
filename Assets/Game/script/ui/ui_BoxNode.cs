using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_BoxNode : MonoBehaviour
{
    public ui_Boxout mRefUIBOXOut;
    public GameObject mUpObj;
    public GameObject mUpObj1;
    public GameObject mLightObj;

    public Text mNameText;
    public Text mTimeText;
    public int mBoxIndex;
    public ui_Box mRefUIBox;
    float mT = 0;
    int mIndex = 0;
    public Image mSEImage;

    public Font mNormalFont;
    public Font mNumFont;

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }

    // public void Init( int BoxIndex, ui_Box UIBox)
    // {
    //     mBoxIndex = BoxIndex;
    //     mRefUIBox = UIBox;

    //     mNameText .text = gDefine.gBoxData.GetBoxName(BoxIndex);
    //     mTimeText.text = gDefine.gBoxData.GetBoxT(BoxIndex);

    //     gameObject.SetActive(true);


    // }

    void Refresh()
    {
        mUpObj.SetActive(true);
        mUpObj1.SetActive(false);
        mLightObj.SetActive(false);

        mNameText.text = gDefine.gBoxData.GetBoxName(mBoxIndex);

        if (gDefine.gBoxData.IsCanGetBoxPrize(mBoxIndex))
        {
            mSEImage.gameObject.SetActive(true);
            mTimeText.text = "00:00:00";
            
            if (mT < 0)
            {
                mIndex = (mIndex + 1) % mRefUIBOXOut.mSEArr.Length;
                mT = 0.1f;
            }
            mT -= Time.deltaTime;
            mSEImage.sprite = mRefUIBOXOut.mSEArr[mIndex];
        }
        else
        {
            mSEImage.gameObject.SetActive(false);
            mTimeText.text = gDefine.gBoxData.GetBoxT(mBoxIndex);
            
            if (mTimeText.text.Contains(":"))
                mTimeText.font = mNumFont;
            else
                mTimeText.font = mNormalFont;
        }

    }



    public void Btn_Click()
    {

        if (gDefine.gBoxData.IsCanGetBoxPrize(mBoxIndex))
            mRefUIBox.ShowPrize(mBoxIndex);
        else
        {
            gDefine.ShowTip(gDefine.GetStr(379));//"时间未到,无法开启宝箱"
        }
    }
}
