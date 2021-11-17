using System;

namespace MedHelper_EF.Models
{
    public class BaseEntity
    {
        public int GetId()
        {
            var type = GetType();
            var propertyName = type.Name + "ID";

            return (int)type.GetProperty(propertyName).GetValue(this, null);
        }
    }
}