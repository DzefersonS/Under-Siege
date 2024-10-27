using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    [CreateAssetMenu(fileName = "SOEvent", menuName = "Utils/Create New SO Event")]
    public class SOEvent : ScriptableObject
    {
        protected List<Action> m_Callbacks = new List<Action>();

        public void Register(Action callback)
        {
            m_Callbacks.Add(callback);
        }

        public void Unregister(Action callback)
        {
            m_Callbacks.Remove(callback);
        }

        public void Raise()
        {
            foreach (Action action in m_Callbacks)
            {
                action();
            }
        }
    }
}