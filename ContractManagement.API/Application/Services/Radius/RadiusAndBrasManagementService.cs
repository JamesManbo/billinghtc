using AutoMapper;
using ContractManagement.Domain.Models.RadiusAndBras;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using GenericRepository.Configurations;
using GenericRepository.Extensions;
using Global.Models.StateChangedResponse;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikForNet;
using TikForNet.Objects;
using TikForNet.Objects.Ppp;

namespace ContractManagement.API.Application.Services.Radius
{
    public interface IRadiusAndBrasManagementService
    {
        Task<IEnumerable<RadiusServiceByServerDTO>> GetRadiusServiceByAllServer();
        Task<ActionResponse> CreateUser(RmUsers user);
        Task<ActionResponse> UpdateSrv(string userName, int newServiceId);
        Task<ActionResponse> DeactivateUserByUserName(string userName);
        Task<ActionResponse> ActivateUserByUserName(string userName);
        Task<ActionResponse> MultipleDeactivateUserByUserName(string[] userNames);
        Task<ActionResponse> MultipleActivateUserByUserName(string[] userNames);
        Task<ActionResponse> MultipleUpgradeSrvByUserName(string[] userNames, int servicePackageId);
        Task<ActionResponse> MultipleTerminateUserByUserName(string[] userNames);
    }
    public class RadiusAndBrasManagementService : IRadiusAndBrasManagementService
    {
        private readonly ILogger<RadiusAndBrasManagementService> _logger;
        private readonly IRadiusServerInfoQueries _radiusServerInfoQueries;
        private readonly IBrasInfoQueries _brasInfoQueries;
        private readonly IServicePackageQueries _servicePackageQueries;
        private readonly IRadiusServicePackageQueries _radiusServiceQueries;
        private readonly IRadiusManagementRepository _radiusManagementRepository;

        private HashSet<ITikConnection> _tikConnections;

        private readonly RadiusServerInfoDTO[] RadiusServers;
        private readonly BrasInfoDTO[] BrasInfos;
        public RadiusAndBrasManagementService(
            IRadiusServerInfoQueries radiusServerInfoQueries,
            IRadiusServicePackageQueries radiusServiceQueries,
            IBrasInfoQueries brasInfoQueries,
            IRadiusManagementRepository raidusManagementRepository,
            ILogger<RadiusAndBrasManagementService> logger,
            IServicePackageQueries servicePackageQueries)
        {
            _radiusServerInfoQueries = radiusServerInfoQueries;
            _brasInfoQueries = brasInfoQueries;
            _radiusServiceQueries = radiusServiceQueries;
            _radiusManagementRepository = raidusManagementRepository;

            RadiusServers = _radiusServerInfoQueries.GetAll().ToArray();
            BrasInfos = _brasInfoQueries.GetAll().ToArray();
            this._logger = logger;
            this._servicePackageQueries = servicePackageQueries;
            RecreateTikConnections();
        }

        public async Task<IEnumerable<RadiusServiceByServerDTO>> GetRadiusServiceByAllServer()
        {
            return await RadiusExecuteAsync(async radiusServer =>
            {
                var resultLine = new RadiusServiceByServerDTO()
                {
                    RadiusServerId = radiusServer.Id,
                    RadiusServerIpAddress = radiusServer.IP,
                    RadiusServerName = radiusServer.ServerName
                };

                var radiusServicesOfServer = await _radiusManagementRepository.GetAllEnableServices();
                resultLine.RadiusServices = radiusServicesOfServer.Select(e => new RadiusServiceDTO()
                {
                    Descr = e.Descr,
                    Downrate = e.Downrate,
                    Srvid = e.Srvid,
                    Srvname = e.Srvname,
                    Uprate = e.Uprate
                });

                return resultLine;
            });
        }

