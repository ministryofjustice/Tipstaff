using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Models;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Mappers
{
    public interface IMapperCollections<T, U> where U : DynamoTable
                                              where T : IModel
    {
        IEnumerable<T> GetAll(IEnumerable<U> entities);

        IEnumerable<U> GetAll(IEnumerable<T> entities);
    }
}
