using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Combat
{
    public interface ICombatElement
    {
        void Accept(ICombatVisitor visitor);
    }
}

