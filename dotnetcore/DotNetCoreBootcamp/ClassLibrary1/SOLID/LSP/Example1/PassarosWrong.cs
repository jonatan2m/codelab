using System;
using System.Collections.Generic;
using System.Text;

namespace TalkExamples.SOLID.LSP.Example1.Wrong
{
    public abstract class Passaro
    {
        public abstract void Voar();
    }


    public class Papagaio : Passaro
    {
        public override void Voar()
        {
            //Pode Voar
        }
    }

    public class Pato : Passaro
    {
        public override void Voar()
        {
            // Não é possivel implementar, pato não voa
            throw new NotSupportedException();
        }
    }
}
