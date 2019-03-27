using DesignPatterns.State.Example1;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DesignPatternsTest
{
    public class StateTest
    {
        [Fact]
        public void State_Example1()
        {
            var player = new Player("music1");
            
            Assert.IsType<ReadyState>(player.GetState());

            player.Play();
            Assert.Equal("music1", player.CurrentMusicName);
            Assert.IsType<PlayingState>(player.GetState());
        }

        [Fact]
        public void State_Example1_1()
        {
            var player = new Player("music1");
                        
            player.Play();
            Assert.Equal("music1", player.CurrentMusicName);

            player.Pause();
            Assert.IsType<PauseState>(player.GetState());

            player.Play();
            Assert.Equal("music1", player.CurrentMusicName);
        }

        [Fact]
        public void State_Example1_2()
        {
            var player = new Player("music1", "music2", "music3", "music4");

            player.Play();                        
            player.Play();
            Assert.IsType<PauseState>(player.GetState());
            player.Play();
            Assert.IsType<PlayingState>(player.GetState());
            Assert.Equal("music1", player.CurrentMusicName);

            player.Next();
            Assert.Equal("music2", player.CurrentMusicName);

            player.Play();
            player.Next();
            Assert.Equal("music3", player.CurrentMusicName);
            player.Next();
            Assert.Equal("music4", player.CurrentMusicName);

            player.Play();
            player.Next();
            Assert.Equal("music1", player.CurrentMusicName);

            player.Previous();
            Assert.Equal("music4", player.CurrentMusicName);
            player.Previous();
            Assert.Equal("music3", player.CurrentMusicName);

            player.Lock();
            Assert.IsType<LockingState>(player.GetState());
            Assert.Equal("music3", player.CurrentMusicName);

            player.Lock();
            Assert.IsType<PlayingState>(player.GetState());
        }
    }
}
