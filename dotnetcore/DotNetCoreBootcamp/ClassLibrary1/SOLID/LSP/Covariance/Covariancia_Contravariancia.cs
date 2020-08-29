using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TalkExamples.SOLID.LSP.Covariance
{
    public class Covariancia_Contravariancia
    {
        public class Animal { }
        public class Passaro : Animal { }
         /*
            Animal -> Animal
            Animal -> Passaro
            Passaro -> Passaro
            Passaro -> Animal (x)
         */
        #region Inicio
        private static void Inicio()
        {
            Passaro passaro = new Passaro();
            Animal animal = new Animal();
            animal = passaro;
            //passaro = animal;
                        
            InicioHeranca(animal);                        
            InicioHeranca(passaro);
            
        }

        private static void InicioHeranca(Animal animal)
        {

        }
        #endregion

        #region Caso1
        public interface IFoo<out T>
        {
            T FazAlgo();
        }

        public class Foo<T> : IFoo<T>
        {            
            public T FazAlgo()
            {
                throw new NotImplementedException();
            }
        }

        private static void Caso1()
        {
            #region Generics
            IFoo<Animal> animalFoo = new Foo<Animal>();
            Caso1Generico(animalFoo);

            IFoo<Passaro> passaroFoo = new Foo<Passaro>();
            Caso1Generico(passaroFoo);
            #endregion
        }

        private static void Caso1Generico(IFoo<Animal> animalFoo)
        {

        }
        #endregion
                
        #region Caso 2
        public interface IBar<in T>
        {
            void FazAlgo(T obj);
        }

        public class Bar<T> : IBar<T>
        {
            public void FazAlgo(T obj)
            {
                throw new NotImplementedException();
            }
        }

        private static void Caso2()
        {
            Passaro passaro = new Passaro();
            Animal animal = new Animal();
            animal = passaro;
            //passaro = animal;

            #region Generics
            IBar<Animal> animalFoo = new Bar<Animal>();
            Caso2Generico(animalFoo);

            IBar<Passaro> passaroFoo = new Bar<Passaro>();
            Caso2Generico(passaroFoo);
            #endregion
        }

        private static void Caso2Generico(IBar<Passaro> passaroFoo)
        {

        }
        #endregion

        #region Exemplos .Net
        public static void ExemplosDotNet()
        {
            IEnumerable<Animal> passaros = new List<Passaro>();            
            Action<Passaro> actionPassaro = ActionAnimal;                       
            Func<Animal> func = () => new Passaro();
        }

        public static void ActionAnimal(Animal animal) { }

        #endregion
    }
}
