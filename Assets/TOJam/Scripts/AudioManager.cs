using System;
using System.Collections;
using System.Collections.Generic;
using StopMode = FMOD.Studio.STOP_MODE;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : Manager
{
    public static AudioManager Instance;
    
    private const string DEBUG_LOG_FORMAT = "AudioManager.{0} :: {1}";
    private const float INSTANCE_RELEASE_DELAY = 10f;

    private readonly Dictionary<string, List<EventInstance>> EventInstanceMapping = new Dictionary<string, List<EventInstance>>();

    protected override void Initialize()
    {
	    DontDestroyOnLoad(gameObject);
	    
	    Instance = this;
	    
	    base.Initialize();
    }

    #region EventInstance

		public EventInstance CreateEventInstance(string audioEventName)
		{
			// Create a new one shot event instance.
			var instance = RuntimeManager.CreateInstance(audioEventName);

			// Check if we need to replace an existing event instance, or create a new entry.
			if (EventInstanceMapping.ContainsKey(audioEventName))
			{
				// Entry exists. Find an available event instance.
				// An available instance is one that is no longer playing.
				var instances = EventInstanceMapping[audioEventName];
				var availableInstanceIndex = -1;
				for (var i = 0; i < instances.Count; i++)
				{
					var eventInstance = instances[i];
					eventInstance.getPlaybackState(out var playbackState);
					if (playbackState != PLAYBACK_STATE.STOPPED) continue;
					availableInstanceIndex = i;
					break;
				}
				if (availableInstanceIndex == -1)
				{
					EventInstanceMapping[audioEventName].Add(instance);
				}
				else
				{
					EventInstanceMapping[audioEventName][availableInstanceIndex] = instance;
				}
			}
			else
			{
				// No entry exists. Create a new entry.
				EventInstanceMapping.Add(audioEventName, new List<EventInstance> { instance });
			}

			return instance;
		}

		public EventInstance GetExistingInstance(string audioEventName)
		{
			if (EventInstanceMapping.ContainsKey(audioEventName))
			{
				return EventInstanceMapping[audioEventName][0];
			}
			
			return CreateEventInstance(audioEventName);
		}
		
		public List<EventInstance> GetExistingInstances(string audioEventName)
		{
			if (EventInstanceMapping.ContainsKey(audioEventName))
			{
				return EventInstanceMapping[audioEventName];
			}

			return new List<EventInstance>{ CreateEventInstance(audioEventName) };
		}
		
		public void DeleteEventMap(string audioEventName)
		{
			if (!EventInstanceMapping.ContainsKey(audioEventName)) return;
			EventInstanceMapping.Remove(audioEventName);
		}
		
		#endregion EventInstance

	#region PersistentAudio

		public virtual void PlayPersistentAudio(AudioData data)
		{
			const string logMethod = "PlayPersistentAudio";
			if (string.IsNullOrEmpty(data.AudioEventName))
			{
				Debug.LogErrorFormat(DEBUG_LOG_FORMAT, logMethod, "Audio event name is not provided.");
				return;
			}

			var instance = GetExistingInstance(data.AudioEventName);

			if (!instance.isValid()) return;

			RuntimeManager.AttachInstanceToGameObject(
				instance,
				data.AttachPoint ? data.AttachPoint.transform : transform,
				data.AttachPoint ? data.AttachPoint.GetComponent<Rigidbody>() : GetComponent<Rigidbody>());

			if (data.ParameterCollection == null) return;
			foreach (var parameter in data.ParameterCollection)
			{
				if(string.IsNullOrEmpty(parameter.Key)) continue;
				instance.setParameterByName(parameter.Key, parameter.Value);
			}
			
			instance.start();
		}

		public virtual void ChangePersistentAudio(AudioData data)
		{
			const string logMethod = "ChangePersistentAudio";
			if (string.IsNullOrEmpty(data.AudioEventName))
			{
				Debug.LogErrorFormat(DEBUG_LOG_FORMAT, logMethod, "Audio event name is not provided.");
				return;
			}
			
			var instance = GetExistingInstance(data.AudioEventName);

			if (!instance.isValid()) return;

			//Start playing the audio if it isn't playing
			instance.getPlaybackState(out var playbackState);
			if (playbackState == PLAYBACK_STATE.STOPPED) instance.start();

			if (data.ParameterCollection == null) return;
			foreach (var parameter in data.ParameterCollection)
			{
				if(string.IsNullOrEmpty(parameter.Key)) continue;
				instance.setParameterByName(parameter.Key, parameter.Value);
			}
		}

		public virtual void StopPersistentAudio(string audioEventName, StopMode stopMode = StopMode.IMMEDIATE)
		{
			const string logMethod = "StopPersistentAudio";
			if (string.IsNullOrEmpty(audioEventName))
			{
				Debug.LogErrorFormat(DEBUG_LOG_FORMAT, logMethod, "Audio event name is not provided.");
				return;
			}
			
			var instance = GetExistingInstance(audioEventName);

			if (!instance.isValid()) return;

			instance.stop(stopMode);
			StartCoroutine(StopAudioRoutine(instance, stopMode));
		}

		private IEnumerator StopAudioRoutine(EventInstance instance, StopMode stopMode)
		{
			yield return new WaitForSeconds(INSTANCE_RELEASE_DELAY);
			
			if (!instance.isValid()) yield break;
			instance.stop(stopMode);
		}
		
		#endregion PersistentAudio

	#region OneShot

		public virtual void PlayOneShot(string audioEventName, GameObject attachPoint = null)
		{
			const string logMethod = "PlayOneShot";
			if (string.IsNullOrEmpty(audioEventName))
			{
				Debug.LogErrorFormat(DEBUG_LOG_FORMAT, logMethod, "Audio event name is not provided.");
				return;
			}
			
			var instance = CreateEventInstance(audioEventName);

			if (attachPoint != null)
			{
				RuntimeManager.AttachInstanceToGameObject(instance, attachPoint.transform, attachPoint.GetComponent<Rigidbody>());
			}
			
			instance.start();
			instance.release();
		}
		
		#endregion OneShot
}

[Serializable]
public class AudioData
{
	public GameObject AttachPoint;
	public string AudioEventName;
	public Dictionary<string, float> ParameterCollection;
}
