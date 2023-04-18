using UnityEngine;

namespace HoMa.Sudoku.Framework
{
    /// <summary>
    /// (Optionally) Persistent singleton pattern implementation for unique globally accessible unity Component.
    /// </summary>
    /// <typeparam name="T">Any Unity Component, originally intended for scripts.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<T>();

                    if (m_Instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        m_Instance = obj.AddComponent<T>();
                    }
                }

                return m_Instance;
            }
        }

        /// <summary>
        /// Delete any other instances of this type T singleton on Awake in order to preserve it's uniqueness.
        /// </summary>
        /// <param name="persist">true: The singleton Component and GameObject will persist through scene loads. false: will not persist as described.</param>
        protected virtual void Awake(bool persist = false)
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;

                if (persist)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Cleanup the singleton instance if the it's gameObject is destroyed.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (m_Instance == this)
            {
                m_Instance = null;
            }
        }
    }
}