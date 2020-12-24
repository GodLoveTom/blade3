using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipNode : MonoBehaviour
{
    const int mArrL = 5;
    public Image[] mImage= new Image[mArrL];
    public Text [] mName= new Text[mArrL]; 
    public Text [] mLVL = new Text[mArrL];

    public GameObject [] mUpArrow; //装备升级箭头
    public Text [] mCombinText; //碎片合成进度提示
    public GameObject [] mCombinProgressBar; //碎片进度条
    public GameObject [] mCombinTipObj; //可以合成提示
    public GameObject [] mCombinRootObj; //碎片合成


    CGird[] mGridArr = new CGird[mArrL];
    public GameObject [] mBtn = new GameObject[mArrL];

    ui_ClickAnim [] mClickAnim = new ui_ClickAnim[6];
    float [] mDelayT = new float[5]{0,0,0,0,0};

    ui_mainEquip mRefMainEquip ;

    private void Start() {
        // for(int i=0; i<mBtn.Length; i++)
        // {
        //     Button btn = mBtn[i].GetComponent<Button>();
        //     btn.onClick.RemoveAllListeners();
        //     btn.onClick.AddListener(()=>
        //     {
        //         int index = transform.GetSiblingIndex();
        //         Btn_Click(index-1);
        //     });
        // }
    }
    
    public void Init(CGird [] GirdArr, ui_mainEquip MainEquip)
    {
        mRefMainEquip = MainEquip;

        for(int i=0; i<mArrL; i++)
            mGridArr[i] = GirdArr[i];

        for(int i=0; i<mArrL; i++)
        {
            if(mGridArr[i] == null)
            {
                mName[i].text = "";
                mImage[i].gameObject.SetActive(false); 
                mLVL[i].text ="";
                mBtn[i].SetActive(false);
                if( mCombinRootObj.Length>0)
                    mCombinRootObj[i].SetActive(false);
                
            }
            else
            {
                mName[i].text = mGridArr[i].mRefItem.mName;
                mImage[i].gameObject.SetActive(true); 
                mImage[i].sprite = mGridArr[i].mRefItem.GetIconSprite();
                mLVL[i].text = mGridArr[i].mLVL.ToString();
                mBtn[i].SetActive(true);
                
                if( mGridArr[i].mRefItem.mMainType == CItem.eMainType.ComPiece)
                {
                     mCombinRootObj[i].SetActive(true);

                    //是否数量够合成
                    if( mGridArr[i].CanCombin())
                    {
                        mCombinTipObj[i].SetActive(true);
                    }
                    else
                    {
                        mCombinTipObj[i].SetActive(false);
                    }

                    //进度条，以及进度数字
                    int comNeedNum = mGridArr[i].GetCombinPieceNum();
                    float perc = Mathf.Clamp01( (float) mGridArr[i].mNum/ comNeedNum );

                    mCombinProgressBar[i].transform.localScale= new Vector3( perc,1,1);
                    if( mGridArr[i].mNum >= comNeedNum)
                        mCombinText[i].text =   mGridArr[i].mNum .ToString() + "/"  + comNeedNum.ToString();
                    else
                        mCombinText[i].text = "<color=red>"+  mGridArr[i].mNum .ToString()+ "</color>/"  +comNeedNum.ToString();

                }
                else
                {
                    if( mGridArr[i].EquipCanLvLUp())
                        mUpArrow[i].SetActive(true);
                    else
                        mUpArrow[i].SetActive(false);
                }


            }
        }
    }

    

    public void Btn_Click(int Index)
    {
        if(mGridArr[Index] !=null)
        {
             if(mClickAnim[Index]==null)
            mClickAnim[Index] = new ui_ClickAnim();
                mClickAnim[Index].Init( mImage[Index].gameObject, 1);

                  mDelayT[Index] = 0.2f;
              //mRefMainEquip.ShowItemDes( mGridArr[Index] ,false);
        }
          
    }

     void Update()
    {
        for(int i=0; i<mClickAnim.Length; i++)
            if(mClickAnim[i] != null)
                mClickAnim[i].Update();
        
        for(int i=0 ; i<5; i++)
            if(mDelayT[i]>0)
            {
                mDelayT[i]-=Time.deltaTime;
                if(mDelayT[i]<=0)
                {
                    if(mGridArr[i].mRefItem.mMainType == CItem.eMainType.ComPiece)
                        mRefMainEquip.ShowPiecePage( mGridArr[i]);
                    else
                        mRefMainEquip.ShowItemDes( mGridArr[i] ,false);
                }
                    
            }



    }



}
