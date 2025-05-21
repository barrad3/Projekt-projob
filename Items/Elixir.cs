using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Items
{
    public class Elixir : BaseItem
    {
        public Elixir(string name = "Elixir", char symbol = 'E', int damage = 0) : base(name, symbol, damage)
        {
            IsElixir = true;
        }
    }

    public class HealthElixir : Elixir
    {
        public HealthElixir(string name = "Health Elixir", char symbol = 'H', int damage = 0) : base(name, symbol, damage)
        {
        }
        public override void OnEquip(Player player)
        {
            player.Health += 20;
        }
    }

    public class StrengthElixir : Elixir
    {
        public StrengthElixir(string name = "Strength Elixir", char symbol = '1', int damage = 0) : base(name, symbol, damage)
        {
        }

        public override void OnEquip(Player player)
        {
            player.AddEffect(new Effects.StrengthEffect(5, 2));
        }
    }

    public class LuckElixir : Elixir
    {
        public LuckElixir(string name = "Luck Elixir", char symbol = '2', int damage = 0) : base(name, symbol, damage)
        {
        }

        public override void OnEquip(Player player)
        {
            player.AddEffect(new Effects.LuckEffect(5));
        }
    }

    public class AntidotumElixir : Elixir
    {
        public AntidotumElixir(string name = "Antidotum", char symbol = 'A', int damage = 0) : base(name, symbol, damage)
        {
        }
        public override void OnEquip(Player player)
        {
            player.RemoveAllEffects();
        }
    }
}
