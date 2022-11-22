using AutoMapper;
using ContractManagement.Domain.AggregatesModel.CurrencyUnitAggregate;
using ContractManagement.Domain.AggregatesModel.EquipmentAggregate;
using ContractManagement.Domain.AggregatesModel.OutContractAggregate;
using ContractManagement.Domain.Commands.EquipmentTypeCommand;
using ContractManagement.Domain.Commands.OutContractCommand;
using ContractManagement.Domain.Exceptions;
using ContractManagement.Domain.Models.OutContracts;
using ContractManagement.Infrastructure.Queries;
using ContractManagement.Infrastructure.Repositories.ContractEquipmentRepository;
using ContractManagement.Utility;
using Global.Models.StateChangedResponse;
using MediatR;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ContractManagement.API.Application.Commands.OutContractCommandHandler
{
    public class ImportContractEquipmentCommandHandler : IRequestHandler<ImportContractEquipmentCommand, ActionResponse<int>>
    {
        private readonly ILogger<ImportContractEquipmentCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IContractEquipmentRepository _contractEquipmentRepository;
        private readonly IEquipmentTypeQueries _equipmentTypeQueries;
        private readonly IOutContractServicePackageQueries _channelQueries;

        public ImportContractEquipmentCommandHandler(
            IMapper mapper,
            IContractEquipmentRepository contractEquipmentRepository,
            IEquipmentTypeQueries equipmentTypeQueries,
            IOutContractServicePackageQueries channelQueries,
            IMediator mediator,
            ILogger<ImportContractEquipmentCommandHandler> logger)
        {
            this._mapper = mapper;
            this._contractEquipmentRepository = contractEquipmentRepository;
            this._equipmentTypeQueries = equipmentTypeQueries;
            this._channelQueries = channelQueries;
            this._mediator = mediator;
            _logger = logger;
        }

        public async Task<ActionResponse<int>> Handle(ImportContractEquipmentCommand request, CancellationToken cancellationToken)
        {
            var commandResponse = new ActionResponse<int>();
            try
            {
                using (var stream = new MemoryStream())
                {
                    var timer = new Stopwatch();
                    timer.Start();
                    await request.FormFileOutContract.CopyToAsync(stream, cancellationToken);
                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var importModels = new List<ImportContractEquipment>();
                        var allEquipmentTypes = this._equipmentTypeQueries.GetAll();
                        var allChannels = this._channelQueries.GetAllOutChannels();

                        var rowCount = package.Workbook.Worksheets[1].Dimension.Rows;
                        for (int row = 5; row <= rowCount; row++)
                        {
                            var wsHandler = new ExcelWorksheetHandler(package.Workbook.Worksheets[1], row);
                            var equipQuantity = wsHandler.GetFloat("J");

                            if (string.IsNullOrWhiteSpace(wsHandler.Get("I")) || !equipQuantity.HasValue || equipQuantity <= 0)
                            {
                                continue;
                            }

                            var targetChannel = allChannels.FirstOrDefault(c =>
                                string.IsNullOrWhiteSpace(wsHandler.Get("L"))
                                ? c.EndPoint.InstallationAddress.Street.ToAscii().Equals(wsHandler.GetAscii("D"))
                                : wsHandler.Get("L").EqualsIgnoreCase(c.RadiusAccount)
                            );

                            if (targetChannel == null) continue;

                            var equipmentType = allEquipmentTypes.FirstOrDefault(e => e.Code.EqualsIgnoreCase(wsHandler.Get("I")));
                            if (equipmentType == null)
                            {
                                var createNewEquipTypeCmd = new CreateEquipmentTypeCommand()
                                {
                                    Code = wsHandler.GetAscii("I"),
                                    Name = wsHandler.Get("H"),
                                    CurrencyUnitId = CurrencyUnit.VND.Id,
                                    UnitOfMeasurement = UnitOfMeasurement.DefaultUnit.Label,
                                    UnitOfMeasurementId = UnitOfMeasurement.DefaultUnit.Id,
                                    CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode,
                                    Price = 0
                                };

                                var createNewEquipTypeRsp = await this._mediator.Send(createNewEquipTypeCmd);
                                if (!createNewEquipTypeRsp.IsSuccess) throw new ContractDomainException($"Can not create Equipment Type {createNewEquipTypeCmd.Code}: {createNewEquipTypeRsp.Message}"); ;

                                equipmentType = createNewEquipTypeRsp.Result;
                            }

                            var newEquip = new ContractEquipment()
                            {
                                Culture = "vi",
                                CreatedBy = "System",
                                CreatedDate = DateTime.Now,
                                DeviceCode = equipmentType.Code,
                                EquipmentName = equipmentType.Name,
                                EquipmentId = equipmentType.Id,
                                IsActive = true,
                                IsDeleted = false,
                                EquipmentUom = UnitOfMeasurement.DefaultUnit.Label,
                                CurrencyUnitCode = CurrencyUnit.VND.CurrencyUnitCode,
                                CurrencyUnitId = CurrencyUnit.VND.Id,
                                OutContractPackageId = targetChannel.Id,
                                OutputChannelPointId = targetChannel.EndPoint.Id,
                                Manufacturer = equipmentType.Manufacturer,
                                Specifications = equipmentType.Specifications,
                                UnitPrice = equipmentType.Price
                            };

                            newEquip.SetExaminedUnits(equipQuantity.Value);
                            newEquip.SetRealUnits(equipQuantity.Value);
                            newEquip.SetStatusId(EquipmentStatus.Deployed.Id);
                            newEquip.UpdateSerialCode(wsHandler.Get("K"));
                            newEquip.CalculateTotal();

                            importModels.Add(_mapper.Map<ImportContractEquipment>(newEquip));
                        }

                        if (importModels.Count > 0)
                        {
                            Console.WriteLine(importModels.Count);
                            var importedContractSrvPcks = await _contractEquipmentRepository.InsertBulk(importModels);
                            if (importedContractSrvPcks <= 0) throw new ContractDomainException();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.Message);
            }

            return commandResponse;
        }
    }
}
