using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Skill_LightChain : MonoBehaviour
{
    [Header("命中效果")]
    public GameObject mHitSEPreb;
    // Start is called before the first frame update
    CNpcInst mNpc;
    LineRenderer mLineRender;
    int mNextLineNum;
    [Header("伤害系数")]
    public float mDamageParam = 1.3f;
    [Header("传播距离")]
    public float mSpreadL = 3.0f;

    [Header("生存时间")]
    public float mLiveT = 0.8f;
     [Header("伤害衰减系数")]
    public float mDecayPerc = 0.9f;
    int mDamage;
    bool mIsFirstSpread = true;

    Transform mBTrans = null;
    Vector3 mNpcPos;

    // void Event_End()
    // {
    //    // gameObject.SetActive(false);
    //    // GameObject.Destroy(gameObject);
    // }

    void Event_DoDamage()
    {
        // if(mNpc!=null)
        // {
        //     mNpc.BeDamage(mDamage, false,false,true,false, CSkill.eSkill.LightChain);
        // }
      
    }

      public void Event_PlaySound(int SoundId )
    {
        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if(clip != null)
            gDefine.gSound.Play(clip);
    }

    void Update() 
    {
        // if( mNpc == null || mBTrans == null )
        // {
        //     Event_End();
        // }
        // else if(!mNpc.IsLive())
        // {
        //     Event_End();
        // }
        // else
        if( mBTrans==null || Time.time > mLiveT)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }

        if( Time.time >= mLiveT && mDamage > 0 )
        {
              if( mNpc.IsLive() )
                  mNpc.BeDamage(mDamage, false,false,true,false, CSkill.eSkill.LightChain);  
            mDamage = 0;
        }

        {
 
            Vector3[] posArr = new Vector3[2];
            posArr[0] = mBTrans.position ;
            if( mNpc.IsLive())
            {
                posArr[1] = mNpc.GetRefMid().transform.position;
                mNpcPos = posArr[1];
            }
            else
            {
                posArr[1] = mNpcPos;
            }
            
            mLineRender.SetPositions(posArr);
        }
    }

    void Event_NextChain()
     {
    //     if( mIsFirstSpread )
    //     {
    //         if(Random.Range(0,100) < 50 )
    //             return;

    //         mIsFirstSpread = false;
    //     }

    //     CNpcInst[] Arr = gDefine.gNpc.FindByL( mNpc.GetPos().x, mSpreadL, mNpc);
    //     if(Arr.Length>0)
    //     {
    //         int Index = Random.Range(0,Arr.Length);
    //         CNpcInst nextNpc = Arr[Index];

    //         GameObject o = GameObject.Instantiate(gameObject);
    //         se_Skill_LightChain script = o.GetComponent<se_Skill_LightChain>();
    //         script.Init( mNpc.GetRefMid().transform, nextNpc, mNextLineNum--,(int)( mDamage*0.9f));
    //     }
    }

   public void Init(Transform T, CNpcInst Npc, int NextLineNum)
    {
        mLineRender = gameObject.GetComponent<LineRenderer>();

        transform.position = T.position;
        mNpc = Npc;
        mNextLineNum = NextLineNum;

        mLineRender.positionCount =  2;//设置顶点数量
        Vector3[] posArr = new Vector3[2];
        posArr[0] = T.transform.position ;
        posArr[1] = Npc.GetRefMid().transform.position;
        mLineRender.SetPositions(posArr);
        mDamage = (int)(gDefine.gPlayerData.mDamage  *  mDamageParam);

        mNpcPos = posArr[1];

        mLiveT = Time.time + mLiveT;

        mBTrans = T;

          GameObject hit = GameObject.Instantiate(mHitSEPreb);
                hit.transform.position = mNpc.GetRefMid().transform.position;
    }

    public void Init(Transform T, CNpcInst Npc, int NextLineNum, float DamgeParam)
    {
        mLineRender = gameObject.GetComponent<LineRenderer>();

        transform.position = T.position;
        mNpc = Npc;
        mNextLineNum = NextLineNum;

        mLineRender.positionCount =  2;//设置顶点数量
        Vector3[] posArr = new Vector3[2];
        posArr[0] = T.position ;
        posArr[1] = Npc.GetRefMid().transform.position;
        mLineRender.SetPositions(posArr);
        mDamage = (int)(gDefine.gPlayerData.mDamage  *  DamgeParam);

         mNpcPos = posArr[1];

        mLiveT = Time.time + mLiveT;

        mBTrans = T;

          GameObject hit = GameObject.Instantiate(mHitSEPreb);
                hit.transform.position = mNpc.GetRefMid().transform.position;
    }

    public void Init(Transform T, CNpcInst Npc, int NextLineNum, int Damage)
    {
        mLineRender = gameObject.GetComponent<LineRenderer>();
        
        transform.position = T.position;
        mNpc = Npc;
        mNextLineNum = NextLineNum;

        mLineRender.positionCount =  2;//设置顶点数量
        Vector3[] posArr = new Vector3[2];
        posArr[0] = T.position ;
        posArr[1] = Npc.GetRefMid().transform.position;
        mLineRender.SetPositions(posArr);
        mDamage = Damage;
        mNpcPos = posArr[1];
        mLiveT = Time.time + mLiveT;

        mBTrans = T;
          GameObject hit = GameObject.Instantiate(mHitSEPreb);
                hit.transform.position = mNpc.GetRefMid().transform.position;
    }

    // public void Init(Transform T, CNpcInst Npc, int NextLineNum, int Damage)
    // {
    //     mLineRender = gameObject.GetComponent<LineRenderer>();
        
    //     transform.SetParent(T.transform) ;

    //     mNpc = Npc;
    //     mNextLineNum = NextLineNum;

    //     mLineRender.positionCount =  2;//设置顶点数量
    //     Vector3[] posArr = new Vector3[2];
    //     posArr[0] = T.transform.position ;
    //     posArr[1] = Npc.GetRefMid().transform.position;
    //     mLineRender.SetPositions(posArr);
    //     mDamage = Damage;

    //     mLiveT = Time.time + mLiveT;

    //     mBTrans = T;
    // }


}
