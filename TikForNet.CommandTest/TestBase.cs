using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TickForNet.Objects;

namespace TikForNet.CommandTest
{
    public class TestBase
    {

        protected const string SandboxConnection1 = "host=103.238.69.163;username=cnmadmin;password=Htc@Cnm20032020!";

        private HashSet<ITikConnection> _connections;

        public TestBase()
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
            var sanbox1ConnSettings = TikConnectionSetting.ReadFromConnectionString(SandboxConnection1);
            var sanbox1Conn = ConnectionFactory.OpenConnection(TikConnectionType.Api, sanbox1ConnSettings.TikHost, sanbox1ConnSettings.TikUser, sanbox1ConnSettings.TikPassword);
            sanbox1Conn.DebugEnabled = true;

            _connections.Add(sanbox1Conn);
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
