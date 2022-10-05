using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.State.Example1
{
    public abstract class BaseState
    {
        protected Player player;
        protected List<string> playlist;
        protected int currentPlayingIndex = -1;

        public BaseState(BaseState state)
            : this(state.player, state.playlist)
        {
            currentPlayingIndex = state.currentPlayingIndex;
        }

        public BaseState(Player player, List<string> playlist)
        {
            this.player = player;
            this.playlist = playlist;
        }

        public abstract void Play();
        public abstract void Next();
        public abstract void Previous();
        public abstract void Lock();

        int Index
        {
            get
            {
                return currentPlayingIndex % playlist.Count;
            }
        }
        protected void NextMusic()
        {
            currentPlayingIndex++;

            player.CurrentMusicName = playlist[Index];
        }

        protected void PreviousMusic()
        {
            currentPlayingIndex--;
            player.CurrentMusicName = playlist[Index];
        }
    }
}
