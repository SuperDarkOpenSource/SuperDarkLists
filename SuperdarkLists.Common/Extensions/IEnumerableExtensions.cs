using System;
using System.Collections.Generic;
using System.Linq;

namespace SuperdarkLists.Common.Extensions;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> EmptyIfNull<T>(IEnumerable<T>? enumerable)
    {
        if (enumerable is null)
        {
            return Enumerable.Empty<T>();
        }

        return enumerable;
    }

    public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> enumerable, int partitions)
    {
        if (partitions < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(partitions), "Cannot partition something into negative or zero parts");
        }
        
        List<T> partition = new();

        foreach (var item in enumerable)
        {
            partition.Add(item);
            
            if (partition.Count == partitions)
            {
                yield return partition;

                partition = new();
            }
        }

        if (partition.Any())
        {
            yield return partition;
        }
    }
}