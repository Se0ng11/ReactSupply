using System;

namespace ReactSupply.Models.ViewModel.User
{
    public class UserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTimeOffset? LockupEnd { get; set; }
        public bool Locked { get; set; }
    }
}
