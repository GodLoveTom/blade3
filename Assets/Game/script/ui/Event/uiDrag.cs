using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class uiDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public int mValue;
    public delegate  void CallBackFunc(int Param);

    CallBackFunc mFunc;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCallBackFunc(CallBackFunc Func)
    {
        mFunc = Func;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
       // Debug.Log("OnBeginDrag");
       if(mFunc!=null)
        mFunc(mValue);
    }
    public void OnDrag(PointerEventData eventData)
    {
       
    }
}
