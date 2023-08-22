using System;

namespace NWDFoundation.Configuration.Environments
{
    /// <summary>
    /// Define the Net-Worked-data environment concept :
    /// <list type="bullet">
    ///     <item>
    ///         <term><see cref="Dev"/></term>
    ///         <description> environment to create the game</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="PlayTest"/></term>
    ///         <description> environment to parameter the game</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Qualification"/></term>
    ///         <description> environment to prepare the release</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="PreProduction"/></term>
    ///         <description> environment to test the release</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="Production"/></term>
    ///         <description> environment to play</description>
    ///     </item>
    ///     <item>
    ///         <term><see cref="PostProduction"/></term>
    ///         <description> environment for the customer service</description>
    ///     </item>
    /// </list>
    /// </summary>
    [Serializable]
    public enum NWDEnvironmentKind
    {
        /// <summary>
        /// Developers use this environment to create the game
        /// </summary>
        Dev = 0,
        /// <summary>
        /// Level-designers use this environment to parameter the game
        /// </summary>
        PlayTest = 1,
        /// <summary>
        /// Editors use this environment to create an new data train from playtest to prepare an PreProduction package
        /// </summary>
        Qualification = 2,
        /// <summary>
        /// Editors can test PreProduction package in this environment before publish data train package 
        /// </summary>
        PreProduction = 3,
        /// <summary>
        /// Players use this environment to play
        /// </summary>
        Production = 4,
        /// <summary>
        /// Customer-Service can use this environment to copy Player data and try to solve/debug 
        /// </summary>
        PostProduction = 5,
    }
}