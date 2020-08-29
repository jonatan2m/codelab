using System;
using System.Collections.Generic;

namespace TalkExamples.SOLID.LSP.Covariance
{
    /// <summary>
    /// https://www.it-swarm.dev/pt/c%23/diferenca-entre-covariancia-e-contravariancia/968341511/
    /// 
    /// https://pt.stackoverflow.com/questions/32880/o-que-s%C3%A3o-covari%C3%A2ncia-e-contravari%C3%A2ncia
    /// Melhor Resposta: https://pt.stackoverflow.com/a/32903
    /// </summary>
    public class Example2
    {
        public class Animal
        {

        }

        public class Gato : Animal
        {

        }

        public interface MinhaInterface<T>
        {

        }

        public interface MinhaInterfaceCovariante<out T>
        {

        }


        public static void Run()
        {
            Animal animal = new Animal();
            Gato gato = new Gato();
            
            gato = gato;
            animal = animal;
            animal = gato;
            
            IEnumerable<Animal> animais = null;
            IEnumerable<Gato> gatos = null;

            gatos = gatos;
            animais = animais;
            animais = gatos;
            
            //Erro compilação
            //gatos = animais;

            //dado o mapeamento T -> IE<T>, eu posso considerar que existe uma compatibilidade de atribuição em x => y (x pode ser atribuido a y)
            //Isso é covariancia, quando vc pode pegar o lado direito da seta e substituir em ambos os lados
            //uma lista de Gatos pode ser usada onde é esperado uma lista de Animais, mas o contrário não.

            IComparable<Animal> animaisIC = null;
            IComparable<Gato> gatoIC = null;

            gatoIC = gatoIC;
            animaisIC = animaisIC;
            gatoIC = animaisIC;
            
            //Erro compilação
            //animaisIC = gatoIC;

            //Você já deve ter percebido isso, mas vamos criar uma interface própria (MinhaInterface) e colocar ela generica.

            MinhaInterface<Animal> interfaceAnimal = null;
            MinhaInterface<Gato> interfaceGato = null;

            interfaceGato = interfaceGato;
            interfaceAnimal = interfaceAnimal;
            
            //Erro compilação
            //interfaceAnimal = interfaceGato;
            //interfaceGato = interfaceAnimal;

            MinhaInterfaceCovariante<Animal> interfaceCovarianteAnimal = null;
            MinhaInterfaceCovariante<Gato> interfaceCovarianteGato = null;

            interfaceCovarianteGato = interfaceCovarianteGato;
            interfaceCovarianteAnimal = interfaceCovarianteAnimal;
            interfaceCovarianteAnimal = interfaceCovarianteGato;

            

        }
    }
}
