using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_StoryTalk : MonoBehaviour
{
    List<GameObject> mDict = new List<GameObject>();
    public delegate void CallBackFunc();

    public GameObject mLeftPreb;
    public GameObject mRightPreb;
    public GameObject mGhostPreb;

    public RectTransform mRoot;

    CallBackFunc mCallBackFunc;


    public void AddTalkR(int StrId, CallBackFunc Func)
    {
        Clear();
        mCallBackFunc = Func;
        GameObject o = GameObject.Instantiate(mRightPreb);
        Text text = o.GetComponentInChildren<Text>();
        text.text = gDefine.GetStr(StrId);

        o.transform.SetParent(mRoot);
        o.transform.localPosition = Vector3.zero;
        o.SetActive(true);
        
        gameObject.SetActive(true);

        mDict.Add(o);
    }

    public void AddTalkL(int StrId, CallBackFunc Func)
    {
        Clear();
        mCallBackFunc = Func;
        GameObject o = GameObject.Instantiate(mLeftPreb);
        Text text = o.GetComponentInChildren<Text>();
        text.text = gDefine.GetStr(StrId);

        o.transform.SetParent(mRoot);
        o.transform.localPosition = Vector3.zero;
          o.SetActive(true);

          gameObject.SetActive(true);
          mDict.Add(o);
    }

    public void AddTalkGhost(int StrId, CallBackFunc Func)
    {
        Clear();
        mCallBackFunc = Func;
        GameObject o = GameObject.Instantiate(mGhostPreb);
        Text text = o.GetComponentInChildren<Text>();
        text.text = gDefine.GetStr(StrId);

        o.transform.SetParent(mRoot);
        o.transform.localPosition = Vector3.zero;
          o.SetActive(true);

          gameObject.SetActive(true);
          mDict.Add(o);
    }

    void Clear(int MaxNum=1)
    {
        while(mDict.Count>MaxNum)
        {
            GameObject.Destroy(mDict[0]);
            mDict.RemoveAt(0);
        }
    }

    public void ClearAll()
    {
        while(mDict.Count>0)
        {
            GameObject.Destroy(mDict[0]);
            mDict.RemoveAt(0);
        }
    }




    public void Event_CallBack()
    {
        if(mCallBackFunc!=null)
            mCallBackFunc();
    }

    public void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(mCallBackFunc!=null)
                mCallBackFunc();
            //mCallBackFunc = null;
        }
    }
}
