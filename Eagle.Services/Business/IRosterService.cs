using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eagle.Services.Dtos.Business.Personnel;
using Eagle.Services.Dtos.Business.Roster;

namespace Eagle.Services.Business
{
    public interface IRosterService : IBaseService
    {
        #region Public Holiday Set
        PublicHolidaySetDetail InsertPublicHolidaySet(PublicHolidaySetEntry entry);
        void UpdatePublicHolidaySet(PublicHolidaySetEditEntry entry);
        #endregion

        #region Public Holiday

        PublicHolidayDetail InsertPublicHoliday(PublicHolidayEntry entry);
        void UpdatePublicHoliday(PublicHolidayEditEntry entry);

        #endregion

    }
}
