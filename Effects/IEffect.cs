using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Effects
{
    public interface IEffect
    {
        void Apply(Player player);
        void Remove(Player player);
        void Update(Player player);
        bool IsExpired { get; }
        string Description { get; }
    }

}
