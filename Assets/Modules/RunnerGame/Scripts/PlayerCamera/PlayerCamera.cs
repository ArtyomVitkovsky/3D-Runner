using UnityEngine;
using Zenject;

namespace Modules.RunnerGame.Scripts.PlayerCamera
{
    public class PlayerCamera : CameraFollower
    {
        [SerializeField] private Camera camera;

        private Vector3 lookPoint;
        
        private Player.Player player;

        public Camera Camera => camera;


        [Inject]
        private void Construct(Player.Player player)
        {
            SetPlayerUnit(player);
        }

        private void SetPlayerUnit(Player.Player player)
        {
            this.player = player;
            SetFollowTarget(this.player.transform);
        }

    }
}