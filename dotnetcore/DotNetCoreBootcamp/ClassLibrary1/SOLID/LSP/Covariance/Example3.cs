using System;
using System.Collections.Generic;
using System.Text;

namespace TalkExamples.SOLID.LSP.Covariance
{
    

    public interface IAve
    {
        void Bicar();
    }

    public interface IAveQueVoa : IAve
    {
        void Voar();
    }

    public class PicaPau : IAveQueVoa
    {
        public void Bicar()
        {
            Console.WriteLine("Bicar");
        }

        public void Voar()
        {
            Console.WriteLine("Voar");
        }
    }

    public class Pinguim : IAve
    {
        public void Bicar()
        {
            Console.WriteLine("Bicar");
        }
    }

    /// <summary>
    /// This isn't related to covariance but I wanna make it more complete soon.
    /// </summary>
    public class Example3
    {
        public static void Run()
        {
            PicaPau picapau = new PicaPau();
            Pinguim pinguim = new Pinguim();

            Bica(picapau);
            Bica(pinguim);

            Voa(picapau);
            //Voa(pinguim);
        }

        public static void Bica(IAve ave) { ave.Bicar(); }

        public static void Voa(IAveQueVoa aveQueVoa)
        {
            aveQueVoa.Voar();
        }
    }
}
