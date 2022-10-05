using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.Example1
{
    public class PlayingState : BaseState
    {
        public PlayingState(BaseState state) : base(state)
        {
        }

        public override void Lock()
        {
            player.SetState(new LockingState(this));
        }

        public override void Next()
        {
            NextMusic();
        }

        public override void Play()
        {
            player.SetState(new PauseState(this));
        }

        public override void Previous()
        {
            PreviousMusic();
        }
    }
}
