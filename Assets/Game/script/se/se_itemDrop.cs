using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_itemDrop : MonoBehaviour
{
    [Header("生命周期")]
    public float mLiveT;
    float mT;
    [Header("初始向上速度")]
    public float mVy;

    [Header("重力，下落速度")]
    public float mDownAcc = -20;
    float mVx;
    public SpriteRenderer mR;
    public float mTimeScale = 1f;

    public bool mIsRot = false;
    public float mRotV = 360;

    [Header("使用地面高度，进行2次弹跳")]
    public bool mUseGroundY = false;

    [Header("金币，结束会被玩家吸收")]
    public bool mEndFlyToPc = false;

    public float mYOffset = 0;


    public void Init()
    {
        mLiveT = Random.Range(1.0f, 1.5f);
        mVy = Random.Range(4.0f, 8.0f) * 2.5f;
        mVx = Random.Range(-5.0f, 5.0f);
        mT = 0;
    }

    public void Init(float VX, float VY)
    {
        mLiveT = Random.Range(1.0f, 1.5f);
        mVy = VX;
        mVx = VY;
        mT = 0;
    }

    // Update is called once per frame
    void Update()
    {
        mT += Time.deltaTime*mTimeScale;
        if (mT > mLiveT)
        {
            if( !mEndFlyToPc )
            {
                GameObject.Destroy(gameObject);
                return;
            }
            else
            {
                FlyToPc();
            }
        }
        else
        {
            UpdateDrop();
        }
    }


    void FlyToPc()
    {
        Vector3 pcPos = gDefine.GetPcRefMid().position;
        Vector3 pos = Vector3.MoveTowards( transform.position, pcPos, 30*Time.deltaTime);
        transform.position = pos;
        if( Vector3.Distance(pos, pcPos)<0.01f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
           // Debug.Log("...Drop end...");
        }
    }

    void UpdateDrop()
    {
        if (mT > mLiveT * 0.5f)
        {
            float a = 1.0f - (mT - mLiveT * 0.5f) / mLiveT * 0.5f;
            mR.color = new Color(1, 1, 1, a);
        }

        mVy += mDownAcc * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.x += mVx * Time.deltaTime;
        pos.y += mVy * Time.deltaTime;
        if (mUseGroundY)
        {
            if (pos.y < gDefine.gGrounY)
            {
                pos.y = gDefine.gGrounY;
                mVy = Mathf.Abs(mVy) * 0.5f;
            }
        }

        transform.position = pos;

        if (mIsRot)
        {
            transform.Rotate(0, 0, -Time.deltaTime * mRotV, Space.World);
        }
    }
}
