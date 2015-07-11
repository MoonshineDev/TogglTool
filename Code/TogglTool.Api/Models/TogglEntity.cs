using System.Diagnostics.CodeAnalysis;

namespace TogglTool.Api.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
