using System.Collections.Immutable;
using VueExample.Errors;

namespace VueExample.EFAbstract
{
    public interface IEFAction<in Tin, out Tout>
    {
        ImmutableList<ValidationError> validationErrorList {get;}
        bool HasErrors {get;}
        Tout Action(Tin dto);
    }
}