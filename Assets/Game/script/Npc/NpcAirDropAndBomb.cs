using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAirDropAndBomb : MonoBehaviour
{
    bool mIsGo = false;
    public float mDropV = 18;
    float mVx = 10;
    float mDeadLineY;

    public bool mHasDropAct = false;
    // Start is called before the first frame update    

    public void GoDie1()
    {
        mIsGo = true;
        mVx = Random.Range(5f,8f);
        if(Random.Range(0,100)<50)
            mVx = -mVx;
        mDeadLineY = gDefine.gGrounY + Mathf.Abs(transform.position.y - gDefine.gGrounY)*Random.Range(0.3f,0.5f);
        if(mHasDropAct)
        {
            Animator a = gameObject.GetComponent<Animator>();
            a.Play("dropDie1",0,0);
        }

        //Debug.Log("Act:Drop");
    
    }
    // Update is called once per frame
    void Update()
    {
        if(mIsGo)
        {
            Vector3 pos = transform.position;
            pos.y -= Time.deltaTime * mDropV;
            if(Mathf.Abs( mVx) > 0.1f)
            {
                float vx = Mathf.Abs(mVx) - Time.deltaTime * 10 ;
                if( vx < 0 ) vx = 0;
                mVx = mVx * vx / Mathf.Abs(mVx);
            }

            pos.x += mVx * Time.deltaTime;

            transform.position = pos;
                float r = Time.deltaTime*270;
                transform.Rotate(0,0,r,Space.Self);

            if(pos.y<=gDefine.gGrounY)
            {
                //pos.y = gDefine.gGrounY;
                gameObject.SetActive(false);
                GameObject.Destroy(gameObject);
                //bomb
                GameObject o = GameObject.Instantiate(gDefine.gData.m空中怪死亡落地爆照特效Preb);
                o.transform.position = pos;
                o.transform.localScale = Vector3.one * 2;
                //Vibration.VibratePop ();
                gDefine.PlaySound(41);
            }
           
        }
        
    }
}
