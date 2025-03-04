using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Microblog.Posts
{
    //public class CreatePostDto
    //{
    //    [StringLength(140)]
    //    public required string Content { get; set; }

    //    public IFormFile Image { get; set; }
    //}

    public class CreatePostDto
    {
        [Required(ErrorMessage = "Post content is required")]
        [StringLength(140, ErrorMessage = "Post content cannot exceed 140 characters")]
        public required string Content { get; set; }

        [DynamicMaxFileSize(2 * 1024 * 1024)] // 2MB max file size
        [DynamicAllowedFileExtensions(new[] { ".jpg", ".jpeg", ".png", ".webp" })] // Allowed extensions
        public IFormFile? Image { get; set; }
    }

    // Custom validation attributes
    public class DynamicMaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public DynamicMaxFileSizeAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxFileSize)
            {
                return new ValidationResult($"The file size exceeds the limit of {_maxFileSize / (1024 * 1024)} MB.");
            }

            return ValidationResult.Success;
        }
    }

    public class DynamicAllowedFileExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public DynamicAllowedFileExtensionsAttribute(string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > 0)
            {
                var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

                if (Array.IndexOf(_allowedExtensions, extension) == -1)
                {
                    return new ValidationResult($"Only {string.Join(", ", _allowedExtensions)} file types are allowed.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
