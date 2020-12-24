using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_openStory : MonoBehaviour
{
    public Text mText;
    bool mIsPlay = false;
    public float mV= 15;
    public bool mIsPlayEnd = false;

    public RectTransform mRefBeginTrans;
    public RectTransform mTextTrans;
    // Start is called before the first frame update
    void Start()
    {
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        RectTransform rt = mTextTrans;
        if(mIsPlay)
        {
             rt.Translate(0, Time.deltaTime*mV,0,Space.World);
             if(rt.position.y > mRefBeginTrans.position.y + rt.sizeDelta.y + mRefBeginTrans.sizeDelta.y * 0.5f)
             {
                 mIsPlay = false;
                 mIsPlayEnd = true;
             }
        }
    }

    public void Play()
    {
        mIsPlay = true;
        Vector3 bpos = mRefBeginTrans.position;
        bpos.y -= mRefBeginTrans.sizeDelta.y*0.5f;
        mTextTrans.transform.position = bpos;
        mIsPlayEnd = false;

        mText.text = gDefine.GetStr(449);
    }
}
