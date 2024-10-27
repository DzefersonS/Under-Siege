using System;
using UnityEngine;

namespace Utils
{
    public abstract class Poolable : MonoBehaviour
    {
        public Action<Poolable> freeToPoolCallback { set; private get; }

        public virtual void FreeToPool()
        {
            freeToPoolCallback.Invoke(this);
            gameObject.SetActive(false);
        }

        public virtual void Initialize()
        {

        }
    }
}