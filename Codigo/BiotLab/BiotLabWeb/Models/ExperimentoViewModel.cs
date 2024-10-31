using Core;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class ExperimentoViewModel
    {
        [Display(Name = "Código do Experimento")]
        [Required(ErrorMessage = "O código da experimento é obrigatório")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Nome do Pesquisador")]
        [Required(ErrorMessage = "O nome do Pesquisador é obrigatório")]
        [StringLength(50, ErrorMessage = "O nome deve ter no máximo 50 caracteres")]
        public string Nome { get; set; } = null!;

        [Display(Name = "DataInicio")]
        [Required(ErrorMessage = "O DataInicio é obrigatório")]
        [RegularExpression(@"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", ErrorMessage = "Formato de DataInicio inválido")]
        public string DataInicio { get; set; } = null!;

        [Display(Name = "DataFim")]
        [Required(ErrorMessage = "O DataFim é obrigatório")]
        [RegularExpression(@"\d{5}-\d{3}", ErrorMessage = "Formato de DataFim inválido")]
        public string DataFim { get; set; } = null!;

        [Display(Name = "Cepa")]
        [StringLength(50, ErrorMessage = "O nome da Cepa deve ter no máximo 50 caracteres")]
        public string? Cepa { get; set; }

        [Display(Name = "Id Pesquisador Navigation")]
        [StringLength(50, ErrorMessage = "O Id Pesquisador Navigation deve ter no máximo 50 caracteres")]
        public string? IdPesquisadorNavigation { get; set; }

        [Display(Name = "Anestesico Usado")]
        [StringLength(10, ErrorMessage = "O nome do Anestesico deve ter no máximo 10 caracteres")]
        public string? Usoanestesicos { get; set; }

        [Display(Name = "Gaiolas")]
        [StringLength(50, ErrorMessage = "O número das Gaiolas deve ter no máximo 50 caracteres")]
        public string? Gaiolas { get; set; } 

    }
}
