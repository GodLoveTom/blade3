using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_MainEquipTip : MonoBehaviour
{
    public Text mTipText;
    float mTipT;

    // Update is called once per frame
    void Update()
    {
        if (mTipT >= 0)
        {
            mTipT -= Time.deltaTime;
            if (mTipT < 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void Tip(string Str)
    {
        gameObject.SetActive(true);
        mTipText.text = Str;
        mTipT = 1.5f;

         Text [] textArr = gameObject.transform.GetComponentsInChildren<Text>(true);
        foreach(Text _t in textArr)
            gDefine.ResetFontBold(_t);
    }
}
