using UnityEngine;
using Strategy;

namespace Commands
{
    public class CmdRotateTowardsMouse : ICommand
    {
        private IMoveable _player;

        public CmdRotateTowardsMouse(IMoveable player)
        {
            _player = player;
        }

        public void Execute()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float y = (_player as MonoBehaviour).transform.position.y;
            Plane plane = new Plane(Vector3.up, new Vector3(0, y, 0));

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 point = ray.GetPoint(distance);
                _player.RotateTowards(point); 
            }
        }
    }
}
