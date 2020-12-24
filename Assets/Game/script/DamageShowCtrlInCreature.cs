using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDSCIC_Data
{
    public int mDamage;
    public string mStr;
    public Color mC;
    public bool mIsHeavy;
    public Vector3 mPos;
}


public class DamageShowCtrlInCreature 
{
    List<CDSCIC_Data> mDict = new List<CDSCIC_Data>();

    float mT = 0;

    // Update is called once per frame
    public void Update()
    {
        mT -= Time.deltaTime;
        if (mT <= 0)
        {
            if (mDict.Count > 0)
            {
                CDSCIC_Data d = mDict[0];
                mDict.RemoveAt(0);
                mT = 0.2f;
                if( ! string.IsNullOrEmpty(d.mStr))
                    gDefine.gDamageShow.CreateDamageShow(d.mStr, d.mPos, d.mC);
                else
                    gDefine.gDamageShow.CreateDamageShow(d.mDamage, d.mPos, d.mC, d.mIsHeavy);

            }
        }  
    }

    public void Add( int Damage, Vector3 Pos, Color C, bool IsHeavy)
    {
        if (mT <= 0)
        {
            gDefine.gDamageShow.CreateDamageShow(Damage, Pos, C, IsHeavy);
            mT = 0.2f;
        }
        else
        {
            CDSCIC_Data d = new CDSCIC_Data();
            d.mC = C;
            d.mDamage = Damage;
            d.mIsHeavy = IsHeavy;
            d.mPos = Pos;
            mDict.Add(d);
        }
    }

    public void Add( string Str, Vector3 Pos, Color C)
    {
        if (mT <= 0)
        {
            gDefine.gDamageShow.CreateDamageShow(Str, Pos, C);
            mT = 0.2f;
        }
        else
        {
            CDSCIC_Data d = new CDSCIC_Data();
            d.mC = C;
            d.mStr = Str;
            //d.mIsHeavy = IsHeavy;//文字提示的时候，暂时这里用不到，
            d.mPos = Pos;
            mDict.Add(d);
        }
    }
}
