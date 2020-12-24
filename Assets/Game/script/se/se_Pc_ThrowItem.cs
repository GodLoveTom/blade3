using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Pc_ThrowItem : MonoBehaviour
{
    [Header("Y轴偏离修正")]
    public float mMaxOffH;
    [Header("x方向扔出距离")]
    public float mXL;
    [Header("x方向扔出速度")]
    public float mVx;
    [Header("落地后特效")]
    public GameObject mCreateItemSEPreb;
    Vector3 mBPos;
    Vector3 mEPos;
    float mT=0;
    CSkill.eSkill mSkillType;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mT += Time.deltaTime;
        float perc = mT * mVx / Mathf.Abs(mEPos.x - mBPos.x) ;
        if( perc >= 1.0f)
        {
            GameObject o = GameObject.Instantiate(mCreateItemSEPreb);
            o.transform.position = mEPos;
            se_CreateItem script = o.GetComponent<se_CreateItem>();
            script.Init(mSkillType, mEPos.x > mBPos.x ? true :false);

            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);

        }
        else
        {
             float x = mBPos.x + (mEPos.x - mBPos.x)*perc;
             float y = mBPos.y + (mEPos.y - mBPos.y)*perc +  ((perc<0.5f) ?  mMaxOffH * perc : mMaxOffH *(1-perc));

             transform.position = new Vector3( x, y, transform.position.z);
        }
        
    }

    public void Init(Vector3 BPos,  Vector3 EPos,  CSkill.eSkill SkillType, bool FaceRight)
    {
        mT = 0;
        mSkillType = SkillType;
        transform.position = BPos;
        mBPos = BPos;
        mEPos = EPos;
        mEPos.y = gDefine.gGrounY;

        Event_PlaySound(30);
    }

    public void Event_PlaySound(int SoundId )
    {
        gDefine.PlaySound(SoundId);
    }
}
