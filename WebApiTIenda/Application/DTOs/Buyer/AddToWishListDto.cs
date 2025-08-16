using System.ComponentModel.DataAnnotations;

namespace WebApiTIenda.Application.DTOs.Buyer
{
    public class AddToWishListDto
    {
        [Required]
        public int IdProduct { get; set; }
    }
}
