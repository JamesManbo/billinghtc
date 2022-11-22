using FeedbackAndReports.API.Reponsitory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeedbackAndReports.API.Repository
{
    public interface IFeedbackRepository : IRepository<Model.FeedbackModel>
    {
    }
}
