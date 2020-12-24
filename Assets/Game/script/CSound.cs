using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CSound : MonoBehaviour
{
	AudioSource source_back;
	public ArrayList mSource = new ArrayList();
	public ArrayList mCurSource = new ArrayList();

	public bool mUseSound = true;
    public bool mUseMusic = true;

    public float mCoolDownTime = 0;
	
	public bool mIsFadeClose = false;
	public bool mIsFadeOpen = false;
	public float mFadeTime = 0.0f;
	public float mFadeLastTime = 1.0f;

	public float mFadeBeginVolume = 0.0f;
	public float mFadeEndVolume = 0.0f;

    int mCurMusic = -1;

    static bool mExist = false;

	List <AudioSource> mLoopDict = new List<AudioSource>();

	float mStartSleepT = 0;

	

	public void SetMusicVolume(float v)
	{
		if( source_back == null )
			return;

		source_back.volume = v;
	}


	public void FadeCloseMusic( float lastTime = 1.0f )
	{
		if(source_back != null && source_back.isPlaying)
		{
			mFadeBeginVolume = source_back.volume;
			mFadeEndVolume = 0.0f;
			mIsFadeClose = true;
			mFadeTime = 0.0f;
			mFadeLastTime = lastTime;
		}
	}

	public void FadeOpenMusic( float lastTime = 1.0f )
	{
		if (!mUseSound)
			return;

		if(source_back != null && source_back.isPlaying)
		{
			//mFadeEndVolume = source_back.volume;
			mFadeEndVolume = 0.8f;
			mFadeBeginVolume = 0.0f;
			mIsFadeOpen = true;
			mFadeTime = 0.0f;
			mFadeLastTime = lastTime;
		}
	}

	public void PlayMusic ( AudioClip c, bool loop, bool fadeOpen = false, float delay=0.0f )
	{
		if (!mUseSound)
			return;
		if(Time.time < mStartSleepT)
			return;

		if(source_back == null)
			source_back = gameObject.AddComponent<AudioSource> ();
		{
			source_back.clip = c;
			source_back.volume = 0.0f;
			source_back.loop = loop;
			source_back.PlayDelayed (delay);
		}
        if (fadeOpen)
        {
            FadeOpenMusic();
        }
        else
            fadeOpen = false;
	}

	public bool IsMusicPlaying()
	{
		if(source_back!=null)
			return source_back.isPlaying;
		else
			return false;
	}

	public void StopMusic()
	{
		if(source_back != null)
			source_back.Stop ();
	}

	public void EnableMusic( bool enable )
	{
        if (mUseMusic != enable)
        {
            mUseMusic = enable;
            MuteMusic(!enable);
        }
	}

    void MuteMusic( bool isMute)
    {
        if (isMute)
            source_back.mute = true;
        else
            source_back.mute = false;
    }

    public void EnableSound( bool enable)
    {
        mUseSound = enable;
    }


	// Use this for initialization
	void Start () 
	{
        if( mExist )
        {
            mUseSound = false;
            mUseMusic = false;

            GameObject.Destroy(gameObject);

            return;
        }

		mStartSleepT = Time.time + 1;

    	GameObject.DontDestroyOnLoad(gameObject);
        gDefine.gSound = this;
        mExist = true;

		source_back = gameObject.AddComponent<AudioSource> ();
		//source_back.mute = true;
		mUseMusic = true;

    }

	AudioSource Get()
	{
		if (mSource.Count > 0)
		{
			AudioSource s = (AudioSource)mSource [mSource.Count - 1];
			mSource.RemoveAt (mSource.Count - 1);
			return s;
		} 
		else 
		{
			AudioSource s = gameObject.AddComponent<AudioSource> ();
			return s;
		}
	}

	public void Play(  AudioClip c, float delay = 0.0f, float volume = 0.8f,float begintime = 0.0f)
	{
		if( !mUseSound)
			return;
		
		if(Time.time <= mStartSleepT)
			return;

		AudioSource s = Get();
		s.Stop();
		s.clip = c;
		s.volume = volume;
		s.loop = false;
		s.mute = false;
		
		if( delay > 0.0f)
			s.PlayDelayed(delay);
		else
			s.Play();

		mCurSource.Add(s);

	}

	public int PlayLoop(  AudioClip c, float delay = 0.0f, float volume = 0.8f,float begintime = 0.0f)
	{
		if( !mUseSound)
			return 0;

		AudioSource s = Get();
		s.clip = c;
		s.volume = volume;
		s.loop = true;
		s.transform.position = Camera.main.transform.position;
        

		if( delay > 0.0f)
			s.PlayDelayed(delay);
		else
			s.Play();

		mLoopDict.Add(s);

		return s.GetHashCode();
	}

	public void EndSoundSE(int HashCode)
	{
		for(int i=0; i<mLoopDict.Count; i++)
		{
			if( mLoopDict[i].GetHashCode() == HashCode )
			{
				mLoopDict[i].Stop();
				mSource.Add(mLoopDict[i]);
				mLoopDict.RemoveAt(i);
				break;
			}
		}
	}


	void CleanAllSound()
	{
		for (int i=0; i<mCurSource.Count; i++) 
		{
			AudioSource s = (AudioSource)mCurSource [i];
			s.Stop();
			mSource.Add(s);
		}
		mCurSource.Clear ();
	}

	// Update is called once per frame
	void Update () 
	{
		if( !mUseSound)
			return;

		if( (mIsFadeOpen || mIsFadeClose) && source_back!=null)
		{
			mFadeTime += Time.deltaTime;
			if( mFadeTime >= mFadeLastTime )
			{
				if( mIsFadeClose )
					source_back.Stop();
				else if( mIsFadeOpen )
					source_back.volume = mFadeEndVolume;
				mIsFadeOpen = false;
				mIsFadeClose = false;
			}
			else
			{
				float perc = mFadeTime / mFadeLastTime;
				source_back.volume = mFadeBeginVolume +( mFadeEndVolume - mFadeBeginVolume)* perc;
			}
		}

        source_back.transform.position = Camera.main.transform.position;

        for (int i=0; i<mCurSource.Count; i++) 
		{
			AudioSource s = (AudioSource)mCurSource [i];
			if( !s.isPlaying )
			{
				mCurSource.RemoveAt(i);
				i--;
				mSource.Add(s);
			}
		}
        
	}

}
