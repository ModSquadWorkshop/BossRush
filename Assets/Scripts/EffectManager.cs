using UnityEngine;
using System.Collections;

//[System.Serializable]
public class EffectManager : MonoBehaviour
{
	public GameObject emitter;
	public AudioClip[] sourceSounds;
	public ParticleEmitter[] particles;
	private AudioSource _source;

	void Start()
	{
		_source = GetComponent<AudioSource>();
	}

	public void PlayRandomSound( int begin, int end, float vol, float delayed )
	{
		if ( sourceSounds.Length > 0 && _source != null )
		{
			audio.clip = sourceSounds[Random.Range( begin, end )];
			audio.volume = vol;
			if ( delayed == 0 )
			{
				audio.Play();
			}
			else
			{
				audio.PlayDelayed( delayed );
			}
		}
	}

	public void PlaySound( int index, float vol, float delayed )
	{
		if ( sourceSounds.Length > 0 && _source != null )
		{
			audio.clip = sourceSounds[index];
			audio.volume = vol;
			if ( delayed == 0 )
			{
				audio.Play();
			}
			else
			{
				audio.PlayDelayed( delayed );
			}
		}
	}

	public void PlayRandomLoopingSound( int begin, int end, float vol, float delayed )
	{
		if ( sourceSounds.Length > 0 && _source != null )
		{
			audio.clip = sourceSounds[Random.Range( begin, end )];
			audio.loop = true;
			audio.volume = vol;
			if ( delayed == 0 )
			{
				audio.Play();
			}
			else
			{
				audio.PlayDelayed( delayed );
			}
		}
	}

	public void PlayLoopingSound( int index, float vol, float delayed )
	{
		if ( sourceSounds.Length > 0 && _source != null )
		{
			audio.clip = sourceSounds[index];
			audio.loop = true;
			audio.volume = vol;
			if ( delayed == 0 )
			{
				audio.Play();
			}
			else
			{
				audio.PlayDelayed( delayed );
			}
		}
	}

	public void StopSound()
	{
		if ( _source != null && audio.isPlaying )
		{
			audio.Stop();
		}
	}
}
