using System;
using System.Collections.Generic;

namespace Project
{
    public class Choice<T>
    {
        List<T> choiceItems;
        public bool ItemHasBeenChosen;
        Action<T> onChooseCallback;

        public Choice(List<T> items, Action<T> callback)
        {
            this.choiceItems = items;
            onChooseCallback = callback;
        }

        public void ChooseItem(int index)
        {
            if (!ItemHasBeenChosen && index < choiceItems.Count)
            {
                T item = choiceItems[index];
                onChooseCallback(item);
                ItemHasBeenChosen = true;
            }
        }

        public T GetItem(int i) => i < choiceItems.Count ? choiceItems[i] : default;
        public List<T> GetAllItems() => choiceItems;
    }
}