namespace MTCG.Cards
{
    public abstract class Spell : Card
    {
        public override bool IsResistant(Card enemyCard)
        {
            return false;
        }

        public override bool IsWeak(Card enemyCard)
        {
            return false;
        }

        public override bool CanEvade(Card enemyCard)
        {
            return false;
        }
    }
}