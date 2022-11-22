using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickForNet.Objects;

namespace TikForNet.FuntionalTests.MikroTikObjects
{
    public class MikroTickTestBase
    {
        protected string[] BrasConnections 
            = new string[] {
                "host=203.128.245.35;username=admin;password=htcno1",
                //"host=103.130.211.43;username=cnmadmin;password=Htc@Cnm20032020!"
            };

        private HashSet<ITikConnection> _connections;

        public MikroTickTestBase()
        {
            _connections = new HashSet<ITikConnection>();
        }

        protected ITikConnection[] Connections
        {
            get { return _connections.ToArray(); }
        }
        protected virtual void OnInitialize()
        {
            // dummy
        }

        protected virtual void OnCleanup()
        {
            // dummy
        }

        public void Init()
        {

        }

        public void Cleanup()
        {
            OnCleanup();
            foreach (var conn in _connections)
            {
                conn?.Dispose();
            }
        }

        protected void RecreateConnection()
        {
            foreach (var connectionStr in BrasConnections)
            {
                var sanboxConnSettings = TikConnectionSetting.ReadFromConnectionString(connectionStr);
                var sanboxConn 
                    = ConnectionFactory.OpenConnection(TikConnectionType.Api, sanboxConnSettings.TikHost, sanboxConnSettings.TikUser, sanboxConnSettings.TikPassword);
                _connections.Add(sanboxConn);
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
    }
}
