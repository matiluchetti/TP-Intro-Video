using Strategy;
namespace Commands
{
    public class CmdReload : ICommand
    {
        private IGun _weapon;

        public CmdReload(IGun weapon)
        {
            _weapon = weapon;
        }

        public void Execute()
        {
            _weapon.Reload();
        }
    }
}
