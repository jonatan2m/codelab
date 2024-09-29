namespace GeneralResources.CSharp.Records
{
    //dá pra usar essa classe como Value Object. As duas propriedades são somente leitura
    record FullName(string FirstName, string LastName);

    #region Implementação dos records em classes (apenas para estudo)
    //primary constructor: C#12 or greater
    //class CustomFullName(string FirstName, string LastName)
    class CustomFullName
    {
        public string FirstName { get; init; }
        public string LastName { get; init; }

        public CustomFullName(string firstName, string lastName)
            => (FirstName, LastName) = (firstName, lastName);

        public CustomFullName(CustomFullName other) : this(other.FirstName, other.LastName) { }

        public void Deconstruct(out string firstName, out string lastName)
            => (firstName, lastName) = (FirstName, LastName);

    }
    #endregion

    public class RecordsConcepts
    {
        [Fact]
        public void TestImutability()
        {
            FullName fullName = new FullName("Jonatan", "Machado");
            //error, reaonly property
            //fullName.FirstName = "";
        }
        [Fact]
        public void TestInstanciates()
        {
            CustomFullName customFullName = new("Jonatan", "Machado");
            //non-destructive mutations
            CustomFullName customFullName2 = new(customFullName) { LastName = "Martins" };
            //deconstruct: get only the properties you're interested in
            var (customFirstName2, _) = customFullName2;

            //###############################################

            FullName fullName = new FullName("Jonatan", "Machado");
            //non-destructive mutations
            FullName fullName2 = fullName with { LastName = "Martins" };
            //deconstruct: get only the properties you're interested in
            var (firstName2, _) = fullName;

            Assert.NotEqual("", fullName.ToString());
            
        }
    }
}
