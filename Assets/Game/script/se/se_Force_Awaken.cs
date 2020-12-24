using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_Force_Awaken : MonoBehaviour
{
    public float mLiveT = 3.0f;
    float mT;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > mT)
        {
            transform.SetParent(null);
            gameObject.SetActive(false);
            GameObject .Destroy(gameObject);      
        }
    }

    public void Init(Transform T)
    {
        transform.SetParent(T);
        transform.localPosition = Vector3.zero;
        mT = Time.time + mLiveT;
        gDefine.gPlayerData.mIgonrDamageT = Time.time + mLiveT;

         CSkillAddData d = gDefine.gPlayerData.mSkillAdd.Find(CSkillAdd.eSkillAdd.ForceAwaken);
        if (d != null)
        {
             mT = Time.time + mLiveT + d.mLearnNum;
             CGird gird = gDefine.gPlayerData.mEquipGird[(int)gDefine.eEuqipPos.Cloak];
             if(gird.mRefItem!=null&& gird.mRefItem.mSpecialIndex==3)
                mT+=1;
             gDefine.gPlayerData.mIgonrDamageT = Time.time + mLiveT+ d.mLearnNum;
        }
    }




}
