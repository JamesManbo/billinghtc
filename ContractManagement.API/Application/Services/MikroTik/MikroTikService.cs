using ContractManagement.Infrastructure.Repositories.RadiusAndBrasRepository;
using ContractManagement.RadiusDomain.Models;
using ContractManagement.RadiusDomain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TikForNet;
using TikForNet.Objects.Ppp;

namespace ContractManagement.API.Application.Services.MikroTik
{
    public interface IMikroTikService
    {
        IEnumerable<PppActive> GetAllPppActives();
        IEnumerable<PppActive> GetPppActiveByUsername(string userName);
        void DisablePppActiveByUsernames(string[] userNames);
        void EnablePppActiveByUsernames(string[] userNames);
        void ResetPppActiveByUsernames(string[] userNames);
        void UpgradeServiceByUsernames(string[] userNames);

        Task CreateRadiusUser(RmUsers user, int radiusServerId = 0);
        (bool, string) IsUserExisted(string userName, int radiusServerId = 0);
    }

    public class MikroTikService : IMikroTikService
    {
        private readonly IRadiusManagementRepository _radiusManagementUserRepository;
        private readonly IRadiusServerInfoRepository _radiusServerInfoRepository;
        private readonly IBrasInfoRepository _brasInfoRepository;
        private HashSet<ITikConnection> _connections;

        public MikroTikService(IRadiusManagementRepository radiusManagementUserRepository,
            IRadiusServerInfoRepository radiusServerInfoRepository,
            IBrasInfoRepository brasInfoRepository)
        {
            _radiusManagementUserRepository = radiusManagementUserRepository;
            _radiusServerInfoRepository = radiusServerInfoRepository;
            _brasInfoRepository = brasInfoRepository;
            //RecreateConnection();
        }

        protected ITikConnection[] Connections
        {
            get { return _connections.ToArray(); }
        }
        protected void RecreateConnection()
        {
            var brasInformation = _brasInfoRepository.GetAll();
            if (brasInformation != null && brasInformation.Count() > 0)
            {
                foreach (var brasInfo in brasInformation)
                {
                    _connections.Add(ConnectionFactory
                        .OpenConnection(TikConnectionType.Api,
                            brasInfo.IP,
                            brasInfo.UserName,
                            brasInfo.Password));
                }
            }
        }
        protected IEnumerable<T> Execute<T>(Func<ITikConnection, T> command)
        {
            foreach (var conn in _connections)
            {
                yield return command(conn);
            }
        }

        protected IEnumerable<T> Execute<T>(Func<ITikConnection, IEnumerable<T>> command)
        {
            var result = Enumerable.Empty<T>();

            foreach (var conn in _connections)
            {
                result = result.Concat(command(conn));
            }

            return result;
        }

        protected void Execute(Action<ITikConnection> act)
        {
            foreach (var conn in _connections)
            {
                act(conn);
            }
        }

        public void DisablePppActiveByUsernames(string[] userNames)
        {
            throw new NotImplementedException();
        }

        public void EnablePppActiveByUsernames(string[] userNames)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PppActive> GetAllPppActives()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PppActive> GetPppActiveByUsername(string userName)
        {
            throw new NotImplementedException();
        }

        public void ResetPppActiveByUsernames(string[] userNames)
        {
            throw new NotImplementedException();
        }

        public void UpgradeServiceByUsernames(string[] userNames)
        {
            throw new NotImplementedException();
        }

        public async Task CreateRadiusUser(RmUsers user, int radiusServerId)
        {
            if (radiusServerId > 0)
            {
                var targetRadiusServer = await _radiusServerInfoRepository.GetByIdAsync(radiusServerId);
                _radiusManagementUserRepository.RemakeRaidusServerConnection(targetRadiusServer.GetConnectionString());
                await _radiusManagementUserRepository.CreateRmUser(user);
            }
            else
            {
                var allRadiusServer = await _radiusServerInfoRepository.GetAll();
                foreach (var radiusServer in allRadiusServer)
                {
                    _radiusManagementUserRepository.RemakeRaidusServerConnection(radiusServer.BuildConnectionString());
                    await _radiusManagementUserRepository.CreateRmUser(user);
                }
            }
        }

        public (bool, string) IsUserExisted(string userName, int radiusServerId = 0)
        {
            return (true, "");
        }
    }
}
