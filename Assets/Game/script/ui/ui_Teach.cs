using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ui_Teach : MonoBehaviour
{
    public Text mTipText;
    public Text mTipText0;
    public GameObject mLeftTip;
    public GameObject mRightTip;
    public GameObject mTip;
    public GameObject mTip0;
    public GameObject mUpTip;

    public void Refresh()
    {

    }

    public void Btn_Click()
    {
        gameObject.SetActive(false);
        gDefine.gLogic.mTeach.PlayerClick_AndNext();
    }

    public void ShowNullBtn()
    {
        gameObject.SetActive(true);
         mLeftTip.gameObject.SetActive(false);
        mRightTip.gameObject.SetActive(false);
         mTip.SetActive(false);

         mUpTip.SetActive(false);
         mTip0.SetActive(false);
    }

    public void ShowRightBtn()
    {
        gameObject.SetActive(true);
        mLeftTip.gameObject.SetActive(false);
        mRightTip.gameObject.SetActive(true);
         mTip.SetActive(false);

           mUpTip.SetActive(false);
         mTip0.SetActive(false);
    }

    public void ShowLeftBtn()
    {
        mTipText .text = gDefine.GetStr(399);
        mTip.SetActive(true);
        gameObject.SetActive(true);
        mLeftTip.gameObject.SetActive(true);
        mRightTip.gameObject.SetActive(false);

          mUpTip.SetActive(false);
         mTip0.SetActive(false);
    }

     public void ShowUpBtn()
    {
       // mTipText .text = gDefine.GetStr(399);
        mTip.SetActive(false);
        gameObject.SetActive(true);
        mLeftTip.gameObject.SetActive(false);
        mRightTip.gameObject.SetActive(false);

        mUpTip.SetActive(true);
        mTip0.SetActive(true);
        mTipText0 .text = gDefine.GetStr(406);
    }
}
