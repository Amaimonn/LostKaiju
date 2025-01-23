using R3;

namespace LostKaiju.Game.Player.LiveSystem
{
    public class CheckpointRespawnable : IRespawnable
    {
        public Observable<ICheckpoint> OnRespawn => _onRespawn;

        private Subject<ICheckpoint> _onRespawn = new();
        private ICheckpoint _checkpoint;

#region IRespawnable
        public void Respawn()
        {
            if (_checkpoint != null)
                _onRespawn.OnNext(_checkpoint);
        }
#endregion

        public void SetCheckPoint(ICheckpoint checkPoint)
        {
            _checkpoint = checkPoint;
        }
    }
}
