using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;

namespace Techyard.Revit.Geometry
{
    public class FilteredGeometryCollector : IEnumerable<GeometryObject>
    {
        private List<IGeometryFilter> Filters { get; } = new List<IGeometryFilter>();
        private Element SourceElement { get; }

        public FilteredGeometryCollector(Element element)
        {
            SourceElement = element;
        }

        public IEnumerator<GeometryObject> GetEnumerator()
        {
            return new InnerEnumerator(SourceElement, Filters);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public FilteredGeometryCollector OfClass<T>() where T : GeometryObject
        {
            return WherePasses(new GeometryClassFilter(typeof(T)));
        }

        public FilteredGeometryCollector WherePasses(IGeometryFilter filter)
        {
            Filters.Add(filter);
            return this;
        }

        public IEnumerable<GeometryObject> ToGeometryObjects()
        {
            var iterator = GetEnumerator();
            iterator.Reset();
            while (iterator.MoveNext())
            {
                yield return iterator.Current;
            }
            iterator.Dispose();
        }

        private class InnerEnumerator : IEnumerator<GeometryObject>
        {
            private GeometryElement Geometry { get; set; }

            private List<GeometryObject> _objects;

            private List<IGeometryFilter> Filters { get; }

            private IEnumerable<GeometryObject> Objects => _objects ??
                                                           (_objects = Geometry?.GetObjects().Where(go =>
                                                               Filters.Count == 0 ||
                                                               Filters.All(filter => filter.Filter(go))).ToList());

            private int Position { get; set; } = -1;

            public int Count => Objects.Count();

            public InnerEnumerator(Element element, List<IGeometryFilter> filters = null, Options options = null)
            {
                Geometry = element.get_Geometry(options ?? new Options());
                Filters = filters ?? new List<IGeometryFilter>();
            }

            public void Dispose()
            {
                _objects.Clear();
                _objects = null;
                Geometry = null;
            }

            public bool MoveNext()
            {
                if (Position + 1 == Count)
                {
                    return false;
                }
                Position++;
                return true;
            }

            public void Reset()
            {
                Position = -1;
            }

            public GeometryObject Current => Objects.ElementAt(Position);

            object IEnumerator.Current => Current;
        }
    }
}
