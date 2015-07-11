using System;

namespace TogglTool.Api.Models
{
    public abstract class WebApiEntity : BaseEntity
    {
        public DateTime? ExpirationOn { get; set; }
        public DateTime? FirstRequestedOn { get; set; }
        public DateTime? LastRequestedOn { get; set; }
        public DateTime? LastChangedOn { get; set; }

        protected WebApiEntity()
        {
            FirstRequestedOn = DateTime.UtcNow;
        }

        public override bool Update(BaseEntity newData)
        {
            var changed = base.Update(newData);
            LastRequestedOn = DateTime.UtcNow;
            if (changed)
                LastChangedOn = DateTime.UtcNow;
            return changed;
        }
    }
}
