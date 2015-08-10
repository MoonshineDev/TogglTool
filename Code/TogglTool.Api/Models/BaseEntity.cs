using System;
using System.Collections.Generic;
using System.Linq;

namespace TogglTool.Api.Models
{
    public abstract class BaseEntity
    {
        public DateTime? LogCreatedOn { get; set; }
        public string LogCreatedBy { get; set; }
        public DateTime? LogChangedOn { get; set; }
        public string LogChangedBy { get; set; }

        protected BaseEntity()
        {
            // TODO: Move LogCreatedOn update to DatabaseContext
            // TODO: Set LogCreatedBy based on SecurityContext
            LogCreatedOn = DateTime.UtcNow;
        }

        public virtual bool Update(BaseEntity newData)
        {
            if (newData == null)
                return false;
            if (GetType() != newData.GetType())
                return false;
            var changed = false;
            foreach (var prop in GetType().GetProperties().Where(x => x.CanRead))
            {
                var oldValue = prop.GetValue(this);
                var newValue = prop.GetValue(newData);
                if (newValue == null)
                    continue;
                if (oldValue != null && oldValue.Equals(newValue))
                    continue;
                // TODO: Use prop.PropertyType.IsGenericType instead
                if (oldValue is IEnumerable<object> && newValue is IEnumerable<object>)
                {
                    var oldCollection = oldValue as IEnumerable<object>;
                    var newCollection = newValue as IEnumerable<object>;
                    if (new HashSet<object>(oldCollection).SetEquals(newCollection))
                        continue;
                }
                prop.SetValue(this, newValue);
                changed = true;
            }
            if(changed)
                LogChangedOn = DateTime.UtcNow;
            return changed;
        }
    }
}
