using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_skill_Sword_Flush : MonoBehaviour
{
    bool mIsFaceRight = true;
    float mV;
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
            float x = transform.position.x + mV * Time.deltaTime;
            transform.position = new Vector3(x, transform.position.y,
                transform.position.z);

            //calcDamage
            CalcDamge(x);


        }
    }

    void CalcDamge(float X)
    {
        CNpcInst[] Arr = gDefine.gNpc.DoDamageShoot(X-1, X+1, mDamage , mDamgeArr,CNpcInst.eNpcClass.OnGround,true);
        for (int i = 0; i < Arr.Length; i++)
        {
            mDamgeArr.Add(Arr[i]);
        }
    }

    

    public void Init(Vector3 BPos, bool FaceRight, float V, int Damage)
    {
        mIsFaceRight = FaceRight;

        mV = FaceRight ? V : -V;
        mDamage = Damage;
        mT = 0;
        mBeginx = gDefine.GetPCTrans().position.x;
        BPos.y += Random.Range(-0.3f, 0.3f);
        gameObject.transform.position = BPos;

        if (!FaceRight)
            GetComponent<SpriteRenderer>().flipX = true;
    }
}
