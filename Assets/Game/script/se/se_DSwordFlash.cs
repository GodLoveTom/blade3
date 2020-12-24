using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_DSwordFlash : MonoBehaviour
{
    List<CNpcInst> mDamgeArr = new List<CNpcInst>();
    public float mV = 15;
    float mDamage;
    Vector3 mBpos, mEpos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, mEpos, Time.deltaTime * mV);
        if (Vector3.Distance(pos, mEpos) < 0.01f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
        else
        {
            transform.position = pos;
            CNpcInst[] Arr = gDefine.gNpc.DoDamageShoot(mBpos.x, pos.x, (int)mDamage, mDamgeArr, CNpcInst.eNpcClass.OnGround,true);
            foreach (CNpcInst inst in Arr)
                mDamgeArr.Add(inst);
        }
    }

    public void Init(Vector3 Bpos, Vector3 Epos, float Damage)
    {
        mBpos = Bpos;
        mEpos = Epos;
        mDamage = Damage;

        transform.position = Bpos;

        transform.rotation = new Quaternion();
        if( Epos.x < Bpos.x)
            transform.Rotate(0, 180, 0, Space.World);
    }
}
