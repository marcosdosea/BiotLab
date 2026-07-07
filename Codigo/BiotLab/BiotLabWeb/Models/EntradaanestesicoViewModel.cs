using System;
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class EntradaanestesicoViewModel
    {
        [Display(Name = "Entrada")]
        [Required(ErrorMessage = "A entrada é obrigatória.")]
        public uint IdEntrada { get; set; }

        [Display(Name = "Anestésico")]
        [Required(ErrorMessage = "O anestésico é obrigatório.")]
        public uint IdAnestesico { get; set; }

        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(typeof(decimal), "0,01", "999999999", ErrorMessage = "A quantidade deve ser maior que zero.")]
        public decimal Quantidade { get; set; }

        [Display(Name = "Lote")]
        [Required(ErrorMessage = "O lote é obrigatório.")]
        [StringLength(50, ErrorMessage = "O lote deve ter no máximo 50 caracteres.")]
        public string Lote { get; set; } = string.Empty;

        [Display(Name = "Valor Unitário")]
        [Required(ErrorMessage = "O valor unitário é obrigatório.")]
        [Range(typeof(decimal), "0,01", "999999999", ErrorMessage = "O valor unitário deve ser maior que zero.")]
        public decimal ValorUnitario { get; set; }

        [Display(Name = "Subtotal")]
        public decimal SubTotal { get; set; }

        [Display(Name = "Anestésico")]
        public string? NomeAnestesico { get; set; }

        [Display(Name = "Data da Entrada")]
        [DataType(DataType.Date)]
        public DateTime? DataEntrada { get; set; }
    }
}