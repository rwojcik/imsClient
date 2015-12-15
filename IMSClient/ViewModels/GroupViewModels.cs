using System;
using System.Collections.Generic;

namespace IMSPrototyper.ViewModels
{
    public class GroupViewModel
    {
        public long GroupId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public string CreatedBy { get; set; }
        
        public string UpdatedBy { get; set; }

        public ICollection<long> DevicesIds { get; set; }

        public long DevicesCount => DevicesIds?.Count ?? 0;
    }
    
    public class AddGroupViewModel
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }

    public class UpdateGroupViewModel
    {
        public long GroupId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
