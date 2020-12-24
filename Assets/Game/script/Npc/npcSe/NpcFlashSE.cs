using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcFlashSE : MonoBehaviour
{
    [Header("闪烁一次时间")]
    public float mFlashT=0.33333f;
    int mFlashNum = 0;
    float mT = 0;

    public void BeginFlash(int Num=1)
    {
        mFlashNum = Num;
        mT = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if( mFlashNum>0)
        {
            mT += Time.deltaTime;
            if(mT>=mFlashT)
            {
                SetColor(Color.black);
                mFlashNum--;
                mT = 0;
                return;
            }
            mT = mT%mFlashT;
            if(mT<mFlashT*0.5f)
            {
                float c = mT * 2 * 0.8f / mFlashT;
                 SetColor(new Color(c,c,c,1));
            }
            else
            {
                float c = (1.0f - (mT-mFlashT*0.5f)/ (mFlashT*0.5f))*0.8f;
                SetColor(new Color(c,c,c,1));
            }
        }
        
    }
    public void SetColor(Color C)
    {
        gameObject.GetComponent<Renderer>().material.SetColor("_Color0", C);
    }
}
