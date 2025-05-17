using Strategy;

namespace Commands
{
    public class CmdShoot : ICommand
    {
        private IGun _weapon;

        public CmdShoot(IGun weapon)
        {
            _weapon = weapon;
        }

        public void Execute()
        {
            _weapon.Attack();
        }
    }
}
