using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_Rock : MonoBehaviour
{
    Vector3 mOriPos;
    float mRockX = 5;
    float mRockY = 5;
    bool mIsPlay = true;
    bool mIsRockX = false;
    bool mIsRockY = false;
    float mT = 0;
    int mState = 0; // 0 wait 1 rock
    const float mWaitT = 3f;
    const float mRockT = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        mOriPos = gameObject.transform.position;
        PlayRock();
    }

    // Update is called once per frame
    void Update()
    {
        if(mIsPlay)
        {
            if(mState==0)
            {
                transform.position = mOriPos;
                if(Time.time >= mT)
                {
                      mState = 1;
                      mT = Time.time + mRockT;
                }
                  
            }
            else
            {
                //rock
                if(Time.time > mT)
                {
                    mState = 0;
                    mT = Time.time + mWaitT;
                }
                else
                {
                    float perc = gDefine.RecalcUIScale();
                    Vector3 pos = mOriPos;
                    if(mIsRockX)
                        pos.x += Random.Range(-mRockX*perc, mRockX*perc);
                    if(mIsRockY)
                        pos.y += Random.Range(-mRockY*perc, mRockY*perc);
                    
                    transform.position = pos;
                }
            }
        }
    }

    public void PlayRock(bool RockX=true, bool RockY = true)
    {
        
        if(!mIsPlay)
            mOriPos = transform.position;

        mIsPlay = true;
        mIsRockX = RockX;
        mIsRockY = RockY;

    }

    public void Stop()
    {
        return;
        // if(mIsPlay)
        //     transform.position = mOriPos;
        // mIsPlay = false;
    }

}
