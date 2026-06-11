using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class ObituarioViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Data do óbito")]
        [Required(ErrorMessage = "A data do óbito é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; } = DateTime.Today;

        [Display(Name = "Gaiola")]
        [Required(ErrorMessage = "A gaiola é obrigatória.")]
        public uint IdGaiola { get; set; }

        [Display(Name = "Pesquisador")]
        [Required(ErrorMessage = "O pesquisador é obrigatório.")]
        public uint IdPesquisador { get; set; }
    }
}