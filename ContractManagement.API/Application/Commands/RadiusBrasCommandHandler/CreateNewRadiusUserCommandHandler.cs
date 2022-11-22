using ContractManagement.API.Application.Services.Radius;
using ContractManagement.Domain.Commands.RadiusAndBrasCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractServicePackageRepository;
using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.RadiusBrasCommandHandler
{
    public class CreateNewRadiusUserCommandHandler : IRequestHandler<CreateNewRadiusUserCommand, ActionResponse>
    {
        private readonly ILogger<CreateNewRadiusUserCommandHandler> _logger;
        //private readonly ITikConnection _tikConnection;
        private readonly IRadiusAndBrasManagementService _radiusManagementService;
        private readonly IRadiusServicePackageQueries _servicePackageQueries;
        private readonly IContractSrvPckRepository _contractServicePackageRepository;
        private readonly IContractorQueries _contractorQueries;
        private readonly IOutContractServicePackageQueries _contractSrvPckQueries;

        public CreateNewRadiusUserCommandHandler(
            IContractorQueries contractorQueries,
            IContractSrvPckRepository contractServicePackageRepository,
            IRadiusServicePackageQueries servicePackageQueries,
            ILogger<CreateNewRadiusUserCommandHandler> logger,
            IRadiusAndBrasManagementService radiusManagementService,
            IOutContractServicePackageQueries contractSrvPckQueries)
        {
            this._logger = logger;
            _contractorQueries = contractorQueries;
            _contractServicePackageRepository = contractServicePackageRepository;
            _servicePackageQueries = servicePackageQueries;
            this._radiusManagementService = radiusManagementService;
            this._contractSrvPckQueries = contractSrvPckQueries;
        }

        public async Task<ActionResponse> Handle(CreateNewRadiusUserCommand request, CancellationToken cancellationToken)
        {
            if (request.OutContractServicePackages != null && request.OutContractServicePackages.Any())
            {
                return await this.ContractHandle(request);
            }

            if (request.TransactionServicePackages != null && request.TransactionServicePackages.Any())
            {
                return await this.TransactionHandle(request);
            }

            return ActionResponse.Success;
        }

        private async Task<ActionResponse> ContractHandle(CreateNewRadiusUserCommand request)
        {
            foreach (var channel in request.OutContractServicePackages)
            {
                if (await _contractSrvPckQueries.IsRadiusAccountExisted(channel.RadiusAccount, channel.Id))
                {
                    return ActionResponse.Failed($"Tài khoản Radius {channel.RadiusAccount}" +
                        $" tại kênh \"{(string.IsNullOrEmpty(channel.CId) ? channel.ServiceName : channel.CId)}\" đã được sử dụng.");
                }
            }

            if (request.Contractor == null)
            {
                request.Contractor =
                    _contractorQueries.FindByContractorId(request.ContractId);
            }

            foreach (var outContractServicePck in request.OutContractServicePackages
                .Where(s => (s.Id == 0 || !s.IsRadiusAccountCreated)
                && !string.IsNullOrWhiteSpace(s.RadiusAccount)
                && !string.IsNullOrWhiteSpace(s.RadiusPassword)
                && s.ServicePackageId.HasValue))
            {
                var contractorFirstName = request.Contractor.ContractorFullName.Split(' ').LastOrDefault();
                var contractorLastName = string.Empty;
                if (request.Contractor.ContractorFullName.Split(' ').Length > 1)
                {
                    contractorLastName
                        = request
                            .Contractor.ContractorFullName
                            .Substring(0, request.Contractor.ContractorFullName.Length - contractorFirstName.Length - 1);
                }

                var validPhone = string.IsNullOrEmpty(request.Contractor.ContractorPhone)
                    ? string.Empty
                    : request.Contractor.ContractorPhone.Split(',')[0];

                validPhone = validPhone?.Substring(0, validPhone.Length > 15 ? 15 : validPhone.Length);

                var channelText = string.IsNullOrEmpty(outContractServicePck.CId)
                    ? $"{outContractServicePck.ServiceName}, {outContractServicePck.PackageName}"
                    : outContractServicePck.CId;

                if (outContractServicePck.RadiusAccount.Length > 64)
                {
                    return ActionResponse
                        .Failed($"Tài khoản Radius \"{outContractServicePck.RadiusAccount}\" tại kênh {channelText} quá dài(tối đa 64 ký tự).");
                }

                if (outContractServicePck.RadiusPassword.Length > 32)
                {
                    return ActionResponse
                        .Failed($"Mật khẩu tài khoản Radius \"{outContractServicePck.RadiusAccount}\" tại kênh {channelText} quá dài(tối đa 32 ký tự).");
                }

                var radiusUser = new RmUsers()
                {
                    Username = outContractServicePck.RadiusAccount,
                    Password = outContractServicePck.RadiusPassword,
                    Country = "Vietnam",
                    Address = request.Contractor.ContractorAddress?.Substring(0, request.Contractor.ContractorAddress.Length > 100 ? 100 : request.Contractor.ContractorAddress.Length) ?? string.Empty,
                    Zip = request.Contractor.ContractorCityId?.Substring(0, request.Contractor.ContractorCityId.Length > 6 ? 6 : request.Contractor.ContractorCityId.Length) ?? string.Empty,
                    Contractid = request.ContractCode.Substring(0, request.ContractCode.Length > 50 ? 50 : request.ContractCode.Length),
                    Firstname = contractorFirstName ?? string.Empty,
                    Lastname = contractorLastName ?? string.Empty,
                    Phone = validPhone,
                    Mobile = validPhone,
                    Verifymobile = validPhone,
                    Enableuser = 1,
                    Email = request.Contractor.ContractorEmail?.Substring(0, request.Contractor.ContractorEmail.Length > 100 ? 100 : request.Contractor.ContractorEmail.Length) ?? string.Empty,
                    Actcode = "",
                    Acctype = 0,
                    Contractvalid = DateTime.Now.AddYears(100),
                    Expiration = DateTime.Now.AddYears(100),
                    Alertemail = 1,
                    Alertsms = 1,
                    City = request.Contractor.ContractorCity ?? string.Empty,
                    Lang = "Vietnamese",
                    Ipmodecpe = 0,
                    Ipmodecm = 0,
                    Poolidcm = 0,
                    Poolidcpe = 0,
                    Staticipcpe = "",
                    Comment = request.Contractor.ContractorPhone,
                    Srvid = outContractServicePck.ServicePackageId.Value,
                    Taxid = request.Contractor.ContractorTaxIdNo ?? string.Empty,
                    Createdon = DateTime.UtcNow
                };

                var createRadiusRsp = await this._radiusManagementService.CreateUser(radiusUser);
                if (!createRadiusRsp.IsSuccess)
                {
                    return createRadiusRsp;
                }

                if (outContractServicePck.Id > 0)
                {
                    await this._contractServicePackageRepository.UpdateIsCreatedRadiusAccount(outContractServicePck.Id);
                }
            }

            return ActionResponse.Success;
        }


        private async Task<ActionResponse> TransactionHandle(CreateNewRadiusUserCommand request)
        {
            foreach (var channel in request.TransactionServicePackages)
            {
                if (await _contractSrvPckQueries.IsRadiusAccountExisted(channel.RadiusAccount, channel.Id))
                {
                    return ActionResponse.Failed($"Tài khoản Radius {channel.RadiusAccount} đã được sử dụng.");
                }
            }

            if (request.Contractor == null)
            {
                request.Contractor =
                    _contractorQueries.FindByContractorId(request.ContractId);
            }

            foreach (var tranServicePackage in request.TransactionServicePackages
                .Where(s => !string.IsNullOrWhiteSpace(s.RadiusAccount)
                    && !string.IsNullOrWhiteSpace(s.RadiusPassword)
                    && s.ServicePackageId.HasValue)
                )
            {
                var contractorFirstName = request.Contractor.ContractorFullName.Split(' ').LastOrDefault();
                var contractorLastName = string.Empty;
                if (request.Contractor.ContractorFullName.Split(' ').Length > 1)
                {
                    contractorLastName
                        = request
                            .Contractor.ContractorFullName
                            .Substring(0, request.Contractor.ContractorFullName.Length - contractorFirstName.Length - 1);
                }

                var validPhone = string.IsNullOrEmpty(request.Contractor.ContractorPhone)
                    ? string.Empty
                    : request.Contractor.ContractorPhone.Split(',')[0];

                validPhone = validPhone?.Substring(0, validPhone.Length > 15 ? 15 : validPhone.Length);


                if (tranServicePackage.RadiusAccount.Length > 64)
                {
                    return ActionResponse
                        .Failed($"Tên tài khoản Radius \"{tranServicePackage.RadiusAccount}\" quá dài(tối đa 64 ký tự).");
                }

                if (tranServicePackage.RadiusPassword.Length > 32)
                {
                    return ActionResponse
                        .Failed($"Mật khẩu tài khoản Radius \"{tranServicePackage.RadiusAccount}\" quá dài(tối đa 32 ký tự).");
                }

                var radiusUser = new RmUsers()
                {
                    Username = tranServicePackage.RadiusAccount,
                    Password = tranServicePackage.RadiusPassword,
                    Country = "Vietnam",
                    Address = request.Contractor.ContractorAddress?.Substring(0, request.Contractor.ContractorAddress.Length > 100 ? 100 : request.Contractor.ContractorAddress.Length) ?? string.Empty,
                    Zip = request.Contractor.ContractorCityId?.Substring(0, request.Contractor.ContractorCityId.Length > 6 ? 6 : request.Contractor.ContractorCityId.Length) ?? string.Empty,
                    Contractid = request.ContractCode.Substring(0, request.ContractCode.Length > 50 ? 50 : request.ContractCode.Length),
                    Firstname = contractorFirstName ?? string.Empty,
                    Lastname = contractorLastName ?? string.Empty,
                    Phone = validPhone,
                    Mobile = validPhone,
                    Verifymobile = validPhone,
                    Enableuser = 1,
                    Email = request.Contractor.ContractorEmail?.Substring(0, request.Contractor.ContractorEmail.Length > 100 ? 100 : request.Contractor.ContractorEmail.Length) ?? string.Empty,
                    Actcode = "",
                    Acctype = 0,
                    Contractvalid = DateTime.Now.AddYears(100),
                    Expiration = DateTime.Now.AddYears(100),
                    Alertemail = 1,
                    Alertsms = 1,
                    City = request.Contractor.ContractorCity ?? string.Empty,
                    Lang = "Vietnamese",
                    Ipmodecpe = 0,
                    Ipmodecm = 0,
                    Poolidcm = 0,
                    Poolidcpe = 0,
                    Staticipcpe = "",
                    Comment = request.Contractor.ContractorPhone,
                    Srvid = tranServicePackage.ServicePackageId.Value,
                    Taxid = request.Contractor.ContractorTaxIdNo ?? string.Empty,
                    Createdon = DateTime.UtcNow
                };

                var createRadiusRsp = await this._radiusManagementService.CreateUser(radiusUser);
                if (!createRadiusRsp.IsSuccess)
                {
                    return createRadiusRsp;
                }
            }

            return ActionResponse.Success;
        }
    }
}
