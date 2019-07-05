using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo.User.Identity.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using Demo.Data.Mongo;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;

namespace Demo.User.Identity
{
    public class UserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _memoryCache;
        private readonly IMongoCollection<ApplicationUser> _collection;

        public UserService(IDatabase db, UserManager<ApplicationUser> userManager, IMemoryCache memoryCache)
        {
            _userManager = userManager;
            _memoryCache = memoryCache;
            var database = db.GetDatabase();
            _collection = database.GetCollection<ApplicationUser>("site.user");
        }

        public async Task SaveAsync(ApplicationUser user)
        {
            _memoryCache.Remove(user.Id);
            await _userManager.UpdateAsync(user);
        }

        public async Task RemoveRoleAsync(string role)
        {
            var users =  await _userManager.GetUsersInRoleAsync(role);
            foreach (var user in users)
            {
                _memoryCache.Remove(user.Id);
                await _userManager.RemoveFromRoleAsync(user, role);
            }
        }

        public async Task<IList<ApplicationUser>> UserByRoleAsync(string role)
        {
            return  await _userManager.GetUsersInRoleAsync(role);
        }


        public async Task<ApplicationUser> FindApplicationUserByIdAsync(string userId)
        {
            ApplicationUser user;
            _memoryCache.TryGetValue<ApplicationUser>(userId, out user);
            if ( user != null)
            {
                return user;
            }
            user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await
                    _memoryCache.GetOrCreateAsync(userId, entry =>
                    {
                        entry.SlidingExpiration = TimeSpan.FromHours(3);
                        return Task.FromResult(user);
                    });
            }

            return user;
        }

        public async Task<ApplicationUser> FindApplicationUserByEmailAsync(string userEmail)
        {
            return await _userManager.FindByEmailAsync(userEmail);
        }

        public async Task<UserInfo> GetUserInfoAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return null;
            }

            var userDb = await FindApplicationUserByIdAsync(userId);

            if (userDb == null)
            {
                return null;
            }

            var userInfo = new UserInfo();
            userInfo.UserName = userDb.FullName;
            userInfo.AuthorUrl = userDb.AuthorUrl;

            return userInfo;
        }

        public async Task<IList<ApplicationUser>> GetAllAsync()
        {
            var userDb = await _collection.Find(new BsonDocument()).ToListAsync();

            var list = new List<ApplicationUser>();

            foreach (var identityUser in userDb)
            {
                list.Add(identityUser);
            }

            return list;
        }

        public async void RemoveAsync(string userId)
        {
            _memoryCache.Remove(userId);
            var builder = Builders<ApplicationUser>.Filter;
            var filter = builder.Eq(x => x.Id, userId);
            await _collection.DeleteOneAsync(filter);
        }
        
    }
}