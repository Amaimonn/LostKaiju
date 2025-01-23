namespace LostKaiju.Game.Player.LiveSystem
{
    public class PlayerRespawner : IRespawner
    {
        public void HandleRespawn(IRespawnable respawnable)
        {
            respawnable.Respawn();
        }
    }
}
