namespace Crossfire.Workspace
{
    public interface IPlayerLocator
    {
        Player Player { get; }
        void SetupPlayer(Player player);
    }

    public sealed class PlayerLocator : IPlayerLocator
    {
        public Player Player { get; private set; }

        public PlayerLocator(Player player)
        {
            Player = player;
        }

        public void SetupPlayer(Player player)
        {
            Player = player;
        }
    }
}

