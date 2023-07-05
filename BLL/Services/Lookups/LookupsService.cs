using AutoMapper;
using BusinessLogicLayer.UnitOfWork;
using DataAccessLayer.Models;

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

        public async Task<IList<LookupTable>> GetLookups(string tableName, string columnName)
        {
            return _unit.LookupsRepository
                    .Get(e => e.TableName == tableName && e.ColumnName == columnName)
                    .ToList();
        }
    }
}
