using ContractManagement.Domain.AggregatesModel.BaseContract;
using ContractManagement.Domain.AggregatesModel.TransactionAggregate;
using ContractManagement.Domain.Models;
using ContractManagement.Domain.Models.Notification;
using Global.Models.Notification;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ContractManagement.Domain.AggregatesModel.TransactionAggregate.TransactionType;

namespace ContractManagement.API.Application.Commands.TransactionCommandHandler.TransactionNotification
{
    public enum TransNotifyMethod
    {
        /// <summary>
        /// Send notify via email from <billing@htc-itc.com.vn>
        /// </summary>
        Email,
        /// <summary>
        /// Send notify by notification to user applications
        /// </summary>
        AppNotification,
        /// <summary>
        /// Send notify by sms to user phone number
        /// </summary>
        SMS
    }

    public class TransactionNotifyBuilder
    {
        public TransactionNotifyBuilder(TransactionDTO transaction, bool isUpdateNotify = false)
        {
            Transaction = transaction;
            IsUpdateNotify = isUpdateNotify;
        }
        private readonly string WebsiteDomain = "https://billing.htc-itc.vn/";

        public bool IsUpdateNotify { get; set; }
        public string TransactionLabel => this.Transaction.IsAppendix == true ? "Phụ lục" : "Giao dịch";
        public TransactionDTO Transaction { get; set; }
        public TransNotifyMethod NotifyMethod { get; set; }

