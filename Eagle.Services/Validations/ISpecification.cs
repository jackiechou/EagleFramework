using System.Collections.Generic;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Validations
{
    public interface ISpecification<TData>
    {
        bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null);

        ISpecification<TData> Not(ISpecification<TData> specification);
        ISpecification<TData> And(ISpecification<TData> right);
        ISpecification<TData> Or(ISpecification<TData> right);
    }
}
