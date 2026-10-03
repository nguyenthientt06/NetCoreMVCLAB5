using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCoreMVCLAB5.Models
{
    public class Product : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Tên sản phẩm")]
        [Required(ErrorMessage = "Tên sản phẩm bắt buộc nhập")]
        [StringLength(150, MinimumLength = 6, ErrorMessage = "Tên sản phẩm phải từ 6 đến 150 ký tự")]
        public string Name { get; set; }

        [Display(Name = "Ảnh sản phẩm")]
        [Required(ErrorMessage = "Vui lòng chọn ảnh sản phẩm")]
        public string Image { get; set; }

        [Display(Name = "Giá chuẩn")]
        [Required(ErrorMessage = "Giá chuẩn bắt buộc nhập")]
        [Range(100000, double.MaxValue, ErrorMessage = "Giá chuẩn phải từ 100,000 trở lên")]
        public float Price { get; set; }

        [Display(Name = "Giá khuyến mãi")]
        [Required(ErrorMessage = "Giá khuyến mãi bắt buộc nhập")]
        [Range(0, double.MaxValue, ErrorMessage = "Giá khuyến mãi không được âm")]
        public float SalePrice { get; set; }

        [Display(Name = "Mô tả sản phẩm")]
        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(1500, ErrorMessage = "Mô tả không được vượt quá 1500 ký tự")]
        public string Description { get; set; }

        [Display(Name = "Danh mục sản phẩm")]
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        // Custom validation cho SalePrice và từ nhạy cảm trong Description
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // SalePrice phải nhỏ hơn giá chuẩn 10%
            if (SalePrice >= Price * 0.9f)
            {
                yield return new ValidationResult("Giá khuyến mãi phải nhỏ hơn giá chuẩn ít nhất 10%", new[] { nameof(SalePrice) });
            }

            // Kiểm tra từ nhạy cảm trong Description
            if (!string.IsNullOrEmpty(Description))
            {
                string[] badWords = { "die", "admin", "fack" };
                foreach (var word in badWords)
                {
                    if (Description.ToLower().Contains(word))
                    {
                        yield return new ValidationResult($"Mô tả chứa từ nhạy cảm không được phép ('{word}')", new[] { nameof(Description) });
                    }
                }
            }
        }
    }
}