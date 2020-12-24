using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipParam : MonoBehaviour
{
    const int EquipLen = 6;
    public Text [] mConstTipText = new Text[7];
    public Image[] mIcon6 = new Image[EquipLen];
    public Text[] mParamText6 = new Text[EquipLen];

    public Text[] mLvLText6 = new Text[EquipLen];

    public GameObject []mLvLUpTip6 = new GameObject[EquipLen];
    ui_mainEquip mRefMainEquip;
    public Camera mRefCam;
    public ui_EquipGirlObj mRefDGirl;
    public ui_EquipGirlObj mRefLunGirl;
    ui_EquipGirlObj mCurGirl;

    ui_ClickAnim [] mClickAnim = new ui_ClickAnim[6];
    float [] mDelayT = new float[6]{0,0,0,0,0,0};

    public void Btn_ChangeGirlAct()
    {
        if (mCurGirl == null)
            mCurGirl = mRefDGirl;
        mCurGirl.PlayNext();
    }

     void Update()
    {
        for(int i=0; i<mClickAnim.Length; i++)
            if(mClickAnim[i] != null)
                mClickAnim[i].Update();
        
        for(int i=0 ; i<6; i++)
            if(mDelayT[i]>0)
            {
                mDelayT[i]-=Time.deltaTime;
                if(mDelayT[i]<=0)
                    mRefMainEquip.ShowItemDes(gDefine.gPlayerData.mEquipGird[i], true);
            }
    }

    public void Refresh(ui_mainEquip MainEquip)
    {
        mRefMainEquip = MainEquip;
        //refresh gird
        for (int i = 0; i < EquipLen; i++)
        {
            if (gDefine.gPlayerData.mEquipGird[i].mRefItem != null)
            {
                mIcon6[i].gameObject.SetActive(true);
                mIcon6[i].sprite = gDefine.gPlayerData.mEquipGird[i].mRefItem.GetIconSprite();
                mLvLText6[i].text = gDefine.gPlayerData.mEquipGird[i].mLVL.ToString();
                if( gDefine.gPlayerData.mEquipGird[i].EquipCanLvLUp())
                    mLvLUpTip6[i].SetActive(true);
                else
                    mLvLUpTip6[i].SetActive(false);
            }
            else
            {
                mIcon6[i].gameObject.SetActive(false);
                mLvLText6[i].text ="";
                mLvLUpTip6[i].SetActive(false);
            }
                
        }

        //refresh param;
        int pet = 0;
        int damage = 0;
        float gundamage = 0;
        int hp = 0;
        float cricial = 0;
        float dodge = 0;

        for (int i = 0; i < gDefine.gPlayerData.mEquipGird.Length; i++)
        {
            if (gDefine.gPlayerData.mEquipGird[i].mRefItem != null)
            {
                gDefine.gPlayerData.mEquipGird[i].ReCalcValue();
                
                damage += gDefine.gPlayerData.mEquipGird[i].mDamage;
                gundamage += gDefine.gPlayerData.mEquipGird[i].mGunDamage;
                hp += gDefine.gPlayerData.mEquipGird[i].mHp;
                pet += gDefine.gPlayerData.mEquipGird[i].mPet;
                cricial += gDefine.gPlayerData.mEquipGird[i].mCrit;
                dodge += gDefine.gPlayerData.mEquipGird[i].mDodge;
            }
        }

        if (hp == 0)
            hp = 1;
        if (damage == 0)
            damage = 1;
        if (gundamage == 0)
            gundamage = 1;


        // int damage = gDefine.gPlayerData.mEquipGird[0].mRefItem!=null?
        //     gDefine.gPlayerData.mEquipGird[0].mRefItem.mDamage:1;
        // int gun = gDefine.gPlayerData.mEquipGird[1].mRefItem!=null?
        //     gDefine.gPlayerData.mEquipGird[1].mRefItem.mGunDamage:1;
        // int hp = gDefine.gPlayerData.mEquipGird[2].mRefItem!=null?
        //     gDefine.gPlayerData.mEquipGird[2].mRefItem.mHp:10;
        // hp += gDefine.gPlayerData.mEquipGird[3].mRefItem!=null?
        //     gDefine.gPlayerData.mEquipGird[3].mRefItem.mHp:0;
        // float circial = gDefine.gPlayerData.mEquipGird[4].mRefItem!=null?
        //     gDefine.gPlayerData.mEquipGird[4].mRefItem.mCriticalAtk:0;
        // float dodge = gDefine.gPlayerData.mEquipGird[4].mRefItem!=null?
        //     gDefine.gPlayerData.mEquipGird[4].mRefItem.mDodge:0;

        int fontSize = (int)gDefine.RecalcUIScale() * 40;
        mParamText6[0].fontSize = fontSize;
        mParamText6[2].fontSize = fontSize;

        mParamText6[0].text = gDefine.GetStr("伤害") + ": " + damage.ToString();
        mParamText6[1].text = "枪械:" + gundamage.ToString();
        mParamText6[2].text = gDefine.GetStr("生命值") + ": " + hp.ToString();
        mParamText6[3].text = "暴击:" + cricial.ToString() + "%";
        mParamText6[4].text = "闪避:" + dodge.ToString() + "%";
        mParamText6[5].text = "宠物:" + pet.ToString();

        RefreshPic();

        for(int i=0; i<7;i++)
            mConstTipText[i].text = gDefine.GetStr(368+i);

        for(int i=0; i<6; i++)
            mDelayT[i] = 0;

    }

    public void Btn_ShowItem(int Index)
    {
        if (gDefine.gPlayerData.mEquipGird[Index].mRefItem != null)
        {
            //mRefMainEquip.ShowItemDes(gDefine.gPlayerData.mEquipGird[Index], true);

            if(mClickAnim[Index]==null)
            mClickAnim[Index] = new ui_ClickAnim();
                mClickAnim[Index].Init( mIcon6[Index].gameObject, 1);

            mDelayT[Index] = 0.2f;

            gDefine.PlaySound(74);
        }

    }

    void RefreshPic()
    {
        if (mRefDGirl != null)
            mRefDGirl.gameObject.SetActive(false);

        if (gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon].mRefItem != null)
        {
            if (gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.MainWeapon].mRefItem.mWeaponClass
                == gDefine.eWeaponClass.DSword)
            {
                mRefDGirl.gameObject.SetActive(true);
                mRefLunGirl.gameObject.SetActive(false);
                mCurGirl = mRefDGirl;
            }
            else
            {
                mRefDGirl.gameObject.SetActive(false);
                mRefLunGirl.gameObject.SetActive(true);
                mCurGirl = mRefLunGirl;
            }
        }
        else
        {
            mRefDGirl.gameObject.SetActive(true);
            mRefLunGirl.gameObject.SetActive(false);
            mCurGirl = mRefDGirl;
        }
    }

    public void SetGirlDefaultAct()
    {
        mRefCam.gameObject.SetActive(true);
        mCurGirl.Play(ui_EquipGirlObj.eAct.Idle);
    }

}
