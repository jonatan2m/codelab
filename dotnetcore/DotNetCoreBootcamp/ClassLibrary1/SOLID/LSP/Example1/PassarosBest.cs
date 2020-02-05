using System;
using System.Collections.Generic;
using System.Text;

namespace TalkExamples.SOLID.LSP.Example1.Best
{
    public abstract class Passaro
    {
    }

    public abstract class PassaroQueVoa : Passaro
    {
        public abstract void Voar();
    }

    public class Papagaio : PassaroQueVoa
    {
        public override void Voar()
        {
            //Pode Voar
        }
    }
    public class Pato : Passaro
    {
    }
}
