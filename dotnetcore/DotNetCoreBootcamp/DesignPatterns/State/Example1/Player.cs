using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace DesignPatterns.State.Example1
{
    public class Player
    {   
        BaseState state;

        public string CurrentMusicName { get; internal set; }

        public Player(params string[] music)
        {
            if (music == null || music.Count() == 0)
                throw new ArgumentNullException();

            state = new ReadyState(this, new List<string>(music));
        }

        public void Play()
        {
            state.Play();
        }
        
        public BaseState GetState()
        {
            return state;
        }

        public void SetState(BaseState state)
        {
            this.state = state;
        }

        public void Pause()
        {
            state.Play();
        }

        public void Next()
        {
            state.Next();
        }

        public void Previous()
        {
            state.Previous();
        }

        public void Lock()
        {
            state.Lock();
        }
    }
}
