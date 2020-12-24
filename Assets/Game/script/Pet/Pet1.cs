

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet1 : MonoBehaviour
{
    #region 数据区域
    [Header("眼睛参考点")]
    public GameObject mRefEyePoint;
    [Header("跟随距离最小")]
    public float mFollowLMinParam = 2;
    [Header("跟随距离最大")]
    public float mFollowLMaxParam = 3;
    [Header("跟随速度")]
    public float mFollowVParam = 8;
    [Header("攻击间隔时间s")]
    public float mAtkTParam = 7; // 攻击间隔时间
    float mAtkT = 0; // 攻击间隔时间
    [Header("子弹发射间隔时间s")]
    public float mBulletFireTParam = 0.3f;

    float mBulletFireT = 0; // 发射子弹间隔时间
    int mFireNum = 0; //攻击的子弹数量
#region  技能相关属性
    [Header("技能间隔时间s")]
    public float mSkillTParam = 15;
    [Header("技能持续时间s")]
    public float mSkillLastTParam = 3;
    float mSkillNextBT; //技能间隔时间计时
    float mSkillBT; //技能开始时间
    bool mSkillFaceRight = true; //技能发射时，我的朝向
    [Header("技能开火的火花")]
    public GameObject mFireSEObj;

    [Header("技能line")]
    public LineRenderer mLine;
    [Header("技能命中特效")]
    public GameObject mSkillHitSEPreb;
    float mSkillLength = 0; //技能长度
    float mSkillV = 30; //技能穿越速度
    float mSkillDamageT = 0; //伤害间隔时间
    List<CNpcInst> mSkillDamageNpcArr = new List<CNpcInst>();
    List<GameObject> mSkillHitSEArr = new List<GameObject>(); 
    

#endregion


    [Header("动画")]
    public Animator mAnim;
    Vector3 mFllowPos; // 这只是一个偏移，动态计算在每一个帧里

    GameObject mBulletPreb;
    bool mIsDamageX2;

    enum eState
    {
        Idle,
        Follow,
        Skill,
    }
    eState mState = eState.Idle;
    float mStateT = 0;
    #endregion

    // Update is called once per frame
    void Update()
    {

        #region 技能

        if (UpdateSkill())
            return;

        #endregion

        #region 移动判断
        mBulletFireTParam = 0.08f;
        if (mState == eState.Idle)
        {
            if (Time.time > mStateT || Mathf.Abs(gDefine.GetPCTrans().position.x - transform.position.x) > mFollowLMaxParam)
            {
                float x = Random.Range(mFollowLMinParam, (mFollowLMinParam + mFollowLMaxParam) / 2);
                mFllowPos = new Vector3(Random.Range(0, 100) < 50 ? x : -x, 0, 0);
                mState = eState.Follow;
            }
        }
        else if (mState == eState.Follow)
        {
            Vector3 pos = mFllowPos + gDefine.GetPCTrans().position;
            Vector3 npos = Vector3.MoveTowards(transform.position, pos, Time.deltaTime * mFollowVParam);
            transform.position = npos;
            if (Vector3.Distance(pos, npos) < 0.01f)
            {
                mState = eState.Idle;
                mStateT = Random.Range(12, 16) + Time.time;
            }
        }
        #endregion



        #region 普通攻击
        UpdateAtk();
        #endregion

    }

    bool UpdateSkill()
    {

        //检查是否可以开始技能
        if (mState != eState.Skill && mFireNum <= 0 && Time.time > mSkillNextBT)
        {
            CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 7, CNpcInst.eNpcClass.OnGround);
            if(npc!=null)
            {
                mState = eState.Skill;
                mAnim.Play("skill", 0, 0);
                mSkillNextBT = Time.time + 15;
                mSkillFaceRight = npc.GetPos().x > transform.position.x ? true:false;
                mSkillBT = Time.time;

                mSkillDamageNpcArr.Clear();
                mSkillHitSEArr.Clear();

                mSkillLength = 0;
            }
        }

        if (mState == eState.Skill)
        {
            mFireSEObj.transform.right = mSkillFaceRight?Vector3.right:Vector3.left;
            if(Time.time > mSkillBT + mSkillLastTParam)
            {
                mState = eState.Idle;
                mAnim.Play("move",0);
                foreach (var item in mSkillHitSEArr)
                    GameObject.Destroy(item);
                mSkillHitSEArr.Clear();
                return false;
            }

            mSkillLength += mSkillV * Time.deltaTime;
            //float ex = transform.position.x + ((mSkillFaceRight)?mSkillLength:-mSkillLength);
            //
            mLine.positionCount = 2;

            Vector3[] arr = new Vector3[2];
            arr[0].x = mRefEyePoint.transform.position.x;
            arr[0].y = mRefEyePoint.transform.position.y;
            arr[0].z = mRefEyePoint.transform.position.z;

            arr[1].x = arr[0].x + ((mSkillFaceRight)?mSkillLength:-mSkillLength);
            arr[1].y = mRefEyePoint.transform.position.y;
            arr[1].z = mRefEyePoint.transform.position.z;

            mLine.SetPositions(arr);
            
            //添加npc
            CNpcInst [] npcarr =  gDefine.gNpc.FindByLine(transform.position.x ,
                 arr[1].x, CNpcInst.eNpcClass.OnGround );

            for(int i=0; i<npcarr.Length; i++)
            {
                if(!mSkillDamageNpcArr.Contains(npcarr[i]))
                {
                    mSkillDamageNpcArr.Add(npcarr[i]);
                    GameObject hit = GameObject .Instantiate( mSkillHitSEPreb);
                    hit.transform.SetParent( npcarr[i].GetRefMid().transform);
                    hit.transform.localPosition = Vector3.zero;
                    mSkillHitSEArr.Add(hit);
                }
            }

            bool needCalcDamage = false;
            if(Time.time > mSkillDamageT )
            {
                needCalcDamage = true;
                mSkillDamageT = Time.time + 0.33333f;
            }

            for (int i = 0; i < mSkillDamageNpcArr.Count; i++)
            {
                if( !mSkillDamageNpcArr[i].IsLive())
                {
                    mSkillDamageNpcArr.RemoveAt(i);
                    GameObject.Destroy( mSkillHitSEArr[i]);
                    mSkillHitSEArr.RemoveAt(i);
                }
                else if( needCalcDamage)
                {
                    mSkillDamageNpcArr[i].BeDamage( (int)(gDefine.gPlayerData.mDamage * 0.3333f),
                   false, false, true, false );
                }
            }

            return true;
        }
        else
            return false;
    }



    #region  攻击函数
    void UpdateAtk()
    {
        CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 7, CNpcInst.eNpcClass.All);
        if (mFireNum > 0)
        {
            if (Time.time > mBulletFireT + mBulletFireTParam)
            {
                mBulletFireT = Time.time + mBulletFireTParam;
                //fire
                mAnim.Play("fire", 0, 0);
                //
                if (mBulletPreb == null)
                    mBulletPreb = gDefine.gABLoad.GetPreb("obj.bytes", "Pet1Bullet");
                GameObject b = GameObject.Instantiate(mBulletPreb);
                Pet1Bullet s = b.GetComponent<Pet1Bullet>();

                Vector3 endPos;
                if (npc != null)
                {
                    endPos = mRefEyePoint.transform.position + (npc.GetHitSEPos() - mRefEyePoint.transform.position).normalized * 30;
                }
                else
                {
                    endPos = mRefEyePoint.transform.position + Vector3.right * 20 * ((gDefine.GetPCTrans().position.x > transform.position.x) ? -1 : 1);
                }

                Vector3 bPos = mRefEyePoint.transform.position + (endPos - mRefEyePoint.transform.position).normalized * 1;

                s.Init(bPos, endPos, mIsDamageX2);
                mFireNum--;
            }
        }
        else if (Time.time > mAtkTParam + mAtkT && npc != null)
        {
            mFireNum = 5;
            mAtkT = Time.time;
            gDefine.PlaySound(6);
        }
        else
        {
            mFireNum = 0;
        }
    }
    #endregion

    public void Init(Vector3 BPos, bool IsDamageX2)
    {
        BPos.x += Random.Range(0, 100) < 50 ? (mFollowLMinParam + mFollowLMaxParam) * 0.5f : -(mFollowLMinParam + mFollowLMaxParam) * 0.5f;
        transform.position = BPos;
        mIsDamageX2 = IsDamageX2;
        mSkillNextBT = Time.time + 15;
    }
}
