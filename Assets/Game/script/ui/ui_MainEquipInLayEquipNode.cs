using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipInLayEquipNode : MonoBehaviour
{
    const int mNodeCount = 5;
    public CGird [] mGirdArr = new CGird[mNodeCount];
    bool [] mIsEquiped = new bool[mNodeCount];
    
    public Image [] mImageArr = new Image[mNodeCount];
    public Image [] mFrameArr = new Image[mNodeCount];
    public Text [] mNameArr = new Text[mNodeCount];
    public Text [] mEquipedArr = new Text[mNodeCount];
    public Text [] mLvLArr = new Text[mNodeCount];
    ui_MainEquipInLay mRefRoot;

    public void Init(CGird [] GridArr, bool [] IsEquipedArr, ui_MainEquipInLay MainEquipInLay)
    {
        mRefRoot = MainEquipInLay;

        if(mRefRoot.mTeachWeaponGird == null)
        {
           mRefRoot. mTeachWeaponGird = GridArr[0];
           mRefRoot.mTeachWeaponRef = mImageArr[0].gameObject;
        }    

        for(int i=0; i<mNodeCount; i++)
        {
            mGirdArr[i] = GridArr[i];
            mIsEquiped[i] = IsEquipedArr[i];

            if(mGirdArr[i] == MainEquipInLay.mEquipGird)
                mFrameArr[i].gameObject.SetActive(true);
            else
            {
                mFrameArr[i].gameObject.SetActive(false);
            }

            if( GridArr[i] !=null)
            {
                mImageArr[i].gameObject.SetActive(true);
                mImageArr[i].sprite = GridArr[i].mRefItem.GetIconSprite();
                //mNameArr[i].text = GridArr[i].mRefItem.mName;
                mLvLArr[i] .text= GridArr[i].mLVL.ToString();
            }
            else
            {
                mImageArr[i].gameObject.SetActive(false);
                mNameArr[i].text = "";
                 mLvLArr[i] .text= "";
            }

            if(IsEquipedArr[i])
            {
                mEquipedArr[i].gameObject.SetActive(true);
            }
            else
            {
                mEquipedArr[i].gameObject.SetActive(false);
            }
        }
    }
   
    public void Btn_Click(int Index)
    {
        if(mGirdArr[Index] != null)
            mRefRoot.SetInLayItem( mGirdArr[Index], mIsEquiped[Index] );
    }

}
