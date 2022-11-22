using FeedbackAndReports.API.Context;
using FeedbackAndReports.API.Reponsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAndReports.API.Repository
{
    public class FeedbackRepository : BaseRepository<Model.FeedbackModel>, IFeedbackRepository
    {
        public FeedbackRepository(IMongoContext context) : base(context)
        {
        }
    }
}
