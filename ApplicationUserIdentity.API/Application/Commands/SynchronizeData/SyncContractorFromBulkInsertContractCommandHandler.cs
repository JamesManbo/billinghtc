using ApplicationUserIdentity.API.Infrastructure.Helper;
using ApplicationUserIdentity.API.Infrastructure.Queries;
using ApplicationUserIdentity.API.Infrastructure.Repositories;
using ApplicationUserIdentity.API.Models;
using ApplicationUserIdentity.API.Models.Configs;
using ApplicationUserIdentity.API.Services.GRPC.Clients;
using AutoMapper;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApplicationUserIdentity.API.Application.Commands.SynchronizeData
{
    public class SyncContractorFromBulkInsertContractCommandHandler : IRequestHandler<SyncContractorFromBulkInsertContractCommand, ActionResponse>
    {
        private readonly IContractorGrpcService _contractorGrpcService;
        private readonly IUserRepository _userRepository;
        private readonly IUserQueries _userQueries;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;

        public SyncContractorFromBulkInsertContractCommandHandler(
            IContractorGrpcService contractorGrpcService,
            IUserRepository userRepository,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IUserQueries userQueries)
        {
            this._contractorGrpcService = contractorGrpcService;
            this._userRepository = userRepository;
            this._mapper = mapper;
            this._appSettings = appSettings.Value;
            this._userQueries = userQueries;
        }

        public async Task<ActionResponse> Handle(SyncContractorFromBulkInsertContractCommand request, CancellationToken cancellationToken)
        {
            var timer = new Stopwatch();
            timer.Start();
            var bulkInsertedContractors = await _contractorGrpcService.GetFromId(request.FromContractorId);
            timer.Stop();
            Console.WriteLine("Fetch data time: " + timer.Elapsed.ToString(@"m\:ss"));
            timer.Reset();

            timer.Start();
            var importApplicationUsers = _mapper.Map<List<ApplicationUser>>(bulkInsertedContractors.ToList());
            var existedUserNames = _userQueries.GetAllUserName();

            foreach (var user in importApplicationUsers)
            {
                user.CreatedBy = "Hệ thống";
                user.CreatedDate = DateTime.UtcNow;
                user.Password = $"{user.PasswordSalt}{user.Password}".EncryptMD5(_appSettings.MD5CryptoKey);

                if (!existedUserNames.Contains(user.UserName))
                {
                    existedUserNames.Add(user.UserName);
                    continue;
                }

                // Append index siffix to prevent dupplicate username
                var suffix = 1;
                while (existedUserNames.Contains(user.UserName))
                {
                    user.UserName = $"{user.UserName}{suffix:D2}";
                    user.Password = user.UserName;
                    user.Password = $"{user.PasswordSalt}{user.Password}".EncryptMD5(_appSettings.MD5CryptoKey);
                    suffix++;
                }

                existedUserNames.Add(user.UserName);
            }
            timer.Stop();

            Console.WriteLine("Mapping data time: " + timer.Elapsed.ToString(@"m\:ss"));

            var formatedImportingModels = importApplicationUsers.Select(d => (object)d).ToList();
            var insertedCount = await _userRepository.InsertBulk(formatedImportingModels);
            Console.WriteLine(insertedCount);
            return ActionResponse.Success;
        }
    }
}
