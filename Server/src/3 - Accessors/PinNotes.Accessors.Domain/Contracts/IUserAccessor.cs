using System.Collections.Generic;

namespace PinNotes.Accessors.Domain.Contracts
{
    public interface IUserAccessor
    {
        DTO.User FindUser(string email);
        DTO.User FindUser(long userId);
        long AddUser(DTO.User user);
        void RemoveUser(DTO.User user);
        ICollection<DTO.User> AllUsers();
    }
}
