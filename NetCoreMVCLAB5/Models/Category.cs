using System.ComponentModel.DataAnnotations;

namespace NetCoreMVCLAB5.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên danh mục")]
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        public string Name { get; set; }
    }
}