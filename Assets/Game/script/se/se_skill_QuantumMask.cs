using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_skill_QuantumMask : MonoBehaviour
{
    [Header("存在时间")]
    public float mLiveT;

    float mT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if( Time.time > mT )
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            GameObject.Destroy(gameObject);
        }
    }

    public void Init(Transform T)
    {
        gameObject.transform.SetParent(T.transform);
        gameObject.transform.localPosition = Vector3.zero;

        mT = mLiveT +    Time.time;
        CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.QuantumMask);
            if(d!=null)
            mT += d.mLearnNum;

        CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Cloak];
             if(gird.mRefItem!=null&& gird.mRefItem.mSpecialIndex==3)
                mT+=1;
                
        gDefine.gPlayerData.mIgonrDamageT = mT;
               
    }
}
