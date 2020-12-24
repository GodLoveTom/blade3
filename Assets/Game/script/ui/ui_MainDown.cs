using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ui_MainDown : MonoBehaviour
{
    public RectTransform mShop;
    public RectTransform mEquip;
    public RectTransform mFight;
    public RectTransform mTalent;
    public RectTransform mEndKill;

    public GameObject [] mNameObj = new GameObject[5];

    enum eSelect
    {
        Shop=0,
        Equip,
        Fight,
        Talent,
        EndKill,
        SkillAdd,
    }
    eSelect mCurSelect = eSelect.Fight;
    bool mInit = false;

    // Start is called before the first frame update
    void Start()
    {
        mCurSelect = eSelect.Fight;
        //Refresh();
    }

    public float CalcH()
    {
       return Screen.width * 327.0f / 1080.0f;
    }

    public void Btn_Fight()
    {
        mCurSelect = eSelect.Fight;
        gDefine.gMainUI.ChangeToFight();
        Refresh();
        gDefine.PlayUIClickSound();
    }

    public void Btn_Equip()
    {
        mCurSelect = eSelect.Equip;
        gDefine.gMainUI.ChangeToEquipent();
        Refresh();
        gDefine.PlayUIClickSound();
    }
    public void Btn_Shop()
    {
        mCurSelect = eSelect.Shop;
        gDefine.gMainUI.ChangeToShop();
        Refresh();
        gDefine.PlayUIClickSound();
    }
    public void Btn_Talent()
    {
        mCurSelect = eSelect.Talent;
        gDefine.gMainUI.ChangeToTalent();
        Refresh();
        gDefine.PlayUIClickSound();
    }
    public void Btn_EndKill()
    {
        mCurSelect = eSelect.EndKill;
        gDefine.gMainUI.ChangeToEndKill();
        Refresh();
        gDefine.PlayUIClickSound();
    }

    public void Btn_SkillAdd()
    {
         mCurSelect = eSelect.SkillAdd;
         gDefine.gMainUI.ChangeToKillAdd();
         Refresh();
         gDefine.PlayUIClickSound();
    }

    public void Refresh()
    {
        foreach(var v in mNameObj)
            v.SetActive(true);

        mShop.sizeDelta = new Vector2(180, 180);
        mShop.Find("Image").gameObject.SetActive(false);
        mEquip.sizeDelta = new Vector2(180, 180);
        mEquip.Find("Image").gameObject.SetActive(false);
        mFight.sizeDelta = new Vector2(180, 180);
        mFight.Find("Image").gameObject.SetActive(false);
        mTalent.sizeDelta = new Vector2(180, 180);
        mTalent.Find("Image").gameObject.SetActive(false);
        mEndKill.sizeDelta = new Vector2(180, 180);
        mEndKill.Find("Image").gameObject.SetActive(false);

        switch (mCurSelect)
        {
            case eSelect.Shop:
                mShop.sizeDelta = new Vector2(190, 190);
                mShop.GetChild(0).gameObject.SetActive(true);
                mNameObj[0].SetActive(true);
                break;

            case eSelect.Equip:
                mEquip.sizeDelta = new Vector2(190, 190);
                mEquip.GetChild(0).gameObject.SetActive(true);
                mNameObj[1].SetActive(true);
                break;

            case eSelect.Fight:
                mFight.sizeDelta = new Vector2(190, 190);
                mFight.GetChild(0).gameObject.SetActive(true);
                mNameObj[2].SetActive(true);
                break;

            case eSelect.Talent:
                mTalent.sizeDelta = new Vector2(190, 190);
                mTalent.GetChild(0).gameObject.SetActive(true);
                mNameObj[3].SetActive(true);
                break;

            case eSelect.EndKill:
                mEndKill.sizeDelta = new Vector2(190, 190);
                mEndKill.GetChild(0).gameObject.SetActive(true);
                mNameObj[4].SetActive(true);
                break;

            case eSelect.SkillAdd:
                mEndKill.sizeDelta = new Vector2(190, 190);
                mEndKill.GetChild(0).gameObject.SetActive(true);
                mNameObj[4].SetActive(true);
                break;
        }

        mNameObj[0].GetComponent<Text>().text = gDefine.gMyStr.Get("商店",gDefine.gPlayerData.mLanguageType);
     
         mNameObj[1].GetComponent<Text>().text = gDefine.gMyStr.Get("装备",gDefine.gPlayerData.mLanguageType);
  
          mNameObj[2].GetComponent<Text>().text = gDefine.gMyStr.Get("闯关",gDefine.gPlayerData.mLanguageType);
    
           mNameObj[3].GetComponent<Text>().text = gDefine.gMyStr.Get("升级",gDefine.gPlayerData.mLanguageType);
       
            mNameObj[4].GetComponent<Text>().text = gDefine.gMyStr.Get("技能",gDefine.gPlayerData.mLanguageType);
          

             Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }

    public void Update()
    {
        if(!mInit)
        {
            mInit = true;
            Refresh();
        }
    }
}
