using System;
using UnityEngine;

namespace TBox
{

    [Serializable]
    public class Field<T> : ISerializationCallbackReceiver
    {

        [SerializeField] private T initial;
        [SerializeField] public T value;

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => value = initial;

    }

}
