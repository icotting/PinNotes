using PinNotes.Accessors.Domain.Contracts;
using PinNotes.Accessors.Domain.Core.EntityFramework;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using PinNotes.Accessors.Domain.Contracts.DTO;
using System.Collections.Generic;

namespace PinNotes.Accessors.Domain.Core
{
    public class UserAccessor : EntityAccessor, IUserAccessor
    {
        private DbSet<Models.User> _users;

        public UserAccessor(IDbSetFactory dbSetFactory) : base(dbSetFactory)
        {
            this._users = dbSetFactory.CreateDbSet<Models.User>();
        }

        public Contracts.DTO.User AddUser(Contracts.DTO.User user)
        {
            if (user == null
                || user.FirstName == null 
                || user.LastName == null 
                || user.Email == null )
            {
                throw new ArgumentException("A user must have a first name, last name, and email address");
            }
            else if (this._users.Where(u => u.UserId == user.UserId)
                .FirstOrDefault() != null)
            {
                throw new ArgumentException(string.Format("User already exists with ID {0}", user.UserId));
            }
            else
            {
                var id = Guid.NewGuid().ToString();

                var userEntity = new Models.User()
                {
                    UserId = id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    AvatarUrl = user.AvatarUrl,
                    Email = user.Email,
                    Location = user.Location
                };

                this._users.Add(userEntity);

                user.UserId = id;
                return user;
            }
        }

        public ICollection<Contracts.DTO.User> AllUsers()
        {
            return this._users.ToList<Models.User>().Select(u => new Contracts.DTO.User()
            {
                UserId = u.UserId,
                FirstName = u.FirstName, 
                LastName = u.LastName,
                AvatarUrl = u.AvatarUrl, 
                Location = u.Location, 
                Email = u.Email
            }).ToList<Contracts.DTO.User>();
        }

        public Contracts.DTO.User FindUser(string userId)
        {
            var user = this._users.Where(u => u.UserId == userId).FirstOrDefault();

            if ( user != null )
            {
                return new Contracts.DTO.User()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName, 
                    LastName = user.LastName, 
                    AvatarUrl = user.AvatarUrl, 
                    Location = user.Location, 
                    Email = user.Email
                };
            }
            else 
            {
                return null;
            }
        }

        public Contracts.DTO.User FindUserByEmail(string email)
        {
            var user = this._users.Where(u => u.Email == email).FirstOrDefault();

            if ( user != null )
            {
                return new Contracts.DTO.User()
                {
                    UserId = user.UserId,
                    FirstName = user.FirstName, 
                    LastName = user.LastName, 
                    AvatarUrl = user.AvatarUrl, 
                    Location = user.Location, 
                    Email = user.Email
                };
            }
            else 
            {
                return null;
            }
        }

        public void RemoveUser(User user)
        {
            var entity = this._users.Where(u => u.UserId == user.UserId).FirstOrDefault();
            if (entity != null)
            {
                this._users.Remove(entity);
            } 
            else
            {
                throw new ArgumentException(string.Format("User {0} not found", user.UserId));
            }
        }
    }
}
