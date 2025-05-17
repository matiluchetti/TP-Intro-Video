using UnityEngine;
using Strategy;

namespace Commands
{
    public class CmdRotateTowardsMouse
    {
        private IMoveable _player;

        public CmdRotateTowardsMouse(IMoveable player)
        {
            _player = player;
        }

        public void Do()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            if (plane.Raycast(ray, out float distance))
            {
                Vector3 point = ray.GetPoint(distance);
                _player.RotateTowards(point);  // <-- usa Vector3 point
            }
        }
    }
}
