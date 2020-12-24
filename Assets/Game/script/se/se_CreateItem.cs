using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class se_CreateItem : MonoBehaviour
{
    CSkill.eSkill mSkillType;
    bool mFaceRight;
    // Start is called before the first frame update

    void Event_CreateItem()
    {
        if( mSkillType == CSkill.eSkill.GroundArrow)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mGroundArrowSEPreb);
            o.transform.position = transform.position;
            se_Skill_GroundArrow script = o.GetComponent<se_Skill_GroundArrow>();
            script.Init( transform.position, mFaceRight);
        }
        else  if( mSkillType == CSkill.eSkill.XianJing)
        {
            GameObject o = GameObject.Instantiate(gDefine.gData.mXianJingSEPreb);
            o.transform.position = transform.position;
            se_Skill_XianJing script = o.GetComponent<se_Skill_XianJing>();
            script.Init();
        }

    }

    void Event_Close()
    {
        gameObject.SetActive(false);
        GameObject.Destroy(gameObject);
    }
    public void Init(CSkill.eSkill SkillType, bool FaceRight)
    {
        mSkillType = SkillType;
        mFaceRight = FaceRight;
    }
}
