using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFDemo.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Column(TypeName = "decimal(5,2)")] 
        public decimal Weight { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Height { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Width { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal Length { get; set; }
    }
}
