using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_lunbo : MonoBehaviour
{
    public float mV;
    int mDamage;
    float mT = 0;
    float mLifeT = 0;

    public GameObject mHitTargetSEPreb;

    List<CNpcInst>mArr = new List<CNpcInst>();

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
            CalcDamge();

        }

        if (Time.time > mLifeT + 10)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

    }

    void CalcDamge()
    {
        CNpcInst[] Arr = gDefine.gNpc.DoDamageLR(transform.position, 1, mDamage, false, CNpcInst.eNpcClass.OnGround,
        true, mArr);
        if (Arr.Length > 0)
        {
            GameObject o = GameObject.Instantiate(mHitTargetSEPreb);
            o.transform.position = Arr[0].GetHitSEPos();

            //gameObject.SetActive(false);
            //GameObject.Destroy(gameObject);

            CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.BigLun_DoubleWave);
            if (d != null && d.mLearnNum > 0)
            {
                for (int i = 0; i < Arr.Length; i++)
                    if (Arr[i].IsLive()&& Random.Range(0,100)<40)
                        Arr[i].AddBuff(CBuff.eBuff.Paralysis, 2 * d.mLearnNum);
            }
        }

        for(int i=0;i<Arr.Length; i++)
            mArr.Add(Arr[i]);

    }

    public void Init(Vector3 BPos, bool FaceRight, int Damage)
    {

        mV = FaceRight ? Mathf.Abs(mV) : -Mathf.Abs(mV);
        mDamage = Damage;
        mT = 0;

        gameObject.transform.position = BPos;

        if (!FaceRight)
            GetComponent<SpriteRenderer>().flipX = true;

        mLifeT = Time.time;
    }
}
