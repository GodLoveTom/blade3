using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_Ice : MonoBehaviour
{
    enum eState
    {
        Fly,
        Frozen,
        broken,
    }

    eState mState = eState.Fly;
    bool mIsFaceRight = true;
    [Header("飞行速度")]
    public float mV;

    [Header("冻结时间")]
    public float mFronzeT = 3;
   
    int mDamage;
    float mT = 0;
    float mBeginx = 0;
    Animator  mAnimator;

    CNpcInst mNpc;

    // Update is called once per frame
    void Update()
    {
        if(mState == eState.Fly )
        {
            if(Mathf.Abs(mBeginx-transform.position.x) > 7.8f )
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
            }
            else
            {
                float x = transform.position.x + mV * Time.deltaTime;
                transform.position = new Vector3(x, transform.position.y,
                transform.position.z);

                //calcDamage
                CalcDamge(x);
            }
        }
        else if( mState == eState.Frozen)
        {
            if(Time.time > mT || !mNpc.IsLive() )
            {
                mAnimator.Play("broken");
                mState = eState.broken;
            }
        }
    }

    void Event_End()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    void CalcDamge(float X)
    {
        mNpc  = gDefine.gNpc.FindByLine(mBeginx, X);
        if(mNpc != null)
        {
            mAnimator.Play("hit");
            mNpc.Fronze(mFronzeT);
            mT = Time.time + mFronzeT;
            mState = eState.Frozen;
            transform.SetParent(mNpc.GetRefMid().transform);
            transform.localPosition = Vector3.zero;
        }
      
    }

    public void Init( Vector3 BPos, bool FaceRight, int Damage)
    {
        mIsFaceRight = FaceRight;
        
        mV = FaceRight ? mV:-mV;
        mDamage = Damage;
        mT = Time.time;
        mBeginx = gDefine.GetPCTrans().position.x;
        BPos.y = gDefine.GetPcRefMid().transform.position.y;
        transform.position = BPos;

        if(mIsFaceRight)
            transform.rotation = new Quaternion();
        else
            transform.rotation = new Quaternion(0,Mathf.Sin(Mathf.PI*0.5f),0, Mathf.Cos( Mathf.PI*0.5f));
        
        mState = eState.Fly;

        mAnimator = GetComponent<Animator>();
    }
}
