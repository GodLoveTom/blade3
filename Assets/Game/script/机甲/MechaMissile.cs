using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechaMissile : MonoBehaviour
{
    [Header("命中特效")]
    public GameObject mHitSEPreb; // 
    [Header("飞行速度")]
    public float mV;
    [Header("飞出最高距离")]
    public float mL;

    public Renderer [] mRenderArr;
    CNpcInst mAim;
    Vector3 mAimPos;


    public enum  eState
    {
        Null,
        FlyUp=0, //上升阶段
        Clockwise, //顺时针
        AntiClockwise, //逆时针
        FlyToAim, 
    }

    eState mState = eState.Null;
    Vector3 mCircleCenPos;
    float mRadin;
    float mR;
    Vector3 mOriPos;
    float mDelayT;

    // Update is called once per frame
    void Update()
    {
        if(Time.time > mDelayT)
        {
           mRenderArr[0].sortingOrder = 4;
           mRenderArr[1].sortingOrder = 5;
        }

        if(mState == eState.FlyUp)
        {
            float y = transform.position.y + mV * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            if( y >= mL + gDefine.gGrounY)
            {
                mR = Random.Range(3f,5f);
                mOriPos = transform.position;
                int r = Random.Range(0,100);
                //if(mAimPos.x > transform.position.x)
                if(r<50)
                {
                    mState = eState.Clockwise;
                    mCircleCenPos = transform.position + new Vector3(mR,0,0);
                    mRadin = Mathf.PI;
                }
                else
                {
                    mState = eState.AntiClockwise;
                    mCircleCenPos = transform.position - new Vector3(mR,0,0);
                    mRadin = 0f;
                }
            }
        }
        else if( mState == eState.Clockwise)
        {
            float l = mV * Time.deltaTime; // 飞行的距离
            float radin =   l /  mR;
            mRadin -= radin;
            float dx = mR * Mathf.Cos(mRadin);
            float dy = mR * Mathf.Sin(mRadin);
            if( mRadin <= 0)
            {
                mState = eState.FlyToAim;
                CheckAim();
            }
            transform.position = mOriPos + new Vector3(mR+dx, dy, 0);
            transform.right = new Vector3(-dx,-dy,0);
            
        }
         else if( mState == eState.AntiClockwise)
        {
            float l = mV * Time.deltaTime; // 飞行的距离
            float radin =   l /  mR;
            mRadin += radin;
            float dx = mR * Mathf.Cos(mRadin);
            float dy = mR * Mathf.Sin(mRadin);
            if( mRadin >= Mathf.PI)
            {
                mState = eState.FlyToAim;
                CheckAim();
            }
            transform.position = mOriPos + new Vector3(-mR+dx, dy, 0);
            transform.right = new Vector3(dx,dy,0);
        }
        else if(mState == eState.FlyToAim)
        {
            RecalcAimPos();
            Vector3 pos = Vector3.MoveTowards( transform.position, mAimPos, mV * Time.deltaTime);
            transform.position = pos;
            Vector3 dir = mAimPos - pos;
            transform.up = dir;
            if( Vector3.Distance(pos, mAimPos) <= 0.1f )
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
                //bobm.
                GameObject hit = GameObject.Instantiate(mHitSEPreb);
                hit.transform.position = mAimPos;
                hit.transform.localScale = Vector3.one * 2;
                 
                if(mAim != null)
                    mAim.BeDamage((int)(gDefine.gPlayerData.mDamage*2), false,false,false);
                
            }
        }
    }

    void CheckAim()
    {
        if(! (mAim != null  && mAim.IsLive() && mAim.CanBeSelect() ) )
        {
            CNpcInst [] arr = gDefine.gNpc.FindAllWithR(transform.position.x, 9, CNpcInst.eNpcClass.All);
            if(arr.Length>0)
                mAim = arr[ Random.Range(0,arr.Length)];
        }

        if(mAim!=null)
            mAimPos = mAim.GetHitSEPos();
        else
        {
            mAimPos.x =   Random.Range(-8f, 8f) + transform.position.x;
            mAimPos.y =  gDefine.gGrounY;
            mAimPos.z = transform.position.z;
        }

    }


    void RecalcAimPos()
    {
        if(mAim!=null && mAim.IsLive() && mAim.CanBeSelect())
            mAimPos = mAim.GetHitSEPos();
    }

    public void Init(CNpcInst Aim)
    {
        mAim = Aim;
        mState = eState.FlyUp;
        mDelayT = Time.time +0.5f;

        
    }
}
