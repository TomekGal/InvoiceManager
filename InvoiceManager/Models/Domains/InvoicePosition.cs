using System.ComponentModel.DataAnnotations;


namespace InvoiceManager.Models.Domains
{
    public class InvoicePosition
    {
        
        public int Id { get; set; }

        public int Lp { get; set; }

        public int InvoiceId { get; set; }

        [Display(Name ="Wartośc")]
        [Required(ErrorMessage = "Pole Wartośc jest wymagane.")]
        public decimal Value { get; set; }

        [Display (Name ="Produkt")]
        [Required(ErrorMessage = "Pole Produkt jest wymagane.")]
        public int ProductId { get; set; }

        [Display (Name ="Ilość")]
        [Required(ErrorMessage = "Pole Ilość jest wymagane.")]
        public int Quantity { get; set; }

        public Invoice Invoice { get; set; }
        public Product Product { get; set; }
    }
}