using Global.Models.Filter;

namespace CMS.APIGateway.Models.FeedbackAndRequest
{
    public class FeedbackAndRequestFilterModel : RequestFilterModel
    {
        public string CustomerId { get; set; }
        public string Source { get; set; }
    }
}
