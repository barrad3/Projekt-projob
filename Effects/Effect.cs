using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Effects
{
    public abstract class Effect : IEffect
    {
        protected int duration;
        protected int remainingTurns;

        public Effect(int duration)
        {
            this.duration = duration;
            this.remainingTurns = duration;
        }

        public abstract void Apply(Player player);
        public abstract void Remove(Player player);

        public virtual void Update(Player player)
        {
            if (remainingTurns > 0)
            {
                remainingTurns--;
                if (remainingTurns == 0)
                {
                    player.RemoveEffect(this);
                }
            }
        }

        public bool IsExpired => remainingTurns <= 0;

        public abstract string Description { get; }
    }

    public class StrengthEffect : Effect
    {
        private int strengthIncrease;

        public StrengthEffect(int duration, int strengthIncrease) : base(duration)
        {
            this.strengthIncrease = strengthIncrease;
        }

        public override void Apply(Player player)
        {
            player.Strength += strengthIncrease;
        }

        public override void Remove(Player player)
        {
            player.Strength -= strengthIncrease;
        }

        public override string Description => $"Strength +{strengthIncrease} for {remainingTurns} turns";
    }

    public class LuckEffect : Effect
    {
        public LuckEffect(int duration) : base(duration)
        {
        }

        public override void Apply(Player player)
        {
        }

        public override void Remove(Player player)
        {
            player.Luck = Player.BaseLuck;
        }

        public override void Update(Player player)
        {
            if (!IsExpired)
            {
                player.Luck = Player.BaseLuck * remainingTurns;
            }
            base.Update(player);
        }

        public override string Description => $"Luck multiplier for {remainingTurns} turns";
    }


}
