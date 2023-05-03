using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KF.Records.UseCases.Records.GetAllRecords;

/// <summary>
/// Parametrs for getting reacords
/// </summary>
public class GetAllRecordsQuery: IRequest <List<GetRecordDto>>
{
}
