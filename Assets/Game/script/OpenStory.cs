using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStory : MonoBehaviour
{
    public float mCamBeginPosX = 5.15f;
    public GameObject mRefBossPos;
    public ui_se_BlackMask mBlckMask;
    public ui_StoryTalk mRefUIStoryTalk;
    public NpcStoryBoss mBoss;
    public GameObject mBossPreb;
    public enum eStep
    {
        NUll=0,
        FirstShowBlackToWhite,
        Talk,
        Talk0,
        Talk1,
        Talk2,
        Talk3,
        BossSkill,
        PlayerDie,
         Talk4,
        Talk5,
        BossGo,
        ScreenToBlackToLight,
        SpriteTalk0,
        SpriteTalk1,
        PlayerCure,
        PlayerRelive,
        PlayerTalk0,
        PlayerTalk1,
        End,
    }

    eStep mStep = eStep.NUll;
    float mStepT = 0;
    public bool mIsOpen = false;


    public void BeginOPStory()
    {
        mStep = eStep.FirstShowBlackToWhite;
        mBlckMask.ShowBlackToClear(NextStep);
        Camera.main.GetComponent<camfollow>().BeginStory(mCamBeginPosX);

        GameObject boss = GameObject.Instantiate(mBossPreb);
        boss.SetActive(true);
        mBoss = boss.GetComponent<NpcStoryBoss>();
        boss.transform.SetParent(mRefBossPos.transform);
        boss.transform.localPosition = Vector3.zero;

        gDefine.gGameMainUI.ShowStoryUI();
        mIsOpen = true;

    }

    void EndStory()
    {
        mRefUIStoryTalk.gameObject.SetActive(false);
        Camera.main.GetComponent<camfollow>().EndStory();
        mIsOpen = false;
        gDefine.gLogic. mTeach.BeginTeach();
    }

    public void NextStep() 
    {
        mStep++;
        switch( mStep)
        {
            case eStep.Talk:
                 mRefUIStoryTalk.AddTalkR( 463,NextStep);
                break;
            case eStep.Talk0:
                mRefUIStoryTalk.AddTalkL( 464,NextStep);
                break;
            case eStep.Talk1:
               mRefUIStoryTalk.AddTalkR( 465,NextStep);
                break;
            case eStep.Talk2:
                mRefUIStoryTalk.AddTalkL( 466,NextStep);
                break;
            case eStep.Talk3:
                mRefUIStoryTalk.AddTalkR( 467,NextStep);
                break;
            case eStep.BossSkill:
                mRefUIStoryTalk.gameObject.SetActive(false);
                mBoss.PlayKill(NextStep);
                break;
            case eStep.PlayerDie:
               gDefine.gPcDSword.PlayAct("die");
               mStepT = 1.5f;
                break;
            case eStep.Talk4:
                mRefUIStoryTalk.ClearAll();
                mRefUIStoryTalk.AddTalkR( 468,NextStep);
                break;
            case eStep.Talk5:
                mRefUIStoryTalk.AddTalkR( 469,NextStep);
                break;

            case eStep.BossGo:
                mRefUIStoryTalk.gameObject.SetActive(false);
                mBoss.PlayGo(NextStep);
                break;
            
        case eStep.ScreenToBlackToLight:
                mBlckMask.PlayToBlackToDisapper(NextStep);
                Camera.main.GetComponent<camfollow>().BeginStoryMoveToPlayer();
                break;
        case eStep.SpriteTalk0:
                mRefUIStoryTalk.ClearAll();
            mRefUIStoryTalk.AddTalkGhost( 470,NextStep);
                break;
        case eStep.SpriteTalk1:
            mRefUIStoryTalk.AddTalkGhost( 471,NextStep);
                break;
        case eStep.PlayerCure:
            mRefUIStoryTalk.gameObject.SetActive(false);
            mStepT = 2;
            PlayerCureSEToPlayer();
             break;
        case eStep.PlayerRelive:
             gDefine.gPcDSword.PlayAct("DSGirl_Act_Idle");
             mStepT = 2f;
             break;
        case eStep.PlayerTalk0:
            mRefUIStoryTalk.ClearAll();
            mRefUIStoryTalk.AddTalkL( 472,NextStep);
                break;
        case eStep.PlayerTalk1:
            mRefUIStoryTalk.AddTalkL( 473,NextStep);
                break;
        case eStep.End:
            EndStory();
                break;
        }
    }

    void Update()
    {
        if(mStepT>0)
        {
            mStepT -=Time.deltaTime;
            if(mStepT<0)
                NextStep();
        }
    }

    public void  PlayerCureSEToPlayer()
    {
        GameObject o = GameObject.Instantiate(gDefine.gData.mLifeSpringSEPreb);

        Transform t = gDefine.GetPcRefMid();

        se_Skill_LifeSpring script = o.GetComponent<se_Skill_LifeSpring>();

        script.Init(gDefine.GetPcRefMid().transform);
    }

}
