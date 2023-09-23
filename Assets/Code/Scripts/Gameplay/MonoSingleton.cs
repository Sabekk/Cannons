using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	static T _instance;
	public static T Instance {
		get {
			return _instance;
		}
		set {
			_instance = value;
		}
	}

	protected virtual void Awake () {
		{
			if (_instance == null)
				_instance = this as T;
			else {
				Debug.LogError ("Instance exist!", this);
				return;
			}
		}
	}
}
