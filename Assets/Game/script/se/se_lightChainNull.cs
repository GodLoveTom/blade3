using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_lightChainNull : MonoBehaviour
{
    LineRenderer mLineRender;
    [Header("生存时间")]
    public float mLiveT = 0.8f;
    Transform mBTrans = null;
    Vector3 mOff;
    public GameObject  mSEbomb;

    public void Event_PlaySound(int SoundId)
    {
        AudioClip clip = gDefine.gData.GetSoundClip(SoundId);
        if (clip != null)
            gDefine.gSound.Play(clip);
    }

    void Update()
    {
        if( Time.time >= mLiveT)
        {
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
        else
        {
            Vector3[] posArr = new Vector3[2];
            posArr[0] = mBTrans.position;
            posArr[1] = posArr[0] + mOff;
            mLineRender.SetPositions(posArr);
        }
        
    }

    public void Init(Transform T, Vector3 Off, int Damage, float L )
    {
        mLineRender = gameObject.GetComponent<LineRenderer>();

        transform.position = T.position;

        mBTrans = T;

        mLineRender.positionCount = 2;//设置顶点数量
        Vector3[] posArr = new Vector3[2];
        posArr[0] = T.position;
        posArr[1] = posArr[0] + Off;
        mLineRender.SetPositions(posArr);

        mLiveT = Time.time + mLiveT;

        mOff = Off;

        CNpcInst [] arr = gDefine.gNpc.FindByLine( T.position.x, T.position.x + Off.x, CNpcInst.eNpcClass.OnGround);
        for(int i=0; i<arr.Length;i++)
        {
            arr[i].BeDamage(Damage, false, false,true);
            GameObject o = GameObject.Instantiate(mSEbomb);
            o.transform.position = arr[i].GetHitSEPos();
        }
    }



}
