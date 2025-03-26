using Unity.Cinemachine;

using LostKaiju.Game.World.Creatures.Features;
using LostKaiju.Game.World.Creatures.Views;

namespace LostKaiju.Infrastructure.Managers
{
    public class CameraManager
    {
        private readonly CinemachineCamera _cinemachineCamera;

        public CameraManager(CinemachineCamera cinemachineCamera)
        {
            _cinemachineCamera = cinemachineCamera;
        }

        public void FollowCreature(CreatureBinder creature)
        {
            if (creature.Features.TryResolve<ICameraTarget>(out var cameraTarget) &&
                cameraTarget.TargetTransform != null)
            {
                _cinemachineCamera.Follow = cameraTarget.TargetTransform;
            }
            else
            {
                _cinemachineCamera.Follow = creature.transform;
            }
        }

        public void StopFollowing()
        {
            _cinemachineCamera.Follow = null;
        }
    }
}