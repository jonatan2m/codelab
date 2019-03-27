using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.Example1
{
    public class ReadyState : BaseState
    {
        public ReadyState(BaseState state)
            : base(state) { }

        public ReadyState(Player player, List<string> playlist) : base(player, playlist)
        {
        }

        public override void Lock()
        {
            //Do Nothing
        }

        public override void Next()
        {
            //Do Nothing
        }

        public override void Play()
        {
            NextMusic();
            player.SetState(new PlayingState(this));
        }

        public override void Previous()
        {
            //Do Nothing
        }
    }

}
