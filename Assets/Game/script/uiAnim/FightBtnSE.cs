using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightBtnSE : MonoBehaviour
{
    public GameObject mLeftArrTex;
    public GameObject mRightArrTex;

    public float mLT;
    public float mRT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Btn_LeftDown()
    {
        gDefine.gGameMainUI.BtnLeft_Up();
        mLT = 0.1f;

    }

    public void Btn_RightDown()
    {
        gDefine.gGameMainUI.BtnRight_Up();
        mRT = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        mLT -= Time.deltaTime;
        mRT -= Time.deltaTime;

        if (mLT > 0)
            mLeftArrTex.transform.localScale = new Vector3(1.1f, 1.1f, 1);
           // mLeftArrTex.transform.localScale = new Vector3(0.9f, 0.9f, 1);
        else
           mLeftArrTex.transform.localScale = new Vector3(1.0f, 1.0f, 1);

        if (mRT > 0)
            mRightArrTex.transform.localScale = new Vector3(1.1f, 1.1f, 1);
        else
            mRightArrTex.transform.localScale = new Vector3(1.0f, 1.0f, 1);
    }
}
