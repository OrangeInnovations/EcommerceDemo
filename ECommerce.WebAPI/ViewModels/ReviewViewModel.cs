using System;

namespace ECommerce.WebAPI.ViewModels
{
    public class ReviewViewModel 
    {
        public  string Id { get; set; }
        public int ProductId { get; set; }
        public string ProductTitle { get; set; }
        public string ReviewContent { get; set; }
        public string UserId { get; set; }
       
        public string ReviewerFullName { get; set; }

        public DateTimeOffset CreateDateTimeOffset { get; set; }
    }
}