using EntityLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EntityLayer.ViewModels
{
    public class DiscountVm
    {
        public Discount Discount { get; set; } = new Discount();
        public IEnumerable<SelectListItem>? Products { get; set; } 
    }
}
