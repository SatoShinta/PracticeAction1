using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> :
    Dictionary<TKey, TValue>,
    ISerializationCallbackReceiver
{
    [Serializable]
    public class Pair
    {
        public TKey key = default;
        public TValue value = default;

        /// <summary>
        /// Pairのコンストラクタ
        /// </summary>
        public Pair(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }

    // Pair型のListを作成することにより、KeyとValueの設定を行えるListを作成できる
    [SerializeField]
    private List<Pair> _list = null;

    /// <summary>
    /// OnAfterDeserialize
    /// </summary>
    void ISerializationCallbackReceiver.OnAfterDeserialize()
    {
        Clear();
        foreach (Pair pair in _list)
        {
            if (ContainsKey(pair.key))
            {
                continue;
            }
            Add(pair.key, pair.value);
        }
    }

    /// <summary>
    /// OnBeforeSerialize
    /// </summary>
    void ISerializationCallbackReceiver.OnBeforeSerialize()
    {
        // 処理なし
    }
}