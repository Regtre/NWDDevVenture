using System.Reflection;

namespace NWDUnityEditor.Tools
{
    public class NWDListPropertyDrawer : NWDArrayPropertyDrawer
    {

        public NWDListPropertyDrawer(INWDPropertyDrawer sModelTypeField) : base(sModelTypeField)
        {

        }
        public NWDListPropertyDrawer(PropertyInfo sPropertyInfo, INWDPropertyDrawer sModelTypeField) : base(sPropertyInfo, sModelTypeField)
        {

        }

        public override void SetPropertyInfo(PropertyInfo sPropertyInfo)
        {
            if (Property == null)
            {
                Property = new NWDListSerializedProperty(sPropertyInfo);
            }
        }
    }
}
