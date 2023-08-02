using AutoMapper;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.DTO;

namespace BusinessLogicLayer.Services.Lookups
{
    public class LookupsService : ILookupsService
    {
        private readonly IUnitOfWork _unit;
        private readonly IMapper _mapper;
        public LookupsService(IUnitOfWork unit, IMapper mapper)
        {
            _unit = unit;
            _mapper = mapper;
        }
        public async Task<string> GetDescription(string tableName, string columnName, int columnValue)
        {
            var lookup =  _unit.LookupsRepository
                    .Get(e => e.TableName == tableName 
                    && int.Parse(e.ColumnValue) == columnValue
                    && e.ColumnName == columnName)
                    .FirstOrDefault();

            return lookup.ColumnDescription;
        }

        public async Task<string> GetDescriptionAr(string tableName, string columnName, int columnValue)
        {
            var lookup = _unit.LookupsRepository
                    .Get(e => e.TableName == tableName
                    && int.Parse(e.ColumnValue) == columnValue
                    && e.ColumnName == columnName)
                    .FirstOrDefault();

            return lookup.ColumnDescriptionAr;
        }

        public async Task<IList<LookupDto>> GetLookups(string tableName, string columnName)
        {
            if (columnName == null)
                columnName = string.Empty;

            var lookups =  _unit.LookupsRepository
                    .PQuery(e => e.TableName == tableName && e.ColumnName == columnName)
                    .ToList();

            return _mapper.Map<IList<LookupDto>>(lookups);
        }
    }
}
