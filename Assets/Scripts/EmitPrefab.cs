using System.Collections.Generic;
using UnityEngine;

public class EmitPrefab<T> : MonoBehaviour where T : Component {
    public T Prefab;
    public Vector3 Position = Vector3.zero;
    public Quaternion Rotation = Quaternion.identity;
    public Transform Parent;
    public int PoolSize = 0;
    public bool WarmUpOnStart = true;

    private Pool<T> _pool;

    void Awake () {
        var poolSize = 0 < PoolSize ? PoolSize : (int?) null;
        _pool = new Pool<T>(poolSize) {
            Create = () => UnityEngine.Object.Instantiate(
                original: Prefab,
                position: Position,
                rotation: Rotation,
                parent: Parent.transform
            ),
            Delete = obj => Destroy(obj.gameObject),
            Smudge = obj => { obj.gameObject.SetActive(true); Smudge(obj); },
            Clean = obj => { Clean(obj); obj.gameObject.SetActive(false); },
        };

        if (WarmUpOnStart) {
            var objects = new List<T>();
            for (var i = 0; i < PoolSize; i++) {
                objects.Add(Acquire());
            }
            foreach (var obj in objects) {
                Release(obj);
            }
        }
    }

    public virtual void Smudge (T obj) {} 

    public virtual void Clean (T obj) {} 

    public T Acquire (
        Vector3? position = null,
        Quaternion? rotation = null,
        Transform parent = null
    ) {
        var obj = _pool.Acquire();
        obj.transform.position = position ?? Position;
        obj.transform.rotation = rotation ?? Rotation;
        obj.transform.SetParent(parent ?? Parent);
        return obj;
    }

    public void Release (T obj) {
        _pool.Release(obj);
    }
}
