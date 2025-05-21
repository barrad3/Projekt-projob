namespace Projekt1.Items
{
    public interface IItem
    {
        string Name { get; }
        char Symbol { get; }
        int Damage { get; }
        int RequiredHands { get; }
        bool IsElixir { get; }

        string GetName();
        int GetDamage();
        void OnEquip(Player player);
        void OnUnequip(Player player);
        int GetCurrencyAmount();

        void IncreaseDamage(int bonus);
        void DecreaseDamage(int bonus);

        void ApplyPickup(Player player, Room room);

        bool Equip(Player player, char targetHand)
        {
            if (RequiredHands == 1)
            {
                if ((targetHand == 'R' || targetHand == 'r') && player.RightHand == null)
                {
                    player.RightHand = this;
                    OnEquip(player);
                    return true;
                }
                else if ((targetHand == 'L' || targetHand == 'l') && player.LeftHand == null)
                {
                    player.LeftHand = this;
                    OnEquip(player);
                    return true;
                }
                return false;
            }
            else if (RequiredHands == 2)
            {
                if (player.LeftHand == null && player.RightHand == null)
                {
                    player.LeftHand = this;
                    player.RightHand = this;
                    OnEquip(player);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
