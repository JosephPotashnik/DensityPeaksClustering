using System;
using System.Collections.Generic;
using System.Linq;

//adapted from https://rosettacode.org/wiki/Quickselect_algorithm

namespace DensityPeaksClustering
{
    internal static class ArrayExtension
    {
        public static double StandardDeviation(this IEnumerable<double> values)
        {
            var avg = values.Average();
            return Math.Sqrt(values.Average(v => Math.Pow(v - avg, 2)));
        }

        /// <summary>
        ///     Partially sort array such way that elements before index position n are smaller or equal than elemnt at position n.
        ///     And elements after n are larger or equal.
        /// </summary>
        /// <typeparam name="T">The type of the elements of array. Type must implement IComparable(T) interface.</typeparam>
        /// <param name="input">The array which elements are being partially sorted. This array is not modified.</param>
        /// <param name="n">Nth smallest element.</param>
        /// <returns>Partially sorted array.</returns>
        public static T[] QuickSelectSmallest<T>(T[] input, int n) where T : IComparable<T>
        {
            var partiallySortedArray = input;

            // Initially we are going to execute quick select to entire array
            var startIndex = 0;
            var endIndex = input.Length - 1;

            // Selecting initial pivot
            var r = new Random();
            var pivotIndex = r.Next(startIndex, endIndex);

            // Loop until there is nothing to loop (this actually shouldn't happen - we should find our value before we run out of values)
            while (endIndex > startIndex)
            {
                pivotIndex = QuickSelectPartition(partiallySortedArray, startIndex, endIndex, pivotIndex);
                if (pivotIndex == n)
                    // We found our n:th smallest value - it is stored to pivot index
                    break;
                if (pivotIndex > n)
                    // Array before our pivot index have more elements that we are looking for                    
                    endIndex = pivotIndex - 1;
                else
                    // Array before our pivot index has less elements that we are looking for                    
                    startIndex = pivotIndex + 1;

                // Omnipotent beings don't need to roll dices - but we do...
                // Randomly select a new pivot index between end and start indexes (there are other methods, this is just most brutal and simplest)
                pivotIndex = r.Next(startIndex, endIndex);
            }

            return partiallySortedArray;
        }

        /// <summary>
        ///     Sort elements in sub array between startIndex and endIndex, such way that elements smaller than or equal with value
        ///     initially stored to pivot index are before
        ///     new returned pivot value index.
        /// </summary>
        /// <typeparam name="T">The type of the elements of array. Type must implement IComparable(T) interface.</typeparam>
        /// <param name="array">The array that is being sorted.</param>
        /// <param name="startIndex">Start index of sub array.</param>
        /// <param name="endIndex">End index of sub array.</param>
        /// <param name="pivotIndex">Pivot index.</param>
        /// <returns>
        ///     New pivot index. Value that was initially stored to <paramref name="pivotIndex" /> is stored to this newly returned
        ///     index. All elements before this index are
        ///     either smaller or equal with pivot value. All elements after this index are larger than pivot value.
        /// </returns>
        /// <remarks>This method modifies paremater array.</remarks>
        private static int QuickSelectPartition<T>(this T[] array, int startIndex, int endIndex, int pivotIndex)
            where T : IComparable<T>
        {
            var pivotValue = array[pivotIndex];
            // Initially we just assume that value in pivot index is largest - so we move it to end (makes also for loop more straight forward)
            array.Swap(pivotIndex, endIndex);
            for (var i = startIndex; i < endIndex; i++)
            {
                if (array[i].CompareTo(pivotValue) > 0)
                    continue;

                // Value stored to i was smaller than or equal with pivot value - let's move it to start
                array.Swap(i, startIndex);
                // Move start one index forward 
                startIndex++;
            }

            // Start index is now pointing to index where we should store our pivot value from end of array
            array.Swap(endIndex, startIndex);
            return startIndex;
        }

        private static void Swap<T>(this T[] array, int index1, int index2)
        {
            if (index1 == index2)
                return;

            var temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}