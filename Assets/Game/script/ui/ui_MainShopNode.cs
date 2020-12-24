using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainShopNode : MonoBehaviour
{
    public GameObject [] mRPoint;

    public GameObject [] mBtn;
    public Image[] mIcon = new Image[4];
    public Text[] mText = new Text[4];
    public Text [] mFreeText ;
    public GameObject [] mFlashObj;
    public GameObject [] mPlayIcon = new GameObject[4];
    public GameObject [] mCrystalIcon = new GameObject[4];

    public Text[]mNumText = new Text[4];
    ShopData[] mDataArr = new ShopData[4];
    float [] mDelayTArr = new float[4]{0f,0f,0f,0f};
    ui_MainShop mUIShop;

    public Material mMatGray;
    ui_ClickAnim [] mClickAnim = new ui_ClickAnim[4];

    public void Init(ui_MainShop UIShop, ShopData[] DataArr, int Index)
    {
        mUIShop = UIShop;
        mDataArr[0] = mDataArr[1] = mDataArr[2] = mDataArr[3] = null;
        for (int i = 0; i < 4; i++)
        {
            if (Index + i >= DataArr.Length)
                break;
            else
                mDataArr[i] = DataArr[Index + i];
        }
        Refresh();
        gameObject.SetActive(true);
    }

    public void Refresh()
    {
        //图标
        //金额
        for (int i = 0; i < mDataArr.Length; i++)
        {
            if (mDataArr[i] != null)
            {
                mIcon[i].gameObject.SetActive(true);

                CItem it = gDefine.gData.GetItemData(mDataArr[i].mItemId);

                 mIcon[i].sprite = it.GetIconSprite();

                if (mDataArr[i].mIsSold)
                {
                    string str = gDefine.gMyStr.Get("售完", gDefine.gPlayerData.mLanguageType);
                    mText[i].text = str;

                    for(int j=0; j<mBtn[i].transform.childCount; j++)
                    {
                        Image image = mBtn[i].transform.GetChild(j).gameObject.GetComponent<Image>();
                        if(image!=null)
                            image.material = mMatGray;
                    }

                    mIcon[i].GetComponent<Image>().material = mMatGray;


                    mNumText[i].color = new Color(0.6f,0.6f,0.6f);
                }
                else
                {
                    if(it.mComPieceId>0)
                    {
                        CItem comPiece = gDefine.gData.GetItemData(it.mComPieceId);
                         mText[i].text = comPiece.mPrice.ToString();
                    }
                    else
                        mText[i].text = it.mPrice.ToString();  //mDataArr[i].mMoney.ToString();
                    
                    for(int j=0; j<mBtn[i].transform.childCount; j++)
                    {
                        Image image = mBtn[i].transform.GetChild(j).gameObject.GetComponent<Image>();
                        if(image!=null)
                            image.material = null;
                    }

                    mIcon[i].GetComponent<Image>().material = null;

                    mNumText[i].color = new Color(0.95f,0.94f,0.38f);
                }
                   

                // if(it.mMainType == CItem.eMainType.Gem)
                //     mNumText[i].text = it.mMaxLvL.ToString();
                // else
                    mNumText[i].text = "";
                mFreeText[i].text = gDefine.GetStr(452);

                if( it.mMainType == CItem.eMainType.Box)
                {
                    mText[i].gameObject.SetActive(false);
                    mCrystalIcon[i].gameObject.SetActive(false);
                    mPlayIcon[i].gameObject.SetActive(true);

                    mFreeText[i].gameObject.SetActive(true);
                    mFlashObj[i].gameObject.SetActive(true);
                    
                }
                else
                {
                    mText[i].gameObject.SetActive(true);
                    mCrystalIcon[i].gameObject.SetActive(true);
                    mPlayIcon[i].gameObject.SetActive(false);
                     mFreeText[i].gameObject.SetActive(false);
                    mFlashObj[i].gameObject.SetActive(false);
                }


            }
            else
            {
                mIcon[i].gameObject.SetActive(false);

                mText[i].text = "";
            }
        }
    }

    public void Btn_Tip(int Index)
    {
        if(mClickAnim[Index]==null)
            mClickAnim[Index] = new ui_ClickAnim();
        mClickAnim[Index].Init( mIcon[Index].gameObject, 1);
        mUIShop.ShowTip( mRPoint[Index].transform, mDataArr[Index]);
         gDefine.PlaySound(74);

        // return;
        // if (mDataArr[Index] != null)
        // {
        //     if (mDataArr[Index].mIsSold)
        //     {
        //         string str = gDefine.gMyStr.Get("该商品已售完", gDefine.gPlayerData.mLanguageType);
        //         mUIShop.Tip(str);
        //     }
                
        //     else
        //         mUIShop.OpenConfirmCtl(mDataArr[Index]);
        // }
    }

    public void Btn_Buy(int Index)
    {
         if(mClickAnim[Index]==null)
            mClickAnim[Index] = new ui_ClickAnim();
        mClickAnim[Index].Init( mBtn[Index], 1);

        mDelayTArr[Index]=0.22f;

        //if(!mDataArr[Index].mIsSold)
          //   mUIShop.Btn_Buy( mDataArr[Index] );
    }

    void Update()
    {
        for(int i=0; i<mClickAnim.Length; i++)
        {
            if(mClickAnim[i] != null)
                mClickAnim[i].Update();
            if(mDelayTArr[i]>0)
            {
                mDelayTArr[i] -= Time.deltaTime;
                if(mDelayTArr[i]<=0)
                {
                     if(!mDataArr[i].mIsSold)
                           mUIShop.Btn_Buy( mDataArr[i] );
                }
            }

        }  
    }


}
