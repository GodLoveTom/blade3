using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipInLayGemNode : MonoBehaviour
{
    const int mNodeCount = 5;
    public CGird [] mGirdArr = new CGird[mNodeCount];
    public Transform [] mGirdPos = new Transform[mNodeCount];
    
    public Image [] mImageArr = new Image[mNodeCount];
    public Text [] mNameArr = new Text[mNodeCount];
    public Text [] mNumArr = new Text[mNodeCount];
    ui_MainEquipInLay mRefRoot;

    //拖拽0.1s延迟
    float mDragDelayT = 0;
    CGird mRefGird;

    public GameObject [] mBtnArr = new GameObject[mNodeCount];

    public GameObject mTeachTipRef;


    public void Init(CGird [] GridArr,  ui_MainEquipInLay MainEquipInLay)
    {
        mRefRoot = MainEquipInLay;

        if( mRefRoot.mTeachGemRef == null )
        {
            mRefRoot.mTeachGemRef = mBtnArr[0].gameObject;
            mRefRoot.mTeachGemTipRef = mTeachTipRef;
            mRefRoot.mTeachGemGird = GridArr[0];
        }

        for(int i=0; i<mNodeCount; i++)
        {
            mGirdArr[i] = GridArr[i];

            if( GridArr[i] !=null)
            {
                mImageArr[i].gameObject.SetActive(true);
                mImageArr[i].sprite = GridArr[i].mRefItem.GetIconSprite();
                if(mGirdArr[i].mNum>1)
                    mNameArr[i].text = "x" + GridArr[i].mNum.ToString(); //GridArr[i].mRefItem.mName;
                    else
                    {
                        mNameArr[i].text="";
                    }
                mNumArr[i].text = GridArr[i].mRefItem.mMaxLvL.ToString();
                 mBtnArr[i].SetActive(true);
                uiDragEx script = mBtnArr[i].GetComponent<uiDragEx>();
                if(script!=null)
                    script.SetCallBackFunc(Btn_ShowDrag);
            }
            else
            {
                mImageArr[i].gameObject.SetActive(false);
                mNameArr[i].text = "";
                mNumArr[i].text = "";
                mBtnArr[i].SetActive(false);
            }
        }
    }
   
    public void Btn_Click(int Index)
    {
        if(mGirdArr[Index] != null)
            mRefRoot.ShowGemTip( mGirdPos[Index],    mGirdArr[Index] );
    }

    public void Btn_ShowDrag(int Index)
    {
        if(mRefRoot!=null)
            mRefRoot.ShowDrag( mGirdArr[Index] , false);
    }

    
}
