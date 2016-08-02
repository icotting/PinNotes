using System.Collections.Generic;

namespace PinNotes.Accessors.Domain.Contracts
{
    public interface IUserAccessor
    {
        DTO.User FindUserByEmail(string email);
        DTO.User FindUser(string userId);
        DTO.User AddUser(DTO.User user);
        void RemoveUser(DTO.User user);
        ICollection<DTO.User> AllUsers();
    }
}
