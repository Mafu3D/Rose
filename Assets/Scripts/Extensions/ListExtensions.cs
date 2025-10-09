using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    static Random rng;

    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if (rng == null) { rng = new Random(); }
        int count = list.Count;
        while (count > 1)
        {
            --count;
            int index = rng.Next(count + 1);
            (list[index], list[count]) = (list[count], list[index]);
        }

        return list;
    }

    /// <summary>
    /// Remove item from list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="index"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IList<T> Pop<T>(this IList<T> list, int index, out T item)
    {
        item = list[index];
        list.RemoveAt(index);

        return list;
    }
}