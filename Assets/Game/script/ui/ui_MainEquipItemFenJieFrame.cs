using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ui_MainEquipItemFenJieFrame : MonoBehaviour
{
    public Image mEquipIcon;
    public Image mPieceIcon;
    public Text mCoinText;
    public Text mPieceText;

    public Text mFenJieTipText;
    public Text mGainTipText;
    public Text mConfirmTipText;
    public Text mCancelTipText;
    
    ui_MainEquipItem mFather;
    float mDelayCloseT;

    public void Init( CGird Gird, ui_MainEquipItem Father)
    {
        mFather = Father;
        mEquipIcon .sprite = Gird.mRefItem.GetIconSprite();
        CItem pieceIt = gDefine.gData.GetItemData( Gird.mRefItem.mPieceItId);
        mPieceIcon .sprite = pieceIt.GetIconSprite() ;

        mFenJieTipText.text = gDefine.GetStr("分解");
        mGainTipText.text = gDefine.GetStr("获得");
        mConfirmTipText.text = gDefine.GetStr(272);//"确    定"
        mCancelTipText.text = gDefine.GetStr(384);//"取    消"
        
        mCoinText.text = "+" + Gird.CalcChaiFenMoney().ToString();
        mPieceText.text = "+" + Gird.CalcChaiPieceNum().ToString();

        mDelayCloseT = Time.time + 0.5f;

        gameObject.SetActive(true);

    }

    public void Btn_FenJie()
    {
        mFather.ChaiFen();
        gameObject.SetActive(false);
    }

    public void Btn_Close()
    {
        if( Time.time >= mDelayCloseT)
            mFather.CloseFenJieFrame();
    }

   
}
