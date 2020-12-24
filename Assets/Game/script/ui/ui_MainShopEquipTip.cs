//武器，衣服，披风，枪使用
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainShopEquipTip : MonoBehaviour
{
    public Text mNameText;
    public Text mPinZhiText;
    public Text mValue;
    public Text mSpecial;

    public Text mConstSpecialTip;

    Transform mRefT;

    float mT = 0;
    Vector3 mBpos;

    void Update()
    {

        if (Time.time > mT || Vector3.Distance(mBpos, mRefT.position) > 50*gDefine.RecalcUIScale())
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.transform.position = mRefT.transform.position;

        if (gameObject.transform.position.x < 249)
            gameObject.transform.position = new Vector3(249, transform.position.y,
            transform.position.z);
        else if (transform.position.x > Screen.width - 249)
            transform.position = new Vector3(Screen.width - 249, transform.position.y,
                transform.position.z);

    }

    public void Show(Transform T, ShopData Item)
    {
        mConstSpecialTip.text = gDefine.GetStr(327);

        mBpos = T.position;

        mT = 1.6f + Time.time;
        CItem it = gDefine.gData.GetItemData(Item.mItemId);

        string str = gDefine.GetStr(it.mName);
        mNameText.text = str + "("+ gDefine.GetStr(273)  +")";
        str = gDefine.GetStr(it.mDes);

        mPinZhiText.text = it.GetPinZhiStr();
        mPinZhiText.color = it.GetPinZhiColor();

        string value0str = it.GetValueStrInShop(0);

        
            // CGird  gird = gDefine.gPlayerData.mEquipGird[(int)it.mEquipPos];
            // if(gird!=null && gird.mRefItem!=null )
            // {
            //     float delt = 0;
            //     if(it.mMainType == CItem.eMainType.MainWeapon)
            //         delt = it.mValue - gird.mDamage;
            //     else if(it.mMainType == CItem.eMainType.GunWeapon)
            //         delt = it.mValue - gird.mGunDamage;
            //     else if(it.mMainType == CItem.eMainType.Clothe)
            //         delt = it.mValue - gird.mHp;
            //     else if(it.mMainType == CItem.eMainType.Cloak)
            //         delt = it.mValue - gird.mHp;
            //     else if(it.mMainType == CItem.eMainType.Ring)
            //         delt = it.mValue - gird.mCrit;
            //     if(delt>0)
            //         value0str +=  "<color=#00ff00ff>("+ "↑+" + delt.ToString()  +")</color>" ;
            //     else if(delt<0)
            //         value0str +="<color=#ff0000ff>("+ "↓" + delt  +")</color>";
            // }
            // else if(gird==null || gird.mRefItem == null )
            // {
            //     value0str +="<color=#00ff00ff>("+ "↑+" + it.mValue.ToString()  +")</color>";
            // }
        
        mValue.text =  value0str;

       // mSpecial.text = it.GetSpeicalSkillStr();

        gameObject.SetActive(true);

        mRefT = T;
        Update();

    }
}