        public async Task<ActionResponse> DeactivateUserByUserName(string userName)
        {
            var collectionResult = await RadiusExecuteAsync(async radiusServer =>
            {
                return await _radiusManagementRepository.DeactivateUserByUserName(userName);
            });

            if(collectionResult.All(r => !r.IsSuccess))
            {
                return collectionResult.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleUpgradeSrvByUserName(string[] userNames, int servicePakcageId)
        {
            try
            {
                // Upgrade Radius service and re-active users
                var radiusExecutions = await RadiusExecuteAsync(async radiusServer =>
                {
                    var radiusService = _servicePackageQueries.FindRadiusService(servicePakcageId, radiusServer.Id);
                    if (radiusService == null)
                    {
                        return ActionResponse.Failed($"Không tìm thấy gói cước trên hệ thống. Vui lòng kiểm tra lại thông tin đã nhập.");
                    }

                    if (radiusService.RadiusServiceId == 0)
                    {
                        return ActionResponse.Failed($"Gói cước {radiusService.BillingPackageName} chưa được gán với bất kỳ gói cước nào trên hệ thống Radius.\n" +
                            $"Vui lòng thực hiện việc chỉ định gói cước tương đương trước khi thực hiện nâng cấp.");
                    }

                    var radSaveChangeRes = await _radiusManagementRepository
                        .MultipleUpgradeSrvByUserNames(userNames, radiusService.RadiusServiceId);

                    return radSaveChangeRes;
                });

                if (radiusExecutions.All(r => !r.IsSuccess))
                {
                    return radiusExecutions.FirstOrDefault() ?? ActionResponse.Success;
                }

                return ActionResponse.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError("Upgrade RADIUS & BRAS channel {0}", ex);
                return ActionResponse.Failed("Không thể thực hiện điều chỉnh gói cược tại hệ thống Radius. Vui lòng liên hệ quản trị viên.");
            }
        }

        public async Task<ActionResponse> ActivateUserByUserName(string userName)
        {
            var collectionResult = await RadiusExecuteAsync(async radiusServer =>
            {
                return await _radiusManagementRepository.ActivateUserByUserName(userName);
            });

            if(collectionResult.All(r => !r.IsSuccess))
            {
                return collectionResult.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleActivateUserByUserName(string[] userNames)
        {
            var collectionResult = await RadiusExecuteAsync(async radiusServer =>
            {
                return await _radiusManagementRepository.MultipleActivateUserByUserName(userNames);
            });

            if(collectionResult.All(r => !r.IsSuccess))
            {
                return collectionResult.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleDeactivateUserByUserName(string[] userNames)
        {
            var collectionResult = await RadiusExecuteAsync(async radiusServer =>
            {
                return await _radiusManagementRepository.MultipleDeactivateUserByUserNames(userNames);
            });

            try
            {
                if (collectionResult.Any(c => c.IsSuccess))
                {
                    // Delete BRAS live session of users
                    BrasDeletePppActives(userNames);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Can not delete BRAS live sessions: {string.Join(',', userNames)}. Exception: {exception}");
            }

            if(collectionResult.All(x => !x.IsSuccess))
            {
                return collectionResult.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleTerminateUserByUserName(string[] userNames)
        {
            var collectionResult = await RadiusExecuteAsync(async radiusServer =>
            {
                return await _radiusManagementRepository.MultipleRemoveUserByUserNames(userNames);
            });

            try
            {
                if (collectionResult.Any(c => c.IsSuccess))
                {
                    // Delete BRAS live session of users
                    BrasDeletePppActives(userNames);
                }
            }
            catch (Exception exception)
            {
                _logger.LogError($"Can not delete BRAS live sessions: {string.Join(',', userNames)}. Exception: {exception}");
            }

            if(collectionResult.Any(_ => !_.IsSuccess))
            {
                return collectionResult.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }

        private void BrasDeletePppActives(string[] userNames)
        {
            string[] targetUserNames = userNames.Select(c => c.ToLower().Trim()).ToArray();
            BrasExecute(conn =>
            {
                var allActives = conn.LoadAll<PppActive>();
                var targets = allActives.Where(a => targetUserNames.Contains(a.Name.ToLower().Trim()));
                foreach (var target in targets)
                {
                    try
                    {
                        conn.Delete(target);
                    }
                    catch (TikNoSuchItemException)
                    {
                        continue;
                    }
                }
            });
        }

        protected async Task<IEnumerable<TResult>> RadiusExecuteAsync<TResult>(Func<RadiusServerInfoDTO, Task<TResult>> caller)
        {
            if (RadiusServers == null || RadiusServers.Length == 0)
            {
                return Enumerable.Empty<TResult>();
            }
            var resultCollection = new List<TResult>();

            foreach (var radServer in RadiusServers)
            {
                _radiusManagementRepository.RemakeRaidusServerConnection(radServer.BuildConnectionString());
                resultCollection.Add(await caller(radServer));
            }
            return resultCollection;
        }

        protected IEnumerable<TResult> RadiusExecute<TResult>(Func<RadiusServerInfoDTO, TResult> caller)
        {
            if (RadiusServers == null || RadiusServers.Length == 0)
            {
                yield return default(TResult);
            }

            foreach (var radServer in RadiusServers)
            {
                _radiusManagementRepository.RemakeRaidusServerConnection(radServer.BuildConnectionString());
                yield return caller(radServer);
            }
        }

        protected IEnumerable<TResult> RadiusExecuteAsync<TResult>(Func<RadiusServerInfoDTO, IEnumerable<TResult>> caller)
        {
            if (RadiusServers == null || RadiusServers.Length == 0)
            {
                return Enumerable.Empty<TResult>();
            }

            var resultCollection = Enumerable.Empty<TResult>();
            foreach (var radServer in RadiusServers)
            {
                _radiusManagementRepository.RemakeRaidusServerConnection(radServer.BuildConnectionString());
                resultCollection = resultCollection.Concat(caller(radServer));
            }

            return resultCollection;
        }

        protected async Task<IEnumerable<TResult>> RadiusExecuteAsync<TResult>(Func<RadiusServerInfoDTO, Task<IEnumerable<TResult>>> caller)
        {
            if (RadiusServers == null || RadiusServers.Length == 0)
            {
                return Enumerable.Empty<TResult>();
            }

            var resultCollection = Enumerable.Empty<TResult>();
            foreach (var radServer in RadiusServers)
            {
                _radiusManagementRepository.RemakeRaidusServerConnection(radServer.BuildConnectionString());
                resultCollection = resultCollection.Concat(await caller(radServer));
            }

            return resultCollection;
        }
        public void TikCleanup()
        {
            foreach (var conn in _tikConnections)
            {
                conn?.Dispose();
            }
        }

        protected void RecreateTikConnections()
        {
            _tikConnections = new HashSet<ITikConnection>();
            if (BrasInfos != null && BrasInfos.Length > 0)
            {
                foreach (var brasInfo in BrasInfos)
                {
                    var tikConnection
                        = ConnectionFactory.OpenConnection(TikConnectionType.Api, brasInfo.IP, brasInfo.ManualAPIPort, brasInfo.UserName, brasInfo.Password);
                    _tikConnections.Add(tikConnection);
                }
            }
        }

        protected IEnumerable<T> BrasExecute<T>(Func<ITikConnection, T> command)
        {
            foreach (var conn in _tikConnections)
            {
                yield return command(conn);
            }
        }
        protected IEnumerable<T> BrasExecute<T>(Func<ITikConnection, IEnumerable<T>> command)
        {
            var result = Enumerable.Empty<T>();
            foreach (var conn in _tikConnections)
            {
                result = result.Concat(command(conn));
            }

            return result;
        }

        protected void BrasExecute(Action<ITikConnection> act)
        {
            foreach (var conn in _tikConnections)
            {
                act(conn);
            }
        }

        public async Task<ActionResponse> CreateUser(RmUsers user)
        {
            var results = await RadiusExecuteAsync(async radiusServer =>
            {
                if (!_radiusManagementRepository
                       .IsUserExisted(user.Username))
                {
                    var mapping = _servicePackageQueries.FindRadiusService(user.Srvid, radiusServer.Id);

                    if (mapping == null)
                    {
                        return ActionResponse.Failed($"Gói cước không tồn tại trên hệ thống.");
                    }

                    if (mapping.RadiusServiceId == 0)
                    {
                        return ActionResponse.Failed($"Gói cước {mapping.BillingPackageName} chưa được gán với bất kỳ gói cước nào trên hệ thống Radius.\n" +
                            $"Vui lòng thực hiện việc chỉ định gói cước tương đương trước khi thực hiện thêm mới kênh.");
                    }

                    var radiusService = await _radiusManagementRepository.GetRmService(mapping.RadiusServiceId);

                    if (radiusService == null)
                    {
                        return ActionResponse.Failed($"Không tìm thấy gói cước {mapping.BillingPackageName} trên hệ thống Radius. Liên hệ quản trị viên để khắc phục sự cố.");
                    }

                    user.Srvid = radiusService.Srvid;
                    user.Uplimit = radiusService.Uprate;
                    user.Downlimit = radiusService.Downrate;
                    await this._radiusManagementRepository.CreateRmUser(user);
                    return ActionResponse.Success;
                }

                return ActionResponse.Failed($"Tài khoản Radius \"{user.Username}\" đã tồn tại, vui lòng nhập tên tài khoản mới.");
            });

            if (results.All(_ => !_.IsSuccess))
            {
                return results.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> UpdateSrv(string userName, int newServiceId)
        {
            var results = await RadiusExecuteAsync(async radiusServer =>
            {

                var radiusUser = await _radiusManagementRepository.GetRmUser(userName);
                if (radiusUser == null)
                {
                    return ActionResponse.Failed($"Không tìm thấy tài khoản {userName} trên hệ thống Radius. Liên hệ quản trị viên để khắc phục sự cố.");
                }
                var mapping = _servicePackageQueries.FindRadiusService(newServiceId, radiusServer.Id);

                if (mapping == null)
                {
                    return ActionResponse.Failed($"Gói cước không tồn tại trên hệ thống.");
                }

                if (mapping.RadiusServiceId == 0)
                {
                    return ActionResponse.Failed($"Gói cước {mapping.BillingPackageName} chưa được gán với bất kỳ gói cước nào trên hệ thống Radius.\n" +
                            $"Vui lòng thực hiện việc chỉ định gói cước tương đương trước khi thực hiện nâng cấp.");
                }

                var radiusService = await _radiusManagementRepository.GetRmService(mapping.RadiusServiceId);

                if (radiusService == null)
                {
                    return ActionResponse.Failed($"Không tìm thấy gói cước {mapping.BillingPackageName} trên hệ thống Radius. Liên hệ quản trị viên để khắc phục sự cố.");
                }

                radiusUser.Srvid = radiusService.Srvid;
                radiusUser.Uplimit = radiusService.Uprate;
                radiusUser.Downlimit = radiusService.Downrate;
                radiusUser.Enableuser = 1;
                this._radiusManagementRepository.UpdateRmUser(radiusUser);
                return ActionResponse.Success;
            });

            if (results.All(_ => !_.IsSuccess))
            {
                return results.FirstOrDefault() ?? ActionResponse.Success;
            }

            return ActionResponse.Success;
        }
    }
}
