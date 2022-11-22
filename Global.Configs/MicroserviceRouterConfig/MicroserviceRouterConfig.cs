using System;
using System.Collections.Generic;
using System.Text;

namespace Global.Configs.MicroserviceRouterConfig
{
    public static class MicroserviceRouterConfig
    {
        public static string Contract = "http://contractmanagementapi.api:80";
        public static string GrpcContract = "http://contractmanagementapi.api:81";
        public static string GrpcApplicationUser = "http://applicationuseridentity.api:81";
        public static string GrpcSystemUserIdentity = "http://systemuseridentity.api:81";
        public static string GrpcLocation = "http://location.api:81";
        public static string GrpcStaticResource = "http://staticresource.api:81";
        public static string GrpcNotification = "http://notification.api:81";
        public static string GrpcDebt = "http://debtmanagement.api:81";
        public static string GrpcFeedback = "http://feedback.api:81";
        public static string GrpcNews = "http://news.api:81";
        public static string GrpcOrganizationUnit = "http://organizationunit.api:81";
    }
}
