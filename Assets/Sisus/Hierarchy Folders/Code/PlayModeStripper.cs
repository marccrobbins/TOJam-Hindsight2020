#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

namespace Sisus.HierarchyFolders
{
	public class PlayModeStripper
	{
		private static PlayModeStripper instance;

		private readonly HashSet<Scene> playModeStrippingHandledForScenes = new HashSet<Scene>();
		private readonly Dictionary<Scene, HashSet<Transform>> playModeStrippingHandledForSceneRootObjects = new Dictionary<Scene, HashSet<Transform>>(1);
		private readonly StrippingType playModeStripping = StrippingType.None;
		private readonly PlayModeStrippingMethod playModeStrippingMethod = PlayModeStrippingMethod.EntireSceneImmediate;

		public PlayModeStripper(StrippingType setStrippingType, PlayModeStrippingMethod setStrippingMethod)
		{
			playModeStripping = setStrippingType;
			playModeStrippingMethod = setStrippingMethod;

			if(playModeStripping == StrippingType.None)
			{
				return;
			}

			SceneManager.sceneLoaded += OnSceneLoaded;
			SceneManager.sceneUnloaded += OnSceneUnloaded;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			for(int s = 0, scount = SceneManager.sceneCount; s < scount; s++)
			{
				var scene = SceneManager.GetSceneAt(s);
				if(scene.isLoaded && playModeStrippingHandledForScenes.Add(scene))
				{
					HierarchyFolderUtility.ApplyStrippingType(scene, playModeStripping);
				}
			}
		}

		private void OnPlayModeStateChanged(PlayModeStateChange playModeState)
		{
			switch(playModeState)
			{
				case PlayModeStateChange.ExitingPlayMode:
					instance = null;
					SceneManager.sceneLoaded -= OnSceneLoaded;
					SceneManager.sceneUnloaded -= OnSceneUnloaded;
					EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
					break;
			}
		}

		private static PlayModeStripper Instance()
		{
			if(instance == null)
			{
				if(!EditorApplication.isPlayingOrWillChangePlaymode)
				{
					instance = new PlayModeStripper(StrippingType.None, default(PlayModeStrippingMethod));
				}
				else
				{
					var preferences = HierarchyFolderPreferences.Get();
					instance = new PlayModeStripper(preferences.playModeBehaviour, preferences.playModeStrippingMethod);
				}
			}
			return instance;
		}

		public static void OnSceneObjectAwake(GameObject gameObject)
		{
			Instance().HandleOnSceneObjectAwake(gameObject);
		}

		private void HandleOnSceneObjectAwake(GameObject gameObject)
		{
			if(playModeStripping == StrippingType.None)
			{
				return;
			}

			#if DEV_MODE
			Debug.Assert(EditorApplication.isPlayingOrWillChangePlaymode);
			#endif

			var scene = gameObject.scene;
			if(playModeStrippingHandledForScenes.Contains(scene))
			{
				return;
			}

			switch(playModeStrippingMethod)
			{
				case PlayModeStrippingMethod.EntireSceneWhenLoaded:
					if(!scene.isLoaded)
					{
						return;
					}
					break;
				case PlayModeStrippingMethod.IndividuallyDuringAwake:
					if(!scene.isLoaded)
					{
						var rootTransform = gameObject.transform.transform.root;
						HashSet<Transform> handledRootObjects;
						if(!playModeStrippingHandledForSceneRootObjects.TryGetValue(gameObject.scene, out handledRootObjects))
						{
							playModeStrippingHandledForSceneRootObjects.Add(gameObject.scene, new HashSet<Transform>(){ rootTransform });
						}
						else if(!handledRootObjects.Add(rootTransform))
						{
							return;
						}
						HierarchyFolderUtility.CheckForAndRemoveHierarchyFoldersInChildren(gameObject.transform.root, playModeStripping);
						return;
					}
					break;
			}

			playModeStrippingHandledForScenes.Add(gameObject.scene);

			try
			{
				HierarchyFolderUtility.ApplyStrippingType(gameObject.scene, playModeStripping);
			}
			catch(System.ArgumentException e) // catch "ArgumentException: The scene is not loaded." which can occur when using PlayModeStrippingMethod.EntireSceneImmediate.
			{
				Debug.LogError("Exception encountered while stripping Hierarchy Folders from scene " + gameObject.scene.name + " for play mode using method "+ playModeStripping + ". You may need to switch to a different play mode stripping method in preferences.\n" + e);
				playModeStrippingHandledForScenes.Remove(gameObject.scene);

				#if DEV_MODE
				Debug.Assert(!HierarchyFolderUtility.NowStripping);
				#endif
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			#if DEV_MODE
			Debug.Assert(playModeStripping != StrippingType.None);
			Debug.Assert(scene.isLoaded);
			#endif

			if(!playModeStrippingHandledForScenes.Add(scene))
			{
				return;
			}

			playModeStrippingHandledForScenes.Add(scene);
			HierarchyFolderUtility.ApplyStrippingType(scene, playModeStripping);
		}

		private void OnSceneUnloaded(Scene scene)
		{
			playModeStrippingHandledForScenes.Remove(scene);
			playModeStrippingHandledForSceneRootObjects.Remove(scene);
		}
	}
}
#endif