using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using Tinder_Admin.Data;
using Tinder_Admin.Entities;
using Tinder_Admin.Models;
using Tinder_Admin.Repository.Shared;

namespace Tinder_Admin.Repository
{
    public class _UserRepository : ICRUDRepository<AppUser>
    {
        private readonly TinderCoreDBContext __context;

        public _UserRepository(TinderCoreDBContext context)
        {
            __context = context;
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await __context.Users.ToListAsync();
        }

        public async Task<AppUser> GetById(int id)
        {
            var user = await __context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }

        public async Task<PagedResponseData<AppUser>> GetPaged(QueryData queryData)
        {
            int totalRecords;

            if (queryData.GetAllRecords)
            {
                var entityList = await __context.Users.ToListAsync();
                totalRecords = entityList.Count;

                return new PagedResponseData<AppUser>
                {
                    Data = entityList,
                    Pager = new PagerInfo
                    {
                        TotalRecords = totalRecords,
                        PageSize = totalRecords,
                        TotalPages = 1,
                        FirstPage = 1,
                        PreviousPage = 1,
                        PageNumber = 1,
                        NextPage = 1,
                        LastPage = 1
                    }
                };
            }

            int pageNumber = queryData.PageNumber;
            int pageSize = queryData.PageSize;

            var pagedEntityList = await __context.Users
                                        .Skip((pageNumber - 1) * pageSize)
                                        .Take(pageSize)
                                        .ToListAsync();

            totalRecords = await __context.Users.CountAsync();
            int totalPages = totalRecords / pageSize;
            if (totalPages * pageSize < totalRecords)
            {
                totalPages += 1;
            }

            int lastPage = totalPages;

            if (pageNumber == 0)
            {
                pageNumber = 1;
            }

            int nextPage = pageNumber + 1;
            if (nextPage > lastPage)
            {
                nextPage = lastPage;
            }

            int previousPage = pageNumber - 1;
            if (previousPage < 1)
            {
                previousPage = 1;
            }

            var responseData = new PagedResponseData<AppUser>
            {
                Data = pagedEntityList,
                Pager = new PagerInfo
                {
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    TotalPages = totalPages,
                    FirstPage = 1,
                    PreviousPage = previousPage,
                    PageNumber = pageNumber,
                    NextPage = nextPage,
                    LastPage = lastPage
                }
            };

            return responseData;
        }


        public async Task<AppUser> Create(AppUser model)
        {
            __context.Users.Add(model);
            await __context.SaveChangesAsync();
            return model;
        }

        public async Task BatchCreate(IEnumerable<AppUser> models)
        {
            __context.Users.AddRange(models);
            await __context.SaveChangesAsync();
        }

        public async Task<AppUser> Update(int id, AppUser model)
        {
            var user = await __context.Users.FindAsync(id);

            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            __context.Entry(model).State = EntityState.Modified;
            await __context.SaveChangesAsync();
            return model;
        }

        public Task BatchUpdate(IEnumerable<AppUser> models)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> Delete(int id)
        {
            var user = await __context.Users.FindAsync(id);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            user.DeletedAt = DateTime.Now;
            __context.Entry(user).State = EntityState.Modified;
            await __context.SaveChangesAsync();

            return user;
        }

        public Task BatchDelete(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAll()
        {
            var users = await __context.Users.ToListAsync();
            foreach (var user in users)
            {
                user.DeletedAt = DateTime.Now;
                __context.Entry(user).State = EntityState.Modified;
            }
            await __context.SaveChangesAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await __context.Users.AnyAsync(u => u.Id.Equals(id.ToString()));
        }

        public async Task<IEnumerable<AppUser>> Find(Func<AppUser, bool> predicate)
        {
            return await Task.FromResult(__context.Users.Where(predicate));
        }

        public async Task<IEnumerable<AppUser>> GetByField<TField>(Expression<Func<AppUser, TField>> fieldSelector, TField value)
        {
            var parameter = Expression.Parameter(typeof(AppUser), "user");
            var body = Expression.Equal(fieldSelector.Body, Expression.Constant(value));
            var lambda = Expression.Lambda<Func<AppUser, bool>>(body, parameter);

            var users = await __context.Users.Where(lambda).ToListAsync();
            return users;
        }


        public async Task<int> Count()
        {
            return await __context.Users.CountAsync();
        }
    }
}
