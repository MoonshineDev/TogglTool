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
            LogCreatedOn = DateTime.UtcNow;
        }

        public void Update(BaseEntity newData)
        {
            LogCreatedOn = DateTime.UtcNow;
            // TODO: Update all properties by reflection
        }
    }
}
