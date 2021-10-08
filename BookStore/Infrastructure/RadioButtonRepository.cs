using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure
{
    public class RadioButtonRepository
    {
        private Dictionary<string, bool> buttons;
        public RadioButtonRepository(string[] keys)
        {
            buttons = new Dictionary<string, bool>();
            if (keys is null) return;
            foreach (var key in keys)
            {
                buttons[key] = false;
            }
            buttons[keys.LastOrDefault()] = true;
        }
        public bool this[string key]
        {
            get
            {
                if (buttons.ContainsKey(key))
                {
                    return buttons[key];
                }
                throw new ArgumentOutOfRangeException(nameof(RadioButtonRepository));
            }
            set
            {
                if (buttons.ContainsKey(key))
                {
                    buttons[key] = value;
                }
            }
        }
    }
}
