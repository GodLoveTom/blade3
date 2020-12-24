/// <summary>
/// 终结技
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char_SkillEndKill : MonoBehaviour
{
    enum eType
    {
        Half,   //半身
        Total, //全身
    }
    enum eState
    {
        MoveToTarget,
        PlayKill,
    }
    CNpcInst mTarget;
    float mBeginT=0 ; //发动的时间间隔，1s
    eType mType;
    eState mState;

    bool BeginSkill(Transform T, Animator Anim)
    {
        // if(Time.time >= mBeginT + 1  )
        // {
        //     mBeginT = Time.time;
        //     if( BeginHalfKill(T,Anim) )
        //         return true;
        //     else if( BeginTotalKill())
        //         return true;
        //     else
        //     {
        //         return false;
        //     }
        // }

        return false;
    }

    // Update is called once per frame
    // void Update( Transform T, Animator Anim)
    // {
    //    // if(e)
        
    // }
}
