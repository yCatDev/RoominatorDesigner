using System.Collections.Generic;
using System.Linq;

namespace Logic.Action
{
    public class ActionsManager
    {
        private HashSet<ActionBehaviour> m_actions = new HashSet<ActionBehaviour>();

        public void RegisterBehaviour(ActionBehaviour behaviour)
        {
            m_actions.Add(behaviour);
        }

        public bool IsAnyActionActive() => m_actions.Any(x => x.Active);
    }
}