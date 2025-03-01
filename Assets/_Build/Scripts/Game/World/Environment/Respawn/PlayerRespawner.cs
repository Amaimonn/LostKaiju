namespace LostKaiju.Game.World.Environment.Respawn
{
    public class PlayerRespawner : IRespawner
    {
        public void HandleRespawn(IRespawnable respawnable)
        {
            respawnable.Respawn();
        }
    }
}
