using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api.Models
{
    public abstract class TogglEntity : BaseEntity
    {
        public int id { get; set; }

        public override void Update(BaseEntity newData)
        {
            var entity = newData as TogglEntity;
            if (entity == null)
                return;
            if (id != entity.id)
                return;
            base.Update(newData);
        }
    }
}
