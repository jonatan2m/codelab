using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.Example1
{
    public class LockingState : BaseState
    {
        private BaseState lastState;

        public LockingState(BaseState state) : base(state)
        {
            lastState = state;
        }

        public LockingState(Player player, List<string> playlist) : base(player, playlist)
        {
        }

        public override void Lock()
        {
            if (lastState == null)
                player.SetState(new ReadyState(this));
            else
                player.SetState(lastState);
        }

        public override void Next()
        {
            //Do Nothing
        }

        public override void Play()
        {
            //Do Nothing
        }

        public override void Previous()
        {
            //Do Nothing
        }
    }
}
