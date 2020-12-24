using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_Boxout : MonoBehaviour
{
    public Sprite [] mSEArr;
    public Image mSEImage;
    float mT=-1;
    int mIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if( gDefine.gBoxData.IsAnyBoxReady() )
        {
            mSEImage.gameObject.SetActive(true);
            if( mT < 0 ) 
            {
                mIndex = (mIndex+1)%mSEArr.Length;
                mT = 0.1f;
            }
            mT -= Time.deltaTime;
            mSEImage.sprite = mSEArr[mIndex];
        }
        else
        {
            mSEImage.gameObject.SetActive(false);
        }

        
    }
}
