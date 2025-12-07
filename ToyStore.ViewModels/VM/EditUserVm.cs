using System.Collections.Generic;

namespace ToyStore.ViewModels.VM
{
    public class EditUserVm
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public List<string> AllRoles { get; set; } = new();
        public string SelectedRole { get; set; }
    }
}
