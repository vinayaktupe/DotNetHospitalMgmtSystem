using HospitalMgmtSystem.DAL.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalMgmtSystem.DAL.Repository
{
    public abstract class ContextRepository : IDisposable
    {
        private static ApplicationDbContext _joinContext;

        public ContextRepository()
        {

        }

        internal static ApplicationDbContext JoinContext
        {
            get
            {
                if (_joinContext==null)
                {
                    _joinContext = new ApplicationDbContext();
                }

                return _joinContext;
            }
        }

        public void Dispose()
        {
            if (_joinContext!=null)
            {
                _joinContext.Dispose();
                _joinContext = null;
            }
        }
    }
}
