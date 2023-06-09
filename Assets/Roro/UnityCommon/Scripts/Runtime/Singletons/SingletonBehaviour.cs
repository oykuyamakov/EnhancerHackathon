﻿using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UnityCommon.Singletons
{
	public static class SingletonBehaviour
	{
		public static T GetInstance<T>() where T : Behaviour
		{
			var instance = Object.FindObjectOfType<T>();

			if (instance == null)
			{
				var obj = new GameObject(typeof(T) + "_Instance");
				Object.DontDestroyOnLoad(obj);
				instance = obj.AddComponent<T>();
			}

			return instance;
		}
	}

	public abstract class SingletonBehaviour<T> : SerializedMonoBehaviour where T : SingletonBehaviour<T>
	{
		private static bool didCacheInstance = false;
		protected static T instance;

		protected static T I => Instance;
		public static T Instance
		{
			get
			{
				if (!didCacheInstance)
				{
					if (instance == null)
					{
						var instances = FindObjectsOfType<T>();
						instance = instances.FirstOrDefault(i => i.wasInstance) ?? instances.FirstOrDefault();
						if (instance == null)
						{
							Debug.Log("Instance of type " + typeof(T) + " does not exist, creating new");

							CreateSingletonInstance();
						}
					}

					didCacheInstance = true;
				}

				return instance;
			}

			protected set { instance = value; }
		}

		public static void CreateSingletonInstance()
		{
			var obj = new GameObject(typeof(T).Name + "_Instance");

			if (Application.isPlaying)
			{
				DontDestroyOnLoad(obj);
			}

			//obj.hideFlags = HideFlags.HideInHierarchy;
			instance = obj.AddComponent<T>();
		}

		public static bool HasInstance() => instance;

		private bool wasInstance;


		protected bool SetupInstance(bool persistOnLoad = true)
		{
			if (instance != null && instance != this)
			{
				// Another instance present

				Debug.Log($"An instance of type {this.GetType()} already exists. Destroying duplicate.");

				Destroy(this.gameObject);
				return false;
			}


			instance = (T) this;
			wasInstance = true;
			if (persistOnLoad)
			{
				DontDestroyOnLoad(this.gameObject);
			}

			return true;
		}
	}
}
