public class Operation
    {
        public int Value1 { get; set; }
        public int Value2 { get; set; }
        public string OperationType { get; set; }

        public int Execute()
        {
            if(OperationType == "+") return Value1 + Value2;

            return int.MinValue;
        }
    }
