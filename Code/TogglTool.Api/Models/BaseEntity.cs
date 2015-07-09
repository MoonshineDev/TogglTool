﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Reflection;
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

        public virtual bool Update(BaseEntity newData)
        {
            if (GetType() != newData.GetType())
                return false;
            var changed = false;
            foreach (var prop in GetType().GetProperties().Where(x => x.CanRead))
            {
                var oldValue = prop.GetValue(this);
                var newValue = prop.GetValue(newData);
                if (oldValue == null)
                {
                    if (newValue == null)
                        continue;
                }
                else if (oldValue.Equals(newValue))
                    continue;
                if (oldValue is IEnumerable<object>)
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
