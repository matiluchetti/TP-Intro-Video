using UnityEngine;
using Strategy;
namespace Commands
{
    public class CmdMovement: ICommand
    {
        private Vector3 _direction;
        private IMoveable _player;

        public CmdMovement(Vector3 direction, IMoveable player)
        {
            _direction = direction;
            _player = player;
        }

        public void Execute()
        {
            _player.Move(_direction);
        }
    }
}
