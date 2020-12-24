using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CGainInFight 
{
    int mCoins = 0;
    int mCrystals = 0;

    public void Init()
    {
        mCoins = 0;
        mCrystals = 0;
    }
    
    public void AddCoins(int Num, string Reason)
    {
        mCoins += Num;
        Debug.Log("Coins:" + Num.ToString() +":" + mCoins.ToString()  +":"+Reason);
        
    }

     public void AddCrystals(int Num, string Reason)
    {
        mCrystals += Num;
        Debug.Log("crystal: " + Num.ToString() +":" + mCrystals.ToString()  +":"+Reason);
    }

   
}
