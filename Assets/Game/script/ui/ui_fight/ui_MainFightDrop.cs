using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainFightDrop : MonoBehaviour
{
    public ui_MainShopEquipRingTip mRefRingTip;
    public ui_MainShopEquipTip mRefEquipTip;
    public ui_MainShopItemScrollTip mRefScrollTip;

    public Text mTipText;
    public Image [] mImageArr;

    public ShopData mShopdata = new ShopData();
    
    public void Show()
    {
        CChapterDropDataParam drop =  gDefine.gDropSystem.FindCharpter(gDefine.gChapterId);

        mTipText .text = gDefine.GetStr(448);
        gDefine.SetTextBold();

        int index =0;

        for(int i=0; i<drop.mPiece.Count; i++)
        {
             mImageArr[index].sprite = drop.mPiece[i].GetIconSprite();
             mImageArr[index++].gameObject.SetActive(true);
        }   

        for(int i=0; i<drop.mSkill.Count; i++)
        {
             mImageArr[index].sprite = drop.mSkill[i].GetIconSprite();
             mImageArr[index++].gameObject.SetActive(true);
        }   

        for(int i= index; i<mImageArr.Length; i++)
            mImageArr[i].gameObject.SetActive(false);
        
        gameObject.SetActive(true);
    }

   public void Btn_Click(int Index)
   {
       CChapterDropDataParam drop =  gDefine.gDropSystem.FindCharpter(gDefine.gChapterId);
       CItem it;
       if(Index < drop.mPiece.Count)
            it = drop.mPiece[Index];
       else
           it = drop.mSkill[Index-drop.mPiece.Count];

        mShopdata.mItemId = it.Id;
        mShopdata.mItemNum = 1;

        mRefRingTip.gameObject.SetActive(false);
         mRefScrollTip.gameObject.SetActive(false);
          mRefEquipTip.gameObject.SetActive(false);

        if(it.mMainType==CItem.eMainType.Ring)
            mRefRingTip.Show(  mImageArr[Index].gameObject.transform,mShopdata);
        else if(it.mMainType==CItem.eMainType.Scroll)
            mRefScrollTip.Show(  mImageArr[Index].gameObject.transform,mShopdata);
        else
            mRefEquipTip.Show(  mImageArr[Index].gameObject.transform,mShopdata);
   }

   public void Btn_Close()
   {
       gameObject.SetActive(false);
   }
}
