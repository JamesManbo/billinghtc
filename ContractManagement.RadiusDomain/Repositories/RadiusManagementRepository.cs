using ContractManagement.RadiusDomain.Models;
using Global.Models.StateChangedResponse;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContractManagement.RadiusDomain.Repositories
{
    public interface IRadiusManagementRepository
    {
        void UpdateRmUser(RmUsers rmUser);
        void RemakeRaidusServerConnection(string radiusSrvrConn);
        bool IsUserExisted(string userName);
        Task<RmUsers> CreateRmUser(RmUsers rmUser);
        Task<RmUsers> GetRmUser(string userName);
        Task<RmServices> GetRmService(int id);
        Task<ActionResponse> RemoveRmUser(string userName);
        Task<ActionResponse> ActivateUserByUserName(string userName);
        Task<ActionResponse> DeactivateUserByUserName(string userName);
        Task<ActionResponse> MultipleActivateUserByUserName(string[] userNames);
        Task<ActionResponse> MultipleDeactivateUserByUserNames(string[] userNames);
        Task<ActionResponse> MultipleUpgradeSrvByUserNames(string[] userNames, int srvId);
        Task<ActionResponse> MultipleRemoveUserByUserNames(string[] userNames);
        Task<IEnumerable<RmServices>> GetAllEnableServices();
        Task<IEnumerable<RmUsers>> GetAllTestUsers();
        Task PureDeleteUsers(IEnumerable<RmUsers> removingUsers);
    }

    public class RadiusManagementRepository : IRadiusManagementRepository
    {
        public RadiusContext RadiusDbContext { get; set; }

        public RadiusManagementRepository(RadiusContext radiusDbContext)
        {
            RadiusDbContext = radiusDbContext;
        }

        public void RemakeRaidusServerConnection(string radiusSrvrConn)
        {
            if (!string.IsNullOrWhiteSpace(radiusSrvrConn))
            {
                var radiusSrvrConnBuilder = new DbConnectionStringBuilder
                {
                    ConnectionString = radiusSrvrConn
                };

                if (radiusSrvrConnBuilder.TryGetValue("server", out var radiusServer))
                {
                    if (!RadiusDbContext.Database.GetDbConnection().DataSource
                        .Equals(radiusSrvrConn, StringComparison.OrdinalIgnoreCase))
                    {
                        var newDbContextOptBuilder = new DbContextOptionsBuilder<RadiusContext>();
                        newDbContextOptBuilder.UseMySql(radiusSrvrConnBuilder.ConnectionString);
                        RadiusDbContext = new RadiusContext(newDbContextOptBuilder.Options);
                    }
                }
            }
        }

        public async Task<RmUsers> CreateRmUser([NotNull] RmUsers rmUser)
        {
            var clearTextPsswrdAttribute = new Radcheck()
            {
                Op = ":=",
                Username = rmUser.Username,
                Attribute = "Cleartext-Password",
                Value = rmUser.Password
            };
            RadiusDbContext.Radcheck.Add(clearTextPsswrdAttribute);

            var simultaneousUseAttribute = new Radcheck()
            {
                Op = ":=",
                Username = rmUser.Username,
                Attribute = "Simultaneous-Use",
                Value = 1.ToString()
            };
            RadiusDbContext.Radcheck.Add(simultaneousUseAttribute);

            rmUser.Password = MD5Hash(rmUser.Password);
            rmUser.Createdon = DateTime.Now;
            rmUser.Createdby = "BillingSystem";

            RadiusDbContext.RmUsers.Add(rmUser);
            await RadiusDbContext.SaveChangesAsync();
            return rmUser;
        }

        private string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5CryptoService = new MD5CryptoServiceProvider();
            byte[] bytes = md5CryptoService.ComputeHash(new UTF8Encoding().GetBytes(input));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }

            return hash.ToString();
        }

        public bool IsUserExisted(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return false;
            return RadiusDbContext.RmUsers.Any(e => userName.Equals(e.Username, StringComparison.OrdinalIgnoreCase));
        }

        private async Task TruncateRadAcct()
        {
            var radActTableNames = new string[] { "radacct", "Radacct", "RadAcct" };
            foreach (var radAcctTableName in radActTableNames)
            {
                try
                {
                    await RadiusDbContext.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE {radAcctTableName}");
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        public async Task<ActionResponse> DeactivateUserByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return ActionResponse.Failed("Tên người dùng không hợp lệ.");
            }

            var targetUser = RadiusDbContext
                .RmUsers
                .FirstOrDefault(u => userName.Equals(u.Username, StringComparison.OrdinalIgnoreCase));

            if (targetUser == null) return ActionResponse.Success;

            targetUser.Enableuser = 0;
            RadiusDbContext.RmUsers.Update(targetUser);
            await TruncateRadAcct();
            await RadiusDbContext.SaveChangesAsync();

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleActivateUserByUserName(string[] userNames)
        {
            var lowercaseUserNames = userNames.Select(e => e.ToLower());
            var targetUsers = RadiusDbContext.RmUsers.Where(u => lowercaseUserNames.Contains(u.Username.ToLower()));

            if (targetUsers == null || targetUsers.Count() == 0)
            {
                return ActionResponse.Failed($"Không tìm thấy tài khoản nào sau đây trên hệ thống Radius: {string.Join(',', userNames)}");
            }

            foreach (var target in targetUsers)
            {
                target.Enableuser = 1;
            }

            RadiusDbContext.RmUsers.UpdateRange(targetUsers);
            await TruncateRadAcct();
            await RadiusDbContext.SaveChangesAsync();

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleUpgradeSrvByUserNames(string[] userNames, int srvId)
        {
            var lowercaseUserNames = userNames.Select(e => e.ToLower());
            var targetUsers = RadiusDbContext.RmUsers.Where(u => lowercaseUserNames.Contains(u.Username.ToLower()));
            var newSrv = RadiusDbContext.RmServices.FirstOrDefault(c => c.Srvid == srvId);

            if (targetUsers == null || targetUsers.Count() == 0) {
                return ActionResponse.Failed($"Không tìm thấy tài khoản nào sau đây trên hệ thống Radius: {string.Join(',', userNames)}");
            }

            foreach (var target in targetUsers)
            {
                target.Srvid = srvId;
                target.Uplimit = newSrv.Uprate;
                target.Downlimit = newSrv.Downrate;
                target.Enableuser = 1;
            }

            RadiusDbContext.RmUsers.UpdateRange(targetUsers);

            await TruncateRadAcct();
            await RadiusDbContext.SaveChangesAsync();

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> RemoveRmUser(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName)) return ActionResponse.Failed("Tên người dùng không hợp lệ.");

            var targetUser = await RadiusDbContext.RmUsers.FirstOrDefaultAsync(c => userName.Equals(c.Username, StringComparison.OrdinalIgnoreCase));
            if (targetUser == null) return ActionResponse.Failed($"Không tìm thấy người dùng {userName} trên hệ thống Radius.");

            RadiusDbContext.RmUsers.Remove(targetUser);
            await RadiusDbContext.SaveChangesAsync();
            return ActionResponse.Success;
        }

        public async Task<IEnumerable<RmServices>> GetAllEnableServices()
        {
            return await RadiusDbContext.RmServices.Where(s => s.Enableservice != 0)
                .ToListAsync();
        }

        public async Task<IEnumerable<RmUsers>> GetAllTestUsers()
        {
            return await RadiusDbContext.RmUsers.Where(s => s.Username.StartsWith("test"))
                .ToListAsync();
        }

        public async Task PureDeleteUsers(IEnumerable<RmUsers> removingUsers)
        {
            RadiusDbContext.RmUsers.RemoveRange(removingUsers);
            await RadiusDbContext.SaveChangesAsync();
        }

        public async Task<ActionResponse> MultipleRemoveUserByUserNames(string[] userNames)
        {
            var lowercaseUserNames = userNames.Select(e => e.ToLower());
            var targetUsers = RadiusDbContext.RmUsers.Where(u => lowercaseUserNames.Contains(u.Username.ToLower()));

            if (targetUsers == null || targetUsers.Count() == 0) return ActionResponse.Failed($"Không tìm thấy tài khoản nào sau đây trên hệ thống Radius: {string.Join(',', userNames)}");

            await this.PureDeleteUsers(targetUsers);
            await TruncateRadAcct();
            await RadiusDbContext.SaveChangesAsync();

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> MultipleDeactivateUserByUserNames(string[] userNames)
        {
            var lowercaseUserNames = userNames.Select(e => e.ToLower());
            var targetUsers = RadiusDbContext.RmUsers.Where(u => u.Enableuser == 1
                && lowercaseUserNames.Contains(u.Username.ToLower()));

            if (targetUsers == null || targetUsers.Count() == 0)
            {
                return ActionResponse.Failed($"Không tìm thấy tài khoản nào sau đây trên hệ thống Radius: {string.Join(',', userNames)}");
            }

            foreach (var target in targetUsers)
            {
                target.Enableuser = 0;
            }

            RadiusDbContext.RmUsers.UpdateRange(targetUsers);

            await TruncateRadAcct();

            await RadiusDbContext.SaveChangesAsync();

            return ActionResponse.Success;
        }

        public async Task<ActionResponse> ActivateUserByUserName(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                return ActionResponse.Failed("Tên người dùng không hợp lệ");
            }

            var targetUser = RadiusDbContext
                .RmUsers
                .FirstOrDefault(u => userName.Equals(u.Username, StringComparison.OrdinalIgnoreCase));

            if (targetUser == null) return ActionResponse.Failed($"Không tìm thấy người dùng {userName} trên hệ thống Radius.");

            targetUser.Enableuser = 1;
            RadiusDbContext.RmUsers.Update(targetUser);
            await RadiusDbContext.SaveChangesAsync();

            return ActionResponse.Success;
        }

        public Task<RmServices> GetRmService(int id)
        {
            return RadiusDbContext.RmServices.FirstOrDefaultAsync(r => r.Srvid == id);
        }

        public async Task<RmUsers> GetRmUser(string userName)
        {
            if (string.IsNullOrEmpty(userName?.Trim())) return default;

            return await RadiusDbContext.RmUsers
                .Where(s => userName.Equals(s.Username, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync();
        }

        public void UpdateRmUser(RmUsers rmUser)
        {
            RadiusDbContext.RmUsers.Update(rmUser);
            RadiusDbContext.SaveChanges();
        }
    }
}
