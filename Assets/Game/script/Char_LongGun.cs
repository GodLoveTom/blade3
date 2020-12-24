using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Char_LongGun : MonoBehaviour
{
    Animator mAnimator;
    public Transform mRefPointL;
    public Transform mRefPointR;
    public GameObject mBulletPreb;
    bool mIsFaceR = true;

    public Text mHpText;
    public Image mHpImage;

    float mT = 5;

    int mSoundHashCode = 0;

    public GameObject mHpRoot;

    Vector3 mCurAimPos;
    bool mIsShootAir = false;

    CNpcInst mShootAim;

    bool mCanFire = true;

    string mActName = "";

    //打断相关
    float mNoBreakT = 0; //不可打断时间
    float mFirstBtUpT = -1; //第一次按键抬起计时，连续两次点击，打断当前的射击
    [Header("射击时子弹壳位置引用")]
    public GameObject mBulletPos;
    [Header("子弹壳preb")]
    public GameObject mBulletShellPreb;

    //是否是影子，如果是影子，则不会被打断
    bool mIsShadow = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!mIsShadow)
        {
            mAnimator = GetComponent<Animator>();
            SetStandAct(true);
            //gDefine.gPcTrans = gameObject.transform.parent.transform;

            gDefine.gPcLongGun = this;

            gameObject.SetActive(false);

        }

    }

    // Update is called once per frame
    void Update()
    {
        mT -= Time.deltaTime;
        if (mT <= 0)
        {
            //close and back..
            gameObject.SetActive(false);
            gDefine.gSound.EndSoundSE(mSoundHashCode);
            if (mIsShadow)
                GameObject.Destroy(gameObject);
            else
                gDefine.EndUseGunGirl();
        }

        //查找最近的npc
        CNpcInst npc = gDefine.gNpc.FindByR(transform.position.x, 11, CNpcInst.eNpcClass.All);
        if (npc != null)
        {
            mIsFaceR = npc.GetPos().x > transform.position.x ? true : false;
            //计算持枪角度
            string actName = CalcActNameByAirNpcAngle(npc);

            mActName = actName;

            mAnimator.Play(actName);

            mShootAim = npc;

            mCurAimPos = npc.GetHitSEPos();
            if (!npc.mNpc.mIsInAir)
            {
                if (mIsFaceR)
                    mCurAimPos = mRefPointR.transform.position + Vector3.right * 50;
                else
                    mCurAimPos = mRefPointR.transform.position + Vector3.left * 50;

            }
            else
            {
                Vector3 dir = mCurAimPos - mRefPointR.transform.position;
                dir.Normalize();
                mCurAimPos = mRefPointR.transform.position + dir * 50;
            }

        }
        else
        {
            mCurAimPos = mRefPointR.transform.position + (mIsFaceR ? Vector3.right * 40 : Vector3.left * 40);
            mAnimator.Play("fire");
            mIsShootAir = false;

            mShootAim = null;
        }

        transform.rotation = new Quaternion();
        mHpRoot.transform.rotation = new Quaternion();

        if (!mIsFaceR)
        {
            transform.Rotate(0, 180, 0, Space.World);
            mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
        }

    }

    public void Mouse_UpOrClick(bool IsRight)
    {
        if (mIsShadow)
            return;

        //Debug.Log ("IsClick ");
        if (Time.time <= mNoBreakT)
        {
            //Debug.Log ("return ");
            return;
        }

        if (mFirstBtUpT < 0)
        {
            /// Debug.Log ("first ");
            mFirstBtUpT = Time.time;
        }

        else if (Time.time - mFirstBtUpT < 0.5f)
        {
            //break
            //  Debug.Log ("break "+ (Time.time - mFirstBtUpT).ToString());

            gameObject.SetActive(false);
            gDefine.EndUseGunGirl();
            gDefine.Btn_Click(IsRight);

        }
    }

    public void Begin(Vector3 Pos, bool FaceR, float ExistT, bool IsShadow = false)
    {
        mAnimator = GetComponent<Animator>();

        mNoBreakT = Time.time + 0.1f;
        mFirstBtUpT = -1;

        mIsFaceR = FaceR;
        gameObject.SetActive(true);
        SetPosition(Pos);

        //SetAtkAct(FaceR);
        mT = ExistT;
        RefreshHPUI();

        AudioClip clip = gDefine.gData.GetSoundClip(6);
        if (clip != null)
            gDefine.gSound.Play(clip);

        float l = Mathf.Abs(mRefPointR.transform.position.x - Pos.x);

        mIsShadow = IsShadow;
        gDefine.gNpc.PushBackWithInR(Pos, l + 0.5f);

        Update();

    }


    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;
    }

    // public void SetAtkAct(bool IsFaceR)
    // {
    //     //查找最近的空中怪
    //     CNpcInst airNpc = gDefine.gNpc.FindAirByL(transform.position.x, mIsFaceR, 7);
    //     //查找最近的npc
    //     CNpcInst npc = gDefine.gNpc.FindByL(transform.position.x, mIsFaceR, 7);

    //     //如果npc，空中怪都存在，较高几率攻击空中怪，较低几率平打npc
    //     //空中怪计算夹角
    //     bool atkAir = false;

    //     if ((airNpc != null && npc != null && Random.Range(0, 100) < 85) || (airNpc != null && npc == null))
    //         atkAir = true;
    //     else
    //         atkAir = false;

    //     //设置方向
    //     if (atkAir)
    //     {

    //         string actName = CalcActNameByAirNpcAngle(airNpc);
    //         mAnimator.Play(actName);
    //         mIsShootAir = true;
    //         mCurAimPos = airNpc.GetHitSEPos();
    //     }
    //     else
    //     {
    //         mCurAimPos = mRefPointR.transform.position + (mIsFaceR ? Vector3.right * 100 : Vector3.left * 100);
    //         mAnimator.Play("fire");
    //         mIsShootAir = false;
    //     }

    //     //if( IsFaceR )

    //     //else
    //     //    mAnimator.Play("gun_fireL");

    //     transform.rotation = new Quaternion();
    //     mHpRoot.transform.rotation = new Quaternion();

    //     if (!IsFaceR)
    //     {
    //         transform.Rotate(0, 180, 0, Space.World);
    //         mHpRoot.transform.Rotate(0, 180, 0, Space.Self);
    //     }
    // }

    string CalcActNameByAirNpcAngle(CNpcInst Npc)
    {
        Vector3 dir = Npc.GetPos() - transform.position;
        float angle = Vector3.Angle(Vector3.right, dir);

        angle = angle > 90 ? 180 - angle : angle;
        if (angle <= 5 || !Npc.mNpc.mIsInAir)
            return "fire";
        else
         if (angle <= 20)
            return "fire15";
        else if (angle <= 40)
            return "fire30";
        else if (angle <= 50)
            return "fire45";
        else if (angle <= 65)
            return "fire60";
        else if (angle <= 80)
            return "fire75";
        else
            return "fire90";


    }

    public void SetStandAct(bool IsFaceR)
    {
        if (IsFaceR)
            mAnimator.Play("longgun_stand");
        else
            mAnimator.Play("longgun_standL");
    }

    void BeginShoot()
    {
        if (mCanFire)
        {
            GameObject o = GameObject.Instantiate(mBulletPreb);
            Vector3 Pos;
            //if (mIsFaceR)
            Pos = mRefPointR.position;
            Vector3 dest = mCurAimPos;
            if (mActName == "fire")
            {
                float delty = Random.Range(-0.2f, 0.6f);
                Pos.y += delty;
                dest.y = Pos.y;
            }

            //else
            //  Pos = mRefPointL.position;

            bullet1 bullet = o.GetComponent<bullet1>();

            //if(mShootAim!=null && mShootAim.GetHitSEPos().y > Pos.y)
            bullet.Init(Pos, dest, 30, (int)gDefine.gPlayerData.mGunDamage);


            GameObject shell = GameObject.Instantiate(mBulletShellPreb);

            float vx = Random.Range(3.0f, 5.0f) * 2;
            float vy = Random.Range(4.0f, 8.0f) * 2;
            if (mIsFaceR)
                vx = -vx;

            shell.GetComponent<se_itemDrop>().Init(vx, vy);
            shell.transform.position = mBulletPos.transform.position;
            //else
            //  bullet.Init(Pos, mIsFaceR, 30, (int)gDefine.gPlayerData.mGunDamage);
        }
        mCanFire = !mCanFire;

    }

    public void RefreshHPUI()
    {
        mHpText.text = gDefine.gPlayerData.mHp.ToString();
        mHpImage.transform.localScale = new Vector3(gDefine.gPlayerData.mHp / (float)gDefine.gPlayerData.mHpMax, 1, 1);
    }



}
