using System;

namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    /// <summary>
    ///  1 - higher priority
    ///  0 - same priority
    /// -1 - lower priority
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex { get; set; }

    }
}