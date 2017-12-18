using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Techyard.Revit.Common;

namespace Techyard.Revit.Database
{
    public static class FamilyExtension
    {
        public static ElementId GetCategoryId(this Family family)
        {
            try
            {
                return family.FamilyCategory.Id;
            }
            catch
            {
                return GetFamilySymbols(family).FirstOrDefault()?.Category?.Id;
            }
        }

        internal static IEnumerable<FamilySymbol> GetFamilySymbols(this Family family)
        {
            var document = family.Document;
            try
            {
#if REVIT2014
                return family.Symbols.AsList<FamilySymbol>();
#elif REVIT2015 || REVIT2016 || REVIT2017 || REVIT2018
                return
                    family.GetFamilySymbolIds()
                        .Map<ElementId, FamilySymbol>(symbolId => document.GetElement(symbolId) as FamilySymbol)
                        .Where(symbol => symbol != null);
#endif
            }
            catch
            {
                return
                    new FilteredElementCollector(document).OfClass(typeof(FamilySymbol))
                        .Cast<FamilySymbol>()
                        .Where(symbol => family.Id.Equals(symbol.Family.Id));
            }
        }
    }
}