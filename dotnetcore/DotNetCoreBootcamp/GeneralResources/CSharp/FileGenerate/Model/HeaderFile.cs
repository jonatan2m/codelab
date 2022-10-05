using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp.FileGenerate.Model
{
    public enum MessageType
    {
        Header = 10,
        Record = 20,
        Trailer = 30
    }

    public class HeaderFile
    {
        public MessageType MessageType { get; set; } = MessageType.Header;

        /// <summary>
        /// 32 carcteres
        /// </summary>
        public string Code { get; set; }

        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 1 - enviado, 2 - aprovado, 3 reprovado
        /// </summary>
        public int Status { get; set; }

        public override string ToString()
        {
            return $"{MessageType}{Code}{CreatedAt:yyyyMMdd}{Status}";
        }
    }

    public class RecordFile
    {

    }

    public class TrailerFile
    {

    }
}
