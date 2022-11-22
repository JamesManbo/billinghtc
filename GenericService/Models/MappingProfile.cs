using System.ComponentModel;
using System.Reflection;

namespace GenericRepository.Models
{
    public class MappingProfile : AutoMapper.Profile
        {
        public MappingProfile()
        {
        }

        public MappingProfile(bool addIgnoreParts)
        {
            if (addIgnoreParts)
                ForAllPropertyMaps(pm => Filter(pm.SourceMember), (pm, opt) => opt.Ignore());
        }

        /// <summary>
        /// This returns true for source properties that we DON'T want to be copied
        /// This stops DTP properties that are null, or have a [ReadOnly(true)] attribute, fom being copied.
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        private static bool Filter(MemberInfo member)
        {
            return member == null || (member.GetCustomAttribute<ReadOnlyAttribute>()?.IsReadOnly ?? false);
        }
    }
}
