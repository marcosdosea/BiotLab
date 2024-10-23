using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class GaiolaharemViewModel
    {
        [Display(Name = "ID da Gaiola")]
        [Required(ErrorMessage = "O ID da gaiola é obrigatório")]
        public uint IdGaiola { get; set; }

        [Display(Name = "ID do Harem")]
        [Required(ErrorMessage = "O ID do harem é obrigatório")]
        public uint IdHarem { get; set; }

        [Display(Name = "Data de Povoamento")]
        [Required(ErrorMessage = "A data de povoamento é obrigatória")]
        [DataType(DataType.Date)]
        public DateTime DataPovoamento { get; set; }

        [Display(Name = "ID do Pesquisador")]
        [Required(ErrorMessage = "O ID do pesquisador é obrigatório")]
        public uint IdPesquisador { get; set; }

        // Propriedades adicionais para exibição
        [Display(Name = "Nome da Gaiola")]
        public string? NomeGaiola { get; set; }

        [Display(Name = "Nome do Harem")]
        public string? NomeHarem { get; set; }

        [Display(Name = "Nome do Pesquisador")]
        public string? NomePesquisador { get; set; }
    }
}
