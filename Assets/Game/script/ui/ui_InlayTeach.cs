using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_InlayTeach : MonoBehaviour
{
    public GameObject mTipObj0;
    public Text mTipText0;

    public GameObject mTipObj1;
    public Text mTipText1;
    public GameObject mHandObj;
    public GameObject mDragHandObj;
    [Header("宝石合成参考点0")]
    public GameObject mRef0; 
    [Header("宝石合成参考点1")]
    public GameObject mRef1; 
    public GameObject mWeaponRef;
    public GameObject mGemTipRef;
    public GameObject mGemRef;
    public GameObject mSlotRef;
    public GameObject mQuitRef;
    public GameObject mDragObj;

    public enum eStep
    {
        Null,
        GemCom,
        Weapon,
        Drag,
        Quit,
    }

    eStep mStep = eStep.Null;
    public bool mIsOn = false;
    float mDragT = 1.8f;
    float mT = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mStep == eStep.Drag)
        {
            UpdateDrag();
        }
    }

    public void Btn_ClickToNext()
    {
        if(mStep == eStep.GemCom)
        {
            gDefine.gMainUI.mRefMainEquip.mRefInLayScript.Btn_GemLvLUp();
            mStep = eStep.Weapon;
            RefreshWeaponCom();
        }
        else if( mStep == eStep.Weapon)
        {
            gDefine.gMainUI.mRefMainEquip.mRefInLayScript.TeachSetWeapon();
            mStep = eStep.Drag;
            RefreshDrag();
        }
         else if( mStep == eStep.Quit)
        {
            gDefine.gMainUI.mRefMainEquip.mRefInLayScript.Btn_Close();
            gameObject.SetActive(false);
            mIsOn = false;
        }
    }

    public void Event_BDrag()
    {
        gDefine.gMainUI.mRefMainEquip.mRefInLayScript.TeachBeginDrag( );
    }

    public void DragEnd()
    {
        mStep = eStep.Quit;
        RefreshQuit();
    }

    public void Show()
    {
        mIsOn = true;
        mStep = eStep.GemCom;
        RefreshGemCom();
        gameObject.SetActive(true);
    }

    void UpdateDrag()
    {
        mT += Time.deltaTime;
        if(mT>=mDragT)
            mT = 0;
        float perc = mT / mDragT;

        Vector3 bpos = mGemRef.transform.position;
        Vector3 ePos = mSlotRef .transform.position;

        Vector3 pos = bpos + (ePos - bpos)* perc;
        mDragHandObj.transform.position = pos;
    }


    void RefreshGemCom()
    {
        mTipObj0.SetActive(true);
        mTipObj1 .SetActive(false);
        mDragHandObj.SetActive(false);
        mTipText0 .text = gDefine.GetStr(401);
        gDefine.SetTextBold();
        mHandObj.SetActive(true);

        mTipObj0.transform.position = mRef1.transform.position;
        mHandObj.transform.position = mRef0.transform.position;
    }

    void RefreshWeaponCom()
    {
        mTipObj0.SetActive(false);
        mTipObj1 .SetActive(false);
        mDragHandObj.SetActive(false);
 
        mHandObj.SetActive(true);
        mHandObj.transform.position = mWeaponRef.transform.position;
    }

    void RefreshQuit()
    {
        mTipObj0.SetActive(false);
        mTipObj1 .SetActive(false);
        mDragHandObj.SetActive(false);

        mHandObj.SetActive(true);
        mHandObj.transform.position = mQuitRef.transform.position;
    }

     void RefreshDrag()
    {
        mTipObj0.SetActive(false);
        mTipObj1 .SetActive(true);
        mDragHandObj.SetActive(true);
        mTipText1 .text = gDefine.GetStr(402); 
        gDefine.SetTextBold();
        mHandObj.SetActive(false);
        mDragObj.SetActive(true);

        mTipObj1.transform.position = mGemTipRef.transform.position;
        mDragHandObj .transform.position = mGemRef.transform.position;
        mT = 0;

        mDragObj.transform.position = mGemRef.transform.position;
    }




    
}
