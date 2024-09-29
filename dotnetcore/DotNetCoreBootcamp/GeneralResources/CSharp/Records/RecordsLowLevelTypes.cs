namespace GeneralResources.CSharp.Records
{
    //struct
    record struct PersonId(Guid Value);

    record struct NonEmptyString(string Value)
    {
        public string Value { get; init; } =
            string.IsNullOrWhiteSpace(Value) ? Value.Trim()
            : throw new ArgumentException("value cannot be null or whitespace", nameof(Value));

        public static implicit operator string(NonEmptyString value) => value.Value;
    }
    abstract record Name;
    sealed record Monomym (string FirstName) : Name;    

    //Make invalid states unrepresentable
    record Book(NonEmptyString Title, Name name);

    public class RecordsLowLevelTypes
    {
        [Fact]
        public void Test1()
        {
            PersonId personId = new();
            Assert.True(Guid.Empty == personId.Value);
        }
        
    }
}
