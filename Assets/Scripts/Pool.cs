using System;
using System.Linq;
using System.Collections.Generic;

public class Pool<T> {
    public Func<T> Create;
    public Action<T> Delete;
    public Action<T> Smudge = t => {}; 
    public Action<T> Clean  = t => {}; 

    private int? _hotSize;
    private Stack<T> _objects;

    public Pool (int? hotSize = null) {
        _hotSize = hotSize;
        _objects = new Stack<T>();
    }

    public T Acquire () {
        if (0 < _objects.Count()) {
            var pooledObject = _objects.Pop();
            Smudge(pooledObject);
            return pooledObject;
        } else {
            return Create();
        }
    }

    public void Release (T pooledObject) {
        if (_hotSize != null && _hotSize <= _objects.Count) {
            Delete(pooledObject);
        } else {
            Clean(pooledObject);
            _objects.Push(pooledObject);
        }
    }
}
