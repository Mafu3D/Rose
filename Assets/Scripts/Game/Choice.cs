using System;
using System.Collections.Generic;

namespace Project
{
    public class Choice<T>
    {
        /// <summary>
        /// Generic collection of items that can be chosen.
        /// Contains two lists of items that have been chosen and items that have not been chosen.
        /// When finished will execute a callback with chosen items and not chosen items.
        /// </summary>

        private List<T> items;
        private List<T> chosen = new ();
        private List<T> notChosen = new ();
        public int NumberOfChoices => items.Count;
        private Action<List<T>, List<T>> resolveCallback;

        public Choice(List<T> items, Action<List<T>, List<T>> callback)
        {
            this.items = new List<T>(items);
            this.notChosen = new List<T>(items);
            resolveCallback = callback;
        }

        public T GetItem(int i) => i < items.Count ? items[i] : default;
        public List<T> GetAllItems() => items;
        public List<T> GetChosen() => chosen;
        public List<T> GetNotChosen() => notChosen;

        public void ChooseItem(int index)
        {
            if (index < items.Count)
            {
                T chosenItem = items[index];
                List<T> notChosenItems = new();
                for (int i = 0; i < items.Count; i++)
                {
                    if (i != index) notChosenItems.Add(items[i]);
                }
                chosen.Add(chosenItem);
                notChosen.Remove(chosenItem);

            }
        }

        public void UnchooseItem(T item)
        {
            if (chosen.Contains(item))
            {
                chosen.Remove(item);
                notChosen.Add(item);
            }
        }

        public void AddItem(T item)
        {
            items.Add(item);
            notChosen.Add(item);
        }

        public void RemoveItem(T item)
        {
            if (items.Contains(item))
            {
                items.Remove(item);
            }
            if (notChosen.Contains(item))
            {
                notChosen.Remove(item);
            }
        }

        public void ReplaceItem(T itemToAdd, T itemToReplace)
        {
            RemoveItem(itemToReplace);
            AddItem(itemToAdd);
        }

        public void Resolve() => resolveCallback(chosen, notChosen);
    }
}