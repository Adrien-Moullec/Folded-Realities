using System;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public struct NetworkPlayerData : INetworkSerializable, IEquatable<NetworkPlayerData> {
    public ulong ClientId;
    public FixedString32Bytes Name;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref Name);
    }

    public bool Equals(NetworkPlayerData other) {
        return other.ClientId == ClientId &&
            other.Name == Name;
    }
}