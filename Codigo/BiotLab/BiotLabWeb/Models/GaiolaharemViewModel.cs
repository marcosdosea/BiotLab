using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class GaiolaharemViewModel
    {
        [Display(Name = "Gaiola")]
        [Required(ErrorMessage = "A gaiola é obrigatória")]
        public uint IdGaiola { get; set; }

        [Display(Name = "Berçário")]
        [Required(ErrorMessage = "O berçário é obrigatório")]
        public uint IdHarem { get; set; }

        [Display(Name = "Data de Povoamento")]
        [Required(ErrorMessage = "A data de povoamento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataPovoamento { get; set; }

        [Display(Name = "Pesquisador")]
        [Required(ErrorMessage = "O pesquisador é obrigatório")]
        public uint IdPesquisador { get; set; }

        [Display(Name = "Gaiola")]
        public string? NomeGaiola { get; set; }

        [Display(Name = "Berçário")]
        public string? NomeHarem { get; set; }

        [Display(Name = "Pesquisador")]
        public string? NomePesquisador { get; set; }

        [Display(Name = "Código Interno da Gaiola")]
        public string? CodigoInternoGaiola { get; set; }

        [Display(Name = "Código Interno do Berçário")]
        public string? CodigoInternoHarem { get; set; }
    }
}
