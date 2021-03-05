using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Commands
{
    interface ICommand
    {
        bool CanExecute();
        void Execute();
    }
}
