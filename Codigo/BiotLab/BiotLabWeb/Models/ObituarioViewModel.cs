using System.ComponentModel.DataAnnotations;

namespace BiotLabWeb.Models
{
    public class ObituarioViewModel
    {
        [Display(Name = "Código do Animal")]
        [Required(ErrorMessage = "O código do animal é obrigatório")]
        [Key]
        public uint Id { get; set; }

        [Display(Name = "IdPesquisador")]
        [Required(ErrorMessage = "O Id do Pesquisador é obrigatório")]
        [StringLength(50, ErrorMessage = "O Id deve ter no máximo 100 caracteres")]
        public string IdPesquisador { get; set; } = null!;

        [Display(Name = "Data")]
        [Required(ErrorMessage = "O Data é obrigatório")]
        [RegularExpression(@"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", ErrorMessage = "Formato de Data inválido")]
        public string Data { get; set; } = null!;

        [Display(Name = "IdGaiola")]
        [Required(ErrorMessage = "O id da Gaiola é obrigatório")]
        [RegularExpression(@"\d{5}-\d{3}", ErrorMessage = "Formato do Id da Gaiola inválido")]
        public string IdGaiola { get; set; } = null!;

        [Display(Name = "Id Pesquisador Navigation")]
        [StringLength(50, ErrorMessage = "O Id Pesquisador Navigation deve ter no máximo 50 caracteres")]
        public string? IdPesquisadorNavigation { get; set; }

        [Display(Name = "Id Gaiola Navigation")]
        [StringLength(10, ErrorMessage = "O Id da Gaiola deve ter no máximo 10 caracteres")]
        public string? IdGaiolaNavigation { get; set; }
    }
}
