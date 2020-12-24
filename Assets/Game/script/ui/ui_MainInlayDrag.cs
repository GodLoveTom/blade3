using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainInlayDrag : MonoBehaviour
{
    bool mIsShow = false;
    CGird mRefGird;
    public Image mImage;
    bool mIsFromEquip = false;
    ui_MainEquipInLay mFatherUI;
    int mGemIndex = 0;

    public void Show(CGird Gird, ui_MainEquipInLay FatherUI )
    {
        mIsFromEquip = false;
        mFatherUI = FatherUI;
        mRefGird = Gird;
        mIsShow = true;
        mImage.sprite = Gird.mRefItem.GetIconSprite();
        Vector3 scale = Vector3.one * gDefine.RecalcUIScale();
        mImage .transform.localScale = scale;
        mImage.gameObject.SetActive(true);
        Update();
        
    }

    public void Show(CGird Gird, int GemIndex, ui_MainEquipInLay FatherUI )
    {
        mIsFromEquip = true;
        mFatherUI = FatherUI;
        mRefGird = Gird;
        mIsShow = true;
        mGemIndex = GemIndex;

        CItem gem = gDefine.gData.GetItemData(Gird.mGem[ GemIndex]);

        mImage.sprite = gem.GetIconSprite();


        Vector3 scale = Vector3.one * gDefine.RecalcUIScale();
        mImage .transform.localScale = scale;
        mImage.gameObject.SetActive(true);
        Update();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!mIsShow)
            return;

        if( Input.GetMouseButtonUp(0) )
        {
            mIsShow = false;
            mImage.gameObject.SetActive(false);
            //取消，去除，放入
            if( !mIsFromEquip )
            {
                if( mFatherUI.IsInGemGird(0, Input.mousePosition) )
                {
                    mFatherUI.SetGem( mRefGird, 0);
                }
                else  if( mFatherUI.IsInGemGird(1, Input.mousePosition) )
                {
                    mFatherUI.SetGem( mRefGird, 1);
                }
                else  if( mFatherUI.IsInGemGird(2, Input.mousePosition) )
                {
                    mFatherUI.SetGem( mRefGird, 2);
                }
            }
            else
            {
                if( !mFatherUI.IsInGemGird(0, Input.mousePosition) && 
                ! mFatherUI.IsInGemGird(1, Input.mousePosition) &&  ! mFatherUI.IsInGemGird(2, Input.mousePosition))
                {
                    mFatherUI.Btn_RemoveGem( mGemIndex);
                }
            }

        }

        if( mIsShow )
        {
            mImage.transform.position = Input.mousePosition;
        }
        
    }
}
