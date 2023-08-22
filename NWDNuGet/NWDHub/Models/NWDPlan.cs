namespace NWDFoundation.Models
{
    [Serializable]
    public enum NWDPlan
    {
        /// <summary>
        /// Totally free ... no cluster, no service, local device only
        /// </summary>
        Disconnected = 0,
        /// <summary>
        /// pay only active user on standard game
        /// </summary>
        Standard = 1,
        /// <summary>
        /// pay for your server user on standard game
        /// </summary>
        Dedicated = 2,
        /// <summary>
        /// pay for your server user on customized model game
        /// </summary>
        Custom = 3,
        /// <summary>
        /// manage your cluster by yourself
        /// </summary>
        Community = 4,
        /// <summary>
        /// manage your cluster by yourself
        /// </summary>
        Fork = 5,
    }
}