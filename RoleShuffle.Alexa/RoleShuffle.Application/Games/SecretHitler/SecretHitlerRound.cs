namespace RoleShuffle.Application.Games.SecretHitler
{
    public class SecretHitlerRound
    {
        public SecretHitlerRound(short playerAmount)
        {
            PlayerAmount = playerAmount;
        }

        public short PlayerAmount { get; }
    }
}