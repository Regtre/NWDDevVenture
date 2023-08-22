using NWDFoundation.Configuration.Environments;
using System;
using NWDFoundation.Models;
using System.Collections.Generic;

namespace NWDFoundation.Configuration.Permissions
{
    [Serializable]
    //[Obsolete("Pourquoi c'est un heritage de 'NWDDatabaseWebBasicModel' ... c'est pas un heritage de NWDDatabasebBasicModel?")]
    public class NWDDataTrackDescription: NWDDatabaseWebBasicModel
    {
        /// <summary>
        /// The datatrack identifier.
        /// There is a unique couple (<see cref="Id"/>;<see cref="Kind"/>).
        /// </summary>
        public int Track { get; set; }
        public ulong StalkedReference { get; set; }
            
        /// <summary>
        /// The environment of the data track.
        /// There is a unique couple (<see cref="Id"/>;<see cref="Kind"/>).
        /// </summary>
        public NWDEnvironmentKind Kind { get; set; }

        /// <summary>
        /// The name of the data track.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The color of the data track.
        /// </summary>
        public string Color { get; set; } = "FFFFFFFF";

        public NWDDataTrackDescription()
        {

        }
    }
}
