using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAndReports.API.Model
{
    public class FeedbackModel
    {
        public FeedbackModel()
        {
            FeedbackModel feedbackModel = new FeedbackModel();
        }

        public int Id { get; set; }
        public int CustomerId  { get; set; }
        public string ContractId { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }

    }
}
