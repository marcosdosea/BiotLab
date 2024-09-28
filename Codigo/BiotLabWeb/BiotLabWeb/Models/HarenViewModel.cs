namespace BiotLabWeb.Models
{
    public class HarenViewModel
    {
        public int Id { get; set; }

        public int NumeroMacho { get; set; }

        public int NumeroFemea { get; set; }

        /// <summary>
        /// Status do Haren
        /// </summary>
        public string? Status { get; set; }
    }
}
