using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoSingleton<ObjectPool> {
	Dictionary<string, PoolInstance> poolDictionary;
	Dictionary<string, Transform> poolCategory;
	public List<PoolInstancee> poolInstance;

	Transform mainPoolParent;

	protected override void Awake () {
		base.Awake ();

		poolDictionary = new Dictionary<string, PoolInstance> ();
		poolCategory = new Dictionary<string, Transform> ();

		InitializePools ();
	}

	void InitializePools () {
		poolDictionary = new Dictionary<string, PoolInstance> ();
		foreach (var poolInstance in poolInstance) {
			Transform parent = GetCategoryParent (poolInstance.name);
			poolDictionary[poolInstance.name] = new PoolInstance (poolInstance.name, poolInstance.prefab, poolInstance.size, parent);
		}
	}

	Transform GetCategoryParent (string name) {
		if (mainPoolParent == null) {
			mainPoolParent = new GameObject ("Pools").transform;
			mainPoolParent.SetParent (transform);
		}
		string category = name.Substring (0, name.IndexOf ('_'));
		if (poolCategory.ContainsKey (category))
			return poolCategory[category];
		else {
			Transform categoryParent = new GameObject (category).transform;
			categoryParent.SetParent (mainPoolParent);
			poolCategory.Add (category, categoryParent);
			return categoryParent;
		}
	}

	public PoolObject GetFromPool (string tag) {
		PoolInstance instance = null;
		poolDictionary.TryGetValue (tag, out instance);

		if (instance != null)
			return instance.GetFromPool ();
		else {
			Debug.LogError ("Pool cannot be found");
			return null;
		}
	}

	public void ReturnToPool (IPoolable poolableObject) {
		PoolObject poolObj = poolableObject.Poolable;
		PoolInstance instance = null;
		poolDictionary.TryGetValue (poolObj.name, out instance);

		if (instance != null)
			instance.ReturnToPool (poolObj);
		else {
			Debug.LogError ("Object is not from pool", poolObj.prefab);
		}
	}

	public void GetAllPoolsOfType(string type, ref List<string> pools) {
		foreach (var poolDictionary in poolDictionary) {
			if (!poolDictionary.Key.Contains (type))
				continue;
			pools.Add (poolDictionary.Key);
		}
	}

	public class PoolObject {
		public string name;
		public GameObject prefab;
		public Dictionary<Type, Component> components;
		public bool taken;

		public PoolObject (string name, GameObject prefab) {
			this.name = name;
			this.prefab = prefab;

			components = new Dictionary<Type, Component> ();
			Component[] componentsTmp = prefab.GetComponents (typeof (Component));
			foreach (var compTmp in componentsTmp) {
				if (compTmp is IPoolable poolableComp)
					poolableComp.AssignPoolable (this);
				components[compTmp.GetType ()] = compTmp;
			}
		}

		public Component GetComponent (Type type) {
			Component component = null;
			components.TryGetValue (type, out component);
			if (!component)
				foreach (var comp in components) {
					if (type.IsAssignableFrom (comp.Key))
						return comp.Value;
				}
			return component;
		}

		public T GetComponent<T> () where T : Component {
			return GetComponent (typeof (T)) as T;
		}
	}

	public class PoolInstance {
		string name;
		GameObject prefab;
		Transform parent;
		Stack<PoolObject> objects;
		List<PoolObject> taken;

		public PoolInstance (string name, GameObject prefab, int initCount, Transform parent) {
			this.name = name;
			this.prefab = prefab;
			objects = new Stack<PoolObject> (initCount);
			taken = new List<PoolObject> (initCount);

			this.parent = new GameObject (name).transform;
			this.parent.SetParent (parent);

			for (int i = 0; i < initCount; i++) {
				AddToPool ();
			}
		}

		public PoolObject GetFromPool () {
			if (objects.Count == 0)
				AddToPool ();
			PoolObject poolObject = objects.Pop ();
			poolObject.taken = true;
			poolObject.prefab.SetActive (true);
			taken.Add (poolObject);
			return poolObject;
		}

		public void ReturnToPool (PoolObject poolObject) {
			objects.Push (poolObject);
			poolObject.taken = false;
			poolObject.prefab.SetActive (false);
			poolObject.prefab.transform.SetParent (parent);

			poolObject.prefab.transform.position = Vector3.zero;
			poolObject.prefab.transform.rotation = Quaternion.identity;
			poolObject.prefab.transform.localScale = Vector3.one;

			taken.Remove (poolObject);
		}

		void AddToPool () {
			GameObject newPoolObject = GameObject.Instantiate (prefab);
			newPoolObject.name = newPoolObject.name + "_Pool";
			ReturnToPool (new PoolObject (name, newPoolObject));
		}
	}

	[System.Serializable]
	public struct PoolInstancee {
		public string name;
		public GameObject prefab;
		public int size;
	}

	public interface IPoolable {
		PoolObject Poolable { get; }
		void AssignPoolable (PoolObject poolable);
	}
}
