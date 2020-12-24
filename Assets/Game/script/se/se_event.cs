using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_event : MonoBehaviour
{  
    float mLifeT = 0;
    public void End_CallBack()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }

    public void Event_PlaySound(int SoundId)
    {
        gDefine.PlaySound(SoundId);
    }

    public void Event_Hide()
    {
        gameObject.SetActive(false);
    }

    public void Update()
    {
        if(mLifeT>0)
        {
            mLifeT -= Time.deltaTime;
            if(mLifeT<=0)
            {
                 gameObject.SetActive(false);
                 GameObject.Destroy(gameObject);
            }
        }
    }


    public void InitLiftT(float LifeT)
    {
        mLifeT = LifeT;
    }
}
