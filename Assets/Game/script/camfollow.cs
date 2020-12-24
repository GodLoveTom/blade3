using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camfollow : MonoBehaviour
{
    int mLastNum = 8;
    int mCurNum = -1;
    //float mT = 0;
    //float mLastT = 0;
    float mOffset = 40;

   public bool mIsFollow = true;

    public bool mIsInStory = false;
    bool mIsInStoryMoveToNormalPos = false;

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gFollowCam = this;
        mOffset = 0.1f;
        mLastNum = 3;
        Vibration.Init ();
    }

    public void BeginStory(float X)
    {
        mIsInStory = true;

        Vector3 newPos = new Vector3(X, gDefine.gGrounY+5, transform.position.z);
        
        gDefine.CalcSceMove(newPos - transform.position);

        transform.position = newPos;
    }

    public void BeginStoryMoveToPlayer()
    {
        mIsInStoryMoveToNormalPos = true;
    }

    public void EndStory()
    {
        mIsInStory = false;
    }

    // Update is called once per frame
    void Update()
    {
        if( mIsInStory)
        {
            if(mIsInStoryMoveToNormalPos)
            {
                float x = gDefine.GetPCCamFollowPos().x;
                float y = gDefine.gGrounY;

                Vector3 aimPos = new Vector3(x, y+5, transform.position.z);
                Vector3 newPos = Vector3.MoveTowards(transform.position, aimPos, 3*Time.deltaTime);

                gDefine.CalcSceMove(newPos - transform.position);

                transform.position = newPos;
            }

        }
        else
        if(mIsFollow && (gDefine.gLogic.mIsBegin|| gDefine.gLogic.mTeach.mIsInTeach))
        {
            if (gDefine.GetPCTrans() != null )
            {
                float x = gDefine.GetPCCamFollowPos().x;
                float y = gDefine.gGrounY;
                if (mCurNum >= 0)
                    mCurNum++;


                if (mCurNum >= 0 && mCurNum < mLastNum)
                {
                    if (mCurNum % 2 == 0)
                        x += mOffset;
                    else
                        x -= mOffset;
                }
                else
                {
                    mCurNum = -1;
                }

                Vector3 aimPos = new Vector3(x, y+5, transform.position.z);
                Vector3 newPos = Vector3.MoveTowards(transform.position, aimPos, 12*Time.deltaTime);

                gDefine.CalcSceMove(newPos - transform.position);

                transform.position = newPos;
            }

        }

    } 

    public void PlayVibrate()
    {
        mCurNum = 0;
    }

    public void PauseFollow()
    {
        mIsFollow = false;
    }

    public void ContinueFollow()
    {
        mIsFollow = true;
    }

    public void InitPos()
    {
        float x = gDefine.GetPCCamFollowPos().x;
        float y = gDefine.gGrounY;
        Vector3 aimPos = new Vector3(x, y+5, transform.position.z);
        transform.position = aimPos;
    }

}