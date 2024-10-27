using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class SOTypeEvent<T> : SOEvent
    {
        private T m_Value;

        public T value
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
                Raise();
            }
        }
    }
}