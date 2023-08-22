namespace NWDUnityEditor.Tools
{
    public interface INWDCollectionSerializedProperty
    {
        public string Name { get; }
        public string ElementName { get; }
        public void SetIndex(int sIndex);
        public object GetValueAt(int sIndex);
        public void SetValueAt(int sIndex, object sValue);
        public object GetValue();
        public void SetValue(object sValue);
        public int GetLength();
        public void Resize(int sSize);
        public int GetHashCode();
    }
}

