
using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class EntradaanestesicoViewModel
    {
        [Display(Name = "ID da Entrada")]
        [Required(ErrorMessage = "O ID da entrada é obrigatório")]
        public uint IdEntrada { get; set; }

        [Display(Name = "ID do Anestésico")]
        [Required(ErrorMessage = "O ID do anestésico é obrigatório")]
        public uint IdAnestesico { get; set; }

        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "A quantidade é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero")]
        public decimal Quantidade { get; set; }

        [Display(Name = "Lote")]
        [Required(ErrorMessage = "O lote é obrigatório")]
        [StringLength(50, ErrorMessage = "O lote deve ter no máximo 50 caracteres")]
        public string Lote { get; set; } = null!;

        [Display(Name = "Valor Unitário")]
        [Required(ErrorMessage = "O valor unitário é obrigatório")]
        [DataType(DataType.Currency)]
        public decimal ValorUnitario { get; set; }

        [Display(Name = "Subtotal")]
        [DataType(DataType.Currency)]
        public decimal SubTotal { get; set; }

        // Propriedades adicionais para exibição
        [Display(Name = "Nome do Anestésico")]
        public string? NomeAnestesico { get; set; }

        [Display(Name = "Data da Entrada")]
        [DataType(DataType.Date)]
        public DateTime? DataEntrada { get; set; }
    }
}
