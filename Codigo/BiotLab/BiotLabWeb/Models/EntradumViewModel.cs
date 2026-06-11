using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class EntradumViewModel
    {
        [Display(Name = "Código da Entrada")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Número da Nota Fiscal")]
        [Required(ErrorMessage = "O número da nota fiscal é obrigatório.")]
        [StringLength(50, ErrorMessage = "O número da nota fiscal deve ter no máximo 50 caracteres.")]
        public string NumeroNotaFiscal { get; set; } = string.Empty;

        [Display(Name = "Data da Entrada")]
        [Required(ErrorMessage = "A data da entrada é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime DataEntrada { get; set; }

        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "O fornecedor é obrigatório.")]
        public uint IdFornecedor { get; set; }

        [Display(Name = "Instituição")]
        [Required(ErrorMessage = "A instituição é obrigatória.")]
        public uint IdInstituicao { get; set; }

        [Display(Name = "Fornecedor")]
        public string? NomeFornecedor { get; set; }

        [Display(Name = "Instituição")]
        public string? NomeInstituicao { get; set; }
    }
}