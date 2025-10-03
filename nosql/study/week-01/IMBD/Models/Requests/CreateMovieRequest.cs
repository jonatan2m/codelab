using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IMBD.Models.Requests
{
    public class CreateMovieRequest
    {
        public string Title { get; set; } = null!;
        public int Year { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public List<string>? Castings { get; set; }

    }

    /// <summary>
    /// a ser utilizado no futuro, para forçar uma migração
    /// </summary>
    public class CreateActorRequest
    {
        public string? Id { get; set; }
        public string Name { get; set; } = null!;
    }
}