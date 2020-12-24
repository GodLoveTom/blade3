using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_PosionDagger : MonoBehaviour
{
    [Header("Npc毒镖 飞行速度")]
    public float mV = 20;
    Vector3 mDestPos;
    int mDamge;

    // Update is called once per frame
    void Update()
    {
        //calc move
        Vector3 pos = Vector3.MoveTowards(transform.position, mDestPos, Time.deltaTime * mV);
        if (Vector3.Distance(pos, mDestPos) < 0.01f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
            return;
        }
        transform.position = pos;

        // calc damage.//
        if (
            Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 1 &&
           Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
        {
            gDefine.PcBeAtk(mDamge);
            int Damage = (int)(gDefine.gPlayerData.mHpMax*0.01f);
            if( Damage < 1 ) Damage = 1;
            gDefine.PcAddBuff(CBuff.eBuff.Posion, 10,  Damage);
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

    }

    public void Init(Vector3 pos, Vector3 Dest, int Damage, float V = -1)
    {
        transform.position = pos;
        mDestPos = Dest;
        mDamge = Damage;
        mV = V > 0 ? V : mV;

        transform.rotation = new Quaternion();

        if (Dest.x > pos.x)
            transform.Rotate(0, 180, 0, Space.World);
    }
}
