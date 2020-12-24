using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 脚本挂载到每个可拖拽的Item上面即可
/// </summary>
public class uiDragEx : MonoBehaviour, IPointerDownHandler, 
IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ScrollRect mScrollRect;
    // 是否按下
    bool mIsDown = false;
    // 按下与松开鼠标之间的距离
    public float mBorderDis = 0.5f;
    // 总的按下时间
    public float mTotalTime = 1;
    private float mCurTime = 0;
    // 当前鼠标位置
    Vector3 mCurPos;
    // 上一次鼠标位置
    Vector3 mPrevPos;

    bool mOnDrag = false;

    public delegate void BeginDragFunc(int Param);
    BeginDragFunc mFunc;
    public int mParam=-1;

    public void SetCallBackFunc(BeginDragFunc Func)
    {
        mFunc = Func;
    }

    void Update()
    {
        if (mIsDown&&!mOnDrag)
        {
            mCurTime += Time.deltaTime * 1;
            if (mCurTime >= mTotalTime)
            {
                if (Vector3.Distance(mPrevPos, mCurPos) > mBorderDis)
                {
                    mCurTime = 0f;
                    return;
                }
                mCurTime = 0f;
                mIsDown = false;

                if(mFunc!=null)
                    mFunc(mParam);

                mOnDrag = true;
            }
        }
    }
 
    public void OnPointerDown(PointerEventData eventData)
    {
        mPrevPos = Input.mousePosition;
        mCurPos = Input.mousePosition;
        mIsDown = true;
        mCurTime = 0;
    }
 
    public void OnPointerUp(PointerEventData eventData)
    {
        mPrevPos = Vector3.zero;
        mCurPos = Vector3.zero;
        mIsDown = false;
        mCurTime = 0; 
    }
 
    public void OnBeginDrag(PointerEventData eventData)
    {
        mScrollRect.OnBeginDrag(eventData);
    }
 
    public void OnDrag(PointerEventData eventData)
    {
        mCurPos = eventData.position;
        if (!mOnDrag)
            mScrollRect.OnDrag(eventData);
    }
 
    public void OnEndDrag(PointerEventData eventData)
    {
        mOnDrag = false;
        mCurTime = 0;
        mScrollRect.OnEndDrag(eventData);
    }
 
 
}
 