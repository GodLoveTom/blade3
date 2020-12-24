using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainGainTip : MonoBehaviour
{
    public ui_ScaleBig mRefUIScaleBig;
    public Image mIcon;
    public Sprite[] mSpriteArr = new Sprite[2];
    public Text mNumText;
    float mT = 0;
    float mFlyCoinT = 0;
    bool mIsCoin = false;
    bool mIsCrystal = false;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > mT)
            gameObject.SetActive(false);

        if (Time.time > mFlyCoinT && (mIsCoin || mIsCrystal))
        {
            if (mIsCoin)
                gDefine.PlayUIGainCoinSE(gameObject);
            if (mIsCrystal)
                gDefine.PlayUIGainCrystalSE(gameObject);
            mIsCoin = false;
            mIsCrystal = false;
        }
    }

    public void Show(int ItemId, int Num)
    {
        CItem it = gDefine.gData.GetItemData(ItemId);

        mIcon.sprite = it.GetIconSprite();
        mNumText.text = "+" + Num.ToString();
        gameObject.SetActive(true);

        mRefUIScaleBig.Play();

        mIsCoin = (ItemId == 201)?true:false;
        mIsCrystal = (ItemId == 202)?true:false;

        if( mIsCoin || mIsCrystal)
            mT = Time.time + 3f;
        else
             mT = Time.time + 2f;

        mFlyCoinT = Time.time + 0.4f;

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }
}
