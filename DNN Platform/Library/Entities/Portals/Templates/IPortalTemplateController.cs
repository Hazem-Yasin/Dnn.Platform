﻿using DotNetNuke.Entities.Users;
using DotNetNuke.Services.Localization;
using System;
using System.Collections.Generic;

namespace DotNetNuke.Entities.Portals.Templates
{
    /// <summary>
    /// Work with Portal Templates.
    /// </summary>
    public interface IPortalTemplateController
    {
        /// <summary>
        /// Processess a template file for the new portal.
        /// </summary>
        /// <param name="portalId">PortalId of the new portal.</param>
        /// <param name="template">The template.</param>
        /// <param name="administratorId">UserId for the portal administrator. This is used to assign roles to this user.</param>
        /// <param name="mergeTabs">Flag to determine whether Module content is merged.</param>
        /// <param name="isNewPortal">Flag to determine is the template is applied to an existing portal or a new one.</param>
        /// <remarks>
        /// The roles and settings nodes will only be processed on the portal template file.
        /// </remarks>
        void ApplyPortalTemplate(int portalId, IPortalTemplateInfo template, int administratorId, PortalTemplateModuleAction mergeTabs, bool isNewPortal);

        Tuple<bool, string> ExportPortalTemplate(int portalId, string fileName, string description, bool isMultiLanguage, IEnumerable<string> locales, string localizationCulture, IEnumerable<int> exportTabIds, bool includeContent, bool includeFiles, bool includeModules, bool includeProfile, bool includeRoles);
    }
}
