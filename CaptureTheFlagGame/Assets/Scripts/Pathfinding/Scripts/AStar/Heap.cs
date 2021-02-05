namespace Assets.Scripts.Pathfinding.Scripts.AStar
{
    public class Heap<T> where T : IHeapItem<T>
    {
        /// <summary>
        /// List that will contain all the heap items
        /// </summary>
        private readonly T[] _items;
        /// <summary>
        /// Current item count.
        /// </summary>
        private int _count;
        /// <summary>
        /// Returns the current item count.
        /// </summary>
        public int Count { get { return _count; } }

        public Heap(int maxSize)
        {
            _items = new T[maxSize];
        }

        /// <summary>
        /// Add a new item to the heap
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            // add the item to the end of the heap
            item.HeapIndex = _count;
            _items[_count]= item;
            // sort the item to its place
            SortUp(item);
            _count++;
        }

        /// <summary>
        /// Removes and returns the head of the Heap.
        /// </summary>
        /// <returns></returns>
        public T RemoveFirst()
        {
            var firstItem = _items[0];
            _count--;
            // places the last item of the heap in the first position
            _items[0] = _items[_count];
            _items[0].HeapIndex = 0;
            // sort the head item down to its correct position
            SortDown(_items[0]);
            return firstItem;
        }

        /// <summary>
        /// Update the item to its correct position
        /// </summary>
        public void UpdateItem(T item)
        {
            SortUp(item);
            //SortDown(item); // this never happens in the path finding scenario, uncomment if you want to use this heap for other scenarios than pathfinding
        }

        /// <summary>
        /// Check if the heap contains the item received in the parameters
        /// </summary>
        public bool Contains(T item)
        {
            return Equals(_items[item.HeapIndex], item);
        }

        /// <summary>
        /// Sorts the head item down the heap, to its correct position.
        /// </summary>
        /// <param name="item"></param>
        private void SortDown(T item)
        {
            while (true)
            {
                var childIndexLeft = item.HeapIndex * 2 + 1; // left child on the binary tree
                var childIndexRight = item.HeapIndex * 2 + 2; // right child on the binary tree

                // check if left child index is still inside the heap
                if (childIndexLeft < _count) 
                {
                    var swapIndex = childIndexLeft; // set the left child as default swap
                    // check if right child is inside the heap and has an index lower than the right index
                    if (childIndexRight < _count && _items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        swapIndex = childIndexRight; // swap with the right
                    if (item.CompareTo(_items[swapIndex]) < 0) // if the item being sorted down has a higher index than the item in the swap index position, then swap those items.
                        Swap(item, _items[swapIndex]);
                    else
                        return;
                }
                else
                    return;
            }
        }

        /// <summary>
        /// Sorts the item up the heap to its correct position
        /// </summary>
        /// <param name="item"></param>
        private void SortUp(T item)
        {
            // get this item parent item index
            var parentIndex = (item.HeapIndex - 1) / 2;

            while (true)
            {
                // get the parent item
                var parantItem = _items[parentIndex];

                //  1 - higher priority
                //  0 - same priority
                // -1 - lower priority
                if (item.CompareTo(parantItem) > 0)
                    Swap(item, parantItem);
                else
                    break; // exit the loop

                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Swaps two items in the heap
        /// </summary>
        private void Swap(T itemA, T itemB)
        {
            // swap the items
            _items[itemA.HeapIndex] = itemB;
            _items[itemB.HeapIndex] = itemA;

            // swap the indexes
            var itemAIndex = itemA.HeapIndex;
            itemA.HeapIndex = itemB.HeapIndex;
            itemB.HeapIndex = itemAIndex;
        }
    }
}
