using Projekt1.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1
{
    public class Enemy : ICombatElement
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Strength { get; set; }
        public int Armor { get; set; }

        public char Symbol => '@';

        public Enemy(int health = 50, int strength = 20, int armor = 5)
        {
            MaxHealth = health;
            CurrentHealth = health;
            Strength = strength;
            Armor = armor;
        }

        public void Accept(ICombatVisitor visitor) => visitor.VisitEnemy(this);

        public override string ToString() =>
            $"HP: {CurrentHealth}/{MaxHealth}  STR: {Strength}  ARM: {Armor}";
    }
}
