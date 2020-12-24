using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMusic : MonoBehaviour
{
    public int[] mMusicArr = new int[] { 36, 38, 39 };
    public int[] mMainUIMusicArr = new int[] { 53 };

    int mCurMusicId = -1;


    // Update is called once per frame
    void Update()
    {
        if (gDefine.gMainUI !=null && gDefine.gMainUI.gameObject.activeSelf)
        {
            //if(!gDefine.gSound.IsMusicPlaying())
            BeginPlayUIMusic();
            //if(gDefine.gSound.IsMusicPlaying())
            //   gDefine.gSound.FadeCloseMusic(0.1f);
        }
        else
        {
            //if(!gDefine.gSound.IsMusicPlaying())
            BeginPlayFighMusic();

            // if(Input.GetMouseButtonUp(0))
            // {
            //     //gDefine.CreateSomeCoinInGame(15);
            //     //gDefine.CreateSomeCrystalInGame(15);
            //     //PlayUIClick();
            // }

        }

        // if(Input.GetMouseButtonUp(0))
        // {
        //     PlaySound(63);
        //     //PlayUIClick();
        // }

        gDefine.Update();
            
    }

    public void BeginPlayFighMusic()
    {
        if (!gDefine.gSound.IsMusicPlaying())
        {
            int index = Random.Range(0, mMusicArr.Length);
            // if(gDefine.gChapterId == 1)
            //     index = 2;
            AudioClip clip = gDefine.gData.GetSoundClip(mMusicArr[index]);
            if (clip != null)
                gDefine.gSound.PlayMusic(clip, false,true);
            mCurMusicId = mMusicArr[index];
        }
        else
        {
            for (int i = 0; i < mMusicArr.Length; i++)
                if (mCurMusicId == mMusicArr[i])
                    return;

            int index = Random.Range(0, mMusicArr.Length);
            //if(gDefine.gChapterId == 1)
               // index = 2;
            AudioClip clip = gDefine.gData.GetSoundClip(mMusicArr[index]);
            if (clip != null)
                gDefine.gSound.PlayMusic(clip, true,true);
            mCurMusicId = mMusicArr[index];
        }
    }

    public void BeginPlayUIMusic()
    {
        if (!gDefine.gSound.IsMusicPlaying() || mCurMusicId != mMainUIMusicArr[0])
        {
            int index = Random.Range(0, mMainUIMusicArr.Length);
            AudioClip clip = gDefine.gData.GetSoundClip(mMainUIMusicArr[index]);
            if (clip != null)
                gDefine.gSound.PlayMusic(clip, true,true);
            mCurMusicId = mMainUIMusicArr[index];
        }

    }

    public void PlaySound(int SoundId)
    {
        gDefine.PlaySound(SoundId);
    }

    public void PlayUIClick()
    {
        gDefine.PlaySound(24);
    }
}
