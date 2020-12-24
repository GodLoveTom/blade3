using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_MainUI : MonoBehaviour
{
    public ui_MainFight mRefMainFightUI;
    public ui_MainUp mRefMainUp;
    public ui_mainEquip mRefMainEquip;
    public ui_MainShop mRefMainShop;
    public UI_MainTalent mRefMainTalent;
    public ui_MainEndKill mRefMainEndKill;
    public ui_MainDown mRefMainDown;
    public ui_MainSkillAdd mRefMainSkillAdd;
    public ui_Option mRefMainOption;
    public ui_Box mRefMainBox;
    public ui_BuyTiLi mRefMainBuyTili;
    public ui_MainEquipTip mRefMainTip;
    public ui_MainGainTip mRefMainGainTip;
    public ui_LackTip mRefMainLackTip;
    public ui_appraise mRefAppraise;
    public ui_MainIap mRefMainIap;
    public ui_MainCombine mRefMainCombie;
    public GameObject mRefTipExPreb;

    float mDefaultScreenX = 1080.0f;
    //float mDefaultScreenY = 1920.0f;

    float mCurScreenX, mCurScreenY;


    // Start is called before the first frame update
    void Start()
    {
        ChangeToFight();
        gDefine.gMainUI = this;
        mCurScreenX = 0;
        //mCurScreenY = 0;
        ResetSize();
        gDefine.gMainGainTip = mRefMainGainTip;

    }

    public ui_BoxTip CreateTip()
    {
        GameObject o = GameObject.Instantiate(mRefTipExPreb);
        o.transform.SetParent( mRefTipExPreb.transform.parent);
        o.transform.localPosition = Vector3.zero;
        o.transform.SetAsLastSibling();
        return o.GetComponent<ui_BoxTip>();
    }

    // Update is called once per frame
    void Update()
    {
        ResetSize();
        mRefAppraise.Check();
    }

    public void ShowTip(string Str)
    {
        mRefMainTip.Tip(Str);
    }

    void ResetSize()
    {
        if (Screen.width != mCurScreenX || Screen.height != mCurScreenY)
        {
            float xScale = Screen.width / mDefaultScreenX;


            mRefMainUp.gameObject.transform.localScale = new Vector3(xScale, xScale, 1);
            mRefMainDown.gameObject.transform.localScale = new Vector3(xScale, xScale, 1);
            //mRefMainTalent.gameObject.transform.localScale = new Vector3(xScale,xScale,1);

            mCurScreenX = Screen.width;
            mCurScreenY = Screen.height;
        }
    }

    public void ChangeToEquipent()
    {

        CloseAll();
        mRefMainEquip.Open();

    }

    public void ChangeToOption()
    {

        int Where = CalcCurOpen();
        CloseAll();
        //mRefMainOption.gameObject.SetActive(true);
        //mRefMainOption.Refresh();
        mRefMainOption.Show(Where);
    }

    public void ChangeToFight()
    {
        gameObject.SetActive(true);
        CloseAll();
        mRefMainFightUI.gameObject.SetActive(true);
        mRefMainFightUI.Refresh();


    }

    public void ChangeToShop()
    {
        CloseAll();
        mRefMainShop.gameObject.SetActive(true);
        mRefMainShop.Refresh();
    }

    public void ChangeToTalent()
    {
        CloseAll();
        mRefMainTalent.gameObject.SetActive(true);
        mRefMainTalent.Refresh();
    }

    public void ChangeToEndKill()
    {
        CloseAll();
        mRefMainEndKill.gameObject.SetActive(true);
    }

    public void ChangeToKillAdd()
    {
        CloseAll();
        mRefMainSkillAdd.gameObject.SetActive(true);
        mRefMainSkillAdd.Refresh();
    }

    int CalcCurOpen()
    {
        if (mRefMainShop.gameObject.activeSelf)
            return 0;
        else if (mRefMainEquip.gameObject.activeSelf)
            return 1;
        else if (mRefMainFightUI.gameObject.activeSelf)
            return 2;
        else if (mRefMainTalent.gameObject.activeSelf)
            return 3;
        else if (mRefMainSkillAdd.gameObject.activeSelf)
            return 4;
        else
            return 2;

    }

    public void ChangeToBuyTiLi()
    {
        int curOpen = CalcCurOpen();
        CloseAll();
        mRefMainBuyTili.Show(curOpen);
        gDefine.PlayUIClickSound();
    }

    public void ChangeToBox()
    {
        CloseAll();
        mRefMainBox.gameObject.SetActive(true);
        //mRefMainBox.Refresh();
        mRefMainBox.Show();
    }

    public void ChangeTiIap()
    {
        CloseAll();
        mRefMainIap.Show();
        gDefine.PlayUIClickSound();

    }

    void CloseAll()
    {
        mRefMainFightUI.gameObject.SetActive(false);
        mRefMainEquip.gameObject.SetActive(false);
        mRefMainShop.gameObject.SetActive(false);
        mRefMainTalent.gameObject.SetActive(false);
        mRefMainEndKill.gameObject.SetActive(false);
        mRefMainSkillAdd.gameObject.SetActive(false);
        mRefMainOption.gameObject.SetActive(false);
        mRefMainBox.gameObject.SetActive(false);
        mRefMainBuyTili.gameObject.SetActive(false);
        mRefMainLackTip.gameObject.SetActive(false);
        mRefMainIap.gameObject.SetActive(false);
    }

    public void RefreshPCParam()
    {
        mRefMainUp.Refresh();
    }

    public void Btn_ContinueFight()
    {
        mRefMainFightUI.ContinueGame();

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

}
