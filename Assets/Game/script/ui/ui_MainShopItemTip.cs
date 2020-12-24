using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainShopItemTip : MonoBehaviour
{
    public Text mNameText;
    public Text mDesText;

    Transform mRefT;

    float mT = 0;

    Vector3 mBpos;
   

    // Update is called once per frame
    void Update()
    {
        if (Time.time > mT || Vector3.Distance(mBpos, mRefT.position) > 50*gDefine.RecalcUIScale())
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.transform.position = mRefT.transform.position;
        //gameObject.transform.localPosition = Vector3.zero;
        if( gameObject.transform.position.x < 249)
            gameObject.transform.position += Vector3.right * (249-gameObject.transform.position.x);
        else if (transform.position.x > Screen.width - 249)
            transform.position = new Vector3(Screen.width - 249, transform.position.y,
 transform.position.z);
        
    }

    public void Show( Transform T,  ShopData Item )
    {
        mBpos = T.position;
        mT = 1.6f + Time.time;
        CItem it = gDefine.gData.GetItemData(Item.mItemId);

        string str = it.GetNameLocal();
        mNameText.text = str;

        //
        if( it.mMainType == CItem.eMainType.Piece || it.mMainType == CItem.eMainType.Box)
        {
            str = it.GetDesLocal();    
        }
        else if(it.mMainType == CItem.eMainType.Gem)
        {
            //
            str += " LV." + it.mMaxLvL.ToString();
            mNameText.text = str;
            //
            str = it.GetValueStr(0);
        }
        mDesText.text = str;
       
        gameObject.SetActive(true);

        mRefT = T;
        Update();
        
        //mConfirmMoneyText.text = mRefShopData.mMoneyType == 0 ? "金币:" + mRefShopData.mMoney.ToString() :
        //   "钻石:" + mRefShopData.mMoney.ToString();
        //mConfirmMoneyText.text = "钻石:" + it.mPrice.ToString();



    }
}
