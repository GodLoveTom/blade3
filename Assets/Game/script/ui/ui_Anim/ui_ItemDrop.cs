using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ui_ItemDrop : MonoBehaviour
{
    float mLiveT;
    float mT;
    [Header("初始向上速度")]
    float mVy;

    [Header("重力，下落速度")]
    public float mDownAcc = -20;
    float mVx;
    public Image mImage;

    [Header("结束会被位置吸收")]
    public bool mEndFlyToPc = false;
    [Header("是金币")]
    public bool mIsCoin = true;
    

    public void Init()
    {
        mLiveT = Random.Range(0.8f, 1.4f);
        mVy = Random.Range(4.0f, 5.0f) * 2.5f*180;
        mVx = Random.Range(-5.0f, 5.0f)*100;
        mDownAcc = -4000;
        mT = 0;
    }

    // public void Init(float VX, float VY)
    // {
    //     mLiveT = Random.Range(1.0f, 1.5f);
    //     mVy = VX;
    //     mVx = VY;
    //     mT = 0;
    // }

    // Update is called once per frame
    void Update()
    {
        mT += Time.deltaTime;
        if (mT > mLiveT)
        {
            if( !mEndFlyToPc )
            {
                gameObject.SetActive(false);
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
        

        Vector3 pcPos = gDefine.gUICoinPos;
        if(!mIsCoin)
            pcPos = gDefine.gUICrystalPos;
        Vector3 pos = Vector3.MoveTowards( transform.position, pcPos, 2600f*Time.deltaTime);
        transform.position = pos;
        if( Vector3.Distance(pos, pcPos)<0.01f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }

    void UpdateDrop()
    {
        if (mT > mLiveT * 0.5f)
        {
            float a = 1.0f - (mT - mLiveT * 0.5f) / mLiveT * 0.5f;
            mImage.color = new Color(1, 1, 1, a);
        }

        mVy += mDownAcc * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.x += mVx * Time.deltaTime;
        pos.y += mVy * Time.deltaTime;

        transform.position = pos;
    }
}