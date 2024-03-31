using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Linq.Expressions;
using Tinder_Admin.Models;

namespace Tinder_Admin.Repository.Shared
{
    public interface ICRUDRepository<ModelType> where ModelType : class
    {
        Task<IEnumerable<ModelType>> GetAll();

        Task<ModelType> GetById(int id);

        Task<IEnumerable<ModelType>> GetByField<TField>(Expression<Func<ModelType, TField>> fieldSelector, TField value);

        Task<ModelType> Create(ModelType model);

        Task BatchCreate(IEnumerable<ModelType> models);

        Task<ModelType> Update(int id, ModelType model);

        Task BatchUpdate(IEnumerable<ModelType> models);

        Task<ModelType> Delete(int id);

        Task BatchDelete(IEnumerable<int> ids);

        Task DeleteAll();

        Task<IEnumerable<ModelType>> Find(Func<ModelType, bool> predicate);

        Task<bool> Exists(int id);

        Task<int> Count();

        Task<PagedResponseData<ModelType>> GetPaged(QueryData query);
    }
}
