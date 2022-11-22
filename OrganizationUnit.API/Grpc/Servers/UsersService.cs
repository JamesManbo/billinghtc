using AutoMapper;
using Global.Models.Filter;
using Global.Models.StateChangedResponse;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Newtonsoft.Json;
using OrganizationUnit.API.Protos.Users;
using OrganizationUnit.Domain.Models.User;
using OrganizationUnit.Infrastructure.Queries;
using OrganizationUnit.Infrastructure.Repositories;
using OrganizationUnit.Infrastructure.Repositories.ConfigurationSettingUserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrganizationUnit.API.Grpc.Servers
{
    public class UsersService : UsersGrpc.UsersGrpcBase
    {
        private readonly IUserQueries _userQueries;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersService(IUserQueries userQueries, IMapper mapper, IUserRepository userRepository)
        {
            _userQueries = userQueries;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public override Task<ResultGrpc> GetList(UserRequestFilterModelGrpc filterModelGrpc, ServerCallContext context)
        {
            if (filterModelGrpc.Type == Protos.Users.RequestType.Selection)
            {
                return Task.FromResult(new ResultGrpc()
                {
                    Result = JsonConvert.SerializeObject(_userQueries.GetSelectionList())
                });
            }

            if (filterModelGrpc.Type == Protos.Users.RequestType.Autocomplete)
            {
                return Task.FromResult(new ResultGrpc()
                {
                    Result = JsonConvert.SerializeObject(_userQueries.Autocomplete(_mapper.Map<RequestFilterModel>(filterModelGrpc)))
                });
            }

            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetList(_mapper.Map<UserRequestFilterModel>(filterModelGrpc)))
            });
        }

        public override Task<ResultGrpc> GetListByRoleCode(StringValue request, ServerCallContext context)
        {
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetAllUserByRole(request.Value))
            });
        }

        public override Task<ResultGrpc> GetListTypeSelection(IsPartnerGrpc isPartner, ServerCallContext context)
        {
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetListTypeSelection(isPartner.IsPartner, ""))
            });
        }

        public override Task<ResultGrpc> GetByUid(StringValue request, ServerCallContext context)
        {
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.FindById(request.Value))
            });
        }

        public override Task<ListTokenResponseGrpc> GetFCMTokensByRoleUser(StringValue request, ServerCallContext context)
        {
            var tokens = _userQueries.GetListTokenByRoleUser(request.Value);
            var rs = new ListTokenResponseGrpc();
            tokens.ForEach(tk =>
            {
                rs.Tokens.Add(_mapper.Map<FcmTokenGrpc>(tk));
            });
            return Task.FromResult(rs);
        }

        public override Task<ListTokenResponseGrpc> GetFCMTokensByUids(StringValue request, ServerCallContext context)
        {
            var tokens = _userQueries.GetUserFCMTokens(request.Value);
            var result = new ListTokenResponseGrpc();
            if (tokens != null && tokens.Count > 0)
            {
                result.Tokens.AddRange(tokens.Select(_mapper.Map<FcmTokenGrpc>));
            }
            return Task.FromResult(result);
        }

        public override Task<ListTokenResponseGrpc> GetFCMTokensByDepartent(StringValue request, ServerCallContext context)
        {
            var tokens = _userQueries.GetListTokenByDepartment(request.Value);
            var result = new ListTokenResponseGrpc();
            if (tokens != null && tokens.Count > 0)
            {
                result.Tokens.AddRange(tokens.Select(_mapper.Map<FcmTokenGrpc>));
            }

            return Task.FromResult(result);
        }

        public override Task<ResultGrpc> GetUserByIds(StringValue request, ServerCallContext context)
        {
            var userIds = request.Value.Split(',').Select(int.Parse).ToArray();
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetUserByIds(userIds))
            });
        }

        public override async Task<SettingAccountResponseGrpc> ChangeSettingAccount(SettingAccountCommandGrpc request, ServerCallContext context)
        {
            if (string.IsNullOrEmpty(request.IdentityGuid)) return new SettingAccountResponseGrpc { IsSuccess = false };

            var user = await _userRepository.GetByIdAsync(request.UserId);
            user.FullName = request.FullName;
            user.MobilePhoneNo = request.MobilePhoneNo;
            user.Email = request.Email;
            if (user.ConfigurationAccount != null)
            {
                user.ConfigurationAccount.AllowSendEmail = request.AllowSendEmail;
                user.ConfigurationAccount.AllowSendSMS = request.AllowSendSMS;
                user.ConfigurationAccount.AllowSendNotification = request.AllowSendNotification;
            }
            else
            {
                user.ConfigurationAccount = new Domain.AggregateModels.ConfigurationUserAggregate.ConfigurationPersonalAccount
                {
                    AllowSendEmail = request.AllowSendEmail,
                    AllowSendSMS = request.AllowSendSMS,
                    AllowSendNotification = request.AllowSendNotification,
                    CreatedDate = DateTime.Now
                };
            }
            var rs = await _userRepository.UpdateAndSave(user);


            return new SettingAccountResponseGrpc { IsSuccess = rs.IsSuccess, Message = rs.Message };
        }

        public override Task<ResultGrpc> GetEmailsOfServiceProvider(StringValue code, ServerCallContext context)
        {
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetEmailsOfServiceProvider())
            });
        }

        public override Task<RepeatedUserDtoGrpc> GetManagementUser(StringValue request, ServerCallContext context)
        {
            var managers = this._userQueries.GetManagementUserByOrgUnit(request.Value);
            var result = new RepeatedUserDtoGrpc();
            if (managers != null && managers.Count() > 0)
            {
                result.Users.AddRange(managers.Select(_mapper.Map<UserDtoGrpc>));
            }
            return Task.FromResult(result);
        }

        public override Task<ResultGrpc> GetListByDepartmentCode(StringValue request, ServerCallContext context)
        {
            return Task.FromResult(new ResultGrpc()
            {
                Result = JsonConvert.SerializeObject(_userQueries.GetAllUserByDepartment(request.Value))
            });
        }
    }
}
