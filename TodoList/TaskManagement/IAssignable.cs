using System;
using System.Collections.Generic;
using System.Text;

namespace TaskManagement
{
    public interface IAssignable
    {
        List<User> Assigned { get; set; }

        uint Limit { get; set; }

        public bool DeleteAssigned(User user);

        public bool AddAssigned(User user);
    }
}
