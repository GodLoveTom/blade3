using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet0CallBack : MonoBehaviour
{
    public Pet0 mRefPet;

    void Event_SkillCallBack()
    {
        mRefPet.Event_SkillAtk();
    }

    public void Event_SkillBegin()
    {
        mRefPet.mIsSkillOn = true;
    }

    public void Event_SkillEnd()
    {
        mRefPet.mIsSkillOn = false;
    }
}
