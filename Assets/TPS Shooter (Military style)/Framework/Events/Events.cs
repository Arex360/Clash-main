using System;
using System.Reflection;

namespace LightDev
{
  public partial class Events
  {
    static Events()
    {
      FieldInfo[] fields = typeof(Events).GetFields(BindingFlags.Public | BindingFlags.Static);

      foreach (FieldInfo field in fields)
      {
        if (field.FieldType.IsSubclassOf(typeof(IEvent)))
        {
          field.SetValue(null, Activator.CreateInstance(field.FieldType, field.Name));
        }
      }
    }
  }
}
