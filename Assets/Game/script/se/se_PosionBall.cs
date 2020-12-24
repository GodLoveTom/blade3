using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_PosionBall : MonoBehaviour
{
    public float mV = 15;
    int mNextNum;
    int mDamage;
    float mAlramT = 0;
    public GameObject mTipObj;
    public GameObject mBombSE;


    // Update is called once per frame
    void Update()
    {
        mAlramT -= Time.deltaTime;
        if (mAlramT < 0)
        {
            mTipObj.SetActive(false);
            Vector3 pos = transform.position;
            pos.y -= Time.deltaTime * mV;
            if (pos.y <= gDefine.gGrounY)
            {
                //..bomb..
                GameObject bombse = GameObject.Instantiate(mBombSE);
                bombse.transform.position = pos;

                //do damage..
                if (
                    Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 1 &&
                     Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
                {
                    gDefine.PcBeAtk(mDamage);

                    int damage = (int)(gDefine.gPlayerData.mHpMax * 0.01f);
                    if (damage <= 0) damage = 1;
                    gDefine.PcAddBuff(CBuff.eBuff.Posion, 10, damage);


                }

                //create 陷阱
                {

                    GameObject trap = GameObject.Instantiate(gDefine.gData.mNpcPosionTrapPreb);
                    se_NpcPosionTrap script = trap.GetComponent<se_NpcPosionTrap>();
                    pos.y = gDefine.gGrounY;
                    script.Init(pos);
                }

                if (mNextNum > 0)
                {
                    GameObject o = GameObject.Instantiate(gDefine.gData.mPosionPurSuerPosionBallPreb);
                    se_PosionBall s = o.GetComponent<se_PosionBall>();
                    s.Init(mNextNum, mDamage);
                }

                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);


            }
            else
            {
                transform.position = pos;

                Vector3 tippos = pos;
                tippos.y = gDefine.gGrounY;
                mTipObj.transform.position = tippos;

            }

        }


    }

    public void Init(int Num, int Damage)
    {
        mNextNum = Num - 1;
        mDamage = Damage;
        Vector3 pos = gDefine.GetPCTrans().position;
        pos.y = gDefine.gGrounY + 30;
        transform.position = pos;
        mAlramT = 1.5f;

        Vector3 tippos = pos;
        tippos.y = gDefine.gGrounY;
        mTipObj.transform.position = tippos;
    }
}
