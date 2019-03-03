using System;

namespace TestProject.Domain.Models
{
    public class StoreManagerPutModel : StoreManagerPostModel
    {
        public Guid Id { get; set; }
    }
}
