using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.Example1
{
    public class PauseState : BaseState
    {
        public PauseState(BaseState state) : base(state)
        {
        }

        public PauseState(Player player, List<string> playlist) : base(player, playlist)
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
            player.SetState(new PlayingState(this));
        }

        public override void Previous()
        {
            PreviousMusic();
        }
    }
}
