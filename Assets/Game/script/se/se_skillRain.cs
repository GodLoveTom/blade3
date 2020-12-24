using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_skillRain : MonoBehaviour
{
    public GameObject mSwordPreb;
    public int mSwordNum=12;
    public float mSpareT=1.0f;
    public float mSwordH=20;
    public float mSwordV=50;
    float t;
    bool mBegin = false;
    Vector3 mCurAimPos;
    int mDamage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int Damage)
    {
        t = mSpareT;
        mDamage = Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if( mSwordNum > 0 )
        {
            t -= Time.deltaTime;
            if(t < 0 )
            {
                t = mSpareT;
                CNpcInst inst = gDefine.gNpc.FindByL(gDefine.GetPCTrans().position.x - 7, true, 14);
                if( inst != null )
                {
                    if(!mBegin)
                          gDefine.gDamageShow.CreateDamageShow("剑雨", gDefine.GetPCTrans().position + Vector3.up * 3, new Color(0.9f, 0.5f, 0.9f, 1));
                    mBegin = true;
                    mCurAimPos = inst.GetPos();
                    mCurAimPos.y = gDefine.gGrounY;

                }

                GameObject sword = GameObject.Instantiate(mSwordPreb);
                Vector3 fpos = mCurAimPos + Vector3.right * Random.Range(-1.0f,1.0f);
                sword.GetComponent<se_skillRain_node>().Init(mDamage, fpos + Vector3.up * 20, fpos, mSwordV,
                    4, 3);

                mSwordNum--;

                AudioClip clip = gDefine.gData.GetSoundClip(40);
                if(clip!=null)
                    gDefine.gSound.Play(clip);
            }
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
        
    }
}
