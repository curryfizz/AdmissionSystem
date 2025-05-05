using System;
using System.Collections.Generic;
using AdmissionSystem.Core.Models;

namespace AdmissionSystem.Core.Patterns
{
    // AdmissionSystem.cs
    public class AdmissionSystem
    {
        private static readonly Lazy<AdmissionSystem> _instance = new(() => new AdmissionSystem());

        // Update this line to dereference the Lazy type to get the actual instance.
        public static AdmissionSystem GetInstance() => _instance.Value; // Access the actual instance

        public List<AdmissionCenter> Centers { get; } = new();

        private AdmissionSystem() { }

        public void RegisterCenter(AdmissionCenter center) => Centers.Add(center);
    }

}
