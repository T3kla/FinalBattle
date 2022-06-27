using System;
using System.Threading.Tasks;
using UnityEngine;

namespace TBox
{

    [Serializable]
    public class Field<T> : ISerializationCallbackReceiver
    {

        [SerializeField] private T initial;
        [SerializeField] public T value;

        public virtual void OnBeforeSerialize() { }
        public virtual void OnAfterDeserialize() => value = initial;

        public Field(T value)
        {
            this.initial = value;
            this.value = value;
        }
    }

}
