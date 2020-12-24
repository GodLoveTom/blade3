using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//飞轮截斩甩出去的飞轮

public class se_skill_lun_ReadyFly : MonoBehaviour
{
    bool mIsFaceRight = true;
    public float mV = 40;
    int mDamage;
    float mT = 0;
    float mBeginx = 0;

    List<CNpcInst> mDamgeArr = new List<CNpcInst>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mT += Time.deltaTime;
        if (mT > 3.0f)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
        else
        {
            Vector3 pos = transform.position;
            pos.x += mV * Time.deltaTime;
            transform.position = pos;

            //calcDamage
            CalcDamge(pos.x);
        }
    }

    void CalcDamge(float X)
    {
        CNpcInst[] Arr = gDefine.gNpc.DoDamageShoot(mBeginx, X, mDamage, mDamgeArr,CNpcInst.eNpcClass.OnGround,
        true);
        for (int i = 0; i < Arr.Length; i++)
        {
            mDamgeArr.Add(Arr[i]);
            
        }
    }


    public void Init(Vector3 BPos, bool FaceRight,  int Damage)
    {
        mIsFaceRight = FaceRight;

        mV = FaceRight ? mV : -mV;
        mDamage = Damage;
        mT = 0;
        mBeginx = gDefine.GetPCTrans().position.x;
        BPos.y += Random.Range(-0.3f, 0.3f);
        gameObject.transform.position = BPos;

        if (!FaceRight)
        {
            transform.rotation = new Quaternion();
            transform.Rotate(0, 180, 0, Space.World);
        }

        gDefine.PlaySound(43);
            
    }
}
