using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_NpcPosionTrap : MonoBehaviour
{
    public float mLiveT = 0;
    float mDamageT = 0;

    // Update is called once per frame
    void Update()
    {
        mLiveT -= Time.deltaTime;
        mDamageT -= Time.deltaTime;

        if (mLiveT <= 0)
        {
            GameObject.Destroy(gameObject);
            return;
        }

        if (mDamageT <= 0)
        {
            mDamageT = 1;

            if (
                   Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 1 &&
                    Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
            {


                int damage = (int)(gDefine.gPlayerData.mHpMax * 0.01f);
                if (damage <= 0) damage = 1;
                gDefine.PcAddBuff(CBuff.eBuff.Posion, 10, damage);

                gDefine.PcBeAtk(damage);

                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
            }
        }

    }

    public void Init(Vector3 Pos)
    {
        transform.position = Pos;
        mDamageT = 1;
    }
}
