using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace BPWA.Web.Helpers
{
    public static class ModelStateExtensions
    {
        public static void AddError(this ModelStateDictionary modelStateDictionary, string error)
        {
            modelStateDictionary.AddModelError(Guid.NewGuid().ToString(), error);
        }        
        
        public static void AddErrors(this ModelStateDictionary modelStateDictionary, IEnumerable<string> errors)
        {
            foreach (var error in errors)
                modelStateDictionary.AddModelError(Guid.NewGuid().ToString(), error);
        }
    }
}
