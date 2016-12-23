using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Coin.Shared
{
    public class MaterialGrid : Grid
    {
        public MaterialGrid()
        {
            for (int i = 0; i < 12; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}
