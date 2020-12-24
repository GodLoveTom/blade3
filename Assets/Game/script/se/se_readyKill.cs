using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_readyKill : MonoBehaviour
{
    float mDamage;

    public void Init(int Damage)
    {
        mDamage = Damage;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Atk()
    {
        gDefine.gNpc.DoAllDamge((int)mDamage,true);
    }
}
