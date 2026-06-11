using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class UsoanestesicoViewModel
    {
        [Key]
        public uint Id { get; set; }

        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        public decimal Quantidade { get; set; }

        [Display(Name = "Procedimento")]
        [Required(ErrorMessage = "O procedimento é obrigatório.")]
        [StringLength(255, ErrorMessage = "O procedimento deve ter no máximo 255 caracteres.")]
        public string Procedimento { get; set; } = null!;

        [Display(Name = "Data")]
        [Required(ErrorMessage = "A data é obrigatória.")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Display(Name = "Cepa")]
        [Required(ErrorMessage = "A cepa é obrigatória.")]
        [StringLength(50, ErrorMessage = "A cepa deve ter no máximo 50 caracteres.")]
        public string Cepa { get; set; } = null!;

        [Display(Name = "Número de Animais")]
        [Required(ErrorMessage = "O número de animais é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "Informe um número de animais válido.")]
        public int NumeroAnimais { get; set; }

        [Display(Name = "Pesquisador")]
        [Required(ErrorMessage = "O pesquisador é obrigatório.")]
        public uint IdPesquisador { get; set; }

        [Display(Name = "Experimento")]
        [Required(ErrorMessage = "O experimento é obrigatório.")]
        public uint IdExperimento { get; set; }

        [Display(Name = "Entrada")]
        [Required(ErrorMessage = "A entrada é obrigatória.")]
        public uint IdEntrada { get; set; }

        [Display(Name = "Anestésico")]
        [Required(ErrorMessage = "O anestésico é obrigatório.")]
        public uint IdAnestesico { get; set; }

        [Display(Name = "Pesquisador")]
        public string? NomePesquisador { get; set; }

        [Display(Name = "Experimento")]
        public string? NomeExperimento { get; set; }

        [Display(Name = "Anestésico")]
        public string? NomeAnestesico { get; set; }

        [Display(Name = "Lote")]
        public string? Lote { get; set; }

        [Display(Name = "Data da Entrada")]
        [DataType(DataType.Date)]
        public DateTime? DataEntrada { get; set; }
    }
}