using DutchTreat.Data.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }      
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        public string ProductCategory { get; set; }
        public string ProductSize { get; set; }      
        public string ProductTitle { get; set; }   
        public string ProductArtist { get; set; }

        public string ProductArtId { get; set; }
        
        // Daca se prefixeaza numele proprietati cu numele entitatii din care face parte (vezi toate proprietatile care incep cu Product),
        
        // automat valorile acestor propietati se fill-uiesc cu valorile corespunzatore din tabela cu acelasi nume cu al entitatii 
        
        // de care corespondente fara sa fie nevoie de configurarea mapper-ul.

    }
}