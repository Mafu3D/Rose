using System;
using System.Collections.Generic;

namespace Project
{
    public class Choice<T>
    {
        public List<T> Items { get; private set; }
        public bool ItemHasBeenChosen { get; private set; }
        public int NumberOfChoices => Items.Count;
        private Action<T> onChooseCallback;

        public Choice(List<T> items, Action<T> callback)
        {
            this.Items = items;
            onChooseCallback = callback;
        }

        public void ChooseItem(int index)
        {
            if (!ItemHasBeenChosen && index < Items.Count)
            {
                T item = Items[index];
                onChooseCallback(item);
                ItemHasBeenChosen = true;
            }
        }

        public T GetItem(int i) => i < Items.Count ? Items[i] : default;
        public List<T> GetAllItems() => Items;
    }
}