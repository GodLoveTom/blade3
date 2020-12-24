using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_AirFireBullet : MonoBehaviour
{
    Vector3 mDestPos;
    Vector3 mOriPos;
    public float mV;
    int mDamage;

    public GameObject mBombSE;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, mDestPos, Time.deltaTime * mV);
        if (pos.y <= gDefine.gGrounY)
        {
            pos.y = gDefine.gGrounY;
            GameObject.Destroy(gameObject);

            GameObject o = GameObject.Instantiate(mBombSE);
            o.transform.position = pos;
            return;
        }

        transform.position = pos;
        Vector3 dir = mDestPos - transform.position;
        dir.z = 0;
        transform.right = -dir; 

        if (Vector3.Distance(pos, mDestPos) < 0.01f)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        if (Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 1 &&
             Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 1)
        {
            gDefine.PcBeAtk(mDamage);

            GameObject.Destroy(gameObject);

            Vector3 pcPos = gDefine.GetPcRefMid().transform.position;

            GameObject o = GameObject.Instantiate(mBombSE);
            o.transform.position = pcPos;
        }
    }

    public void Init(Vector3 BPos, Vector3 EPos, int Damage)
    {
        mOriPos = BPos;
        mDestPos = EPos;
        mDamage = Damage;

        transform.position = BPos;
    }
}
