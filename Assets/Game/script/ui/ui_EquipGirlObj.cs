using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ui_EquipGirlObj : MonoBehaviour
{
    public enum eAct
    {
        Idle,
        Atk0,
        Atk1,
        Atk2,
        Atk3,
        Count,
    }

    eAct mCurAct = eAct.Idle;
    public string [] mActName;

    public void Play(eAct Act)
    {
         Animator animator = GetComponent<Animator>();
         animator.Play(mActName[(int)Act],0,0);
         mCurAct = Act;
     }

    public void PlayNext()
    {
        if( mCurAct == eAct.Idle)
            Play(eAct.Atk0);
        // mCurAct=mCurAct+1;
        // if( mCurAct == eAct.Count)
        //     mCurAct = eAct.Idle;
        // Play(mCurAct);
    }

    void Event_Idle()
    {
        mCurAct = eAct.Idle;
    }

    

    

}
