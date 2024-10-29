using UnityEngine;
using System;

namespace Utils
{
    [CreateAssetMenu(fileName = "ObjectPoolSO", menuName = "Utils/Create Object Pool SO")]
    public class ObjectPoolSO : ScriptableObject
    {
        [SerializeField] private Poolable m_PoolablePrefab = default;
        [SerializeField] private int m_BufferSize = 20;

        private Poolable[] m_Objects = new Poolable[0];
        private Transform m_Container = default;
        private int m_ActiveObjectCount = 0;
        private int m_BufferIndex = 0;

        public Poolable prefab => m_PoolablePrefab;
        public Poolable[] activeObjects => m_Objects;
        public Transform container { set => m_Container = value; }
        public int activeObjectsCount => m_ActiveObjectCount;

        public Poolable GetFreeObject()
        {
            if (m_Objects == null || m_ActiveObjectCount == m_Objects.Length)
                Expand();

            var obj = m_Objects[m_ActiveObjectCount++];
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void FreeAll()
        {
            if (m_Objects == null)
                return;

            for (int i = 0, c = m_ActiveObjectCount; i < c; ++i)
                m_Objects[i].gameObject.SetActive(false);

            m_ActiveObjectCount = 0;
        }

        public void DestroyContainer()
        {
            if (m_Objects == null)
                return;

            for (int i = 0, c = m_Objects.Length; i < c; ++i)
                Destroy(m_Objects[i]);

            m_Objects = null;
            m_BufferIndex = 0;
            m_ActiveObjectCount = 0;
        }

        public void FreeObject(Poolable objectToFree)
        {
            if (m_Objects == null)
                return;

            var index = Array.FindIndex(m_Objects, 0, m_ActiveObjectCount, (Poolable activeObj) => { return objectToFree == activeObj; });
            if (index < 0)
                return;

            var freedObj = m_Objects[index];
            m_Objects[index] = m_Objects[--m_ActiveObjectCount];
            m_Objects[m_ActiveObjectCount] = freedObj;
        }

        public void Expand()
        {
            int initialSize = m_Objects == default ? 0 : m_Objects.Length;
            int newElementCount = ++m_BufferIndex * m_BufferSize;
            Array.Resize(ref m_Objects, newElementCount);

            for (int i = initialSize; i < newElementCount; ++i)
            {
                var obj = Instantiate(m_PoolablePrefab);
                obj.transform.SetParent(m_Container);
                obj.gameObject.SetActive(false);
                //obj.transform.localScale = Vector3.one;
                obj.freeToPoolCallback = FreeObject;
                m_Objects[i] = obj;
            }
        }
    }
}