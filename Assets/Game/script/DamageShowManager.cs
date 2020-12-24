using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDamageShowNode
{
    float mT = 0;
    float mUpV = 0;
    const float mDownAcc = -0.2f;
    const float mLastT = 0.6f;
    Vector3 mOriLocalScale;
    public DamagerShowUI mNode;
    bool mIsHeavy = false;

    public void Init(int Damage,Vector3 Pos, Color C, bool IsHeavy)
    {
        Pos.z -= 0.5f;
        mUpV =5.0f;
        mNode.Refresh(Damage,C);
        mNode.gameObject.transform.position = Pos;
        mOriLocalScale = mNode.gameObject.transform.localScale;
        mIsHeavy = IsHeavy;
        if (mIsHeavy)
            mOriLocalScale *= 1.4f;
    }

    public void Init(string Str, Vector3 Pos, Color C)
    {
        Pos.z -= 0.5f;
        mUpV = 5.0f;
        mNode.Refresh(Str, C);
        mNode.gameObject.transform.position = Pos;
        mOriLocalScale = mNode.gameObject.transform.localScale;
    }

    public void DestorySelf()
    {
        GameObject.Destroy(mNode.gameObject);
    }


    public bool Update(float DeltT)
    {
        mT += DeltT;
        if(mT >= mLastT)
        {
            mNode.gameObject.SetActive(false);
            return false;
        }
        else
        {
            mNode.gameObject.transform.Translate(Vector3.up * mUpV * DeltT);

            mUpV += mDownAcc * DeltT;
            if(mT>0.4f)
            {
                mNode.gameObject.transform.localScale = mOriLocalScale *
                   ( ((mLastT - mT) / (mLastT-0.4f ))*0.8f +0.2f);
            }
            else
            {
                mNode.gameObject.transform.localScale = mOriLocalScale;
            }
            
            return true;
        }
    }

}

public class DamageShowManager : MonoBehaviour
{
    public GameObject mDamageShowPreb;
    List<CDamageShowNode> mDict = new List<CDamageShowNode>();

    // Start is called before the first frame update
    void Start()
    {
        gDefine.gDamageShow = this;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i=0; i<mDict.Count; i++)
        {
            if( !mDict[i].Update(Time.deltaTime))
            {
                mDict[i].DestorySelf();
                mDict.RemoveAt(i);
                i--; 
            }
        }       
    }

    public void CreateDamageShow(int Damage, Vector3 Pos, Color C, bool IsHeavy)
    {
        return;

        CDamageShowNode n = new CDamageShowNode();
        n.mNode = Instantiate(mDamageShowPreb).GetComponent<DamagerShowUI>();

        n.Init(Damage, Pos, C, IsHeavy);

        mDict.Add(n);
    }

    public void CreateDamageShow(string Str, Vector3 Pos, Color C, int code=0)
    {
        if(code==0)
            return;
        
        CDamageShowNode n = new CDamageShowNode();
        n.mNode = Instantiate(mDamageShowPreb).GetComponent<DamagerShowUI>();

        n.Init(Str, Pos, C);

        mDict.Add(n);
    }
}
