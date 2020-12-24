using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Hp : MonoBehaviour
{
    public GameObject mHp; 
    public NpcMono mRefNpc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( mRefNpc.mNpcInst.mHp<=0)
        {
            gameObject.SetActive(false);
            return;
        }
        float perc = (float)mRefNpc.mNpcInst.mHp / (float)mRefNpc.mNpcInst.mMaxHp;
        perc = Mathf.Clamp(perc,0f,1f);
        if(perc<0) perc = 0;
        mHp.transform.localScale = new Vector3(perc, 1,1);
    }
}
