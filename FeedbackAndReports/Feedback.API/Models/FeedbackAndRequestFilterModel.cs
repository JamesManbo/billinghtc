using Global.Models.Filter;

namespace Feedback.API.Models
{
    public class FeedbackAndRequestFilterModel : RequestFilterModel
    {
        public string CId { get; set; }
        public string CustomerId { get; set; }
        public string Source { get; set; }
        public string CreatedBy { get; set; }
    }
}
