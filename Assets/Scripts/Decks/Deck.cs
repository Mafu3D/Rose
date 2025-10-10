using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Decks
{
    public class Deck<T>
    {
        private List<T> startingItems = new();
        private List<T> currentItems = new();
        public int CurrentCount => currentItems.Count;
        public int TotalCount => startingItems.Count;

        public void AddPermanent(List<T> itemsToAdd, bool shuffle=true, bool addToTop=false)
        {
            foreach (T item in itemsToAdd)
            {
                if (addToTop) startingItems.Insert(0, item);
                else startingItems.Add(item);
                AddToRemaining(itemsToAdd, shuffle, addToTop);
            }
        }

        public void AddToRemaining(List<T> itemsToAdd, bool shuffle=true, bool addToTop=false)
        {
            foreach (T item in itemsToAdd)
            {
                if (addToTop) currentItems.Insert(0, item);
                else currentItems.Add(item);
                if (shuffle) Shuffle();
            }
        }

        public void Shuffle()
        {
            currentItems.Shuffle();
        }

        public void Reset()
        {
            currentItems = new List<T>(startingItems);
            Shuffle();
        }

        public T Draw(int index = 0)
        {
            if (currentItems.Count == 0 || index > currentItems.Count - 1)
            {
                return default;
            }

            T drawnItem;
            currentItems.Pop(index, out drawnItem);
            return drawnItem;
        }

        public List<T> DrawMultiple(int amount, int startIndex = 0)
        {
            List<T> drawn = new();
            for (int i = 0; i < amount; i++)
            {
                T item = Draw(startIndex); // Don't need to add i since the list is popped
                if (item != null)
                {
                    drawn.Add(item);
                }
            }
            return drawn;
        }

        // public List<T> DrawMultipleNoDuplicates(int amount, int startIndex = 0)
        // {
        //     int count = currentItems.Count - startIndex;
        //     List<T> remainingItems = new List<T>(currentItems.GetRange(startIndex, count));

        //     int totalDrawn = 0;
        //     List<T> drawn = new();
        //     for (int i = 0; i < remainingItems.Count; i++)
        //     {
        //         T item = Draw(startIndex); // Don't need to add i since the list is popped
        //         if (item != null && !drawn.Contains(item))
        //         {
        //             drawn.Add(item);
        //             totalDrawn++;
        //         }

        //         if (totalDrawn == amount) continue;
        //     }
        //     return drawn;
        // }
    }

    public interface IDrawable
    {
        public int GetItemHash();
    }
}