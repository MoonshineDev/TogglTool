using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api.Models
{
    public abstract class BaseEntity
    {
        public DateTime? LogCreatedOn { get; set; }
        public string LogCreatedBy { get; set; }
        public DateTime? LogChangedOn { get; set; }
        public string LogChangedBy { get; set; }

        public BaseEntity()
        {
            // TODO: Move LogCreatedOn update to DatabaseContext
            // TODO: Set LogCreatedBy based on SecurityContext
            LogCreatedOn = DateTime.UtcNow;
        }

        public void Update(BaseEntity newData)
        {
            var changed = false;
            // TODO: Update all properties by reflection
            // TODO: Set changed = true if any value is changed
            // TODO: Move LogChangedOn update to DatabaseContext
            if(changed)
                LogChangedOn = DateTime.UtcNow;
        }
    }
}
