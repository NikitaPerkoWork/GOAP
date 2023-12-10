using System.Collections.Generic;
using System.Linq;

namespace CrashKonijn.Goap.Classes.Validators
{
    public class ValidationResults
    {
        private readonly string _name;
        private readonly List<string> _errors = new();
        private readonly List<string> _warnings = new();

        public ValidationResults(string name)
        {
            this._name = name;
        }
        
        public void AddError(string error)
        {
            _errors.Add($"[{_name}] {error}");
        }
        
        public void AddWarning(string warning)
        {
            _warnings.Add($"[{_name}] {warning}");
        }
        
        public List<string> GetErrors()
        {
            return _errors;
        }
        
        public List<string> GetWarnings()
        {
            return _warnings;
        }
        
        public bool HasErrors()
        {
            return _errors.Any();
        }
        
        public bool HasWarnings()
        {
            return _warnings.Any();
        }
    }
}