using AutoMapper;
using KF.Records.Domain;
using KF.Records.UseCases.Records.GetAllRecords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.AutoMapper;

/// <summary>
/// Profile for mapping
/// </summary>
public class AppMappingProfile : Profile
{
    /// <summary>
    /// Creating map
    /// </summary>
    public AppMappingProfile()
    {
        CreateMap<Record, GetRecordDto>().ReverseMap();
    }
}