        public string DistinctServices => string.Join(", ", Transaction.TransactionServicePackages.Select(c => c.ServiceName).Distinct());
        public string DistinctChannels => string.Join(", ", Transaction.TransactionServicePackages.Select(c => c.CId).Distinct());
        public T Build<T>() where T : new()
        {
            var notifyInstance = new T();
            if (notifyInstance is PushNotificationRequest appNotification)
            {
                this.NotifyMethod = TransNotifyMethod.AppNotification;
                appNotification.Type = NotificationType.App;
                appNotification.Zone = NotificationZone.Contract;

                if (Transaction.StatusId == TransactionStatus.Cancelled.Id)
                {
                    appNotification.Title = $"Từ chối duyệt nghiệm thu";
                    appNotification.Content = $"{TransactionLabel} {Transaction.TypeName} số {Transaction.Code} đã bị từ chối nghiệm thu.\n" +
                            $"Lý do: {Transaction.ReasonCancelAcceptance}";
                }
                else
                {
                    appNotification.Title = this.NotifyTitle;
                    appNotification.Content = this.NotifyContent;
                }

                appNotification.Category = NotificationCategory.ContractTransaction;
                appNotification.Payload = JsonConvert.SerializeObject(new
                {
                    Type = Transaction.Type,
                    TypeName = Transaction.TypeName,
                    Id = Transaction.Id,
                    Code = Transaction.Code,
                    Category = NotificationCategory.ContractTransaction
                }, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            }
            else if (notifyInstance is SendMailRequest email)
            {
                this.NotifyMethod = TransNotifyMethod.Email;
                if (Transaction.StatusId == TransactionStatus.Cancelled.Id)
                {
                    email.Subject = $"Giao dịch {Transaction.Code} đã bị từ chối duyệt nghiệm thu";
                    var builder = new TransactionEmailContentBuilder();
                    builder.AddHtml($"Giao dịch <a href=\"{WebsiteDomain}/transaction-management?c={Transaction.Code}\">{Transaction.Code}</a> đã bị từ chối nghiệm thu.");
                    builder.Break();
                    builder.AddHtml($"Lý do: {Transaction.ReasonCancelAcceptance}");
                    builder.Break();

                    builder.Break();
                    builder.AddHtml("<p>Email được gửi tự động từ <strong>Phần mềm quản lý hợp đồng và tính cước.</strong></p>");
                    builder.AddHtml("<p>Vui lòng không trả lời email này.</p>");

                    email.Body = builder.Build();
                }
                else
                {
                    email.Subject = this.NotifyTitle;
                    email.Body = this.NotifyContent;
                }
            }

            return notifyInstance;
        }

        public string NotifyTitle
        {
            get
            {
                var today = DateTime.Now.AddHours(7).ToString("dd/MM/yyyy");
                if (this.IsUpdateNotify)
                {
                    if (this.NotifyMethod == TransNotifyMethod.AppNotification)
                    {
                        return $"{TransactionLabel} {Transaction.Code} vừa cập nhật thông tin mới.";
                    }
                    else if (this.NotifyMethod == TransNotifyMethod.Email)
                    {
                        switch (this.Transaction.Type)
                        {
                            case (int)TransactionTypeEnums.DeployNewOutContract:
                            case (int)TransactionTypeEnums.AddNewServicePackage:
                                return $"{TransactionLabel} triển khai kênh mới {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.ChangeEquipment:
                                return $"{TransactionLabel} thay đổi thiết bị {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.ChangeLocation:
                                return $"{TransactionLabel} dịch chuyển địa điểm {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.ChangeServicePackage:
                                return $"{TransactionLabel} điều chỉnh dịch vụcho hợp đồng CodeTransactionvừa cập nhật thông tin mới.{today}";
                            case (int)TransactionTypeEnums.ReclaimEquipment:
                                return $"{TransactionLabel} thu hồi thiết bị {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.RestoreServicePackage:
                                return $"{TransactionLabel} khôi phục dịch vụ {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.SuspendServicePackage:
                                return $"{TransactionLabel} tạm ngưng dịch vụ {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.TerminateContract:
                                return $"{TransactionLabel} thanh lý hợp đồng {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.TerminateServicePackage:
                                return $"{TransactionLabel} hủy dịch vụ {Transaction.Code} vừa cập nhật thông tin mới.";
                            case (int)TransactionTypeEnums.UpgradeBandwidth:
                                return $"{TransactionLabel} nâng cấp băng thông {Transaction.Code} vừa cập nhật thông tin mới.";
                            default:
                                return string.Empty;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                else
                {
                    if (this.NotifyMethod == TransNotifyMethod.AppNotification)
                    {
                        switch (this.Transaction.Type)
                        {
                            case (int)TransactionTypeEnums.DeployNewOutContract:
                            case (int)TransactionTypeEnums.AddNewServicePackage:
                                return $"Triển khai kênh mới";
                            case (int)TransactionTypeEnums.ChangeEquipment:
                                return "Thay đổi thiết bị";
                            case (int)TransactionTypeEnums.ChangeLocation:
                                return "Dịch chuyển địa điểm";
                            case (int)TransactionTypeEnums.ChangeServicePackage:
                                return "Điều chỉnh dịch vụ/gói cước";
                            case (int)TransactionTypeEnums.ReclaimEquipment:
                                return "Thu hồi thiết bị";
                            case (int)TransactionTypeEnums.RestoreServicePackage:
                                return "Khôi phục dịch vụ";
                            case (int)TransactionTypeEnums.SuspendServicePackage:
                                return "Tạm ngưng dịch vụ";
                            case (int)TransactionTypeEnums.TerminateContract:
                                return "Thanh lý hợp đồng";
                            case (int)TransactionTypeEnums.TerminateServicePackage:
                                return "Hủy dịch vụ";
                            case (int)TransactionTypeEnums.UpgradeBandwidth:
                                return "Nâng cấp băng thông";
                            default:
                                return string.Empty;
                        }
                    }
                    else if (this.NotifyMethod == TransNotifyMethod.Email)
                    {
                        switch (this.Transaction.Type)
                        {
                            case (int)TransactionTypeEnums.DeployNewOutContract:
                            case (int)TransactionTypeEnums.AddNewServicePackage:
                                return $"Triển khai kênh mới cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.ChangeEquipment:
                                return $"Thay đổi thiết bị cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.ChangeLocation:
                                return $"Dịch chuyển địa điểm cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.ChangeServicePackage:
                                return $"Điều chỉnh dịch vụ/gói cước cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.ReclaimEquipment:
                                return $"Thu hồi thiết bị cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.RestoreServicePackage:
                                return $"Khôi phục dịch vụ cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.SuspendServicePackage:
                                return $"Tạm ngưng dịch vụ cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.TerminateContract:
                                return $"Thanh lý hợp đồng cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.TerminateServicePackage:
                                return $"Hủy dịch vụ cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            case (int)TransactionTypeEnums.UpgradeBandwidth:
                                return $"Nâng cấp băng thông cho hợp đồng {Transaction.ContractCode} ngày {today}";
                            default:
                                return string.Empty;
                        }
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
            }
        }

        public string NotifyContent
        {
            get
            {
                if (this.NotifyMethod == TransNotifyMethod.AppNotification)
                {
                    if (IsUpdateNotify)
                    {
                        var notifyHeader = $"Vào ngày {Transaction.UpdatedDate:dd/MM/yyyy},\n" +
                            $"{TransactionLabel} {Transaction.Code}, hợp đồng {Transaction.ContractCode}.\n" +
                            $"Khách hàng: {Transaction.ContractorFullName}, {Transaction.Contractor.ContractorPhone}.";
                        string notifyBody = string.Empty;
                        switch (this.Transaction.Type)
                        {
                            case (int)TransactionTypeEnums.DeployNewOutContract:
                            case (int)TransactionTypeEnums.AddNewServicePackage:
                                notifyBody = $"Yêu cầu triển khai kênh mới cho dịch vụ {DistinctServices} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.ChangeEquipment:
                                notifyBody = $"Yêu cầu thay đổi thiết bị tại kênh {DistinctChannels} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.ChangeLocation:
                                notifyBody = $"Yêu cầu dịch chuyển địa điểm cho kênh {DistinctChannels} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.ChangeServicePackage:
                                notifyBody = $"Yêu cầu điều chỉnh dịch vụ/gói cước {DistinctChannels} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.ReclaimEquipment:
                                notifyBody = $"Yêu cầu thu hồi thiết bị tại {DistinctChannels} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.RestoreServicePackage:
                                notifyBody = $"Yêu cầu khôi phục dịch vụ kênh {DistinctChannels} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.SuspendServicePackage:
                                notifyBody = $"Yêu cầu tạm ngưng dịch vụ kênh {DistinctChannels} đã được cập nhật";
                                break;
                            case (int)TransactionTypeEnums.TerminateContract:
                                notifyBody = $"Yêu cầu thanh lý hợp đồng đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.TerminateServicePackage:
                                notifyBody = $"Yêu cầu hủy dịch vụ kênh {DistinctChannels} đã được cập nhật.";
                                break;
                            case (int)TransactionTypeEnums.UpgradeBandwidth:
                                notifyBody = $"Yêu cầu nâng cấp băng thông kênh {DistinctChannels} đã được cập nhật.";
                                break;
                            default:
                                notifyBody = string.Empty;
                                break;
                        }
                        return $"{notifyHeader}\n{notifyBody}";
                    }
                    else
                    {
                        var notifyHeader = $"Vào ngày {Transaction.TransactionDate:dd/MM/yyyy},\n" +
                            $"Hợp đồng số {Transaction.ContractCode}.\n" +
                            $"Khách hàng: {Transaction.ContractorFullName}, {Transaction.Contractor.ContractorPhone}.";
                        string notifyBody = string.Empty;
                        switch (this.Transaction.Type)
                        {
                            case (int)TransactionTypeEnums.DeployNewOutContract:
                            case (int)TransactionTypeEnums.AddNewServicePackage:
                                notifyBody = $"Yêu cầu triển khai kênh mới cho dịch vụ {DistinctServices}.";
                                break;
                            case (int)TransactionTypeEnums.ChangeEquipment:
                                notifyBody = $"Yêu cầu thay đổi thiết bị tại kênh {DistinctChannels}.";
                                break;
                            case (int)TransactionTypeEnums.ChangeLocation:
                                notifyBody = $"Yêu cầu dịch chuyển địa điểm cho kênh {DistinctChannels}.";
                                break;
                            case (int)TransactionTypeEnums.ChangeServicePackage:
                                notifyBody = $"Yêu cầu điều chỉnh dịch vụ/gói cước {DistinctChannels}.";
                                break;
                            case (int)TransactionTypeEnums.ReclaimEquipment:
                                notifyBody = $"Yêu cầu thu hồi thiết bị tại {DistinctChannels}.";
                                break;
                            case (int)TransactionTypeEnums.RestoreServicePackage:
                                notifyBody = $"Yêu cầu khôi phục dịch vụ kênh {DistinctChannels}.";
                                break;
                            case (int)TransactionTypeEnums.SuspendServicePackage:
                                notifyBody = $"Yêu cầu tạm ngưng dịch vụ kênh {DistinctChannels}";
                                break;
                            case (int)TransactionTypeEnums.TerminateContract:
                                notifyBody = $"Yêu cầu thanh lý hợp đồng.";
                                break;
                            case (int)TransactionTypeEnums.TerminateServicePackage:
                                notifyBody = $"Yêu cầu hủy dịch vụ kênh {DistinctChannels}.";
                                break;
                            case (int)TransactionTypeEnums.UpgradeBandwidth:
                                notifyBody = $"Yêu cầu nâng cấp băng thông kênh {DistinctChannels}.";
                                break;
                            default:
                                notifyBody = string.Empty;
                                break;
                        }
                        return $"{notifyHeader}\n{notifyBody}";
                    }
                }
                else if (this.NotifyMethod == TransNotifyMethod.Email)
                {
                    string emailBody = string.Empty;
                    switch (this.Transaction.Type)
                    {
                        case (int)TransactionTypeEnums.DeployNewOutContract:
                        case (int)TransactionTypeEnums.AddNewServicePackage:
                            emailBody = BuildNewChannelTransEmail();
                            break;
                        case (int)TransactionTypeEnums.ChangeEquipment:
                            emailBody = BuildChangeEquipmentTransEmail();
                            break;
                        case (int)TransactionTypeEnums.ChangeLocation:
                            emailBody = BuildChangeInstallationAddressTransEmail();
                            break;
                        case (int)TransactionTypeEnums.ReclaimEquipment:
                            emailBody = BuildReclaimEquipmentTransEmail();
                            break;
                        case (int)TransactionTypeEnums.SuspendServicePackage:
                            emailBody = BuildSuspendChannelTransEmail();
                            break;
                        case (int)TransactionTypeEnums.RestoreServicePackage:
                            emailBody = BuildRestoreChannelTransEmail();
                            break;
                        case (int)TransactionTypeEnums.TerminateContract:
                            emailBody = BuildTerminateContractTransEmail();
                            break;
                        case (int)TransactionTypeEnums.TerminateServicePackage:
                            emailBody = BuildTerminateChannelsTransEmail();
                            break;
                        case (int)TransactionTypeEnums.UpgradeBandwidth:
                            emailBody = $"Nâng cấp băng thông cho kênh {DistinctChannels}.";
                            break;
                        case (int)TransactionTypeEnums.ChangeServicePackage:
                            emailBody
                                = BuildChangeServicePackageTransEmail();
                            break;
                        default:
                            emailBody = string.Empty;
                            break;
                    }

                    return emailBody;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        private string BuildNewChannelTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.AddHtml($"Có {Transaction.TransactionServicePackages.Count:D2} cần được triển khai mới:");
            builder.Break();
            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Mã CID", 150);
            if (Transaction.TransactionServicePackages.Any(c => !string.IsNullOrEmpty(c.RadiusAccount)))
            {
                builder.AddTh("Tài khoản Radius");
            }
            builder.AddTh("Loại dịch vụ");
            builder.AddTh("Gói cước/ băng thông");
            builder.AddTh("Địa chỉ lắp đặt", 200);
            builder.AddTh("Phí hòa mạng/ lắp đặt");
            builder.AddTh("Thiết bị", 300);
            builder.AddTh("Ghi chú");
            builder.CloseThead();
            builder.OpenTableBody();
            for (int i = 0; i < Transaction.TransactionServicePackages.Count; i++)
            {
                var channel = Transaction.TransactionServicePackages.ElementAt(i);
                builder.OpenTableRow();
                builder.AddTd((i + 1).ToString("D2"), customStyle: "vertical-align: middle;font-weight: bold;text-align: center;");
                builder.AddTd(channel.CId, customStyle: "vertical-align: middle;text-align: center;");
                if (!string.IsNullOrEmpty(channel.RadiusAccount))
                {
                    builder.AddTd($"Tài khoản: {channel.RadiusAccount}.<br/>Mật khẩu: {channel.RadiusPassword}");
                }
                builder.AddTd(channel.ServiceName);
                var packageAndBandwidthStr = new StringBuilder();
                if (channel.ServicePackageId > 0)
                {
                    packageAndBandwidthStr.AppendLine("<p>" + channel.PackageName + "</p>");
                }

                packageAndBandwidthStr.AppendLine($"{(channel.HasDistinguishBandwidth ? "Trong nước: " : string.Empty)}{channel.DomesticBandwidth} {channel.DomesticBandwidthUom}");

                if (channel.HasDistinguishBandwidth)
                {
                    packageAndBandwidthStr.AppendLine($"/ Quốc tế: {channel.InternationalBandwidth} {channel.InternationalBandwidthUom}");
                }
                builder.AddTd(packageAndBandwidthStr.ToString());

                builder.OpenTd();
                if (channel.HasStartAndEndPoint)
                {
                    builder.AddHtml($"Điểm đầu: {channel.StartPoint.InstallationAddress.FullAddress}");
                    builder.Break();
                    builder.Break();
                    builder.AddHtml($"Điểm cuối: {channel.EndPoint.InstallationAddress.FullAddress}");
                }
                else
                {
                    builder.AddHtml(channel.EndPoint.InstallationAddress.FullAddress);
                }
                builder.CloseTd();
                string formatedFee = "0";
                if (channel.InstallationFee > 0)
                {
                    CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                    formatedFee = channel.InstallationFee.ToString("#,###", cul.NumberFormat);
                }
                builder.AddTd($"{formatedFee}đ", customStyle: "vertical-align: middle;text-align: center;");

                builder.OpenTd();
                if (channel.HasStartAndEndPoint)
                {
                    builder.AddHtml("Thiết bị điểm đầu: ");
                    if (channel.StartPoint.Equipments != null && channel.StartPoint.Equipments.Count > 0)
                    {
                        builder.OpenUl();
                        foreach (var equipment in channel.StartPoint.Equipments)
                        {
                            builder.AddHtml($"<li>{equipment.EquipmentName} ({equipment.ExaminedUnit} {equipment.EquipmentUom})</li>");

                        }
                        builder.CloseUl();
                    }
                    else
                    {
                        builder.AddHtml("<i>Không có</i>");
                        builder.Break();
                    }

                    builder.AddHtml("Thiết bị điểm cuối: ");
                    if (channel.EndPoint.Equipments != null && channel.EndPoint.Equipments.Count > 0)
                    {
                        builder.OpenUl();
                        foreach (var equipment in channel.EndPoint.Equipments)
                        {
                            builder.AddHtml($"<li>{equipment.EquipmentName} ({equipment.ExaminedUnit} {equipment.EquipmentUom})</li>");

                        }
                        builder.CloseUl();
                    }
                    else
                    {
                        builder.AddHtml("<i>Không có</i>");
                        builder.Break();
                    }
                }
                else
                {
                    builder.OpenUl();
                    foreach (var equipment in channel.EndPoint.Equipments)
                    {
                        builder.AddHtml($"<li>{equipment.EquipmentName} ({equipment.ExaminedUnit} {equipment.EquipmentUom})</li>");

                    }
                    builder.CloseUl();
                }
                builder.CloseTd();
                builder.AddTd(channel.Note, customStyle: "");
                builder.CloseTableRow();
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.AddMailFooter(Transaction.Code);

            return builder.Build();
        }
        private string BuildChangeInstallationAddressTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.Break();
            builder.AddHtml($"Yêu cầu dịch chuyển địa điểm kênh {Transaction.TransactionServicePackages.First().CId}.");
            builder.Break();

            builder.AddTextStrong("Địa điểm lắp đặt ban đầu:");
            builder.Break();
            var originChannel = Transaction.TransactionServicePackages.First(c => c.IsOld == true);
            if (originChannel.HasStartAndEndPoint)
            {
                builder.AddTextWithLabel("Điểm đầu", originChannel.StartPoint.InstallationAddress.FullAddress);
                builder.Break();
                builder.AddTextWithLabel("Điểm cuối", originChannel.EndPoint.InstallationAddress.FullAddress);
            }
            else
            {
                builder.AddHtml(originChannel.EndPoint.InstallationAddress.FullAddress);
            }

            builder.Break();
            builder.Break();
            builder.AddTextStrong("Địa điểm lắp đặt mới:");
            builder.Break();

            var newChannel = Transaction.TransactionServicePackages.First(c => c.IsOld != true);
            if (newChannel.HasStartAndEndPoint)
            {
                builder.AddTextWithLabel("Điểm đầu", newChannel.StartPoint.InstallationAddress.FullAddress);
                builder.Break();
                builder.AddTextWithLabel("Điểm cuối", newChannel.EndPoint.InstallationAddress.FullAddress);
            }
            else
            {
                builder.AddHtml(newChannel.EndPoint.InstallationAddress.FullAddress);
            }
            builder.Break();
            builder.Break();
            builder.AddMailFooter(Transaction.Code);

            return builder.Build();
        }
        private string BuildChangeEquipmentTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.Break();
            builder.AddHtml("Danh sách thiết bị cần thu hồi: ");
            builder.Break();

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị");
            builder.AddTh("Mã thiết bị", 120);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt", 250);
            builder.AddTh("Số lượng thu hồi", 150, "text-align: center;");
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        if (equipment.IsOld != true) continue;
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    if (equipment.IsOld != true) continue;
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.AddHtml("Danh sách thiết bị triển khai mới: ");
            builder.Break();

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị");
            builder.AddTh("Mã thiết bị", 120);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt", 250);
            builder.AddTh("Số lượng dự kiến", 150);
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        if (equipment.IsOld == true) continue;
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.ExaminedUnit} {equipment.EquipmentUom}", customStyle: "text-align:center;");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    if (equipment.IsOld == true) continue;
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.ExaminedUnit} {equipment.EquipmentUom}", customStyle: "text-align:center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
        private string BuildReclaimEquipmentTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.Break();
            builder.AddHtml($"<p>Yêu cầu thu hồi thiết bị tại kênh: {DistinctChannels}.</p>");
            builder.AddHtml("Danh sách thiết bị cần thu hồi: ");
            builder.Break();

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị");
            builder.AddTh("Mã thiết bị", 120);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt", 250);
            builder.AddTh("Số lượng thu hồi", 150, "text-align: center;");
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
        private string BuildSuspendChannelTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.AddHtml($"<p>Yêu cầu tạm ngưng hoạt động kênh: {DistinctChannels}.</p>");

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Mã CID", 150);
            builder.AddTh("Dịch vụ", 250);
            builder.AddTh("Gói cước/băng thông", 200);
            builder.AddTh("Địa chỉ lắp đặt");

            builder.OpenTableBody();
            for (int i = 0; i < Transaction.TransactionServicePackages.Count; i++)
            {
                var channel = Transaction.TransactionServicePackages.ElementAt(i);
                builder.OpenTableRow();
                builder.AddTd((i + 1).ToString("D2"), customStyle: "vertical-align: middle;font-weight: bold;text-align: center;");
                builder.AddTd(channel.CId, customStyle: "vertical-align: middle;text-align: center;");
                builder.AddTd(channel.ServiceName);
                var packageAndBandwidthStr = new StringBuilder();
                if (channel.ServicePackageId > 0)
                {
                    packageAndBandwidthStr.AppendLine("<p>" + channel.PackageName + "</p>");
                }

                packageAndBandwidthStr.AppendLine($"{(channel.HasDistinguishBandwidth ? "Trong nước: " : string.Empty)}{channel.DomesticBandwidth} {channel.DomesticBandwidthUom}");

                if (channel.HasDistinguishBandwidth)
                {
                    packageAndBandwidthStr.AppendLine($"/ Quốc tế: {channel.InternationalBandwidth} {channel.InternationalBandwidthUom}");
                }
                builder.AddTd(packageAndBandwidthStr.ToString());

                builder.OpenTd();
                if (channel.HasStartAndEndPoint)
                {
                    builder.AddHtml($"Điểm đầu: {channel.StartPoint.InstallationAddress.FullAddress}");
                    builder.Break();
                    builder.Break();
                    builder.AddHtml($"Điểm cuối: {channel.EndPoint.InstallationAddress.FullAddress}");
                }
                else
                {
                    builder.AddHtml(channel.EndPoint.InstallationAddress.FullAddress);
                }
                builder.CloseTd();
                builder.CloseTableRow();
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.Break();
            builder.AddHtml($"<p>Danh sách thiết bị tạm giữ:</p>");

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị", 250);
            builder.AddTh("Mã thiết bị", 150);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt");
            builder.AddTh("Số lượng tạm giữ", 150);
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.WillBeHoldUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.WillBeHoldUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();
            builder.Break();
            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
        private string BuildRestoreChannelTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.AddHtml($"<p>Yêu cầu khôi phục hoạt động kênh: {DistinctChannels}.</p>");

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Mã CID", 150);
            builder.AddTh("Dịch vụ", 250);
            builder.AddTh("Gói cước/băng thông", 200);
            builder.AddTh("Địa chỉ lắp đặt");

            builder.OpenTableBody();
            for (int i = 0; i < Transaction.TransactionServicePackages.Count; i++)
            {
                var channel = Transaction.TransactionServicePackages.ElementAt(i);
                builder.OpenTableRow();
                builder.AddTd((i + 1).ToString("D2"), customStyle: "vertical-align: middle;font-weight: bold;text-align: center;");
                builder.AddTd(channel.CId, customStyle: "vertical-align: middle;text-align: center;");
                builder.AddTd(channel.ServiceName);
                var packageAndBandwidthStr = new StringBuilder();
                if (channel.ServicePackageId > 0)
                {
                    packageAndBandwidthStr.AppendLine("<p>" + channel.PackageName + "</p>");
                }

                packageAndBandwidthStr.AppendLine($"{(channel.HasDistinguishBandwidth ? "Trong nước: " : string.Empty)}{channel.DomesticBandwidth} {channel.DomesticBandwidthUom}");

                if (channel.HasDistinguishBandwidth)
                {
                    packageAndBandwidthStr.AppendLine($"/ Quốc tế: {channel.InternationalBandwidth} {channel.InternationalBandwidthUom}");
                }
                builder.AddTd(packageAndBandwidthStr.ToString());

                builder.OpenTd();
                if (channel.HasStartAndEndPoint)
                {
                    builder.AddHtml($"Điểm đầu: {channel.StartPoint.InstallationAddress.FullAddress}");
                    builder.Break();
                    builder.Break();
                    builder.AddHtml($"Điểm cuối: {channel.EndPoint.InstallationAddress.FullAddress}");
                }
                else
                {
                    builder.AddHtml(channel.EndPoint.InstallationAddress.FullAddress);
                }
                builder.CloseTd();
                builder.CloseTableRow();
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.Break();
            builder.AddHtml($"<p>Danh sách thiết bị kỹ thuật đang tạm giữ:</p>");

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị", 250);
            builder.AddTh("Mã thiết bị", 150);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt");
            builder.AddTh("Số lượng đang tạm giữ", 150);
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.SupporterHoldedUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.SupporterHoldedUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();
            builder.Break();
            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
        private string BuildTerminateContractTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.AddHtml($"<p>Yêu cầu thanh lý hợp đồng.</p>");
            builder.AddHtml($"<p>Danh sách các kênh đang hoạt động thuộc hợp đồng cần thanh lý:</p>");
            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Mã CID", 150);
            builder.AddTh("Dịch vụ", 250);
            builder.AddTh("Gói cước/băng thông", 200);
            builder.AddTh("Địa chỉ lắp đặt");
            builder.CloseThead();
            builder.OpenTableBody();
            for (int i = 0; i < Transaction.TransactionServicePackages.Count; i++)
            {
                var channel = Transaction.TransactionServicePackages.ElementAt(i);
                builder.OpenTableRow();
                builder.AddTd((i + 1).ToString("D2"), customStyle: "vertical-align: middle;font-weight: bold;text-align: center;");
                builder.AddTd(channel.CId, customStyle: "vertical-align: middle;text-align: center;");
                builder.AddTd(channel.ServiceName);
                var packageAndBandwidthStr = new StringBuilder();
                if (channel.ServicePackageId > 0)
                {
                    packageAndBandwidthStr.AppendLine("<p>" + channel.PackageName + "</p>");
                }

                packageAndBandwidthStr.AppendLine($"{(channel.HasDistinguishBandwidth ? "Trong nước: " : string.Empty)}{channel.DomesticBandwidth} {channel.DomesticBandwidthUom}");

                if (channel.HasDistinguishBandwidth)
                {
                    packageAndBandwidthStr.AppendLine($"/ Quốc tế: {channel.InternationalBandwidth} {channel.InternationalBandwidthUom}");
                }
                builder.AddTd(packageAndBandwidthStr.ToString());

                builder.OpenTd();
                if (channel.HasStartAndEndPoint)
                {
                    builder.AddHtml($"Điểm đầu: {channel.StartPoint.InstallationAddress.FullAddress}");
                    builder.Break();
                    builder.Break();
                    builder.AddHtml($"Điểm cuối: {channel.EndPoint.InstallationAddress.FullAddress}");
                }
                else
                {
                    builder.AddHtml(channel.EndPoint.InstallationAddress.FullAddress);
                }
                builder.CloseTd();
                builder.CloseTableRow();
            }
            builder.CloseTableBody();
            builder.CloseTable();
            builder.Break();
            builder.AddHtml($"<p>Danh sách thiết bị cần thu hồi:</p>");

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị", 250);
            builder.AddTh("Mã thiết bị", 150);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt");
            builder.AddTh("Số lượng cần thu hồi", 150);
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();
            builder.Break();
            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
        private string BuildTerminateChannelsTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.AddHtml($"<p>Yêu cầu hủy dịch vụ kênh: {DistinctChannels}.</p>");
            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Mã CID", 150);
            builder.AddTh("Dịch vụ", 250);
            builder.AddTh("Gói cước/băng thông", 200);
            builder.AddTh("Địa chỉ lắp đặt");
            builder.CloseThead();
            builder.OpenTableBody();
            for (int i = 0; i < Transaction.TransactionServicePackages.Count; i++)
            {
                var channel = Transaction.TransactionServicePackages.ElementAt(i);
                builder.OpenTableRow();
                builder.AddTd((i + 1).ToString("D2"), customStyle: "vertical-align: middle;font-weight: bold;text-align: center;");
                builder.AddTd(channel.CId, customStyle: "vertical-align: middle;text-align: center;");
                builder.AddTd(channel.ServiceName);
                var packageAndBandwidthStr = new StringBuilder();
                if (channel.ServicePackageId > 0)
                {
                    packageAndBandwidthStr.AppendLine("<p>" + channel.PackageName + "</p>");
                }

                packageAndBandwidthStr.AppendLine($"{(channel.HasDistinguishBandwidth ? "Trong nước: " : string.Empty)}{channel.DomesticBandwidth} {channel.DomesticBandwidthUom}");

                if (channel.HasDistinguishBandwidth)
                {
                    packageAndBandwidthStr.AppendLine($"/ Quốc tế: {channel.InternationalBandwidth} {channel.InternationalBandwidthUom}");
                }
                builder.AddTd(packageAndBandwidthStr.ToString());

                builder.OpenTd();
                if (channel.HasStartAndEndPoint)
                {
                    builder.AddHtml($"Điểm đầu: {channel.StartPoint.InstallationAddress.FullAddress}");
                    builder.Break();
                    builder.Break();
                    builder.AddHtml($"Điểm cuối: {channel.EndPoint.InstallationAddress.FullAddress}");
                }
                else
                {
                    builder.AddHtml(channel.EndPoint.InstallationAddress.FullAddress);
                }
                builder.CloseTd();
                builder.CloseTableRow();
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.AddHtml($"<p>Danh sách thiết bị cần thu hồi:</p>");

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị", 250);
            builder.AddTh("Mã thiết bị", 150);
            builder.AddTh("Kênh lắp đặt", 150);
            builder.AddTh("Địa chỉ lắp đặt");
            builder.AddTh("Số lượng cần thu hồi", 150);
            builder.CloseThead();
            builder.OpenTableBody();
            foreach (var channel in Transaction.TransactionServicePackages)
            {
                var index = 1;
                if (channel.HasStartAndEndPoint)
                {
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        builder.OpenTableRow();
                        builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                        builder.AddTd(equipment.EquipmentName);
                        builder.AddTd(equipment.DeviceCode);
                        builder.AddTd(channel.CId);
                        builder.AddTd(channel.StartPoint.InstallationAddress.FullAddress);
                        builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                        builder.CloseTableRow();
                        index++;
                    }
                }
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(channel.CId);
                    builder.AddTd(channel.EndPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            builder.CloseTableBody();
            builder.CloseTable();

            builder.Break();
            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
        private string BuildChangeServicePackageTransEmail()
        {
            var builder = new TransactionEmailContentBuilder();
            builder.AddMailHeader1(Transaction.ContractCode, Transaction.Contractor);
            builder.Break();
            builder.AddHtml($"Yêu cầu điều chỉnh dịch vụ kênh {Transaction.TransactionServicePackages.First().CId}.");
            builder.Break();

            builder.AddHtml($"Thông tin điều chỉnh:");
            builder.Break();
            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Mã CID", 150);
            builder.AddTh("Loại dịch vụ");
            builder.AddTh("Gói cước/ băng thông");
            builder.AddTh("Địa chỉ lắp đặt", 200);
            builder.AddTh("Phí hòa mạng/ lắp đặt");
            builder.AddTh("Thiết bị mới", 300);
            builder.AddTh("Ghi chú");
            builder.CloseThead();
            builder.OpenTableBody();

            var channel = Transaction.TransactionServicePackages.First(c => c.IsOld != true);
            builder.OpenTableRow();
            builder.AddTd(1.ToString("D2"), customStyle: "vertical-align: middle;font-weight: bold;text-align: center;");
            builder.AddTd(channel.CId, customStyle: "vertical-align: middle;text-align: center;");
            builder.AddTd(channel.ServiceName);
            var packageAndBandwidthStr = new StringBuilder();
            if (channel.ServicePackageId > 0)
            {
                packageAndBandwidthStr.AppendLine("<p>" + channel.PackageName + "</p>");
            }

            packageAndBandwidthStr.AppendLine($"{(channel.HasDistinguishBandwidth ? "Trong nước: " : string.Empty)}{channel.DomesticBandwidth} {channel.DomesticBandwidthUom}");

            if (channel.HasDistinguishBandwidth)
            {
                packageAndBandwidthStr.AppendLine($"/ Quốc tế: {channel.InternationalBandwidth} {channel.InternationalBandwidthUom}");
            }
            builder.AddTd(packageAndBandwidthStr.ToString());

            builder.OpenTd();
            if (channel.HasStartAndEndPoint)
            {
                builder.AddHtml($"Điểm đầu: {channel.StartPoint.InstallationAddress.FullAddress}");
                builder.Break();
                builder.Break();
                builder.AddHtml($"Điểm cuối: {channel.EndPoint.InstallationAddress.FullAddress}");
            }
            else
            {
                builder.AddHtml(channel.EndPoint.InstallationAddress.FullAddress);
            }
            builder.CloseTd();
            string formatedFee = "0";
            if (channel.InstallationFee > 0)
            {
                CultureInfo cul = CultureInfo.GetCultureInfo("vi-VN");
                formatedFee = channel.InstallationFee.ToString("#,###", cul.NumberFormat);
            }
            builder.AddTd($"{formatedFee}đ", customStyle: "vertical-align: middle;text-align: center;");

            builder.OpenTd();
            if (channel.HasStartAndEndPoint)
            {
                builder.AddHtml("Thiết bị điểm đầu: ");
                if (channel.StartPoint.Equipments != null && channel.StartPoint.Equipments.Count > 0)
                {
                    builder.OpenUl();
                    foreach (var equipment in channel.StartPoint.Equipments)
                    {
                        builder.AddHtml($"<li>{equipment.EquipmentName} ({equipment.ExaminedUnit} {equipment.EquipmentUom})</li>");

                    }
                    builder.CloseUl();
                }
                else
                {
                    builder.AddHtml("<i>Không có</i>");
                    builder.Break();
                }

                builder.AddHtml("Thiết bị điểm cuối: ");
                if (channel.EndPoint.Equipments != null && channel.EndPoint.Equipments.Count > 0)
                {
                    builder.OpenUl();
                    foreach (var equipment in channel.EndPoint.Equipments)
                    {
                        builder.AddHtml($"<li>{equipment.EquipmentName} ({equipment.ExaminedUnit} {equipment.EquipmentUom})</li>");

                    }
                    builder.CloseUl();
                }
                else
                {
                    builder.AddHtml("<i>Không có</i>");
                    builder.Break();
                }
            }
            else
            {
                builder.OpenUl();
                foreach (var equipment in channel.EndPoint.Equipments)
                {
                    builder.AddHtml($"<li>{equipment.EquipmentName} ({equipment.ExaminedUnit} {equipment.EquipmentUom})</li>");
                }
                builder.CloseUl();
            }
            builder.CloseTd();
            builder.AddTd(channel.Note, customStyle: "");
            builder.CloseTableRow();
            builder.CloseTableBody();
            builder.CloseTable();
            builder.Break();
            builder.AddHtml("Danh sách thiết bị cần thu hồi: ");
            builder.Break();

            builder.OpenTable();
            builder.OpenThead();
            builder.AddTh("STT", 60);
            builder.AddTh("Tên thiết bị");
            builder.AddTh("Mã thiết bị", 120);
            builder.AddTh("Kênh lắp đặt", 120);
            builder.AddTh("Địa chỉ lắp đặt", 250);
            builder.AddTh("Số lượng thu hồi", 150, "text-align: center;");
            builder.CloseThead();
            builder.OpenTableBody();
            var index = 1;

            var originChannel = Transaction.TransactionServicePackages.First(c => c.IsOld == true);
            if (originChannel.HasStartAndEndPoint)
            {
                foreach (var equipment in originChannel.StartPoint.Equipments)
                {
                    builder.OpenTableRow();
                    builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                    builder.AddTd(equipment.EquipmentName);
                    builder.AddTd(equipment.DeviceCode);
                    builder.AddTd(originChannel.CId);
                    builder.AddTd(originChannel.StartPoint.InstallationAddress.FullAddress);
                    builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                    builder.CloseTableRow();
                    index++;
                }
            }
            foreach (var equipment in originChannel.EndPoint.Equipments)
            {
                builder.OpenTableRow();
                builder.AddTd($"{index:D2}", customStyle: "text-align:center;");
                builder.AddTd(equipment.EquipmentName);
                builder.AddTd(equipment.DeviceCode);
                builder.AddTd(originChannel.CId);
                builder.AddTd(originChannel.EndPoint.InstallationAddress.FullAddress);
                builder.AddTd($"{equipment.WillBeReclaimUnit} {equipment.EquipmentUom}", customStyle: "text-align: center;");
                builder.CloseTableRow();
                index++;
            }

            builder.CloseTableBody();
            builder.CloseTable();

            builder.AddMailFooter(Transaction.Code);
            return builder.Build();
        }
    }
}
