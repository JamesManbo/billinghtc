using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContractManagement.API.Protos;
using Feedback.API.Grpc.Client;
using Feedback.API.Models;
using GenericRepository.Extensions;
using Global.Models.Filter;
using Global.Models.Notification;
using Global.Models.PagedList;
using Global.Models.StateChangedResponse;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoClusterRepository;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Feedback.API.Repository
{
    public interface IFeedbackAndRequestRepository
    {
        Task<ActionResponse> Create(FeedbackAndRequest feedbackAndRequestRequest);
        Task<ActionResponse> Update(FeedbackAndRequest feedbackAndRequest);
        Task<IPagedList<FeedbackAndRequest>> GetList(FeedbackAndRequestFilterModel map);
        Task<ActionResponse> MarkFeedbacksAsResolved(string[] feedbackIds);
        Task Remove(FeedbackAndRequest feedbackAndRequest);
        Task Remove(string id);
    }
    public class FeedbackAndRequestRepository : IFeedbackAndRequestRepository
    {
        // _httpClient isn't exposed publicly
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly INotificationGrpcService _notificationGrpcService;
        private readonly IOutContractServiceGrpc _outContractServiceGrpc;
        private readonly IMongoClusterCollection<FeedbackAndRequest> _collection;
        private readonly ILogger<FeedbackAndRequestRepository> _logger;
        private readonly string CurrentEnvironment;

        public FeedbackAndRequestRepository(IFeedbackMongoDbContext context,
            IHttpClientFactory httpClientFactory, IMapper mapper,
            INotificationGrpcService notificationGrpcService,
            IOutContractServiceGrpc outContractServiceGrpc,
            ILogger<FeedbackAndRequestRepository> logger)
        {
            _collection = context.GetCollection<FeedbackAndRequest>();
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
            this._notificationGrpcService = notificationGrpcService;
            this._outContractServiceGrpc = outContractServiceGrpc;
            this._logger = logger;
            this.CurrentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }

        public async Task<ActionResponse> Create(FeedbackAndRequest feedbackAndRequest)
        {
            var actionResponse = new ActionResponse();
            feedbackAndRequest.DateCreated = DateTime.UtcNow;
            try
            {
                ContractGrpcDTO contractInfoModel;
                OutContractServicePackageGrpcDTO channelInfoModel;

                if (feedbackAndRequest.OutContractServicePackageId.HasValue
                    && feedbackAndRequest.OutContractServicePackageId > 0)
                {
                    contractInfoModel = await _outContractServiceGrpc.GetOutContractByChannelId(feedbackAndRequest.OutContractServicePackageId.Value);
                    channelInfoModel = contractInfoModel.ServicePackages.First(c => feedbackAndRequest.OutContractServicePackageId == feedbackAndRequest.OutContractServicePackageId);
                }
                else
                {
                    contractInfoModel = await _outContractServiceGrpc.GetOutContractByChannelCId(feedbackAndRequest.CId);
                    channelInfoModel = contractInfoModel.ServicePackages.First(c => feedbackAndRequest.CId.Equals(c.CId, StringComparison.OrdinalIgnoreCase));
                }

                feedbackAndRequest.StartTime = feedbackAndRequest.StartTime.HasValue ? feedbackAndRequest.StartTime.Value : DateTime.UtcNow.AddHours(7);
                feedbackAndRequest.RequestCode = Guid.NewGuid().ToString();
                feedbackAndRequest.GlobalId = string.IsNullOrEmpty(feedbackAndRequest.GlobalId) ? Guid.NewGuid().ToString() : feedbackAndRequest.GlobalId;

                if (channelInfoModel.HasStartAndEndPoint)
                {
                    feedbackAndRequest.Address = $"Điểm đầu: {channelInfoModel.StartPoint.InstallationAddress.FullAddress}.";
                    feedbackAndRequest.Address += $"Điểm cuối: {channelInfoModel.EndPoint.InstallationAddress.FullAddress}";
                }
                else
                {
                    feedbackAndRequest.Address = channelInfoModel.EndPoint.InstallationAddress.FullAddress;
                }

                feedbackAndRequest.City = channelInfoModel.EndPoint.InstallationAddress.City;
                feedbackAndRequest.CityId = channelInfoModel.EndPoint.InstallationAddress.CityId;
                feedbackAndRequest.District = channelInfoModel.EndPoint.InstallationAddress.District;
                feedbackAndRequest.DistrictId = channelInfoModel.EndPoint.InstallationAddress.DistrictId;

                feedbackAndRequest.ContractCode = contractInfoModel.ContractCode;
                feedbackAndRequest.ContractId = contractInfoModel.Id?.ToString();
                feedbackAndRequest.CustomerId = contractInfoModel.Contractor.ApplicationUserIdentityGuid;
                feedbackAndRequest.CustomerCode = contractInfoModel.Contractor.ContractorCode;
                feedbackAndRequest.CId = channelInfoModel.CId;
                feedbackAndRequest.OutContractServicePackageId = channelInfoModel.Id;

                await _collection.InsertOneAsync(feedbackAndRequest);
                if (feedbackAndRequest.Source != "HTC_ITC_Ticket_System"
                    && this.CurrentEnvironment == Environments.Production)
                {
                    await SendToHTCTicketSystem(feedbackAndRequest);
                }

                actionResponse.Message = "Thêm mới báo cáo sự cố thành công";
            }
            catch (Exception e)
            {
                actionResponse.AddError(e.Message);
                _logger.LogError("CREATE Feedback has an error: {0}", e);
            }

            return actionResponse;
        }

        public async Task<ActionResponse> Update(FeedbackAndRequest feedbackAndRequest)
        {
            var actionResponse = new ActionResponse();
            try
            {
                var filterBuilder = Builders<FeedbackAndRequest>.Filter;

                FilterDefinition<FeedbackAndRequest> filter = null;
                if (string.IsNullOrWhiteSpace(feedbackAndRequest.GlobalId))
                {
                    filter = filterBuilder.Eq("Id", feedbackAndRequest.Id);
                }
                else
                {
                    filter = filterBuilder.Eq("GlobalId", feedbackAndRequest.GlobalId);
                }

                // Lấy thông tin đối tượng cần cập nhật
                var findFarQuery = await _collection.FindAsync<FeedbackAndRequest>(filter);
                var toUpdateFar = findFarQuery.FirstOrDefault();
                bool notifyRequestHandled = toUpdateFar.Status == 0 && feedbackAndRequest.Status == 1;
                if (feedbackAndRequest.UpdateFrom != "HTC_ITC_Ticket_System")
                {
                    toUpdateFar.Status = feedbackAndRequest.Status;
                    toUpdateFar.StartTime = feedbackAndRequest.StartTime;
                    toUpdateFar.Note = feedbackAndRequest.Note;
                    toUpdateFar.Title = feedbackAndRequest.Title;
                    toUpdateFar.Content = feedbackAndRequest.Content;
                    toUpdateFar.ReceiptLineId = feedbackAndRequest.ReceiptLineId;
                    toUpdateFar.ContractId = feedbackAndRequest.ContractId;
                    toUpdateFar.ContractCode = feedbackAndRequest.ContractCode;
                    toUpdateFar.OutContractServicePackageId = feedbackAndRequest.OutContractServicePackageId;
                    toUpdateFar.Service = feedbackAndRequest.Service;
                    toUpdateFar.ServicePackage = feedbackAndRequest.ServicePackage;
                    toUpdateFar.Address = feedbackAndRequest.Address;
                    toUpdateFar.District = feedbackAndRequest.District;
                    toUpdateFar.DistrictId = feedbackAndRequest.DistrictId;
                    toUpdateFar.City = feedbackAndRequest.City;
                    toUpdateFar.CityId = feedbackAndRequest.CityId;
                    toUpdateFar.DateUpdated = DateTime.UtcNow;
                    toUpdateFar.Handled = toUpdateFar.Status == 1;
                    toUpdateFar.CustomerRate = feedbackAndRequest.CustomerRate;
                    toUpdateFar.CustomerComment = feedbackAndRequest.CustomerComment;
                }
                else
                {
                    if (feedbackAndRequest.StopTime != null)
                    {
                        toUpdateFar.StopTime = feedbackAndRequest.StopTime;
                        toUpdateFar.Duration = feedbackAndRequest.Duration;
                        toUpdateFar.Status = feedbackAndRequest.Status;
                        toUpdateFar.DateUpdated = DateTime.UtcNow;
                        toUpdateFar.Handled = toUpdateFar.Status == 1;
                    }
                }

                // Gửi thông báo đến khách hàng thông báo sự cố đã được xử lý
                if (notifyRequestHandled)
                {
                    var requestHandledNotification = new PushNotificationRequest()
                    {
                        Zone = NotificationZone.FeedBack,
                        Type = NotificationType.App,
                        Category = NotificationCategory.Problem,
                        Title = "Thông báo sự cố",
                        Content = $"HTC-ITC xin thông báo.\nSự cố {feedbackAndRequest.Content} đã được đội ngũ kỹ thuật xử lý.",
                        Payload = JsonConvert.SerializeObject(new
                        {
                            Id = toUpdateFar.Id,
                            GlobalId = toUpdateFar.GlobalId,
                            Category = NotificationCategory.Problem
                        },
                        new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver(),
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        })
                    };

                    await _notificationGrpcService.PushNotificationByCustomerUids(requestHandledNotification, toUpdateFar.CustomerIdentityGuid);
                }

                await _collection.ReplaceOneAsync(filter, toUpdateFar);
                return actionResponse;
            }
            catch (Exception e)
            {
                actionResponse.AddError(e.Message);
                _logger.LogError("UPDATE Feedback has an error: {0}", e);
            }

            return actionResponse;
        }

        public async Task<IPagedList<FeedbackAndRequest>> GetList()
        {
            var filterBuilder = Builders<FeedbackAndRequest>.Filter;
            var subSetResult = await _collection.FindAsync<FeedbackAndRequest>(filterBuilder.Empty);
            var totalRecords = (int)await _collection.EstimatedDocumentCountAsync();
            var result = new PagedList<FeedbackAndRequest>(0, 10, totalRecords)
            {
                Subset = subSetResult.ToList()
            };

            return result;
        }

        public async Task<ActionResponse> MarkFeedbacksAsResolved(string[] feedbackIds)
        {
            var builder = Builders<FeedbackAndRequest>.Filter;
            var filters = builder.Where(c => feedbackIds.Contains(c.Id));

            var updateDefinition = Builders<FeedbackAndRequest>.Update
                .Set(x => x.Status, (int)FeedbackAndRequestStatus.Resolved)
                .Set(x => x.Handled, true);

            var updateResult = await _collection.UpdateManyAsync(filters, updateDefinition);
            return ActionResponse.Success;
        }

        public async Task Remove(Models.FeedbackAndRequest feedbackAndRequest)
        {
            var filterBuilder = Builders<FeedbackAndRequest>.Filter;
            var filter = filterBuilder.Eq("Id", feedbackAndRequest.Id);
            await _collection.DeleteOneAsync(filter);
        }
        public async Task Remove(string id)
        {
            var filterBuilder = Builders<FeedbackAndRequest>.Filter;
            var filter = filterBuilder.Eq("Id", id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IPagedList<FeedbackAndRequest>> GetList(FeedbackAndRequestFilterModel filterModel)
        {
            var builder = Builders<FeedbackAndRequest>.Filter;
            var filters = builder.Empty;

            if (filterModel.Filters != null)
            {
                CreateFilter(filterModel, builder, ref filters);
            }

            if (!string.IsNullOrEmpty(filterModel.CustomerId))
            {
                filters = filters & builder.Eq("CustomerId", filterModel.CustomerId);
            }

            if (!string.IsNullOrEmpty(filterModel.CreatedBy))
            {
                filters = filters & builder.Eq("CreatedBy", filterModel.CreatedBy);
            }

            if (!string.IsNullOrEmpty(filterModel.Source))
            {
                filters = filters & builder.Eq("source", filterModel.Source);
            }

            var sortBuilder = Builders<FeedbackAndRequest>.Sort;
            SortDefinition<FeedbackAndRequest> sort;
            if (filterModel.Dir.Equals("desc", StringComparison.OrdinalIgnoreCase))
            {
                sort = sortBuilder.Descending(filterModel.OrderBy);
            }
            else
            {
                sort = sortBuilder.Ascending(filterModel.OrderBy);
            }

            var findOptions = new FindOptions<FeedbackAndRequest, FeedbackAndRequest>()
            {
                Sort = sort,
                Limit = filterModel.Take,
                Skip = filterModel.Skip
            };

            var subSetResult = await _collection
                .FindAsync(filters, findOptions);

            var totalRecords = (int)await _collection.CountDocumentsAsync(filters);

            var result = new PagedList<FeedbackAndRequest>(
                filterModel.Skip,
                filterModel.Take,
                totalRecords
            )
            {
                Subset = subSetResult.ToList()
            };

            return result;
        }

        private void CreateFilter(RequestFilterModel filterModel, FilterDefinitionBuilder<FeedbackAndRequest> builder,
                                        ref FilterDefinition<FeedbackAndRequest> filters)
        {
            foreach (string item in filterModel.Filters.Split("|"))
            {
                string[] filter = item.Split("::");
                switch (filter[2])
                {
                    case "contains":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        else
                        {
                            filters = builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        break;
                    case "doesnotcontain":
                        if (filters != builder.Empty)
                        {
                            filters = filters & !builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        else
                        {
                            filters = !builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(filter[1]));
                        }
                        break;
                    case "eq":
                        try
                        {
                            if (bool.Parse(filter[1]))
                            {
                                if (filters != builder.Empty)
                                {
                                    filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                                }
                                else
                                {
                                    filters = builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                                }
                            }
                            else
                            {
                                if (filters != builder.Empty)
                                {
                                    filters = builder.And(filters, builder.Or(builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]), builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value)));
                                }
                                else
                                {
                                    filters = builder.Or(builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]), builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value));
                                }
                            }
                        }
                        catch
                        {
                            if (filters != builder.Empty)
                            {
                                filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                            }
                            else
                            {
                                filters = builder.Eq(filter[0].ToUpperFirstLetter(), filter[1]);
                            }
                        }


                        break;
                    case "neq":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Ne(filter[0].ToUpperFirstLetter(), filter[1]);
                        }
                        else
                        {
                            filters = builder.Ne(filter[0].ToUpperFirstLetter(), filter[1]);
                        }
                        break;
                    case "startswith":
                        string fil = filter[1].ToString();
                        fil = "^" + fil;
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(fil));
                        }
                        else
                        {
                            filters = builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(fil));
                        }
                        break;
                    case "endswith":
                        string file = filter[1].ToString();
                        file = file + "^";
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(file));
                        }
                        else
                        {
                            filters = builder.Regex(filter[0].ToUpperFirstLetter(), new BsonRegularExpression(file));
                        }
                        break;
                    case "isnull":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);
                        }
                        else
                        {
                            filters = builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);

                        }
                        break;
                    case "isnotnull":
                        if (filters != builder.Empty)
                        {
                            filters = filters & !builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);
                        }
                        else
                        {
                            filters = !builder.Eq(filter[0].ToUpperFirstLetter(), BsonNull.Value);
                        }
                        break;
                    case "isempty":
                        if (filters != builder.Empty)
                        {
                            filters = filters & builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        else
                        {
                            filters = builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        break;
                    case "isnotempty":
                        if (filters != builder.Empty)
                        {
                            filters = filters & !builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        else
                        {
                            filters = !builder.Eq(filter[0].ToUpperFirstLetter(), string.Empty);
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        public async Task SendToHTCTicketSystem(FeedbackAndRequest feedbackItem)
        {
            try
            {
                var httpClient = this._httpClientFactory.CreateClient("htcticket");
                var requestBodyModel = this._mapper.Map<HTCTicketTransferModel>(feedbackItem);
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                httpClient.DefaultRequestHeaders.Add("AccessToken", this.EncryptSHA256(requestBodyModel.code, "AAAAB3NzaC1yc2EAAAABJQAAAQEAjObBH8pzt"));

                var todoItemJson = new StringContent(
                     JsonConvert.SerializeObject(requestBodyModel),
                     Encoding.UTF8,
                     "application/json");

                using var httpResponse =
                    await httpClient.PostAsync(@"api/billing/insert", todoItemJson);

                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                }
                httpResponse.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        public string EncryptSHA256(string s, string cryptoKey)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            var encoding = new ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(cryptoKey);
            byte[] messageBytes = encoding.GetBytes(s);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
