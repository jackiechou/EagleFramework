using System;
using System.Collections.Generic;
using System.Reflection;

namespace Eagle.Common.Extensions
{
    public class FileVersion
    {
        #region Private Variables
        private static  string _errMsg;
        private static AssemblyInformation _info;
        private static  List<AssemblyInformation> _lstReferences;
        #endregion

        #region Public Properties
        public string ErrorMessage => _errMsg;

        public AssemblyInformation CurrentAssemblyInfo => _info;

        public List<AssemblyInformation> ReferenceAssembly => _lstReferences;

        #endregion

        #region Private Methods
        private static List<AssemblyInformation> GetReferenceAssembly(Assembly asm)
        {
            try
            {
                _lstReferences = new List<AssemblyInformation>();
                AssemblyName[] list = asm.GetReferencedAssemblies();
                if (list.Length > 0)
                {
                    foreach (AssemblyName t in list)
                    {
                        var info = new AssemblyInformation
                        {
                            Name = t.Name,
                            Version = t.Version.ToString(),
                            FullName = t.ToString()
                        };

                        _lstReferences.Add(info);
                    }
                }
                return _lstReferences;
            }
            catch (Exception err)
            {
                _errMsg = err.Message;
                return null;
            }
        }
        #endregion

        #region Public Methods
        public static List<AssemblyInformation> GetVersion(string fileName)
        {
            Assembly asm;
            try
            {
                asm = Assembly.LoadFrom(fileName);
            }
            catch (Exception err)
            {
                _errMsg = err.Message;
                return null;
            }
            if (asm != null)
            {
                _info = new AssemblyInformation
                {
                    Name = asm.GetName().Name,
                    Version = asm.GetName().Version.ToString(),
                    FullName = asm.GetName().ToString()
                };
            }
            else
            {
                _errMsg = "Invalid assembly";
                return null;
            }

            return GetReferenceAssembly(asm);
        }
        public static List<AssemblyInformation> GetVersion(Assembly asm)
        {
            if (asm != null)
            {
                _info = new AssemblyInformation
                {
                    Name = asm.GetName().Name,
                    Version = asm.GetName().Version.ToString(),
                    FullName = asm.GetName().ToString()
                };
            }
            else
            {
                _errMsg = "Invalid assembly";
                return null;
            }

            return GetReferenceAssembly(asm);
        }

        public static AssemblyInformation GetFileVersion(string fileName)
        {
            Assembly asm;
            try
            {
                asm = Assembly.LoadFrom(fileName);
            }
            catch (Exception err)
            {
                _errMsg = err.Message;
                return null;
            }

                if (asm != null)
            {
                _info = new AssemblyInformation
                {
                    Name = asm.GetName().Name,
                    Version = asm.GetName().Version.ToString(),
                    FullName = asm.GetName().ToString()
                };
                return _info;
            }
            else
            {
                _errMsg = "Invalid assembly";
                return null;
            }
        }

        public static AssemblyInformation GetAssemblyVersion(Assembly asm)
        {
            if (asm != null)
            {
                _info = new AssemblyInformation
                {
                    Name = asm.GetName().Name,
                    Version = asm.GetName().Version.ToString(),
                    FullName = asm.GetName().ToString()
                };
                return _info;
            }
            else
            {
                _errMsg = "Invalid assembly";
                return null;
            }
        }

        #endregion
    }
}
