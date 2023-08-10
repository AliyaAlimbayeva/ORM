using System;
using System.ComponentModel.DataAnnotations;

namespace EFDemo.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; } 
        public DateTime UpdatedDate { get; set; }
        [Required]
        public int ProductId { get; set; }
    }
    public enum OrderStatus
    {
        NotStarted,
        Loading,
        InProgress,
        Arrived,
        Unloading,
        Cancelled,
        Done
    }
}
