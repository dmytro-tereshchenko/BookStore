using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Interfaces
{
    internal interface IViewModel
    {
        event PropertyChangedEventHandler PropertyChanged;
    }
}
