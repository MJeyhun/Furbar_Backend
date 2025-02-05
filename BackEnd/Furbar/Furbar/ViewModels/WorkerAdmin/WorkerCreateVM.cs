namespace Furbar.ViewModels.WorkerAdmin
{
    public class WorkerCreateVM
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string? FullName { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? Position { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public IFormFile? Image { get; set; }
    }
}
