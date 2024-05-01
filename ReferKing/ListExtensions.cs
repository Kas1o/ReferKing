using System;
using System.Collections.Generic;

public static class ListExtensions
{
    private static Random random = new Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }
}