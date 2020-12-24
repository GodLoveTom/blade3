using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Npc_Bomb : MonoBehaviour
{
    [Header("Npc炸弹 滚动速度")]
    public float mV = 20;
    [Header("Npc炸弹 爆炸特效")]
    public GameObject mRefBombPreb;
    [Header("Npc炸弹 爆炸范围")]
    public float mBombL;

    [Header("Npc炸弹 球的animator")]
    public Animator mBallAnim;
    GameObject mAlermFrame;
    float mAlermT = 0;
    Vector3 mDestPos;
    int mDamge;
    int mState = 0; //0 被扔出来， （到达最远处，或者遇到玩家跳转1） 1，2s的警告时间，2s后爆炸 



    // Update is called once per frame
    void Update()
    {
        if (mState == 0)
        {
            //calc move
            Vector3 pos = Vector3.MoveTowards(transform.position, mDestPos, Time.deltaTime * mV);
            transform.position = pos;
            if (Vector3.Distance(pos, mDestPos) < 0.01f)
            {
                mState = 1;
                mAlermT = Time.time + 1;
                mAlermFrame = GameObject.Instantiate(gDefine.gData.mLockSEPreb);
                pos.y = gDefine.gGrounY;
                mAlermFrame.transform.position = pos;
                mAlermFrame.SetActive(true);
                mBallAnim.Play("flash",0);
            }
            else if
                (
                Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 0.5f &&
               Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3
            )
            {
                mState = 1;
                mAlermT = Time.time + 1;
                mAlermFrame = GameObject.Instantiate(gDefine.gData.mLockSEPreb);
                pos = gDefine.GetPCTrans().position;
                pos.y = gDefine.gGrounY;
                mAlermFrame.transform.position = pos;
                mAlermFrame.SetActive(true);

                Vector3 thisPos = transform.position;
                thisPos.x = pos.x;

                transform.position = thisPos;

                 mBallAnim.Play("flash",0);

                //     GameObject o = GameObject.Instantiate(mRefBombPreb);
                //     pos = transform.position;
                //     pos.y = gDefine.gGrounY;
                //     o.transform.position = pos;

                //     if (Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < mBombL &&
                //   Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
                //     {
                //         gDefine.PcBeAtk(mDamge);
                //     }



                //..bomb..
                // gameObject.SetActive(false);
                // GameObject.Destroy(gameObject);

            }


        }
        else if (mState == 1)
        {
            if (Time.time > mAlermT)
            {
                GameObject o = GameObject.Instantiate(mRefBombPreb);
                Vector3 pos = transform.position;
                pos.y = gDefine.gGrounY;
                o.transform.position = pos;

                if (Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < mBombL &&
          Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
                {
                    gDefine.PcBeAtk(mDamge);
                }

                gDefine.PlayVibrate();

                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
                mAlermFrame.SetActive(false);
                GameObject.Destroy(mAlermFrame);

            }
        }

    }

    public void Init(Vector3 pos, Vector3 Dest, int Damage, float V = -1)
    {
        transform.position = pos;
        mDestPos = Dest;
        mDamge = Damage;
        mV = V > 0 ? V : mV;

        transform.rotation = new Quaternion();

        if (Dest.x < pos.x)
            transform.Rotate(0, 180, 0, Space.World);

        mState = 0;
    }
}
