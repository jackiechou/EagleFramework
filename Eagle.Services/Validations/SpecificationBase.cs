using System.Collections.Generic;
using Eagle.Core.Common;
using Eagle.Services.Dtos.Common;

namespace Eagle.Services.Validations
{
    public abstract class SpecificationBase<TData> : DisposableObject, ISpecification<TData>
    {
        protected abstract bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null);

        protected virtual ISpecification<TData> Not(ISpecification<TData> specification)
        {
            return new NotSpecification(specification);
        }

        protected virtual ISpecification<TData> And(ISpecification<TData> left, ISpecification<TData> right)
        {
            return new AndSpecification(left, right);
        }

        protected virtual ISpecification<TData> Or(ISpecification<TData> left, ISpecification<TData> right)
        {
            return new OrSpecification(left, right);
        }

        bool ISpecification<TData>.IsSatisfyBy(TData data, IList<RuleViolation> violations)
        {
            return IsSatisfyBy(data, violations);
        }

        ISpecification<TData> ISpecification<TData>.Not(ISpecification<TData> specification)
        {
            return Not(specification);
        }

        ISpecification<TData> ISpecification<TData>.And(ISpecification<TData> right)
        {
            return And(this, right);
        }

        ISpecification<TData> ISpecification<TData>.Or(ISpecification<TData> right)
        {
            return Or(this, right);
        }

        protected bool IsNegated { get; set; }

        private class AndSpecification : SpecificationBase<TData>
        {
            public AndSpecification(ISpecification<TData> left, ISpecification<TData> right)
            {
                Left = left;
                Right = right;
            }

            protected override bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null)
            {
                return Left.IsSatisfyBy(data, violations) && Right.IsSatisfyBy(data, violations);
            }

            private ISpecification<TData> Left { get; set; }
            private ISpecification<TData> Right { get; set; }
        }

        private class OrSpecification : SpecificationBase<TData>
        {
            public OrSpecification(ISpecification<TData> left, ISpecification<TData> right)
            {
                Left = left;
                Right = right;
            }

            protected override bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null)
            {
                return Left.IsSatisfyBy(data, violations) || Right.IsSatisfyBy(data, violations);
            }

            private ISpecification<TData> Left { get; set; }
            private ISpecification<TData> Right { get; set; }
        }

        private class NotSpecification : SpecificationBase<TData>
        {
            public NotSpecification(ISpecification<TData> specification)
            {
                Specification = specification;
                var baseSpecification = Specification as SpecificationBase<TData>;
                if (baseSpecification != null)
                {
                    baseSpecification.IsNegated = !baseSpecification.IsNegated;
                }
            }

            protected override bool IsSatisfyBy(TData data, IList<RuleViolation> violations = null)
            {
                return !Specification.IsSatisfyBy(data, violations);
            }

            private ISpecification<TData> Specification { get; set; }
        }

        private bool disposed = false;

        protected override void Dispose(bool isDisposing)
        {
            if (!this.disposed)
            {
                if (isDisposing)
                {
                    // Nothing to do here.
                }
                disposed = true;
            }
            base.Dispose(isDisposing);
        }
    }
}
