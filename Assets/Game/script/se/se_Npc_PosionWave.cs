using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Npc_PosionWave : MonoBehaviour
{
    public GameObject mSEBombPreb;
    Vector3 mDest;
    public float mV = 15;

    int mDamage;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, mDest, Time.deltaTime * mV);

        if (
            Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 1 &&
            Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
        {
            //int damage = (int)(gDefine.gPlayerData.mHpMax * 0.1f);

            gDefine.PcBeAtk(mDamage);

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);

            //create se..
            GameObject se = GameObject.Instantiate(mSEBombPreb);
            Vector3 sePos = gDefine.GetPCTrans().position;
            sePos.y = gDefine.gGrounY;
            se.transform.position = sePos;
        }
        else if (Vector3.Distance(pos, mDest) < 0.01f)
        {


            GameObject se = GameObject.Instantiate(mSEBombPreb);
            Vector3 sePos = mDest;
            sePos.y = gDefine.gGrounY;
            se.transform.position = sePos;

            if (
           Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) < 2 &&
           Mathf.Abs(gDefine.GetPCTrans().position.y - transform.position.y) < 3)
            {
                gDefine.PcBeAtk(mDamage);
            }

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);

        }
        else
        {
            transform.position = pos;
        }
    }

    public void Init(Vector3 Pos, Vector3 Dest, int Damage)
    {
        mDest = Dest;
        transform.position = Pos;
        mDamage = Damage;

        transform.rotation = new Quaternion();
        if (Dest.x < Pos.x)
            transform.Rotate(0, 180, 0, Space.World);
    }
}
