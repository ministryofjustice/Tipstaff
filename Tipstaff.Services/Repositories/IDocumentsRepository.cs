using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tipstaff.Services.DynamoTables;

namespace Tipstaff.Services.Repositories
{
    public interface IDocumentsRepository
    {
        void AddDocument(Document doc);

        void DeleteDocument(Document doc);
    
        Document GetDocument(string id);

        Document GetEntityByObjectKey(object key);
    }
}
