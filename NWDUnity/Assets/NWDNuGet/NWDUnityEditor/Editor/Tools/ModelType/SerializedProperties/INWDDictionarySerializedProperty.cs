namespace NWDUnityEditor.Tools
{
    public interface INWDDictionarySerializedProperty
    {
        public string Name { get; }
        public string ElementName { get; }
        public void SetIndexForKey(int sIndex);
        public void SetIndexForValue(int sIndex);
        public object GetValueAt(int sIndex);
        public void SetValueAt(int sIndex, object sValue);
        public object GetValue();
        public void SetValue(object sValue);
        public int GetLength();
        public void Add(object sKey, object sValue);
        public void RemoveAt(int sIndex);
        public bool IsValidNewKey(object sKey);
        public int GetHashCode();
    }
}

