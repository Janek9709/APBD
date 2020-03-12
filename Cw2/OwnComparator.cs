using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Cw2
{
    public class OwnComparator : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals($"{x.imie} {x.nazwisko} {x.numer}",
                $"{y.imie} {y.nazwisko} {y.numer}");
        }

        public int GetHashCode(Student obj)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode($"{obj.imie} {obj.nazwisko} {obj.numer}");
        }
    }

    public class ActiveOwnComparator : IEqualityComparer<ActiveStudies>
    {
        public bool Equals(ActiveStudies x,ActiveStudies y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals($"{x.name}", $"{y.name}");
        }

        public int GetHashCode([DisallowNull] ActiveStudies obj)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode($"{obj.name}");
        }
    }
}
