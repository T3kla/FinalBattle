using System;
using UnityEngine;

[Serializable]
public class Field<T> : ISerializationCallbackReceiver
{

    [SerializeField] private T initial;
    public T runtime;

    public void OnBeforeSerialize() { }
    public void OnAfterDeserialize() => runtime = initial;

}