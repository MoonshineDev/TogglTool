using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api.Models
{
    public abstract class TogglEntity : WebApiEntity
    {
        public int id { get; set; }

        public override bool Update(BaseEntity newData)
        {
            var entity = newData as TogglEntity;
            if (entity == null)
                return false;
            if (id != entity.id)
                return false;
            return base.Update(newData);
        }
    }
}
