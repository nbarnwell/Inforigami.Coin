using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Coin.Shell
{
    public class WorkspaceHostViewModel : Conductor<Screen>.Collection.OneActive, IWorkspaceHost
    {
    }
}
