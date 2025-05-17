using UnityEngine;
using Strategy;
namespace Commands
{
    public class CmdMovement
    {
        private Vector3 _direction;
        private IMoveable _player;

        public CmdMovement(Vector3 direction, IMoveable player)
        {
            _direction = direction;
            _player = player;
        }

        public void Do()
        {
            _player.Move(_direction);
        }
    }
}
