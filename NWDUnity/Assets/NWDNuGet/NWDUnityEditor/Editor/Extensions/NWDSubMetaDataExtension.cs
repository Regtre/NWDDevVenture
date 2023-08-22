using NWDEditor;

namespace NWDEditor
{
    static class NWDSubMetaDataExtension
    {
        /// <summary>
        /// Checks if a <see cref="NWDSubMetaData"/> can be edited in inspector.
        /// </summary>
        /// <param name="sObject">The current <see cref="NWDSubMetaData"/>.</param>
        /// <returns>true if data is editable, false otherwise.</returns>
        static public bool CanEditData (this NWDSubMetaData sObject)
        {
            return (sObject.Origin == 0 || sObject.State.HasFlag(NWDSubMetaDataState.Overriden)) && !sObject.Trashed;
        }

        /// <summary>
        /// Checks if a data was inserted has a value in database.
        /// </summary>
        /// <param name="sObject">The current <see cref="NWDSubMetaData"/>.</param>
        /// <returns>true if data is in database, false otherwise.</returns>
        static public bool ExistsInDatabase (this NWDSubMetaData sObject)
        {
            return !string.IsNullOrEmpty(sObject.Data);
        }
    }
}