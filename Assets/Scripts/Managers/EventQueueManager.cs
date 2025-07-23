using System.Collections.Generic;
using UnityEngine;
using Strategy;
using Commands;

namespace Managers
{
    public class EventQueueManager : MonoBehaviour
    {
        public static EventQueueManager instance;

        public List<ICommand> Events => _events;
        private List<ICommand> _events = new List<ICommand>();

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }
            instance = this;
        }

        private void Update()
        {
            for (int i = _events.Count - 1; i >= 0; i--)
            {
                _events[i].Execute();
                _events.RemoveAt(i);
            }
        }

        public void AddEvent(ICommand command) => _events.Add(command);
    }
}