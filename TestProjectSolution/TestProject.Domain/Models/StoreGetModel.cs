using TestProject.Entities;

namespace TestProject.Domain.Models
{
    public class StoreGetModel : StorePutModel
    {
        public StoreManager StoreManager { get; set; }
        public Country Country { get; set; }
        public Stock Stock { get; set; }
    }
}
