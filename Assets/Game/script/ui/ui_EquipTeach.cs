using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_EquipTeach : MonoBehaviour
{
    public GameObject mRef0;
    public GameObject mRef1;
   public Text mTipText;
   public GameObject mTip;
   public GameObject mHand;

   public void Show()
   {
       mTipText.text = gDefine.GetStr(400);
       gDefine.SetTextBold();
       gameObject.SetActive(true);
       mTip.transform.position = mRef0.transform.position;
       mHand.transform.position = mRef1.transform.position;
   }

    public void Btn_Click()
    {
        gameObject.SetActive(false);
        gDefine.gMainUI.mRefMainEquip.Btn_OPenXiangQian();

    }
}
