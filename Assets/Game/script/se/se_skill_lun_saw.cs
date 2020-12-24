using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_skill_lun_saw : MonoBehaviour
{
    [Tooltip("伤害间隔时间")]
    public float mDamageSpareT = 1;

    //[Tooltip("内部使用的伤害计时")]
    private float mTmpT;

    [Tooltip("伤害半径")]
    public float mDamageR;

    //[Tooltip("伤害")]
    int mDamage;

    //[Tooltip("生存时间，单位秒")]
    float mLifeT;

    [Tooltip("飞行速度")]
    public float mV = 50;

    [Tooltip("飞行中大小 (0-1.0) 默认0.6")]
    public float mFlyScale = 0.6f;

    [Tooltip("最终大小 (0-1.0) 默认1.6")]
    public float mEndScale = 1.6f;

    //[Tooltip("目标位置")]
    Vector3 mEPos;

    //[Tooltip("飞到位置么")]
    bool mFlyOver = false;



    // Start is called before the first frame update
    void Start()
    {
        mTmpT = mDamageSpareT;
        gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mFlyOver)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, mEPos, Time.deltaTime * mV);

            gameObject.transform.position = pos;

            if (Mathf.Abs(pos.x - mEPos.x) < 0.001f)
            {
                mFlyOver = true;
                gameObject.transform.localScale = Vector3.one * mEndScale;
            }

        }
        else
        {
            mLifeT -= Time.deltaTime;
            if (mLifeT < 0)
            {
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
                return;
            }

            CNpcInst npc = gDefine.gNpc.FindWithR(transform.position.x, 6, CNpcInst.eNpcClass.OnGround);
            if (npc != null)
            {
                if (Mathf.Abs(npc.GetPos().x - transform.position.x) > 0.1f)
                {
                    mEPos.x = npc.GetPos().x;
                    Vector3 pos = Vector3.MoveTowards(transform.position, mEPos, Time.deltaTime * 1.5f);
                    transform.position = pos;
                }
            }

            if (Time.time >= mTmpT + mDamageSpareT)
            {
                mTmpT = Time.time + mDamageSpareT;
                DoDamage();
            }

        }
    }

    public void DoDamage()
    {
        gDefine.gNpc.DoDamageLRAndPushOff(transform.position, mDamageR * 2, mDamage, CNpcInst.eNpcClass.OnGround,
        true);
    }

    public void Init(Vector3 EPos, int Damage, float LiveT, bool FaceRight)
    {
        mDamage = Damage;

        mLifeT = LiveT;

        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.QuantumMask);
        if (d != null)
            mLifeT += 3 * d.mLearnNum;
        mEPos = EPos;

        Animator animator = GetComponent<Animator>();
        if (FaceRight)
            animator.Play("right");
        else
            animator.Play("left");


    }
}
