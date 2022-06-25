using UnityEngine;
using static TBox.Logger;

namespace TBox
{

    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {

        public static Singleton<T> Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                LogWarn($"Instance already exists", typeof(T).Name);
                Destroy(gameObject);
            }
        }

    }

}
