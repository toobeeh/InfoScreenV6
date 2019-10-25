using System;
using System.Text;
using System.Collections;
using System.Web.Security;

using System.Security.Principal;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

//http://msdn.microsoft.com/de-de/library/ms180890(v=vs.90).aspx

namespace Infoscreen_Verwaltung.classes
{
    /// <summary>
    /// Eine Klasse zum Prüfen der Login Informationen und zum Abrufen der Daten eines Benutzers aus der Domain.
    /// </summary>
    public class Authentifizierung
    {
        /// <summary>
        /// Prüfen ob der angegebene Benutzer authentifiziert ist.
        /// </summary>
        /// <param name="_domain">Die Domain des Benutzers</param>
        /// <param name="_username">Der Benutzername des Benutzers</param>
        /// <param name="_pwd">Das Kennwort des Benutzers</param>
        /// <returns>Ob die angegebenen Informationen korrekt sind.</returns>
        static public bool IsAuthenticated(string _domain, string _username, string _pwd)
        {
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _domain))
            {
                return context.ValidateCredentials(_username, _pwd, ContextOptions.Negotiate);
            }
        }

        /// <summary>
        /// Gibt den vollständigen Namen des Benutzers zurück.
        /// </summary>
        /// <param name="_username">Der Benutzername des Benutzers, wessen Name zurückgegeben werden soll.</param>
        /// <returns>Den Namen des Benutzers</returns>
        static public string GetName(string _username)
        {
            DirectoryEntry entry = new DirectoryEntry(
                "LDAP://" + Properties.Resources.standardDomain, 
                Properties.Resources.domainUser, 
                Properties.Resources.domainPassword, 
                AuthenticationTypes.Secure);

            entry.Username = Properties.Resources.domainUser;
            entry.Password = Properties.Resources.domainPassword;
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(cn=" + _username + ")";
            search.PropertiesToLoad.Add("displayName");
            string name;

            try
            {
                SearchResult result = search.FindOne();
                name = (string)result.Properties["displayName"][0];
            }
            catch (Exception ex)
            {
                //throw new Exception("Error obtaining name. " + ex.Message);
                name = "Unbekannt";
            }
            return name;
        }

        /// <summary>
        /// Gibt die Klasse eines Schülers zurück.
        /// </summary>
        /// <param name="_username">Der Benutzername des Schülers, desen Klasse zurückgegeben werden soll.</param>
        /// <returns>Die Klasse des übergebenen Schülers.</returns>
        static public string GetKlasse(string _username)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + Properties.Resources.standardDomain, Properties.Resources.domainUser, Properties.Resources.domainPassword, AuthenticationTypes.Secure);
            entry.Username = Properties.Resources.domainUser;
            entry.Password = Properties.Resources.domainPassword;
            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(cn=" + _username + ")";
            search.PropertiesToLoad.Add("physicalDeliveryOfficeName");
            string klasse;

            try
            {
                SearchResult result = search.FindOne();
                klasse = (string)result.Properties["physicalDeliveryOfficeName"][0];
            }
            catch (Exception ex)
            {
                //throw new Exception("Error obtaining office. " + ex.Message);
                klasse = "";
            }
            return klasse;
        }
        /// <summary>
        /// Gibt die ID des Benutzers zurück.
        /// </summary>
        /// <param name="_username">Der Benutzername des Benutzers, wessen Name zurückgegeben werden soll.</param>
        /// <returns>Die ID des Benutzers</returns>
        static public string GetID(string _username, string _klasse)
        {
            DirectoryEntry entry = new DirectoryEntry("LDAP://" + Properties.Resources.standardDomain, Properties.Resources.domainUser, Properties.Resources.domainPassword, AuthenticationTypes.Secure);
            entry.Username = Properties.Resources.domainUser;
            entry.Password = Properties.Resources.domainPassword;

            DirectorySearcher search = new DirectorySearcher(entry);
            search.Filter = "(displayName=" + _username + ")";
            search.PropertiesToLoad.Add("userPrincipalName");

            string id;
            
            try
            {
                SearchResult result = search.FindOne();
                id = (string)result.Properties["userPrincipalName"][0];
                
            }
            catch (Exception ex)
            {
                //throw new Exception("Error obtaining id. " + ex.Message);
                id = "Unbekannt";
            }

            return id;
        }
    }
}