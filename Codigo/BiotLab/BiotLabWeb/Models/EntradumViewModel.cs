using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class EntradumViewModel
    {
        [Display(Name = "Código do Registro")]
        [Required(ErrorMessage = "O código do registro é obrigatório")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Número da Nota Fiscal")]
        [Required(ErrorMessage = "O número da nota fiscal é obrigatório")]
        [StringLength(50, ErrorMessage = "O número deve ter no máximo 50 caracteres")]
        public string NumeroNotaFiscal { get; set; } = string.Empty; // Número da Nota Fiscal

        [Display(Name = "Data de Entrada")]
        [Required(ErrorMessage = "A data de entrada é obrigatória")]
        public DateTime DataEntrada { get; set; } // Data de entrada

        [Display(Name = "Código do Fornecedor")]
        [Required(ErrorMessage = "O código do fornecedor é obrigatório")]
        public uint IdFornecedor { get; set; } // Identificador do fornecedor

        public FornecedorViewModel Fornecedor { get; set; } = new FornecedorViewModel(); // Representação do fornecedor

        [Display(Name = "Código da Instituição")]
        [Required(ErrorMessage = "O código da instituição é obrigatório")]
        public uint IdInstituicao { get; set; } // Identificador da instituição

        public InstituicaoViewModel Instituicao { get; set; } = new InstituicaoViewModel(); // Representação da instituição

    }
}
