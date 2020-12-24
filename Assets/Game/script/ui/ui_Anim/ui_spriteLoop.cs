using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_spriteLoop : MonoBehaviour
{
    public Sprite [] mSpriteArr;
    public Image mRefImage;
    public float mLoopT=0.1f;
    int Index = -1;
    float mT = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Time.time > mT)
        {
            Index = (++Index)%mSpriteArr.Length;
            mRefImage.sprite = mSpriteArr[Index]; 
            mT = Time.time + mLoopT;
        }
    }
}
